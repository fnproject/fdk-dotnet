using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Fnproject.Fn.Fdk.Tests")]
namespace Fnproject.Fn.Fdk.Context
{
    internal class HTTPContext : IHTTPContext
    {
        private static readonly HashSet<string> ALLOWED_RAW_HEADERS = new HashSet<string>(){
          Constants.FN_HTTP_REQUEST_URL_HEADER,
          Constants.FN_HTTP_REQUEST_METHOD_HEADER,
          Constants.FN_INTENT_HEADER,
          Constants.TRACING_FLAGS_HEADER,
          Constants.TRACING_SAMPLED_HEADER,
          Constants.TRACING_SPAN_ID_HEADER,
          Constants.TRACING_TRACE_ID_HEADER,
          Constants.TRACING_PARENT_SPAN_ID_HEADER
        };

        private IRuntimeContext runtimeContext;
        private IHeaderDictionary headers;
        private IQueryCollection queryParams;
        private IHeaderDictionary responseHeaders;

        public HTTPContext(IRuntimeContext runtimeCtx,
            IHeaderDictionary reqHeaders, IQueryCollection query)
        {
            this.headers = new HeaderDictionary();
            this.runtimeContext = runtimeCtx;
            this.responseHeaders = new HeaderDictionary();
            this.queryParams = query;

            if (runtimeCtx.FnIntent() == string.Empty ||
                runtimeCtx.FnIntent() == Constants.INTENT_HTTP_REQUEST)
            {
                foreach (string key in reqHeaders.Keys)
                {
                    if (ALLOWED_RAW_HEADERS.Contains(key) ||
                        key.ToLower() == HeaderNames.ContentType.ToLower())
                    {
                        headers[key] = reqHeaders[key];
                    }
                    else if (key.StartsWith(Constants.FN_HTTP_HEADER_PREFIX))
                    {
                        headers[key.Remove(0, Constants.FN_HTTP_HEADER_PREFIX.Length)] =
                            reqHeaders[key];
                    }
                    reqHeaders.Remove(key);
                }
            }
        }

        public IRuntimeContext RuntimeContext()
        {
            return runtimeContext;
        }
        public string RequestURL()
        {
            return headers[Constants.FN_HTTP_REQUEST_URL_HEADER].Count == 0 ?
                string.Empty : headers[Constants.FN_HTTP_REQUEST_URL_HEADER][0];
        }
        public string RequestMethod()
        {
            return string.IsNullOrEmpty(headers[Constants.FN_HTTP_REQUEST_METHOD_HEADER]) ? string.Empty :
                headers[Constants.FN_HTTP_REQUEST_METHOD_HEADER][0];
        }
        public IHeaderDictionary Headers()
        {
            return headers;
        }
        public string GetHeaderByKey(string key)
        {
            return headers[key].Count == 0 ? string.Empty : string.Join(';', headers[key]);
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
            responseHeaders[Constants.FN_HTTP_STATUS_HEADER] = statusCode.ToString();
        }

        internal IHeaderDictionary ResponseHeaders()
        {
            responseHeaders[Constants.FN_FDK_VERSION_HEADER] =
                String.Format("fdk-dotnet/{0}", Version.Value);
            responseHeaders[Constants.FN_FDK_RUNTIME_HEADER] =
                String.Format("dotnet/{0}", System.Environment.Version.ToString());
            if (!responseHeaders.ContainsKey(Constants.FN_HTTP_STATUS_HEADER))
            {
                responseHeaders[Constants.FN_HTTP_STATUS_HEADER] =
                    StatusCodes.Status200OK.ToString();
            }
            return responseHeaders;
        }
    }
}
