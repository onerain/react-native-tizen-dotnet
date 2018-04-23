using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tizen.Applications;
using NUnit.Framework;
using ReactNative.Modules.Core;
using ReactNative.Bridge;

namespace ReactNative.Tracing.Tests
{
    [TestFixture]
    public class TraceTests
    {
        [Test]
        [Description("TC Tracer Trace")]
        [Property("SPEC", "ReactNative.Tracing.Tracer.Trace M")]
        public static void Trace_Return()
        {
            var tracer = Tracer.Trace(0, "testname");
        }

        [Test]
        [Description("TC Tracer Write")]
        [Property("SPEC", "ReactNative.Tracing.Tracer.Write M")]
        public static void Write_Return()
        {
            Tracer.Write("testtag", "testname");
        }

        [Test]
        [Description("TC Tracer Error1")]
        [Property("SPEC", "ReactNative.Tracing.Tracer.Error M")]
        [Property("COVPARAM", "string,string,string,string,int")]
        public static void Error_ReturnA()
        {
            Tracer.Error("testtag", "testname");
        }

        [Test]
        [Description("TC Tracer Error2")]
        [Property("SPEC", "ReactNative.Tracing.Tracer.Error M")]
        [Property("COVPARAM", "string,string,System.Exception,string,string,int")]
        public static void Error_ReturnB()
        {
            Tracer.Error("testtag", "testname", new Exception());
        }
    }
}