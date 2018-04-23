using ReactNative.Bridge;
using ReactNative.Bridge.Queue;
using ReactNative.JavaScriptCore.Executor;
using ReactNative.Common;
using ReactNative.DevSupport;
using ReactNative.Modules.Core;
//using ReactNative.Touch;
using ReactNative.Tracing;
using ReactNative.UIManager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.FormattableString;
using Newtonsoft.Json.Linq;
using System.Threading;

using ReactNativeTizen.ElmSharp.Extension;
using Tizen;

using ReactNative.Modules.MediaPlayer; // for test

namespace ReactNative
{
    /// <summary>
    /// This interface manages instances of <see cref="IReactInstance" />.
    /// It exposes a way to configure React instances using
    /// <see cref="IReactPackage"/> and keeps track of the lifecycle of that
    /// instance. It also sets up a connection between the instance and the
    /// developer support functionality of the framework.
    ///
    /// An instance of this manager is required to start the JavaScript
    /// application in <see cref="ReactRootView"/>
    /// (<see cref="ReactRootView.StartReactApplication(IReactInstanceManager, string)"/>).
    ///
    /// The lifecycle of the instance of <see cref="IReactInstanceManager"/>
    /// should be bound to the application that owns the
    /// <see cref="ReactRootView"/> that is used to render the React
    /// application using this instance manager. It is required to pass
    /// lifecycle events to the instance manager (i.e., <see cref="OnSuspend"/>,
    /// <see cref="IAsyncDisposable.DisposeAsync"/>, and <see cref="OnResume(Action)"/>).
    /// </summary>
    /// 

    public class ReactInstanceManager : IReactInstanceManager
    {
        private readonly List<ReactRootView> _attachedRootViews = new List<ReactRootView>();

        private readonly string _jsBundleFile;
        private readonly string _jsMainModuleName;
        private readonly IReadOnlyList<IReactPackage> _packages;
        private readonly IDevSupportManager _devSupportManager;
        private readonly bool _useDeveloperSupport;
        private readonly UIImplementationProvider _uiImplementationProvider;
        private readonly Action<Exception> _nativeModuleCallExceptionHandler;

        private LifecycleState _lifecycleState;
        private bool _hasStartedCreatingInitialContext;
        private Task _contextInitializationTask;
        private Func<IJavaScriptExecutor> _pendingJsExecutorFactory;
        private JavaScriptBundleLoader _pendingJsBundleLoader;
        private string _sourceUrl;
        private ReactContext _currentReactContext;
        private Action _defaultBackButtonHandler;

        /// <summary>
        /// Event triggered when a React context has been initialized.
        /// </summary>
        public event EventHandler<ReactContextInitializedEventArgs> ReactContextInitialized;

        private ReactInstanceManager(
            string jsBundleFile,
            string jsMainModuleName,
            IReadOnlyList<IReactPackage> packages,
            bool useDeveloperSupport,
            LifecycleState initialLifecycleState,
            UIImplementationProvider uiImplementationProvider,
            Action<Exception> nativeModuleCallExceptionHandler)
        {
            if (packages == null)
                throw new ArgumentNullException(nameof(packages));

            if (uiImplementationProvider == null)
                throw new ArgumentNullException(nameof(uiImplementationProvider));

            _jsBundleFile = jsBundleFile;
            _jsMainModuleName = jsMainModuleName;
            _packages = packages;

            _useDeveloperSupport = useDeveloperSupport;
            _devSupportManager = _useDeveloperSupport
                ? (IDevSupportManager)new DevSupportManager(
                    new ReactInstanceDevCommandsHandler(this),
                    _jsBundleFile,
                    _jsMainModuleName)
                : new DisabledDevSupportManager();

            _lifecycleState = initialLifecycleState;
            _uiImplementationProvider = uiImplementationProvider;
            _nativeModuleCallExceptionHandler = nativeModuleCallExceptionHandler;
        }

        /// <summary>
        /// The developer support manager for the instance.
        /// </summary>
        public IDevSupportManager DevSupportManager
        {
            get
            {
                return _devSupportManager;
            }
        }

        /// <summary>
        /// Signals whether <see cref="CreateReactContextInBackground"/> has
        /// been called. Will return <code>false</code> after 
        /// <see cref="IAsyncDisposable.DisposeAsync"/>  until a new initial
        /// context has been created.
        /// </summary>
        public bool HasStartedCreatingInitialContext
        {
            get
            {
                return _hasStartedCreatingInitialContext;
            }
        }

        /// <summary>
        /// The URL where the last bundle was loaded from.
        /// </summary>
        public string SourceUrl
        {
            get
            {
                return _sourceUrl;
            }
        }

        /// <summary>
        /// Gets the current React context instance.
        /// </summary>
        public ReactContext CurrentReactContext
        {
            get
            {
                return _currentReactContext;
            }
        }

        /// <summary>
        /// Trigger the React context initialization asynchronously in a
        /// background task. This enables applications to pre-load the
        /// application JavaScript, and execute global core code before the
        /// <see cref="ReactRootView"/> is available and measure. This should
        /// only be called the first time the application is set up, which is
        /// enforced to keep developers from accidentally creating their
        /// applications multiple times.
        /// </summary>
        public async void CreateReactContextInBackground()
        {
            await CreateReactContextInBackgroundAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Trigger the React context initialization asynchronously in a
        /// background task. This enables applications to pre-load the
        /// application JavaScript, and execute global core code before the
        /// <see cref="ReactRootView"/> is available and measure. This should
        /// only be called the first time the application is set up, which is
        /// enforced to keep developers from accidentally creating their
        /// applications multiple times.
        /// </summary>
        /// <returns>A task to await the result.</returns>
        internal async Task CreateReactContextInBackgroundAsync()
        {
            if (_hasStartedCreatingInitialContext)
            {
                throw new InvalidOperationException(
                    "React context creation should only be called when creating the React " +
                    "application for the first time. When reloading JavaScript, e.g., from " +
                    "a new file, explicitly, use the re-create method.");
            }

            _hasStartedCreatingInitialContext = true;
            await RecreateReactContextInBackgroundInnerAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Recreate the React application and context. This should be called
        /// if configuration has changed or the developer has requested the
        /// application to be reloaded.
        /// </summary>
        public async void RecreateReactContextInBackground()
        {
            await RecreateReactContextInBackgroundAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Recreate the React application and context. This should be called
        /// if configuration has changed or the developer has requested the
        /// application to be reloaded.
        /// </summary>
        /// <returns>A task to await the result.</returns>
        internal async Task RecreateReactContextInBackgroundAsync()
        {
            if (!_hasStartedCreatingInitialContext)
            {
                throw new InvalidOperationException(
                    "React context re-creation should only be called after the initial " +
                    "create context background call.");
            }

            await RecreateReactContextInBackgroundInnerAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Method that gives JavaScript the opportunity to consume the back
        /// button event. If JavaScript does not consume the event, the
        /// default back press action will be invoked at the end of the
        /// round trip to JavaScript.
        /// </summary>
        public void OnBackPressed()
        {
            DispatcherHelpers.AssertOnDispatcher();
            var reactContext = _currentReactContext;
            if (reactContext == null)
            {
                RNTracer.Write(ReactConstants.Tag, "Instance detached from instance manager.");
                InvokeDefaultOnBackPressed();
            }
            else
            {
                reactContext.GetNativeModule<DeviceEventManagerModule>().EmitHardwareBackPressed();
            }

            Log.Debug(ReactConstants.Tag, "## Inform JS of Back button being Pressed ##");
        }

        /// <summary>
        /// Called when the application is suspended.
        /// </summary>
        public void OnSuspend()
        {
            DispatcherHelpers.AssertOnDispatcher();

            _lifecycleState = LifecycleState.BeforeResume;
            _defaultBackButtonHandler = null;

            if (_useDeveloperSupport)
            {
                _devSupportManager.IsEnabled = false;
            }

            var currentReactContext = _currentReactContext;
            if (currentReactContext != null)
            {
                _currentReactContext.OnSuspend();
            }
        }

        /// <summary>
        /// Used when the application resumes to reset the back button handling
        /// in JavaScript.
        /// </summary>
        /// <param name="onBackPressed">
        /// The action to take when back is pressed.
        /// </param>
        public void OnResume(Action onBackPressed)
        {
            if (onBackPressed == null)
                throw new ArgumentNullException(nameof(onBackPressed));

            DispatcherHelpers.AssertOnDispatcher();

            _lifecycleState = LifecycleState.Resumed;

            _defaultBackButtonHandler = onBackPressed;

            if (_useDeveloperSupport)
            {
                _devSupportManager.IsEnabled = true;
            }

            var currentReactContext = _currentReactContext;
            if (currentReactContext != null)
            {
                currentReactContext.OnResume();
            }
        }

        /// <summary>
        /// Destroy the <see cref="IReactInstanceManager"/>.
        /// </summary>
        public async Task DisposeAsync()
        {
            DispatcherHelpers.AssertOnDispatcher();

            // TODO: memory pressure hooks
            if (_useDeveloperSupport)
            {
                _devSupportManager.IsEnabled = false;
            }

            var currentReactContext = _currentReactContext;
            if (currentReactContext != null)
            {
                await currentReactContext.DisposeAsync().ConfigureAwait(false);
                _currentReactContext = null;
                _hasStartedCreatingInitialContext = false;
            }
        }

        /// <summary>
        /// Attach given <paramref name="rootView"/> to a React instance
        /// manager and start the JavaScript application using the JavaScript
        /// module provided by the <see cref="ReactRootView.JavaScriptModuleName"/>. If
        /// the React context is currently being (re-)created, or if the react
        /// context has not been created yet, the JavaScript application
        /// associated with the provided root view will be started
        /// asynchronously. This view will then be tracked by this manager and
        /// in case of React instance restart, it will be re-attached.
        /// </summary>
        /// <param name="rootView">The root view.</param>
        public void AttachMeasuredRootView(ReactRootView rootView)
        {
            Log.Info(ReactConstants.Tag, "[ReactInstanceManager] Enter AttachMeasuredRootView ");
            if (rootView == null)
                throw new ArgumentNullException(nameof(rootView));

            DispatcherHelpers.AssertOnDispatcher();

            _attachedRootViews.Add(rootView);

            // If the React context is being created in the background, the
            // JavaScript application will be started automatically when
            // creation completes, as root view is part of the attached root
            // view list.
            var currentReactContext = _currentReactContext;

            if (_contextInitializationTask == null && currentReactContext != null)
            {
                AttachMeasuredRootViewToInstance(rootView, currentReactContext.ReactInstance);
            }
            Log.Info(ReactConstants.Tag, "[ReactInstanceManager] Exit AttachMeasuredRootView ");
        }

        /// <summary>
        /// Detach given <paramref name="rootView"/> from the current react
        /// instance. This method is idempotent and can be called multiple
        /// times on the same <see cref="ReactRootView"/> instance.
        /// </summary>
        /// <param name="rootView">The root view.</param>
        public void DetachRootView(ReactRootView rootView)
        {
            Log.Info(ReactConstants.Tag, "[ReactInstanceManager] Enter DetachRootView ");
            if (rootView == null)
                throw new ArgumentNullException(nameof(rootView));

            DispatcherHelpers.AssertOnDispatcher();

            if (_attachedRootViews.Remove(rootView))
            {
                var currentReactContext = _currentReactContext;
                if (currentReactContext != null && currentReactContext.HasActiveReactInstance)
                {
                    DetachViewFromInstance(rootView, currentReactContext.ReactInstance);
                }
            }

            // release 'RootView'
            Log.Info(ReactConstants.Tag, $"Time to release RootView, but its children count = {rootView.GetChildrenCount()} ThreadID={Thread.CurrentThread.ManagedThreadId}");

            // TODO: TO BE FIXXED
            // When 'Teminate', operation for 'remove' won't be execute, so, 
            // if rootView want to be unrealized, firstly, its children must
            // be removed, otherwise, it will coredump ...
            rootView.RemoveViewInfo();
            rootView.Unrealize();

            Log.Info(ReactConstants.Tag, "[ReactInstanceManager] Exit DetachRootView ");
        }

        /// <summary>
        /// Uses the configured <see cref="IReactPackage"/> instances to create
        /// all <see cref="IViewManager"/> instances.
        /// </summary>
        /// <param name="reactContext">
        /// The application context.
        /// </param>
        /// <returns>The list of view managers.</returns>
        public IReadOnlyList<IViewManager> CreateAllViewManagers(ReactContext reactContext)
        {
            if (reactContext == null)
                throw new ArgumentNullException(nameof(reactContext));

            using (RNTracer.Trace(RNTracer.TRACE_TAG_REACT_BRIDGE, "createAllViewManagers").Start())
            {
                var allViewManagers = new List<IViewManager>();
                foreach (var package in _packages)
                {
                    allViewManagers.AddRange(
                        package.CreateViewManagers(reactContext));
                }

                return allViewManagers;
            }
        }

        private async Task RecreateReactContextInBackgroundInnerAsync()
        {
            DispatcherHelpers.AssertOnDispatcher();

            if (_useDeveloperSupport && _jsBundleFile == null && _jsMainModuleName != null)
            {
                //if (await _devSupportManager.HasUpToDateBundleInCacheAsync()) //TODO: shoushou temp disable this because getcreatetime api error
                //{
                //    OnJavaScriptBundleLoadedFromServer();
                //}
                //else
                //{
                    _devSupportManager.HandleReloadJavaScript();
                //}
            }
            else
            {
                RecreateReactContextInBackgroundFromBundleFile();
            }
        }

        private void RecreateReactContextInBackgroundFromBundleFile()
        {
            RecreateReactContextInBackground(
                () => new JSCoreJavaScriptExecutor(),
                JavaScriptBundleLoader.CreateFileLoader(_jsBundleFile));
        }

        private void InvokeDefaultOnBackPressed()
        {
            DispatcherHelpers.AssertOnDispatcher();
            _defaultBackButtonHandler?.Invoke();
        }

        private void OnReloadWithJavaScriptDebugger(Func<IJavaScriptExecutor> javaScriptExecutorFactory)
        {
            RecreateReactContextInBackground(
                javaScriptExecutorFactory,
                JavaScriptBundleLoader.CreateRemoteDebuggerLoader(
                    _devSupportManager.JavaScriptBundleUrlForRemoteDebugging,
                    _devSupportManager.SourceUrl));
        }

        private void OnJavaScriptBundleLoadedFromServer()
        {
            RecreateReactContextInBackground(
                () => new JSCoreJavaScriptExecutor(),
                JavaScriptBundleLoader.CreateCachedBundleFromNetworkLoader(
                    _devSupportManager.SourceUrl,
                    _devSupportManager.DownloadedJavaScriptBundleFile));
        }

        private void RecreateReactContextInBackground(
            Func<IJavaScriptExecutor> jsExecutorFactory,
            JavaScriptBundleLoader jsBundleLoader)
        {
            if (_contextInitializationTask == null)
            {
                _contextInitializationTask = InitializeReactContextAsync(jsExecutorFactory, jsBundleLoader);
            }
            else
            {
                _pendingJsExecutorFactory = jsExecutorFactory;
                _pendingJsBundleLoader = jsBundleLoader;
            }
        }

        private async Task InitializeReactContextAsync(
            Func<IJavaScriptExecutor> jsExecutorFactory,
            JavaScriptBundleLoader jsBundleLoader)
        {
            Log.Info(ReactConstants.Tag, "## Enter InitializeReactContextAsync ##");
            var currentReactContext = _currentReactContext;
            if (currentReactContext != null)
            {
                await TearDownReactContextAsync(currentReactContext);
                _currentReactContext = null;
            }
            try
            {
                Log.Info(ReactConstants.Tag, "## CreateReactContextAsync(jsExecutorFactory, jsBundleLoader) ##");
                var reactContext = await CreateReactContextAsync(jsExecutorFactory, jsBundleLoader);

                using (RNTracer.Trace(RNTracer.TRACE_TAG_REACT_BRIDGE, "SetupReactContext").Start())
                {
                    SetupReactContext(reactContext);
                }
            }
            catch (Exception ex)
            {
                _devSupportManager.HandleException(ex);
            }
            finally
            {
                _contextInitializationTask = null;
            }

            if (_pendingJsExecutorFactory != null)
            {
                var pendingJsExecutorFactory = _pendingJsExecutorFactory;
                var pendingJsBundleLoader = _pendingJsBundleLoader;

                _pendingJsExecutorFactory = null;
                _pendingJsBundleLoader = null;

                RecreateReactContextInBackground(
                    pendingJsExecutorFactory,
                    pendingJsBundleLoader);
            }
        }

        private void SetupReactContext(ReactContext reactContext)
        {
            Log.Info(ReactConstants.Tag, "[ReactInstanceManager] Enter SetupReactContext ");
            DispatcherHelpers.AssertOnDispatcher();
            if (_currentReactContext != null)
            {
                throw new InvalidOperationException(
                    "React context has already been setup and has not been destroyed.");
            }

            _currentReactContext = reactContext;
            var reactInstance = reactContext.ReactInstance;
            _devSupportManager.OnNewReactContextCreated(reactContext);

            // TODO: set up memory pressure hooks
            MoveReactContextToCurrentLifecycleState(reactContext);

            foreach (var rootView in _attachedRootViews)
            {
                AttachMeasuredRootViewToInstance(rootView, reactInstance);
            }

            OnReactContextInitialized(reactContext);
            Log.Info(ReactConstants.Tag, "[ReactInstanceManager] Exit SetupReactContext ");
        }


        #region '[UT METHOD]'

        public static void UT_CreateView(object obj)
        {
            ReactInstanceManager RCTIntance = (ReactInstanceManager)obj;
            var currentReactContext = RCTIntance._currentReactContext;

            Log.Info(ReactConstants.Tag, " ## UT_CreateView-1 ## ThreadID=" + Thread.CurrentThread.ManagedThreadId.ToString()
    + ", RCTIntance=" + RCTIntance);

            if (currentReactContext != null)
            {
                UIManagerModule uiMgrModule = currentReactContext.ReactInstance.GetNativeModule<UIManagerModule>();
                Log.Info(ReactConstants.Tag, "### [BGN-1] Create a button view ### ");
                var prop1 = default(JObject);
                uiMgrModule.createView(3, "RCTButton", 1, prop1);
                uiMgrModule.OnBatchComplete();

                int[] szTag1 = { 3, };
                int[] szIndices1 = { 0, };
                uiMgrModule.manageChildren(1, null, null, szTag1, szIndices1, null);
                uiMgrModule.OnBatchComplete();
                Log.Info(ReactConstants.Tag, "### [END-1] Create a button view ### ");

            }
        }

        public static void UT_DropView(object obj)
        {
            for (int i = 1; i <= 15; i++)
            {
                Log.Info(ReactConstants.Tag, "## UT_DropView ## thread is Waiting>>>>>>>>>>>>>>>[" + i + " sec]");
                Thread.Sleep(1000 * 1);
            }

            ReactInstanceManager RCTIntance = (ReactInstanceManager)obj;
            var currentReactContext = RCTIntance._currentReactContext;

            Log.Info(ReactConstants.Tag, " ## UT_DropView-2 ## ThreadID=" + Thread.CurrentThread.ManagedThreadId.ToString()
                + ", RCTIntance="+ RCTIntance);

            if (currentReactContext != null)
            {
                UIManagerModule uiMgrModule = currentReactContext.ReactInstance.GetNativeModule<UIManagerModule>();

                Log.Info(ReactConstants.Tag, "### [BGN-2] Destroy a button view ### ");
                int[] szTagDel = { 0 };
                int[] szIndicesDel = { 0, };
                uiMgrModule.manageChildren(1, null, null, null, null, szTagDel);
                uiMgrModule.OnBatchComplete();
                Log.Info(ReactConstants.Tag, "### [END-2] Destroy a button view ### ");
            }

        }

        /// <summary>
        /// ## UIManagerModule TEST-STUB ##
        /// </summary>
        //public static void UT_UIManagerModuleTest(object obj)
        public void UT_UIManagerModuleTest(object obj)
        {
            Log.Info(ReactConstants.Tag, "<<<<<<<<<<<<<<<<<<<< UT_UIManagerModuleTest in thread ");

            //ReactInstanceManager RCTIntance = (ReactInstanceManager)obj;
            //var currentReactContext = RCTIntance._currentReactContext;

            var currentReactContext = _currentReactContext;

            Log.Info(ReactConstants.Tag, "### UT_UIManagerModuleTest ### " + (currentReactContext != null));

            if (currentReactContext != null)
            {
                Log.Info(ReactConstants.Tag, "### [BGN] UT_UIManagerModuleTest ### ");

                UIManagerModule uiMgrModule = currentReactContext.ReactInstance.GetNativeModule<UIManagerModule>();
                //
                /*
                //Log.Info(ReactConstants.Tag, ">>>>>>>>>>>>>>> [UT]  Network BGN <<<<<<<<<<<<<<<<< ");
                NetworkingModule module = currentReactContext.ReactInstance.GetNativeModule<NetworkingModule>();

                var method = "GET";

                var passed = false;
                var waitHandle = new AutoResetEvent(false);

                module.sendRequest(method, new Uri("www.baidu.com"), 1, null, null, "", false, 1000);
                waitHandle.WaitOne();
                */
                //Assert.IsTrue(passed);
                //Log.Info(ReactConstants.Tag, ">>>>>>>>>>>>>>> [UT]  Network BGN <<<<<<<<<<<<<<<<< ");
                //

                //2. TEXT
                //Log.Info(ReactConstants.Tag, "### [UT] Create a Text view ### ");
                //var prop = default(JObject);
                //uiMgrModule.createView(6, "RCTText", 1, prop);
                //uiMgrModule.OnBatchComplete();

                //int[] szTag = { 6, };
                //int[] szIndices = { 0, };
                //uiMgrModule.manageChildren(1, null, null, szTag, szIndices, null);
                //int[] szChildren = { 6, };
                //uiMgrModule.setChildren(1, szChildren);
                //uiMgrModule.OnBatchComplete();

                // grid
                //Log.Info(ReactConstants.Tag, "### [UT] Create a grid view ### ");
                //uiMgrModule.createView(2, "RCTGridView", 1, prop);

                /*
                //2. button
                Log.Info(ReactConstants.Tag, "### [BGN] Create a button view ### ");
                var prop1 = default(JObject);
                uiMgrModule.createView(3, "RCTButton", 1, prop1);
                uiMgrModule.OnBatchComplete();

                int[] szTag1 = { 3, };
                int[] szIndices1 = { 0, };
                uiMgrModule.manageChildren(1, null, null, szTag1, szIndices1, null);
                uiMgrModule.OnBatchComplete();
                Log.Info(ReactConstants.Tag, "### [END] Create a button view ### ");

                for (int i = 1; i <= 10; i++)
                {
                    Log.Info(ReactConstants.Tag, "Count >>>> "+i+" sec");
                    Thread.Sleep(1000*1);
                }

                Log.Info(ReactConstants.Tag, "### [BGN] Destroy a button view ### ");
                int[] szTagDel = { 0 };
                int[] szIndicesDel = { 0, };
                uiMgrModule.manageChildren(1, null, null, null, null, szTagDel);
                uiMgrModule.OnBatchComplete();
                Log.Info(ReactConstants.Tag, "### [END] Destroy a button view ### ");
                */

                // scroll
                Log.Info(ReactConstants.Tag, "### [UT] Create a RCTScrollView ### ");
                var propScroll = default(JObject);
                int[] szScrollViewTag = { 10, };
                int[] szScrollViewIndices = { 0, };
                uiMgrModule.createView(10, "RCTScrollView", 1, propScroll);
                uiMgrModule.manageChildren(1, null, null, szScrollViewTag, szScrollViewIndices, null);

                // View
                var propView = default(JObject);
                uiMgrModule.createView(11, "RCTView", 1, propView);
                int[] szViewTag = { 11, };
                int[] szViewIndices = { 0, };
                uiMgrModule.manageChildren(10, null, null, szViewTag, szViewIndices, null);

                // image
                Log.Info(ReactConstants.Tag, "### [UT] Create a RCTImageView ### ");
                var propImage = default(JObject);
                uiMgrModule.createView(8, "RCTImageView", 1, propImage);
                int[] szImageTag = { 8, };
                int[] szImageIndices = { 0, };
                uiMgrModule.manageChildren(11, null, null, szImageTag, szImageIndices, null);

                uiMgrModule.OnBatchComplete();

                //int[] childrenTag = { 8, };
                //uiMgrModule.setChildren(1, childrenTag);
                //uiMgrModule.OnBatchComplete();

                //int[] szTag1 = { 8,};
                //int[] szIndices1 = { 0,};

                //uiMgrModule.manageChildren(1, null, null, szTag1, szIndices1, null);
                //Log.Info(ReactConstants.Tag, "### [END] Manage RCTImageView ### ");
                //uiMgrModule.OnBatchComplete();

                /*                
                //2. Scroll
                Log.Info(ReactConstants.Tag, "### [UT] Create a Scroll view ### ");
                //var prop = default(JObject);
                uiMgrModule.createView(6, "RCTScrollView", 1, prop);

                //int[] szTag = { 21, };
                //int[] szIndices = { 0, };
                //uiMgrModule.manageChildren(1, null, null, szTag, szIndices, null);

                // Layout
                Log.Info(ReactConstants.Tag, "### [UT] Create a Layout view ### ");
                uiMgrModule.createView(10, "RCTView", 1, prop);

                int[] szTag1 = { 6, };
                int[] szIndices1 = { 0, };
                uiMgrModule.manageChildren(1, null, null, szTag1, szIndices1, null);

                //Log.Info(ReactConstants.Tag, "### [UT] Create Image view ### ");
                uiMgrModule.createView(11, "RCTImageView", 1, prop);
                uiMgrModule.createView(12, "RCTImageView", 1, prop);
                int[] szTag = { 11, 12};
                int[] szIndices = { 0, 1 };
                uiMgrModule.manageChildren(6, null, null, szTag, szIndices, null);
                */

                //Log.Info(ReactConstants.Tag, "### [UT] Create MyText view ### ");
                //uiMgrModule.createView(12, "RCTMyText", 1, prop);
                //uiMgrModule.createView(12, "RCTImageView", 1, prop);
                //int[] szTag1 = { 12, };
                //int[] szIndices1 = { 0, };
                //uiMgrModule.manageChildren(1, null, null, szTag1, szIndices1, null);

                // grid
                //Log.Info(ReactConstants.Tag, "### [UT] Create a grid view ### ");
                //uiMgrModule.createView(13, "RCTGridView", 1, prop);

                //int[] szTag5 = { 13, };
                //int[] szIndices5 = { 0, };
                //uiMgrModule.manageChildren(1, null, null, szTag5, szIndices5, null);

                //3. Image
                ////Log.Info(ReactConstants.Tag, "### [UT] Create two Image view ### ");
                //uiMgrModule.createView(23, "RCTImageView", 1, prop);
                //uiMgrModule.createView(24, "RCTImageView", 1, prop);

                // button
                //Log.Info(ReactConstants.Tag, "### [UT] Create a button view ### ");
                //uiMgrModule.createView(3, "RCTButton", 1, prop);


                //int[] szTag2 = { 23, 24 };
                //int[] szIndices2 = { 0, 1};
                //uiMgrModule.manageChildren(22, null, null, szTag2, szIndices2, null);

                // wait for 5 sec
                // Thread.Sleep(5000);

                // test for dispatchCommand of view
                //Log.Info(ReactConstants.Tag, "### [UT] Generate a command to the given view ### ");
                //var args = new JArray("Focus");
                //uiMgrModule.dispatchViewManagerCommand(3, 1, args);
                //uiMgrModule.dispatchViewManagerCommand(3, 2, args);


                Log.Info(ReactConstants.Tag, "### [END] UT_UIManagerModuleTest ### ");
            }
            // End UT
			
        }
        #endregion

        private void AttachMeasuredRootViewToInstance(
            ReactRootView rootView,
            IReactInstance reactInstance)
        {
            Log.Info(ReactConstants.Tag, "[ReactInstanceManager] ## Enter AttachMeasuredRootViewToInstance ##");

            DispatcherHelpers.AssertOnDispatcher();

            // Reset view content as it's going to be populated by the
            // application content from JavaScript

            //rootView.TouchHandler?.Dispose();
            //rootView.RemoveAllView();
            rootView.SetTag(0);

            var uiManagerModule = reactInstance.GetNativeModule<UIManagerModule>();
            var rootTag = uiManagerModule.AddMeasuredRootView(rootView);
            //rootView.TouchHandler = new TouchHandler(rootView);

            // TODO: commented temporarily 
            var jsAppModuleName = rootView.JavaScriptModuleName;
            var appParameters = new Dictionary<string, object>
            {
                { "rootTag", rootTag },
                { "initalProps", null /* TODO: add launch options to root view */ }
            };

            /* Entry of RN.js world */
            Log.Info(ReactConstants.Tag, "[ReactInstanceManager] Entry of RN.js world:[" + jsAppModuleName.ToString() +"]");
            reactInstance.GetJavaScriptModule<AppRegistry>().runApplication(jsAppModuleName, appParameters);
 
            // [UT-STUB]
            // TEST STUB BGN
            //Log.Info("MPM", "## MediaPlayerModule Test begin ##");
            //MediaPlayerModule mpModule = _currentReactContext.ReactInstance.GetNativeModule<MediaPlayerModule>();
            
            //mpModule.play();    // exceptional testing
            
            // to pick which one ?
            //mpModule.init("http://109.123.100.75:8086/assets/MusicPlayer/resources/music.mp3?platform=tizen&hash=5a9a91184e611dae3fed162b8787ce5f");
            //mpModule.init("https://pc.tedcdn.com/talk/stream/2017S/None/SineadBurke_2017S-950k.mp4");
           // mpModule.init("/opt/usr/apps/MusicPlayer.Tizen/shared/res/assets/MusicPlayer/resources/adele.mp3");
            //mpModule.seekTo(20*1000);
/*
            string[] uri= {"/opt/baisuzhen.mp3", "/opt/aaa.mp3"};

            for (int i = 0; i < 100; i++)
            {
                mpModule.init(uri[i%2]);  // wrong type with right source
                mpModule.play();
                Thread.Sleep(1000*5);
            }
*/
            //mpModule.init("/opt/baisuzhen.mp3");  // wrong type with right source

            //mpModule.setVolume(0.1f);

            //mpModule.setSubtitle("/opt/[zmk.tw]The.Circle.2017.srt");
            //mpModule.play();

            //Thread.Sleep(1000*5);

           // mpModule.stop();

            //Thread.Sleep(1000*3);

            //mpModule.play();

            //Thread.Sleep(1000*15);

            //mpModule.deInit();
            //mpModule.pause();
            //mpModule.seekTo(20*1000);
            //mpModule.seekTo(700*1000);

            //mpModule.setVolume(0.4f);
            //mpModule.play();
            //Thread.Sleep(1000*36);

            //mpModule.pause();
            //mpModule.deInit();
            //mpModule.play();
            //Log.Info(ReactConstants.Tag, "## MediaPlayerModule 55555555555 ##");
            //Thread.Sleep(1000*650);
            //Log.Info("MediaPlayerModule", $"## MediaPlayerModule  5555555555");
            //mpModule.stop();

            //Log.Info(ReactConstants.Tag, "## MediaPlayerModule 66666666666 ##");
            // TEST STUB END            

            /*
            // Create 'Button' thread 
            Thread th_On = new Thread(new ParameterizedThreadStart(UT_CreateView));
            th_On.IsBackground = true;
            th_On.Start(this);

            // terminate 'Button' thread
            Thread th_Off = new Thread(new ParameterizedThreadStart(UT_DropView));
            th_Off.IsBackground = true;
            th_Off.Start(this);

            // No detach mode = =!!
            //th_On.Join();  
            //th_Off.Join();

            */
            //UT_UIManagerModuleTest(this);
            // [UT-STUB]

            Log.Info(ReactConstants.Tag, "[ReactInstanceManager] ## Exit AttachMeasuredRootViewToInstance ThreadID=" + Thread.CurrentThread.ManagedThreadId.ToString()+" ##");

        }

        private void DetachViewFromInstance(ReactRootView rootView, IReactInstance reactInstance)
        {
            DispatcherHelpers.AssertOnDispatcher();

            if (-1 != rootView.GetTag() )
            {
                reactInstance.GetJavaScriptModule<AppRegistry>().unmountApplicationComponentAtRootTag(rootView.GetTag());
            }

            // timming issue
            Thread.Sleep(2*1000);
        }

        private async Task TearDownReactContextAsync(ReactContext reactContext)
        {
            DispatcherHelpers.AssertOnDispatcher();

            if (_lifecycleState == LifecycleState.Resumed)
            {
                reactContext.OnSuspend();
            }

            foreach (var rootView in _attachedRootViews)
            {
                DetachViewFromInstance(rootView, reactContext.ReactInstance);
            }

            await reactContext.DisposeAsync();
            _devSupportManager.OnReactContextDestroyed(reactContext);

            // TODO: add memory pressure hooks
        }

        private async Task<ReactContext> CreateReactContextAsync(
            Func<IJavaScriptExecutor> jsExecutorFactory,
            JavaScriptBundleLoader jsBundleLoader)
        {
            RNTracer.Write(ReactConstants.Tag, "Creating React context.");

            _sourceUrl = jsBundleLoader.SourceUrl;

            var nativeRegistryBuilder = new NativeModuleRegistry.Builder();
            var jsModulesBuilder = new JavaScriptModuleRegistry.Builder();

            var reactContext = new ReactContext();
            if (_useDeveloperSupport)
            {
                reactContext.NativeModuleCallExceptionHandler = _devSupportManager.HandleException;
            }

            using (RNTracer.Trace(RNTracer.TRACE_TAG_REACT_BRIDGE, "createAndProcessCoreModulesPackage").Start())
            {
                var coreModulesPackage =
                    new CoreModulesPackage(this, InvokeDefaultOnBackPressed, _uiImplementationProvider);


                ProcessPackage(coreModulesPackage, reactContext, nativeRegistryBuilder, jsModulesBuilder);
                                
            }

            foreach (var reactPackage in _packages)
            {
                using (RNTracer.Trace(RNTracer.TRACE_TAG_REACT_BRIDGE, "createAndProcessCustomReactPackage").Start())
                {
                    ProcessPackage(reactPackage, reactContext, nativeRegistryBuilder, jsModulesBuilder);
                }
            }

            var nativeModuleRegistry = default(NativeModuleRegistry);
            using (RNTracer.Trace(RNTracer.TRACE_TAG_REACT_BRIDGE, "buildNativeModuleRegistry").Start())
            {
                nativeModuleRegistry = nativeRegistryBuilder.Build();
            }

            var exceptionHandler = _nativeModuleCallExceptionHandler ?? _devSupportManager.HandleException;
            var reactInstanceBuilder = new ReactInstance.Builder
            {
                QueueConfigurationSpec = ReactQueueConfigurationSpec.Default,
                JavaScriptExecutorFactory = jsExecutorFactory,
                Registry = nativeModuleRegistry,
                JavaScriptModuleRegistry = jsModulesBuilder.Build(),
                BundleLoader = jsBundleLoader,
                NativeModuleCallExceptionHandler = exceptionHandler,
            };

            var reactInstance = default(ReactInstance);
            using (RNTracer.Trace(RNTracer.TRACE_TAG_REACT_BRIDGE, "createReactInstance").Start())
            {
                reactInstance = reactInstanceBuilder.Build();
            }

            // TODO: add bridge idle debug listener
            reactContext.InitializeWithInstance(reactInstance);

            reactInstance.Initialize();

            //using (RNTracer.Trace(RNTracer.TRACE_TAG_REACT_BRIDGE, "RunJavaScriptBundle").Start())
            {
                await reactInstance.InitializeBridgeAsync().ConfigureAwait(false);
            }

            return reactContext;
        }

        private void ProcessPackage(
            IReactPackage reactPackage,
            ReactContext reactContext,
            NativeModuleRegistry.Builder nativeRegistryBuilder,
            JavaScriptModuleRegistry.Builder jsModulesBuilder)
        {
            foreach (var nativeModule in reactPackage.CreateNativeModules(reactContext))
            {
                nativeRegistryBuilder.Add(nativeModule);
            }

            foreach (var type in reactPackage.CreateJavaScriptModulesConfig())
            {
                jsModulesBuilder.Add(type);
            }
        }

        private void MoveReactContextToCurrentLifecycleState(ReactContext reactContext)
        {
            if (_lifecycleState == LifecycleState.Resumed)
            {
                reactContext.OnResume();
            }
        }

        private void OnReactContextInitialized(ReactContext reactContext)
        {
            ReactContextInitialized?
                .Invoke(this, new ReactContextInitializedEventArgs(reactContext));
        }

        private void ToggleElementInspector()
        {
            _currentReactContext?
                .GetJavaScriptModule<RCTDeviceEventEmitter>()
                .emit("toggleElementInspector", null);
        }

        /// <summary>
        /// A Builder responsible for creating a React Instance Manager.
        /// </summary>
        public sealed class Builder
        {
            private List<IReactPackage> _packages = new List<IReactPackage>();

            private bool _useDeveloperSupport;
            private string _jsBundleFile;
            private string _jsMainModuleName;
            private LifecycleState? _initialLifecycleState;
            private UIImplementationProvider _uiImplementationProvider;
            private Action<Exception> _nativeModuleCallExceptionHandler;

            /// <summary>
            /// A provider of <see cref="UIImplementation" />.
            /// </summary>
            /// 
            
            public UIImplementationProvider UIImplementationProvider
            {
                set
                {
                    _uiImplementationProvider = value;
                }
            }
            
            /// <summary>
            /// Path to the JavaScript bundle file to be loaded from the file
            /// system.
            /// </summary>
            public string JavaScriptBundleFile
            {
                set
                {
                    _jsBundleFile = value;
                }
            }

            /// <summary>
            /// Path to the applications main module on the packager server.
            /// </summary>
            public string JavaScriptMainModuleName
            {
                set
                {
                    _jsMainModuleName = value;
                }
            }

            /// <summary>
            /// The mutable list of React packages.
            /// </summary>
            public List<IReactPackage> Packages
            {
                get
                {
                    return _packages;
                }
            }

            /// <summary>
            /// Signals whether the application should enable developer support.
            /// </summary>
            public bool UseDeveloperSupport
            {
                set
                {
                    _useDeveloperSupport = value;
                }
            }

            /// <summary>
            /// The initial lifecycle state of the host.
            /// </summary>
            public LifecycleState InitialLifecycleState
            {
                set
                {
                    _initialLifecycleState = value;
                }
            }

            /// <summary>
            /// The exception handler for all native module calls.
            /// </summary>
            public Action<Exception> NativeModuleCallExceptionHandler
            {
                set
                {
                    _nativeModuleCallExceptionHandler = value;
                }
            }

            /// <summary>
            /// Instantiates a new <see cref="ReactInstanceManager"/>.
            /// </summary>
            /// <returns>A React instance manager.</returns>
            /// 

            public ReactInstanceManager Build()
            {
                AssertNotNull(_initialLifecycleState, nameof(InitialLifecycleState));

                if (!_useDeveloperSupport && _jsBundleFile == null)
                {
                    throw new InvalidOperationException("JavaScript bundle file has to be provided when dev support is disabled.");
                }

                if (_jsBundleFile == null && _jsMainModuleName == null)
                {
                    throw new InvalidOperationException("Either the main module name or the JavaScript bundle file must be provided.");
                }

                if (_uiImplementationProvider == null)
                {
                    _uiImplementationProvider = new UIImplementationProvider();
                }
  
                return new ReactInstanceManager(
                    _jsBundleFile,
                    _jsMainModuleName,
                    _packages,
                    _useDeveloperSupport,
                    _initialLifecycleState.Value,
                    _uiImplementationProvider,
                    _nativeModuleCallExceptionHandler);
                    
            }
            

            private void AssertNotNull(object value, string name)
            {
                if (value == null)
                    throw new InvalidOperationException(Invariant($"'{name}' has not been set."));
            }
        }

        class ReactInstanceDevCommandsHandler : IReactInstanceDevCommandsHandler
        {
            private readonly ReactInstanceManager _parent;

            public ReactInstanceDevCommandsHandler(ReactInstanceManager parent)
            {
                _parent = parent;
            }

            public void OnBundleFileReloadRequest()
            {
                _parent.RecreateReactContextInBackground();
            }

            public void OnJavaScriptBundleLoadedFromServer()
            {
                _parent.OnJavaScriptBundleLoadedFromServer();
            }

            public void OnReloadWithJavaScriptDebugger(Func<IJavaScriptExecutor> javaScriptExecutorFactory)
            {
                _parent.OnReloadWithJavaScriptDebugger(javaScriptExecutorFactory);
            }

            public void ToggleElementInspector()
            {
                _parent.ToggleElementInspector();
            }
        }
    }
}
