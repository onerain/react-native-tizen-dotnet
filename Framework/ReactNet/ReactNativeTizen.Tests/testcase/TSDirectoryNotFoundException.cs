using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tizen.Applications;
using NUnit.Framework;
using PCLStorage;

namespace PCLStorage.Exceptions.Tests
{
    [TestFixture]
    public class DirectoryNotFoundExceptionTests
    {
        [Test]
        [Description("TC DirectoryNotFoundException Ctor")]
        [Property("SPEC", "PCLStorage.Exceptions.DirectoryNotFoundException.DirectoryNotFoundException C")]
        [Property("COVPARAM", "string")]
        public static void DirectoryNotFoundException_ReturnA()
        {
            var exception = new DirectoryNotFoundException("test");
        }

        [Test]
        [Description("TC DirectoryNotFoundException Ctor2")]
        [Property("SPEC", "PCLStorage.Exceptions.DirectoryNotFoundException.DirectoryNotFoundException C")]
        [Property("COVPARAM", "string,System.Exception")]
        public static void DirectoryNotFoundException_ReturnB()
        {
            var exception = new DirectoryNotFoundException("test", new Exception());
        }
    }
}