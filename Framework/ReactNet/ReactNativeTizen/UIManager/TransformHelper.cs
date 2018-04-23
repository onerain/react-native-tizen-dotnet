using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using System.Threading;

using static System.FormattableString;

namespace ReactNative.UIManager
{

    /**
     * Class providing helper methods for converting transformation list (as accepted by 'transform'
     * view property) into a transformation matrix.
     */
    public class TransformHelper
    {

        private static ThreadLocal<double[]> sHelperMatrix = new ThreadLocal<double[]>(() =>
        {
            return new double[16];
        });

        private static double convertToRadians(JObject transformMap, string key)
        {
            var value = default(double);
            var inRadians = true;
            var mapValue = transformMap.GetValue(key);
            if (mapValue.Type == JTokenType.String)
            {
                var stringValue = mapValue.Value<string>();
                if (stringValue.EndsWith("rad"))
                {
                    stringValue = stringValue.Substring(0, stringValue.Length - 3);
                }
                else if (stringValue.EndsWith("deg"))
                {
                    inRadians = false;
                    stringValue = stringValue.Substring(0, stringValue.Length - 3);
                }

                value = double.Parse(stringValue);
            }
            else
            {
                value = mapValue.Value<double>();
            }

            return inRadians ? value : MatrixMathHelper.degreesToRadians(value);
        }

        public static void processTransform(JArray transforms, double[] result)
        {
            double[] helperMatrix = sHelperMatrix.Value;
            MatrixMathHelper.resetIdentityMatrix(ref result);
            
            var firstToken = transforms.First;
            if (firstToken.Type != JTokenType.Object)
            {
                for (int i = 0; i < 16; i++)
                {
                    helperMatrix[i] = transforms.Value<double>(i);
                }
                MatrixMathHelper.multiplyInto(ref result, result, helperMatrix);
                return;
            }

            foreach (var transform in transforms)
            {
                var transformMap = (JObject)transform;
                var transformType = transformMap.Properties().SingleOrDefault().Name;

                MatrixMathHelper.resetIdentityMatrix(ref helperMatrix);

                switch (transformType)
                {
                    case "matrix":
                        JArray matrix = (JArray)transformMap.GetValue(transformType);
                        for (int i = 0; i < 16; i++)
                        {
                            helperMatrix[i] = matrix.Value<double>(i);
                        }
                        break;
                    case "perspective":
                        MatrixMathHelper.applyPerspective(ref helperMatrix, transformMap.Value<double>(transformType));
                        break;
                    case "rotateX":
                        MatrixMathHelper.applyRotateX(
                            ref helperMatrix,
                            convertToRadians(transformMap, transformType));
                        break;
                    case "rotateY":
                        MatrixMathHelper.applyRotateY(
                            ref helperMatrix,
                            convertToRadians(transformMap, transformType));
                        break;
                    case "rotate":
                    case "rotateZ":
                        MatrixMathHelper.applyRotateZ(
                            ref helperMatrix,
                            convertToRadians(transformMap, transformType));
                        break;
                    case "scale":
                        var scale = transformMap.Value<double>(transformType);
                        MatrixMathHelper.applyScaleX(ref helperMatrix, scale);
                        MatrixMathHelper.applyScaleY(ref helperMatrix, scale);
                        break;
                    case "scaleX":
                        MatrixMathHelper.applyScaleX(ref helperMatrix, transformMap.Value<double>(transformType));
                        break;
                    case "scaleY":
                        MatrixMathHelper.applyScaleY(ref helperMatrix, transformMap.Value<double>(transformType));
                        break;
                    case "translate":
                        var value = (JArray)transformMap.GetValue(transformType);
                        var x = value.Value<double>(0);
                        var y = value.Value<double>(1);
                        var z = value.Count > 2 ? value.Value<double>(2) : 0.0;
                        MatrixMathHelper.applyTranslate3D(ref helperMatrix, x, y, z);
                        break;
                    case "translateX":
                        MatrixMathHelper.applyTranslate2D(ref helperMatrix, transformMap.Value<double>(transformType), 0.0);
                        break;
                    case "translateY":
                        MatrixMathHelper.applyTranslate2D(ref helperMatrix, 0.0, transformMap.Value<double>(transformType));
                        break;
                    case "skewX":
                        MatrixMathHelper.applySkewX(
                            ref helperMatrix,
                            convertToRadians(transformMap, transformType));
                        break;
                    case "skewY":
                        MatrixMathHelper.applySkewY(
                            ref helperMatrix,
                            convertToRadians(transformMap, transformType));
                        break;
                    default:
                        throw new InvalidOperationException(
                            Invariant($"Unsupported transform type: '{transformType}'"));
                }
                

                MatrixMathHelper.multiplyInto(ref result, result, helperMatrix);
            }
        }
    }

}
