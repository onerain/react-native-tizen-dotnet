using Newtonsoft.Json.Linq;
using ReactNative.Bridge;
using ReactNative.Common;
using ReactNative.Modules.Core;
using ReactNative.Modules.DevSupport;
using ReactNative.Tracing;
using System;
using System.IO;
using System.Reactive.Disposables;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using PCLStorage;
using System.Reflection;
using System.Windows;

using Tizen;
using Tizen.Applications;

namespace ReactNative.DevSupport
{
    class DevSupportManager : IDevSupportManager, IDisposable
    {
        private const int NativeErrorCookie = -1;
        private const string JSBundleFileName = "ReactNativeDevBundle.js";
        private readonly SerialDisposable _pollingDisposable = new SerialDisposable();

        private readonly IReactInstanceDevCommandsHandler _reactInstanceCommandsHandler;
        private readonly string _jsBundleFile;
        private readonly string _jsAppBundleName;
        private readonly DevInternalSettings _devSettings;
        private readonly DevServerHelper _devServerHelper;

        private bool _isDevSupportEnabled = true;

        private ReactContext _currentContext;
        private RedBoxDialog _redBoxDialog;
        private Action _dismissRedBoxDialog;
        private bool _redBoxDialogOpen;
        private DevOptionDialog _devOptionsDialog;
        private Action _dismissDevOptionsDialog;
        private bool _devOptionsDialogOpen;

        private DevServerHostDialog _devServerHostDialog;

        public DevSupportManager(
            IReactInstanceDevCommandsHandler reactInstanceCommandsHandler,
            string jsBundleFile,
            string jsAppBundleName)
        {
            _reactInstanceCommandsHandler = reactInstanceCommandsHandler;
            _jsBundleFile = jsBundleFile;
            _jsAppBundleName = jsAppBundleName;
            _devSettings = new DevInternalSettings(this);
            _devServerHelper = new DevServerHelper(_devSettings);
            ReloadSettings();
        }

        public IDeveloperSettings DevSettings
        {
            get
            {
                return _devSettings;
            }
        }

        public string DownloadedJavaScriptBundleFile
        {
            get
            {
                return JSBundleFileName;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isDevSupportEnabled;
            }
            set
            {
                if (value != _isDevSupportEnabled)
                {
                    _isDevSupportEnabled = value;
                    ReloadSettings();
                }
            }
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
                if (_jsAppBundleName == null)
                {
                    return "";
                }

                return _devServerHelper.GetSourceMapUrl(_jsAppBundleName);
            }
        }

        public string SourceUrl
        {
            get
            {
                if (_jsAppBundleName == null)
                {
                    return "";
                }

                return _devServerHelper.GetSourceUrl(_jsAppBundleName);
            }
        }

        public string JavaScriptBundleUrlForRemoteDebugging
        {
            get
            {
                return _devServerHelper.GetJavaScriptBundleUrlForRemoteDebugging(_jsAppBundleName);
            }
        }

        public void HandleException(Exception exception)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif

            if (IsEnabled)
            {
                ShowNewNativeError(exception.Message + " ==> " +exception.InnerException?.Message, exception);
            }
            else
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }
        }

        public async Task<bool> HasUpToDateBundleInCacheAsync()
        {
            Log.Info(ReactConstants.Tag, "## DevSupportManager ## HasUpToDateBundleInCacheAsync");
            if (_isDevSupportEnabled)
            {
                var curApp = Application.Current;
                var exePath = curApp.ApplicationInfo.ExecutablePath;
                var lastUpdateTime = File.GetCreationTime(exePath);
                var localFolder = FileSystem.Current.LocalStorage;
                if (await localFolder.CheckExistsAsync(JSBundleFileName) == ExistenceCheckResult.FileExists)
                {
                    var ret = File.GetLastWriteTime(JSBundleFileName) > lastUpdateTime;
                    return ret;
                }
            }
            return false;
        }

        public void ShowNewNativeError(string message, Exception exception)
        {
            var javaScriptException = exception as JavaScriptException;
            if (javaScriptException != null && javaScriptException.JavaScriptStackTrace != null)
            {
                var stackTrace = StackTraceHelper.ConvertJSCoreStackTrace(javaScriptException.JavaScriptStackTrace);
                ShowNewError(exception.Message, stackTrace, NativeErrorCookie);
            }
            else
            {
                RNTracer.Error(ReactConstants.Tag, "Exception in native call from JavaScript.", exception);
                ShowNewError(message, StackTraceHelper.ConvertNativeStackTrace(exception), NativeErrorCookie);
            }
        }

        public void ShowNewJavaScriptError(string title, JArray details, int errorCookie)
        {
            ShowNewError(title, StackTraceHelper.ConvertJavaScriptStackTrace(details), errorCookie);
        }

        public void UpdateJavaScriptError(string message, JArray details, int errorCookie)
        {

            DispatcherHelpers.RunOnDispatcher(() =>
            {
                if (_redBoxDialog == null
                    || !_redBoxDialogOpen
                    || errorCookie != _redBoxDialog.ErrorCookie)
                {
                    return;
                }

                _redBoxDialog.Message = message;
                _redBoxDialog.StackTrace = StackTraceHelper.ConvertJavaScriptStackTrace(details);
            });

            RNTracer.Error(ReactConstants.Tag, "[RN_EXCEPTION] DevSupportManager::UpdateJavaScriptError:[" + message + "]");
        }

        public void HideRedboxDialog()
        {
            var dismissRedBoxDialog = _dismissRedBoxDialog;
            if (_redBoxDialogOpen && dismissRedBoxDialog != null)
            {
                dismissRedBoxDialog();
            }
        }

        public void ShowDevOptionsDialog()
        {
            if (_devOptionsDialog != null || !IsEnabled)
            {
                return;
            }
            Log.Info(ReactConstants.Tag, "Before build DevOptionHandler");
            DispatcherHelpers.RunOnDispatcher(() =>
            {
                var options = new[]
                {
                    new DevOptionHandler(
                        "Reload JavaScript",
                        HandleReloadJavaScript),
                    new DevOptionHandler(
                        IsRemoteDebuggingEnabled
                            ? "Stop JS Remote Debugging"
                            : "Start JS Remote Debugging",
                        () =>
                        {
                            IsRemoteDebuggingEnabled = !IsRemoteDebuggingEnabled;
                            HandleReloadJavaScript();
                        }),
                    new DevOptionHandler(
                        _devSettings.IsHotModuleReplacementEnabled
                            ? "Disable Hot Reloading"
                            : "Enable Hot Reloading",
                        () =>
                        {
                            _devSettings.IsHotModuleReplacementEnabled = !_devSettings.IsHotModuleReplacementEnabled;
                            HandleReloadJavaScript();
                        }),
                    new DevOptionHandler(
                        _devSettings.IsReloadOnJavaScriptChangeEnabled
                            ? "Disable Live Reload"
                            : "Enable Live Reload",
                        () =>
                            _devSettings.IsReloadOnJavaScriptChangeEnabled =
                                !_devSettings.IsReloadOnJavaScriptChangeEnabled),
                    new DevOptionHandler(
                        _devSettings.IsElementInspectorEnabled
                            ? "Hide Inspector"
                            : "Show Inspector",
                        () =>
                        {
                            _devSettings.IsElementInspectorEnabled = !_devSettings.IsElementInspectorEnabled;
                            _reactInstanceCommandsHandler.ToggleElementInspector();
                        }),
                   new DevOptionHandler(
                        "Set Host IP Address",
                        () =>
                        {
                            _devSettings.DebugServerHost = "109.123.120.200:8084";
                            _devServerHostDialog = DevServerHostDialog.GetInstance();
                            HideDevOptionsDialog();
                            _devServerHostDialog.Show();
                            _devServerHostDialog.ResetCloseEvent();
                            _devServerHostDialog.Closed += (dialog, __) =>
                            {

                                var ipText = ((DevServerHostDialog)dialog).text;
                                if (ipText.IndexOf(':') == -1)
                                {
                                    ipText += ":8081";
                                }
                                _devSettings.DebugServerHost = ipText;
                                Log.Info(ReactConstants.Tag, "IP is " + ipText);
                            };

                        }),
                };

                _devOptionsDialogOpen = true;
                _devOptionsDialog = DevOptionDialog.GetInstance();
                _devOptionsDialog.ResetCloseEvent();
                _devOptionsDialog.Closed += (_, __) =>
                {
                    _devOptionsDialogOpen = false;
                    _dismissDevOptionsDialog = null;
                    _devOptionsDialog = null;
                };

                foreach (var option in options)
                {
                    _devOptionsDialog.Add(option);
                }

                if (_redBoxDialog != null)
                {
                    _dismissRedBoxDialog();
                }


                if (Application.Current != null && ReactProgram.RctWindow != null)
                {
                    _devOptionsDialog.Owner = ReactProgram.RctWindow;
                }

                _dismissDevOptionsDialog = _devOptionsDialog.Close;
                _devOptionsDialog.Show();

                foreach (var option in options)
                {
                    option.HideDialog = _dismissDevOptionsDialog;
                }
            });
        }

        private void HideDevOptionsDialog()
        {
            var dismissDevOptionsDialog = _dismissDevOptionsDialog;
            if (_devOptionsDialogOpen && dismissDevOptionsDialog != null)
            {
                dismissDevOptionsDialog();
            }
        }

        public void OnNewReactContextCreated(ReactContext context)
        {
            ResetCurrentContext(context);
        }

        public void OnReactContextDestroyed(ReactContext context)
        {
            if (context == _currentContext)
            {
                ResetCurrentContext(null);
            }
        }

        public Task<bool> IsPackagerRunningAsync()
        {
            return _devServerHelper.IsPackagerRunningAsync();
        }

        public async void HandleReloadJavaScript()
        {
            DispatcherHelpers.AssertOnDispatcher();

            HideRedboxDialog();
            HideDevOptionsDialog();

            var message = !IsRemoteDebuggingEnabled
                ? "Fetching JavaScript bundle."
                : "Connecting to remote debugger.";

            /*
            var progressDialog = new ProgressDialog("Please wait...", message);

            if (Application.Current != null && Application.Current.MainWindow != null && Application.Current.MainWindow.IsLoaded)
            {
                progressDialog.Owner = Application.Current.MainWindow;
            }
            else
            {
                progressDialog.Topmost = true;
                progressDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            
            Action cancel = progressDialog.Close;
            progressDialog.Show();
			*/

            if (IsRemoteDebuggingEnabled)
            {
                Log.Info(ReactConstants.Tag, "ReloadJavaScriptInProxyMode");
                await ReloadJavaScriptInProxyMode(()=>{}, CancellationToken.None).ConfigureAwait(false);
            }
            else if (_jsBundleFile == null)
            {
                Log.Info(ReactConstants.Tag, "ReloadJavaScriptFromServerAsync");
                await ReloadJavaScriptFromServerAsync(()=>{}, CancellationToken.None).ConfigureAwait(false);
            }
            else
            {
                Log.Info(ReactConstants.Tag, "ReloadJavaScriptFromFileAsync");
                await ReloadJavaScriptFromFileAsync(CancellationToken.None);
                //TODO 
                //cancel();
            }
        }

        public void ReloadSettings()
        {
            if (_isDevSupportEnabled)
            {
                if (_devSettings.IsReloadOnJavaScriptChangeEnabled)
                {
                    _pollingDisposable.Disposable =
                    _devServerHelper.StartPollingOnChangeEndpoint(HandleReloadJavaScript);
                }
                else
                {
                    // Disposes any existing poller
                    _pollingDisposable.Disposable = Disposable.Empty;
                }
            }
            else
            {

                if (_redBoxDialog != null)
                {
                    _dismissRedBoxDialog();
                }

                _pollingDisposable.Disposable = Disposable.Empty;
            }
        }

        public void Dispose()
        {
            _pollingDisposable.Dispose();
            _devServerHelper.Dispose();
        }

        private void ResetCurrentContext(ReactContext context)
        {
            if (_currentContext == context)
            {
                return;
            }

            _currentContext = context;

            if (_devSettings.IsHotModuleReplacementEnabled && context != null)
            {
                try
                {
                    Log.Info(ReactConstants.Tag, "## DevSupportManager ## SourceUrl:["+ SourceUrl.ToString()+"]");
	                var uri = new Uri(SourceUrl);
	                var path = uri.LocalPath.Substring(1); // strip initial slash in path
	                var host = uri.Host;
	                var port = uri.Port;
	                context.GetJavaScriptModule<HMRClient>().enable("tizen", path, host, port);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

		//TODO:  Use log to instead temporarily
        private void ShowNewError(string message, IStackFrame[] stack, int errorCookie)
        {
			RNTracer.Error(ReactConstants.Tag, "[RN_EXCEPTION] DevSupportManager::ShowNewError:[" + message + "]");

            DispatcherHelpers.RunOnDispatcher(() =>
            {
                if (_redBoxDialog == null)
                {
                    _redBoxDialog = new RedBoxDialog(HandleReloadJavaScript);
                }
                if (_redBoxDialogOpen)
                {
                    return;
                }
                _redBoxDialogOpen = true;
                _redBoxDialog.ErrorCookie = errorCookie;
                _redBoxDialog.Message = message;
                _redBoxDialog.StackTrace = stack;
                _redBoxDialog.Closed += (_, __) =>
                {
                    _redBoxDialogOpen = false;
                    _dismissRedBoxDialog = null;
                    _redBoxDialog = null;
                };


                //if (Application.Current != null && Application.Current.MainWindow != null && Application.Current.MainWindow.IsLoaded)
                //{
                //    _redBoxDialog.Owner = Application.Current.MainWindow;
                //}
                //else
                //{
                //    _redBoxDialog.Topmost = true;
                //    _redBoxDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //}
                _dismissRedBoxDialog = _redBoxDialog.Close;
                _redBoxDialog.Show();
            });

        }

        private async Task ReloadJavaScriptInProxyMode(Action dismissProgress, CancellationToken token)
        {
            try
            {
                await _devServerHelper.LaunchDevToolsAsync(token).ConfigureAwait(true);
                var factory = new Func<IJavaScriptExecutor>(() =>
                {
                    var executor = new WebSocketJavaScriptExecutor();
                    executor.ConnectAsync(_devServerHelper.WebsocketProxyUrl, token).Wait();
                    return executor;
                });

                _reactInstanceCommandsHandler.OnReloadWithJavaScriptDebugger(factory);
                dismissProgress();
            }
            catch (DebugServerException ex)
            {
                dismissProgress();
                ShowNewNativeError(ex.Message, ex);
            }
            catch (Exception ex)
            {
                dismissProgress();
                ShowNewNativeError(
                    "Unable to download JS bundle. Did you forget to " +
                    "start the development server or connect your device?",
                    ex);
            }
        }

        private async Task ReloadJavaScriptFromServerAsync(Action dismissProgress, CancellationToken token)
        {
            var moved = false;

            var temporaryFilePath = Path.GetTempPath() + JSBundleFileName;
            try
            {
                using (var stream = new FileStream(temporaryFilePath, FileMode.Create))
                {
                    await _devServerHelper.DownloadBundleFromUrlAsync(_jsAppBundleName, stream, token);
                }

                var temporaryFile = await FileSystem.Current.GetFileFromPathAsync(temporaryFilePath, token);
                var localStorage = FileSystem.Current.LocalStorage;
                string newPath = PortablePath.Combine(localStorage.Path, JSBundleFileName);

                await temporaryFile.MoveAsync(newPath, NameCollisionOption.ReplaceExisting, token);
                moved = true;

                dismissProgress();
                _reactInstanceCommandsHandler.OnJavaScriptBundleLoadedFromServer();
            }
            catch (DebugServerException ex)
            {
                dismissProgress();
                ShowNewNativeError(ex.Message, ex);
            }
            catch (Exception ex)
            {
                dismissProgress();
                ShowNewNativeError(
                    "Unable to download JS bundle. Did you forget to " +
                    "start the development server or connect your device?",
                    ex);
            }
            finally
            {
                if (!moved)
                {
                    var temporaryFile = await FileSystem.Current.GetFileFromPathAsync(temporaryFilePath, token).ConfigureAwait(false);

                    if (temporaryFile != null)
                    {
                        await temporaryFile.DeleteAsync(token).ConfigureAwait(false);
                    }
                }
            }

        }

        private Task ReloadJavaScriptFromFileAsync(CancellationToken token)
        {
            _reactInstanceCommandsHandler.OnBundleFileReloadRequest();
            return Task.CompletedTask;
        }
    }
}
