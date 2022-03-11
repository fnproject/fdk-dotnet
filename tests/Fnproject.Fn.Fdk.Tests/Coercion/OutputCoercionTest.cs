using Fnproject.Fn.Fdk.Coercion;
using Xunit;

namespace Fnproject.Fn.Fdk.Tests
{
    public class OutputCoercionTest
    {
        public TestCustomOutputData testFunc()
        {
            return new TestCustomOutputData();
        }

        public class TestCustomOutputData : IOutputCoercible
        {
            public string name { get; set; }
            public string title { get; set; }

            public string Coerce()
            {
                return string.Format("{0},{1}", name, title);
            }
        }

        [Fact]
        public void TestCustomCoercion()
        {
            TestCustomOutputData result = new TestCustomOutputData();
            result.name = "myname";
            result.title = "mytitle";
            string output =
                OutputCoercion.Coerce(result, typeof(TestCustomOutputData));
            Assert.Equal("myname,mytitle", output);
        }

        [Fact]
        public void TestStringCoercion()
        {
            string result = "myresult";
            string output =
                OutputCoercion.Coerce(result, typeof(string));
            Assert.Equal(result, output);
        }

        [Fact]
        public void TestByteArrayCoercion()
        {
            string result = "myresult";
            byte[] resultBytes = System.Text.Encoding.UTF8.GetBytes(result);
            string output =
                OutputCoercion.Coerce(resultBytes, typeof(byte[]));
            Assert.Equal(result, output);
        }


        public class TestOutputData
        {
            public string name { get; set; }
            public string title { get; set; }
        }
        [Fact]
        public void TestJSONCoercion()
        {
            string result = "{\"name\":\"myname\",\"title\":\"mytitle\"}";
            var outputData = new TestOutputData();
            outputData.name = "myname";
            outputData.title = "mytitle";
            string output =
                OutputCoercion.Coerce(outputData, typeof(TestOutputData));
            Assert.Equal(result, output);
        }
    }
}
