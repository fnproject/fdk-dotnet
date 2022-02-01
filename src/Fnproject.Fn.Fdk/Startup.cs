using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Fnproject.Fn.Fdk
{
    public class Startup<T, S> where T : notnull where S : notnull
    {
        public static Func<IContext, T, S> userFunc;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddLogging(
                builder =>
                {
                    builder.AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("NToastNotify", LogLevel.Warning)
                        .AddConsole();
                });
        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime applicationLifetime)
        {
            Middleware<T, S>.userFunc = userFunc;

            app.UseRouting();

            // Middleware for Logging
            app.Use(async (context, next) =>
            {
                Logger.Start(context.Request.Headers);
                await next.Invoke();
            });

            app.UseMiddleware<Middleware<T, S>>();


            applicationLifetime.ApplicationStarted.Register(() =>
            {
                Server.SocketPermissions(Server.PHONY_SOCKET_PATH, Server.SOCKET_PATH);
            });
        }
    }
}
