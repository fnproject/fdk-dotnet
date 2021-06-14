using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDK
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            /*var host = new WebHostBuilder()
                            .UseKestrel()
                            .UseStartup<Startup>()
                            .Build();
                host.Run();*/
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.
                    ConfigureKestrel(options =>
                    {
                        string UnixListener = new ContainerEnvironment().FN_LISTENER;
                        string ListenerUnixSocketPath = UnixListener.Substring(5);
                        options.ListenUnixSocket("/tmp/api.sock");
                        Console.WriteLine("Unix Domain Socket connected");
                        Console.WriteLine(ListenerUnixSocketPath);
                    }).
                    UseStartup<Startup>();
                }); 
    }
}
