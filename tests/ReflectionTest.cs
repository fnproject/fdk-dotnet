using Xunit;
using System;
using System.Reflection;

namespace FDK.Tests
{
    public class UserClass
    {
        public static string UserMethod()
        {
            return "Hello World";
        }
    }

    public class UserClassOne
    {
        public string UserMethod()
        {
            return "Hello World Non Static";
        }
    }

    public class ReflectionTest
    {
        [Fact]
        public void TestHandlerFunctionForNonStaticMethod()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Type functionType = executingAssembly.GetType("FDK.Tests.UserClassOne");
            object functionInstance = Activator.CreateInstance(functionType);
            InvokeClass.FunctionDissection(functionType,functionInstance);
            Assert.Equal("UserMethod",InvokeClass._functionName);
            Assert.Equal("System.String",InvokeClass._returnType.ParameterType.ToString());
        }

        [Fact]
        public void TestHandlerFunction()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Type functionType = executingAssembly.GetType("FDK.Tests.UserClass");
            object functionInstance = Activator.CreateInstance(functionType);
            InvokeClass.FunctionDissection(functionType,functionInstance);
            Assert.Equal("UserMethod",InvokeClass._functionName);
            Assert.Equal("System.String", InvokeClass._returnType.ParameterType.ToString());
            Assert.Equal(0,InvokeClass._parameters.Length);
        }

        [Fact]
        public void TestRunUserFunction()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Type functionType = executingAssembly.GetType("FDK.Tests.UserClass");
            object functionInstance = Activator.CreateInstance(functionType);
            InvokeClass.FunctionDissection(functionType,functionInstance);
            object output = InvokeClass._userFunction.Invoke(InvokeClass._functionInstance,null);
            Assert.Equal("Hello World",(string)output);
        }
    }
}