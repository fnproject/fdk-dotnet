using System;

namespace Fnproject.Fn.Fdk
{
    public class Fdk
    {
        private Fdk() { }

        public static void Handle<T, S>(Func<IContext, T, S> userFunc)
                where T : notnull
                where S : notnull
        {
            Server server = new Server();
            server.Run(userFunc);
        }
    }
}
