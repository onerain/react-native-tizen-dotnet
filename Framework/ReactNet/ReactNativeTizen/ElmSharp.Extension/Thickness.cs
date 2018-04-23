namespace ReactNativeTizen.ElmSharp.Extension
{
    /// <summary>
    /// Struct defining thickness around the edges of a Rectangle using integer
    /// </summary>
    public struct Thickness
    {

        /// <summary>
        /// Creates a new Thickness object that represents a uniform thickness of size uniformSize.
        /// </summary>
        public Thickness(int uniformSize)
        {
            Left = Right = Top = Bottom = uniformSize;
        }

        /// <summary>
        /// Creates a new Thickness object that has a horizontal thickness of horizontalSize and a vertical thickness of verticalSize.
        /// </summary>
        public Thickness(int horizontal, int vertical)
        {
            Left = Right = horizontal;
            Top = Bottom = vertical;
        }

        /// <summary>
        /// Creates a new Thickness object with thicknesses defined by left, top, right, and bottom.
        /// </summary>
        public Thickness(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// The thickness of the left of a rectangle.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// The thickness of the right of a rectangle.
        /// </summary>
        public int Right { get; set; }

        /// <summary>
        /// The thickness of the top of a rectangle.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// The thickness of the bottom of a rectangle.
        /// </summary>
        public int Bottom { get; set; }

        /// <summary>
        /// The sum of Thickness.Left and Thickness.Right.
        /// </summary>
        public int HorizontalThickness
        {
            get
            {
                return Left + Right;
            }
        }

        /// <summary>
        /// The sum of Thickness.Top and Thickness.Bottom.
        /// </summary>
        public int VerticalThickness
        {
            get
            {
                return Top + Bottom;
            }
        }

        /// <summary>
        /// Returns a string that represents the Thickness values
        /// </summary>
        public override string ToString()
        {
            return string.Format("Left({0}), Top({1}), Right({2}), Bottom({3})", Left, Top, Right, Bottom);
        }

        /// <summary>
        /// Whether the obj is a Thickness with equivalent values.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Thickness)) return false;

            Thickness t = (Thickness)obj;
            return t.Left == Left && t.Right == Right && t.Top == Top && t.Bottom == Bottom;
        }

        /// <summary>
        /// Whether the two given Thickness are equivalent values.
        /// </summary>
        public static bool operator ==(Thickness t1, Thickness t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Whether the two given Thickness are not equivalent values.
        /// </summary>
        public static bool operator !=(Thickness t1, Thickness t2)
        {
            return !t1.Equals(t2);
        }

        /// <summary>
        /// A has value for this Thickness.
        /// </summary>
        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 37 + Left.GetHashCode();
            hash = hash * 37 + Top.GetHashCode();
            hash = hash * 37 + Right.GetHashCode();
            hash = hash * 37 + Bottom.GetHashCode();

            return hash;
        }
    }
}