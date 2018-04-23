using ElmSharp;

namespace ReactNative.UIManager
{
    interface IPropertySetter
    {
        string Name { get; }

        string PropertyType { get; }

        void UpdateShadowNodeProperty(ReactShadowNode shadowNode, ReactStylesDiffMap value);

        void UpdateViewManagerProperty(IViewManager viewManager, EvasObject view, ReactStylesDiffMap value);
    }
}
