using Fnproject.Fn.Fdk.Coercion;
using Xunit;

namespace Fnproject.Fn.Fdk.Tests
{
    public class InputCoercionTest
    {
        internal class TestCustomInputData : IInputCoercible
        {
            public string name { get; set; }
            public string title { get; set; }

            public object Coerce(string s)
            {
                var inputCSVDataSegments = s.Split(',');
                TestCustomInputData data = new TestCustomInputData();
                data.name = inputCSVDataSegments[0];
                data.title = inputCSVDataSegments[1];
                return data;
            }
        }

        [Fact]
        public void TestCustomCoercion()
        {
            TestCustomInputData data =
                (TestCustomInputData)InputCoercion.Coerce("myname,mytitle",
                    typeof(TestCustomInputData));
            Assert.Equal("myname", data.name);
            Assert.Equal("mytitle", data.title);
        }

        [Fact]
        public void TestStringCoercion()
        {
            string data =
                (string)InputCoercion.Coerce("hello-peter", typeof(string));
            Assert.Equal("hello-peter", data);
        }

        [Fact]
        public void TestByteArrayCoercion()
        {
            string input = "hola-peter";
            byte[] output = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] data =
                (byte[])InputCoercion.Coerce(
                    input, typeof(byte[]));
            Assert.Equal(data, output);
        }

        internal class TestInputData
        {
            public string name { get; set; }
            public string title { get; set; }
        }

        [Fact]
        public void TestJSONCoercion()
        {
            TestInputData data =
                (TestInputData)InputCoercion.Coerce(
                    "{\"name\":\"myname\", \"title\":\"mytitle\"}", typeof(TestInputData));
            Assert.Equal("myname", data.name);
            Assert.Equal("mytitle", data.title);
        }
    }
}
