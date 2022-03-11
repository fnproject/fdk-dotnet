namespace Fnproject.Fn.Fdk.Context
{
    /// <summary>
    /// ITracingContext is the APM Tracing context passed by the service if tracing
    /// is enabled for the function.
    /// </summary>
    public interface ITracingContext
    {
        /// <summary>
        /// TracingEnabled returns true if tracing is enabled for this function invocation
        /// </summary>
        /// <returns><see cref="System.Bool"/> whether tracing is enabled</returns>
        bool TracingEnabled();

        /// <summary>
        /// TraceCollectorURL returns the URL to be used in tracing libraries as the destination for
        /// the tracing data
        /// </summary>
        /// <returns><see cref="System.String"/> containing the trace collector URL</returns>
        string TraceCollectorURL();

        /// <summary>
        /// TraceId returns the current trace ID as extracted from Zipkin B3 headers if they
        /// are present on the request
        /// </summary>
        /// <returns><see cref="System.String"/> containing the trace ID</returns>
        string TraceId();

        /// <summary>
        /// SpanId returns the current span ID as extracted from Zipkin B3 headers if they
        /// are present on the request
        /// </summary>
        /// <returns><see cref="System.String"/> containing the span ID</returns>
        string SpanId();

        /// <summary>
        /// ParentSpanId returns the parent span ID as extracted from Zipkin B3 headers if they
        /// are present on the request
        /// </summary>
        /// <returns><see cref="System.String"/> containing the parent span ID</returns>
        string ParentSpanId();

        /// <summary>
        /// Sampled returns the value of the Sampled header of the Zipkin B3 headers if they
        /// are present on the request
        /// </summary>
        /// <returns><see cref="System.Bool"/> whether sampling is enabled for the request</returns>
        bool Sampled();
        
        /// <summary>
        /// Flags returns the value of the Flags header of the Zipkin B3 headers if they
        /// are present on the request
        /// </summary>
        /// <returns><see cref="System.String"/> containing the verbatim value of the X-B3-Flags header</returns>
        string Flags();

        /// <summary>
        /// ServiceName returns the name of the service
        /// </summary>
        /// <returns><see cref="System.String"/> containing the service name as app name + function name</returns>
        string ServiceName();
    }
}
