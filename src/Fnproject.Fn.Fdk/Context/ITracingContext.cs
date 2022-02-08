namespace Fnproject.Fn.Fdk.Context
{
    public interface ITracingContext
    {
        /**
         * Returns true if tracing is enabled for this function invocation
         * @return whether tracing is enabled
         */
        bool TracingEnabled();

        /**
         * Returns the URL to be used in tracing libraries as the destination for
         * the tracing data
         * @return a string containing the trace collector URL
         */
        string TraceCollectorURL();

        /**
         * Returns the current trace ID as extracted from Zipkin B3 headers if they
         * are present on the request
         * @return the trace ID as a string
         */
        string TraceId();

        /**
         * Returns the current span ID as extracted from Zipkin B3 headers if they
         * are present on the request
         * @return the span ID as a string
         */
        string SpanId();

        /**
         * Returns the parent span ID as extracted from Zipkin B3 headers if they
         * are present on the request
         * @return the parent span ID as a string
         */
        string ParentSpanId();

        /**
         * Returns the value of the Sampled header of the Zipkin B3 headers if they
         * are present on the request
         * @return true if sampling is enabled for the request
         */
        bool Sampled();

        /**
         * Returns the value of the Flags header of the Zipkin B3 headers if they
         * are present on the request
         * @return the verbatim value of the X-B3-Flags header
         */
        string Flags();

        // ServiceName is Config()["FN_APP_ID"] + Config()["FN_FN_Name"]
        string ServiceName();
    }
}
