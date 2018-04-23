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
    public class FileNotFoundExceptionTests
    {
        [Test]
        [Description("TC FileNotFoundException Ctor")]
        [Property("SPEC", "PCLStorage.Exceptions.FileNotFoundException.FileNotFoundException C")]
        [Property("COVPARAM", "string")]
        public static void FileNotFoundException_ReturnA()
        {
            var exception = new FileNotFoundException("test");
        }

        [Test]
        [Description("TC FileNotFoundException Ctor2")]
        [Property("SPEC", "PCLStorage.Exceptions.FileNotFoundException.FileNotFoundException C")]
        [Property("COVPARAM", "string,System.Exception")]
        public static void FileNotFoundException_ReturnB()
        {
            var exception = new FileNotFoundException("test",new Exception());
        }
    }
}