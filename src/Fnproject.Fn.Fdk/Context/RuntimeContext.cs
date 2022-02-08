using Microsoft.AspNetCore.Http;
using System;
using System.Collections;

namespace Fnproject.Fn.Fdk.Context
{
    internal class RuntimeContext : IRuntimeContext
    {
        private string appId;
        private string appName;
        private string fnId;
        private string fnName;
        private string callId;
        private string fnIntent;

        private DateTime deadline;
        public static readonly IDictionary config = System.Environment.GetEnvironmentVariables();
        private ITracingContext tracingContext;

        private string valueOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }

        public RuntimeContext(IHeaderDictionary reqHeaders)
        {
            appId = valueOrEmpty(RuntimeContext.config[Constants.FN_APP_ID] as string);
            appName = valueOrEmpty(RuntimeContext.config[Constants.FN_APP_NAME] as string);
            fnId = valueOrEmpty(RuntimeContext.config[Constants.FN_FN_ID] as string);
            fnName = valueOrEmpty(RuntimeContext.config[Constants.FN_FN_NAME] as string);
            callId = valueOrEmpty(reqHeaders[Constants.FN_CALL_ID_HEADER]);
            fnIntent = valueOrEmpty(reqHeaders[Constants.FN_INTENT_HEADER]);

            if (string.IsNullOrEmpty(reqHeaders[Constants.FN_DEADLINE_HEADER]))
            {
                deadline = DateTime.Now.Add(TimeSpan.FromDays(1));
            }
            else
            {
                deadline = DateTime.Parse(reqHeaders[Constants.FN_DEADLINE_HEADER]);
            }
            tracingContext = new TracingContext(RuntimeContext.config, reqHeaders);
        }
        public string AppId()
        {
            return appId;
        }
        public string FunctionId()
        {
            return fnId;
        }
        public string AppName()
        {
            return appName;
        }
        public string FunctionName()
        {
            return fnName;
        }
        public string CallId()
        {
            return callId;
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
            return valueOrEmpty(RuntimeContext.config[key] as string);
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
