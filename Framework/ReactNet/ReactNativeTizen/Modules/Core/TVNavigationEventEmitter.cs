using ReactNative.Bridge;

namespace ReactNative.Modules.Core
{
    //Just for promise require('NativeModules').TVNavigationEventEmitter is not null
    class TVNavigationEventEmitter : ReactContextNativeModuleBase
    {

        /// <summary>
        /// Instantiates the <see cref="TVNavigationEventEmitter"/> module.
        /// </summary>
        /// <param name="reactContext">The React context.</param>
        public TVNavigationEventEmitter(ReactContext reactContext)
            : base(reactContext)
        {
        }

        /// <summary>
        /// The name of the module.
        /// </summary>
        public override string Name
        {
            get
            {
                return "RCTTVNavigationEventEmitter";
            }
        }
    }
}
