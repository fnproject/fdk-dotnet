using Xunit;
using NSubstitute;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace FDK.Tests
{
    public class ResultTest
    {
        [Fact]
        public void RawResultTest()
        {
            string str = "Kevin";
            ConstructResult res = Substitute.For<RawResult>(str);
            res.Encoding = Encoding.UTF8;
            var ctx = Substitute.For<IRequestContext>();
            ctx.ContentType().Returns("text/plain");
            var response = Substitute.For<HttpResponse>();
            response.ContentType.Returns("text/plain");
            var responseHeaders = new HeaderDictionary
            {
                {"Fn-Http-Status","200"}
            };
            response.Headers.Returns(responseHeaders);
            res.WriteResult(response,ctx);
            res.Received().WriteResultBody(response);
        }

        [Fact]
        public void JsonResultTest()
        {
            string str = "@{'name' : 'Kevin'}";
            ConstructResult res = Substitute.For<JsonResult>(str);
            res.Encoding = Encoding.UTF8;
            var ctx = Substitute.For<IRequestContext>();
            ctx.ContentType().Returns("application/json");
            var response = Substitute.For<HttpResponse>();
            response.ContentType.Returns("application/json");
            var responseHeaders = new HeaderDictionary
            {
                {"Fn-Http-Status","200"}
            };
            response.Headers.Returns(responseHeaders);
            res.WriteResult(response,ctx);
            res.Received().WriteResultBody(response);
        }
    }
}