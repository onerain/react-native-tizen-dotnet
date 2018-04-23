using Newtonsoft.Json.Linq;
//using ReactNative.Touch;
using System;
using System.Collections.Generic;
using Tizen;
using ElmSharp;
using static System.FormattableString;

using ReactNativeTizen.ElmSharp.Extension;

namespace ReactNative.UIManager
{
    /// <summary>
    /// Class responsible for knowing how to create and update views of a given
    /// type. It is also responsible for creating and updating
    /// <see cref="ReactShadowNode"/> subclasses used for calculating position
    /// and size for the corresponding native view.
    /// </summary>
    public abstract class ViewManager<TView, TReactShadowNode> : IViewManager
        where TView : Widget
        where TReactShadowNode : ReactShadowNode
    {
        /// <summary>
        /// The name of this view manager. This will be the name used to 
        /// reference this view manager from JavaScript.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The <see cref="Type"/> instance that represents the type of shadow
        /// node that this manager will return from
        /// <see cref="CreateShadowNodeInstance"/>.
        /// 
        /// This method will be used in the bridge initialization phase to
        /// collect properties exposed using the <see cref="Annotations.ReactPropAttribute"/>
        /// annotation from the <see cref="ReactShadowNode"/> subclass.
        /// </summary>
        public virtual Type ShadowNodeType
        {
            get
            {
                return typeof(TReactShadowNode);
            }
        }

        /// <summary>
        /// The commands map for the view manager.
        /// </summary>
        public virtual IReadOnlyDictionary<string, object> CommandsMap { get; }

        /// <summary>
        /// The exported custom bubbling event types.
        /// </summary>
        public virtual IReadOnlyDictionary<string, object> ExportedCustomBubblingEventTypeConstants { get; }

        /// <summary>
        /// The exported custom direct event types.
        /// </summary>
        public virtual IReadOnlyDictionary<string, object> ExportedCustomDirectEventTypeConstants { get; }

        /// <summary>
        /// The exported view constants.
        /// </summary>
        public virtual IReadOnlyDictionary<string, object> ExportedViewConstants { get; }

        /// <summary>
        /// Creates a shadow node for the view manager.
        /// </summary>
        /// <returns>The shadow node instance.</returns>
        public IReadOnlyDictionary<string, string> NativeProperties
        {
            get
            {
                return ViewManagersPropertyCache.GetNativePropertiesForView(GetType(), ShadowNodeType);
            }
        }

        /// <summary>
        /// Update the properties of the given view.
        /// </summary>
        /// <param name="viewToUpdate">The view to update.</param>
        /// <param name="props">The properties.</param>
        public void UpdateProperties(TView viewToUpdate, ReactStylesDiffMap props)
        {
            var propertySetters =
                ViewManagersPropertyCache.GetNativePropertySettersForViewManagerType(GetType());

            var keys = props.Keys;
            foreach (var key in keys)
            {
                var setter = default(IPropertySetter);
                if (propertySetters.TryGetValue(key, out setter))
                {
                    setter.UpdateViewManagerProperty(this, viewToUpdate, props);
                }
            }

            OnAfterUpdateTransaction(viewToUpdate);
        }

        /// <summary>
        /// Creates a view and installs event emitters on it.
        /// </summary>
        /// <param name="reactContext">The context.</param>
        /// <param name="responderHandler">The responder handler.</param>
        /// <returns>The view.</returns>
        public TView CreateView(
            ThemedReactContext reactContext/*,
            JavaScriptResponderHandler responderHandler*/)
        {
            var view = CreateViewInstance(reactContext);

            AddEventEmitters(reactContext, view);

            // TODO: enable touch intercepting view parents

            return (TView)view;
        }

        /// <summary>
        /// Called when view is detached from view hierarchy and allows for 
        /// additional cleanup by the <see cref="IViewManager"/> subclass.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <param name="view">The view.</param>
        public virtual void OnDropViewInstance(ThemedReactContext reactContext, TView view)
        {
        }

        /// <summary>
        /// This method should return the subclass of <see cref="ReactShadowNode"/>
        /// which will be then used for measuring the position and size of the
        /// view. 
        /// </summary>
        /// <remarks>
        /// In most cases, this will just return an instance of
        /// <see cref="ReactShadowNode"/>.
        /// </remarks>
        /// <returns>The shadow node instance.</returns>
        public abstract TReactShadowNode CreateShadowNodeInstance();

        /// <summary>
        /// Implement this method to receive optional extra data enqueued from
        /// the corresponding instance of <see cref="ReactShadowNode"/> in
        /// <see cref="ReactShadowNode.OnCollectExtraUpdates"/>.
        /// </summary>
        /// <param name="root">The root view.</param>
        /// <param name="extraData">The extra data.</param>
        public abstract void UpdateExtraData(TView root, object extraData);

        /// <summary>
        /// Implement this method to receive events/commands directly from
        /// JavaScript through the <see cref="UIManagerModule"/>.
        /// </summary>
        /// <param name="view">
        /// The view instance that should receive the command.
        /// </param>
        /// <param name="commandId">Identifer for the command.</param>
        /// <param name="args">Optional arguments for the command.</param>
        public virtual void ReceiveCommand(TView view, int commandId, JArray args)
        {
        }

        /// <summary>
        /// Gets the dimensions of the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>The view dimensions.</returns>
        public Dimensions GetDimensions(TView view)
        {
            var dimension = view.GetDimensions();
            if (dimension == null)
            {
                var geometry = view.Geometry;
                return new Dimensions
                {
                    X = geometry.X,
                    Y = geometry.Y,
                    Width = geometry.Width,
                    Height = geometry.Height,
                };
            }
            else
                return dimension;
        }

        /// <summary>
        /// Sets the dimensions of the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="dimensions">The output buffer.</param>
        public virtual void SetDimensions(TView view, Dimensions dimensions)
        {
            var dimensionsBefore = view.GetDimensions();
            if(!dimensionsBefore.Equals(dimensions))
            {
                view.SetDimensions(dimensions);

                //For trigger layout callback
                view.Geometry = new Rect(view.GetAbsoluteX(), view.GetAbsoluteY(), (int)dimensions.Width, (int)dimensions.Height);

                view.Move(view.GetAbsoluteX(), view.GetAbsoluteY());
                view.Resize((int)dimensions.Width, (int)dimensions.Height);

                //Solved scroll view can't scroll issue
                view.MinimumHeight = (int)dimensions.Height;
                view.MinimumWidth = (int)dimensions.Width;

                Log.Info(Common.ReactConstants.Tag, Invariant($"View['{view.GetTag()}'] move to [{(int)dimensions.X}, {(int)dimensions.Y}] resize to [{(int)dimensions.Width}, {(int)dimensions.Height}]"));
            }
            else
            {
                Log.Info(Common.ReactConstants.Tag, Invariant($"View['{view.GetTag()}'] [{(int)dimensions.X}, {(int)dimensions.Y}] resize to [{(int)dimensions.Width}, {(int)dimensions.Height}] doesn't need move, caused by no change."));
            }
        }

        /// <summary>
        /// Creates a new view instance of type <typeparamref name="TView"/>.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <returns>The view instance.</returns>
        protected abstract TView CreateViewInstance(ThemedReactContext reactContext);

        /// <summary>
        /// Subclasses can override this method to install custom event 
        /// emitters on the given view.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        /// <param name="view">The view instance.</param>
        /// <remarks>
        /// Consider overriding this method if your view needs to emit events
        /// besides basic touch events to JavaScript (e.g., scroll events).
        /// </remarks>
        protected virtual void AddEventEmitters(ThemedReactContext reactContext, TView view)
        {
        }

        /// <summary>
        /// Callback that will be triggered after all properties are updated in
        /// the current update transation (all <see cref="Annotations.ReactPropAttribute"/> handlers
        /// for properties updated in the current transaction have been called).
        /// </summary>
        /// <param name="view">The view.</param>
        protected virtual void OnAfterUpdateTransaction(TView view)
        {
        }

#region IViewManager

        void IViewManager.UpdateProperties(Widget viewToUpdate, ReactStylesDiffMap props)
        {
            UpdateProperties((TView)viewToUpdate, props);
        }

        Widget IViewManager.CreateView(ThemedReactContext reactContext/*, JavaScriptResponderHandler jsResponderHandler*/)
        {
            return CreateView(reactContext/*, jsResponderHandler*/);
        }

        void IViewManager.OnDropViewInstance(ThemedReactContext reactContext, Widget view)
        {
            OnDropViewInstance(reactContext, (TView)view);
        }

        ReactShadowNode IViewManager.CreateShadowNodeInstance()
        {
            return CreateShadowNodeInstance();
        }

        void IViewManager.UpdateExtraData(Widget root, object extraData)
        {
            UpdateExtraData((TView)root, extraData);
        }

        void IViewManager.ReceiveCommand(Widget view, int commandId, JArray args)
        {
            ReceiveCommand((TView)view, commandId, args);
        }

        Dimensions IViewManager.GetDimensions(Widget view)
        {
            return GetDimensions((TView)view);
        }

        void IViewManager.SetDimensions(Widget view, Dimensions dimensions)
        {
            SetDimensions((TView)view, dimensions);
        }

#endregion
    }
}
