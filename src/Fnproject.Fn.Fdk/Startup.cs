using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fnproject.Fn.Fdk
{
    internal class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddLogging(
                builder =>
                {
                    builder
                        .AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("NToastNotify", LogLevel.Warning)
                        .AddConsole();
                });
        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime applicationLifetime)
        {
            app.UseRouting();

            // Middleware for Logging
            app.Use(async (context, next) =>
            {
                Logger.Start(context.Request.Headers);
                await next.Invoke();
            });

            app.UseMiddleware<Middleware>();

            applicationLifetime.ApplicationStarted.Register(() =>
            {
                Server.SocketPermissions(Server.PHONY_SOCKET_PATH, Server.SOCKET_PATH);
            });
        }
    }
}
