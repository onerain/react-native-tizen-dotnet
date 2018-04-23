using System;

using ElmSharp;
using ReactNativeTizen.ElmSharp.Extension;

namespace ReactNative.UIManager
{
    /// <summary>
    /// allows registering for size change events. 
    /// The main purpose for this class is to hide complexity of ReactRootView
    /// BOY.YANG
    /// </summary>
    public class SizeMonitoringCanvas : Canvas
    {
        private EventHandler _sizeChangedEventHandler;

        public SizeMonitoringCanvas(EvasObject evasObj) : base(evasObj) { }

        /// <summary>
        /// Sets and registers the event handler responsible for monitoring
        /// size change events.
        /// </summary>
        public void SetOnSizeChangedListener(EventHandler sizeChangedEventHandler)
        {
            // TODO: Layout need to provide sizeChanged event  
            var current = _sizeChangedEventHandler;
            if (current != null)
            {
                Resized -= current;
            }

            if (sizeChangedEventHandler != null)
            {
                _sizeChangedEventHandler = sizeChangedEventHandler;
                Resized += _sizeChangedEventHandler;
            }
        }

        /// <summary>
        /// UnSets the size changed event handler.
        /// </summary>
        public void RemoveSizeChanged()
        {
            var current = _sizeChangedEventHandler;
            if (current != null)
            {
                Resized -= current;
            }

            _sizeChangedEventHandler = null;
        }
    }
}
