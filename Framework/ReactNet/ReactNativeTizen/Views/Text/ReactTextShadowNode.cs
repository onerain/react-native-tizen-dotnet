using Facebook.Yoga;
using ReactNative.Bridge;
using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using ReactNative.Common;
using System;
using ReactNative.Tracing;
using Tizen;
using ElmSharp;

namespace ReactNative.Views.Text
{
    /// <summary>
    /// The shadow node implementation for text views.
    /// </summary>
    public class ReactTextShadowNode : LayoutShadowNode
    {
        public static readonly int UNSET = -1;
        private double? _fontSize;
        private string _fontFamily;
        private string _fontStyle;
        private string _fontWeight;
        private int _lineHeight;
        private string _textHAlign;
        private string _textVAlign;
        private double _textVAignScale;
        uint _color = 0xffffff; //white
        uint _shadowcolor = 0x55000000; //according to android
        uint? _textDecorationColor = 0x0;
        uint? _backgroundColor = null;
        private string _text;
        private bool _IsUnderlineTextDecorationSet = false;
        private bool _IsLineThroughTextDecorationSet = false;
        private string _ellipsizeMode = null;


        private bool _isVirtual;
        static private readonly string _textPropertyHead = "<span>";
        static private readonly string _textPropertyTail = "</span>";
        private string _textProperty = null;
        string _preparedSpannableText;


        //private int _letterSpacing;
        //private int _numberOfLines;
        //private double _lineHeight;
        //private FontStyle? _fontStyle;
        //private FontWeight? _fontWeight;
        //private TextAlignment _textAlignment = TextAlignment.Left;
        //private string _fontFamily;
        public int id;

        /// <summary>
        /// Nodes that return <code>true</code> will be treated as "virtual"
        /// nodes. That is, nodes that are not mapped into native views (e.g.,
        /// nested text node).
        /// </summary>
        /// <remarks>
        /// By default this method returns <code>false</code>.
        /// </remarks>
        public override bool IsVirtual
        {
            get
            {
                return _isVirtual;
            }
        }


        public override bool IsVirtualAnchor
        {
            get
            {
                return !_isVirtual;
            }
        }

        /// <summary>
        /// Instantiates the <see cref="ReactTextShadowNode"/>.
        /// </summary>
        /// A flag signaling whether or not the shadow node is virtual.
        /// </param>
        public ReactTextShadowNode(bool isVirtual)
        {
            _isVirtual = isVirtual;
            if (!isVirtual)
            {
                MeasureFunction = (node, width, widthMode, height, heightMode) =>
                    MeasureText(this, node, width, widthMode, height, heightMode);
            }
        }

        [ReactProp(ViewProps.TextAlign)]
        public void setTextAlign(string textHAlign)
        {
            if (_textHAlign != textHAlign)
            {
                chechProperty();
                _textHAlign = textHAlign;
                if (_textHAlign == null || "auto".Equals(_textHAlign))
                {
                    _textProperty = eraseProperty(_textProperty, "align");
                }
                else if ("left".Equals(_textHAlign))
                {
                    string strAlign = " align=left";
                    _textProperty = eraseProperty(_textProperty, "align");
                    int pos = _textProperty.IndexOf('>');
                    _textProperty = _textProperty.Insert(pos, strAlign);
                }
                else if ("right".Equals(_textHAlign))
                {
                    string strAlign = " align=right";
                    _textProperty = eraseProperty(_textProperty, "align");
                    int pos = _textProperty.IndexOf('>');
                    _textProperty = _textProperty.Insert(pos, strAlign);
                }
                else if ("center".Equals(_textHAlign))
                {
                    string strAlign = " align=center";
                    _textProperty = eraseProperty(_textProperty, "align");
                    int pos = _textProperty.IndexOf('>');
                    _textProperty = _textProperty.Insert(pos, strAlign);
                }
                else if ("justify".Equals(_textHAlign))
                {
                    // Fallback gracefully for cross-platform compat instead of error
                    string strAlign = " align=left";
                    _textProperty = eraseProperty(_textProperty, "align");
                    int pos = _textProperty.IndexOf('>');
                    _textProperty = _textProperty.Insert(pos, strAlign);
                }
                else
                {
                    throw new Exception("Invalid textAlign: " + _textHAlign);
                }
                MarkUpdated();
            }
        }


        [ReactProp(ViewProps.TextAlignVertical)]
        public void setTextAlignVertical(string textVAlign)
        {
            if (_textVAlign != textVAlign)
            {
                chechProperty();
                _textVAlign = textVAlign;
                if (_textVAlign == null || "auto".Equals(_textVAlign))
                {
                    _textProperty = eraseProperty(_textProperty, "valign");
                    _textVAignScale = 0.5;
                }
                else if ("top".Equals(_textVAlign))
                {
                    string strAlign = " valign=top";
                    _textProperty = eraseProperty(_textProperty, "valign");
                    int pos = _textProperty.IndexOf('>');
                    _textProperty = _textProperty.Insert(pos, strAlign);
                    _textVAignScale = 0;
                }
                else if ("bottom".Equals(_textVAlign))
                {
                    string strAlign = " valign=bottom";
                    _textProperty = eraseProperty(_textProperty, "valign");
                    int pos = _textProperty.IndexOf('>');
                    _textProperty = _textProperty.Insert(pos, strAlign);
                    _textVAignScale = 1.0;
                }
                else if ("center".Equals(_textVAlign))
                {
                    string strAlign = " valign=center";
                    _textProperty = eraseProperty(_textProperty, "valign");
                    int pos = _textProperty.IndexOf('>');
                    _textProperty = _textProperty.Insert(pos, strAlign);
                    _textVAignScale = 0.5;
                }
                else
                {
                    throw new Exception("Invalid textAlignVertical: " + _textVAlign);
                }
                MarkUpdated();
            }
        }
        /*
        ///// <summary>
        ///// Used to truncate the text with an ellipsis after computing the text layout
        ///// </summary>
        ///// <param name="text">The number of line.</param>
        [ReactProp(ViewProps.NumberOfLines)]
        public void SetNumberOfLines(int numberOfLines)
        {
            throw new NotImplementedException();
        }
        */

        [ReactProp(ViewProps.LineHeight)]
        public void setLineHeight(int lineHeight)
        {
            if (_lineHeight != lineHeight)
            {
                chechProperty();
                _lineHeight = lineHeight;
                string strlineHeight = " linegap=" + _lineHeight.ToString();
                _textProperty = eraseProperty(_textProperty, "linegap");
                int pos = _textProperty.IndexOf('>');
                _textProperty = _textProperty.Insert(pos, strlineHeight);
                MarkUpdated();
            }
        }

        ///// <summary>
        ///// Sets the raw text.
        ///// </summary>
        ///// <param name="text">The text.</param>
        [ReactProp("text")]
        public void SetText(string text)
        {
            text = ReactTextView.filterText(text);
            _text = text ?? "";
            MarkUpdated();
        }


        /// <summary>
        /// Sets the font color for the node.
        /// </summary>
        /// <param name="color">The masked color value.</param>
        [ReactProp(ViewProps.Color, CustomType = "Color")]
        public void SetColor(uint? color)
        {
            if (color.HasValue && _color != color)
            {
                chechProperty();
                _color = color.Value;
                uint alpha = color.Value >> 24;
                color = color & 0xffffff;
                string strColor = " color=#" + string.Format("{0:X06}", color) + string.Format("{0:X02}", alpha);
                _textProperty = eraseProperty(_textProperty, "color");
                int pos = _textProperty.IndexOf('>');
                _textProperty = _textProperty.Insert(pos, strColor);
                MarkUpdated();
            }
        }

        [ReactProp(ViewProps.TextShadowColor)]
        public void setTextShadowColor(uint? color)
        {
            if (color.HasValue && _shadowcolor != color)
            {
                chechProperty();
                _shadowcolor = color.Value;
                uint alpha = color.Value >> 24;
                color = color & 0xffffff;
                string strColor = " shadow_color=#" + string.Format("{0:X06}", color) + string.Format("{0:X02}", alpha);
                _textProperty = eraseProperty(_textProperty, "shadow_color");
                _textProperty = eraseProperty(_textProperty, "style");
                int pos = _textProperty.IndexOf('>');
                _textProperty = _textProperty.Insert(pos, strColor);
                pos = _textProperty.IndexOf('>');
                string strStyle = " style=shadow,bottom_right";
                _textProperty = _textProperty.Insert(pos, strStyle);
                MarkUpdated();
            }
        }

        /// <summary>
        /// Sets the font size for the node.
        /// </summary>
        /// <param name="fontSize">The font size.</param>
        [ReactProp(ViewProps.FontSize)]
        public void SetFontSize(double? fontSize)
        {

            if (_fontSize != fontSize)
            {
                chechProperty();
                _fontSize = fontSize;
                string strFontSize = " font_size=" + _fontSize.ToString();
                _textProperty = eraseProperty(_textProperty, "font_size");
                int pos = _textProperty.IndexOf('>');
                _textProperty = _textProperty.Insert(pos, strFontSize);
                MarkUpdated();
            }
        }

        /// <summary>
        /// Sets the font family for the node.
        /// </summary>
        /// <param name="fontFamily">The font family.</param>
        [ReactProp(ViewProps.FontFamily)]
        public void SetFontFamily(string fontFamily)
        {
            if (_fontFamily != fontFamily)
            {
                chechProperty();
                _fontFamily = fontFamily;
                string strFontFamily = " font=" + _fontFamily;
                _textProperty = eraseProperty(_textProperty, "font");
                int pos = _textProperty.IndexOf('>');
                _textProperty = _textProperty.Insert(pos, strFontFamily);
                MarkUpdated();
            }
        }


        /// <summary>
        /// Sets the font weight for the node.
        /// </summary>
        /// <param name="fontWeightValue">The font weight string.</param>
        [ReactProp(ViewProps.FontWeight)]
        public void SetFontWeight(string fontWeight)
        {
            if (_fontWeight != fontWeight)
            {
                chechProperty();
                _fontWeight = fontWeight;
                string strFontWeight;
                if (_fontWeight == "normal" || _fontWeight == "bold")
                {
                    strFontWeight = " font_weight=" + _fontWeight;
                }
                else
                {
                    int nfontWeight = Int32.Parse(_fontWeight);
                    if (nfontWeight >= 500)
                    {
                        strFontWeight = " font_weight=bold";
                    }
                    else
                    {
                        strFontWeight = " font_weight=normal";
                    }
                }
                _textProperty = eraseProperty(_textProperty, "font_weight");
                int pos = _textProperty.IndexOf('>');
                _textProperty = _textProperty.Insert(pos, strFontWeight);
                MarkUpdated();
            }
        }

        /// <summary>
        /// Sets the font style for the node.
        /// </summary>
        /// <param name="fontStyleValue">The font style string.</param>
        [ReactProp(ViewProps.FontStyle)]
        public void SetFontStyle(string fontStyle)
        {
            if (_fontStyle != fontStyle)
            {
                chechProperty();
                _fontStyle = fontStyle;

                string strFontStyle = " font_style=" + _fontStyle;
                _textProperty = eraseProperty(_textProperty, "font_style");
                int pos = _textProperty.IndexOf('>');
                _textProperty = _textProperty.Insert(pos, strFontStyle);
                MarkUpdated();
            }
        }

        [ReactProp(ViewProps.BackgroundColor)]
        public void setBackgroundColor(uint? color)
        {
            // Don't apply background color to anchor TextView since it will be applied on the View directly
            //if (!IsVirtualAnchor)
            //{
                if (color.HasValue)
                {
                    if (!_backgroundColor.HasValue || _backgroundColor.Value != color)
                    {
                        chechProperty();
                        _backgroundColor = color;
                        uint alpha = color.Value >> 24;
                        color = color & 0xffffff;
                        string strBKColor = " backing_color=#" + string.Format("{0:X06}", color) + string.Format("{0:X02}", alpha);
                        _textProperty = eraseProperty(_textProperty, "backing_color");
                        int pos = _textProperty.IndexOf('>');
                        _textProperty = _textProperty.Insert(pos, strBKColor);
                        string strBKSwitch = " backing=on";
                        _textProperty = eraseProperty(_textProperty, "backing");
                        pos = _textProperty.IndexOf('>');
                        _textProperty = _textProperty.Insert(pos, strBKSwitch);
                        MarkUpdated();
                    }
                }
            //}
        }

        [ReactProp(ViewProps.TextDecorationLine)]
        public void setTextDecorationLine(string textDecorationLineString)
        {
            bool IsUnderlineTextDecorationSet = false;
            bool IsLineThroughTextDecorationSet = false;
            if (textDecorationLineString != null)
            {
                foreach (string textDecorationLineSubString in textDecorationLineString.Split(' '))
                {
                    if ("underline".Equals(textDecorationLineSubString))
                    {
                        IsUnderlineTextDecorationSet = true;
                    }
                    else if ("line-through".Equals(textDecorationLineSubString))
                    {
                        IsLineThroughTextDecorationSet = true;
                    }
                }
            }
            if (IsUnderlineTextDecorationSet != _IsUnderlineTextDecorationSet)
            {
                chechProperty();
                _IsUnderlineTextDecorationSet = IsUnderlineTextDecorationSet;
                _textProperty = eraseProperty(_textProperty, "underline");
                if (_IsUnderlineTextDecorationSet == true)
                {
                    string strUnderlineText = " underline=on";
                    if (_textDecorationColor != 0x0)
                    {
                        _textProperty = eraseProperty(_textProperty, "underline_color");
                        uint alpha = _textDecorationColor.Value >> 24;
                        _textDecorationColor = _textDecorationColor & 0xffffff;
                        string strUnderColor = " underline_color=#" + string.Format("{0:X06}", _textDecorationColor) + string.Format("{0:X02}", alpha);
                        strUnderlineText += strUnderColor;
                    }
                    int pos = _textProperty.IndexOf('>');
                    _textProperty = _textProperty.Insert(pos, strUnderlineText);
                }
                MarkUpdated();
            }
            if (IsLineThroughTextDecorationSet != _IsLineThroughTextDecorationSet)
            {
                chechProperty();
                _IsLineThroughTextDecorationSet = IsLineThroughTextDecorationSet;
                _textProperty = eraseProperty(_textProperty, "strikethrough");
                if (_IsLineThroughTextDecorationSet == true)
                {
                    string strLineThroughText = " strikethrough=on";
                    if (_textDecorationColor != 0x0)
                    {
                        _textProperty = eraseProperty(_textProperty, "strikethrough_color");
                        uint alpha = _textDecorationColor.Value >> 24;
                        _textDecorationColor = _textDecorationColor & 0xffffff;
                        string strStrikeColor = " strikethrough_color=#" + string.Format("{0:X06}", _textDecorationColor) + string.Format("{0:X02}", alpha);
                        strLineThroughText += strStrikeColor;
                    }
                    int pos = _textProperty.IndexOf('>');
                    _textProperty = _textProperty.Insert(pos, strLineThroughText);
                }
                MarkUpdated();
            }
        }


        [ReactProp(ViewProps.TextDecorationColor)]
        public void SetTextDecorationColor(uint? color)
        {
            if (color.HasValue)
            {
                if (!_textDecorationColor.HasValue || _textDecorationColor.Value != color)
                {
                    _textDecorationColor = color;
                    if (_IsUnderlineTextDecorationSet || _IsLineThroughTextDecorationSet)
                    {
                        chechProperty();
                        uint alpha = color.Value >> 24;
                        color = color & 0xffffff;
                        string strDecorationColor = "";
                        if (_IsUnderlineTextDecorationSet)
                        {
                            _textProperty = eraseProperty(_textProperty, "underline_color");
                            string strUnderColor = " underline_color=#" + string.Format("{0:X06}", color) + string.Format("{0:X02}", alpha);
                            strDecorationColor += strUnderColor;
                        }
                        if (_IsLineThroughTextDecorationSet)
                        {
                            _textProperty = eraseProperty(_textProperty, "strikethrough_color");
                            string strStrikeColor = " strikethrough_color=#" + string.Format("{0:X06}", color) + string.Format("{0:X02}", alpha);
                            strDecorationColor += strStrikeColor;
                        }
                        int pos = _textProperty.IndexOf('>');
                        _textProperty = _textProperty.Insert(pos, strDecorationColor);
                        MarkUpdated();
                    }
                }
            }
        }


        [ReactProp(ViewProps.EllipsizeMode)]
        public void setEllipsizeMode(string ellipsizeMode)
        {
            if (_ellipsizeMode != ellipsizeMode)
            {
                _ellipsizeMode = ellipsizeMode;
                if (ellipsizeMode == null || ellipsizeMode.Equals("tail"))
                {
                    chechProperty();
                    _textProperty = eraseProperty(_textProperty, "ellipsis");
                    string strellipsizeMode = " ellipsis=1";
                    int pos = _textProperty.IndexOf('>');
                    _textProperty = _textProperty.Insert(pos, strellipsizeMode);
                    MarkUpdated();
                }
                else if (ellipsizeMode.Equals("head"))
                {
                    chechProperty();
                    _textProperty = eraseProperty(_textProperty, "ellipsis");
                    string strellipsizeMode = " ellipsis=0";
                    int pos = _textProperty.IndexOf('>');
                    _textProperty = _textProperty.Insert(pos, strellipsizeMode);
                    MarkUpdated();
                }
                else if (ellipsizeMode.Equals("middle"))
                {
                    chechProperty();
                    _textProperty = eraseProperty(_textProperty, "ellipsis");
                    string strellipsizeMode = " ellipsis=0.5";
                    int pos = _textProperty.IndexOf('>');
                    _textProperty = _textProperty.Insert(pos, strellipsizeMode);
                    MarkUpdated();
                }
                else
                {
                    throw new Exception("Invalid ellipsizeMode: " + ellipsizeMode);
                }
            }
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
            if (_isVirtual)
            {
                return;
            }
            base.OnCollectExtraUpdates(uiViewOperationQueue);
            if (_preparedSpannableText != null)
            {
                ReactTextUpdate reactTextUpdate =
                  new ReactTextUpdate(
                    _preparedSpannableText,
                    0,
                    false,
                    0,
                    0,
                    0,
                    0,
                    0,
                    _textVAignScale
                  );
                uiViewOperationQueue.EnqueueUpdateExtraData(ReactTag, reactTextUpdate);
            }
        }

        /// <summary>
        /// Marks a node as updated.
        /// </summary>
        protected override void MarkUpdated()
        {
            base.MarkUpdated();
            if (!_isVirtual)
            {
                base.dirty();
            }
        }

        static private ReactTextView textView = null;

        private static YogaSize MeasureText(ReactTextShadowNode textNode, YogaNode node, float width,
            YogaMeasureMode widthMode, float height, YogaMeasureMode heightMode)
        {
            Log.Info(ReactConstants.Tag, "MeasureText node=" + textNode.ReactTag);
            // This is not a terribly efficient way of projecting the height of
            // the text elements. It requires that we have access to the
            // dispatcher in order to do measurement, which, for obvious
            // reasons, can cause perceived performance issues as it will block
            // the UI thread from handling other work.
            //
            // TODO: determine another way to measure text elements.

            var task = DispatcherHelpers.CallOnDispatcher(() =>
            {
                if(textView == null)
                {
                    textView = new ReactTextView(ReactProgram.RctWindow);
                    textView.LineWrapType = WrapType.Mixed;
                }

                var normalizedWidth = YogaConstants.IsUndefined(width) ? ReactProgram.RctWindow.ScreenSize.Width : width;
                var normalizedHeight = YogaConstants.IsUndefined(height) ? ReactProgram.RctWindow.ScreenSize.Height : height;
                textView.Resize((int)normalizedWidth, (int)normalizedHeight);

                textView.Text = textNode._preparedSpannableText;
                var textPartObject = textView.EdjeObject["elm.text"];
                if (textPartObject == null)
                {
                    throw new Exception("Invalid ReactTextView.EdjeObject[\"elm.text\"]");
                }
                Size size = textPartObject.TextBlockFormattedSize;
                Log.Info(ReactConstants.Tag, "MeasureText result : width: " + size.Width + " height:" + size.Height);
                
                //textView.Unrealize();

                return MeasureOutput.Make(
                    (float)size.Width,
                    (float)size.Height);
            });
            return task.Result;
        }
        
        /// <summary>
        /// This method will be called by <see cref="UIManagerModule"/> once
        /// per batch, before calculating layout. This will only be called for
        /// nodes that are marked as updated with <see cref="MarkUpdated"/> or
        /// require layout (i.e., marked with <see cref="ReactShadowNode.dirty"/>).
        /// </summary>
        public override void OnBeforeLayout()
        {
            if (_isVirtual)
            {
                return;
            }
            _preparedSpannableText = fromTextCSSNode(this);
            MarkUpdated();
        }


        protected static string fromTextCSSNode(ReactTextShadowNode textCSSNode)
        {
            string span = "";
            buildSpannedFromTextCSSNode(textCSSNode, ref span);
            if (!span.StartsWith("<span"))
            {
                span = _textPropertyHead + span + _textPropertyTail;
            }
            return span;
        }

        private static void buildSpannedFromTextCSSNode(ReactTextShadowNode textShadowNode,ref string sb)
        {
            if (textShadowNode._textProperty != null)
            {
                sb += textShadowNode._textProperty;
            }
            if (textShadowNode._text != null)
            {
                sb += textShadowNode._text;
            }

            for (int i = 0, length = textShadowNode.TotalNativeChildren; i < length; i++)
            {
                ReactShadowNode child = textShadowNode.GetChildAt(i);
                if (child is ReactTextShadowNode)
                {
                    buildSpannedFromTextCSSNode((ReactTextShadowNode)child, ref sb);
                }
                child.MarkUpdateSeen();
            }
            if (textShadowNode._textProperty != null)
            {
                sb += _textPropertyTail;
            }
        }

        private string eraseProperty(string originText, string strProp)
        {
            strProp = " " + strProp + "=";
            int startPos = originText.IndexOf(strProp);
            if (startPos == -1)
            {
                return originText;
            }
            int spaceEndPos = originText.IndexOf(' ', startPos + 1);
            int endPos = 0;
            if (spaceEndPos != -1)
            {
                endPos = Math.Min(spaceEndPos, originText.IndexOf('>', startPos));
            }
            else
            {
                endPos = originText.IndexOf('>', startPos);
            }
            if (endPos == -1)
            {
                return originText;
            }
            return originText.Remove(startPos, endPos - startPos);
        }

        public void chechProperty()
        {
            if (_textProperty == null)
            {
                _textProperty = _textPropertyHead;
            }
        }
    }
}
