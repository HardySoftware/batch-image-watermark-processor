using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public abstract class Watermark : IProcess {
		public bool EnableDebug {
			get;
			set;
		}

		public abstract Image ProcessImage(Image input, ProjectSetting ps);

		/// <summary>
		/// Creates a new Image containing the same image only rotated
		/// </summary>
		/// <param name="image">The <see cref="System.Drawing.Image"/> to rotate</param>
		/// <param name="angle">The amount to rotate the image, clockwise, in degrees.</param>
		/// <returns>A new <see cref="System.Drawing.Bitmap"/> that is just large enough
		/// to contain the rotated image without cutting any corners off.</returns>
		/// <exception cref="System.ArgumentNullException">Thrown if <see cref="image"/> is null.</exception>
		/// <remarks>
		/// Original post from http://netcode.ru/dotnet/?lang=&katID=30&skatID=263&artID=7226
		/// </remarks>
		protected Bitmap RotateImage(Image image, float angle) {
			if (image == null) {
				throw new ArgumentNullException("image");
			}

			const double pi2 = Math.PI / 2.0;

			// Why can't C# allow these to be const, or at least readonly
			// *sigh*  I'm starting to talk like Christian Graus :omg:
			double oldWidth = (double)image.Width;
			double oldHeight = (double)image.Height;

			// Convert degrees to radians
			double theta = ((double)angle) * Math.PI / 180.0;
			double locked_theta = theta;

			// Ensure theta is now [0, 2pi)
			while (locked_theta < 0.0) {
				locked_theta += 2 * Math.PI;
			}

			double newWidth, newHeight;
			int nWidth, nHeight; // The newWidth/newHeight expressed as ints

			#region Explaination of the calculations
			/*
			 * The trig involved in calculating the new width and height
			 * is fairly simple; the hard part was remembering that when 
			 * PI/2 <= theta <= PI and 3PI/2 <= theta < 2PI the width and 
			 * height are switched.
			 * 
			 * When you rotate a rectangle, r, the bounding box surrounding r
			 * contains for right-triangles of empty space.  Each of the 
			 * triangles hypotenuse's are a known length, either the width or
			 * the height of r.  Because we know the length of the hypotenuse
			 * and we have a known angle of rotation, we can use the trig
			 * function identities to find the length of the other two sides.
			 * 
			 * sine = opposite/hypotenuse
			 * cosine = adjacent/hypotenuse
			 * 
			 * solving for the unknown we get
			 * 
			 * opposite = sine * hypotenuse
			 * adjacent = cosine * hypotenuse
			 * 
			 * Another interesting point about these triangles is that there
			 * are only two different triangles. The proof for which is easy
			 * to see, but its been too long since I've written a proof that
			 * I can't explain it well enough to want to publish it.  
			 * 
			 * Just trust me when I say the triangles formed by the lengths 
			 * width are always the same (for a given theta) and the same 
			 * goes for the height of r.
			 * 
			 * Rather than associate the opposite/adjacent sides with the
			 * width and height of the original bitmap, I'll associate them
			 * based on their position.
			 * 
			 * adjacent/oppositeTop will refer to the triangles making up the 
			 * upper right and lower left corners
			 * 
			 * adjacent/oppositeBottom will refer to the triangles making up 
			 * the upper left and lower right corners
			 * 
			 * The names are based on the right side corners, because thats 
			 * where I did my work on paper (the right side).
			 * 
			 * Now if you draw this out, you will see that the width of the 
			 * bounding box is calculated by adding together adjacentTop and 
			 * oppositeBottom while the height is calculate by adding 
			 * together adjacentBottom and oppositeTop.
			 */
			#endregion

			double adjacentTop, oppositeTop;
			double adjacentBottom, oppositeBottom;

			// We need to calculate the sides of the triangles based
			// on how much rotation is being done to the bitmap.
			//   Refer to the first paragraph in the explaination above for 
			//   reasons why.
			if ((locked_theta >= 0.0 && locked_theta < pi2) ||
				(locked_theta >= Math.PI && locked_theta < (Math.PI + pi2))) {
				adjacentTop = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
				oppositeTop = Math.Abs(Math.Sin(locked_theta)) * oldWidth;

				adjacentBottom = Math.Abs(Math.Cos(locked_theta)) * oldHeight;
				oppositeBottom = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
			} else {
				adjacentTop = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
				oppositeTop = Math.Abs(Math.Cos(locked_theta)) * oldHeight;

				adjacentBottom = Math.Abs(Math.Sin(locked_theta)) * oldWidth;
				oppositeBottom = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
			}

			newWidth = adjacentTop + oppositeBottom;
			newHeight = adjacentBottom + oppositeTop;

			nWidth = (int)Math.Ceiling(newWidth);
			nHeight = (int)Math.Ceiling(newHeight);

			Bitmap rotatedBmp = new Bitmap(nWidth, nHeight);

			using (Graphics g = Graphics.FromImage(rotatedBmp)) {
				// This array will be used to pass in the three points that 
				// make up the rotated image
				Point[] points;

				/*
				 * The values of opposite/adjacentTop/Bottom are referring to 
				 * fixed locations instead of in relation to the
				 * rotating image so I need to change which values are used
				 * based on the how much the image is rotating.
				 * 
				 * For each point, one of the coordinates will always be 0, 
				 * nWidth, or nHeight.  This because the Bitmap we are drawing on
				 * is the bounding box for the rotated bitmap.  If both of the 
				 * corrdinates for any of the given points wasn't in the set above
				 * then the bitmap we are drawing on WOULDN'T be the bounding box
				 * as required.
				 */
				if (locked_theta >= 0.0 && locked_theta < pi2) {
					points = new Point[] { 
											 new Point( (int) oppositeBottom, 0 ), 
											 new Point( nWidth, (int) oppositeTop ),
											 new Point( 0, (int) adjacentBottom )
										 };

				} else if (locked_theta >= pi2 && locked_theta < Math.PI) {
					points = new Point[] { 
											 new Point( nWidth, (int) oppositeTop ),
											 new Point( (int) adjacentTop, nHeight ),
											 new Point( (int) oppositeBottom, 0 )						 
										 };
				} else if (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2)) {
					points = new Point[] { 
											 new Point( (int) adjacentTop, nHeight ), 
											 new Point( 0, (int) adjacentBottom ),
											 new Point( nWidth, (int) oppositeTop )
										 };
				} else {
					points = new Point[] { 
											 new Point( 0, (int) adjacentBottom ), 
											 new Point( (int) oppositeBottom, 0 ),
											 new Point( (int) adjacentTop, nHeight )		
										 };
				}

				g.DrawImage(image, points);
			}

			return rotatedBmp;
		}

		/// <summary>
		/// Add a watermark image to a main image at designated position.
		/// </summary>
		/// <param name="watermarkImage">Image to be added.</param>
		/// <param name="mainImage">Image to add to.</param>
		/// <param name="position">Position</param>
		/// <returns>Main image with watermark image on it.</returns>
		protected Bitmap AddImageToImage(Bitmap watermarkImage, Bitmap mainImage, ContentAlignment position) {
			int watermarkWidth = watermarkImage.Width;
			int watermarkHeight = watermarkImage.Height;

			int imageWidth = mainImage.Width;
			int imageHeight = mainImage.Height;

			// Load main Bitmap into a new Graphic Object
			using (Graphics graphic = Graphics.FromImage(mainImage)) {
				int xPosOfWatermark = 0;
				int yPosOfWatermark = 0;

				switch (position) {
					case ContentAlignment.BottomCenter:
						xPosOfWatermark = (imageWidth / 2) - (watermarkWidth / 2);
						yPosOfWatermark = (imageHeight - watermarkHeight) - 10;
						break;
					case ContentAlignment.BottomLeft:
						xPosOfWatermark = 10;
						yPosOfWatermark = (imageHeight - watermarkHeight) - 10;
						break;
					case ContentAlignment.BottomRight:
						xPosOfWatermark = ((imageWidth - watermarkWidth) - 10);
						yPosOfWatermark = (imageHeight - watermarkHeight) - 10;
						break;
					case ContentAlignment.MiddleCenter:
						xPosOfWatermark = (imageWidth / 2) - (watermarkWidth / 2);
						yPosOfWatermark = (imageHeight / 2) - (watermarkHeight / 2);
						break;
					case ContentAlignment.MiddleLeft:
						xPosOfWatermark = 10;
						yPosOfWatermark = (imageHeight / 2) - (watermarkHeight / 2);
						break;
					case ContentAlignment.MiddleRight:
						xPosOfWatermark = ((imageWidth - watermarkWidth) - 10);
						yPosOfWatermark = (imageHeight / 2) - (watermarkHeight / 2);
						break;
					case ContentAlignment.TopCenter:
						xPosOfWatermark = (imageWidth / 2) - (watermarkWidth / 2);
						yPosOfWatermark = 10;
						break;
					case ContentAlignment.TopLeft:
						xPosOfWatermark = 10;
						yPosOfWatermark = 10;
						break;
					case ContentAlignment.TopRight:
						//For this example we will place the watermark in the upper right
						//hand corner of the photograph. offset down 10 pixels and to the 
						//left 10 pixles
						xPosOfWatermark = ((imageWidth - watermarkWidth) - 10);
						yPosOfWatermark = 10;
						break;
				}

				graphic.DrawImage(watermarkImage,
					new Rectangle(xPosOfWatermark, yPosOfWatermark, watermarkWidth, watermarkHeight),  //Set the detination Position
					0,                  // x-coordinate of the portion of the source image to draw. 
					0,                  // y-coordinate of the portion of the source image to draw. 
					watermarkWidth,            // Watermark Width
					watermarkHeight,		    // Watermark Height
					GraphicsUnit.Pixel); // Unit of measurment*/
			}
			return mainImage;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="angle"></param>
		/// <returns></returns>
		/// <remarks>
		/// Code and logic from http://www.codeproject.com/KB/custom-controls/RotatedTextImageControl.aspx
		/// </remarks>
		protected RotationSize CalculateRotationSize(int width, int height, int angle) {
			int verticalOffset, horizontalOffset;
			int rotatedHeight, rotatedWidth;

			// convert the rotation angle to radians
			double radians = ((double)angle / 180) * Math.PI;

			// work out what the transalation offsets will need to be once the text is rotated
			// to make sure the containning rectangle fits on completely on the image
			if (angle <= 90) {
				verticalOffset = 0;
				horizontalOffset = (int)Math.Abs(Math.Ceiling((double)height * Math.Sin(radians)));
			} else if (angle <= 180) {
				horizontalOffset = (int)Math.Ceiling(Math.Abs(width * Math.Sin(radians - (Math.PI / 2))) + Math.Abs(height * Math.Cos(radians - (Math.PI / 2))));
				verticalOffset = (int)Math.Ceiling(Math.Abs(height * Math.Sin(radians - (Math.PI / 2))));
			} else if (angle <= 270) {
				verticalOffset = (int)Math.Ceiling(Math.Abs(width * Math.Sin(radians - (Math.PI))) + Math.Abs(height * Math.Cos(radians - (Math.PI))));
				horizontalOffset = (int)Math.Ceiling(Math.Abs(width * Math.Cos(radians - (Math.PI))));
			} else {
				verticalOffset = (int)Math.Ceiling(Math.Abs(width * Math.Cos(radians - (Math.PI * 1.5))));
				horizontalOffset = 0;
			}

			// work out the size of the containing rectangle
			rotatedHeight = (int)Math.Ceiling(Math.Abs(height * Math.Cos(radians)) + (Math.Abs(width * Math.Sin(radians))));
			rotatedWidth = (int)Math.Ceiling(Math.Abs(width * Math.Cos(radians)) + (Math.Abs(height * Math.Sin(radians))));

			RotationSize rs = new RotationSize() {
				VerticalOffset = verticalOffset,
				HorizontalOffset = horizontalOffset,
				RotatedWidth = rotatedWidth,
				RotatedHeight = rotatedHeight
			};

			return rs;
		}

		protected struct RotationSize {
			public int VerticalOffset;
			public int HorizontalOffset;
			public int RotatedHeight;
			public int RotatedWidth;
		}

		/// <summary>
		/// The space of text box or image box between edge of main image.
		/// </summary>
		protected int Padding {
			get {
				return 10;
			}
		}
	}
}