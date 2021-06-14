using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDK
{
    public static class InvokeClass
    {
        public static string InvokeHandler()
        {
            //Runner.getName();
            string output = handle(Example.Invoke);
            return output;
        }

        public static string handle(Func<string> myMethod)
        {
            return myMethod();
        }
    }
}
