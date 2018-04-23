using Facebook.Yoga;
using ReactNative.Bridge;
//using ReactNative.Reflection;
using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
//using ReactNative.Views.Text;
using System;

using Tizen;
using ReactNative.Common;
using Newtonsoft.Json.Linq;

namespace ReactNative.Views.TextInput
{
    /// <summary>
    /// This extension of <see cref="LayoutShadowNode"/> is responsible for
    /// measuring the layout for Native <see cref="TextBox"/>.
    /// </summary>
    public class ReactTextInputShadowNode : LayoutShadowNode
    {
        private const int Unset = -1;

        private static readonly float[] s_defaultPaddings = 
        {
            10f,
            3f,
            6f,
            5f,
        };

        private float[] _computedPadding;
        private bool[] _isUserPadding = new bool[4];


        /*
        private FontStyle? _fontStyle;
        private FontWeight? _fontWeight;
        private TextAlignment _textAlignment = TextAlignment.DetectFromContent;
        */

        private string _text;

        private int _jsEventCount = Unset;

        /// <summary>
        /// Instantiates the <see cref="ReactTextInputShadowNode"/>.
        /// </summary>
        public ReactTextInputShadowNode()
        {
            SetDefaultPadding(EdgeSpacing.Start, s_defaultPaddings[0]);
            SetDefaultPadding(EdgeSpacing.Top, s_defaultPaddings[1]);
            SetDefaultPadding(EdgeSpacing.End, s_defaultPaddings[2]);
            SetDefaultPadding(EdgeSpacing.Bottom, s_defaultPaddings[3]);
            MeasureFunction = (node, width, widthMode, height, heightMode) =>
                MeasureTextInput(this, node, width, widthMode, height, heightMode);
        }

        /// <summary>
        /// Sets the text for the node.
        /// </summary>
        /// <param name="text">The text.</param>
        [ReactProp("text")]
        public void SetText(string text)
        {
            Log.Info(ReactConstants.Tag, "### SetText ->  ReactProp('text')");
            _text = text ?? "";
            MarkUpdated();
        }

        /// <summary>
        /// Set the most recent event count in JavaScript.
        /// </summary>
        /// <param name="mostRecentEventCount">The event count.</param>
        [ReactProp("mostRecentEventCount")]
        public void SetMostRecentEventCount(int mostRecentEventCount)
        {
            _jsEventCount = mostRecentEventCount;
        }

        /// <summary>
        /// Called to aggregate the current text and event counter.
        /// </summary>
        /// <param name="uiViewOperationQueue">The UI operation queue.</param>
        public override void OnCollectExtraUpdates(UIViewOperationQueue uiViewOperationQueue)
        {
            Log.Info(ReactConstants.Tag, "### Look, OnCollectExtraUpdates invoked ~ ");
            base.OnCollectExtraUpdates(uiViewOperationQueue);

            if (_computedPadding != null)
            {
                uiViewOperationQueue.EnqueueUpdateExtraData(ReactTag, _computedPadding);
                _computedPadding = null;
            }

            if (_jsEventCount != Unset)
            {
                uiViewOperationQueue.EnqueueUpdateExtraData(ReactTag, Tuple.Create(_jsEventCount, _text));
            }
        }

        /// <summary>
        /// Sets the padding of the shadow node.
        /// </summary>
        /// <param name="spacingType">The spacing type.</param>
        /// <param name="padding">The padding value.</param>
        public override void SetPaddings(int index, JValue padding)
        {
            MarkUpdated();
            base.SetPaddings(index, padding);
        }

        /// <summary>
        /// Marks a node as updated.
        /// </summary>
        protected override void MarkUpdated()
        {
            base.MarkUpdated();
            dirty();
        }

        private float[] GetComputedPadding()
        {
            return new[]
            {
                GetPadding(YogaEdge.Start),
                GetPadding(YogaEdge.Top),
                GetPadding(YogaEdge.End),
                GetPadding(YogaEdge.Bottom),
            };
        }


        private static YogaSize MeasureTextInput(ReactTextInputShadowNode textInputNode, YogaNode node, float width, YogaMeasureMode widthMode, float height, YogaMeasureMode heightMode)
        {
            textInputNode._computedPadding = textInputNode.GetComputedPadding();

            var borderLeftWidth = textInputNode.GetBorder(YogaEdge.Left);
            var borderRightWidth = textInputNode.GetBorder(YogaEdge.Right);

            var normalizedWidth = Math.Max(0,
                (YogaConstants.IsUndefined(width) ? double.PositiveInfinity : width)
                - textInputNode._computedPadding[0]
                - textInputNode._computedPadding[2]
                - (YogaConstants.IsUndefined(borderLeftWidth) ? 0 : borderLeftWidth)
                - (YogaConstants.IsUndefined(borderRightWidth) ? 0 : borderRightWidth));
            var normalizedHeight = Math.Max(0, YogaConstants.IsUndefined(height) ? double.PositiveInfinity : height);

            // This is not a terribly efficient way of projecting the height of
            // the text elements. It requires that we have access to the
            // dispatcher in order to do measurement, which, for obvious
            // reasons, can cause perceived performance issues as it will block
            // the UI thread from handling other work.
            //
            // TODO: determine another way to measure text elements.
            var task = DispatcherHelpers.CallOnDispatcher(() =>
            {

                /*
                var textBlock = new TextBlock
                {
                    TextWrapping = TextWrapping.Wrap,
                };

                var normalizedText = string.IsNullOrEmpty(textNode._text) ? " " : textNode._text;
                var inline = new Run { Text = normalizedText };
                FormatInline(textNode, inline);

                textBlock.Inlines.Add(inline);

                textBlock.Measure(new Size(normalizedWidth, normalizedHeight));

                var borderTopWidth = textInputNode.GetBorder(CSSSpacingType.Top);
                var borderBottomWidth = textInputNode.GetBorder(CSSSpacingType.Bottom);

                var finalizedHeight = (float)textBlock.DesiredSize.Height;
                finalizedHeight += textInputNode._computedPadding[1];
                finalizedHeight += textInputNode._computedPadding[3];
                finalizedHeight += CSSConstants.IsUndefined(borderTopWidth) ? 0 : borderTopWidth;
                finalizedHeight += CSSConstants.IsUndefined(borderBottomWidth) ? 0 : borderBottomWidth;
                               
                return new MeasureOutput(width, finalizedHeight);
                 */

                float finalizedHeight = 1;
                return MeasureOutput.Make(
                    (float)Math.Ceiling(width),
                    (float)Math.Ceiling(finalizedHeight));
            });

            return task.Result;
        }

        /*
        /// <summary>
        /// Formats an inline instance with shadow properties.
        /// </summary>
        /// <param name="textNode">The text shadow node.</param>
        /// <param name="inline">The inline.</param>
        protected static void FormatInline(ReactTextInputShadowNode textNode, TextElement inline)
        {
            if (textNode._fontSize != Unset)
            {
                var fontSize = textNode._fontSize;
                inline.FontSize = fontSize;
            }

            if (textNode._fontStyle.HasValue)
            {
                var fontStyle = textNode._fontStyle.Value;
                inline.FontStyle = fontStyle;
            }

            if (textNode._fontWeight.HasValue)
            {
                var fontWeight = textNode._fontWeight.Value;
                inline.FontWeight = fontWeight;
            }

            if (textNode._fontFamily != null)
            {
                var fontFamily = new FontFamily(textNode._fontFamily);
                inline.FontFamily = fontFamily;
            }
        }
        */
    }
}
