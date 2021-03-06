﻿using ReactNative.Animated;
using ReactNative.Bridge;

using ReactNative.UIManager;

using ReactNative.Modules.AppState;
//using ReactNative.Modules.Clipboard;
using ReactNative.Modules.Core;
using ReactNative.Modules.Dialog;
using ReactNative.Modules.I18N;
using ReactNative.Modules.NetInfo;
using ReactNative.Modules.Network;
using ReactNative.Modules.Storage;
using ReactNative.Modules.WebSocket;
using ReactNative.Modules.MediaPlayer;

using ReactNative.Views.TextInput;
using ReactNative.Views.Scroll;
using ReactNative.Views.Text;
using ReactNative.Views.View;
using ReactNative.Views.ReactButton;
using ReactNative.Views.ReactImage;
using ReactNative.Views.Grid;
using ReactNative.Views.ActivityIndicator;
using ReactNative.Views.ReactProgressBar;
using ReactNative.Views.Switch;
using ReactNative.Views.Picker;

using System;
using System.Collections.Generic;

namespace ReactNative.Shell
{
    /// <summary>
    /// Package defining basic modules and view managers.
    /// </summary>
    public class MainReactPackage : IReactPackage
    {
        /// <summary>
        /// Creates the list of native modules to register with the react
        /// instance.
        /// </summary>
        /// <param name="reactContext">The React application context.</param>
        /// <returns>The list of native modules.</returns>
        public IReadOnlyList<INativeModule> CreateNativeModules(ReactContext reactContext)
        {
            return new List<INativeModule>
            {
                new AppStateModule(reactContext),
                new AsyncStorageModule(),
                //new CameraRollManager(reactContext),
                //new ClipboardModule(),
                new DialogModule(reactContext),
                //new ImageLoaderModule(),
                new I18NModule(),
                //new LauncherModule(reactContext),
                //new LocationModule(reactContext),
                new NativeAnimatedModule(reactContext),
                new NetworkingModule(reactContext),
                new NetInfoModule(reactContext),
                //new StatusBarModule(),
                //new VibrationModule(),
                new WebSocketModule(reactContext),
                new MediaPlayerModule(reactContext)
            };
        }

        /// <summary>
        /// Creates the list of JavaScript modules to register with the
        /// React instance.
        /// </summary>
        /// <returns>The list of JavaScript modules.</returns>
        public IReadOnlyList<Type> CreateJavaScriptModulesConfig()
        {
            return new List<Type>(0);
        }

        /// <summary>
        /// Creates the list of view managers that should be registered with
        /// the <see cref="UIManagerModule"/>.
        /// </summary>
        /// <param name="reactContext">The React application context.</param>
        /// <returns>The list of view managers.</returns>
        public IReadOnlyList<IViewManager> CreateViewManagers(
            ReactContext reactContext)
        {
            return new List<IViewManager>
            {
                new ReactGridManager(),
                //new ReactFlipViewManager(),
                new ReactImageManager(),
                //new ReactProgressBarViewManager(),
                //new ReactProgressRingViewManager(),
                new ReactPickerManager(),
                //new ReactRunManager(),
                ////new RecyclerViewBackedScrollViewManager(),
                new ReactScrollManager(),
                //new ReactSliderManager(),
                //new ReactSplitViewManager(),
                new ReactSwitchManager(),
                //new ReactPasswordBoxManager(),
                new ReactTextInputManager(),
                new ReactTextViewManager(),
                new ReactRawTextManager(),
				new ReactVirtualTextViewManager(),
                new ReactViewManager(),
                //new ReactSpanViewManager(),
                ////new SwipeRefreshLayoutManager(),
                //new ReactWebViewManager(),
                new ReactActivityIndicatorManager(),
                new ReactProgressBarManager(),
                new ReactButtonManager()
            };
        }
    }
}
