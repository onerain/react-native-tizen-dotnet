using System;

namespace ReactNativeTizen.ElmSharp.Extension
{
    /// <summary>
    /// Holds information about size of the area which can be used for layout.
    /// </summary>
    public class LayoutEventArgs : EventArgs
    {
        /// <summary>
        /// Whether or not the dimensions have changed.
        /// </summary>
        public bool HasChanged
        {
            get;
            internal set;
        }

        /// <summary>
        /// X coordinate of the layout area, relative to the main window.
        /// </summary>
        public int X
        {
            get;
            internal set;
        }

        /// <summary>
        /// Y coordinate of the layout area, relative to the main window.
        /// </summary>
        public int Y
        {
            get;
            internal set;
        }

        /// <summary>
        /// Width of the layout area.
        /// </summary>
        public int Width
        {
            get;
            internal set;
        }

        /// <summary>
        /// Height of the layout area.
        /// </summary>
        public int Height
        {
            get;
            internal set;
        }
    }
}