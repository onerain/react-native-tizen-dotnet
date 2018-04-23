using Newtonsoft.Json.Linq;
using ReactNative.Bridge;
//using ReactNative.Touch;
using ReactNative.Tracing;
using ReactNative.UIManager.LayoutAnimation;
using System;
using System.Linq;
using System.Collections.Generic;
using static System.FormattableString;


using Tizen;
using ReactNative.Common;
using ReactNativeTizen.ElmSharp.Extension;
using ElmSharp;

namespace ReactNative.UIManager
{
    /// <summary>
    /// Delegate of <see cref="UIManagerModule"/> that owns the native view
    /// hierarchy and mapping between native view names used in JavaScript and
    /// corresponding instances of <see cref="IViewManager"/>. The 
    /// <see cref="UIManagerModule"/> communicates with this class by it's
    /// public interface methods:
    /// - <see cref="UpdateProperties(int, ReactStylesDiffMap)"/>
    /// - <see cref="UpdateLayout(int, int, Dimensions)"/>
    /// - <see cref="CreateView(ThemedReactContext, int, string, ReactStylesDiffMap)"/>
    /// - <see cref="ManageChildren(int, int[], ViewAtIndex[], int[])"/>
    /// executing all the scheduled operations at the end of the JavaScript batch.
    /// </summary>
    /// <remarks>
    /// All native view management methods listed above must be called from the
    /// dispatcher thread.
    /// 
    /// The <see cref="ReactContext"/> instance that is passed to views that
    /// this manager creates differs from the one that we pass to the
    /// constructor. Instead we wrap the provided instance of 
    /// <see cref="ReactContext"/> in an instance of <see cref="ThemedReactContext"/>
    /// that additionally provides a correct theme based on the root view for
    /// a view tree that we attach newly created views to. Therefore this view
    /// manager will create a copy of <see cref="ThemedReactContext"/> that
    /// wraps the instance of <see cref="ReactContext"/> for each root view
    /// added to the manager (see
    /// <see cref="AddRootView(int, SizeMonitoringCanvas, ThemedReactContext)"/>).
    /// 
    /// TODO: 
    /// 1) AnimationRegistry
    /// 2) ShowPopupMenu
    /// </remarks>
    public class NativeViewHierarchyManager
    {
        private readonly IDictionary<int, IViewManager> _tagsToViewManagers;
        private readonly IDictionary<int, Widget> _tagsToViews;
        private readonly IDictionary<int, bool> _rootTags;
        private readonly ViewManagerRegistry _viewManagers;
        //private readonly JavaScriptResponderHandler _jsResponderHandler;
        private readonly RootViewManager _rootViewManager;
        private readonly LayoutAnimationController _layoutAnimator;

        /// <summary>
        /// Instantiates the <see cref="NativeViewHierarchyManager"/>.
        /// </summary>
        /// <param name="viewManagers">The view manager registry.</param>
        public NativeViewHierarchyManager(ViewManagerRegistry viewManagers)
        {
            _viewManagers = viewManagers;
            _layoutAnimator = new LayoutAnimationController();
            _tagsToViews = new Dictionary<int, Widget>();
            _tagsToViewManagers = new Dictionary<int, IViewManager>();
            _rootTags = new Dictionary<int, bool>();
            //_jsResponderHandler = new JavaScriptResponderHandler();
            _rootViewManager = new RootViewManager();
        }

        /// <summary>
        /// Signals if layout animation is enabled.
        /// </summary>
        public bool LayoutAnimationEnabled
        {
            private get;
            set;
        }
        
        /// <summary>
        /// Updates the properties of the view with the given tag.
        /// </summary>
        /// <param name="tag">The view tag.</param>
        /// <param name="props">The properties.</param>
        public void UpdateProperties(int tag, ReactStylesDiffMap props)
        {
            DispatcherHelpers.AssertOnDispatcher();
            var viewManager = ResolveViewManager(tag);
            var viewToUpdate = ResolveView(tag);
            viewManager.UpdateProperties(viewToUpdate, props);
        }

        /// <summary>
        /// Updates the extra data for the view with the given tag.
        /// </summary>
        /// <param name="tag">The view tag.</param>
        /// <param name="extraData">The extra data.</param>
        public void UpdateViewExtraData(int tag, object extraData)
        {
            DispatcherHelpers.AssertOnDispatcher();
            var viewManager = ResolveViewManager(tag);
            var viewToUpdate = ResolveView(tag);
            viewManager.UpdateExtraData(viewToUpdate, extraData);
        }

        /// <summary>
        /// Updates the layout of a view.
        /// </summary>
        /// <param name="parentTag">The parent view tag.</param>
        /// <param name="tag">The view tag.</param>
        /// <param name="dimensions">The dimensions.</param>
        public void UpdateLayout(int parentTag, int tag, Dimensions dimensions)
        {
            DispatcherHelpers.AssertOnDispatcher();
            //Log.Info(ReactConstants.Tag, "[NativeViewHierarcyManager.UpdateLayout] parentTag:"+parentTag+", tag:"+tag);
            using (RNTracer.Trace(RNTracer.TRACE_TAG_REACT_VIEW, "NativeViewHierarcyManager.UpdateLayout")
                .With("parentTag", parentTag)
                .With("tag", tag)
                .Start())
            {
                // obtain the view to be updated via 'tag'
                var viewToUpdate = ResolveView(tag);

                // obtain the specific view manager for updating current view
                var viewManager = ResolveViewManager(tag);

                //Log.Info(ReactConstants.Tag, "[NativeViewHierarcyManager.UpdateLayout] viewToUpdate:" + viewToUpdate + ", viewManager:" + viewManager);

                // check the vilidity of view manager
                var parentViewManager = default(IViewManager);
                var parentViewBoxManager = default(IViewParentManager);

                if (!_tagsToViewManagers.TryGetValue(parentTag, out parentViewManager) ||
                    (parentViewBoxManager = parentViewManager as IViewParentManager) == null)
                {
                    throw new InvalidOperationException(
                        Invariant($"Trying to use view with tag '{tag}' as a parent, but its manager doesn't extend ViewParentManager."));
                }

                if (!parentViewBoxManager.NeedsCustomLayoutForChildren)
                {
                    UpdateLayout(viewToUpdate, viewManager, dimensions);
                }
            }
        }

        /// <summary>
        /// Creates a view with the given tag and class name.
        /// </summary>
        /// <param name="themedContext">The context.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="className">The class name.</param>
        /// <param name="initialProperties">The properties.</param>
        public void CreateView(ThemedReactContext themedContext, int tag, string className, ReactStylesDiffMap initialProperties)
        {
            DispatcherHelpers.AssertOnDispatcher();
            using (RNTracer.Trace(RNTracer.TRACE_TAG_REACT_VIEW, "NativeViewHierarcyManager.CreateView")
                .With("tag", tag)
                .With("className", className)
                .Start())
            {
                var viewManager = _viewManagers.Get(className);
                var view = viewManager.CreateView(themedContext/*, _jsResponderHandler*/);
                _tagsToViews.Add(tag, view);
                _tagsToViewManagers.Add(tag, viewManager);

                // Uses an extension method and `Tag` property on 
                // EvasObject to store the tag of the view.
                view.SetTag(tag);
                view.SetReactContext(themedContext);

                if (initialProperties != null)
                {
                    viewManager.UpdateProperties(view, initialProperties);
                }
            }
        }

        /// <summary>
        /// Sets up the Layout Animation Manager.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="success"></param>
        /// <param name="error"></param>
        public void ConfigureLayoutAnimation(JObject config, ICallback success, ICallback error)
        {
            _layoutAnimator.InitializeFromConfig(config);
        }

        /// <summary>
        /// Clears out the <see cref="LayoutAnimationController"/>.
        /// </summary>
        public void ClearLayoutAnimation()
        {
            _layoutAnimator.Reset();
        }

        /// <summary>
        /// Manages the children of a React view.
        /// </summary>
        /// <param name="tag">The tag of the view to manager.</param>
        /// <param name="indexesToRemove">Child indices to remove.</param>
        /// <param name="viewsToAdd">Views to add.</param>
        /// <param name="tagsToDelete">Tags to delete.</param>
        public void ManageChildren(int tag, int[] indexesToRemove, ViewAtIndex[] viewsToAdd, int[] tagsToDelete)
        {
            var viewManager = default(IViewManager);
            if (!_tagsToViewManagers.TryGetValue(tag, out viewManager))
            {
                throw new InvalidOperationException(
                    Invariant($"Trying to manage children with tag '{tag}' which doesn't exist."));
            }

            var ViewParentManager = (IViewParentManager)viewManager;
            var viewToManage = _tagsToViews[tag];

            var lastIndexToRemove = ViewParentManager.GetChildCount(viewToManage);
            if (indexesToRemove != null)
            {
                for (var i = indexesToRemove.Length - 1; i >= 0; --i)
                {
                    var indexToRemove = indexesToRemove[i];
                    if (indexToRemove < 0)
                    {
                        throw new InvalidOperationException(
                            Invariant($"Trying to remove a negative index '{indexToRemove}' on view tag '{tag}'."));
                    }

                    if (indexToRemove >= ViewParentManager.GetChildCount(viewToManage))
                    {
                        throw new InvalidOperationException(
                            Invariant($"Trying to remove a view index '{indexToRemove}' greater than the child could for view tag '{tag}'."));
                    }

                    if (indexToRemove >= lastIndexToRemove)
                    {
                        throw new InvalidOperationException(
                            Invariant($"Trying to remove an out of order index '{indexToRemove}' (last index was '{lastIndexToRemove}') for view tag '{tag}'."));
                    }

                    var viewToRemove = ViewParentManager.GetChildAt(viewToManage, indexToRemove) as Widget;
                    if (viewToRemove != null &&
                         _layoutAnimator.ShouldAnimateLayout(viewToRemove) &&    //TODO: LayoutAnimation will be merged later .. BOY.YANG  
                        tagsToDelete != null && tagsToDelete.Contains(viewToRemove.GetTag()))
                    {
                        // The view will be removed and dropped by the 'delete'
                        // layout animation instead, so do nothing.
                    }
                    else
                    {
                        ViewParentManager.RemoveChildAt(viewToManage, indexToRemove);
                    }

                    lastIndexToRemove = indexToRemove;
                }
            }

            if (viewsToAdd != null)
            {
                for (var i = 0; i < viewsToAdd.Length; ++i)
                {
                    var viewAtIndex = viewsToAdd[i];
                    var viewToAdd = default(Widget);
                    if (!_tagsToViews.TryGetValue(viewAtIndex.Tag, out viewToAdd))
                    {
                        throw new InvalidOperationException(
                            Invariant($"Trying to add unknown view tag '{viewAtIndex.Tag}'."));
                    }

                    //Log.Info(ReactConstants.Tag, "## ManageChildren ## viewToManage:" + viewToManage + " viewToAdd:" + viewToAdd
                    //    + " Index:"+ viewAtIndex.Index);
                    ViewParentManager.AddView(viewToManage, viewToAdd, viewAtIndex.Index);
                }
            }

            if (tagsToDelete != null)
            {
                for (var i = 0; i < tagsToDelete.Length; ++i)
                {
                    var tagToDelete = tagsToDelete[i];
                    var viewToDestroy = default(Widget);
                    if (!_tagsToViews.TryGetValue(tagToDelete, out viewToDestroy))
                    {
                        throw new InvalidOperationException(
                            Invariant($"Trying to destroy unknown view tag '{tagToDelete}'."));
                    }

                    var elementToDestroy = viewToDestroy as Widget;
                    if (elementToDestroy != null &&
                         _layoutAnimator.ShouldAnimateLayout(elementToDestroy) ) //TODO: LayoutAnimation will be merged later .. BOY.YANG
                    {
                        _layoutAnimator.DeleteView(elementToDestroy, () =>
                        {
                            if (ViewParentManager.TryRemoveView(viewToManage, viewToDestroy))
                            {
                                DropView(viewToDestroy);
                            }
                        });
                    }
                    else
                    {
                        DropView(viewToDestroy);
                    }
                }
            }
        }

        /// <summary>
        /// Simplified version of <see cref="UIManagerModule.manageChildren(int, int[], int[], int[], int[], int[])"/>
        /// that only deals with adding children views.
        /// </summary>
        /// <param name="tag">The view tag to manage.</param>
        /// <param name="childrenTags">The children tags.</param>
        public void SetChildren(int tag, int[] childrenTags)
        {
            var viewToManage = _tagsToViews[tag];

            var viewManager = (IViewParentManager)ResolveViewManager(tag);

            for (var i = 0; i < childrenTags.Length; ++i)
            {
                var viewToAdd = _tagsToViews[childrenTags[i]];
                if (viewToAdd == null)
                {
                    throw new InvalidOperationException(
                        Invariant($"Trying to add unknown view tag: {childrenTags[i]}."));
                }

                //Log.Info(ReactConstants.Tag, "## SetChildren ## viewToManage:" + viewToManage + " viewToAdd:" + viewToAdd + " Index:" + i);
                viewManager.AddView(viewToManage, viewToAdd, i);
            }
        }

        /// <summary>
        /// Remove the root view with the given tag.
        /// </summary>
        /// <param name="rootViewTag">The root view tag.</param>
        public void RemoveRootView(int rootViewTag)
        {
            DispatcherHelpers.AssertOnDispatcher();
            if (!_rootTags.ContainsKey(rootViewTag))
            {
                throw new InvalidOperationException(
                    Invariant($"View with tag '{rootViewTag}' is not registered as a root view."));
            }

            var rootView = _tagsToViews[rootViewTag];
            DropView(rootView);
            _rootTags.Remove(rootViewTag);
        }

        /// <summary>
        /// Measures a view and sets the output buffer to (x, y, width, height).
        /// Measurements are relative to the RootView.
        /// </summary>
        /// <param name="tag">The view tag.</param>
        /// <param name="outputBuffer">The output buffer.</param>
        public void Measure(int tag, double[] outputBuffer)
        {
            DispatcherHelpers.AssertOnDispatcher();
            var view = default(Widget);
            if (!_tagsToViews.TryGetValue(tag, out view))
            {
                throw new ArgumentOutOfRangeException(nameof(tag));
            }

            var viewManager = default(IViewManager);
            if (!_tagsToViewManagers.TryGetValue(tag, out viewManager))
            {
                throw new InvalidOperationException(
                    Invariant($"Could not find view manager for tag '{tag}."));
            }

            var rootView = RootViewHelper.GetRootView(view);
            if (rootView == null)
            {
                throw new InvalidOperationException(
                    Invariant($"Native view '{tag}' is no longer on screen."));
            }

            // TODO: better way to get relative position?
            //var Widget = view.As<Widget>();

            //var rootTransform = Widget.TransformToVisual(rootView);

            /*
#if WINDOWS_UWP
            var positionInRoot = rootTransform.TransformPoint(new Point(0, 0));
#else
            //var positionInRoot = rootTransform.Transform(new Point(0, 0));
            Point point = new Point();                                               // adapt to the new encapsulation of ElmSharp
            point.X = 0;
            point.Y = 0;
            var positionInRoot = rootTransform.Transform(point);
#endif
            */
            var dimensions = viewManager.GetDimensions(rootView);
            //outputBuffer[0] = positionInRoot.X;
            //outputBuffer[1] = positionInRoot.Y;
            outputBuffer[2] = dimensions.Width;
            outputBuffer[3] = dimensions.Height;
        }

        /// <summary>
        /// Measures a view and sets the output buffer to (x, y, width, height).
        /// Measurements are relative to the window.
        /// </summary>
        /// <param name="tag">The view tag.</param>
        /// <param name="outputBuffer">The output buffer.</param>
        public void MeasureInWindow(int tag, double[] outputBuffer)
        {
            DispatcherHelpers.AssertOnDispatcher();
            var view = default(Widget);
            if (!_tagsToViews.TryGetValue(tag, out view))
            {
                throw new ArgumentOutOfRangeException(nameof(tag));
            }

            var viewManager = default(IViewManager);
            if (!_tagsToViewManagers.TryGetValue(tag, out viewManager))
            {
                throw new InvalidOperationException(
                    Invariant($"Could not find view manager for tag '{tag}."));
            }

            var Widget = view.As<Widget>();

/*
#if WINDOWS_UWP
            var windowTransform = Widget.TransformToVisual(Window.Current.Content);
            var positionInWindow = windowTransform.TransformPoint(new Point(0, 0));
#else
            var windowTransform = Widget.TransformToVisual(Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive));
            var positionInWindow = windowTransform.Transform(new Point(0, 0));
#endif
*/
            var dimensions = viewManager.GetDimensions(Widget);

            //outputBuffer[0] = positionInWindow.X;
            //outputBuffer[1] = positionInWindow.Y;
            outputBuffer[0] = ReactProgram.RctWindow.AlignmentX;        //TODO: Conformed that Main Window start position BOY.YANG
            outputBuffer[1] = ReactProgram.RctWindow.AlignmentY;
            outputBuffer[2] = dimensions.Width;
            outputBuffer[3] = dimensions.Height;
        }

        /// <summary>
        /// Adds a root view with the given tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="view">The root view.</param>
        /// <param name="themedContext">The themed context.</param>
        public void AddRootView(int tag, SizeMonitoringCanvas view, ThemedReactContext themedContext)
        {
            AddRootViewParent(tag, view, themedContext);
        }

        /// <summary>
        /// Find the view target for touch coordinates.
        /// </summary>
        /// <param name="reactTag">The view tag.</param>
        /// <param name="touchX">The x-coordinate of the touch event.</param>
        /// <param name="touchY">The y-coordinate of the touch event.</param>
        /// <returns>The view target.</returns>
        /*
        public int FindTargetForTouch(int reactTag, double touchX, double touchY)
        {
            var view = default(EvasObject);
            if (!_tagsToViews.TryGetValue(reactTag, out view))
            {
                throw new InvalidOperationException(
                    Invariant($"Could not find view with tag '{reactTag}'."));
            }

            var Widget = view.As<Widget>();
#if WINDOWS_UWP
            var target = VisualTreeHelper.FindElementsInHostCoordinates(new Point(touchX, touchY), Widget)
#else
            var sources = new List<EvasObject>();
            // ToDo: Consider a pooled structure to improve performance in touch heavy applications
            VisualTreeHelper.HitTest(
                Widget,
                null,
                hit =>
                {
                    sources.Add(hit.VisualHit);
                    return HitTestResultBehavior.Continue;
                },
                new PointHitTestParameters(new Point(touchX, touchY)));
            var target = sources
#endif
                .OfType<Widget>()
                .Where(e => e.HasTag())
                .FirstOrDefault();

            if (target == null)
            {
                throw new InvalidOperationException(
                    Invariant($"Could not find React view at coordinates '{touchX},{touchY}'."));
            }

            return target.GetTag();
        }
        */

        /// <summary>
        /// Sets the JavaScript responder handler for a view.
        /// </summary>
        /// <param name="reactTag">The view tag.</param>
        /// <param name="initialReactTag">The initial tag.</param>
        /// <param name="blockNativeResponder">
        /// Flag to block the native responder.
        /// </param>
        public void SetJavaScriptResponder(int reactTag, int initialReactTag, bool blockNativeResponder)
        {
            if (!blockNativeResponder)
            {
                //_jsResponderHandler.SetJavaScriptResponder(initialReactTag, null);
                return;
            }

            var view = default(Widget);
            if (!_tagsToViews.TryGetValue(reactTag, out view))
            {
                throw new InvalidOperationException(
                    Invariant($"Could not find view with tag '{reactTag}'."));
            }

            // TODO: (#306) Finish JS responder implementation. 
        }

        /// <summary>
        /// Clears the JavaScript responder.
        /// </summary>
        public void ClearJavaScriptResponder()
        {
            //_jsResponderHandler.ClearJavaScriptResponder();
        }

        /// <summary>
        /// Dispatches a command to a view.
        /// </summary>
        /// <param name="reactTag">The view tag.</param>
        /// <param name="commandId">The command identifier.</param>
        /// <param name="args">The command arguments.</param>
        public void DispatchCommand(int reactTag, int commandId, JArray args)
        {
            DispatcherHelpers.AssertOnDispatcher();
            var view = default(Widget);
            if (!_tagsToViews.TryGetValue(reactTag, out view))
            {
                throw new InvalidOperationException(
                    Invariant($"Trying to send command to a non-existent view with tag '{reactTag}."));
            }

            var viewManager = ResolveViewManager(reactTag);
            viewManager.ReceiveCommand(view, commandId, args);
        }

        /// <summary>
        /// Shows a popup menu.
        /// </summary>
        /// <param name="tag">
        /// The tag of the anchor view (the popup menu is
        /// displayed next to this view); this needs to be the tag of a native
        /// view (shadow views cannot be anchors).
        /// </param>
        /// <param name="items">The menu items as an array of strings.</param>
        /// <param name="success">
        /// A callback used with the position of the selected item as the first
        /// argument, or no arguments if the menu is dismissed.
        /// </param>
        public void ShowPopupMenu(int tag, string[] items, ICallback success)
        {
#if WINDOWS_UWP
            DispatcherHelpers.AssertOnDispatcher();
            var view = ResolveView(tag);

            var menu = new PopupMenu();
            for (var i = 0; i < items.Length; ++i)
            {
                menu.Commands.Add(new UICommand(
                    items[i],
                    cmd =>
                    {
                        success.Invoke(cmd.Id);
                    },
                    i));
            }
#endif

            // TODO: figure out where to popup the menu
            // TODO: add continuation that calls the callback with empty args
            throw new NotImplementedException();
        }

        private Widget ResolveView(int tag)
        {
            var view = default(Widget);
            if (!_tagsToViews.TryGetValue(tag, out view))
            {
                throw new InvalidOperationException(
                    Invariant($"Trying to resolve view with tag '{tag}' which doesn't exist."));
            }

            return view;
        }

        private IViewManager ResolveViewManager(int tag)
        {
            var viewManager = default(IViewManager);
            if (!_tagsToViewManagers.TryGetValue(tag, out viewManager))
            {
                throw new InvalidOperationException(
                    Invariant($"ViewManager for tag '{tag}' could not be found."));
            }

            return viewManager;
        }

        private void AddRootViewParent(int tag, Widget view, ThemedReactContext themedContext)
        {
            Log.Info(ReactConstants.Tag, "### [BGN] AddRootViewParent ### ");
            DispatcherHelpers.AssertOnDispatcher();
            _tagsToViews.Add(tag, view);
            _tagsToViewManagers.Add(tag, _rootViewManager);
            _rootTags.Add(tag, true);
            view.SetTag(tag);
            view.SetReactContext(themedContext);
            Log.Info(ReactConstants.Tag, "### [END] AddRootViewParent ### ");
        }

        private void DropView(Widget view)
        {
            DispatcherHelpers.AssertOnDispatcher();
            var tag = view.GetTag();
            if (!_rootTags.ContainsKey(tag))
            {
                Log.Fatal(ReactConstants.Tag, "## DropView ## tag:"+tag);
                // For non-root views, we notify the view manager with `OnDropViewInstance`
                var mgr = ResolveViewManager(tag);
                mgr.OnDropViewInstance(view.GetReactContext(), view);
            }

            var viewManager = default(IViewManager);
            if (_tagsToViewManagers.TryGetValue(tag, out viewManager))
            {
                var ViewParentManager = viewManager as IViewParentManager;
                if (ViewParentManager != null)
                {
                    // it should be a view with 'Container' props
                    Log.Fatal(ReactConstants.Tag, "## ViewParentManager:" + ViewParentManager + ", ChildCount:" + ViewParentManager.GetChildCount(view));
                    for (var i = ViewParentManager.GetChildCount(view) - 1; i >= 0; --i)
                    {
                        var child = ViewParentManager.GetChildAt(view, i);
                        var managedChild = default(Widget);
                        if (_tagsToViews.TryGetValue(child.GetTag(), out managedChild))
                        {
                            DropView(managedChild);
                        }
                    }

                    ViewParentManager.RemoveAllChildren(view);
                }
            }

            Log.Fatal(ReactConstants.Tag, "## Remove View:["+tag+"] & ViewMgr Mapping Relationship");

            _tagsToViews.Remove(tag);
            _tagsToViewManagers.Remove(tag);
            
            // release sub views ('rootview' will only be destroyed while OnTeminate being invoked)
            if ( 1 != tag )
            {
                Log.Fatal(ReactConstants.Tag, ">>>>> release view component:["+tag+"]");

                // Clean View <-> Tag 
                if (false == view.RemoveViewInfo())
                {
                    Log.Fatal(ReactConstants.Tag, "Failed to clean current view info");
                }

                // hmm ... Life is Over
                view.Unrealize();
            }
        }

        private void UpdateLayout(Widget viewToUpdate, IViewManager viewManager, Dimensions dimensions)
        {
            var Widget = viewToUpdate as Widget;
            
            if (Widget != null && _layoutAnimator.ShouldAnimateLayout(Widget))
            {
                _layoutAnimator.ApplyLayoutUpdate(Widget, dimensions);
            }
            else
            {
                viewManager.SetDimensions(viewToUpdate, dimensions);
            }
        }
    }
}
