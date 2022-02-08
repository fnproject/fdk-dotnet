using Microsoft.AspNetCore.Http;
using System.Collections;

namespace Fnproject.Fn.Fdk.Context
{
    internal class TracingContext : ITracingContext
    {

        private string traceCollectorURL;
        private string traceId;
        private string spanId;
        private string parentSpanId;
        private bool sampled = false;
        private string flags;
        private bool tracingEnabled = false;
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
            if (config.Contains(Constants.OCI_TRACING_ENABLED) &&
                config[Constants.OCI_TRACING_ENABLED].ToString() == Constants.ENABLED_STRING)
            {
                tracingEnabled = true;
                traceCollectorURL = config[Constants.OCI_TRACE_COLLECTOR_URL].ToString();
                traceId = headers[Constants.TRACING_TRACE_ID_HEADER];
                spanId = headers[Constants.TRACING_SPAN_ID_HEADER];
                parentSpanId = headers[Constants.TRACING_PARENT_SPAN_ID_HEADER];
                flags = headers[Constants.TRACING_FLAGS_HEADER];
                serviceName = string.Format("{0}::{1}", config[Constants.FN_APP_NAME],
                    config[Constants.FN_FN_NAME]).ToLower();

                if (headers[Constants.TRACING_SAMPLED_HEADER] == Constants.ENABLED_STRING)
                {
                    sampled = true;
                }
            }
        }
    }
}
