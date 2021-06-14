using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDK
{
    public static class Example
    {
        public static string Invoke()
        {
            return "Hello Everyone";
        }

        public static string InvokeWithParams(string input)
        {
            return ($"Hello {input}");
        }
    }
}
