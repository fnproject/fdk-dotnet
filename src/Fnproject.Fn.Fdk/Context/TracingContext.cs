using Microsoft.AspNetCore.Http;
using System;
using System.Collections;

namespace Fnproject.Fn.Fdk.Context {
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
                serviceName = string.Format("{0}::{1}", config["FN_APP_NAME"],
                    config["FN_FN_NAME"]).ToLower();

                if (!bool.TryParse(headers[TRACING_SAMPLED_HEADER].ToString(), out sampled))
                {
                    Console.WriteLine("Failed to parse 'sampled' in tracing context");
                }
            }
        }
    }
}
