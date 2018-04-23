using Newtonsoft.Json.Linq;
using ReactNative.Bridge;
using ReactNative.Common;
using ReactNative.Tracing;
using ReactNative.Modules.DevSupport;
using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using ReactNativeTizen.ElmSharp.Extension;

namespace ReactNative.DevSupport
{
    class DisabledDevSupportManager : IDevSupportManager
    {
        public IDeveloperSettings DevSettings
        {
            get
            {
                return null;
            }
        }

        public string DownloadedJavaScriptBundleFile
        {
            get
            {
                return null;
            }
        }

        public bool IsEnabled
        {
            get;
            set;
        }

        public bool IsRemoteDebuggingEnabled
        {
            get;
            set;
        }

        public string SourceMapUrl
        {
            get
            {
                return null;
            }
        }

        public string SourceUrl
        {
            get
            {
                return null;
            }
        }

        public string JavaScriptBundleUrlForRemoteDebugging
        {
            get
            {
                return null;
            }
        }

		//TODO: Use log to instead temporarily, 
		//need to decide whether to show these failures on error window
        public void HandleException(Exception exception)
        {
            //await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            //{
            //  ExceptionDispatchInfo.Capture(exception).Throw();
            //}, DispatcherPriority.Send).Task.ConfigureAwait(false);
			RNTracer.Error(ReactConstants.Tag, "[RN_EXCEPTION] DisabledDevSupportManager::HandleException:[" + exception.ToString() + "]");
            EcoreLoopProxy.Quit();
        }

        public void HandleReloadJavaScript()
        {
        }

        public Task<bool> HasUpToDateBundleInCacheAsync()
        {
            return Task.FromResult(false);
        }

        public void HideRedboxDialog()
        {
        }

        public Task<bool> IsPackagerRunningAsync()
        {
            return Task.FromResult(false);
        }

        public void OnNewReactContextCreated(ReactContext context)
        {
        }

        public void OnReactContextDestroyed(ReactContext context)
        {
        }

        public void ReloadSettings()
        {
        }

        public void ShowDevOptionsDialog()
        {
        }

        public void ShowNewJavaScriptError(string title, JArray details, int exceptionId)
        {
        }

        public void ShowNewNativeError(string message, Exception ex)
        {
        }

        public void UpdateJavaScriptError(string title, JArray details, int exceptionId)
        {
        }
    }
}
