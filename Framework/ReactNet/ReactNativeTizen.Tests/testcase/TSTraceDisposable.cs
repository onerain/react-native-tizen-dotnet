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
    public class TraceDisposableTests
    {
        [Test]
        [Description("TC Ctor")]
        [Property("SPEC", "ReactNative.Tracing.TraceDisposable.TraceDisposable C")]
        public static void TraceDisposable_Return()
        {
            var traceDisposable = new TraceDisposable(0, "test", "", "", 0);
            Assert.NotNull(traceDisposable);
        }

        [Test]
        [Description("TC Start")]
        [Property("SPEC", "ReactNative.Tracing.TraceDisposable.Start M")]
        public static void Start_Return()
        {
            var traceDisposable = new TraceDisposable(0, "test", "", "", 0);
            var ret = traceDisposable.Start();
            Assert.NotNull(ret);
        }

        [Test]
        [Description("TC With")]
        [Property("SPEC", "ReactNative.Tracing.TraceDisposable.With M")]
        public static void With_Return()
        {
            var traceDisposable = new TraceDisposable(0, "test", "", "", 0);
            var ret = traceDisposable.With("testkey","testvalue");
            Assert.NotNull(ret);
        }

        [Test]
        [Description("TC Dispose")]
        [Property("SPEC", "ReactNative.Tracing.TraceDisposable.Dispose M")]
        public static void Dispose_Return()
        {
            var traceDisposable = new TraceDisposable(0, "test", "", "", 0);
            traceDisposable.Dispose();
        }
    }
}