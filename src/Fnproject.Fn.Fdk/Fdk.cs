using System;
using System.Threading.Tasks;

namespace Fnproject.Fn.Fdk
{
    public class Fdk
    {
        private Fdk() { }

        public static void Handle<T, S>(Func<IContext, T, S> userFunc)
                where T : notnull, new()
                where S : notnull, new()
        {
            Server server = new Server();
            server.Run(userFunc);
        }
    }
}
