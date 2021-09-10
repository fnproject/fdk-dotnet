using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Fnproject.Fn.Fdk.Tests")]
namespace Fnproject.Fn.Fdk.Context
{
    internal class HTTPContext : IHTTPContext
    {
        private readonly static string FN_HTTP_REQUEST_URL = "Fn-Http-Request-Url";
        private readonly static string FN_HTTP_METHOD = "Fn-Http-Method";
        private readonly static string CONTENT_TYPE = "Content-Type";
        private readonly static string FN_INTENT = "Fn-Intent";
        private readonly static string FN_HTTP_HEADER_PREFIX = "Fn-Http-H-";
        private readonly static string FN_FDK_VERSION_HEADER = "Fn-Fdk-Version";
        private readonly static string FN_FDK_RUNTIME_HEADER = "Fn-Fdk-Runtime";
        private readonly static string FN_HTTP_STATUS = "Fn-Http-Status";
        private readonly static HashSet<string> ALLOWED_RAW_HEADERS = new HashSet<string>(){
          FN_HTTP_REQUEST_URL,
          FN_HTTP_METHOD,
          CONTENT_TYPE,
          FN_INTENT,
          TracingContext.TRACING_FLAGS_HEADER,
          TracingContext.TRACING_SAMPLED_HEADER,
          TracingContext.TRACING_SPAN_ID_HEADER,
          TracingContext.TRACING_TRACE_ID_HEADER,
          TracingContext.TRACING_PARENT_SPAN_ID_HEADER
        };

        private IRuntimeContext runtimeContext;
        private IHeaderDictionary headers;
        private IQueryCollection queryParams;
        private IHeaderDictionary responseHeaders;

        public HTTPContext(IHeaderDictionary reqHeaders, IQueryCollection query)
        {
            headers = new HeaderDictionary();

            foreach (string key in reqHeaders.Keys)
            {
                if (ALLOWED_RAW_HEADERS.Contains(key))
                {
                    headers[key] = reqHeaders[key];
                }
                else if (key.StartsWith(FN_HTTP_HEADER_PREFIX))
                {
                    headers[key.Remove(0, FN_HTTP_HEADER_PREFIX.Length)] = reqHeaders[key];
                }
                reqHeaders.Remove(key);
            }
            runtimeContext = new RuntimeContext(headers);
            responseHeaders = new HeaderDictionary();
            this.queryParams = query;
        }

        public IRuntimeContext RuntimeContext()
        {
            return runtimeContext;
        }
        public string RequestURL()
        {
            return headers[FN_HTTP_REQUEST_URL].Count == 0 ? "" : headers[FN_HTTP_REQUEST_URL][0];
        }
        public string RequestMethod()
        {
            return string.IsNullOrEmpty(headers[FN_HTTP_METHOD]) ? "" :
                headers[FN_HTTP_METHOD][0];
        }
        public IHeaderDictionary Headers()
        {
            return headers;
        }
        public string GetHeaderByKey(string key)
        {
            return headers[key].Count == 0 ? "" : string.Join(';', headers[key]);
        }
        public IQueryCollection Query()
        {
            return queryParams;
        }
        public string[] GetQueryByKey(string key)
        {
            return queryParams[key];
        }
        public void AddHeader(string key, string value)
        {
            responseHeaders[key] = value;
        }
        public void SetStatus(int statusCode)
        {
            responseHeaders[FN_HTTP_STATUS] = statusCode.ToString();
        }

        internal IHeaderDictionary ResponseHeaders()
        {
            responseHeaders[FN_FDK_VERSION_HEADER] = String.Format("fdk-csharp/{0}", Version.Get());
            responseHeaders[FN_FDK_RUNTIME_HEADER] = String.Format("dotnet/{0}", System.Environment.Version.ToString());
            if (!responseHeaders.ContainsKey(FN_HTTP_STATUS))
            {
                responseHeaders[FN_HTTP_STATUS] = 200.ToString();
            }
            return responseHeaders;
        }
    }
}
