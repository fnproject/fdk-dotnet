using System;
using Microsoft.AspNetCore.Http;
using System.Collections;

namespace Fnproject.Fn.Fdk
{
    public class Context : IContext
    {
        private readonly static string FN_HTTP_REQUEST_URL = "Fn-Http-Request-Url";
        private readonly static string FN_HTTP_METHOD = "Fn-Http-Method";
        private readonly static string CONTENT_TYPE = "Content-Type";
        private readonly static string FN_CALL_ID = "Fn-Call-Id";
        private readonly static string FN_APP_ID = "FN_APP_ID";
        private readonly static string FN_APP_NAME = "FN_APP_NAME";
        private readonly static string FN_FN_ID = "FN_FN_ID";
        private readonly static string FN_FN_NAME = "FN_FN_NAME";
        private readonly static string FN_HTTP_HEADER_PREFIX = "Fn-Http-H-";
        private readonly static string FN_FDK_VERSION_HEADER = "Fn-Fdk-Version";
        private readonly static string FN_FDK_RUNTIME_HEADER = "Fn-Fdk-Runtime";
        private readonly static string FN_HTTP_STATUS = "Fn-Http-Status";

        private readonly static IDictionary config = System.Environment.GetEnvironmentVariables();
        private IHeaderDictionary headers;
        private IHeaderDictionary responseHeaders;
        private ITracingContext tracingContext;

        public Context(IHeaderDictionary reqHeaders)
        {
            headers = new HeaderDictionary();
            foreach (string key in reqHeaders.Keys)
            {
                if (key == CONTENT_TYPE || key == FN_HTTP_REQUEST_URL ||
                    key == FN_HTTP_METHOD)
                {
                    headers.Add(key, reqHeaders[key]);
                }
                else if (key.StartsWith(FN_HTTP_HEADER_PREFIX))
                {
                    headers.Add(key.Remove(0, FN_HTTP_HEADER_PREFIX.Length), reqHeaders[key]);
                }
                reqHeaders.Remove(key);
            }
            responseHeaders = new HeaderDictionary();
            tracingContext = new TracingContext(Context.config, headers);
        }
        public string AppID()
        {
            return headers[FN_APP_ID];
        }
        public string FunctionID()
        {
            return headers[FN_FN_ID];
        }
        public string AppName()
        {
            return headers[FN_APP_NAME];
        }
        public string FunctionName()
        {
            return headers[FN_FN_NAME];
        }
        public string RequestURL()
        {
            return headers[FN_HTTP_REQUEST_URL];
        }
        public string RequestMethod()
        {
            return headers[FN_HTTP_METHOD];
        }
        public string CallID()
        {
            return headers[FN_CALL_ID];
        }
        public IHeaderDictionary Headers()
        {
            return headers;
        }

        public string ConfigValueByKey(string key) {
          return Context.config[key] != null ? (string) Context.config[key] : "";
        }

        public IDictionary Config()
        {
            return Context.config;
        }
        public void AddHeader(string key, string value)
        {
            responseHeaders.Add(key, value);
        }
        public void SetStatus(int statusCode)
        {
            responseHeaders.Add(FN_HTTP_STATUS, statusCode.ToString());
        }

        public IHeaderDictionary ResponseHeaders()
        {
            responseHeaders.Add(FN_FDK_VERSION_HEADER, String.Format("fdk-csharp/{0}", Version.Get()));
            responseHeaders.Add(FN_FDK_RUNTIME_HEADER, String.Format("dotnet/{0}", System.Environment.Version.ToString()));
            if (!responseHeaders.ContainsKey(FN_HTTP_STATUS))
            {
                responseHeaders.Add(FN_HTTP_STATUS, 200.ToString());
            }
            return responseHeaders;
        }

        public ITracingContext TracingContext()
        {
            return tracingContext;
        }
    }

    internal class TracingContext : ITracingContext
    {
        private readonly static string OCI_TRACING_ENABLED = "OCI_TRACING_ENABLED";
        private readonly static string OCI_TRACE_COLLECTOR_URL = "OCI_TRACE_COLLECTOR_URL";
        private readonly static string TRACING_TRACE_ID_HEADER = "x-b3-traceid";
        private readonly static string TRACING_SPAN_ID_HEADER = "x-b3-spanid";
        private readonly static string TRACING_PARENT_SPAN_ID_HEADER = "x-b3-parentspanid";
        private readonly static string TRACING_FLAGS_HEADER = "x-b3-flags";
        private readonly static string TRACING_SAMPLED_HEADER = "x-b3-sampled";

        private string traceCollectorURL;
        private string traceId;
        private string spanId;
        private string parentSpanId;
        private bool sampled;
        private string flags;
        private bool tracingEnabled;
        private string serviceName;
        public bool TracingEnabled()
        {
            return tracingEnabled;
        }

        public string TraceCollectorURL()
        {
            return traceCollectorURL;
        }

        public string TraceId()
        {
            return traceId;
        }

        public string SpanId()
        {
            return spanId;
        }

        public string ParentSpanId()
        {
            return parentSpanId;
        }

        public bool Sampled()
        {
            return sampled;
        }

        public string Flags()
        {
            return flags;
        }

        public string ServiceName()
        {
            return serviceName;
        }
        public TracingContext(IDictionary config, IHeaderDictionary headers)
        {
            if (config.Contains(OCI_TRACING_ENABLED) &&
                bool.TryParse(config[OCI_TRACING_ENABLED].ToString(), out tracingEnabled) &&
                tracingEnabled)
            {
                traceCollectorURL = config[OCI_TRACE_COLLECTOR_URL].ToString();
                traceId = headers[TRACING_TRACE_ID_HEADER];
                spanId = headers[TRACING_SPAN_ID_HEADER];
                parentSpanId = headers[TRACING_PARENT_SPAN_ID_HEADER];
                flags = headers[TRACING_FLAGS_HEADER];
                serviceName = String.Format("{0}::{1}", config["FN_APP_NAME"],
                    config["FN_FN_NAME"]).ToLower();

                if (!bool.TryParse(headers[TRACING_SAMPLED_HEADER].ToString(), out sampled))
                {
                    Console.WriteLine("Failed to parse 'sampled' in tracing context");
                }
            }
        }
    }
}
