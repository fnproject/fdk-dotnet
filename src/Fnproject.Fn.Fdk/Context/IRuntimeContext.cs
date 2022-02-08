using System;
using System.Collections;

namespace Fnproject.Fn.Fdk.Context
{
    /// <summary>
    /// IRuntimeContext provides access to the Runtime Context passed by the service. 
    /// </summary>
    public interface IRuntimeContext
    {
        /// <summary>
        /// AppId provides the ID of the application.
        /// </summary>
        /// <returns><see cref="System.String"/> containing the app ID</returns>
        string AppId();

        /// <summary>
        /// FunctionId provides the ID of the function.
        /// </summary>
        /// <returns><see cref="System.String"/> containing the function ID</returns>
        string FunctionId();

        /// <summary>
        /// AppName provides the name of the application.
        /// </summary>
        /// <returns><see cref="System.String"/> containing the app name</returns>
        string AppName();

        /// <summary>
        /// FunctionName provides the name of the function.
        /// </summary>
        /// <returns><see cref="System.String"/> containing the function name</returns>
        string FunctionName();

        /// <summary>
        /// CallId provides the ID of the call.
        /// </summary>
        /// <returns><see cref="System.String"/> containing ID of the call</returns>
        string CallId();

        /// <summary>
        /// FnIntent provides the invocation intent of the call.
        /// </summary>
        /// <returns><see cref="System.String"/> containing the invocation intent</returns>
        string FnIntent();

        /// <summary>
        /// Deadline provides the deadline of the current request.
        /// </summary>
        /// <returns><see cref="System.DateTime"/> containing the deadline of this invocation</returns>
        DateTime Deadline();

        /// <summary>
        /// Config provides a dictionary containing the config values.
        /// </summary>
        /// <returns><see cref="System.Collections.IDictionary"/> containing config values of the function</returns>
        IDictionary Config();

        /// <summary>
        /// ConfigValueByKey provides a the config value corresponding to the provided key.
        /// </summary>
        /// <param name="key"><see cref="System.String"/> containing key</param>
        /// <returns><see cref="System.String"/> containing config value corresponding to provided key</returns>
        string ConfigValueByKey(string key);

        /// <summary>
        /// TracingContext provides APM tracing context corresponding to this call.
        /// APM Tracing is not enabled by default and needs to be enabled via console.
        /// </summary>
        /// <returns><see cref="Fnproject.Fn.Fdk.Context.ITracingContext"/></returns>
        ITracingContext TracingContext();
    }
}
