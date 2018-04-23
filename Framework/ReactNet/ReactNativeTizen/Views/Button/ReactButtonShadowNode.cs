using Facebook.Yoga;
using ReactNative.Bridge;
using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using ReactNative.Common;
using System;
using ReactNative.Tracing;
using Tizen;
using ElmSharp;

namespace ReactNative.Views.ReactButton
{
    /// <summary>
    /// The shadow node implementation for button views.
    /// </summary>
    public class ReactButtonShadowNode : LayoutShadowNode
    {
        //private string _text { set; get; }
        private string _preparedSpannableText { set; get; }

        /// <summary>
        /// Instantiates the <see cref="ReactButtonShadowNode"/>.
        /// </summary>
        /// A flag signaling whether or not the shadow node is virtual.
        /// </param>
        public ReactButtonShadowNode()
        {
            Log.Info(ReactConstants.Tag, "# construct # ReactButtonShadowNode");
            MeasureFunction = (node, width, widthMode, height, heightMode) =>
                MeasureButton(this, node, width, widthMode, height, heightMode);
        }        

        [ReactProp("title")]
        public void SetTitle(string title)
        {
            Log.Info(ReactConstants.Tag, "## SetTitle ##  title=" + title);
            _preparedSpannableText = title;
            MarkUpdated();
        }

        /// <summary>
        /// This method will be called by <see cref="UIManagerModule"/> once
        /// per batch, before calculating layout. This will only be called for
        /// nodes that are marked as updated with <see cref="MarkUpdated"/> or
        /// require layout (i.e., marked with <see cref="ReactShadowNode.dirty"/>).
        /// </summary>
        public override void OnBeforeLayout()
        {
            RNTracer.Write(ReactConstants.Tag, "[OnBeforeLayout] Button content: " + _preparedSpannableText);
            MarkUpdated();
        }

        /// <summary>
        /// Called after a layout step at the end of a UI batch from
        /// <see cref="UIManagerModule"/>. May be used to enqueue additional UI
        /// operations for the native view. Will only be called on nodes marked
        /// as updated.
        /// </summary>
        /// <param name="uiViewOperationQueue">
        /// Interface for enqueueing UI operations.
        /// </param>
        public override void OnCollectExtraUpdates(UIViewOperationQueue uiViewOperationQueue)
        {
            base.OnCollectExtraUpdates(uiViewOperationQueue);
            if (_preparedSpannableText != null)
            {
                ReactButtonUpdate reactTextUpdate =
                  new ReactButtonUpdate(
                    _preparedSpannableText,
                    0,
                    false,
                    0,
                    0,
                    0,
                    0,
                    0
                  );
                uiViewOperationQueue.EnqueueUpdateExtraData(ReactTag, reactTextUpdate);
            }
        }
        private YogaSize MeasureButton(ReactButtonShadowNode textNode, YogaNode node, float width,
            YogaMeasureMode widthMode, float height, YogaMeasureMode heightMode)
        {
            Log.Info(ReactConstants.Tag, "[1] MeasureButton node=" + textNode.ReactTag + " with="+width + " height="+height+ " content="+_preparedSpannableText);
            // This is not a terribly efficient way of projecting the height of
            // the text elements. It requires that we have access to the
            // dispatcher in order to do measurement, which, for obvious
            // reasons, can cause perceived performance issues as it will block
            // the UI thread from handling other work.
            //
            // TODO: determine another way to measure text elements.

            var task = DispatcherHelpers.CallOnDispatcher(() =>
            {
                var btnView = new Button(ReactProgram.RctWindow);

                var normalizedWidth = YogaConstants.IsUndefined(width) ? double.PositiveInfinity : width;
                var normalizedHeight = YogaConstants.IsUndefined(height) ? double.PositiveInfinity : height;

                btnView.Resize((int)normalizedWidth, (int)normalizedHeight);
                btnView.Text = _preparedSpannableText;

                var btnPartObject = btnView.EdjeObject["elm.text"];
                if (btnPartObject == null)
                {
                    throw new Exception("Invalid Button.EdjeObject[\"elm.text\"]");
                }
                Size size = btnPartObject.TextBlockFormattedSize ;
                Log.Info(ReactConstants.Tag, "[2] EDC conf info ={ width: " + size.Width + " height:" + size.Height + "}");
                btnView.Unrealize();

                return MeasureOutput.Make(
                    (float)(size.Width + 80),
                    (float)(size.Height < 80 ? 80 : size.Height));
            });
            return task.Result;
        }
    };
}