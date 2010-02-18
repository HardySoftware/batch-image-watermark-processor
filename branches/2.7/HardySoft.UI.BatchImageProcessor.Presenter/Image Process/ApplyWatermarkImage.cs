using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class ApplyWatermarkImage : Watermark {
		private int watermarkIndex;

		public ApplyWatermarkImage(int watermarkIndex) {
			this.watermarkIndex = watermarkIndex;
		}

		public override Image ProcessImage(Image input, ProjectSetting ps) {
			if (ps.WatermarkCollection.Count < watermarkIndex) {
				return input;
			}

			WatermarkImage wi = ps.WatermarkCollection[this.watermarkIndex] as WatermarkImage;

			if (wi == null) {
				return input;
			}

#if DEBUG
			System.Diagnostics.Debug.WriteLine("Current Thread: "
					+ System.Threading.Thread.CurrentThread.ManagedThreadId + ","
					+ " Image File Name: " + this.ImageFileName + ","
					+ " Watermark Image index: " + watermarkIndex);
#endif

			try {
				// image used as watermark
				Image watermarkImage;
				using (Stream stream = File.OpenRead(wi.WatermarkImageFile)) {
					watermarkImage = Image.FromStream(stream);
				}

				Bitmap rotatedWatermarkImage;
				if (wi.WatermarkRotateAngle > 0 && wi.WatermarkRotateAngle < 360) {
					rotatedWatermarkImage = RotateImage(watermarkImage, wi.WatermarkRotateAngle);
				} else {
					rotatedWatermarkImage = (Bitmap)watermarkImage;
				}

				rotatedWatermarkImage = setImageOpacity(rotatedWatermarkImage, (float)wi.WatermarkImageOpacity);

				//int watermarkWidth = watermarkImage.Width;
				//int watermarkHeight = watermarkImage.Height;
				int watermarkWidth = rotatedWatermarkImage.Width;
				int watermarkHeight = rotatedWatermarkImage.Height;

				int imageWidth = input.Width;
				int imageHeight = input.Height;

				// Create a Bitmap based on the previously modified photograph Bitmap
				Bitmap bmp = new Bitmap(input);
				bmp.SetResolution(input.HorizontalResolution, input.VerticalResolution);

				// Load this Bitmap into a new Graphic Object
				Graphics graphic = Graphics.FromImage(bmp);

				int xPosOfWatermark = 0;
				int yPosOfWatermark = 0;

				switch (wi.WatermarkPosition) {
					case ContentAlignment.BottomCenter:
						xPosOfWatermark = (imageWidth / 2) - (watermarkWidth / 2);
						yPosOfWatermark = (imageHeight - watermarkHeight) - wi.Padding;
						break;
					case ContentAlignment.BottomLeft:
						xPosOfWatermark = 10;
						yPosOfWatermark = (imageHeight - watermarkHeight) - wi.Padding;
						break;
					case ContentAlignment.BottomRight:
						xPosOfWatermark = (imageWidth - watermarkWidth) - wi.Padding;
						yPosOfWatermark = (imageHeight - watermarkHeight) - wi.Padding;
						break;
					case ContentAlignment.MiddleCenter:
						xPosOfWatermark = (imageWidth / 2) - (watermarkWidth / 2);
						yPosOfWatermark = (imageHeight / 2) - (watermarkHeight / 2);
						break;
					case ContentAlignment.MiddleLeft:
						xPosOfWatermark = wi.Padding;
						yPosOfWatermark = (imageHeight / 2) - (watermarkHeight / 2);
						break;
					case ContentAlignment.MiddleRight:
						xPosOfWatermark = (imageWidth - watermarkWidth) - wi.Padding;
						yPosOfWatermark = (imageHeight / 2) - (watermarkHeight / 2);
						break;
					case ContentAlignment.TopCenter:
						xPosOfWatermark = (imageWidth / 2) - (watermarkWidth / 2);
						yPosOfWatermark = wi.Padding;
						break;
					case ContentAlignment.TopLeft:
						xPosOfWatermark = wi.Padding;
						yPosOfWatermark = wi.Padding;
						break;
					case ContentAlignment.TopRight:
						//For this example we will place the watermark in the upper right
						//hand corner of the photograph. offset down 10 pixels and to the 
						//left 10 pixles
						xPosOfWatermark = (imageWidth - watermarkWidth) - wi.Padding;
						yPosOfWatermark = wi.Padding;
						break;
				}

				graphic.DrawImage(rotatedWatermarkImage,
					new Rectangle(xPosOfWatermark, yPosOfWatermark, watermarkWidth, watermarkHeight),  //Set the detination Position
					0,                  // x-coordinate of the portion of the source image to draw. 
					0,                  // y-coordinate of the portion of the source image to draw. 
					watermarkWidth,            // Watermark Width
					watermarkHeight,		    // Watermark Height
					GraphicsUnit.Pixel); // Unit of measurment

				graphic.Dispose();
				return (Image)bmp;
			} catch (Exception ex) {
				Trace.TraceError(ex.ToString());
				return input;
			}
		}

		/*private ImageAttributes getTranslucentImageAttribute() {
			// To achieve a translucent watermark we will apply (2) color 
			// manipulations by defineing a ImageAttributes object and 
			// seting (2) of its properties.
			ImageAttributes imageAttributes = new ImageAttributes();

			// The first step in manipulating the watermark image is to replace 
			// the backgroundColor color with one that is trasparent (Alpha=0, R=0, G=0, B=0)
			// to do this we will use a Colormap and use this to define a RemapTable
			ColorMap colorMap = new ColorMap();

			// My watermark was defined with a backgroundColor of 100% Green this will
			// be the color we search for and replace with transparency
			colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
			colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

			ColorMap[] remapTable = { colorMap };

			imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

			//The second color manipulation is used to change the opacity of the 
			//watermark.  This is done by applying a 5x5 matrix that contains the 
			//coordinates for the RGBA space.  By setting the 3rd row and 3rd column 
			//to 0.3f we achive a level of opacity
			float[][] colorMatrixElements = { 
												new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},       
												new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},        
												new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},        
												new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},        
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
											};
			ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

			imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default,
				ColorAdjustType.Bitmap);

			return imageAttributes;
		}*/

		/// <summary>
		/// method for changing the opacity of an image
		/// </summary>
		/// <param name="image">image to set opacity on</param>
		/// <param name="opacity">percentage of opacity</param>
		/// <returns></returns>
		private Bitmap setImageOpacity(Bitmap image, float opacity) {
			//create a Bitmap the size of the image provided
			Bitmap bmp = new Bitmap(image.Width, image.Height);

			//create a graphics object from the image
			Graphics gfx = Graphics.FromImage(bmp);

			//create a color matrix object
			ColorMatrix matrix = new ColorMatrix();

			//set the opacity
			matrix.Matrix33 = opacity;

			//create image attributes
			ImageAttributes attributes = new ImageAttributes();

			//set the color(opacity) of the image
			attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

			//now draw the image
			gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height),
				0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
			gfx.Dispose();
			return bmp;
		}
	}
}