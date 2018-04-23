using NUnit.Framework;
using ReactNative.Bridge;
using ReactNative.Bridge.Queue;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Tizen;

namespace ReactNative.Bridge.Queue.Tests
{
    [TestFixture]
    public class MessageQueueThreadTests
    {
        [Test]
        [Description("TC MessageQueueThread Create")]
        [Property("SPEC", "ReactNative.Bridge.Queue.MessageQueueThread.Create M")]
        public async static void Create_Return()
        {
            var thrown = 0;
            var uiThread = await DispatcherHelpers.CallOnDispatcher(() => MessageQueueThread.Create(MessageQueueThreadSpec.DispatcherThreadSpec, ex => thrown++));
            var backgroundThread = MessageQueueThread.Create(MessageQueueThreadSpec.Create("background", MessageQueueThreadKind.BackgroundSingleThread), ex => thrown++);
            var taskPoolThread = MessageQueueThread.Create(MessageQueueThreadSpec.Create("any", MessageQueueThreadKind.BackgroundAnyThread), ex => thrown++);
            Assert.NotNull(uiThread);
            Assert.NotNull(backgroundThread);
            Assert.NotNull(taskPoolThread);
        }

        [Test]
        [Description("TC MessageQueueThread RunOnQueue")]
        [Property("SPEC", "ReactNative.Bridge.Queue.MessageQueueThread.RunOnQueue M")]
        public async void RunOnQueue_Return()
        {
            var thrown = 0;
            var uiThread = await DispatcherHelpers.CallOnDispatcher(() => MessageQueueThread.Create(MessageQueueThreadSpec.DispatcherThreadSpec, ex => thrown++));
            var backgroundThread = MessageQueueThread.Create(MessageQueueThreadSpec.Create("background", MessageQueueThreadKind.BackgroundSingleThread), ex => thrown++);
            var taskPoolThread = MessageQueueThread.Create(MessageQueueThreadSpec.Create("any", MessageQueueThreadKind.BackgroundAnyThread), ex => thrown++);

            var queueThreads = new[]
            {
                uiThread,
                backgroundThread,
                taskPoolThread
            };

            foreach (var queueThread in queueThreads)
            {
                queueThread.RunOnQueue(() =>
                {
                    Log.Info("RNTests", queueThread.ToString());
                });
            }

            Assert.AreEqual(0, thrown);
        }

        [Test]
        [Description("TC MessageQueueThread IsOnThread")]
        [Property("SPEC", "ReactNative.Bridge.Queue.MessageQueueThread.IsOnThread M")]
        public async void IsOnThread_Return()
        {
            var thrown = 0;
            var uiThread = await DispatcherHelpers.CallOnDispatcher(() => MessageQueueThread.Create(MessageQueueThreadSpec.DispatcherThreadSpec, ex => thrown++));
            var backgroundThread = MessageQueueThread.Create(MessageQueueThreadSpec.Create("background", MessageQueueThreadKind.BackgroundSingleThread), ex => thrown++);
            var taskPoolThread = MessageQueueThread.Create(MessageQueueThreadSpec.Create("any", MessageQueueThreadKind.BackgroundAnyThread), ex => thrown++);

            var queueThreads = new[]
            {
                uiThread,
                backgroundThread,
                taskPoolThread
            };

            var countdown = new CountdownEvent(queueThreads.Length);
            foreach (var queueThread in queueThreads)
            {
                queueThread.RunOnQueue(() =>
                {
                    Assert.IsTrue(queueThread.IsOnThread());
                    countdown.Signal();
                });
            }

            Assert.IsTrue(countdown.Wait(5000));
            Assert.AreEqual(0, thrown);
        }

        [Test]
        [Description("TC MessageQueueThread Enqueue")]
        [Property("SPEC", "ReactNative.Bridge.Queue.MessageQueueThread.Enqueue M")]
        public void Enqueue_Return()
        {
            //Enqueue is protected , cannot do UT, coverage tool ERROR!
        }

        [Test]
        [Description("TC MessageQueueThread IsOnThreadCore")]
        [Property("SPEC", "ReactNative.Bridge.Queue.MessageQueueThread.IsOnThreadCore M")]
        public void IsOnThreadCore_Return()
        {
            //IsOnThreadCore is protected , cannot do UT, coverage tool ERROR!
        }

        [Test]
        [Description("TC MessageQueueThread IsDisposed")]
        [Property("SPEC", "ReactNative.Bridge.Queue.MessageQueueThread.IsDisposed M")]
        public void IsDisposed_Return()
        {
            //IsDisposed is protected , cannot do UT, coverage tool ERROR!
        }

        [Test]
        [Description("TC MessageQueueThread Dispose")]
        [Property("SPEC", "ReactNative.Bridge.Queue.MessageQueueThread.Dispose M")]
        [Property("COVPARAM", "")]
        public void Dispose_ReturnA()
        {
            //IsDisposed is protected , cannot do UT, coverage tool ERROR!
        }

        [Test]
        [Description("TC MessageQueueThread Dispose")]
        [Property("SPEC", "ReactNative.Bridge.Queue.MessageQueueThread.Dispose M")]
        [Property("COVPARAM", "bool")]
        public void Dispose_ReturnB()
        {
            //IsDisposed is protected , cannot do UT, coverage tool ERROR!
        }

    }
}

