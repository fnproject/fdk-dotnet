using Microsoft.AspNetCore.Http;
using System.Collections;

namespace Fnproject.Fn.Fdk.Context
{
    internal class TracingContext : ITracingContext
    {
        private readonly static string OCI_TRACING_ENABLED = "OCI_TRACING_ENABLED";
        private readonly static string OCI_TRACE_COLLECTOR_URL = "OCI_TRACE_COLLECTOR_URL";
        internal readonly static string TRACING_TRACE_ID_HEADER = "X-B3-Traceid";
        internal readonly static string TRACING_SPAN_ID_HEADER = "X-B3-Spanid";
        internal readonly static string TRACING_PARENT_SPAN_ID_HEADER = "X-B3-Parentspanid";
        internal readonly static string TRACING_FLAGS_HEADER = "X-B3-Flags";
        internal readonly static string TRACING_SAMPLED_HEADER = "X-B3-Sampled";

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
            if (config.Contains(OCI_TRACING_ENABLED) &&
                config[OCI_TRACING_ENABLED].ToString() == "1")
            {
                tracingEnabled = true;
                traceCollectorURL = config[OCI_TRACE_COLLECTOR_URL].ToString();
                traceId = headers[TRACING_TRACE_ID_HEADER];
                spanId = headers[TRACING_SPAN_ID_HEADER];
                parentSpanId = headers[TRACING_PARENT_SPAN_ID_HEADER];
                flags = headers[TRACING_FLAGS_HEADER];
                serviceName = string.Format("{0}::{1}", config["FN_APP_NAME"],
                    config["FN_FN_NAME"]).ToLower();

                if (headers[TRACING_SAMPLED_HEADER] == "1") sampled = true;
            }
        }
    }
}
