using System;
//using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using ReactNative.UIManager.Events;

using ElmSharp;
using ReactNativeTizen.ElmSharp.Extension;
using Tizen;
using ReactNative.Common;

//using System.Threading;

namespace ReactNative.Views.Grid
{
    public class ReactGridManager : SimpleViewManager<GenGrid>
    {
        private const string ReactClass = "RCTGridView";

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
        /// The view manager event constants.
        /// </summary>
        public override IReadOnlyDictionary<string, object> ExportedCustomDirectEventTypeConstants
        {
            get
            {
                return new Dictionary<string, object>
                {
                    {
                        "topSelectedChange",
                        new Dictionary<string, object>
                        {
                            { "registrationName", "onSelectedChange" }
                        }
                    },
                };
            }
        }

        [ReactProp("horizontal")]
        public void IsHorizatal(GenGrid view, bool bHorizontal)
        {
            Log.Info(ReactConstants.Tag, $"[Views::Grid] set Horizatal:'{bHorizontal}'");

            view.IsHorizontal = bHorizontal;
        }

        [ReactProp("selected")]
        public void SetSelected(GenGrid view, int index)
        {
            Log.Info(ReactConstants.Tag, $"[Views::Grid] Set Selected:'{index}'");

            //view.SetItemSelected(view.GetGridItemById(index), true);  
        }

        [ReactProp("GridItemList")]
        public void ItemList(GenGrid view, JArray imgArray)
        {
            Log.Info(ReactConstants.Tag, "=== Enter ItemList ===");

            foreach (var item in imgArray)
            {
                Log.Info(ReactConstants.Tag, "[GenGrid]  title: "+ item["title"].ToString()+" content:" + item["content"].ToString());
                GenItemClass defaultClass = new GenItemClass("default")
                {
                    GetTextHandler = (obj, part) =>
                    {
                        Log.Info(ReactConstants.Tag, "[GenGrid] img title: " + item["title"].ToString());
                        return item["title"].ToString();
                    },
                    GetContentHandler = (obj, part) =>
                    {
                        if (part == "elm.swallow.icon")
                        {
                            Log.Info(ReactConstants.Tag, "[GenGrid] img path: " + item["content"].ToString());

                            Image img = new Image(ReactProgram.RctWindow);
                            img.Load(item["content"].ToString());

                            img.MinimumWidth = view.ItemWidth;
                            img.MinimumHeight = view.ItemHeight;

                            return img;
                        }

                        return null;
                    }
                };

                Log.Info(ReactConstants.Tag, "grid.Append -> image class");
                view.Append(defaultClass, null);

                Log.Info(ReactConstants.Tag, "ItemStyle: " + defaultClass.ItemStyle.ToString());
            }

            Log.Info(ReactConstants.Tag, "=== Exit ItemList ===");

        }

        // NOT SUPPORTED
        /*
        [ReactProp("ItemDir")]
        public void ItemDir(GenGrid view, string imgPath)
        {
            Log.Info(ReactConstants.Tag, "[Views::Grid] ItemDir: " + imgPath );
        }
        */

        /// <summary>
        /// Receive events/commands directly from JavaScript through the 
        /// <see cref="UIManagerModule"/>.
        /// </summary>
        /// <param name="view">
        /// The view instance that should receive the command.
        /// </param>
        /// <param name="commandId">Identifer for the command.</param>
        /// <param name="args">Optional arguments for the command.</param>
        public override void ReceiveCommand(GenGrid view, int commandId, JArray args)
        {
            // TODO: parse command & change view
            Log.Info(ReactConstants.Tag, "[Views] ReactGridViewManager::ReceiveCommand");
        }

        [ReactProp("highlight")]
        public void IsHighlight(GenGrid view, bool bHighlight)
        {
            Log.Info(ReactConstants.Tag, $"[Views::Grid] IsHighlight:'{bHighlight}'");

            view.IsHighlight = bHighlight;
        }

        [ReactProp("alignmentX")]
        public void SetAlignmentX(GenGrid view, double alignmentX)
        {
            Log.Info(ReactConstants.Tag, $"[Views::Grid] SetAlignmentX:'{alignmentX}'");

            view.AlignmentX = alignmentX;
        }

        [ReactProp("alignmentY")]
        public void SetAlignmentY(GenGrid view, double AlignmentY)
        {
            Log.Info(ReactConstants.Tag, $"[Views::Grid] SetAlignmentY:'{AlignmentY}'");

            view.AlignmentY = AlignmentY;
        }

        [ReactProp("weightX")]
        public void SetWeightX(GenGrid view, double weightX)
        {
            Log.Info(ReactConstants.Tag, $"[Views::Grid] SetWeightX:'{weightX}'");

            view.WeightX = weightX;
        }

        [ReactProp("weightY")]
        public void SetWeightY(GenGrid view, double weightY)
        {
            Log.Info(ReactConstants.Tag, $"[Views::Grid] SetWeightY:'{weightY}'");

            view.WeightY = weightY;
        }

        [ReactProp("itemAlignmentX")]
        public void SetItemAlignmentX(GenGrid view, double itemAlignmentX)
        {
            Log.Info(ReactConstants.Tag, $"[Views::Grid] SetItemAlignmentX:'{itemAlignmentX}'");

            view.ItemAlignmentX = itemAlignmentX;
        }

        [ReactProp("itemAlignmentY")]
        public void SetItemAlignmentY(GenGrid view, double itemAlignmentY)
        {
            Log.Info(ReactConstants.Tag, $"[Views::Grid] SetItemAlignmentY:'{itemAlignmentY}'");

            view.ItemAlignmentY = itemAlignmentY;
        }

        [ReactProp("itemWidth")]
        public void SetItemWidth(GenGrid view, int itemWidth)
        {
            Log.Info(ReactConstants.Tag, $"[Views::Grid] SetItemAlignmentY:'{itemWidth}'");

            view.ItemWidth = itemWidth;
        }

        [ReactProp("itemHeight")]
        public void SetItemHeight(GenGrid view, int itemHeight)
        {
            Log.Info(ReactConstants.Tag, $"[Views::Grid] SetItemAlignmentY:'{itemHeight}'");

            view.ItemHeight = itemHeight;
        }

        /// <summary>
        /// Creates a new view instance of type <see cref="Button"/>.
        /// </summary>
        /// <param name="reactContext">The react context.</param>
        /// <returns>The view instance.</returns>
        protected override GenGrid CreateViewInstance(ThemedReactContext reactContext)
        {
            Log.Info(ReactConstants.Tag, "[Views] ReactGridViewManager::CreateViewInstance BGN ");

            // create view component & set basic prop
            GenGrid grid = new GenGrid(ReactProgram.RctWindow)
            {
                //IsEnabled = true,
            };
            grid.Show();

            return grid;
        }

        /// <summary>
        /// Subclasses can override this method to install custom event 
        /// emitters on the given view.
        /// </summary>
        /// <param name="reactContext">The react context.</param>
        /// <param name="view">The view instance.</param>
        protected override void AddEventEmitters(ThemedReactContext reactContext, GenGrid view)
        {
            Log.Info(ReactConstants.Tag, "[Views] Register custom event , view:" + view );

            view.ItemSelected += OnItemSelected;
            view.ItemUnselected += OnItemUnselected;
            view.ItemRealized += OnItemRealized;
            view.ItemUnrealized += OnItemUnrealized;
            view.ItemPressed += OnItemPressed;
            view.ItemReleased += OnItemReleased;
            view.ItemLongPressed += OnItemLongPressed;
        }

        /// <summary>
        /// Called when view is detached from view hierarchy and allows for 
        /// additional cleanup by the <see cref="ReactGridManager"/>.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <param name="view">The view.</param>
        public override void OnDropViewInstance(ThemedReactContext reactContext, GenGrid view)
        {
            base.OnDropViewInstance(reactContext, view);

            view.ItemSelected -= OnItemSelected;
            view.ItemUnselected -= OnItemUnselected;
            view.ItemRealized -= OnItemRealized;
            view.ItemUnrealized -= OnItemUnrealized;
            view.ItemPressed -= OnItemPressed;
            view.ItemReleased -= OnItemReleased;
            view.ItemLongPressed -= OnItemLongPressed;
        }


        /// <summary>
        /// Selection changed event handler.
        /// </summary>
        /// <param name="sender">an event sender.</param>
        /// <param name="e">the event.</param>
        private void OnSelectionChanged(object sender, GenGridItemEventArgs e)
        {
            var grid = (GenGrid)sender;

            GenGridItem item = (GenGridItem)e.Item;
            if (item == null)
            {
                return;
            }

            Log.Info(ReactConstants.Tag, "[Views] ## Notify to JS ##  Item:[" + item.Id + "]" + " was selected!");

            // emit event to JS
            var reactContext = grid.GetReactContext();
            reactContext.GetNativeModule<UIManagerModule>()
                .EventDispatcher
                .DispatchEvent(
                    new ReactGridViewEvent(
                        grid.GetTag(),
                        item.Id));
        }

        private void OnItemDoubleClicked(object sender, GenGridItemEventArgs e)
        {
            // TODO: Not Supported
            Log.Info(ReactConstants.Tag, " Encounter OnItemDoubleClicked Event ");
        }

        private void OnItemLongPressed(object sender, GenGridItemEventArgs e)
        {
            // TODO: any extended animation here?
            Log.Info(ReactConstants.Tag, " Encounter OnItemLongPressed Event ");
        }

        private void OnItemReleased(object sender, GenGridItemEventArgs e)
        {
            // TODO:Not Supported
            Log.Info(ReactConstants.Tag, " Encounter OnItemReleased Event ");
        }

        private void OnItemPressed(object sender, GenGridItemEventArgs e)
        {
            // TODO: see 'OnItemSelected'
            Log.Info(ReactConstants.Tag, " Encounter OnItemPressed Event ");
        }

        private void OnItemSelected(object sender, GenGridItemEventArgs e)
        {
            Log.Info(ReactConstants.Tag, " Encounter OnItemSelected Event ");
            OnSelectionChanged(sender, e);
        }

        private void OnItemUnselected(object sender, GenGridItemEventArgs e)
        {
            Log.Info(ReactConstants.Tag, " Encounter OnItemUnselected Event ");
        }

        private void OnItemRealized(object sender, GenGridItemEventArgs e)
        {
            Log.Info(ReactConstants.Tag, " Encounter OnItemRealized Event ");
        }

        private void OnItemUnrealized(object sender, GenGridItemEventArgs e)
        {
            Log.Info(ReactConstants.Tag, " Encounter OnItemUnrealized Event ");
        }

        private void OnItemActivated(object sender, GenGridItemEventArgs e)
        {
            Log.Info(ReactConstants.Tag, " Encounter OnItemActivated Event ");
        }
    }

    /* Event for Grid */
    class ReactGridViewEvent : Event
    {
        private readonly int _index;

        public ReactGridViewEvent(int viewTag, int index)
            : base(viewTag, TimeSpan.FromTicks(Environment.TickCount))
        {
            _index = index;
        }

        public override string EventName
        {
            get
            {
                return "topSelectedChange";
            }
        }

        public override void Dispatch(RCTEventEmitter eventEmitter)
        {
            var eventData = new JObject
            {
                { "target", ViewTag },
                { "value", _index },
            };

            Log.Info(ReactConstants.Tag, "[Views] Dispatch Event >> name:Select, viewTag:" + ViewTag + ", cur postion:" + _index);
            eventEmitter.receiveEvent(ViewTag, EventName, eventData);
        }
    }
}
