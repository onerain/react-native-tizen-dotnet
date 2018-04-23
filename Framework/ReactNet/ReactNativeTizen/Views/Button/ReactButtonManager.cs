using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using ReactNative.Common;
//using ReactNative.UIManager.Events;

using Tizen;

using ElmSharp;
using ReactNativeTizen.ElmSharp.Extension;

namespace ReactNative.Views.ReactButton
{
    //public class ReactButtonManager : SimpleViewManager<Button>
    public class ReactButtonManager : BaseViewManager<Button, ReactButtonShadowNode>
    {
        private const int sampleCmd1 = 1;
        private const int sampleCmd2 = 2;

        private const string ReactClass = "RCTButton";

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

        public override IReadOnlyDictionary<string, object> CommandsMap
        {
            get
            {
                return new Dictionary<string, object>
                {
                    { "sampleCmd1", sampleCmd1 },
                    { "sampleCmd2", sampleCmd2 },
                };
            }
        }

        /// <summary>
        /// The exported custom direct event types.
        /// </summary>
        public override IReadOnlyDictionary<string, object> ExportedCustomDirectEventTypeConstants
        {
            get
            {
                return new Dictionary<string, object>
                {
                    {
                        "topPress",
                        new Dictionary<string, object>
                        {
                            { "registrationName", "onPress" },
                        }
                    }
                };
            }
        }
/*
        [ReactProp("title")]
        public void SetTitle(Button view, string title)
        {
            Log.Info(ReactConstants.Tag, "## SetTitle ## view=" + view + ", tag=" + view.GetTag() + ", title=" + title);
            view.Text = title;
        }
*/
        [ReactProp("disabled", DefaultBoolean=false)]
        public void SetDisabled(Button view, bool disable)
        {
            Log.Info(ReactConstants.Tag, "## SetDisabled ## view=" + view + ", tag=" + view.GetTag() + ", disable=" + disable);
            view.IsEnabled = !disable;
            //view.AllowFocus(!disable);
            //view.Color = Color.Gray;
        }

        [ReactProp("image")]
        public void SetImage(Button view, string path)
        {
        }

        /// <summary>
        /// Sets the font color for the node.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="color">The masked color value.</param>
        [ReactProp(ViewProps.Color, CustomType = "Color")]
        public void SetColor(Button view, uint? color)
        {
            if (color.HasValue)
            {
                var c = ColorHelpers.Parse(color.Value);
                view.Color = c;

                Tracing.RNTracer.Write(Common.ReactConstants.Tag, "SetColor = " + c.ToString() + ", from " + color.Value);
            }
        }

        /// <summary>
        /// Receive events/commands directly from JavaScript through the 
        /// <see cref="UIManagerModule"/>.
        /// </summary>
        /// <param name="view">
        /// The view instance that should receive the command.
        /// </param>
        /// <param name="commandId">Identifer for the command.</param>
        /// <param name="args">Optional arguments for the command.</param>
        public override void ReceiveCommand(Button view, int commandId, JArray args)
        {
            // TODO: parse command & change view
            Log.Info(ReactConstants.Tag, "[Views] ReactButtonManager::ReceiveCommand ");

            switch (commandId)
            {
                case sampleCmd1:
                    Log.Info(ReactConstants.Tag, "[Views] ReactButtonManager::ReceiveCommand # One # args1:" + args.First.Value<string>());

                    // TODO: according to the cmd, change prop view.

                    break;

                case sampleCmd2:
                    Log.Info(ReactConstants.Tag, "[Views] ReactButtonManager::ReceiveCommand # Two # args1:" + args.First.Value<string>());

                    // TODO: according to the cmd, change prop view.

                    break;

                default:
                    Log.Info(ReactConstants.Tag, "[Views] UnSupported Command ");
                    break;
            }


        }

        /// <summary>
        /// Receive extra updates from the shadow node.
        /// </summary>
        /// <param name="root">The root view.</param>
        /// <param name="extraData">The extra data.</param>
        public override void UpdateExtraData(Button root, object extraData)
        {
            var update = extraData as ReactButtonUpdate;
            root.Text = update.getText();
            Log.Info(ReactConstants.Tag, "UpdateExtraData:" + update.getText());
        }

        /// <summary>
        /// Creates the shadow node instance.
        /// </summary>
        /// <returns>The shadow node instance.</returns>
        public override ReactButtonShadowNode CreateShadowNodeInstance()
        {
            Log.Info(ReactConstants.Tag, "[Views] ReactButtonManager::CreateShadowNodeInstance");
            return new ReactButtonShadowNode();
        }

        /// <summary>
        /// Creates a new view instance of type <see cref="Button"/>.
        /// </summary>
        /// <param name="reactContext">The react context.</param>
        /// <returns>The view instance.</returns>
        protected override Button CreateViewInstance(ThemedReactContext reactContext)
        {
            Log.Info(ReactConstants.Tag, "[Views] ReactButtonManager::CreateViewInstance ");

            Button btn = new Button(ReactProgram.RctWindow);

            btn.Show();

            return btn;
        }

        /// <param name="reactContext">The React context.</param>
        /// <param name="view">The view.</param>
        public override void OnDropViewInstance(ThemedReactContext reactContext, Button view)
        {
            Log.Info(ReactConstants.Tag, "[Views] OnDropViewInstance Button:" + view);
            view.Clicked -= OnClicked;
        }

        /// <summary>
        /// Subclasses can override this method to install custom event 
        /// emitters on the given view.
        /// </summary>
        /// <param name="reactContext">The react context.</param>
        /// <param name="view">The view instance.</param>
        protected override void AddEventEmitters(ThemedReactContext reactContext, Button view)
        {
            Log.Info(ReactConstants.Tag, "[Views] AddEventEmitters FOR Button:" + view);
            view.Focused += OnFocused;
            view.Clicked += OnClicked;
        }

        private void OnClicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            Log.Info(ReactConstants.Tag, $"[Button] Event:[Clicked] Data:[tag:{btn.GetTag()}] ");
            btn.GetReactContext()
                .GetNativeModule<UIManagerModule>()
                .EventDispatcher
                .DispatchEvent(
                    new ReactButtonClickEvent(btn.GetTag()));
        }

        private void OnFocused(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            Log.Info(ReactConstants.Tag, $"[Button] Event:[Focused] Data:[tag:{btn.GetTag()}] ");
            btn.GetReactContext()
                .GetNativeModule<UIManagerModule>()
                .EventDispatcher
                .DispatchEvent(
                    new ReactButtonFocusedEvent(btn.GetTag()));
        }

    }
}
