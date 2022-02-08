using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Fnproject.Fn.Fdk.Tests")]
namespace Fnproject.Fn.Fdk
{
    sealed internal class Constants
    {
        // Fn config values
        internal static readonly string FN_APP_ID = "FN_APP_ID";
        internal static readonly string FN_APP_NAME = "FN_APP_NAME";
        internal static readonly string FN_FN_ID = "FN_FN_ID";
        internal static readonly string FN_FN_NAME = "FN_FN_NAME";
        internal static readonly string OCI_TRACING_ENABLED = "OCI_TRACING_ENABLED";
        internal static readonly string OCI_TRACE_COLLECTOR_URL = "OCI_TRACE_COLLECTOR_URL";

        // Fn Headers
        internal static readonly string FN_HTTP_REQUEST_URL_HEADER = "Fn-Http-Request-Url";
        internal static readonly string FN_HTTP_REQUEST_METHOD_HEADER = "Fn-Http-Method";
        internal static readonly string CONTENT_TYPE = "Content-Type";
        internal static readonly string FN_HTTP_HEADER_PREFIX = "Fn-Http-H-";
        internal static readonly string FN_FDK_VERSION_HEADER = "Fn-Fdk-Version";
        internal static readonly string FN_FDK_RUNTIME_HEADER = "Fn-Fdk-Runtime";
        internal static readonly string FN_HTTP_STATUS_HEADER = "Fn-Http-Status";
        internal static readonly string FN_INTENT_HEADER = "Fn-Intent";
        internal static readonly string FN_DEADLINE_HEADER = "Fn-Deadline";
        internal static readonly string FN_CALL_ID_HEADER = "Fn-Call-Id";

        internal static readonly string TRACING_TRACE_ID_HEADER = "X-B3-Traceid";
        internal static readonly string TRACING_SPAN_ID_HEADER = "X-B3-Spanid";
        internal static readonly string TRACING_PARENT_SPAN_ID_HEADER = "X-B3-Parentspanid";
        internal static readonly string TRACING_FLAGS_HEADER = "X-B3-Flags";
        internal static readonly string TRACING_SAMPLED_HEADER = "X-B3-Sampled";

        // General constants
        internal static readonly string INTENT_HTTP_REQUEST = "httprequest";
        internal static readonly string COERCION_INTERFACE_METHOD = "Coerce";
        internal static readonly int NUMBER_OF_TRIGGER_SEGMENTS = 3;
        internal static readonly char TRIGGER_DELIMITER = ':';
        internal static readonly int UNASSIGNED_PARAM_INDEX = -1;
        internal static readonly int MAX_ALLOWED_USER_FN_PARAMETERS = 2;
    }
}
