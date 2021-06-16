using Xunit;
using System;
using NSubstitute;
using Microsoft.AspNetCore.Http;

namespace FDK.Tests
{
    public class RequestHeadersTest
    {
        [Fact]
        public void TestRequestHeaders()
        {
            //Check if the Request Headers have been correctly deserialized
            var requestHeaders = new HeaderDictionary
            {
                {"Fn-Call-Id","120034ABQ"},
                {"Fn-Deadline",(new DateTime(1000,1,1)).ToString()},
                {"Fn-Http-Request-Url","http://domainname.com"},
                {"Fn-Intent","application/json"},
                {"Content-Type","application/json"},
            };
            var context = Substitute.For<HttpContext>();
            context.Request.Headers.Returns(requestHeaders);
            IRequestContext ctx = new RequestContext(context);
            IHeaderDictionary fn_headers = ctx.Header(requestHeaders);
            Assert.Equal("120034ABQ",ctx.CallID());
            Assert.Equal("application/json",ctx.ContentType());
            Assert.Equal("application/json",ctx.Intent());
            Assert.Equal(new DateTime(1000,1,1),ctx.Deadline());
            Assert.Equal("http://domainname.com",ctx.RequestURL());
        }
    }
}