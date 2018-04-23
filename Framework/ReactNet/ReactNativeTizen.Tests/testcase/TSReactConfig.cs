using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using ReactNative.Common;
using NUnit.Framework;


namespace ReactNative.Common.Tests
{
    [TestFixture]
    public class ReactConfigTests
    {
        [Test]
        [Description("TC ReactConfig GetValue")]
        [Property("SPEC", "ReactNative.Common.ReactConfig.GetValue M")]
        public void GetValue_Return()
        {
            ReactConfig.SetValue("test","testsetvalue");
            var value = ReactConfig.GetValue("test");
            Assert.AreEqual("testsetvalue", value);
        }

        [Test]
        [Description("TC ReactConfig SetValue")]
        [Property("SPEC", "ReactNative.Common.ReactConfig.SetValue M")]
        public void SetValue_Return()
        {
            ReactConfig.SetValue("test", "testsetvalue");
        }

    }
}