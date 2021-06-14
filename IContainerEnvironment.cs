using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDK
{
    //defines the system wide configuration of the function container.
    public interface IContainerEnvironment
    {
        //	The Function ID of the current function
        string FN_ID{get; set;}
        //	The human readable fn name of the current function
        string FN_NAME{get; set;}
        //The app ID of the current function
        string FN_APP_ID{get; set;}
        //	The human readable app name of the current app
        string FN_APP_NAME{get; set;}
        //The protocol the platform expects the function to use
        //(we have several other legacy protocols which should not be supported and will be deprecated)
        string FN_FORMAT{get; set;}
        //A unix socket address (prefixed with "unix://") on the file system that the FDK should create to listen for requests,
        //the platform will guarantee that this directory is writable to the function.
        //FDKs must not write any other data than the unix socket to this directory.
        string FN_LISTENER { get; set; }
        //	The number of MB of ram allocated to the container
        Int32 FN_MEMORY { get; set; }
        //The maximum amount of data that can be written to /tmp
        Int32 FN_TMPSIZE { get; set; }

        
        //socket type is either Unix Domain Socket(UDS) or TCP. According to the contract we need to implement the UDS.
        string SOCKET_TYPE{get; set;}



    }
}
