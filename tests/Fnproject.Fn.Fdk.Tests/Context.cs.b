using Fnproject.Fn.Fdk.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
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
            const string APP_ID = "app-id-9000";
            const string FN_ID = "fn-id-over-9000";
            const string APP_NAME = "edward-elric";
            const string FN_NAME = "fullmetal";
            const string CALL_ID = "7-w-g-p-e-l-s-c";

            var headers = new HeaderDictionary();
            headers.Add("Content-Type", REQ_CONTENT_TYPE);
            headers.Add("Fn-Http-H-FN_APP_ID", APP_ID);
            headers.Add("Fn-Http-H-FN_FN_ID", FN_ID);
            headers.Add("Fn-Http-H-FN_APP_NAME", APP_NAME);
            headers.Add("Fn-Http-H-FN_FN_NAME", FN_NAME);
            headers.Add("Fn-Http-H-Fn-Call-Id", CALL_ID);
            headers.Add("A-dropped-header", "any-val");

            IHTTPContext ctx = new Fnproject.Fn.Fdk.HTTPContext(headers);

            Assert.True(ctx.Headers()["Content-Type"] == REQ_CONTENT_TYPE, "Content type check");
            Assert.True(ctx.AppID() == APP_ID, "App ID check");
            Assert.True(ctx.FunctionID() == FN_ID, "Fn ID check");
            Assert.True(ctx.AppName() == APP_NAME, "App name check");
            Assert.True(ctx.FunctionName() == FN_NAME, "Fn name check");
            Assert.True(ctx.CallID() == CALL_ID, "Call ID check");
            Assert.True(ctx.Headers()["A-dropped-header"] == StringValues.Empty, "Garbage header check");
        }

        [Fact]
        public void TestHTTPData()
        {
            const string REQUEST_URL = "http://fnproject.io/api/";
            const string REQUEST_METHOD = "POST";

            var headers = new HeaderDictionary();
            headers.Add("Fn-Http-Request-Url", REQUEST_URL);
            headers.Add("Fn-Http-Method", REQUEST_METHOD);

            IContext ctx = new Fnproject.Fn.Fdk.Context(headers);

            Assert.True(ctx.RequestURL() == REQUEST_URL, "Request URL check");
            Assert.True(ctx.RequestMethod() == REQUEST_METHOD, "Request Method check");
        }

        [Fact]
        public void TestResponseHeaderCreation()
        {
            var headers = new HeaderDictionary();

            // ResponseHeaders() is not available on IContext, as user doesn't need to read them.
            Fnproject.Fn.Fdk.Context ctx = new Fnproject.Fn.Fdk.Context(headers);

            ctx.AddHeader("x-power-meter-says", "over-9000");

            var responseHeaders = ctx.ResponseHeaders();

            Assert.True(responseHeaders["x-power-meter-says"] == "over-9000",
                "Custom header check");
            Assert.True(responseHeaders["x-some-header-i-didn't-add"] == StringValues.Empty,
                "Header not present check");
        }

        [Fact]
        public void TestStatusCodeAddition()
        {
            var headers = new HeaderDictionary();

            // ResponseHeaders() is not available on IContext, as user doesn't need to read them.
            Fnproject.Fn.Fdk.Context ctx = new Fnproject.Fn.Fdk.Context(headers);

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
            Fnproject.Fn.Fdk.Context ctx = new Fnproject.Fn.Fdk.Context(headers);

            var responseHeaders = ctx.ResponseHeaders();

            Assert.True(responseHeaders["Fn-Http-Status"] == "200",
                "Default status code check");
        }

        [Fact]
        public void TestRuntimeAndVersion()
        {
            var headers = new HeaderDictionary();

            // ResponseHeaders() is not available on IContext, as user doesn't need to read them.
            Fnproject.Fn.Fdk.Context ctx = new Fnproject.Fn.Fdk.Context(headers);

            var responseHeaders = ctx.ResponseHeaders();

            Assert.True(responseHeaders["Fn-Fdk-Runtime"][0].Length > 0,
                "Runtime version header check");
            Assert.True(responseHeaders["Fn-Fdk-Version"][0].Length > 0,
                "FDK version header check");
        }
    }
}
