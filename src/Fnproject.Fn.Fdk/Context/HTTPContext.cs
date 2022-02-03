using System;
using Microsoft.AspNetCore.Http;

namespace Fnproject.Fn.Fdk.Context
{
    internal class HTTPContext : IHTTPContext
    {
        private readonly static string FN_HTTP_REQUEST_URL = "Fn-Http-Request-Url";
        private readonly static string FN_HTTP_METHOD = "Fn-Http-Method";
        private readonly static string CONTENT_TYPE = "Content-Type";
        private readonly static string FN_HTTP_HEADER_PREFIX = "Fn-Http-H-";
        private readonly static string FN_FDK_VERSION_HEADER = "Fn-Fdk-Version";
        private readonly static string FN_FDK_RUNTIME_HEADER = "Fn-Fdk-Runtime";
        private readonly static string FN_HTTP_STATUS = "Fn-Http-Status";

        private IRuntimeContext runtimeContext;
        private IHeaderDictionary headers;
        private IQueryCollection queryParams;
        private IHeaderDictionary responseHeaders;

        public HTTPContext(IHeaderDictionary reqHeaders, IQueryCollection query)
        {
            runtimeContext = new RuntimeContext(reqHeaders);
            headers = new HeaderDictionary();
            responseHeaders = new HeaderDictionary();
            this.queryParams = query;

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
            responseHeaders.Add(key, value);
        }
        public void SetStatus(int statusCode)
        {
            responseHeaders.Add(FN_HTTP_STATUS, statusCode.ToString());
        }

        internal IHeaderDictionary ResponseHeaders()
        {
            responseHeaders.Add(FN_FDK_VERSION_HEADER, String.Format("fdk-csharp/{0}", Version.Get()));
            responseHeaders.Add(FN_FDK_RUNTIME_HEADER, String.Format("dotnet/{0}", System.Environment.Version.ToString()));
            if (!responseHeaders.ContainsKey(FN_HTTP_STATUS))
            {
                responseHeaders.Add(FN_HTTP_STATUS, 200.ToString());
            }
            return responseHeaders;
        }
    }
}
