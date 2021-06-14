using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using Mono.Unix.Native;

namespace FDK
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //adding services to the Dependency Injection Container
            services.AddSingleton<IRequestContext, RequestContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerfactory, IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(InvokeClass.InvokeHandler());
                }); 
                endpoints.MapGet("/input", async context =>
                 {
                     await context.Response.WriteAsync(Example.InvokeWithParams("Charles"));
                 });
            });
            applicationLifetime.ApplicationStarted.Register(() => {
                string UnixFilePath = "/tmp/api.sock";
                Console.WriteLine("The Kestrel web server is binded to the "+UnixFilePath);
                string SoftStorageFileOfTheUnixFilePath = "tmp/temp_api.sock";
                Syscall.chmod(
                    SoftStorageFileOfTheUnixFilePath,
                    FilePermissions.S_IRUSR | FilePermissions.S_IWUSR | FilePermissions.S_IRGRP | FilePermissions.S_IWGRP 
                    | FilePermissions.S_IROTH | FilePermissions.S_IWOTH
                );

                Syscall.symlink(SoftStorageFileOfTheUnixFilePath,UnixFilePath);
                Console.WriteLine("Unix Socket:", UnixFilePath);
            });
            //deleting the unix socket path when the application is closed
            applicationLifetime.ApplicationStopped.Register(() => 
            {
                Console.WriteLine("Cleaning the sockets before shutting down the application");
                File.Delete("/tmp/api.sock");
            });
        }
    }
}

//srwxr-xr-x 1 bhashkarjya bhashkarjya     0 Jun 15 00:51 api.sock