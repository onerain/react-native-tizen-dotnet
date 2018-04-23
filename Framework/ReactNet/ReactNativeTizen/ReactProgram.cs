using System;
using System.IO;
using System.Collections.Generic;
using ReactNative.Modules.Core;

using Tizen.Applications;                   // application life-cycle  
using ReactNativeTizen.ElmSharp.Extension;  // ui control

using Tizen;                                // FOR debug log
using ReactNative.Common;

using ElmSharp;

namespace ReactNative
{
    /// <summary>
    /// Base class of React Native applications.
    /// </summary>
    public abstract class ReactProgram : CoreUIApplication
    {
        private IReactInstanceManager _reactInstanceManager;   // ReactInstanceManger inst 
        //private LifecycleState mLifecycleState = LifecycleState.BeforeResume;
        public ReactRootView RootView { get; set; }                     // The root view managed by the page.
        public static string ResourceDir { get; private set; }          // ResourceDir here for extension (not bad)

        private static ReactWindow currentWindow;

        //public static Image image; // Tmporarily, testing with a 'real' iamge here.

        /// <summary>
        /// Gets or sets the <see cref="ReactWindow"/> which is using now.
        /// </summary>
        public static ReactWindow RctWindow
        {
            get
            {
                return ReactProgram.currentWindow;
            }
            internal set
            {
                if (ReactProgram.currentWindow == value)
                {
                    return;
                }
                if (value == null)
                {
                    ReactProgram.currentWindow = null;
                }
                ReactProgram.currentWindow = value;
            }
        }

        #region "UICoreApplication Part"
        protected ReactProgram()
        {

        }

        protected override void OnCreate()
        {
            //Log.Info(ReactConstants.Tag, "## ReactProgram ##  Enter Constructor ReactProgram()");
            try
            {
                _reactInstanceManager = CreateReactInstanceManager();
            }
            catch (Exception ex)
            {
                Log.Info(ReactConstants.Tag, "## ReactProgram ## CreateReactInstanceManager Ex:" + ex.ToString());
            }
            //Log.Info(ReactConstants.Tag, "## ReactProgram ##  Exit Constructor ReactProgram()");

            Elementary.Initialize();
            Elementary.ThemeOverlay();

            //Log.Info(ReactConstants.Tag, "## ReactProgram ##  Enter OnCreate()");

            ResourceDir = DirectoryInfo.Resource;

            // 1. Create root window
            ReactWindow rctWin = new ReactWindow("ElmSharp Window");
            RctWindow = rctWin;
            RctWindow.Show();
            RctWindow.BackButtonPressed += (object sender, EventArgs e) =>
            {
                Log.Debug(ReactConstants.Tag, "## Back button being Pressed ##");
                _reactInstanceManager.OnBackPressed();
            };

            RctWindow.RedButtonPressed += (object sender, EventArgs e) =>
            {
                Log.Debug(ReactConstants.Tag, "## Red button being Pressed ##");
                _reactInstanceManager.DevSupportManager.ShowDevOptionsDialog();
            };

            // 2. Create root view
            RootView = CreateRootView();
            //RootView.Show();


            // 3. Entry of 'JS' world 
            RootView.StartReactApplication(_reactInstanceManager, MainComponentName);

            // 4. Set root view
            //RctWindow.Navigator.Push(RootView, "Instagram");
            RctWindow.SetMainPage(RootView);

            base.OnCreate();

            //Log.Info(ReactConstants.Tag, "## ReactProgram ##  Exit OnCreate()");
        }

        /// <summary>
        /// Called before the application is suspended.
        /// </summary>
        protected override void OnPause()
        {
            //Log.Info(ReactConstants.Tag, "## ReactProgram ##  Enter OnPause()");
            _reactInstanceManager.OnSuspend();
            //Log.Info(ReactConstants.Tag, "## ReactProgram ##  Exit OnPause()");
        }

        /// <summary>
        /// Called before the application is resumed.
        /// </summary>
        protected override void OnResume()
        {
            //Log.Info(ReactConstants.Tag, "## ReactProgram ##  Enter OnResume()");
            OnResume(() => {
                Log.Info(ReactConstants.Tag, "## ReactProgram ##  Back key pressed");
            });
            //Log.Info(ReactConstants.Tag, "## ReactProgram ##  Exit OnResume()");
        }

        /// <summary>
        /// Called when the application is resumed.
        /// </summary>
        /// <param name="onBackPressed">
        /// Default action to take when back pressed.
        /// </param>
        protected void OnResume(Action onBackPressed)
        {
            //Log.Info(ReactConstants.Tag, "## ReactProgram ##  Enter OnResume() with BackPress key event");
            _reactInstanceManager.OnResume(onBackPressed);
            //Log.Info(ReactConstants.Tag, "## ReactProgram ##  Exit OnResume() with BackPress key event");
        }

        protected override void OnTerminate()
        {
            //Log.Info(ReactConstants.Tag, "## ReactProgram ##  Enter OnTerminate()");

            // detach root view
            _reactInstanceManager.DetachRootView(RootView);

            base.OnTerminate();
            //Log.Info(ReactConstants.Tag, "## ReactProgram ##  Exit OnTerminate()");
        }
        #endregion


        #region "ReactNative Part"
        /// <summary>
        /// Create root view
        /// </summary>
        /// <returns></returns>
        protected ReactRootView CreateRootView()
        {
            //Log.Info(ReactConstants.Tag, "## ReactProgram ##  CreateRootView()");
            return new ReactRootView(RctWindow);
        }

        /// <summary>
        /// The custom path of the bundle file.
        /// </summary>
        /// <remarks>
        /// This is used in cases where the bundle should be loaded from a
        /// custom path.
        /// </remarks>
        public virtual string JavaScriptBundleFile
        {
            get
            {
                //return "index.tizen.bundle";    // should not return any const string
                //from base class , this will effect debug mode logic, need to pass null
                return null;
            }
        }

        /// <summary>
        /// The name of the main module.
        /// </summary>
        /// <remarks>
        /// This is used to determine the URL used to fetch the JavaScript
        /// bundle from the packager server. It is only used when dev support
        /// is enabled.
        /// </remarks>
        public virtual string JavaScriptMainModuleName
        {
            get
            {
                return "index.tizen";
            }
        }

        /// <summary>
        /// The name of the main component registered from JavaScript.
        /// </summary>
        public abstract string MainComponentName { get; }

        /// <summary>
        /// Signals whether developer mode should be enabled.
        /// </summary>
        public abstract bool UseDeveloperSupport { get; }

        /// <summary>
        /// The list of <see cref="IReactPackage"/>s used by the application.
        /// </summary>
        public abstract List<IReactPackage> Packages { get; }

        private IReactInstanceManager CreateReactInstanceManager()
        {
            //Log.Info(ReactConstants.Tag, "## CreateReactInstanceManager");

            var builder = new ReactInstanceManager.Builder
            {
                UseDeveloperSupport = UseDeveloperSupport,
                InitialLifecycleState = LifecycleState.Resumed,
                JavaScriptBundleFile = JavaScriptBundleFile,
                JavaScriptMainModuleName = JavaScriptMainModuleName,
            };

            //Log.Info(ReactConstants.Tag, "After new ReactInstanceManager.Builder");

            builder.Packages.AddRange(Packages);

            //Log.Info(ReactConstants.Tag, "After builder.Packages.AddRange(Packages)");

            return builder.Build();
        }
        #endregion
    }
}