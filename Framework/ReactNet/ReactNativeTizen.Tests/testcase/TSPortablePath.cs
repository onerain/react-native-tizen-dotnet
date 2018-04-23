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
    public class PortablePathTests
    {
        [Test]
        [Description("TC PortablePath DirectorySeparatorChar")]
        [Property("SPEC", "PCLStorage.PortablePath.DirectorySeparatorChar A")]
        public static void DirectorySeparatorChar_Return()
        {
            var separatorChar = PortablePath.DirectorySeparatorChar;
            Assert.NotNull(separatorChar);
        }

        [Test]
        [Description("TC PortablePath Combine")]
        [Property("SPEC", "PCLStorage.PortablePath.Combine M")]
        public static void Combine_Return()
        {
            var stringCombined = PortablePath.Combine("AAAA"+"BBBBB");
            Assert.AreEqual(stringCombined, "AAAABBBB");
        }
    }
}