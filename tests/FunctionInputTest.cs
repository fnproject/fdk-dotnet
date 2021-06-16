using Xunit;
using System.IO;
using System.Text;
using System.Collections.Generic;
using NSubstitute;
using Microsoft.AspNetCore.Http;

namespace FDK.Tests
{
    public class MyInput
    {
        public string name{get; set;}
        public int age{get; set;}

    }

    public class FunctionInputTest
    {

        private FunctionInput CreateFunctionInput(string input,IRequestContext ctx)
        {
            var context = Substitute.For<HttpContext>();
            MemoryStream memoryStream = new MemoryStream();
            byte[] byteArray = Encoding.ASCII.GetBytes(input);
            memoryStream.Write(byteArray,0,byteArray.Length);
            context.Request.Body.Returns(memoryStream);
            var functionInput = Substitute.For<FunctionInput>(ctx,context);
            return functionInput;
        }

        [Fact]
        public void TestSplitString()
        {
            string input = "Kevin 18, jackson, (91";
            IRequestContext ctx = Substitute.For<IRequestContext>();
            ctx.ContentType().Returns("text/plain");
            FunctionInput functionInput = CreateFunctionInput(input,ctx);
            string[] splitString = {"Kevin" , "18", "jackson", "91"};
            Assert.Equal(splitString,functionInput.SplitString(input));
        }

        [Fact]
        public void TestConvertToDictionary()
        {
            string input = "Kevin 18, jackson, (91";
            IRequestContext ctx = Substitute.For<IRequestContext>();
            ctx.ContentType().Returns("text/plain");
            FunctionInput functionInput = CreateFunctionInput(input,ctx);
            Dictionary<string,string> dict = functionInput.ConvertToDictionary(input);
            Assert.Equal(4,dict.Count);
            Assert.Equal("Kevin",dict["arg1"]);
            Assert.Equal("18",dict["arg2"]);
            Assert.Equal("jackson",dict["arg3"]);
            Assert.Equal("91",dict["arg4"]);
        }

        [Fact]
        public void TestJsonDeserializer()
        {
            string input ="{'Name': 'Kevin', 'Age' : 18}";
            IRequestContext ctx = Substitute.For<IRequestContext>();
            ctx.ContentType().Returns("application/json");
            FunctionInput functionInput = CreateFunctionInput(input,ctx);
            MyInput obj = functionInput.AccessJsonInput<MyInput>();
            // Assert.Equal(input,");
            // Assert.Equal("Kevin",obj.name);
            // Assert.Equal(18,obj.age);
        }
    }
}