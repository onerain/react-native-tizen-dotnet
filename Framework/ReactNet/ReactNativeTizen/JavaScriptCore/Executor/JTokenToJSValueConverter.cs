using Newtonsoft.Json.Linq;
using System;

namespace ReactNative.JavaScriptCore.Executor
{
    sealed class JTokenToJSValueConverter
    {
        private static readonly JTokenToJSValueConverter s_instance =
            new JTokenToJSValueConverter();

        private JTokenToJSValueConverter() { }

        public static JSValue Convert(IntPtr context, JToken token)
        {
            return s_instance.Visit(context, token);
        }

        private JSValue Visit(IntPtr context, JToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            switch (token.Type)
            {
                case JTokenType.Array:
                    return VisitArray(context, (JArray)token);
                case JTokenType.Boolean:
                    return VisitBoolean(context, (JValue)token);
                case JTokenType.Float:
                    return VisitFloat(context, (JValue)token);
                case JTokenType.Integer:
                    return VisitInteger(context, (JValue)token);
                case JTokenType.Null:
				return VisitNull(context, (JValue)token);
                case JTokenType.Object:
                    return VisitObject(context, (JObject)token);
                case JTokenType.String:
                    return VisitString(context, (JValue)token);
                case JTokenType.Undefined:
				return VisitUndefined(context, (JObject)token);
                case JTokenType.Constructor:
                case JTokenType.Property:
                case JTokenType.Comment:
                case JTokenType.Date:
                case JTokenType.Raw:
                case JTokenType.Bytes:
                case JTokenType.Guid:
                case JTokenType.Uri:
                case JTokenType.TimeSpan:
                case JTokenType.None:
                default:
                    throw new NotSupportedException();
            }
        }

        private JSValue VisitArray(IntPtr context, JArray token)
        {
            var n = token.Count;
            var values = new JSValue[n];
            for (var i = 0; i < n; ++i)
            {
				values[i] = Visit(context, token[i]);
            }

			return values[0];
        }

        private JSValue VisitBoolean(IntPtr context, JValue token)
        {
			JSContext Context = new JSContext(context);
			return new JSValue (Context, token.Value<bool>());
        }

        private JSValue VisitFloat(IntPtr context, JValue token)
        {
			JSContext Context = new JSContext(context);
			return new JSValue(Context, token.Value<double>());
        }

        private JSValue VisitInteger(IntPtr context, JValue token)
        {
			JSContext Context = new JSContext(context);
			return new JSValue(Context, token.Value<double>());
        }

        private JSValue VisitNull(IntPtr context, JValue token)
        {
			JSContext Context = new JSContext(context);
			return JSValue.NewNull(Context);
        }

        private JSValue VisitObject(IntPtr context, JObject token)
        {
			JSContext Context = new JSContext(context);
			var jsonObject = new JSObject(Context);

            foreach (var entry in token)
            {
				var value = Visit(context, entry.Value);
                jsonObject.SetProperty(entry.Key, value);
            }

            return jsonObject;
        }

        private JSValue VisitString(IntPtr context, JValue token)
        {
			JSContext Context = new JSContext(context);
			return new JSValue(Context, token.Value<string>());
        }

        private JSValue VisitUndefined(IntPtr context, JObject token)
        {
			JSContext Context = new JSContext(context);
			return JSValue.NewUndefined(Context);
        }
    }
}
