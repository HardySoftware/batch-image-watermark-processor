using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class ApplyWatermarkImage : IProcess {
		public Image ProcessImage(Image input, ProjectSetting ps) {
			// image used as watermark
			Image copyrightImage;
			using (Stream stream = File.OpenRead(ps.Watermark.WatermarkImageFile)) {
				copyrightImage = Image.FromStream(stream);
			}

			// copyrightImage = Bitmap.FromFile(ps.Watermark.WatermarkImageFile);

			int watermarkWidth = copyrightImage.Width;
			int watermarkHeight = copyrightImage.Height;

			int imageWidth = input.Width;
			int imageHeight = input.Height;

			// Create a Bitmap based on the previously modified photograph Bitmap
			Bitmap bmp = new Bitmap(input);
			bmp.SetResolution(input.HorizontalResolution, input.VerticalResolution);

			// Load this Bitmap into a new Graphic Object
			Graphics graphic = Graphics.FromImage(bmp);

			int xPosOfWatermark = 0;
			int yPosOfWatermark = 0;

			switch (ps.Watermark.WatermarkImagePosition) {
				case WatermarkPositions.BottomCenter:
					xPosOfWatermark = (imageWidth / 2) - (watermarkWidth / 2);
					yPosOfWatermark = (imageHeight - watermarkHeight) - 10;
					break;
				case WatermarkPositions.BottomLeft:
					xPosOfWatermark = 10;
					yPosOfWatermark = (imageHeight - watermarkHeight) - 10;
					break;
				case WatermarkPositions.BottomRight:
					xPosOfWatermark = ((imageWidth - watermarkWidth) - 10);
					yPosOfWatermark = (imageHeight - watermarkHeight) - 10;
					break;
				case WatermarkPositions.MiddleCenter:
					xPosOfWatermark = (imageWidth / 2) - (watermarkWidth / 2);
					yPosOfWatermark = (imageHeight / 2) - (watermarkHeight / 2);
					break;
				case WatermarkPositions.MiddleLeft:
					xPosOfWatermark = 10;
					yPosOfWatermark = (imageHeight / 2) - (watermarkHeight / 2);
					break;
				case WatermarkPositions.MiddleRight:
					xPosOfWatermark = ((imageWidth - watermarkWidth) - 10);
					yPosOfWatermark = (imageHeight / 2) - (watermarkHeight / 2);
					break;
				case WatermarkPositions.TopCenter:
					xPosOfWatermark = (imageWidth / 2) - (watermarkWidth / 2);
					yPosOfWatermark = 10;
					break;
				case WatermarkPositions.TopLeft:
					xPosOfWatermark = 10;
					yPosOfWatermark = 10;
					break;
				case WatermarkPositions.TopRight:
					//For this example we will place the watermark in the upper right
					//hand corner of the photograph. offset down 10 pixels and to the 
					//left 10 pixles
					xPosOfWatermark = ((imageWidth - watermarkWidth) - 10);
					yPosOfWatermark = 10;
					break;
			}
			/* TODO for some reason semi-transparent watermark image stops working in this implementation.
			 * if I use image attributes.
			graphic.DrawImage(copyrightImage,
				new Rectangle(xPosOfWatermark, yPosOfWatermark, watermarkWidth, watermarkHeight),  //Set the detination Position
				0,                  // x-coordinate of the portion of the source image to draw. 
				0,                  // y-coordinate of the portion of the source image to draw. 
				watermarkWidth,            // Watermark Width
				watermarkHeight,		    // Watermark Height
				GraphicsUnit.Pixel, // Unit of measurment
				getTranslucentImageAttribute());   //ImageAttributes Object*/
			graphic.DrawImage(copyrightImage,
				new Rectangle(xPosOfWatermark, yPosOfWatermark, watermarkWidth, watermarkHeight),  //Set the detination Position
				0,                  // x-coordinate of the portion of the source image to draw. 
				0,                  // y-coordinate of the portion of the source image to draw. 
				watermarkWidth,            // Watermark Width
				watermarkHeight,		    // Watermark Height
				GraphicsUnit.Pixel); // Unit of measurment*/

			graphic.Dispose();
			return (Image)bmp;
		}

		private ImageAttributes getTranslucentImageAttribute() {
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
		}
	}
}