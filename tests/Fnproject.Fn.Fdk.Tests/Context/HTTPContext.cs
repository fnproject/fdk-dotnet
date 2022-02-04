using System;
using Fnproject.Fn.Fdk.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using Xunit;

namespace Fnproject.Fn.Fdk.Tests
{
    public class Context
    {
        [Fact]
        public void TestContextCreation()
        {
            const string REQ_CONTENT_TYPE = "text/plain";
            const string CALL_ID = "7-w-g-p-e-l-s-c";
            const string SINGLE_QUERY = "test-query-value";
            string[] MULTI_QUERY = new string[]{"val-1", "val-2"};

            var headers = new HeaderDictionary();
            headers.Add("Content-Type", REQ_CONTENT_TYPE);
            headers.Add("Fn-Http-H-Fn-Call-Id", CALL_ID);
            headers.Add("A-dropped-header", "any-val");
            

            Dictionary<string, StringValues> query = new Dictionary<string, StringValues>();
            query.Add("test-query", SINGLE_QUERY);
            query.Add("test-query-multi", MULTI_QUERY);

            IHTTPContext ctx = new HTTPContext(headers, new QueryCollection(query));
            Assert.Equal(REQ_CONTENT_TYPE, ctx.Headers()["Content-Type"]);
            Assert.Equal(CALL_ID, ctx.RuntimeContext().CallID());
            Assert.True(ctx.Headers()["A-dropped-header"] == StringValues.Empty, "Garbage header check");
            Assert.Equal(SINGLE_QUERY, ctx.Query()["test-query"]);
            Assert.Equal(MULTI_QUERY, ctx.Query()["test-query-multi"].ToArray());
        }

        [Fact]
        public void TestHTTPData()
        {
            Uri uri = new Uri("http://fnproject.io/api/?q=test");

            string REQUEST_URL = uri.AbsoluteUri;
            const string REQUEST_METHOD = "POST";

            var headers = new HeaderDictionary();
            headers.Add("Fn-Http-Request-Url", REQUEST_URL);
            headers.Add("Fn-Http-Method", REQUEST_METHOD);

            var queryDict = new Dictionary<string, StringValues>();
            queryDict.Add("q", "test");
            IHTTPContext ctx = new HTTPContext(headers, new QueryCollection(queryDict));

            Assert.Equal(REQUEST_URL, ctx.RequestURL());
            Assert.Equal(REQUEST_METHOD, ctx.RequestMethod());
            Assert.NotNull(ctx.GetQueryByKey("q"));
            Assert.Equal(ctx.GetQueryByKey("q"), new string[]{"test"});
        }

        [Fact]
        public void TestResponseHeaderCreation()
        {
            var headers = new HeaderDictionary();

            // ResponseHeaders() is not available on IContext, as user doesn't need to read them.
            HTTPContext ctx = new HTTPContext(headers, new QueryCollection(
                new Dictionary<string, StringValues>()));

            ctx.AddHeader("x-power-meter-says", "over-9000");

            var responseHeaders = ctx.ResponseHeaders();

            Assert.Equal("over-9000", responseHeaders["x-power-meter-says"]);
            Assert.True(responseHeaders["x-some-header-i-didn't-add"] == StringValues.Empty,
                "Header not present check");
        }

        [Fact]
        public void TestStatusCodeAddition()
        {
            var headers = new HeaderDictionary();

            // ResponseHeaders() is not available on IContext, as user doesn't need to read them.
            HTTPContext ctx = new HTTPContext(headers, new QueryCollection(
                new Dictionary<string, StringValues>()));

            ctx.SetStatus(202);

            var responseHeaders = ctx.ResponseHeaders();

            Assert.True(responseHeaders["Fn-Http-Status"] == "202",
                "Status code check");
        }

        [Fact]
        public void TestStatusCodeDefault()
        {
            var headers = new HeaderDictionary();

            // ResponseHeaders() is not available on IContext, as user doesn't need to read them.
            HTTPContext ctx = new HTTPContext(headers, new QueryCollection(
                new Dictionary<string, StringValues>()));

            var responseHeaders = ctx.ResponseHeaders();

            Assert.True(responseHeaders["Fn-Http-Status"] == "200",
                "Default status code check");
        }

        [Fact]
        public void TestRuntimeAndVersion()
        {
            var headers = new HeaderDictionary();

            // ResponseHeaders() is not available on IContext, as user doesn't need to read them.
            HTTPContext ctx = new HTTPContext(headers, new QueryCollection(
                new Dictionary<string, StringValues>()));

            var responseHeaders = ctx.ResponseHeaders();

            Assert.True(responseHeaders["Fn-Fdk-Runtime"][0].Length > 0,
                "Runtime version header check");
            Assert.True(responseHeaders["Fn-Fdk-Version"][0].Length > 0,
                "FDK version header check");
        }
    }
}
