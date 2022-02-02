using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Fnproject.Fn.Fdk
{
    public class Middleware
    {
        private readonly RequestDelegate _next;

        public IHeaderDictionary ResponseHeaders { get; } = new HeaderDictionary();
        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        private object[] prepareArgs(IContext ctx, string requestBody) {
          var parameters = Function.Method.GetParameters();
          object[] args;
          switch (parameters.Length) {
            case 0:
              args = new object[0];
              break;
            case 1:
              args = new object[1];
              if(Function.ContextParameterIndex != -1) {
                args[0] = ctx;
              } else {
                args[0] = InputCoercion.Coerce(requestBody, parameters[0].ParameterType);
              }
              break;
            default:
              args = new object[2];
              args[Function.ContextParameterIndex] = ctx;
              args[Function.DataParameterIndex] = InputCoercion.Coerce(requestBody, 
                  parameters[Function.DataParameterIndex].ParameterType);
              break;
          }
          return args;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var requestContext = new Context(context.Request.Headers);
                StreamReader reader = new StreamReader(context.Request.Body, encoding: System.Text.Encoding.UTF8);
                var rawBodyString = await reader.ReadToEndAsync();
                context.Request.Body.Close();
                
                object[] args = prepareArgs(requestContext, rawBodyString);
                object result = Function.Invoke(args);
                string responseBodyString = OutputCoercion.Coerce(result);

                foreach (var entry in requestContext.ResponseHeaders())
                    context.Response.Headers.Add(entry.Key, entry.Value);

                await context.Response.WriteAsync(responseBodyString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                context.Response.Headers.Add("Fn-Fdk-Runtime",
                    String.Format("dotnet/{0}", System.Environment.Version.ToString()));
                context.Response.Headers.Add("Fn-Fdk-Version",
                    String.Format("fdk-csharp/{0}", Version.Get()));
                context.Response.Headers.Add("Fn-Fdk-Status", 502.ToString());
                context.Response.StatusCode = 502;

                // Writing response with empty body
                await context.Response.WriteAsync("");
            }
        }
    }
}
