using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Fnproject.Fn.Fdk
{
    public class Middleware<T, S>
        where T : notnull
        where S : notnull
    {
        public static Func<IContext, T, S> userFunc;
        private readonly RequestDelegate _next;

        public IHeaderDictionary ResponseHeaders { get; } = new HeaderDictionary();
        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var requestContext = new Context(context.Request.Headers);
                StreamReader reader = new StreamReader(context.Request.Body, encoding: System.Text.Encoding.UTF8);
                var rawBodyString = await reader.ReadToEndAsync();
                context.Request.Body.Close();

                T input = InputCoercion<T>.Coerce(rawBodyString);
                S output = userFunc(requestContext, input);

                string responseBodyString = OutputCoercion<S>.Coerce(output);

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
