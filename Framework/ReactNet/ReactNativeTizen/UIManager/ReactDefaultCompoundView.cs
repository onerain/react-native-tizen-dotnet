using ElmSharp;
using ReactNativeTizen.ElmSharp.Extension;

namespace ReactNative.UIManager
{
    class ReactDefaultCompoundView : IReactCompoundView
    {
        public int GetReactTagAtPoint(Widget reactView, Point point)
        {
            return reactView.GetTag();
        }
    }
}
