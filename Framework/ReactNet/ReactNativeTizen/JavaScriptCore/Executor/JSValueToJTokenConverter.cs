using Newtonsoft.Json.Linq;
using System;

namespace ReactNative.JavaScriptCore.Executor
{
    sealed class JSValueToJTokenConverter
    {
        private static readonly JToken s_true = new JValue(true);
        private static readonly JToken s_false = new JValue(false);
        private static readonly JToken s_null = JValue.CreateNull();
        private static readonly JToken s_undefined = JValue.CreateUndefined();

        private static readonly JSValueToJTokenConverter s_instance =
            new JSValueToJTokenConverter();

        private JSValueToJTokenConverter() { }

        public static JToken Convert(JSValue value)
        {
            return s_instance.Visit(value);
        }

        private JToken Visit(JSValue value)
        {
            switch (value.JSType)
            {
                case JSType.Array:
                    return VisitArray(value);
                case JSType.Boolean:
                    return VisitBoolean(value);
//                case JSType.Error:
//                    return VisitError(value);
                case JSType.Null:
                    return VisitNull(value);
                case JSType.Number:
                    return VisitNumber(value);
                case JSType.Object:
                    return VisitObject(value);
                case JSType.String:
                    return VisitString(value);
                case JSType.Undefined:
                    return VisitUndefined(value);
                case JSType.Function:
                default:
                    throw new NotSupportedException();
            }
        }

        private JToken VisitArray(JSValue value)
        {
            var array = new JArray();
            var properties = value.ObjectValue.PropertyNames;
            foreach (var property in properties)
            {
                var propertyValue= value.ObjectValue.GetProperty(property);
                array.Add(Visit(propertyValue));
            }

            return array;
        }

        private JToken VisitBoolean(JSValue value)
        {
            return value.BooleanValue ? s_true : s_false;
        }

//        private JToken VisitError(JSValue value)
//        {
//            return new JObject
//            {
//                { "message", Visit(value.GetProperty(JavaScriptPropertyId.FromString("message"))) },
//                { "description", Visit(value.GetProperty(JavaScriptPropertyId.FromString("description"))) },
//                { "stack", Visit(value.GetProperty(JavaScriptPropertyId.FromString("stack"))) },
//            };
//        }

        private JToken VisitNull(JSValue value)
        {
            return s_null;
        }

        private JToken VisitNumber(JSValue value)
        {
            var number = value.NumberValue;

            return number % 1 == 0
                ? new JValue((long)number)
                : new JValue(number);
        }

        private JToken VisitObject(JSValue value)
        {
            var jsonObject = new JObject();
            var properties = value.ObjectValue.PropertyNames;
            foreach (var property in properties)
            {
                var propertyValue= value.ObjectValue.GetProperty(property);
                jsonObject.Add(property, Visit(propertyValue));
            }

            return jsonObject;
        }

        private JToken VisitString(JSValue value)
        {
            return JValue.CreateString(value.ToString());
        }

        private JToken VisitUndefined(JSValue value)
        {
            return s_undefined;
        }
    }
}
