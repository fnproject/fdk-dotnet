using Xunit;
using NSubstitute;
using FDK.Exceptions;
using System.Runtime.InteropServices;

namespace FDK.Tests
{
    public class ContainerTests
    {
        private IContainerEnvironment CreateEnvironment()
        {
            //Creating an instance of containerEnvironment to only work within the test environment.
            var containerEnvironment = Substitute.For<IContainerEnvironment>();
            containerEnvironment.SOCKET_TYPE = "Unix";
            containerEnvironment.FN_FORMAT = "http-stream";
            return containerEnvironment;
        }

        [Fact]
        public void TestValidContainer()
        {
            var containerEnvironment = CreateEnvironment();
            ContainerEnvironmentValidator.Validate(containerEnvironment);
        }
        [Fact] 
        public void CheckFormat()
        {
            //Check if the format is "http-stream".
            var containerEnvironment = CreateEnvironment();
            containerEnvironment.FN_FORMAT.Returns("invalid");
            var exceptionThrown = Assert.Throws<InvalidEnvironmentException>(() => ContainerEnvironmentValidator.Validate(containerEnvironment));
            Assert.Equal("http-stream is the only supported format", exceptionThrown.Message);
        }

        [Fact]
        public void CheckSocketType()
        {
            //Check if the socket type is "Unix".
            var containerEnvironment = CreateEnvironment();
            containerEnvironment.SOCKET_TYPE.Returns("Unknown Socket");
            var exceptionThrown = Assert.Throws<InvalidEnvironmentException>(() => ContainerEnvironmentValidator.Validate(containerEnvironment));
            Assert.Equal("FDK is compliant with only Unix Domain Sockets", exceptionThrown.Message);
        }

        [Fact]
        public void CheckOS()
        {
            //FDK will not work on Windows as it does not support UDS.
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var containerEnvironment = CreateEnvironment();
                var exceptionThrown = Assert.Throws<InvalidEnvironmentException>(() => ContainerEnvironmentValidator.Validate(containerEnvironment));
                Assert.Equal("Unix domain sockets are not supported on Windows",exceptionThrown.Message);
            }
        }

        [Fact]
        public void CheckUnixSocketListener()
        {
            //FN_LISTENER must be a file path starting with unix prefix.
            var containerEnvironment = CreateEnvironment();
            containerEnvironment.FN_LISTENER.Returns("invalidSocket");
            Assert.False(containerEnvironment.FN_LISTENER.StartsWith("unix:"), "FN_LISTENER must start with unix:/");
        }
    }
}
