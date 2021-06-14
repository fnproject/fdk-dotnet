using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDK
{
    public static class Runner
    {
        public static string getName()
        {
            return "Charles";
        }
        public static void handle(Func<string,string> function)
        {
            function("Charles");
        }

        public static IPEndPoint getTCPConnectionPoint()
        {
            IPEndPoint endpoint = new IPEndPoint(0x2414188f, 8080);
            return endpoint;
        }
    }
}
