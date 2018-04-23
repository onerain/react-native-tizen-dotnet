using System;
using System.Collections.Generic;
using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using ReactNative.Reflection;

using Tizen;
using ReactNative.Common;
using ElmSharp;
using ReactNativeTizen.ElmSharp.Extension;

namespace ReactNative.Views.TextInput
{
    public class ReactTextInputManager : SimpleViewManager<ReactTextBox>
    {
        internal const int FocusTextInput = 1;
        internal const int BlurTextInput = 2;

        private const string ReactClass = "RCTTextInput";

        /// <summary>
        /// The name of this view manager. This will be the name used to 
        /// reference this view manager from JavaScript.
        /// </summary>
        public override string Name
        {
            get
            {
                return ReactClass;
            }
        }

        /// <summary>
        /// The exported custom bubbling event types.
        /// </summary>
        public override IReadOnlyDictionary<string, object> ExportedCustomBubblingEventTypeConstants
        {
            get
            {
                return new Dictionary<string, object>()
                {
                    {
                        "topSubmitEditing",
                        new Dictionary<string, object>()
                        {
                            {
                                "phasedRegistrationNames",
                                new Dictionary<string, string>()
                                {
                                    { "bubbled" , "onSubmitEditing" },
                                    { "captured" , "onSubmitEditingCapture" }
                                }
                            }
                        }
                    },
                    {
                        "topEndEditing",
                        new Dictionary<string, object>()
                        {
                            {
                                "phasedRegistrationNames",
                                new Dictionary<string, string>()
                                {
                                    { "bubbled" , "onEndEditing" },
                                    { "captured" , "onEndEditingCapture" }
                                }
                            }
                        }
                    },
                    {
                        "topFocus",
                        new Dictionary<string, object>()
                        {
                            {
                                "phasedRegistrationNames",
                                new Dictionary<string, string>()
                                {
                                    { "bubbled" , "onFocus" },
                                    { "captured" , "onFocusCapture" }
                                }
                            }
                        }
                    },
                    {
                        "topBlur",
                        new Dictionary<string, object>()
                        {
                            {
                                "phasedRegistrationNames",
                                new Dictionary<string, string>()
                                {
                                    { "bubbled" , "onBlur" },
                                    { "captured" , "onBlurCapture" }
                                }
                            }
                        }
                    },
                };
            }
        }

        /// <summary>
        /// The commands map for the <see cref="ReactTextInputManager"/>.
        /// </summary>
        public override IReadOnlyDictionary<string, object> CommandsMap
        {
            get
            {
                return new Dictionary<string, object>()
                {
                    { "focusTextInput", FocusTextInput },
                    { "blurTextInput", BlurTextInput },
                };
            }
        }

                /// <summary>
        /// Sets the font size on the <see cref="ReactTextBox"/>.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="fontSize">The font size.</param>
        [ReactProp(ViewProps.FontSize)]
        public void SetFontSize(ReactTextBox view, double fontSize)
        {
            Log.Info(ReactConstants.Tag, "SetFontSize:" + fontSize);
            view.FontSize = fontSize;
        }

        /// <summary>
        /// Sets the font color for the node.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="color">The masked color value.</param>
        [ReactProp(ViewProps.Color, CustomType = "Color")]
        public void SetColor(ReactTextBox view, uint? color)
        {
            Log.Info(ReactConstants.Tag, "SetColor:" + Color.FromUint((uint)color));
            view.Color = Color.FromUint((uint)color);       // is the color agreed with 'foreground' color?
        }

        /// <summary>
        /// Sets the font family for the node.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="familyName">The font family.</param>
        [ReactProp(ViewProps.FontFamily)]
        public void SetFontFamily(ReactTextBox view, string familyName)
        {
            view.FontFamily = familyName;
        }

        /// <summary>
        /// Sets the font weight for the node.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="fontWeightString">The font weight string.</param>
        [ReactProp(ViewProps.FontWeight)]
        public void SetFontWeight(ReactTextBox view, string fontWeightString)
        {
            Log.Info(ReactConstants.Tag, "SetFontWeight:" + fontWeightString);
            switch (fontWeightString)
            {
                case "Bold":
                    view.FontWeight = FontWeight.Bold;
                    break;
                case "SemiBold":
                    view.FontWeight = FontWeight.SemiBold;
                    break;
                case "UltraBold":
                    view.FontWeight = FontWeight.UltraBold;
                    break;
                case "Black":
                    view.FontWeight = FontWeight.Black;
                    break;
                case "ExtraBlack":
                    view.FontWeight = FontWeight.ExtraBlack;
                    break;
                case "Book":
                    view.FontWeight = FontWeight.Book;
                    break;
                case "Light":
                    view.FontWeight = FontWeight.Light;
                    break;
                case "Medium":
                    view.FontWeight = FontWeight.Medium;
                    break;
                case "Normal":
                    view.FontWeight = FontWeight.Normal;
                    break;
                case "Thin":
                    view.FontWeight = FontWeight.Thin;
                    break;
                case "UltraLight":
                    view.FontWeight = FontWeight.UltraLight;
                    break;
                default:
                    view.FontWeight = FontWeight.None;
                    break;
            }
        }

        /// <summary>
        /// Sets the font style for the node.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="fontStyleString">The font style string.</param>
        [ReactProp(ViewProps.FontStyle)]
        public void SetFontStyle(ReactTextBox view, string fontStyleString)
        {
            var fontStyle = EnumHelpers.ParseNullable<FontAttributes>(fontStyleString);
            Log.Debug(ReactConstants.Tag, "fontStyle:"+ fontStyle);
            view.FontAttributes = fontStyle ?? FontAttributes.normal;
        }

        /// <summary>
        /// Sets auto focus.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="fontStyleString">The font style string.</param>
        [ReactProp("autoFocus")]
        public void SetAutoFocus(ReactTextBox view, bool bAutoFocus)
        {
            Log.Debug(ReactConstants.Tag, "autoFocus:" + bAutoFocus);
            view.SetFocus(true);
        }

        /// <summary>
        /// Sets whether to track selection changes on the <see cref="ReactTextBox"/>.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="onSelectionChange">The indicator.</param>
        [ReactProp("onSelectionChange", DefaultBoolean = false)]
        public void SetSelectionChange(ReactTextBox view, bool onSelectionChange)
        {
            // NOT SUPPORTED

            /*
            if (onSelectionChange)
            {
                _onSelectionChange = true;
                view.SelectTextOnFocus += OnSelectionChanged;
            }
            else
            {
                _onSelectionChange = false;
                view.SelectionChanged -= OnSelectionChanged;
            }
            */
        }

        /// <summary>
        /// Sets the default text placeholder property on the <see cref="ReactTextBox"/>.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="placeholder">The placeholder text.</param>
        [ReactProp("placeholder")]
        public void SetPlaceholder(ReactTextBox view, string placeholder)
        {
            view.Placeholder = placeholder;
        }

        /// <summary>
        /// Sets the default text placeholder property on the <see cref="ReactTextBox"/>.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="placeholder">The placeholder text.</param>
        [ReactProp("placeholderTextColor", CustomType = "Color")]
        public void SetPlaceholderColor(ReactTextBox view, uint? color)
        {
            view.PlaceholderColor = Color.FromUint((uint)color);
        }

        ///// <summary>
        ///// Sets the border color for the <see cref="ReactTextBox"/>.
        ///// </summary>
        ///// <param name="view">The view instance</param>
        ///// <param name="color">The masked color value.</param>
        //[ReactProp("borderColor", CustomType = "Color")]
        //public void SetBorderColor(ReactTextBox view, uint? color)
        //{
        //    // NOT SUPPORTED

        //    /*
        //    view.BorderBrush = color.HasValue
        //        ? new SolidColorBrush(ColorHelpers.Parse(color.Value))
        //        : new SolidColorBrush(DefaultTextBoxBorder);
        //        */
        //}

        /// <summary>
        /// Sets the background color for the <see cref="ReactTextBox"/>.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="color">The masked color value.</param>
        [ReactProp(ViewProps.BackgroundColor, CustomType = "Color")]
        public void SetBackgroundColor(ReactTextBox view, uint? color)
        {
            view.BackgroundColor = Color.FromUint((uint)color);
        }

        /// <summary>
        /// Sets the selection color for the <see cref="ReactTextBox"/>.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="color">The masked color value.</param>
        [ReactProp("selectionColor", CustomType = "Color")]
        public void SetSelectionColor(ReactTextBox view, uint color)
        {
            // NOT SUPPORTED
        }

        /// <summary>
        /// Sets the text alignment property on the <see cref="ReactTextBox"/>.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="alignment">The text alignment.</param>
        [ReactProp(ViewProps.TextAlign)]
        public void SetTextAlign(ReactTextBox view, string alignment)
        {
            // NOT SUPPORTED
            //view.TextAlignment = EnumHelpers.Parse<TextAlignment>(alignment);
        }

        /// <summary>
        /// Sets the text alignment property on the <see cref="ReactTextBox"/>.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="alignment">The text alignment.</param>
        [ReactProp(ViewProps.TextAlignVertical)]
        public void SetTextVerticalAlign(ReactTextBox view, string alignment)
        {
            // NOT SUPPORTED
            //view.VerticalContentAlignment = EnumHelpers.Parse<VerticalAlignment>(alignment);
        }

        // set single line or multi-line
        [ReactProp("MultiLine")]
        public void SetMulLine(ReactTextBox view, bool bMultiLine)
        {
            view.IsSingleLine = !bMultiLine;
        }

        // set text style
        [ReactProp("secureTextEntry")]
        public void SetPwd(ReactTextBox view, bool bPassword)
        {
            Log.Info("RN", "SetPwd value is " + bPassword.ToString());
            view.IsPassword = bPassword;
        }

        [ReactProp("Scrollable", DefaultBoolean = true)]
        public void SetScrollable(ReactTextBox view, bool bScrollable)
        {
            view.Scrollable = bScrollable;
        }

        /// <summary>
        /// Sets the editablity property on the <see cref="ReactTextBox"/>.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="editable">The editable flag.</param>
        [ReactProp("editable", DefaultBoolean = true)]
        public void SetEditable(ReactTextBox view, bool editable)
        {
            view.IsEnabled = editable;
        }

        /// <summary>
        /// Creates a new view instance of type <see cref="Button"/>.
        /// </summary>
        /// <param name="reactContext">The react context.</param>
        /// <returns>The view instance.</returns>
        protected override ReactTextBox CreateViewInstance(ThemedReactContext reactContext)
        {
            Log.Info(ReactConstants.Tag, "[Views] Enter Create ReactTextBox instance ");

            ReactTextBox txtInput = new ReactTextBox(ReactProgram.RctWindow)
            {
                //Color = Color.Green,
                //Placeholder = "Tell me your name ...",
                //PlaceholderColor = Color.Olive,
                //BackgroundColor = Color.Orange,
                //FontAttributes = FontAttributes.Bold | FontAttributes.Italic
            };

            txtInput.Show();

            Log.Info(ReactConstants.Tag, "[Views] Exit Create ReactTextBox instance ");
            return txtInput;
        }

        /// <summary>
        /// Installing the textchanged event emitter on the <see cref="TextInput"/> Control.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <param name="view">The <see cref="ReactTextBox"/> view instance.</param>
        protected override void AddEventEmitters(ThemedReactContext reactContext, ReactTextBox view)
        {
            base.AddEventEmitters(reactContext, view);

            view.KeyDown += OnKeyDown;
            view.Focused += OnGotFocus;
            view.Unfocused += OnLostFocus;
            view.CursorChanged += OnCursorChanged;
            view.ChangedByUser += OnTextChanged;

            // Temp Solution, CursorChanged's supporting will be better.
            view.CursorChanged += OnTextChanged;
        }

        /// <summary>
        /// Called when view is detached from view hierarchy and allows for 
        /// additional cleanup by the <see cref="ReactTextInputManager"/>.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <param name="view">The view.</param>
        public override void OnDropViewInstance(ThemedReactContext reactContext, ReactTextBox view)
        {
            base.OnDropViewInstance(reactContext, view);

            view.KeyDown -= OnKeyDown;
            view.Focused -= OnGotFocus;
            view.Unfocused -= OnLostFocus;
            view.CursorChanged -= OnCursorChanged;
            view.ChangedByUser -= OnTextChanged;

            // Temp Solution, CursorChanged's supporting will be better.
            view.CursorChanged -= OnTextChanged;
        }

        private void OnCursorChanged(object sender, EventArgs e)
        {
            Log.Info(ReactConstants.Tag, "### Invoked -> OnCursorChanged ###");
            var textBox = (ReactTextBox)sender;
        }

        private void OnTextChanging(object sender, EventArgs e)
        {
            Log.Info(ReactConstants.Tag, "### Invoked -> OnTextChanging ###");
            var textBox = (ReactTextBox)sender;
            //textBox.IncrementEventCount();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            var textBox = (ReactTextBox)sender;

            if ( false == textBox.preDecoratedText.Equals(textBox.DecoratedText) )
            {
                Log.Info(ReactConstants.Tag, "### OnTextChanged  ###  PreText = '" + textBox.preDecoratedText + "', CurText = '" + textBox.DecoratedText + "'");
                textBox.GetReactContext()
                    .GetNativeModule<UIManagerModule>()
                    .EventDispatcher
                    .DispatchEvent(
                        new ReactTextChangedEvent(
                            textBox.GetTag(),
                            textBox.DecoratedText,
                            textBox.MinimumWidth,
                            textBox.MinimumHeight,
                            /*textBox.CurrentEventCount*/0));
            }
            else
            {
                Log.Info(ReactConstants.Tag, "### Ignored, Text doesn't changed, maybe only the cursor moved ###");
            }
        }

        private void OnGotFocus(object sender, EventArgs e)
        {
            Log.Info(ReactConstants.Tag, "### Invoked -> OnGotFocus ###");
            var textBox = (ReactTextBox)sender;
            textBox.GetReactContext()
                .GetNativeModule<UIManagerModule>()
                .EventDispatcher
                .DispatchEvent(
                    new ReactTextInputFocusEvent(textBox.GetTag()));

            // show panel
            //textBox.SetInputPanelEnabled(true);
        }

        private void OnLostFocus(object sender, EventArgs e)
        {
            Log.Info(ReactConstants.Tag, "### Invoked -> OnLostFocus ###");
            var textBox = (ReactTextBox)sender;
            var eventDispatcher = textBox.GetReactContext()
                .GetNativeModule<UIManagerModule>()
                .EventDispatcher;

            eventDispatcher.DispatchEvent(
                new ReactTextInputBlurEvent(textBox.GetTag()));

            eventDispatcher.DispatchEvent(
                new ReactTextInputEndEditingEvent(
                      textBox.GetTag(),
                      textBox.Text));

            // hide panel
            // textBox.SetInputPanelEnabled(false);

        }

        private void OnKeyDown(object sender, EventArgs e)
        {
            Log.Info(ReactConstants.Tag, "### Invoked -> OnKeyDown ###");
            /*
            if (e.Key == VirtualKey.Enter)
            {
                var textBox = (ReactTextBox)sender;
                if (!textBox.AcceptsReturn)
                {
                    e.Handled = true;
                    textBox.GetReactContext()
                        .GetNativeModule<UIManagerModule>()
                        .EventDispatcher
                        .DispatchEvent(
                            new ReactTextInputSubmitEditingEvent(
                                textBox.GetTag(),
                                textBox.Text));
                }
            }
            */
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            Log.Info(ReactConstants.Tag, "### Invoked -> OnSelectionChanged ###");
            /*
            var textBox = (ReactTextBox)sender;
            var start = textBox.SelectionStart;
            var length = textBox.SelectionLength;
            textBox.GetReactContext()
                .GetNativeModule<UIManagerModule>()
                .EventDispatcher
                .DispatchEvent(
                    new ReactTextInputSelectionEvent(
                        textBox.GetTag(),
                        start,
                        start + length));
                        */
        }
        
    }
}