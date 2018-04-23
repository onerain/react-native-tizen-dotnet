using System;
using System.Collections.Generic;
using ElmSharp;

using Tizen;
using ReactNative.Common;

using ReactNative.UIManager;
using System.Collections;

namespace ReactNativeTizen.ElmSharp.Extension
{
    /// <summary>
    /// Extends the ElmSharp.Box class with functionality useful to React Native renderer.
    /// </summary>
    /// <remarks>
    /// This class overrides the layout mechanism. Instead of using the native layout,
    /// <c>LayoutUpdated</c> event is sent.
    /// </remarks>
    public class ReactBox : Box
	{
		/// <summary>
		/// The last processed geometry of the Box which was reported from the native layer.
		/// </summary>
		Rect _previousGeometry;

        Label forFocus;
        bool setTouchabled;

        public event EventHandler FocusEventHandler;
        public event EventHandler BlurEventHandler;
        public event EventHandler PressEventHandler;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="parent">The parent EvasObject.</param>
        public ReactBox(EvasObject parent) : base(parent)
		{
            Resized += NotifyOnLayout;
            Moved += NotifyOnLayout;
            SetLayoutCallback(() => { NotifyOnLayout(this, null); });

            forFocus = null;
            setTouchabled = false;
        }

        protected override void OnUnrealize()
        {
            if (forFocus != null)
            {
                UnPack(forFocus);
                forFocus.Focused -= FocusEvent;
                forFocus.Unfocused -= BlurEvent;
                forFocus.KeyDown -= KeyDownEvent;
                //forFocus.KeyUp -= PressEvent;
                forFocus.AllowFocus(false);
                forFocus.Hide();
                forFocus.Unrealize();
            }
        }

        /// <summary>
        /// Notifies that the layout has been updated.
        /// </summary>
        public event EventHandler<LayoutEventArgs> LayoutUpdated;
        
        public bool Touchable
        {
            set
            {
                if (value && !setTouchabled)
                {
                    //Log.Info("ZK_TEST", "Tag:" + this.GetTag() + " forFocus == null  -> " + (forFocus == null));
                    if(forFocus == null)
                    {
                        forFocus = new Label(this);
                        forFocus.Show();
                        PackEnd(forFocus);
                    }
                    forFocus.AllowFocus(value);
                    forFocus.Focused += FocusEvent;
                    forFocus.Unfocused += BlurEvent;
                    forFocus.KeyDown += KeyDownEvent;
                    //forFocus.KeyUp += PressEvent;
                    setTouchabled = true;
                }
                else if(!value && setTouchabled)
                {
                    if (forFocus != null)
                    {
                        UnPack(forFocus);
                        forFocus.Focused -= FocusEvent;
                        forFocus.Unfocused -= BlurEvent;
                        forFocus.KeyDown -= KeyDownEvent;
                        //forFocus.KeyUp -= PressEvent;
                        forFocus.AllowFocus(value);
                        forFocus.Hide();
                        forFocus.Unrealize();
                    }
                    setTouchabled = false;
                }
            }
            get
            {
                return setTouchabled;
            }
        }

        void NotifyOnResized(object sender, EventArgs e)
        {
            if (forFocus == null)
                return;
            var s = sender as ReactBox;

            forFocus.Move(s.GetAbsoluteX(), s.GetAbsoluteY());
            forFocus.Resize((int)s.GetDimensions().Width, (int)s.GetDimensions().Height);

            return;
        }


        public virtual void FocusEvent(object sender, EventArgs e)
        {
            //Log.Info("ZK_TEST", "Tag:" + this.GetTag() + " Focus");
            FocusEventHandler?.Invoke(this, e);
        }

        public virtual void BlurEvent(object sender, EventArgs e)
        {
            //Log.Info("ZK_TEST", "Tag:" + this.GetTag() + " Blur");
            BlurEventHandler?.Invoke(this, e);
        }

        public virtual void KeyDownEvent(object sender, EvasKeyEventArgs e)
        {
            Log.Info("ZK_TEST", "Tag:" + this.GetTag() + " " + e.KeyName);
            if(e.KeyName == "Return")
                PressEventHandler?.Invoke(this, e);
        }

        /// <summary>
        /// Triggers the <c>LayoutUpdated</c> event.
        /// </summary>
        /// <remarks>
        /// This method is called whenever there is a possibility that the size and/or position has been changed.
        /// </remarks>
        void NotifyOnLayout(object sender, EventArgs e)
        {
            var g = Geometry;
            if (0 == g.Width || 0 == g.Height)
            {
                // ignore irrelevant dimensions
                return;
            }

            if(g != _previousGeometry)
            {
                NotifyOnResized(sender, e);
                    
            }
            //if(this.HasTag())
            //    Log.Info(ReactConstants.Tag, "[BGN] " + this.GetTag() + "Geometry X:" + Geometry.X + ", Y:" + Geometry.Y + ", Width:" + Geometry.Width + ", Height:" + Geometry.Height);

            // Update child's layout
            foreach (Widget _childView in _childrenView)
            {
                //if(_childrenView[nIndex].HasTag())
                //    Log.Info(ReactConstants.Tag, "Current Child " + _childrenView[nIndex].GetTag() + ":" + _childrenView[nIndex] + ", index:" + nIndex);
                Rect area = CalcLayoutArea();   // Currently, it's useless.
                LayoutChild(_childView, area);
            }

            if (null != LayoutUpdated)
            {
                //Log.Info(ReactConstants.Tag, "Geometry x=" + g.X, ", y=" + g.Y);

                LayoutUpdated(this, new LayoutEventArgs()
                {
                    HasChanged = g != _previousGeometry,
                    X = g.X,
                    Y = g.Y,
                    Width = g.Width,
                    Height = g.Height,
                }
                );
            }

            _previousGeometry = g;

            //Log.Info(ReactConstants.Tag, "[END] Geometry X:" + g.X + ", Y:" + g.Y + ", Width:" + g.Width + ", Height:" + g.Height);
        }

        /// <summary>
        /// Calculates the layout area of this layout.
        /// </summary>
        private Rect CalcLayoutArea()
        {
            //Log.Info(ReactConstants.Tag, "### Enter CalcLayoutArea ###");

            // The size of rectangular area should be at least as layout's padding.
            Size size = new Size
            {
                Width = Padding.HorizontalThickness,
                Height = Padding.VerticalThickness,
            };

            /*
            System.Console.WriteLine("Children.Count is {0}", Children);

            // The size of content area should be at least the size of largest child.
            foreach (Widget view in _childrenView)
            {
                Rect requestedRect = LayoutHelper.GetRequestedRect(view.LayoutParam, null, view);
                requestedRect.X += Padding.Left;
                requestedRect.Y += Padding.Top;
                System.Console.WriteLine("CalcLayoutArea view is {0} x:{1} y:{2} h:{3} w:{4}", view.GetType().Name, requestedRect.X, requestedRect.X, requestedRect.Height, requestedRect.Width);

                if (requestedRect.Width >= 0)
                {
                    if (size.Width < requestedRect.Right + Padding.HorizontalThickness)
                    {
                        size.Width = requestedRect.Right + Padding.HorizontalThickness;
                    }
                }
                if (requestedRect.Height >= 0)
                {
                    if (size.Height < requestedRect.Bottom + Padding.VerticalThickness)
                    {
                        size.Height = requestedRect.Bottom + Padding.VerticalThickness;
                    }
                }
            }

            // The size of content area should be at least the size of minimum request size of this layout.
            if (size.Width < Minimum.Width)
            {
                size.Width = Minimum.Width;
            }
            if (size.Height < Minimum.Height)
            {
                size.Height = Minimum.Height;
            }

            // The size of content area should be smaller than the size of maximum request size of this layout.
            if (Maximum.Width >= 0 && size.Width > Maximum.Width)
            {
                size.Width = Maximum.Width;
            }
            if (Maximum.Height >= 0 && size.Height > Maximum.Height)
            {
                size.Height = Maximum.Height;
            }

            // Set minimum
            Minimum = size;

            // At least the Actual size
            if (size.Width < Width)
            {
                size.Width = Width;
            }
            if (size.Height < Height)
            {
                size.Height = Height;
            }
            */

            //Log.Info(ReactConstants.Tag, "### Exit CalcLayoutArea ###");
            return new Rect
            {
                X = 10,
                Y = 10,
                Width = 200,
                Height = 10
            };
        }

        private Rect GetLayoutRect(Widget view, Rect layoutArea)
        {
            Rect layoutRect = new Rect
            {
                //X = view.GetAbsoluteX(),
                //Y = view.GetAbsoluteY(),
                X = Geometry.X + (int)view.GetDimensions().X,
                Y = Geometry.Y + (int)view.GetDimensions().Y,
                Width = (int)view.GetDimensions().Width,
                Height = (int)view.GetDimensions().Height,
            };

            /*
                // Take account alignment.

                // Horizontal alignment.
                if (!param.RequestedWidth.IsMatchParent)
                {
                    if (param.IsAlignLeftWithParent && param.IsAlignRightWithParent)
                    {
                        layoutRect.X = 0;
                        layoutRect.Width = layoutArea.Width;
                    }
                    else if (param.IsAlignLeftWithParent)
                    {
                        layoutRect.X = 0;
                    }
                    else if (param.IsAlignRightWithParent)
                    {
                        layoutRect.X = layoutArea.Width - layoutRect.Width;
                    }
                    else if (param.IsAlignHorizontalCenterWithParent)
                    {
                        layoutRect.X = (layoutArea.Width - layoutRect.Width) / 2;
                    }
                }

                // Vertical alignment.
                if (!param.RequestedHeight.IsMatchParent)
                {
                    if (param.IsAlignTopWithParent && param.IsAlignBottomWithParent)
                    {
                        layoutRect.Y = 0;
                        layoutRect.Height = layoutArea.Height;
                    }
                    else if (param.IsAlignTopWithParent)
                    {
                        layoutRect.Y = 0;
                    }
                    else if (param.IsAlignBottomWithParent)
                    {
                        layoutRect.Y = layoutArea.Height - layoutRect.Height;
                    }
                    else if (param.IsAlignVerticalCenterWithParent)
                    {
                        layoutRect.Y = (layoutArea.Height - layoutRect.Height) / 2;
                    }
                }

                layoutRect.X += X + Padding.Left;
                layoutRect.Y += Y + Padding.Top;
        */
            layoutRect.X += Padding.Left;
            layoutRect.Y += Padding.Top;

            return layoutRect;
        }

        private void LayoutChild(Widget view, Rect layoutArea)
        {
            Rect layoutRect = GetLayoutRect(view, layoutArea);

            view.Move(layoutRect.X, layoutRect.Y);
            view.Resize(layoutRect.Width, layoutRect.Height);

            //When Layout change, need set Matrix again
            if(view.IsSetMatrix())
            {
                var matrix = view.GetMatrix();

                //Log.Info(ReactConstants.Tag, $"setTransformProperty again view[{view.GetTag()}] \n" +
                //    $"\t\tTranslationX={matrix.translation[0]} TranslationY={matrix.translation[1]} \n" +
                //    $"\t\trotationX={matrix.rotationDegrees[0]} rotationY={matrix.rotationDegrees[1]} rotationZ={matrix.rotationDegrees[2]} \n" +
                //    $"\t\tScaleX={matrix.scale[0]} ScaleY={matrix.scale[1]}");

                view.SetMatrix(matrix, true);
            }

            //if(view.HasTag())
            //    Log.Info(ReactConstants.Tag, "### LayoutChild ### view " + view.GetTag() + ":" + view + " tag="+ view.GetTag() + ", {X=" + layoutRect.X + " Y=" + layoutRect.Y
            //        + " Width=" + layoutRect.Width + " Height=" + layoutRect.Height + "}");
        }

        private void OnLayout(IntPtr obj, IntPtr priv, IntPtr userData)
        {
            Rect area = CalcLayoutArea();
        }

#region New Encapsulation for React Native by BOY.YANG 2017-02-16
        // Extension Here 

        /// <summary>
        /// Gets or sets the paddings of the FrameLayout
        /// </summary>
        public Thickness Padding { get; set; }

        ArrayList _childrenView = new ArrayList(); // children with index for 'RN' 

        internal void AddChild(Widget obj, int index)
        {
            //Log.Info(ReactConstants.Tag, "[AddChild] child:"+obj+", index:"+ index);
            _childrenView.Insert(index, obj);
            obj.SetParent(this);
        }

        internal void RemoveChildAt(int index)
        {
            //Log.Info(ReactConstants.Tag, "[RemoveChildAt] index:" + index);
            ((Widget)_childrenView[index]).SetParent(null);
            _childrenView.RemoveAt(index);
        }

        internal EvasObject GetChildAt(int index)
        {
            //Log.Info(ReactConstants.Tag, "[GetChildAt] index:" + index);
            return (EvasObject)(_childrenView[index]);
        }

        internal void ClearAllChildren()
        {
            //Log.Info(ReactConstants.Tag, "[ClearAllChildren]");
            _childrenView.Clear();
        }

        internal int GetChildrenCount()
        {
            //Log.Info(ReactConstants.Tag, "[GetChildrenCount] count:" + _childrenView.Count);
            return _childrenView.Count;
        }
#endregion

    }
}