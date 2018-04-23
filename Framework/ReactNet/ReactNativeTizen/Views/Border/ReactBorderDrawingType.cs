namespace ReactNative.Views.ReactBorderDrawing
{
    /// <summary>
    /// Border property types.
    /// </summary>

	public enum RCTBorderStyle 
	{ 
		RCTBorderStyleUnset = 0, 
		RCTBorderStyleSolid, 
		RCTBorderStyleDotted, 
		RCTBorderStyleDashed, 
	}; 

	/* Points. */     
	struct CGPoint 
	{  
		internal float x;  
		internal float y;

		public CGPoint(float _x, float _y)
		{
			x = _x;
			y = _y;
		}  
	}; 

	/* Sizes. */   
	public struct CGSize 
	{  
		internal float width;  
		internal float height;

		public CGSize(float w, float h)
		{
			width = w;
			height = h;
		}  
	};


	/* Rectangles. */   
	struct CGRect 
	{  
		internal CGPoint origin; //the origin point related to parent window  
		internal CGSize size; 

		public CGRect(CGPoint ori, CGSize _size)
		{
			origin = ori;
			size = _size;
		} 
	};


	public struct UIEdgeInsets
	{ 
		internal float top; 
		internal float left; 
		internal float bottom; 
		internal float right;

		public UIEdgeInsets(float t, float l, float b, float r)
		{
			top = t;
			left = l;
			bottom = b;
			right = r;
		} 
	};

	struct RCTCornerRadii
	{ 
		internal float topLeft; 
		internal float topRight; 
		internal float bottomLeft; 
		internal float bottomRight; 
	}; 
 
 
	struct RCTCornerInsets
	{ 
		internal CGSize topLeft; 
		internal CGSize topRight; 
		internal CGSize bottomLeft; 
		internal CGSize bottomRight; 
	}; 
 
 
	struct RCTBorderColors
	{ 
		internal uint top; 
		internal uint left; 
		internal uint bottom; 
		internal uint right; 
	}; 

}
