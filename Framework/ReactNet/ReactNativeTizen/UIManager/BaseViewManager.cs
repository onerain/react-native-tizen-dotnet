using System;
using Newtonsoft.Json.Linq;
using ReactNative.UIManager.Annotations;
using ElmSharp;

using Tizen;
using ReactNative.Common;
using ReactNativeTizen.ElmSharp.Extension;

namespace ReactNative.UIManager
{
    /// <summary>
    /// Base class that should be suitable for the majority of subclasses of <see cref="IViewManager"/>.
    /// It provides support for base view properties such as opacity, etc.
    /// </summary>
    /// <typeparam name="TView">Type of framework element.</typeparam>
    /// <typeparam name="TLayoutShadowNode">Type of shadow node.</typeparam>
    public abstract class BaseViewManager<TView, TLayoutShadowNode> :
            ViewManager<TView, TLayoutShadowNode>
        where TView : Widget
        where TLayoutShadowNode : LayoutShadowNode
    {
        private static MatrixMathHelper.MatrixDecompositionContext sMatrixDecompositionContext =
            new MatrixMathHelper.MatrixDecompositionContext();
        private static double[] sTransformDecompositionArray = new double[16];

        /// <summary>
        /// Sets the opacity of the <typeparamref name="TView"/>.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="opacity">The opacity value.</param>
        [ReactProp("opacity", DefaultDouble = 1.0)]
        public void SetOpacity(TView view, double? opacity)
        {
            if(opacity.HasValue)
            {
                //Log.Info(ReactConstants.Tag, "## SetOpacity ## view:" + view + " opacity:" + opacity + ", afterOpacity:" + Convert.ToInt32(opacity * 255));
                view.Opacity = Convert.ToInt32(opacity * 255);
            }
        }

        // ToDo: SetOverflow - ReactProp("overflow")

        /// <summary>
        /// Sets the z-index of the element.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="zIndex">The z-index.</param>
        [ReactProp("zIndex")]
        public void SetZIndex(TView view, int zIndex)
        {
            //Log.Info(ReactConstants.Tag, "## SetZIndex ## view:" + view + " zIndex:" + zIndex + "  view.Layer=" + view.Layer);
            //view.Layer = zIndex;
            //view.Lower();
        }

        /// <summary>
        /// Sets the accessibility label of the element.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="label">The label.</param>
        [ReactProp("accessibilityLabel")]
        public void SetAccessibilityLabel(TView view, string label)
        {
            //AutomationProperties.SetName(view, label ?? "");
        }
        
        // ToDo: SetAccessibilityLiveRegion - ReactProp("accessibilityLiveRegion")

        /// <summary>
        /// Sets the test ID, i.e., the automation ID.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="testId">The test ID.</param>
        [ReactProp("testID")]
        public void SetTestId(TView view, string testId)
        {
            //AutomationProperties.SetAutomationId(view, testId ?? "");
        }

        [ReactProp("transform")]
        public void SetTransform(TView view, JArray transforms)
        {
            //view.setTransform(transforms);
            try
            {
                if (transforms == null)
                {
                    resetTransformProperty(view);
                }
                else
                {
                    setTransformProperty(view, transforms);
                }
            }
            catch(Exception ex)
            {
                Log.Error(ReactConstants.Tag, $"view[{view.GetTag()}], transforms={transforms}, ex={ex.ToString()}");
            }
        }
        
        private static void setTransformProperty(TView view, JArray transforms)
        {
            TransformHelper.processTransform(transforms, sTransformDecompositionArray);
            MatrixMathHelper.decomposeMatrix(sTransformDecompositionArray, ref sMatrixDecompositionContext);

            //Log.Info(ReactConstants.Tag, $"setTransformProperty view[{view.GetTag()}] \n" +
            //    $"\t\tTranslationX={sMatrixDecompositionContext.translation[0]} TranslationY={sMatrixDecompositionContext.translation[1]} \n" +
            //    $"\t\trotationX={sMatrixDecompositionContext.rotationDegrees[0]} rotationY={sMatrixDecompositionContext.rotationDegrees[1]} rotationZ={sMatrixDecompositionContext.rotationDegrees[2]} \n" +
            //    $"\t\tScaleX={sMatrixDecompositionContext.scale[0]} ScaleY={sMatrixDecompositionContext.scale[1]}");

            view.SetMatrix(sMatrixDecompositionContext, false);
        }
        
        private static void resetTransformProperty(TView view)
        {
            view.SetMatrix(null, false);
        }

    }
}
