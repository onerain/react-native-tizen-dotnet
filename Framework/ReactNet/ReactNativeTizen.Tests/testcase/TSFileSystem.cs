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


namespace PCLStorage.Tests
{
    [TestFixture]
    public class FileSystemTests
    {
        [Test]
        [Description("TC FileSystem Current")]
        [Property("SPEC", "PCLStorage.FileSystem.Current A")]
        [Property("COVPARAM", "string,System.Exception")]
        public static void Current_Return()
        {
            var current = FileSystem.Current;
            Assert.NotNull(current);
        }
    }
}