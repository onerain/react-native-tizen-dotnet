using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using ReactNative.UIManager.Events;

using ElmSharp;
using Tizen;
using ReactNative.Common;

//using System.Threading;
using System.Runtime.InteropServices;

namespace ReactNative.Views.ReactBorderDrawing
{
    public class ReactBorderDrawingManager : SimpleViewManager<Image>
    {
        private const string ReactClass = "RCTBorderImage";

        //for cairo define
        
        IntPtr img;        

        IntPtr surface;
        IntPtr cairo;
		
		IntPtr pathOuter;
		IntPtr pathInner;
		
		string pathImage;
		  

        /* change angle to arc */
        double ANGLE(double ang)
        {
            return (ang * Math.PI / 180.0);
        }		
		

        // ref with React IOS platform
		const double RCTViewBorderThreshold = 0.001; 
		CGPoint CGPointZero = new CGPoint(0,0);
 
		bool RCTBorderInsetsAreEqual(UIEdgeInsets borderInsets) 
		{ 
			return 
			Math.Abs(borderInsets.left - borderInsets.right) < RCTViewBorderThreshold && 
			Math.Abs(borderInsets.left - borderInsets.bottom) < RCTViewBorderThreshold && 
			Math.Abs(borderInsets.left - borderInsets.top) < RCTViewBorderThreshold; 
		} 
 
 
		bool RCTCornerRadiiAreEqual(RCTCornerRadii cornerRadii) 
		{ 
			return 
			Math.Abs(cornerRadii.topLeft - cornerRadii.topRight) < RCTViewBorderThreshold && 
			Math.Abs(cornerRadii.topLeft - cornerRadii.bottomLeft) < RCTViewBorderThreshold && 
			Math.Abs(cornerRadii.topLeft - cornerRadii.bottomRight) < RCTViewBorderThreshold; 
		} 
 
 
		bool RCTBorderColorsAreEqual(RCTBorderColors borderColors) 
		{ 
			return 
			(borderColors.left==borderColors.right) && 
			(borderColors.left==borderColors.top) && 
			(borderColors.left==borderColors.bottom); 
		} 
 
 
		static RCTCornerInsets RCTGetCornerInsets(RCTCornerRadii cornerRadii, 
										UIEdgeInsets edgeInsets) 
		{   

			// calculate the corner rect inset
			RCTCornerInsets cornerInsets;
			cornerInsets.topLeft.width = Math.Max(0, cornerRadii.topLeft - edgeInsets.left);
			cornerInsets.topLeft.height = Math.Max(0, cornerRadii.topLeft - edgeInsets.top);

			cornerInsets.topRight.width = Math.Max(0, cornerRadii.topRight - edgeInsets.right);
			cornerInsets.topRight.height = Math.Max(0, cornerRadii.topRight - edgeInsets.top);

			cornerInsets.bottomLeft.width = Math.Max(0, cornerRadii.bottomLeft - edgeInsets.left);
			cornerInsets.bottomLeft.height = Math.Max(0, cornerRadii.bottomLeft - edgeInsets.bottom);

			cornerInsets.bottomRight.width = Math.Max(0, cornerRadii.bottomRight - edgeInsets.right);
			cornerInsets.bottomRight.height = Math.Max(0, cornerRadii.bottomRight - edgeInsets.bottom);


			return cornerInsets;

		} 
 
		
 
		float RCTPathAddEllipticArc(IntPtr cr, 
								   IntPtr m, 
								   CGPoint origin, 
								   CGSize size, 
								   double startAngle, 
								   double endAngle, 
								   bool clockwise
								   ) 
		{ 
			float radius = 0; 
			if (size.width != 0) 
			{ 
				radius = size.width; 
			} 
			else if (size.height != 0) 
			{ 
				radius = size.height; 
			} 

			Log.Info(ReactConstants.Tag, "[RCTPathAddEllipticArc] ReactBorderDrawingManager , radius:" + radius );
			Log.Info(ReactConstants.Tag, "[RCTPathAddEllipticArc] size.width:" + size.width + ", size.height:" + size.height );
			Log.Info(ReactConstants.Tag, "[RCTPathAddEllipticArc] origin.x:" + origin.x + ", origin.y:" + origin.y + ", startAngle:" + startAngle + ", endAngle:" + endAngle);	

			// add a new arc path here in cairo context
			Cairo.cairo_arc(cr, origin.x, origin.y, radius, startAngle, endAngle);   

			//Cairo.cairo_stroke(cairo);
			return radius;
		}

		float RectGetMinX(CGRect rect)
		{
			return rect.origin.x;
		}

		float RectGetMinY(CGRect rect)
		{
			return rect.origin.y;
		} 

		float RectGetMaxX(CGRect rect)
		{
			return rect.origin.x+rect.size.width;
		} 

		float RectGetMaxY(CGRect rect)
		{
			return rect.origin.y+rect.size.height;
		}  

		float RectGetMidX(CGRect rect)
		{
			return rect.origin.x+rect.size.width/2;
		}

		float RectGetMidY(CGRect rect)
		{
			return rect.origin.y+rect.size.height/2;
		}
 

		IntPtr RCTPathCreateWithRoundedRect(CGRect bounds, 
											RCTCornerInsets cornerInsets 
										) 
		{
			Log.Info(ReactConstants.Tag, "[RCTPathCreateWithRoundedRect] ReactBorderDrawingManager::RCTPathCreateWithRoundedRect BGN ");
			Log.Info(ReactConstants.Tag, "[RCTPathCreateWithRoundedRect] bounds.origin.x:" + bounds.origin.x + ", bounds.origin.y:" + bounds.origin.y + ", bounds.size.width:" + bounds.size.width + ", bounds.size.height:" + bounds.size.height);

			float minX = RectGetMinX(bounds); 
			float minY = RectGetMinY(bounds); 
			float maxX = RectGetMaxX(bounds); 
			float maxY = RectGetMaxY(bounds); 


			CGSize topLeft = new CGSize
			( 
				Math.Max(0, Math.Min(cornerInsets.topLeft.width, bounds.size.width - cornerInsets.topRight.width)), 
				Math.Max(0, Math.Min(cornerInsets.topLeft.height, bounds.size.height - cornerInsets.bottomLeft.height)) 
			); 
			CGSize topRight = new CGSize
			( 
				Math.Max(0, Math.Min(cornerInsets.topRight.width, bounds.size.width - cornerInsets.topLeft.width)), 
				Math.Max(0, Math.Min(cornerInsets.topRight.height, bounds.size.height - cornerInsets.bottomRight.height)) 
			); 
			CGSize bottomLeft = new CGSize
			( 
				Math.Max(0, Math.Min(cornerInsets.bottomLeft.width, bounds.size.width - cornerInsets.bottomRight.width)), 
				Math.Max(0, Math.Min(cornerInsets.bottomLeft.height, bounds.size.height - cornerInsets.topLeft.height)) 
			); 
			CGSize bottomRight = new CGSize
			( 
				Math.Max(0, Math.Min(cornerInsets.bottomRight.width, bounds.size.width - cornerInsets.bottomLeft.width)), 
				Math.Max(0, Math.Min(cornerInsets.bottomRight.height, bounds.size.height - cornerInsets.topRight.height)) 
			); 


			CGPoint p1 = new CGPoint
			( 
				minX + topLeft.width, minY + topLeft.height 
			);
			CGPoint p2 = new CGPoint
			( 
				maxX - topRight.width, minY + topRight.height 
			);
			CGPoint p3 = new CGPoint
			( 
				maxX - bottomRight.width, maxY - bottomRight.height 
			);
			CGPoint p4 = new CGPoint
			( 
				minX + bottomLeft.width, maxY - bottomLeft.height 
			);   


			// begin a new cairo drawing path without current point
			Cairo.cairo_new_sub_path(cairo);

			RCTPathAddEllipticArc(cairo, IntPtr.Zero, p1, topLeft, Math.PI, 3 * Math.PI/2, false); 
			RCTPathAddEllipticArc(cairo, IntPtr.Zero, p2, topRight, 3 * Math.PI/2, 0, false); 
			RCTPathAddEllipticArc(cairo, IntPtr.Zero, p3, bottomRight, 0, Math.PI/2, false); 
			RCTPathAddEllipticArc(cairo, IntPtr.Zero, p4, bottomLeft, Math.PI/2, Math.PI, false);		

			/// auto close the current by draw a line to the init point
			Cairo.cairo_close_path(cairo);

			IntPtr path = Cairo.cairo_copy_path(cairo);
			//Cairo.cairo_stroke(cairo);

			return path; 
		} 
 
 
		void RCTEllipseGetIntersectionsWithLine(CGRect ellipseBounds, 
												CGPoint lineStart, 
												CGPoint lineEnd, 
												CGPoint[] intersections) 
		{ 

			CGPoint ellipseCenter = new CGPoint
			( 
				RectGetMidX(ellipseBounds), 
				RectGetMidY(ellipseBounds) 
			); 


			lineStart.x -= ellipseCenter.x; 
			lineStart.y -= ellipseCenter.y; 
			lineEnd.x -= ellipseCenter.x; 
			lineEnd.y -= ellipseCenter.y; 


			float m = (lineEnd.y - lineStart.y) / (lineEnd.x - lineStart.x); 
			float a = ellipseBounds.size.width / 2; 
			float b = ellipseBounds.size.height / 2; 
			float c = lineStart.y - m * lineStart.x; 
			float A = (b * b + a * a * m * m); 
			float B = 2 * a * a * c * m; 
			double D = Math.Sqrt((a * a * (b * b - c * c)) / A + Math.Pow(B / (2 * A), 2)); 


			float x_ = -B / (2 * A); 
			double x1 = x_ + D; 
			double x2 = x_ - D; 
			double y1 = m * x1 + c; 
			double y2 = m * x2 + c; 


			intersections[0].x = (float)(x1 + ellipseCenter.x); 
			intersections[0].y = (float)(y1 + ellipseCenter.y);

			intersections[1].x = (float)(x2 + ellipseCenter.x);
			intersections[1].y = (float)(y2 + ellipseCenter.y); 
		} 
 
 
		static bool RCTCornerRadiiAreAboveThreshold(RCTCornerRadii cornerRadii) 
		{ 
			return (cornerRadii.topLeft > RCTViewBorderThreshold || 
			cornerRadii.topRight > RCTViewBorderThreshold      || 
			cornerRadii.bottomLeft > RCTViewBorderThreshold    || 
			cornerRadii.bottomRight > RCTViewBorderThreshold); 
		} 
 
 
		void RCTPathCreateOuterOutline(bool drawToEdge, CGRect rect, RCTCornerRadii cornerRadii) 
		{ 
			if (drawToEdge) 
			{
				Log.Info(ReactConstants.Tag, "[RCTPathCreateOuterOutline] ReactBorderDrawingManager , cairo:" + cairo );
				Log.Info(ReactConstants.Tag, "[RCTPathCreateOuterOutline] rect.origin.x:" + rect.origin.x + ", rect.origin.y:" + rect.origin.y + ", rect.size.width:" + rect.size.width + ", rect.size.height:" + rect.size.height);

				// add a new rectangle path here in cairo context
				Cairo.cairo_rectangle(cairo,rect.origin.x,rect.origin.y,rect.size.width,rect.size.height);

				pathOuter = Cairo.cairo_copy_path(cairo);
				//Cairo.cairo_stroke(cairo);	 

				return;	 
			} 

			//RCTPathCreateWithRoundedRect(rect, RCTGetCornerInsets(cornerRadii, new UIEdgeInsets(0,0,0,0)));
			pathOuter = RCTPathCreateWithRoundedRect(rect, RCTGetCornerInsets(cornerRadii, new UIEdgeInsets(0,0,0,0)));

		} 
 
 
		public void RCTUIGraphicsBeginImageContext(CGSize size, Color backgroundColor, bool hasCornerRadii, bool drawToEdge) 
		{ 

			/// create cairo surface and context here
			surface = Cairo.cairo_image_surface_create(0, 1920, 1080);
			cairo = Cairo.cairo_create(surface);

			/* clear background as white */
			Cairo.cairo_set_source_rgba(cairo, 1, 1, 1, 1);
			Cairo.cairo_paint(cairo);   

			// set default stroke line width and color for Cairo context
			Cairo.cairo_set_line_width(cairo, 1.0);
			Cairo.cairo_set_source_rgba(cairo, 0, 0, 0, 1);  //black line

		} 
 
		CGRect UIEdgeInsetsInsetRect(CGRect rect, UIEdgeInsets ed)
		{
			//rect --> (x,y,w,h)  ed-->(top,left,bottom,right)
			CGPoint p = new CGPoint(rect.origin.x + ed.left, rect.origin.y + ed.top);
			CGSize s = new CGSize(rect.size.width - ed.left - ed.right, rect.size.height - ed.top - ed.bottom);
			CGRect r = new CGRect(p,s);
			return r;
		}


		void RCTContextAddLines(IntPtr cr, CGPoint[] points, int count)
		{	

			Log.Info(ReactConstants.Tag, "[RCTContextAddLines] points[0].x:" + points[0].x + ", points[0].y:" + points[0].y + ", points[1].x:" + points[1].x + ", points[1].y:" + points[1].y);
			Log.Info(ReactConstants.Tag, "[RCTContextAddLines] points[2].x:" + points[2].x + ", points[2].y:" + points[2].y + ", points[3].x:" + points[3].x + ", points[3].y:" + points[3].y);

			Cairo.cairo_move_to(cairo, points[0].x, points[0].y);
			for(int n = 1; n<count; n++)
			{
				Cairo.cairo_line_to(cairo, points[n].x, points[n].y);
			}
			//Cairo.cairo_stroke(cairo);

		}
 
		void RCTContextSetFillColorWithUint(IntPtr cr, uint color)
		{

			uint b = color & 0xFF;  
			uint g = (color >> 8) & 0xFF;  
			uint r = (color >> 16) & 0xFF;

			Cairo.cairo_set_source_rgba(cairo, r, g, b, 1);  //black alpha default
		}
 
		void RCTContextSetFillColorWithColor(IntPtr cr, Color color)
		{
			// set with Color parsed value
			Cairo.cairo_set_source_rgba(cairo, color.R, color.G, color.B, color.A);
		}
 
 
		void RCTContextSaveCurrentImage()
		{
			// for temp test
			Cairo.cairo_surface_flush(surface);	  

			/* Save the generated cairo surface image data to PNG file from Memories */	  
			string path = Tizen.Applications.Application.Current.DirectoryInfo.Data + "CairoBorder.png";
			Cairo.cairo_surface_write_to_png(surface, path);
			pathImage = path;
			//return path;
			return;
		}
  
		void RCTContextUpdateCurrentImage()
		{
			// for temp test
			Cairo.cairo_surface_flush(surface);

			/* display cairo drawin on screen */
			img = Cairo.evas_object_image_filled_add(ReactProgram.RctWindow);	
			IntPtr imageData = Cairo.cairo_image_surface_get_data(surface);
			Cairo.evas_object_image_data_set(img, imageData);

			Cairo.evas_object_image_data_update_add(img, 0, 0, 1920, 1080);
			return;
		}
 

		void RCTGetSolidBorderImage(RCTCornerRadii cornerRadii, 
											CGSize viewSize, 
											UIEdgeInsets borderInsets, 
											RCTBorderColors borderColors, 
											//uint backgroundColor,
											Color backgroundColor, 
											bool drawToEdge) 
		{ 
			bool hasCornerRadii = RCTCornerRadiiAreAboveThreshold(cornerRadii); 
			RCTCornerInsets cornerInsets = RCTGetCornerInsets(cornerRadii, borderInsets); 

			Log.Info(ReactConstants.Tag, "[RCTGetSolidBorderImage] ReactBorderDrawingManager::RCTGetSolidBorderImage BGN ");
			Log.Info(ReactConstants.Tag, "[RCTGetSolidBorderImage] ReactBorderDrawingManager , hasCornerRadii:" + hasCornerRadii );

			bool makeStretchable = 
			(borderInsets.left + cornerInsets.topLeft.width + 
			borderInsets.right + cornerInsets.bottomRight.width <= viewSize.width) && 
			(borderInsets.left + cornerInsets.bottomLeft.width + 
			borderInsets.right + cornerInsets.topRight.width <= viewSize.width) && 
			(borderInsets.top + cornerInsets.topLeft.height + 
			borderInsets.bottom + cornerInsets.bottomRight.height <= viewSize.height) && 
			(borderInsets.top + cornerInsets.topRight.height + 
			borderInsets.bottom + cornerInsets.bottomLeft.height <= viewSize.height); 


			UIEdgeInsets edgeInsets = new UIEdgeInsets
			( 
				borderInsets.top + Math.Max(cornerInsets.topLeft.height, cornerInsets.topRight.height), 
				borderInsets.left + Math.Max(cornerInsets.topLeft.width, cornerInsets.bottomLeft.width), 
				borderInsets.bottom + Math.Max(cornerInsets.bottomLeft.height, cornerInsets.bottomRight.height), 
				borderInsets.right + Math.Max(cornerInsets.bottomRight.width, cornerInsets.topRight.width) 
			); 
			makeStretchable = false;
			Log.Info(ReactConstants.Tag, "[RCTGetSolidBorderImage] ReactBorderDrawingManager , makeStretchable:" + makeStretchable );

			CGSize sizeStretch = new CGSize
			( 
				// 1pt for the middle stretchable area along each axis 
				edgeInsets.left + 1 + edgeInsets.right, 
				edgeInsets.top + 1 + edgeInsets.bottom 
			);
			//CGSize size = makeStretchable ?  sizeStretch: viewSize; 
			CGSize size = viewSize;

			// begin libCairo engine drawing context
			RCTUIGraphicsBeginImageContext(size, backgroundColor, hasCornerRadii, drawToEdge);


			CGRect rect = new CGRect((new CGPoint(0,0)), size); 
			RCTPathCreateOuterOutline(drawToEdge, rect, cornerRadii);
		  

			Log.Info(ReactConstants.Tag, "[RCTGetSolidBorderImage] ReactBorderDrawingManager , backgroundColor.IsDefault:" + backgroundColor.IsDefault );
			if (!backgroundColor.IsDefault) 
			{
				//  paint the background color here
				Log.Info(ReactConstants.Tag, "[RCTGetSolidBorderImage] ReactBorderDrawingManager::Fill the background Color! ");
				RCTContextSetFillColorWithColor(cairo, backgroundColor);	  
				//RCTContextSetFillColorWithUint(cairo, backgroundColor);

				Cairo.cairo_fill(cairo);
			} 


			// for Cairo instead, draw rounded Rectangle path here   
			Cairo.cairo_append_path(cairo, pathOuter);
			pathInner = RCTPathCreateWithRoundedRect(UIEdgeInsetsInsetRect(rect, borderInsets), cornerInsets);
			//Cairo.cairo_append_path(cairo, pathInner);

			/// Clip the outerpath with inner path  by eod rule..
			Cairo.cairo_set_fill_rule(cairo, Cairo.cairo_fill_rule_t.CAIRO_FILL_RULE_EVEN_ODD);
			//Cairo.cairo_set_fill_rule(cairo, Cairo.cairo_fill_rule_t.CAIRO_FILL_RULE_WINDING);   
			Cairo.cairo_clip(cairo);

			RCTContextSetFillColorWithUint(cairo, borderColors.left);
			Cairo.cairo_paint(cairo);


			bool hasEqualColors = RCTBorderColorsAreEqual(borderColors);
			Log.Info(ReactConstants.Tag, "[RCTGetSolidBorderImage] drawToEdge:" + drawToEdge + ", hasCornerRadii:" + hasCornerRadii + ", hasEqualColors:" + hasEqualColors);   
			if ((drawToEdge || !hasCornerRadii) && hasEqualColors) 
			{ 

				Log.Info(ReactConstants.Tag, "[RCTGetSolidBorderImage] rect.origin.x:" + rect.origin.x + ", rect.origin.y:" + rect.origin.y + ", rect.size.width:" + rect.size.width + ", rect.size.height:" + rect.size.height);	

				RCTContextSetFillColorWithUint(cairo, borderColors.left);	
				Cairo.cairo_paint(cairo);

			} 
			else 
			{

				if (( hasCornerRadii) ) 
				{
					Log.Info(ReactConstants.Tag, "[RCTGetSolidBorderImage] ReactBorderDrawingManager-->not support different color for rounded rectangle! ");	
					RCTContextSaveCurrentImage();
					return;

				}

				CGPoint topLeft = new CGPoint(borderInsets.left, borderInsets.top); 
				if (cornerInsets.topLeft.width > 0 && cornerInsets.topLeft.height > 0) 
				{ 
					CGPoint[] points = new CGPoint[2];
					CGRect tl = new CGRect
					( 
						topLeft, new CGSize(2 * cornerInsets.topLeft.width, 2 * cornerInsets.topLeft.height) 
					);
					RCTEllipseGetIntersectionsWithLine(tl, CGPointZero, topLeft, points); 
					if (!Single.IsNaN(points[1].x) && !Single.IsNaN(points[1].y)) 
					{ 
						topLeft = points[1]; 
					} 
				} 


				CGPoint bottomLeft = new CGPoint(borderInsets.left, size.height - borderInsets.bottom); 
				if (cornerInsets.bottomLeft.width > 0 && cornerInsets.bottomLeft.height > 0) 
				{ 
					CGPoint[] points = new CGPoint[2];
					CGRect bl = new CGRect
					( 
						new CGPoint(bottomLeft.x, bottomLeft.y - 2 * cornerInsets.bottomLeft.height), 
						new CGSize(2 * cornerInsets.bottomLeft.width, 2 * cornerInsets.bottomLeft.height) 
					);
					CGPoint ble = new CGPoint(0, size.height); 
					RCTEllipseGetIntersectionsWithLine(bl, ble, bottomLeft, points); 
					if (!Single.IsNaN(points[1].x) && !Single.IsNaN(points[1].y)) 
					{ 
						bottomLeft = points[1]; 
					} 
				} 


				CGPoint topRight = new CGPoint(size.width - borderInsets.right, borderInsets.top); 
				if (cornerInsets.topRight.width > 0 && cornerInsets.topRight.height > 0) 
				{ 
					CGPoint[] points = new CGPoint[2];
					CGRect tr = new CGRect
					( 
						new CGPoint(topRight.x - 2 * cornerInsets.topRight.width, topRight.y), 
						new CGSize(2 * cornerInsets.topRight.width, 2 * cornerInsets.topRight.height) 
					);
					CGPoint tre = new CGPoint(size.width, 0); 
					RCTEllipseGetIntersectionsWithLine(tr, tre, topRight, points); 
					if (!Single.IsNaN(points[0].x) && !Single.IsNaN(points[0].y)) 
					{ 
						topRight = points[0]; 
					} 
				} 


				CGPoint bottomRight = new CGPoint(size.width - borderInsets.right, size.height - borderInsets.bottom); 
				if (cornerInsets.bottomRight.width > 0 && cornerInsets.bottomRight.height > 0) 
				{ 
					CGPoint[] points = new CGPoint[2];
					CGRect br = new CGRect
					( 
						new CGPoint(bottomRight.x - 2 * cornerInsets.bottomRight.width, bottomRight.y - 2 * cornerInsets.bottomRight.height), 
						new CGSize(2 * cornerInsets.bottomRight.width, 2 * cornerInsets.bottomRight.height) 
					);
					CGPoint bre = new CGPoint(size.width, size.height);
					RCTEllipseGetIntersectionsWithLine(br, bre, bottomRight, points); 
					if (!Single.IsNaN(points[0].x) && !Single.IsNaN(points[0].y)) 
					{ 
						bottomRight = points[0]; 
					} 
				} 

				Log.Info(ReactConstants.Tag, "[RCTGetSolidBorderImage] ReactBorderDrawingManager::RCTGetSolidBorderImage BGN Draw each colored Edge!");
				Log.Info(ReactConstants.Tag, "[RCTGetSolidBorderImage] borderInsets.right:" + borderInsets.right + ", borderInsets.bottom:" + borderInsets.bottom + ", borderInsets.left:" + borderInsets.left + ", borderInsets.top:" + borderInsets.top);
			  
				uint currentColor = 0;
				//Cairo.cairo_set_fill_rule(cairo, Cairo.cairo_fill_rule_t.CAIRO_FILL_RULE_WINDING);

				// RIGHT 
				if (borderInsets.right > 0) 
				{ 

					CGPoint[] points = 
					{ 
						new CGPoint(size.width, 0), 
						topRight, 
						bottomRight, 
						new CGPoint(size.width, size.height), 
					}; 

					currentColor = borderColors.right;
					RCTContextAddLines(cairo, points, 4);
				} 


				// BOTTOM 
				if (borderInsets.bottom > 0) 
				{ 

					CGPoint[] points = 
					{ 
						new CGPoint(0, size.height), 
						bottomLeft, 
						bottomRight, 
						new CGPoint(size.width, size.height), 
					}; 


					if (!(currentColor == borderColors.bottom)) 
					{ 
						RCTContextSetFillColorWithUint(cairo, currentColor); 
						Cairo.cairo_fill(cairo); 
						currentColor = borderColors.bottom; 
					} 
					RCTContextAddLines(cairo, points, 4);
				} 


				// LEFT 
				if (borderInsets.left > 0) 
				{ 

					CGPoint[] points = 
					{ 
						CGPointZero, 
						topLeft, 
						bottomLeft, 
						new CGPoint(0, size.height), 
					}; 


					if (!(currentColor == borderColors.left)) 
					{ 
						RCTContextSetFillColorWithUint(cairo, currentColor); 
						Cairo.cairo_fill(cairo); 
						currentColor = borderColors.left; 
					}
					RCTContextAddLines(cairo, points, 4);
				} 


				// TOP 
				if (borderInsets.top > 0) 
				{ 

					CGPoint[] points = 
					{ 
						CGPointZero, 
						topLeft, 
						topRight, 
						new CGPoint(size.width, 0), 
					}; 


					if (!(currentColor == borderColors.top)) 
					{ 
						RCTContextSetFillColorWithUint(cairo, currentColor); 
						Cairo.cairo_fill(cairo); 
						currentColor = borderColors.top; 
					}
					RCTContextAddLines(cairo, points, 4);
				} 
			 

				RCTContextSetFillColorWithUint(cairo, currentColor);
				Cairo.cairo_fill(cairo);
			}

			RCTContextSaveCurrentImage();

			return; 

		} 

	 
		// Currently, the dashed / dotted implementation only supports a single colour + 
		// single width, as that's currently required and supported on Android. 
		// 
		// Supporting individual widths + colours on each side is possible by modifying 
		// the current implementation. The idea is that we will draw four different lines 
		// and clip appropriately for each side (might require adjustment of phase so that 
		// they line up but even browsers don't do a good job at that). 
		// 
		// Firstly, create two paths for the outer and inner paths. The inner path is 
		// generated exactly the same way as the outer, just given an inset rect, derived 
		// from the insets on each side. Then clip using the odd-even rule 
		// (CGContextEOClip()). This will give us a nice rounded (possibly) clip mask. 
		// 
		// +----------------------------------+ 
		// |@@@@@@@@  Clipped Space  @@@@@@@@@| 
		// |@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@| 
		// |@@+----------------------+@@@@@@@@| 
		// |@@|                      |@@@@@@@@| 
		// |@@|                      |@@@@@@@@| 
		// |@@|                      |@@@@@@@@| 
		// |@@+----------------------+@@@@@@@@| 
		// |@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@| 
		// +----------------------------------+ 
		// 
		// Afterwards, we create a clip path for each border side (CGContextSaveGState() 
		// and CGContextRestoreGState() when drawing each side). The clip mask for each 
		// segment is a trapezoid connecting corresponding edges of the inner and outer 
		// rects. For example, in the case of the top edge, the points would be: 
		// - (MinX(outer), MinY(outer)) 
		// - (MaxX(outer), MinY(outer)) 
		// - (MinX(inner) + topLeftRadius, MinY(inner) + topLeftRadius) 
		// - (MaxX(inner) - topRightRadius, MinY(inner) + topRightRadius) 
		// 
		//         +------------------+ 
		//         |\                /| 
		//         | \              / | 
		//         |  \    top     /  | 
		//         |   \          /   | 
		//         |    \        /    | 
		//         |     +------+     | 
		//         |     |      |     | 
		//         |     |      |     | 
		//         |     |      |     | 
		//         |left |      |right| 
		//         |     |      |     | 
		//         |     |      |     | 
		//         |     +------+     | 
		//         |    /        \    | 
		//         |   /          \   | 
		//         |  /            \  | 
		//         | /    bottom    \ | 
		//         |/                \| 
		//         +------------------+ 
		// 
		// 
		// Note that this approach will produce discontinous colour changes at the edge 
		// (which is okay). The reason is that Quartz does not currently support drawing 
		// of gradients _along_ a path (NB: clipping a path and drawing a linear gradient 
		// is _not_ equivalent). 


		/*static void RCTGetDashedOrDottedBorderImage(uint borderStyle, 
											 RCTCornerRadii cornerRadii, 
											 CGSize viewSize, 
											 UIEdgeInsets borderInsets, 
											 RCTBorderColors borderColors, 
											 uint backgroundColor, 
											 bool drawToEdge) 
		{ 
			// to be implement
		}*/ 
	 
 
		/// this is the API supported for RN parent image view invoke
		void RCTGetBorderImage(RCTBorderStyle borderStyle, 
							CGSize viewSize, 
							RCTCornerRadii cornerRadii, 
							UIEdgeInsets borderInsets, 
							RCTBorderColors borderColors, 
							//uint backgroundColor,
							Color backgroundColor, 
							bool drawToEdge) 
		{ 

			Log.Info(ReactConstants.Tag, "[RCTGetBorderImage] ReactBorderDrawingManager::RCTGetBorderImage BGN ");
			Log.Info(ReactConstants.Tag, "[RCTGetBorderImage] ReactBorderDrawingManager , borderStyle:" + borderStyle );

			switch (borderStyle) 
			{ 
				case RCTBorderStyle.RCTBorderStyleSolid: 
					//return RCTGetSolidBorderImage(cornerRadii, viewSize, borderInsets, borderColors, backgroundColor, drawToEdge);
					RCTGetSolidBorderImage(cornerRadii, viewSize, borderInsets, borderColors, backgroundColor, drawToEdge);
				break; 
				case RCTBorderStyle.RCTBorderStyleDashed: 
				case RCTBorderStyle.RCTBorderStyleDotted: 
					//return RCTGetDashedOrDottedBorderImage(borderStyle, cornerRadii, viewSize, borderInsets, borderColors, backgroundColor, drawToEdge);
				break;
				case RCTBorderStyle.RCTBorderStyleUnset: 
				break;
				default:
				break; 
			}

			//return null; 
		} 



		public string cairo_basic_drawingImageFile()
		{

			bool drawToEdge = false;
			drawToEdge = true;

			// for image view invoke here
			RCTCornerRadii cornerRadii;
			cornerRadii.topLeft = 0;
			cornerRadii.topRight = 0;
			cornerRadii.bottomLeft = 0;
			cornerRadii.bottomRight = 0;

			/*cornerRadii.topLeft = 30;
			cornerRadii.topRight = 40;
			cornerRadii.bottomLeft = 20;
			cornerRadii.bottomRight = 10;*/

			//CGSize viewSize = new CGSize(1920,1080);
			CGSize viewSize = new CGSize(1000,800);

			UIEdgeInsets borderInsets = new UIEdgeInsets(10,10,10,30);

			RCTBorderColors borderColors;
			borderColors.top = 0x00FF00;  //green
			borderColors.left = 0xFF0000; //red
			borderColors.bottom = 0xFFFF00;  //yellow
			borderColors.right = 0x0000FF;  //blue

			//borderColors.top = borderColors.right = borderColors.bottom = borderColors.left = 0xFF0000; //red

			//Color backgroundColor = Color.Pink;
			Color backgroundColor = Color.Black;

			Log.Info(ReactConstants.Tag, "[cairo_basic_drawingImageFile] ReactBorderDrawingManager::cairo_basic_drawingImageFile BGN ");
			Log.Info(ReactConstants.Tag, "[cairo_basic_drawingImageFile] ReactBorderDrawingManager , drawToEdge:" + drawToEdge );
			RCTGetSolidBorderImage(cornerRadii, viewSize, borderInsets, borderColors, backgroundColor, drawToEdge);

			//return RCTGetSolidBorderImage(cornerRadii, viewSize, borderInsets, borderColors, backgroundColor, drawToEdge);			  

			return pathImage;            

		}



        /// <summary>
        /// The name of this view manager. This will be the name used to 
        /// reference this view manager from JavaScript.
        /// </summary>
        public override string Name
        {
            get
            {
                return ReactClass;
            }
        }

        /// <summary>
        /// The view manager event constants.
        /// </summary>
        public override IReadOnlyDictionary<string, object> ExportedCustomDirectEventTypeConstants
        {
            get
            {
                return new Dictionary<string, object>
                {
                    {
                        "topSelectedChange",
                        new Dictionary<string, object>
                        {
                            { "registrationName", "onSelectedChange" }
                        }
                    },
                };
            }
        }

        /// <summary>
        /// Sets the font color for the node.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="color">The masked color value.</param>
        [ReactProp(ViewProps.Color, CustomType = "Color")]
        public void SetColor(Image view, uint? color)
        {
            if (color.HasValue)
            {
                var c = ColorHelpers.Parse(color.Value);
                view.Color = c;

                Tracing.RNTracer.Write(Common.ReactConstants.Tag, "SetColor = " + c.ToString() + ", from " + color.Value);
            }
        }

        

        /// <summary>
        /// Receive events/commands directly from JavaScript through the 
        /// <see cref="UIManagerModule"/>.
        /// </summary>
        /// <param name="view">
        /// The view instance that should receive the command.
        /// </param>
        /// <param name="commandId">Identifer for the command.</param>
        /// <param name="args">Optional arguments for the command.</param>
        public override void ReceiveCommand(Image view, int commandId, JArray args)
        {
            // TODO: parse command & change view
            Log.Info(ReactConstants.Tag, "[Views] ReactBorderDrawingManager::ReceiveCommand");
        }
        


        /// <summary>
        /// Creates a new view instance of type <see cref="borderImage"/>.
        /// </summary>
        /// <param name="reactContext">The react context.</param>
        /// <returns>The view instance.</returns>
        protected override Image CreateViewInstance(ThemedReactContext reactContext)
        {
            Log.Info(ReactConstants.Tag, "[Views] ReactBorderDrawingManager::CreateViewInstance BGN ");

            // create view component & set basic prop
            Image borderImage = new Image(ReactProgram.RctWindow)
            {
                Color = Color.Red,
            };            

            borderImage.Resize(1920, 1080);
            
            borderImage.Move(0, 0);
            borderImage.Show();
            

            return borderImage;
        }

        /// <summary>
        /// Subclasses can override this method to install custom event 
        /// emitters on the given view.
        /// </summary>
        /// <param name="reactContext">The react context.</param>
        /// <param name="view">The view instance.</param>
        protected override void AddEventEmitters(ThemedReactContext reactContext, Image view)
        {
            Log.Info(ReactConstants.Tag, "[Views] Register custom event , view:" + view );
            string imageDataCairo = cairo_basic_drawingImageFile();
            
            Log.Info(ReactConstants.Tag, "[Cairo] Begin draw border , imageDataCairo:" + imageDataCairo );
            view.Load(imageDataCairo);
        }

                
    }

    /* Event for ReactBorderImage */
    class ReactBorderDrawingManagerEvent : Event
    {
        private readonly int _index;

        public ReactBorderDrawingManagerEvent(int viewTag, int index)
            : base(viewTag, TimeSpan.FromTicks(Environment.TickCount))
        {
            _index = index;
        }

        public override string EventName
        {
            get
            {
                return "topSelectedChange";
            }
        }

        public override void Dispatch(RCTEventEmitter eventEmitter)
        {
            var eventData = new JObject
            {
                { "target", ViewTag },
                { "value", _index },
            };

            Log.Info(ReactConstants.Tag, "[Views] Dispatch Event >> name:Select, viewTag:" + ViewTag + ", cur postion:" + _index);
            eventEmitter.receiveEvent(ViewTag, EventName, eventData);
        }
    }


    // Libname string define    
    internal static class Libraries
    {
        internal const string Libc = "libc.so.6";
        internal const string Libdl = "libdl.so.2";
        internal const string Evas = "libevas.so.1";
        internal const string Elementary = "libelementary.so.1";
        internal const string Eina = "libeina.so.1";
        internal const string Ecore = "libecore.so.1";
        internal const string EcoreInput = "libecore_input.so.1";
        internal const string Eo = "libeo.so.1";
        internal const string Eext = "libefl-extension.so.0";
        // we use libCairo ref for border drawing
        internal const string Cairo = "libcairo.so.2";
    }
	

    internal static partial class Cairo
    {
        [DllImport(Libraries.Evas)]
        internal static extern void evas_object_image_file_set(IntPtr obj, string file, string key);

        [DllImport(Libraries.Evas)]
        internal static extern void evas_object_image_border_set(IntPtr obj, int l, int r, int t, int b);

        [DllImport(Libraries.Evas)]
        internal static extern void evas_object_image_alpha_set(IntPtr obj, bool has_alpha);

        [DllImport(Libraries.Evas)]
        internal static extern bool evas_object_image_alpha_get(IntPtr obj);

        [DllImport(Libraries.Evas)]
        internal static extern LoadError evas_object_image_load_error_get(IntPtr obj);

        [DllImport(Libraries.Evas)]
        internal static extern void evas_object_image_size_get(IntPtr obj, IntPtr x, out int y);

        [DllImport(Libraries.Evas)]
        internal static extern void evas_object_image_size_get(IntPtr obj, out int x, IntPtr y);

        [DllImport(Libraries.Evas)]
        internal static extern void evas_object_image_size_get(IntPtr obj, out int x, out int y);
		
		
		// jicheng add for border test
		[DllImport(Libraries.Evas)]
        internal static extern IntPtr evas_object_image_filled_add(IntPtr evas);
        //

        [DllImport(Libraries.Evas)]
        //internal static extern unsafe void evas_object_image_data_set(IntPtr obj, char* data);
        internal static extern void evas_object_image_data_set(IntPtr obj, IntPtr data);

        [DllImport(Libraries.Evas)]
        internal static extern void evas_object_image_size_set(IntPtr obj, int w, int h);

        [DllImport(Libraries.Evas)]
        internal static extern void evas_object_image_data_update_add(IntPtr obj, int x, int y, int w, int h);


        [DllImport(Libraries.Evas)]
        internal static extern void evas_object_show(IntPtr obj);

        [DllImport(Libraries.Evas)]
        internal static extern void evas_object_move(IntPtr obj, int x, int y);


        // add API for Cairo
        [DllImport(Libraries.Cairo)]
        internal static extern IntPtr cairo_image_surface_create(int obj, int w, int h);
        //

        [DllImport(Libraries.Cairo)]
        //internal static extern unsafe void cairo_set_source_rgba(IntPtr obj, char* data);
        internal static extern void cairo_set_source_rgba(IntPtr obj, double red, double green, double blue, double alpha);
        //

        [DllImport(Libraries.Cairo)]
        internal static extern IntPtr cairo_create(IntPtr obj);
        //

        [DllImport(Libraries.Cairo)]
        internal static extern IntPtr cairo_destroy(IntPtr obj);
        //

        [DllImport(Libraries.Cairo)]
        internal static extern IntPtr cairo_surface_destroy(IntPtr obj);
        //

        [DllImport(Libraries.Cairo)]
        internal static extern IntPtr cairo_image_surface_get_data(IntPtr obj);
        //

        [DllImport(Libraries.Cairo)]
        internal static extern IntPtr cairo_get_target(IntPtr obj);
        //

        [DllImport(Libraries.Cairo)]
        internal static extern void cairo_surface_flush(IntPtr obj);
        //

        [DllImport(Libraries.Cairo)]
        internal static extern void cairo_paint(IntPtr obj);
        //

        [DllImport(Libraries.Cairo)]
        internal static extern void cairo_stroke(IntPtr obj);
        //

        [DllImport(Libraries.Cairo)]
        internal static extern void cairo_set_line_width(IntPtr obj, double width);
        //
		
		[DllImport(Libraries.Cairo)]
        internal static extern void cairo_move_to(IntPtr obj, double x, double y);
		
		[DllImport(Libraries.Cairo)]
        internal static extern void cairo_line_to(IntPtr obj, double x, double y);
		//

        [DllImport(Libraries.Cairo)]
        internal static extern void cairo_rectangle(IntPtr obj, double x, double y, double width, double height);

        [DllImport(Libraries.Cairo)]
        internal static extern void cairo_arc(IntPtr obj, double x, double y, double r, double ang1, double ang2);

        [DllImport(Libraries.Cairo)]
        internal static extern void cairo_fill(IntPtr obj);
		
		[DllImport(Libraries.Cairo)]
        internal static extern void cairo_clip(IntPtr obj);
        //

        [DllImport(Libraries.Cairo)]
        internal static extern void cairo_surface_write_to_png(IntPtr obj, string file);
        //
		
		[DllImport(Libraries.Cairo)]
        internal static extern IntPtr cairo_copy_path(IntPtr obj);
		
		[DllImport(Libraries.Cairo)]
        internal static extern void cairo_append_path(IntPtr obj, IntPtr path);
		
		[DllImport(Libraries.Cairo)]
        internal static extern void cairo_close_path(IntPtr obj);
		
		[DllImport(Libraries.Cairo)]
        internal static extern void cairo_new_sub_path(IntPtr obj);
		
		internal enum cairo_fill_rule_t
        {
            CAIRO_FILL_RULE_WINDING = 0,
            CAIRO_FILL_RULE_EVEN_ODD = 1,
        }
		
		[DllImport(Libraries.Cairo)]
        internal static extern void cairo_set_fill_rule(IntPtr obj, cairo_fill_rule_t fill_rule);
		

        internal enum LoadError
        {
            None = 0,
            Generic = 1,
            DoesNotExist = 2,
            PermissionDenied = 3,
            ResourceAllocationFailed = 4,
            CorruptFile = 5,
            UnknownFormat = 6,
        }
        
    }

}
