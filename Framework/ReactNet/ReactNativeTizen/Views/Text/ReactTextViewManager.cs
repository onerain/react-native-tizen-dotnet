using System;
using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using System.Collections;
using ReactNative.Tracing;
using ElmSharp;
using System.Runtime.InteropServices;

using Tizen;
using ReactNative.Common;
using Tizen.Applications;


namespace ReactNative.Views.Text
{
    /// <summary>
    /// The view manager for text views.
    /// </summary>
    public class ReactTextViewManager : BaseViewManager<ReactTextView, ReactTextShadowNode>
    {
        [DllImport("libevas.so.1")]
        internal static extern void evas_font_path_global_append(string path);
        public ReactTextViewManager() : base()
        {
            Utility.AppendGlobalFontPath(Application.Current.DirectoryInfo.SharedResource + "font");
        }
        /// <summary>
        /// The name of the view manager.
        /// </summary>
        public override string Name
        {
            get
            {
                return "RCTText";
            }
        }

        /// <summary>
        /// Sets whether or not the text is selectable.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="selectable">A flag indicating whether or not the text is selectable.</param>
        [ReactProp("selectable")]
        public void SetSelectable(ReactTextView view, bool selectable)
        {
            throw new NotImplementedException();
        }
        

        /// <summary>
        /// Creates the shadow node instance.
        /// </summary>
        /// <returns>The shadow node instance.</returns>
        public override ReactTextShadowNode CreateShadowNodeInstance()
        {
            return new ReactTextShadowNode(false);
        }
        

        /// <summary>
        /// Receive extra updates from the shadow node.
        /// </summary>
        /// <param name="root">The root view.</param>
        /// <param name="extraData">The extra data.</param>
        public override void UpdateExtraData(ReactTextView root, object extraData)
        {

            var update = extraData as ReactTextUpdate;
            root.Text = update.getText();
            root.SetVerticalTextAlignment("elm.text", update.getTextVAlign());
            Log.Info(ReactConstants.Tag, "TextUpdateExtraData: " + update.getText());
         }

        /// <summary>
        /// Creates the view instance.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <returns>The view instance.</returns>
        protected override ReactTextView CreateViewInstance(ThemedReactContext reactContext)
        {
            var TextView = new ReactTextView(ReactProgram.RctWindow);
            TextView.LineWrapType = WrapType.Mixed;
            TextView.Show();

            return TextView;
        }
    }
}