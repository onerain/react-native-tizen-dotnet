using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using PCLStorage;
using System;
using System.Threading.Tasks;
using Tizen;
using ReactNative.Common;

namespace ReactNative.Bridge.Tests
{
    [TestFixture]
    public class JavaScriptBundleLoaderTests
    {
        [Test]
        [Description("TC JavaScriptBundleLoader Ctor")]
        [Property("SPEC", "ReactNative.Bridge.JavaScriptBundleLoader.JavaScriptBundleLoader C")]
        public static void JavaScriptBundleLoader_Return()
        {
            //tools error JavaScriptBundleLoader is abstract could not call ctor directly!!
        }

        [Test]
        [Description("TC JavaScriptBundleLoader CreateCachedBundleFromNetworkLoader")]
        [Property("SPEC", "ReactNative.Bridge.JavaScriptBundleLoader.CreateCachedBundleFromNetworkLoader M")]
        public static void CreateCachedBundleFromNetworkLoader_Return()
        {
            var cachedBundleLoader = JavaScriptBundleLoader.CreateCachedBundleFromNetworkLoader(
                                "127.0.0.1:8081",
                                "tizen.bundle");
            Assert.NotNull(cachedBundleLoader);
        }

        [Test]
        [Description("TC JavaScriptBundleLoader CreateFileLoader")]
        [Property("SPEC", "ReactNative.Bridge.JavaScriptBundleLoader.CreateFileLoader M")]
        public static void CreateFileLoader_Return()
        {
            var fileBundleLoader = JavaScriptBundleLoader.CreateFileLoader("tizen.bundle");
            Assert.NotNull(fileBundleLoader);
        }

        [Test]
        [Description("TC JavaScriptBundleLoader CreateRemoteDebuggerLoader")]
        [Property("SPEC", "ReactNative.Bridge.JavaScriptBundleLoader.CreateRemoteDebuggerLoader M")]
        public static void CreateRemoteDebuggerLoader_Return()
        {
            var remoteBundleLoader = JavaScriptBundleLoader.CreateRemoteDebuggerLoader("127.0.0.1", "127.0.0.1");
            Assert.NotNull(remoteBundleLoader);
        }

        [Test]
        [Description("TC JavaScriptBundleLoader InitializeAsync")]
        [Property("SPEC", "ReactNative.Bridge.JavaScriptBundleLoader.InitializeAsync M")]
        public async void InitializeAsync_Return()
        {
            var fileBundle = JavaScriptBundleLoader.CreateFileLoader("tizen.bundle");
            await fileBundle.InitializeAsync().ConfigureAwait(false);
        }

        [Test]
        [Description("TC JavaScriptBundleLoader SourceUrl")]
        [Property("SPEC", "ReactNative.Bridge.JavaScriptBundleLoader.SourceUrl A")]
        public void SourceUrl_Return()
        {
            var fileBundle = JavaScriptBundleLoader.CreateFileLoader("tizen.bundle");
            var sourceUrl = fileBundle.SourceUrl;
        }

        [Test]
        [Description("TC JavaScriptBundleLoader LoadScript")]
        [Property("SPEC", "ReactNative.Bridge.JavaScriptBundleLoader.LoadScript A")]
        public void LoadScript_Return()
        {
            var fileBundle = JavaScriptBundleLoader.CreateFileLoader("tizen.bundle");

            try
            {
                fileBundle.LoadScript(null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.Null(ex.Message);
            }
        }
    }
}