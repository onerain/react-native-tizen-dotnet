using System;
using Newtonsoft.Json.Linq;

//using ReactNative.Reflection;
using ReactNative.Common;
using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using ReactNative.Modules.Core;
using ReactNativeTizen.ElmSharp.Extension;
using ElmSharp;
using Tizen;

namespace ReactNative.Views.View
{
    /// <summary>
    /// View manager for React view instances, 
    /// TODO: extend border props - BOY.YANG
    /// </summary>
    public class ReactViewManager : BoxViewParentManager<ReactViewBox>
    {
        /// <summary>
        /// The name of this view manager. This will be the name used to 
        /// reference this view manager from JavaScript.
        /// </summary>
        public override string Name
        {
            get
            {
                return "RCTView";
            }
        }

        /// <summary>
        /// Sets the background color for the <see cref="ReactTextBox"/>.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="color">The masked color value.</param>
        [ReactProp(ViewProps.BackgroundColor, CustomType = "Color")]
        public void SetBackgroundColor(ReactViewBox view, uint? color)
        {
            Log.Info(ReactConstants.Tag, "SetBackgroundColorSetBackgroundColorSetBackgroundColor");
            if (color.HasValue)
            {
                var c = ColorHelpers.Parse(color.Value);
                //Log.Info(ReactConstants.Tag, "SetBackgroundColor:" + c);
                view.BackgroundColor = c;
            }
            else
            {
                var c = Color.FromRgba(255,255,255,255);
                view.BackgroundColor = c;
            }
        }

        /// <summary>
        /// Sets whether <see cref="ReactTextBox"/> is selectable.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="isTVSelectable">The flag of whether can be selected.</param>
        [ReactProp("isTVSelectable")]
        public void SetTVSelectable(ReactViewBox view, bool isTVSelectable)
        {
            view.Touchable = isTVSelectable;
            if(isTVSelectable)
            {
                view.FocusEventHandler += OnFocus;
                view.BlurEventHandler += OnBlur;
                view.PressEventHandler += OnSelect;
            }
            else
            {
                view.FocusEventHandler -= OnFocus;
                view.BlurEventHandler -= OnBlur;
                view.PressEventHandler -= OnSelect;
            }
        }

        /// <summary>
        /// Sets whether <see cref="ReactTextBox"/> is collapsible.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="isCollapsible">The flag of whether is collapsible.</param>
        [ReactProp(ViewProps.Collapsible)]
        public void setCollapsable(ReactViewBox view, bool isCollapsible)
        {
            // no-op: it's here only so that "collapsable" property is exported to JS. The value is actually
            // handled in NativeViewHierarchyOptimizer
        }

        /*
        /// <summary>
        /// Sets whether or not the view is an accessibility element.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="accessible">A flag indicating whether or not the view is an accessibility element.</param>
        [ReactProp("accessible")]
        public void SetAccessible(Border view, bool accessible)
        {
            // TODO: #557 Provide implementation for View's accessible prop

            // We need to have this stub for this prop so that Views which
            // specify the accessible prop aren't considered to be layout-only.
            // The proper implementation is still to be determined.
        }

        /// <summary>
        /// Set the pointer events handling mode for the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="pointerEventsValue">The pointerEvents mode.</param>
        [ReactProp(ViewProps.PointerEvents)]
        public void SetPointerEvents(Border view, string pointerEventsValue)
        {
            var pointerEvents = EnumHelpers.ParseNullable<PointerEvents>(pointerEventsValue) ?? PointerEvents.Auto;
            view.SetPointerEvents(pointerEvents);
        }
        */

        /// <summary>
        /// Called when view is detached from view hierarchy and allows for 
        /// additional cleanup by the <see cref="ReactTextInputManager"/>.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <param name="view">The view.</param>
        public override void OnDropViewInstance(ThemedReactContext reactContext, ReactViewBox view)
        {
            base.OnDropViewInstance(reactContext, view);

            if(view.Touchable)
            {
                view.Touchable = false;
                view.FocusEventHandler -= OnFocus;
                view.BlurEventHandler -= OnBlur;
                view.PressEventHandler -= OnSelect;
            }
        }

        /// <summary>
        /// Creates a new view instance of type <see cref="Canvas"/>.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <returns>The view instance.</returns>
        protected override ReactViewBox CreateViewInstance(ThemedReactContext reactContext)
        {
            var box = new ReactViewBox(reactContext);
            box.Show();
            return box;
        }

        private void OnFocus(object sender, EventArgs e)
        {
            var box = (ReactViewBox)sender;
            Log.Info(ReactConstants.Tag, $"tag={box.GetTag()} is Focus.");
            box.GetReactContext()
                .GetJavaScriptModule<RCTDeviceEventEmitter>()
                .emit("onTVNavEvent", new JObject()
                    {
                        { "tag", box.GetTag() },
                        { "eventType", "focus" },
                    });
        }

        private void OnBlur(object sender, EventArgs e)
        {
            var box = (ReactViewBox)sender;
            Log.Info(ReactConstants.Tag, $"tag={box.GetTag()} is Blur.");
            box.GetReactContext()
                .GetJavaScriptModule<RCTDeviceEventEmitter>()
                .emit("onTVNavEvent", new JObject()
                    {
                        { "tag", box.GetTag() },
                        { "eventType", "blur" },
                    });
        }

        private void OnSelect(object sender, EventArgs e)
        {
            var box = (ReactViewBox)sender;
            Log.Info(ReactConstants.Tag, $"tag={box.GetTag()} is select.");
            box.GetReactContext()
                .GetJavaScriptModule<RCTDeviceEventEmitter>()
                .emit("onTVNavEvent", new JObject()
                    {
                        { "tag", box.GetTag() },
                        { "eventType", "select" },
                    });
        }
    }
}
