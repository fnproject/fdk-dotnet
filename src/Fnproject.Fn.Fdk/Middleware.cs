using Fnproject.Fn.Fdk.Context;
using Fnproject.Fn.Fdk.Coercion;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Fnproject.Fn.Fdk
{
    internal class Middleware
    {
        private readonly RequestDelegate _next;

        public IHeaderDictionary ResponseHeaders { get; } = new HeaderDictionary();
        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        private object prepareFunctionContext(IHTTPContext ctx)
        {
            if (Function.ContextType == typeof(IRuntimeContext))
            {
                return ctx.RuntimeContext();
            }
            return ctx;
        }

        private object[] prepareArgs(IHTTPContext ctx, string requestBody)
        {
            var parameters = Function.Method.GetParameters();
            object[] args;
            switch (parameters.Length)
            {
                case 0:
                    args = new object[0];
                    break;
                case 1:
                    args = new object[1];
                    if (Function.ContextParameterIndex != -1)
                    {
                        args[0] = prepareFunctionContext(ctx);
                    }
                    else
                    {
                        args[0] = InputCoercion.Coerce(requestBody, parameters[0].ParameterType);
                    }
                    break;
                default:
                    args = new object[2];
                    args[Function.ContextParameterIndex] = prepareFunctionContext(ctx);
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
                RuntimeContext runtimeContext = new RuntimeContext(context.Request.Headers);
                HTTPContext httpContext = new HTTPContext(runtimeContext,
                    context.Request.Headers, context.Request.Query);
                StreamReader reader = new StreamReader(context.Request.Body, encoding: System.Text.Encoding.UTF8);
                var rawBodyString = await reader.ReadToEndAsync();
                context.Request.Body.Close();

                object[] args = prepareArgs(httpContext, rawBodyString);
                object result = Function.Invoke(args);
                string responseBodyString = OutputCoercion.Coerce(result,
                  result.GetType());

                foreach (var entry in httpContext.ResponseHeaders())
                    context.Response.Headers[entry.Key] = entry.Value;

                await context.Response.WriteAsync(responseBodyString);
            }
            catch (Exception e)
            {
                context.Response.Headers[Constants.FN_FDK_RUNTIME_HEADER] =
                    String.Format("dotnet/{0}", System.Environment.Version.ToString());
                context.Response.Headers[Constants.FN_FDK_RUNTIME_HEADER] =
                    String.Format("fdk-dotnet/{0}", Version.Value);
                context.Response.Headers[Constants.FN_HTTP_STATUS_HEADER] = 502.ToString();
                context.Response.StatusCode = StatusCodes.Status502BadGateway;

                await context.Response.WriteAsync(string.Empty);
                throw e;
            }
        }
    }
}
