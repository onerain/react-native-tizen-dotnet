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
    public class ReactRawTextManager : ReactTextViewManager
    {
        /// <summary>
        /// The name of the view manager.
        /// </summary>
        public override string Name
        {
            get
            {
                return "RCTRawText";
            }
        }

        /// <summary>
        /// Should not be called, as this is a virtual view manager.
        /// </summary>
        /// <param name="root">Irrelevant.</param>
        /// <param name="extraData">Irrelevant.</param>
        public override void UpdateExtraData(ReactTextView root, object extraData)
        {

        }

        
        /// <summary>
        /// Creates a shadow node instance for a view.
        /// </summary>
        /// <returns>The shadow node instance.</returns>
		public override ReactTextShadowNode CreateShadowNodeInstance()
        {
			return new ReactTextShadowNode(true);
        }

        /// <summary>
        /// Should not be called, as this is a virtual view manager.
        /// </summary>
        /// <param name="reactContext">Irrelevant.</param>
        /// <returns>Irrelevant.</returns>
        protected override ReactTextView CreateViewInstance(ThemedReactContext reactContext)
        {
            throw new InvalidOperationException("RKRawText doesn't map into a native view");
        }
    }
}
