using ElmSharp;

namespace ReactNativeTizen.ElmSharp.Extension
{
    /// <summary>
    /// A <see cref="VisualElement"/> that occuies most or all of the screen.
    /// </summary>
    public abstract class Page : EvasObject
    {

        private string _title;
        private Background _background;

        /// <summary>
        /// Initializes a new instance of the Page class.
        /// </summary>
        protected Page(ReactWindow win) : base(win)
        {
        }


        /// <summary>
        /// Occurs when the back button was pressed
        /// </summary>
        public virtual void OnBackButtonPressed()
        {
            //Window.Navigation.Pop();
        }

        /// <summary>
        /// Occurs when the title of page is changed.
        /// </summary>
        protected abstract void OnTitleChanged();

        /// <summary>
        /// The title of the Page.
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnTitleChanged();
                }
            }
        }

        /// <summary>
        /// Occurs when the background of page is changed.
        /// </summary>
        protected virtual void OnBackgroundChanged()
        {
        }

        /*
        /// <summary>
        /// Gets the Window of the page is placed.
        /// </summary>
        public new ReactWindow Window
        {
            get { return base.Window; }
        }
        */

        /// <summary>
        /// Gets or sets the background of the Page.
        /// </summary>
        public Background Background
        {
            get { return _background; }
            set
            {
                if (_background != value)
                {
                    _background = value;
                    OnBackgroundChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the padding.
        /// </summary>
        public virtual Thickness Padding { get; set; }
    }
}