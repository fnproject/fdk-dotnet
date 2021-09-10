using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;

namespace Fnproject.Fn.Fdk
{
    public class Middleware<T, S>
        where T : notnull, new()
        where S : notnull, new()
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

                T input = new T();

                if (typeof(IInputCoercible<T>).IsAssignableFrom(typeof(T)))
                {
                    StreamReader reader = new StreamReader(context.Request.Body, encoding: System.Text.Encoding.UTF8);
                    var rawBodyString = await reader.ReadToEndAsync();
                    MethodInfo method = typeof(T).GetMethod("Coerce");
                    input = (T)method.Invoke(input, new object[] { rawBodyString });
                }
                else
                {
                    try
                    {
                        input = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(context.Request.Body);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to deserialize JSON: {0}", e.Message);
                    }
                }

                context.Request.Body.Close();

                S output = userFunc(requestContext, input);

                string responseBodyString = "";

                if (typeof(IOutputCoercible<S>).IsAssignableFrom(typeof(S)))
                {
                    var userOutputCoercion = (IOutputCoercible<S>)output;
                    responseBodyString = userOutputCoercion.Coerce(output);
                }
                else
                {
                    responseBodyString = System.Text.Json.JsonSerializer.Serialize<S>(output);
                }

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