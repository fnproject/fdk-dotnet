
using System;
using Microsoft.AspNetCore.Http;

namespace Fnproject.Fn.Fdk
{
    public static class Logger
    {
        readonly private static string framerName = Environment.GetEnvironmentVariable("FN_LOGFRAME_NAME");
        readonly private static string frameHeader = Environment.GetEnvironmentVariable("FN_LOGFRAME_HDR");

        public static void Start(IHeaderDictionary headers)
        {

            if (string.IsNullOrEmpty(Logger.framerName) ||
                string.IsNullOrEmpty(Logger.frameHeader))
            {
                return;
            }

            string id = headers[Logger.frameHeader];
            if (!string.IsNullOrEmpty(id))
            {
                Console.WriteLine("\n{0}={1}", Logger.frameHeader, id);
                Console.Error.WriteLine("\n{0}={1}", Logger.frameHeader, id);
            }
        }
    }
}
