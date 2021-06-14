using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDK
{
    public class RequestContext: IRequestContext
    {
        public string Fn_Intent()
        {
            return "cloud-event";
        }
        public string Content_Type()
        {
            return "";
        }
        public string Request_URL()
        {
            return "";
        }

        public string Method()
        {
            return "POST";
        }

        public int StatusCode()
        {
            return 200;
        }

        public string RequestID()
        {
            return "";
        }

        public CancellationToken AbortRequestAfterThisTime { get; set; }
        




    }
}
