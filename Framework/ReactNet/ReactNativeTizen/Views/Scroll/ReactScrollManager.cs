using Newtonsoft.Json.Linq;
using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using ReactNative.UIManager.Events;
using System;
using System.Collections.Generic;

using static System.FormattableString;


using ElmSharp;
using ReactNativeTizen.ElmSharp.Extension;

using Tizen;
using ReactNative.Common;

namespace ReactNative.Views.Scroll
{
    /// <summary>
    /// The view manager for scrolling views.
    /// </summary>
    public class ReactScrollManager : ViewParentManager<Scroller>
    {
        private const int CommandScrollTo = 1;

        private const int CommandScrollToEnd = 2;

        private IDictionary<Scroller, ScrollViewerData> _scrollViewerData =
            new Dictionary<Scroller, ScrollViewerData>();

        //private readonly IDictionary<int, Widget> _scrollViewer = new Dictionary<int, Widget>();

        /// <summary>
        /// The name of the view manager.
        /// </summary>
        public override string Name
        {
            get
            {
                return "RCTScrollView";
            }
        }

        /// <summary>
        /// The commands map for the view manager.
        /// </summary>
        public override IReadOnlyDictionary<string, object> CommandsMap
        {
            get
            {
                return new Dictionary<string, object>
                {
                    { "scrollTo", CommandScrollTo },
                    {"scrollToEnd", CommandScrollToEnd}
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
                        ScrollEventType.BeginDrag.GetJavaScriptEventName(),
                        new Dictionary<string, object>
                        {
                            { "registrationName", "onScrollBeginDrag" },
                        }
                    },
                    {
                        ScrollEventType.EndDrag.GetJavaScriptEventName(),
                        new Dictionary<string, object>
                        {
                            { "registrationName", "onScrollEndDrag" },
                        }
                    },
                    {
                        ScrollEventType.Scroll.GetJavaScriptEventName(),
                        new Dictionary<string, object>
                        {
                            { "registrationName", "onScroll" },
                        }
                    }
                };
            }
        }

        /// <summary>
        /// Sets the background color of the view.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="color">The masked color value.</param>
        [ReactProp(
            ViewProps.BackgroundColor,
            CustomType = "Color",
            DefaultUInt32 = ColorHelpers.Transparent)]
        //public void SetBackgroundColor(Scroller view, uint color)
        public void SetBackgroundColor(Scroller view, uint color)
        {
            view.BackgroundColor = Color.FromUint(color);
        }

        /// <summary>
        /// Sets whether scroll is enabled on the view.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="enabled">The enabled value.</param>
        [ReactProp("scrollEnabled", DefaultBoolean = true)]
        public void SetEnabled(Scroller view, bool enabled)
        {
            view.IsEnabled = enabled;
        }

        [ReactProp("pagingEnabled", DefaultBoolean = false)]
        public void SetPagingEnabled(Scroller view, bool enabled)
        {
            view.HorizontalSnap = enabled;
        }

        [ReactProp("HorizontalPageScrollLimit", DefaultInt32 = 1)]
        public void SetHorizontalPageScrollLimit(Scroller view, int horizontalPageLimit)
        {
            view.HorizontalPageScrollLimit = horizontalPageLimit;
        }

        [ReactProp("VerticalPageScrollLimit", DefaultInt32 = 1)]
        public void SetVerticalPageScrollLimit(Scroller view, int verticalPageScrollLimit)
        {
            view.VerticalPageScrollLimit = verticalPageScrollLimit;
        }

        /// <summary>
        /// Sets whether horizontal scroll is enabled on the view.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="horizontal">
        /// The flag signaling whether horizontal scrolling is enabled.
        /// </param>
        [ReactProp("horizontal", DefaultBoolean = false)]
        public void SetHorizontal(Scroller view, bool horizontal)
        {
            if ( true == horizontal )
            {
                view.ScrollBlock = ScrollBlock.Horizontal;
            }
            else
            {
                view.ScrollBlock = ScrollBlock.Vertical;
            }
        }

        /// <summary>
        /// Sets whether the horizontal scroll indicator is shown.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="showIndicator">
        /// The value to show the indicator or not.
        /// </param>
        [ReactProp("showsHorizontalScrollIndicator")]
        public void SetShowsHorizontalScrollIndicator(Scroller view, bool showIndicator)
        {
            if ( true == showIndicator )
            {
                view.HorizontalScrollBarVisiblePolicy = ScrollBarVisiblePolicy.Visible;
            }
            else
            {
                view.HorizontalScrollBarVisiblePolicy = ScrollBarVisiblePolicy.Invisible;
            }
        }

        /// <summary>
        /// Sets whether the vertical scroll indicator is shown.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="showIndicator">
        /// The value to show the indicator or not.
        /// </param>
        [ReactProp("showsVerticalScrollIndicator")]
        public void SetShowsVerticalScrollIndicator(Scroller view, bool showIndicator)
        {
            if (true == showIndicator)
            {
                view.VerticalScrollBarVisiblePolicy = ScrollBarVisiblePolicy.Visible;
            }
            else
            {
                view.VerticalScrollBarVisiblePolicy = ScrollBarVisiblePolicy.Invisible;
            }
        }

        /// <summary>
        /// Sets the content offset of the scroll view.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="contentOffset">The content offset.</param>
        [ReactProp("contentOffset")]
        public void SetContentOffset(Scroller view, JObject contentOffset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the minimum zoom scale for the view.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="zoomScale">The zoom scale.</param>
        [ReactProp("minimumZoomScale")]
        public void SetMinimumZoomScale(Scroller view, float? zoomScale)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the maximum zoom scale for the view.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="zoomScale">The zoom scale.</param>
        [ReactProp("maximumZoomScale")]
        public void SetMaximumZoomScale(Scroller view, float? zoomScale)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the zoom scale for the view.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="zoomScale">The zoom scale.</param>
        [ReactProp("zoomScale")]
        public void SetZoomScale(Scroller view, float? zoomScale)
        {
            // NOT SUPPORTED
            throw new NotImplementedException();
        }

        /// <summary>
        /// Enables or disables scroll view zoom.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="enabled">Signals whether zoom is enabled.</param>
        [ReactProp("zoomEnabled")]
        public void SetZoomScale(Scroller view, bool? enabled)
        {
            // NOT SUPPORTED
            throw new NotImplementedException();
        }


        /// <summary>
        /// Adds a child at the given index.
        /// </summary>
        /// <param name="parent">The parent view.</param>
        /// <param name="child">The child view.</param>
        /// <param name="index">The index.</param>
        /// <remarks>
        /// <see cref="ReactScrollViewManager"/> only supports one child.
        /// </remarks>
        public override void AddView(Scroller parent, Widget child, int index)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), Invariant($"{nameof(Scroller)} currently only supports one child."));
            }

            Log.Info(ReactConstants.Tag, "### ScrollViewManager::AddView ### parent:"+parent.GetTag()+" child:"+child.GetTag()+", index:"+index);

            parent.SetContent(child);
            child.SetParent(parent);

            // cache child
            _scrollViewerData.Add(parent, new ScrollViewerData(){ content = child});
        }

        /// <summary>
        /// Gets the child at the given index.
        /// </summary>
        /// <param name="parent">The parent view.</param>
        /// <param name="index">The index.</param>
        /// <returns>The child view.</returns>
        /// <remarks>
        /// <see cref="ReactScrollViewManager"/> only supports one child.
        /// </remarks>
        public override Widget GetChildAt(Scroller parent, int index)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "ScrollView currently only supports one child.");
            }
            
            Log.Info(ReactConstants.Tag, "### ScrollViewManager::GetChildAt ### parent:"+parent.GetTag()+", index:"+index);

            ScrollViewerData data;
            _scrollViewerData.TryGetValue(parent, out data);

            return data.content;
        }

        /// <summary>
        /// Gets the number of children in the view parent.
        /// </summary>
        /// <param name="parent">The view parent.</param>
        /// <returns>The number of children.</returns>
        public override int GetChildCount(Scroller parent)
        {
            //return _scrollViewer.Count; // currently, only support one child  = =!!
            return 1;
        }

        /// <summary>
        /// Removes all children from the view parent.
        /// </summary>
        /// <param name="parent">The view parent.</param>
        public override void RemoveAllChildren(Scroller parent)
        {
            Log.Info(ReactConstants.Tag, "### ScrollViewManager::RemoveAllChildren ### parent:"+parent.GetTag());
            
            parent.SetContent(null);

            ScrollViewerData data;
            _scrollViewerData.TryGetValue(parent, out data);
            data.content = null;
            _scrollViewerData.Remove(parent);
            //_scrollViewer.Clear();
        }

        /// <summary>
        /// Removes the child at the given index.
        /// </summary>
        /// <param name="parent">The view parent.</param>
        /// <param name="index">The index.</param>
        /// <remarks>
        /// <see cref="ReactScrollViewManager"/> only supports one child.
        /// </remarks>
        public override void RemoveChildAt(Scroller parent, int index)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "ScrollView currently only supports one child.");
            }

            RemoveAllChildren(parent);
        }


        /// <summary>
        /// Called when view is detached from view hierarchy and allows for 
        /// additional cleanup by the <see cref="ReactScrollViewManager"/>.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <param name="view">The view.</param>
        public override void OnDropViewInstance(ThemedReactContext reactContext, Scroller view)
        {
            base.OnDropViewInstance(reactContext, view);
            //_scrollViewerData.Remove(view);

            view.Scrolled -= OnViewChanging;
            view.DragStart -= OnDragStarted;
            view.DragStop -= OnDragCompleted;
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
        public override void ReceiveCommand(Scroller view, int commandId, JArray args)
        {
            switch (commandId)
            {
                case CommandScrollTo:
                    //var x = args[0].Value<double>();
                    //var y = args[1].Value<double>();
                    var x = args[0].Value<int>();
                    var y = args[1].Value<int>();

                    var animated = args[2].Value<bool>();
                    int Width = GetChildAt(view, 0).MinimumWidth;
                    int Height = GetChildAt(view, 0).MinimumHeight;
                    Log.Info(ReactConstants.Tag, $"## CommandScrollTo ## X = {x} Y = {y} Width = {Width} Height = {Height}");

                    Rect region = new Rect(x, y, view.Geometry.Width, view.Geometry.Height);
                    view.ScrollTo(region, animated);
                    break;
                
                case CommandScrollToEnd:

                    //var anim = args[0].Value<bool>(); // depends on the index

                    int X = GetChildAt(view, 0).MinimumWidth;
                    int Y = GetChildAt(view, 0).MinimumHeight;
                    int WidthEnd = GetChildAt(view, 0).MinimumWidth;
                    int HeightEnd = GetChildAt(view, 0).MinimumHeight;

                    Log.Info(ReactConstants.Tag, $"## CommandScrollToEnd ## X = {X} Y = {Y} Width = {WidthEnd} Height = {HeightEnd}" );

                    Rect regionEnd = new Rect(X, Y, WidthEnd, HeightEnd);
                    view.ScrollTo(regionEnd, true);
                    break;
                
                default:
                    throw new InvalidOperationException(
                        Invariant($"Unsupported command '{commandId}' received by '{typeof(ReactScrollManager)}'."));
            }
        }

        /// <summary>
        /// Creates a new view instance.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <returns>The view instance.</returns>
        protected override Scroller CreateViewInstance(ThemedReactContext reactContext)
        {
            //var scrollViewerData = new ScrollViewerData();

            var scrollViewer = new Scroller(ReactProgram.RctWindow)
            {
                //Style = "white",
            };
            //scrollViewer.SetPageSize(1.0, 1.0);
            //scrollViewer.ScrollTo(scrollViewer.HorizontalPageIndex > 0 ? scrollViewer.HorizontalPageIndex - 1 : 0, scrollViewer.VerticalPageIndex, true);
            //_scrollViewerData.Add(scrollViewer, scrollViewerData);
            scrollViewer.Show();

            return scrollViewer;
        }

        /// <summary>
        /// Adds event emitters for drag and scroll events.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <param name="view">The view instance.</param>
        protected override void AddEventEmitters(ThemedReactContext reactContext, Scroller view)
        {
            base.AddEventEmitters(reactContext, view);

            view.DragStart += OnDragStarted;
            view.DragStop += OnDragCompleted;
            view.Scrolled += OnViewChanging;
        }

        private void OnDragCompleted(object sender, object e)
        {
            var Scroller = (Scroller)sender;
            Log.Info(ReactConstants.Tag, "## EndDrag ## x=" + Scroller.CurrentRegion.Location.X + " y=" + Scroller.CurrentRegion.Location.Y);
            EmitScrollEvent(
                Scroller,
                ScrollEventType.EndDrag,
                Scroller.CurrentRegion.Location.X,
                Scroller.CurrentRegion.Location.Y,
                1);
        }

        private void OnDragStarted(object sender, object e)
        {
            var Scroller = (Scroller)sender;
            Log.Info(ReactConstants.Tag, "## BeginDrag ## x=" + Scroller.CurrentRegion.Location.X + " y=" + Scroller.CurrentRegion.Location.Y);
            EmitScrollEvent(
                Scroller,
                ScrollEventType.BeginDrag,
                Scroller.CurrentRegion.Location.X,
                Scroller.CurrentRegion.Location.Y,
                1);
        }

        private void OnViewChanging(object sender, EventArgs args)
        {
            var Scroller = (Scroller)sender;
            Log.Info(ReactConstants.Tag, "## Scroll ## x=" + Scroller.CurrentRegion.Location.X + " y=" + Scroller.CurrentRegion.Location.Y);
            EmitScrollEvent(
                Scroller,
                ScrollEventType.Scroll,
                Scroller.CurrentRegion.Location.X,
                Scroller.CurrentRegion.Location.Y,
                1);
        }

        private void EmitScrollEvent(
            Scroller Scroller,
            ScrollEventType eventType,
            double x,
            double y,
            double zoomFactor)
        {
            var reactTag = Scroller.GetTag();

            // Scroll position
            var contentOffset = new JObject
            {
                { "x", x },
                { "y", y },
            };

            // Distance the content view is inset from the enclosing scroll view
            // TODO: Should these always be 0 for the XAML Scroller?
            var contentInset = new JObject
            {
                { "top", 0 },
                { "bottom", 0 },
                { "left", 0 },
                { "right", 0 },
            };

            // Size of the content view
            var contentSize = new JObject
            {
                { "width", Scroller.CurrentRegion.Width },
                { "height", Scroller.CurrentRegion.Height },
            };

            // Size of the viewport
            var layoutMeasurement = new JObject
            {
                { "width", Scroller.GetDimensions().Width},
                { "height", Scroller.GetDimensions().Height },
            };

            Scroller.GetReactContext()
                .GetNativeModule<UIManagerModule>()
                .EventDispatcher
                .DispatchEvent(
                    new ScrollEvent(
                        reactTag,
                        eventType,
                        new JObject
                        {
                            { "target", reactTag },
                            { "responderIgnoreScroll", true },
                            { "contentOffset", contentOffset },
                            { "contentInset", contentInset },
                            { "contentSize", contentSize },
                            { "layoutMeasurement", layoutMeasurement },
                            { "zoomScale", zoomFactor },
                        }));
        }

        /*
        private static Widget EnsureChild(Scroller view)
        {
            var child = view.SetContent;

            if (child == null)
            {
                throw new InvalidOperationException(Invariant($"{nameof(Scroller)} does not have any children."));
            }

            var widget = child as Widget;
            if (widget == null)
            {
                throw new InvalidOperationException(Invariant($"Invalid child element in {nameof(Scroller)}."));
            }

            return widget;
        }
        */

        class ScrollEvent : Event
        {
            private readonly ScrollEventType _type;
            private readonly JObject _data;

            public ScrollEvent(int viewTag, ScrollEventType type, JObject data)
                : base(viewTag, TimeSpan.FromTicks(Environment.TickCount))
            {
                _type = type;
                _data = data;
            }

            public override string EventName
            {
                get
                {
                    return _type.GetJavaScriptEventName();
                }
            }

            public override void Dispatch(RCTEventEmitter eventEmitter)
            {
                eventEmitter.receiveEvent(ViewTag, EventName, _data);
            }
        }

        class ScrollViewerData
        {
            public Widget content;  // keep child for control
        }
    }
}
