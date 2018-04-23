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

namespace ReactNative.Modules.Core.Tests
{
    [TestFixture]
    public class TimingTests
    {
        static Timing _timer;

        [Order(0)]
        [Test]
        [Description("TC Ctor")]
        [Property("SPEC", "ReactNative.Modules.Core.Timing.Timing C")]
        public static void Timing_Return()
        {
            ReactContext _context = new ReactContext();
            _timer = new Timing(_context);
            Assert.NotNull(_context);
        }

        [Order(1)]
        [Test]
        [Description("TC Name")]
        [Property("SPEC", "ReactNative.Modules.Core.Timing.Name A")]
        public static void Name_Return()
        {
            var name = _timer.Name;
            Assert.AreEqual("RCTTiming", name);
        }

        [Order(2)]
        [Test]
        [Description("TC Initialize")]
        [Property("SPEC", "ReactNative.Modules.Core.Timing.Initialize M")]
        public static void Initialize_Return()
        {
            _timer.Initialize();
        }

        [Order(3)]
        [Test]
        [Description("TC createTimer")]
        [Property("SPEC", "ReactNative.Modules.Core.Timing.createTimer M")]
        public static void createTimer_Return()
        {
            _timer.createTimer(1,1000, DateTimeOffset.Now.ToUnixTimeSeconds() + 10.0, false);
        }

        [Order(4)]
        [Test]
        [Description("TC OnSuspend")]
        [Property("SPEC", "ReactNative.Modules.Core.Timing.OnSuspend M")]
        public static void OnSuspend_Return()
        {
            _timer.OnSuspend();
        }


        [Order(5)]
        [Test]
        [Description("TC OnResume")]
        [Property("SPEC", "ReactNative.Modules.Core.Timing.OnResume M")]
        public static void OnResume_Return()
        {
            _timer.OnResume();
        }

        [Order(6)]
        [Test]
        [Description("TC deleteTimer")]
        [Property("SPEC", "ReactNative.Modules.Core.Timing.deleteTimer M")]
        public static void deleteTimer_Return()
        {
            _timer.deleteTimer(1);
        }

        [Order(7)]
        [Test]
        [Description("TC OnDestroy")]
        [Property("SPEC", "ReactNative.Modules.Core.Timing.OnDestroy M")]
        public static void OnDestroy_Return()
        {
            _timer.OnDestroy();
        }
    }
}