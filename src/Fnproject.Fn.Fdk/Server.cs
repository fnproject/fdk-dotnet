using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Mono.Unix.Native;
using System;
using System.IO;
using System.Threading;

namespace Fnproject.Fn.Fdk
{
    sealed class Server
    {
        internal static string SOCKET_PATH { get; private set; }
        internal static string PHONY_SOCKET_PATH { get; private set; }

        public Server()
        {
            var FN_LISTENER = System.Environment.GetEnvironmentVariable("FN_LISTENER");
            var FN_FORMAT = System.Environment.GetEnvironmentVariable("FN_FORMAT");

            if (string.IsNullOrEmpty(FN_LISTENER) ||
                !FN_LISTENER.StartsWith("unix:"))
            {
                throw new ArgumentException("Malformed FN_LISTENER");
            }

            if (!string.IsNullOrEmpty(FN_FORMAT) &&
                FN_FORMAT != "http-stream")
            {
                throw new ArgumentException("Unsupported FN_FORMAT");
            }

            Uri url = new Uri(FN_LISTENER);

            var socketPath = url.AbsolutePath;
            var socketDir = Path.GetDirectoryName(socketPath);
            var symlinkFileName = $"phony-{Path.GetFileName(socketPath)}";

            var symlinkSocketPath = Path.Join(Path.GetDirectoryName(socketPath), symlinkFileName);

            SOCKET_PATH = socketPath;
            PHONY_SOCKET_PATH = symlinkSocketPath;
        }

        private IHost prepareServer<T, S>(Func<IContext, T, S> userFunc)
            where T : notnull, new()
            where S : notnull, new()
        {
            Startup<T, S>.userFunc = userFunc;

            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup<T, S>>()
                    .UseKestrel(opt =>
                    {
                        DeleteStaleSockets();
                        opt.ListenUnixSocket(PHONY_SOCKET_PATH);
                    });
                })
              .Build();

        }

        internal static void SocketPermissions(string phonySock, string realSock)
        {
            if (Syscall.chmod(
                phonySock,
                NativeConvert.FromOctalPermissionString("0666")
            ) < 0)
            {
                var error = Stdlib.GetLastError();
                throw new ArgumentException("Error setting file permissions: " + error);
            }

            if (Syscall.symlink(Path.GetFileName(phonySock), realSock) < 0)
            {
                var error = Stdlib.GetLastError();
                throw new ArgumentException("Error creating symlink: " + error);
            }
        }

        internal static void DeleteStaleSockets()
        {
            if (File.Exists(SOCKET_PATH))
            {
                File.Delete(SOCKET_PATH);
            }

            if (File.Exists(PHONY_SOCKET_PATH))
            {
                File.Delete(PHONY_SOCKET_PATH);
            }

        }

        public void Run<T, S>(Func<IContext, T, S> userFunc)
            where T : notnull, new()
            where S : notnull, new()
        {

            var server = this.prepareServer(userFunc);
            server.Run();
        }
    }
}
