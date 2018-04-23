using Newtonsoft.Json.Linq;
using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using ReactNative.UIManager.Events;
using System;

using ElmSharp;
using ReactNativeTizen.ElmSharp.Extension;

namespace ReactNative.Views.Picker
{
    /// <summary>
    /// A view manager responsible for rendering picker.
    /// </summary>
    public class ReactPickerManager : SimpleViewManager<ReactPicker>
    { 
        /// <summary>
        /// The name of the view manager.
        /// </summary>
        public override string Name
        {
            get
            {
                return "RCTPicker";
            }
        }

        /// <summary>
        /// Populates a <see cref="ReactPicker"/>
        /// </summary>
        /// <param name="view">a ReactPicker instance.</param>
        /// <param name="title">The picker title.</param>
        [ReactProp("title")]
        public void SetTitle(ReactPicker view, string title)
        {
            view.Text = title;
        }

        [ReactProp("enabled", DefaultBoolean=true)]
        public void SetEnabled(ReactPicker view, bool enabled)
        {
            view.IsEnabled = enabled;
        }

        /// <summary>
        /// Sets the font color for the node.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="color">The masked color value.</param>
        [ReactProp(ViewProps.Color, CustomType = "Color")]
        public void SetColor(ReactPicker view, uint? color)
        {
            if (color.HasValue)
            {
                var c = ColorHelpers.Parse(color.Value);
                view.Color = c;
            }
        }

        /// <summary>
        /// Populates a <see cref="ReactPicker"/>
        /// </summary>
        /// <param name="view">a ReactPicker instance.</param>
        /// <param name="items">The picker items.</param>
        [ReactProp("items")]
        public void SetItems(ReactPicker view, JArray items)
        {
            // Temporarily disable selection changed event handler.
            view.ItemSelected -= OnSelectionChanged;

            view.ClearPlus();

            foreach (var itemToken in items)
            {
                var itemData = (JObject)itemToken;
                var label = itemData.Value<string>("label");
                if (label != null)
                {
                    view.AddItemPlus(label);
                }
            }
             
            view.ItemSelected += OnSelectionChanged;
        }

        /// <summary>
        /// Implement this method to receive optional extra data enqueued from
        /// the corresponding instance of <see cref="ReactShadowNode"/> in
        /// <see cref="ReactShadowNode.OnCollectExtraUpdates"/>.
        /// </summary>
        /// <param name="root">The root view.</param>
        /// <param name="extraData">The extra data.</param>
        public override void UpdateExtraData(ReactPicker root, object extraData)
        {
        }

        /// <summary>
        /// Called when view is detached from view hierarchy and allows for 
        /// additional cleanup by the <see cref="ReactPickerManager"/>.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <param name="view">The view.</param>
        public override void OnDropViewInstance(ThemedReactContext reactContext, ReactPicker view)
        {
            base.OnDropViewInstance(reactContext, view);
            view.ItemSelected -= OnSelectionChanged;
        }
  
        /// <summary>
        /// Creates a new view instance of type <see cref="ReactPicker"/>.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <returns>The view instance.</returns>
        protected override ReactPicker CreateViewInstance(ThemedReactContext reactContext)
        {
            var picker =  new ReactPicker(ReactProgram.RctWindow){
                HoverParent = ReactProgram.RctWindow,
                AutoUpdate = true
            };

            picker.Show();

            return picker;
        }

        /// <summary>
        /// Subclasses can override this method to install custom event 
        /// emitters on the given view.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <param name="view">The view instance.</param>
        protected override void AddEventEmitters(ThemedReactContext reactContext, ReactPicker view)
        {
            base.AddEventEmitters(reactContext, view);
            view.ItemSelected += OnSelectionChanged;
        }

        /// <summary>
        /// Selection changed event handler.
        /// </summary>
        /// <param name="sender">an event sender.</param>
        /// <param name="e">the event.</param>
        private void OnSelectionChanged(object sender, HoverselItemEventArgs e)
        {
            var hoversel = (ReactPicker)sender;
            var reactContext = hoversel.GetReactContext();
            reactContext.GetNativeModule<UIManagerModule>()
                .EventDispatcher
                .DispatchEvent(
                    new ReactPickerEvent(
                        hoversel.GetTag(),
                        hoversel.GetIndex(e.Item.Id)));
        }

        /// <summary>
        /// A picker specific event.
        /// </summary>
        class ReactPickerEvent : Event
        {
            private readonly int _selectedIndex;

            public ReactPickerEvent(int viewTag, int selectedIndex)
                : base(viewTag, TimeSpan.FromTicks(Environment.TickCount))
            {
                _selectedIndex = selectedIndex;
            }

            public override string EventName
            {
                get
                {
                    return "topSelect";
                }
            }

            public override void Dispatch(RCTEventEmitter eventEmitter)
            {
                var eventData = new JObject
                {
                    { "target", ViewTag },
                    { "position", _selectedIndex },
                };

                eventEmitter.receiveEvent(ViewTag, EventName, eventData);
            }
        }
    }
}