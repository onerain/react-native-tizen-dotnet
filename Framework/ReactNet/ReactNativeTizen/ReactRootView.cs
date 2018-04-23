using ReactNative.Bridge;
using ReactNative.UIManager;
using System;

using System.Threading; // For waiting 'RenderPostHandler' event

using Tizen;
using ReactNative.Common;
using ElmSharp;

namespace ReactNative
{
    /// <summary>
    /// Default root view for applications, Provides the ability to listen for
    /// size changes so that the UI manager can re-layout its elements.
    /// 
    /// It is also responsible for handling touch events passed to any of it's
    /// child views and sending those events to JavaScript via the
    /// <see cref="UIManager.Events.RCTEventEmitter"/> module.
    /// </summary>
    public class ReactRootView : SizeMonitoringCanvas
    {
        private IReactInstanceManager _reactInstanceManager;
        private string _jsModuleName;

        private bool _wasMeasured;
        private bool _attachScheduled;

        /// <summary>
        /// Gets the JavaScript module name.
        /// </summary>
        internal string JavaScriptModuleName
        {
            get
            {
                return _jsModuleName;
            }
        }

        public ReactRootView(EvasObject evasObj) : base(evasObj)
        {
            //Log.Info(ReactConstants.Tag, "## ReactRootView ##  Enter Constructor()");
            //RenderPost += RenderPostHandler;
            //Log.Info(ReactConstants.Tag, "## ReactRootView ##  Exit Constructor()");
        }

        protected override void OnRealized()
        {
            _wasMeasured = true;
            base.OnRealized();
        }

        /*
        /// <summary>
        /// The touch handler for the root view.
        /// </summary>
        internal TouchHandler TouchHandler
        {
            get;
            set;
        }
        */

        /// <summary>
        /// Schedule rendering of the React component rendered by the 
        /// JavaScript application from the given JavaScript module 
        /// <paramref name="moduleName"/> using the provided
        /// <paramref name="reactInstanceManager"/> to attach to the JavaScript
        /// context of that manager.
        /// </summary>
        /// <param name="reactInstanceManager">
        /// The React instance manager.
        /// </param>
        /// <param name="moduleName">The module name.</param>
        public void StartReactApplication(IReactInstanceManager reactInstanceManager, string moduleName)
        {
            DispatcherHelpers.AssertOnDispatcher();

            if (_reactInstanceManager != null)
            {
                throw new InvalidOperationException("This root view has already been attached to an instance manager.");
            }

            _reactInstanceManager = reactInstanceManager;
            _jsModuleName = moduleName;

            if (!_reactInstanceManager.HasStartedCreatingInitialContext)
            {
                _reactInstanceManager.CreateReactContextInBackground();
            }

            //Thread.Sleep(5000);    // Wait 'RenderPostHandler' event call 2017-02-24

            // We need to wait for the initial `Measure` call, if this view has
            // not yet been measured, we set the `_attachScheduled` flag, which
            // will enable deferred attachment of the root node.
            if (_wasMeasured)
            {
                _reactInstanceManager.AttachMeasuredRootView(this);
            }
            else
            {
                _attachScheduled = true;
            }
        }

        
        /// <summary>
        /// Hooks into the measurement event to potentially attach the React 
        /// root view.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <returns>The desired size.</returns>
        private void RenderPostHandler(object sender, EventArgs e)
        {
            //Log.Info(ReactConstants.Tag, "## Enter ReactRootView::RenderPostHandler ## ");
            DispatcherHelpers.AssertOnDispatcher();

            _wasMeasured = true;
            
            var reactInstanceManager = _reactInstanceManager;
            if (_attachScheduled && reactInstanceManager != null)
            {
                Log.Info(ReactConstants.Tag, "## ReactRootView::RenderPostHandler ## reactInstanceManager.AttachMeasuredRootView(this);");
                _attachScheduled = false;
                reactInstanceManager.AttachMeasuredRootView(this);
            }
            //Log.Info(ReactConstants.Tag, "## Exit ReactRootView::RenderPostHandler ## ");
        }
        
    }
}
