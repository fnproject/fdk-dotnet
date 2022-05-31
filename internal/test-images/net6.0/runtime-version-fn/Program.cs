using System;
using Fnproject.Fn.Fdk;

namespace Function
{
    class Runtime
    {
        public string getRuntime() {
          var version = System.Environment.Version.ToString().Split('.');
          return string.Format("dotnet{0}.{1}", version[0],version[1]);
        }

        static void Main(string[] args)
        {
          Fdk.Handle(args[0]);
        }
    }
}
