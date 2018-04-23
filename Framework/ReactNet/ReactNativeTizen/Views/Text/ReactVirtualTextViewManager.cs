using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using ReactNative.Common;
using System;
using ElmSharp;
using System.Runtime.InteropServices;
using ReactNative.Tracing;

namespace ReactNative.Views.Text
{
    /// <summary>
    /// A virtual view manager for raw text nodes.
    /// </summary>
    public class ReactVirtualTextViewManager : ReactRawTextManager
    {
        /// <summary>
        /// The name of the view manager.
        /// </summary>
        public override string Name
        {
            get
            {
                return "RCTVirtualText";
            }
        }
    }
}
