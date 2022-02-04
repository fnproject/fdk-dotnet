using System;
using Fnproject.Fn.Fdk;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using Xunit;

namespace Fnproject.Fn.Fdk.Tests
{
    public class Function
    {
        [Fact]
        public void TestFuncWithNoArgs()
        {
            string func(int val) { return ""; }
        }
    }
}
