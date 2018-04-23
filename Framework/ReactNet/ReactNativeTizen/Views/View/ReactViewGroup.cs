using Tizen;
using ReactNative.UIManager;
using ReactNativeTizen.ElmSharp.Extension;

namespace ReactNative.Views.View
{
    /// <summary>
    ///  Backing for a React View. Has support for borders, but since borders aren't common, lazy 
    ///  initializes most of the storage needed for them.
    ///  Need some base classes for providing basic util, such as border, point etc...
    ///  Author:BOY.YANG
    /// </summary>
    /// 
    public class ReactViewBox : ReactBox // 'ReactBox' should act as the a view container here.
    {
        public ReactViewBox(ThemedReactContext reactContext) : 
            base(ReactProgram.RctWindow)
        {
            Log.Info("ReactViewBox", "## ReactViewBox's Contructor "); 
        }
    }
}