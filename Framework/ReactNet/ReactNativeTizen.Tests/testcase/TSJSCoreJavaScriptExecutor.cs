using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Threading.Tasks;


namespace ReactNative.JavaScriptCore.Executor.Tests
{
    [TestFixture]
    public class JSCoreJavaScriptExecutorTests
    {

        [Test]
        [Description("TC JSCoreJavaScriptExecutor Ctor")]
        [Property("SPEC", "ReactNative.JavaScriptCore.Executor.JSCoreJavaScriptExecutor.JSCoreJavaScriptExecutor C")]
        public static void JSCoreJavaScriptExecutor_Return()
        {
            JSCoreJavaScriptExecutor tempExecutor = new JSCoreJavaScriptExecutor();
        }

        [Test]
        [Description("TC JSCoreJavaScriptExecutor Runscript")]
        [Property("SPEC", "ReactNative.JavaScriptCore.Executor.JSCoreJavaScriptExecutor.RunScript M")]
        public static void RunScript_Return()
        {
            JSCoreJavaScriptExecutor executor = new JSCoreJavaScriptExecutor();

            Assert.That(
                () => executor.RunScript(null, "foo"),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("script")
            );

            Assert.That(
                () => executor.RunScript("", null),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("sourceUrl")
            );
        }


        [Test]
        [Description("TC JSCoreJavaScriptExecutor CallFunctionReturnFlushedQueue")]
        [Property("SPEC", "ReactNative.JavaScriptCore.Executor.JSCoreJavaScriptExecutor.CallFunctionReturnFlushedQueue M")]
        public static void CallFunctionReturnFlushedQueue_Return()
        {
            JSCoreJavaScriptExecutor executor = new JSCoreJavaScriptExecutor();

            Assert.That(
                () => executor.CallFunctionReturnFlushedQueue("1", "1", new JArray()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("null")
            );
        }

        [Test]
        [Description("TC JSCoreJavaScriptExecutor Dispose")]
        [Property("SPEC", "ReactNative.JavaScriptCore.Executor.JSCoreJavaScriptExecutor.Dispose M")]
        public static void Dispose_Return()
        {
            JSCoreJavaScriptExecutor executor = new JSCoreJavaScriptExecutor();

            Assert.That(
                () => executor.Dispose(),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("null")
            );
        }

        [Test]
        [Description("TC JSCoreJavaScriptExecutor FlushedQueue")]
        [Property("SPEC", "ReactNative.JavaScriptCore.Executor.JSCoreJavaScriptExecutor.FlushedQueue M")]
        public static void FlushedQueue_Return()
        {
            JSCoreJavaScriptExecutor executor = new JSCoreJavaScriptExecutor();

            Assert.That(
                () => executor.FlushedQueue(),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("null")
            );
        }

        [Test]
        [Description("TC JSCoreJavaScriptExecutor GetGlobalVariable")]
        [Property("SPEC", "ReactNative.JavaScriptCore.Executor.JSCoreJavaScriptExecutor.GetGlobalVariable M")]
        public static void GetGlobalVariable_Return()
        {
            JSCoreJavaScriptExecutor executor = new JSCoreJavaScriptExecutor();

            Assert.That(
                () => executor.GetGlobalVariable("try"),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("null")
            );
        }

        [Test]
        [Description("TC JSCoreJavaScriptExecutor InvokeCallbackAndReturnFlushedQueue")]
        [Property("SPEC", "ReactNative.JavaScriptCore.Executor.JSCoreJavaScriptExecutor.InvokeCallbackAndReturnFlushedQueue M")]
        public static void InvokeCallbackAndReturnFlushedQueue_Return()
        {
            JSCoreJavaScriptExecutor executor = new JSCoreJavaScriptExecutor();

            Assert.That(
                () => executor.InvokeCallbackAndReturnFlushedQueue(1,new JArray()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("null")
            );
        }

        [Test]
        [Description("TC JSCoreJavaScriptExecutor SetGlobalVariable")]
        [Property("SPEC", "ReactNative.JavaScriptCore.Executor.JSCoreJavaScriptExecutor.SetGlobalVariable M")]
        public static void SetGlobalVariable_Return()
        {
            JSCoreJavaScriptExecutor executor = new JSCoreJavaScriptExecutor();

            Assert.That(
                () => executor.SetGlobalVariable("1", new JArray()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("null")
            );
        }

    }


}