using Microsoft.AspNetCore.Http;
using System;
using System.Collections;

namespace Fnproject.Fn.Fdk.Context
{
    internal class RuntimeContext : IRuntimeContext
    {
        private string appID;
        private string appName;
        private string fnID;
        private string fnName;
        private string callID;
        private string fnIntent;

        private DateTime deadline;
        private readonly static IDictionary config = System.Environment.GetEnvironmentVariables();
        private ITracingContext tracingContext;

        public RuntimeContext(IHeaderDictionary reqHeaders)
        {
            appID = RuntimeContext.config["FN_APP_ID"] as string;
            appName = RuntimeContext.config["FN_APP_NAME"] as string;
            fnID = RuntimeContext.config["FN_FN_ID"] as string;
            fnName = RuntimeContext.config["FN_FN_NAME"] as string;
            callID = reqHeaders["Fn-Call-Id"];
            fnIntent = reqHeaders["Fn-Intent"];

            if (string.IsNullOrEmpty(reqHeaders["FN_DEADLINE"]))
            {
                deadline = DateTime.Now.Add(TimeSpan.FromDays(1));
            }
            else
            {
                deadline = DateTime.Parse(reqHeaders["FN_DEADLINE"]);
            }
            tracingContext = new TracingContext(RuntimeContext.config, reqHeaders);
        }
        public string AppID()
        {
            return appID;
        }
        public string FunctionID()
        {
            return fnID;
        }
        public string AppName()
        {
            return appName;
        }
        public string FunctionName()
        {
            return fnName;
        }
        public string CallID()
        {
            return callID;
        }
        public string FnIntent()
        {
            return fnIntent;
        }
        public DateTime Deadline()
        {
            return deadline;
        }
        public string ConfigValueByKey(string key)
        {
            return RuntimeContext.config[key] != null ? (string)RuntimeContext.config[key] : "";
        }

        public IDictionary Config()
        {
            return RuntimeContext.config;
        }
        public ITracingContext TracingContext()
        {
            return tracingContext;
        }
    }

}
