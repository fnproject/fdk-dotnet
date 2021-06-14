using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDK
{
    public class ContainerEnvironment: IContainerEnvironment
    {
        public string FN_ID {get; set; } = "123";
        public string FN_NAME{get; set; } = "Happy_Smurfs";
        public string FN_APP_ID{get; set;} = "XYZ_123";
        public string FN_APP_NAME{get; set;} = "SleepyHead";

        public string FN_FORMAT{get; set;} = "Http-Stream";
        public string FN_LISTENER { get; set; } = "http://127.0.0.1:8080";
        
        public Int32 FN_MEMORY { get; set; } = 128;
        public Int32 FN_TMPSIZE { get; set; } = 512;
        public string SOCKET_TYPE{get; set;} = "Unix";
    }
}
