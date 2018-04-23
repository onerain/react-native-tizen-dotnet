using System;

namespace ReactNative.UIManager
{
    static class ReactStylesDiffMapExtensions
    {
        public static T GetProperty<T>(this ReactStylesDiffMap props, string name)
        {
            var property = props.GetProperty(name, typeof(T));
            if (property == null)
            {
                throw new Exception("ReactStylesDiffMapExtensions GetProperty error, property is null");
            }
            return (T)property;
        }

        public static object GetProperty(this ReactStylesDiffMap props, string name, Type type)
        {
            return props.GetProperty(name)?.ToObject(type);
        }
    }
}
