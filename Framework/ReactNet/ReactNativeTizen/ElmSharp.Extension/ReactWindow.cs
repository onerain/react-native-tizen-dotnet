using System;
using ElmSharp;
using ReactNative;
using Tizen;
using ReactNative.Common;

namespace ReactNativeTizen.ElmSharp.Extension
{
    /// <summary>
    /// Window extensions with ( conformant & navi ) by BOY.YANG
    /// Following Tizen basic UI Layout
    /// Window
    /// - Conformant
    ///   - Naviframe    ---> actually, it's not necessary
    ///     - Canvas
    ///        - Widgets
    /// </summary>
    public class ReactWindow : Window
    {
        private Conformant _conformant;
        //private Naviframe _naviframe;

        /// <summary>
        /// Initializes a new instance of the Window class.
        /// </summary>
        public ReactWindow(string name) : base(name)
        {
            Log.Info(ReactConstants.Tag, "## Contruct ReactWindow [BGN] ## ");

            // size
            //var size = ScreenSize;
            //Resize(size.Width, size.Height);
            //BackgroundColor = Color.Gray;

            // events
            /*
            Deleted += (sender, e) =>
            {
                Closed?.Invoke(this, EventArgs.Empty);
            };
            CloseRequested += (sender, e) =>
            {
                Unrealize();
            };

            KeyGrab(EvasKeyEventArgs.PlatformBackButtonName, false);
            KeyUp += (s, e) =>
            {
                if (e.KeyName == EvasKeyEventArgs.PlatformBackButtonName)
                {
                    BackButtonPressed?.Invoke(this, EventArgs.Empty);
                }
            };
            */
            // active window
            //Active();

            //AutoDeletion = false;   // fuck off here
            //Show();
            this.KeyGrab(EvasKeyEventArgs.PlatformBackButtonName, false);
            this.KeyGrab("XF86Red", false);
            this.KeyUp += (s, e) =>
            {
                if (e.KeyName == EvasKeyEventArgs.PlatformBackButtonName)
                {
                    Log.Info(ReactConstants.Tag, "============================Back key is pressed");
                    BackButtonPressed?.Invoke(this, EventArgs.Empty);
                }
                else if (e.KeyName == "XF86Red")
                {
                    Log.Info(ReactConstants.Tag, "============================Red key is pressed");
                    RedButtonPressed?.Invoke(this, EventArgs.Empty);
                }
            };
            // conformant container 
            _conformant = new Conformant(this);
            _conformant.SetAlignment(-1.0, -1.0);   // fill
            _conformant.SetWeight(1.0, 1.0);        // expand
            _conformant.Show();

            Log.Info(ReactConstants.Tag, "## Contruct ReactWindow [END] ## ");
            // 
            // Initialize();
        }

        /// <summary>
        /// Notifies that the window has been closed.
        /// </summary>
        //public event EventHandler Closed;

        /// <summary>
        /// Notifies that the back button has been pressed.
        /// </summary>
        public new event EventHandler BackButtonPressed;


        public event EventHandler RedButtonPressed;
        /// <summary>
        /// Gets the current orientation.
        /// </summary>
        public DisplayOrientations CurrentOrientation
        {
            get
            {
                if (IsRotationSupported)
                {
                    return GetDisplayOrientation();
                }
                else
                {
                    return DisplayOrientations.None;
                }
            }
        }

        /// <summary>
        /// Gets or sets the orientation of a rectangular screen.
        /// </summary>
        public DisplayOrientations AvailableOrientations
        {
            get
            {
                if (IsRotationSupported)
                {
                    return (DisplayOrientations)AvailableRotations;
                }
                else
                {
                    return DisplayOrientations.None;
                }
            }
            set
            {
                if (IsRotationSupported)
                {
                    AvailableRotations = (DisplayRotation)value;
                }
            }
        }
/*
        /// <summary>
        /// INavigation Object to handle page stack
        /// </summary>
        public Naviframe Navigator
        {
            get { return _naviframe; }
        }
*/

        /// <summary>
        /// Sets the main page of Window.
        /// </summary>
        /// <param name="content">ElmSharp.EvasObject type page to be set.</param>
        public void SetMainPage(EvasObject content)
        {
            Log.Info(ReactConstants.Tag, "### Main Page:"+content);

            // Navigator.Push(content, "Instagram");

            _conformant.SetContent(content);
        }

        void Initialize()
        {
            this.KeyGrab(EvasKeyEventArgs.PlatformBackButtonName, false);
            this.KeyGrab("XF86Red", false);
            this.KeyUp += (s, e) =>
            {
                if (e.KeyName == EvasKeyEventArgs.PlatformBackButtonName)
                {
                    Log.Info(ReactConstants.Tag, "============================Back key is pressed");
                    BackButtonPressed?.Invoke(this, EventArgs.Empty);
                }
                else if (e.KeyName == "XF86Red")
                {
                    Log.Info(ReactConstants.Tag, "============================Red key is pressed");
                    RedButtonPressed?.Invoke(this, EventArgs.Empty);
                }
            };
            // conformant container 
            _conformant = new Conformant(this);
            _conformant.SetAlignment(-1.0, -1.0);   // fill
            _conformant.SetWeight(1.0, 1.0);        // expand
            _conformant.Show();

#region "Navigator navigator container "
            /*
            _naviframe = new Naviframe(this)
            {
                PreserveContentOnPop = true,
                DefaultBackButtonEnabled = true
            };

            _naviframe.Popped += (s, e) =>
            {
                Log.Info(ReactConstants.Tag, "Naviframe was popped: "+(int)(IntPtr)e.Content);
            };

            _naviframe.Show();

            // include navi
            _conformant.SetContent(_naviframe);
            */
#endregion
            AvailableOrientations = DisplayOrientations.Portrait | DisplayOrientations.Landscape | DisplayOrientations.PortraitFlipped | DisplayOrientations.LandscapeFlipped;
        }
        DisplayOrientations GetDisplayOrientation()
        {
            switch (Rotation)
            {
                case 0:
                    return DisplayOrientations.Portrait;

                case 90:
                    return DisplayOrientations.Landscape;

                case 180:
                    return DisplayOrientations.PortraitFlipped;

                case 270:
                    return DisplayOrientations.LandscapeFlipped;

                default:
                    return DisplayOrientations.None;
            }
        }
    }
}