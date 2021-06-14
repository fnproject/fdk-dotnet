using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDK
{
    public interface IRequestContext
    {
        //Fn_Intent specifies how the caller intends the input message to be processed
        string Fn_Intent();
        string Content_Type();
        string Request_URL();
        string Method();
        int StatusCode();

        string RequestID();

        CancellationToken AbortRequestAfterThisTime { get; set; }

    }
}
