using System;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.FormattableString;
using ElmSharp;
using Tizen;
using ReactNative.Common;
using ReactNative.UIManager;

using System.Linq;

namespace ReactNativeTizen.ElmSharp.Extension
{
    /// <summary>
    /// Extension methods for <see cref="EvasObject"/>.
    /// </summary>
    public static class EvasObjectExtensions
    {
        #region "## For Animation 2017-05-04 ##"
        internal const string Eina = "libeina.so.1";
        internal const string Ecore = "libecore.so.1";
        internal const string Elementary = "libelementary.so.1";
        internal delegate bool EcoreTimelineCallback(IntPtr data, double pos);

        internal enum POS_MAPPINGS
        {
            ECORE_POS_MAP_LINEAR,
            ECORE_POS_MAP_ACCELERATE,
            ECORE_POS_MAP_DECELERATE,
            ECORE_POS_MAP_SINUSOIDAL,
            ECORE_POS_MAP_ACCELERATE_FACTOR,
            ECORE_POS_MAP_DECELERATE_FACTOR,
            ECORE_POS_MAP_SINUSOIDAL_FACTOR,
            ECORE_POS_MAP_DIVISOR_INTERP,
            ECORE_POS_MAP_BOUNCE,
            ECORE_POS_MAP_SPRING
        };
        #endregion

        private static readonly ConditionalWeakTable<EvasObject, EvasObjectData> s_properties =
            new ConditionalWeakTable<EvasObject, EvasObjectData>();
        private static readonly IReactCompoundView s_defaultCompoundView = new ReactDefaultCompoundView();


        #region "## For Animation 2017-05-04 ##"

        [DllImport(Ecore)]
        internal static extern void ecore_animator_frametime_set(double frametime);

        [DllImport(Ecore)]
        internal static extern IntPtr ecore_animator_timeline_add(double runtime, EcoreTimelineCallback cb, IntPtr data);

        [DllImport(Ecore)]
        internal static extern double ecore_animator_pos_map(double pos, POS_MAPPINGS ecore_pos_type, double v1, double v2);

        [DllImport(Ecore)]
        internal static extern void ecore_animator_freeze(IntPtr data);

        [DllImport(Ecore)]
        internal static extern void ecore_animator_thaw(IntPtr data);

        [DllImport(Elementary)]
        internal static extern void edje_scale_set(double data);

        [DllImport(Elementary)]
        internal static extern double edje_scale_get();

        [DllImport(Elementary)]
        internal static extern void edje_transition_duration_factor_set(double scale);

        [DllImport(Elementary)]
        internal static extern double edje_transition_duration_factor_get();

        //static EcoreTimelineCallback _nativeHandler;

        #endregion


        /// <summary>
        /// Sets the pointer events for the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="pointerEvents">The pointer events.</param>
        public static void SetPointerEvents(this EvasObject view, PointerEvents pointerEvents)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            s_properties.GetOrCreateValue(view).PointerEvents = pointerEvents;
        }

        /// <summary>
        /// Gets the pointer events for the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>The pointer events.</returns>
        public static PointerEvents GetPointerEvents(this EvasObject view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            var elementData = default(EvasObjectData);
            if (!s_properties.TryGetValue(view, out elementData) || !elementData.PointerEvents.HasValue)
            {
                return PointerEvents.Auto;
            }

            return elementData.PointerEvents.Value;
        }

        /// <summary>
        /// Associates an implementation of IReactCompoundView with the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="compoundView">The implementation of IReactCompoundView.</param>
        public static void SetReactCompoundView(this EvasObject view, IReactCompoundView compoundView)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            s_properties.GetOrCreateValue(view).CompoundView = compoundView;
        }

        /// <summary>
        /// Gets the implementation of IReactCompoundView associated with the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>
        /// The implementation of IReactCompoundView associated with the view. Defaults to
        /// an instance of ReactDefaultCompoundView when no other implementation has been
        /// provided.
        /// </returns>
        public static IReactCompoundView GetReactCompoundView(this EvasObject view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            var elementData = default(EvasObjectData);
            if (s_properties.TryGetValue(view, out elementData))
            {
                var compoundView = elementData.CompoundView;
                if (compoundView != null)
                {
                    return compoundView;
                }
            }

            return s_defaultCompoundView;
        }

        /// <summary>
        /// Set the React tag for the view instance (or create mapping relationship). 
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <returns>The React tag.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if tag is not available for the view.
        /// </exception>
        internal static void SetTag(this EvasObject view, int tag)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }
            Log.Debug(ReactConstants.Tag, "### SetTag ###  View="+view+", tag="+tag);
            s_properties.GetOrCreateValue(view).Tag = tag;
        }

        /// <summary>
        /// Get the React tag for the view instance. 
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <returns>The React tag.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if tag is not available for the view.
        /// </exception>
        public static int GetTag(this EvasObject view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));


            //Log.Error(ReactConstants.Tag, "### SetTag ###  View=" + view );
            var elementData = default(EvasObjectData);
            if (!s_properties.TryGetValue(view, out elementData) || !elementData.Tag.HasValue)
            {
                throw new InvalidOperationException("Could not get tag for view.");
            }

            return elementData.Tag.Value;
        }

        public static void CancelAnimations(EvasObject view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            // TODO: cancel all animations


        }

        /*
        void ApplyRotation(EvasMap map, Rect geometry, ref bool changed)
        {
            var rotationX = Element.RotationX;
            var rotationY = Element.RotationY;
            var rotationZ = Element.Rotation;
            var anchorX = Element.AnchorX;
            var anchorY = Element.AnchorY;

            // apply rotations
            if (rotationX != 0 || rotationY != 0 || rotationZ != 0)
            {
                map.Rotate3D(rotationX, rotationY, rotationZ, (int)(geometry.X + geometry.Width * anchorX),
                                                              (int)(geometry.Y + geometry.Height * anchorY), 0);
                changed = true;
            }
        }

        void ApplyScale(EvasMap map, Rect geometry, ref bool changed)
        {
            var scale = Element.Scale;

            // apply scale factor
            if (scale != 1.0)
            {
                map.Zoom(scale, scale,
                    geometry.X + (int)(geometry.Width * view.AnchorX),
                    geometry.Y + (int)(geometry.Height * Element.AnchorY));
                changed = true;
            }
        }

        void ApplyTranslation(EvasMap map, Rect geometry, ref bool changed)
        {
            var shiftX = Forms.ConvertToScaledPixel(Element.TranslationX);
            var shiftY = Forms.ConvertToScaledPixel(Element.TranslationY);

            // apply translation, i.e. move/shift the object a little
            if (shiftX != 0 || shiftY != 0)
            {
                if (changed)
                {
                    // special care is taken to apply the translation last
                    Point3D p;
                    for (int i = 0; i < 4; i++)
                    {
                        p = map.GetPointCoordinate(i);
                        p.X += shiftX;
                        p.Y += shiftY;
                        map.SetPointCoordinate(i, p);
                    }
                }
                else
                {
                    // in case when we only need translation, then construct the map in a simpler way
                    geometry.X += shiftX;
                    geometry.Y += shiftY;
                    map.PopulatePoints(geometry, 0);

                    changed = true;
                }
            }
        }
        */

        /// <summary>
        /// Set Matrix of view instance. 
        /// </summary>
        internal static void SetMatrix(this EvasObject view, ReactNative.UIManager.MatrixMathHelper.MatrixDecompositionContext matrix, bool reset)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            double TranslationX    = 0;
            double TranslationY    = 0;
            double rotationX       = 0;
            double rotationY       = 0;
            double rotationZ       = 0;
            double ScaleX          = 1;
            double ScaleY          = 1;

            if(matrix != null)
            {
                TranslationX    = matrix.translation[0];
                TranslationY    = matrix.translation[1];
                rotationX       = matrix.rotationDegrees[0];
                rotationY       = matrix.rotationDegrees[1];
                rotationZ       = matrix.rotationDegrees[2];
                ScaleX          = matrix.scale[0];
                ScaleY          = matrix.scale[1];
            }

            var map = new EvasMap(4);
            var g = view.Geometry;
            map.PopulatePoints(g, 0);

            //setScale
            map.Zoom(ScaleX, ScaleY,
                view.Geometry.X + (int)(view.Geometry.Width * 0.5),
                view.Geometry.Y + (int)(view.Geometry.Height * 0.5));

            //setRotation
            map.Rotate3D(rotationX, rotationY, rotationZ, 
                        (int)(view.Geometry.X + view.Geometry.Width * 0.5), (int)(view.Geometry.Y + view.Geometry.Height * 0.5), 0);

            //setTranslation
            var shiftX = TranslationX;
            var shiftY = TranslationY;

            Point3D p;
            for (int i = 0; i < 4; i++)
            {
                p = map.GetPointCoordinate(i);
                p.X += (int)shiftX;
                p.Y += (int)shiftY;
                map.SetPointCoordinate(i, p);
            }

            view.EvasMap = map;
            view.IsMapEnabled = true;

            if(!reset)
            {
                if (matrix != null)
                {
                    s_properties.GetOrCreateValue(view).matrix = matrix.Clone();
                }
            }
        }

        /// <summary>
        /// Get Matrix of view instance. 
        /// </summary>
        internal static ReactNative.UIManager.MatrixMathHelper.MatrixDecompositionContext GetMatrix(this EvasObject view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            var elementData = default(EvasObjectData);
            if (!s_properties.TryGetValue(view, out elementData) || !elementData.Tag.HasValue)
            {
                throw new InvalidOperationException("Could not get tag for view.");   // if current view wasn't set with tag before, just ignore it. 2017-02-24                                                           // or just return invalid tag:'-1'
            }

            return elementData.matrix;
        }

        /// <summary>
        /// Check whether Matrix of view instance is null. 
        /// </summary>
        internal static bool IsSetMatrix(this EvasObject view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            var elementData = default(EvasObjectData);
            if (!s_properties.TryGetValue(view, out elementData) || !elementData.Tag.HasValue)
            {
                return false;
            }

            return (elementData.matrix != null);
        }

        /// <summary>
        /// Clean the mapping relationship of view instance & tag. 
        /// </summary>
        public static bool RemoveViewInfo(this EvasObject view)
        {
            return s_properties.Remove(view);
        }

        /// <summary>
        /// Checks if a React tag is available for the view instance.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <returns>
        /// <code>true</code> if the view has a tag, <code>false</code> otherwise.
        /// </returns>
        public static bool HasTag(this EvasObject view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            var elementData = default(EvasObjectData);
            return s_properties.TryGetValue(view, out elementData) && elementData.Tag.HasValue;
        }

        //Parent
        internal static void SetParent(this EvasObject view, EvasObject parent)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            s_properties.GetOrCreateValue(view).parent = parent;
        }

        internal static EvasObject GetParent(this EvasObject view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            var elementData = default(EvasObjectData);
            if (!s_properties.TryGetValue(view, out elementData) || !elementData.Tag.HasValue)
            {
                throw new InvalidOperationException("Could not get tag for view.");   // if current view wasn't set with tag before, just ignore it. 2017-02-24                                                           // or just return invalid tag:'-1'
            }

            return elementData.parent;
        }

        // Position Extension
        internal static void SetDimensions(this EvasObject view, Dimensions dimensions)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            s_properties.GetOrCreateValue(view).dimension = dimensions;
        }

        internal static Dimensions GetDimensions(this EvasObject view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            var elementData = default(EvasObjectData);
            if (!s_properties.TryGetValue(view, out elementData) || !elementData.Tag.HasValue)
            {
                throw new InvalidOperationException("Could not get tag for view.");   // if current view wasn't set with tag before, just ignore it. 2017-02-24                                                           // or just return invalid tag:'-1'
            }

            return elementData.dimension;
        }

        internal static int GetAbsoluteX(this EvasObject view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (view.GetParent() == null)
                return 0;
            else
                return view.GetParent().GetAbsoluteX() + (int)view.GetDimensions().X;
        }

        internal static int GetAbsoluteY(this EvasObject view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (view.GetParent() == null)
                return 0;
            else
                return view.GetParent().GetAbsoluteY() + (int)view.GetDimensions().Y;
        }
        //

        internal static void SetReactContext(this EvasObject view, ThemedReactContext context)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            s_properties.GetOrCreateValue(view).Context = context;
        }

        /// <summary>
        /// Gets the <see cref="ThemedReactContext"/> associated with the view
        /// instance.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <returns>The context.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if context is not available for the view.
        /// </exception>
        public static ThemedReactContext GetReactContext(this EvasObject view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            var elementData = default(EvasObjectData);
            if (!s_properties.TryGetValue(view, out elementData) || !elementData.Tag.HasValue)
            {
                throw new InvalidOperationException("Could not get tag for view.");
            }

            return elementData.Context;
        }

        internal static T As<T>(this EvasObject view)
            where T : EvasObject
        {
            var convertedView = view as T;
            if (convertedView == null)
            {
                throw new ArgumentOutOfRangeException(Invariant($"Child of type '{view.GetType()}' is not assignable to '{typeof(T)}'."));
            }
            return convertedView;
        }

        class EvasObjectData
        {
            public ThemedReactContext Context { get; set; }

            public PointerEvents? PointerEvents { get; set; }

            public int? Tag { get; set; }

            public IReactCompoundView CompoundView { get; set; }

            // Postion Extension 2017-03-15
            public Dimensions dimension { get; set; }
            //

            public EvasObject parent { get; set; }

            public ReactNative.UIManager.MatrixMathHelper.MatrixDecompositionContext matrix { get; set; } = null;
        }
    }
}
