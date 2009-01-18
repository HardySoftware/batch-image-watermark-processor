using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace HardySoft.UI.BatchImageProcessor {
    class WaterMark {
        string workingDirectory;
        private WaterMarkProperties properties;

        public WaterMarkProperties Properties {
            get {
                return properties;
            }
        }

        public WaterMark (string workingDirectory, WaterMarkProperties properties) {
            this.workingDirectory = workingDirectory;
            this.properties = properties;
        }

        public void MarkImage (string sourcePicture, string destinationPicture) {
            Image processedPhoto = Image.FromFile(sourcePicture);
            ImageFormat format = getImageFormat(sourcePicture);

            #region apply all ImageProcessType tasks first
            if (properties.ShrinkImage && properties.ShrinkLongSidePixelTo > 0) {
                processedPhoto = ShrinkImage(processedPhoto, properties.ShrinkLongSidePixelTo);
            }
            switch (properties.ImageProcessType) {
                case ImageProcessTypes.GrayScale:
                    processedPhoto = this.ConvertToGreyScale(processedPhoto);
                    break;
                case ImageProcessTypes.NegativeImage:
                    processedPhoto = this.RevertImage(processedPhoto);
                    break;
            }
            #endregion

            #region Thumb Nail Images
            if (properties.GenerateThumbNail && properties.ThumbNailSize > 0) {
                Image thumb = ShrinkImage(processedPhoto, properties.ThumbNailSize);
                string prefix = properties.ThumbNailFilePrefix;
                string suffix = properties.ThumbNailFileSuffix;

                if (prefix == string.Empty && suffix == string.Empty) {
                    suffix = "_thumb";
                }

                
                string fileName, fileNameWithoutExtension, extension, thumbFileName;
                FileInfo fi = new FileInfo(destinationPicture);
                fileName = fi.Name;
                int pos = fileName.LastIndexOf(".");
                if (pos > -1) {
                    fileNameWithoutExtension = fileName.Substring(0, pos);
                    extension = fileName.Substring(pos + 1, fileName.Length - pos - 1);
                } else {
                    fileNameWithoutExtension = fileName;
                    extension = "";
                }

                thumbFileName = prefix + fileNameWithoutExtension + suffix + "." + extension;
                thumbFileName = Utilities.FormalizeFolderName(fi.DirectoryName) + thumbFileName;

                thumb.Save(thumbFileName, format);
                thumb.Dispose();
            }
            #endregion

            int imageWidth = processedPhoto.Width;
            int imageHeight = processedPhoto.Height;

            //create a Bitmap the Size of the original photograph
            Bitmap shadowBmp = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);

            shadowBmp.SetResolution(processedPhoto.HorizontalResolution, processedPhoto.VerticalResolution);

            //load the Bitmap into a Graphics object 
            Graphics shadowGraphic = Graphics.FromImage(shadowBmp);

            #region Insert copyright message
            //Set the rendering quality for this Graphics object
            shadowGraphic.SmoothingMode = SmoothingMode.AntiAlias;

            //Draws the photo Image object at original size to the graphics object.
            shadowGraphic.DrawImage(
                processedPhoto,                               // Photo Image object
                new Rectangle(0, 0, imageWidth, imageHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                imageWidth,                                // Width of the portion of the source image to draw. 
                imageHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 

            if (properties.CopyrightText.Trim().Length > 0) {
                /*
                //-------------------------------------------------------
                //to maximize the size of the copyright message we will 
                //test multiple Font sizes to determine the largest posible 
                //font we can use for the width of the Photograph
                //define an array of point sizes you would like to consider as possiblities
                //-------------------------------------------------------
                int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

                Font font = null;
                SizeF fontSize = new SizeF();

                //Loop through the defined sizes checking the length of the copyright string
                //If its length in pixles is less then the image width choose this Font size.
                for (int i = 0; i < sizes.Length; i++) {
                    //set a Font object to our font object, size we will decide...
                    font = new Font(copyrightFont.Name, sizes[i], copyrightFont.Style);

                    //Measure the copyright string in this Font
                    fontSize = shadowGraphic.MeasureString(copyright, font);

                    if ((ushort)fontSize.Width < (ushort)imageWidth) {
                        break;
                    }
                }*/
                Font font = properties.CopyrightTextFont;
                SizeF fontSize = new SizeF();
                fontSize = shadowGraphic.MeasureString(properties.CopyrightText, properties.CopyrightTextFont);

                int yPixlesFromBottom = 0;
                float yPosFromBottom = 0;
                float xCenterOfImg = 0;

                switch (properties.CopyrightTextPosition) {
                    case ContentAlignment.BottomCenter:
                        //Since all photographs will have varying heights, determine a 
                        //position 5% from the bottom of the image
                        yPixlesFromBottom = (int)(imageHeight * .05);

                        //Now that we have a point size use the m_Copyrights string height 
                        //to determine a y-coordinate to draw the string of the photograph
                        yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));

                        //Determine its x-coordinate by calculating the center of the width of the image
                        xCenterOfImg = (imageWidth / 2);
                        break;

                    case ContentAlignment.BottomLeft:
                        yPixlesFromBottom = (int)(imageHeight * .05);
                        yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
                        xCenterOfImg = (fontSize.Width / 2) + 10;
                        break;

                    case ContentAlignment.BottomRight:
                        yPixlesFromBottom = (int)(imageHeight * .05);
                        yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
                        xCenterOfImg = (imageWidth - (fontSize.Width / 2)) - 10;
                        break;

                    case ContentAlignment.MiddleCenter:
                        yPixlesFromBottom = (int)(imageHeight * .50);
                        yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
                        xCenterOfImg = (imageWidth / 2);
                        break;

                    case ContentAlignment.MiddleLeft:
                        yPixlesFromBottom = (int)(imageHeight * .50);
                        yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
                        xCenterOfImg = (fontSize.Width / 2) + 10;
                        break;

                    case ContentAlignment.MiddleRight:
                        yPixlesFromBottom = (int)(imageHeight * .50);
                        yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
                        xCenterOfImg = (imageWidth - (fontSize.Width / 2)) - 10;
                        break;

                    case ContentAlignment.TopCenter:
                        yPixlesFromBottom = (int)(imageHeight * .95);
                        yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
                        xCenterOfImg = (imageWidth / 2);
                        break;

                    case ContentAlignment.TopLeft:
                        yPixlesFromBottom = (int)(imageHeight * .95);
                        yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
                        xCenterOfImg = (fontSize.Width / 2) + 10;
                        break;

                    case ContentAlignment.TopRight:
                        yPixlesFromBottom = (int)(imageHeight * .95);
                        yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
                        xCenterOfImg = (imageWidth - (fontSize.Width / 2)) - 10;
                        break;
                }

                //Define the text layout by setting the text alignment to centered
                StringFormat StrFormat = new StringFormat();
                StrFormat.Alignment = StringAlignment.Center;

                //define a Brush which is semi trasparent black (Alpha set to 153)
                SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

                //Draw the copyright string
                shadowGraphic.DrawString(properties.CopyrightText,                 //string of text
                    font,                                   //font
                    semiTransBrush2,                           //Brush
                    new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
                    StrFormat);

                //define a Brush which is semi trasparent white (Alpha set to 153)
                SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

                //Draw the copyright string a second time to create a shadow effect
                //Make sure to move this text 1 pixel to the right and down 1 pixel
                shadowGraphic.DrawString(properties.CopyrightText,                 //string of text
                    font,                                   //font
                    semiTransBrush,                           //Brush
                    new PointF(xCenterOfImg, yPosFromBottom),  //Position
                    StrFormat);                               //Text alignment
            }
            #endregion

            #region Insert Watermark image
            //create a image object containing the watermark
            if (properties.UseImageWaterMark && properties.CopyrightImage != null) {
                //Create a Bitmap based on the previously modified photograph Bitmap
                Bitmap shadowBmpWithWatermark = new Bitmap(shadowBmp);
                shadowBmpWithWatermark.SetResolution(processedPhoto.HorizontalResolution, processedPhoto.VerticalResolution);
                //Load this Bitmap into a new Graphic Object
                Graphics shadowGraphicWithWatermark = Graphics.FromImage(shadowBmpWithWatermark);

                //To achieve a transulcent watermark we will apply (2) color 
                //manipulations by defineing a ImageAttributes object and 
                //seting (2) of its properties.
                ImageAttributes imageAttributes = new ImageAttributes();

                //The first step in manipulating the watermark image is to replace 
                //the background color with one that is trasparent (Alpha=0, R=0, G=0, B=0)
                //to do this we will use a Colormap and use this to define a RemapTable
                ColorMap colorMap = new ColorMap();

                //My watermark was defined with a background of 100% Green this will
                //be the color we search for and replace with transparency
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
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};
                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default,
                    ColorAdjustType.Bitmap);

                int watermarkWidth = properties.CopyrightImage.Width;
                int watermarkHeight = properties.CopyrightImage.Height;

                //For this example we will place the watermark in the upper right
                //hand corner of the photograph. offset down 10 pixels and to the 
                //left 10 pixles

                int xPosOfWatermark = 0;
                int yPosOfWatermark = 0;

                switch (properties.CopyrightImagePosition) {
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

                shadowGraphicWithWatermark.DrawImage(properties.CopyrightImage,
                    new Rectangle(xPosOfWatermark, yPosOfWatermark, watermarkWidth, watermarkHeight),  //Set the detination Position
                    0,                  // x-coordinate of the portion of the source image to draw. 
                    0,                  // y-coordinate of the portion of the source image to draw. 
                    watermarkWidth,            // Watermark Width
                    watermarkHeight,		    // Watermark Height
                    GraphicsUnit.Pixel, // Unit of measurment
                    imageAttributes);   //ImageAttributes Object

                //Replace the original photgraphs bitmap with the new Bitmap
                processedPhoto = shadowBmpWithWatermark;
                shadowGraphic.Dispose();
                shadowGraphicWithWatermark.Dispose();
            } else {
                processedPhoto = shadowBmp;
                shadowGraphic.Dispose();
            }
            #endregion

            #region Photo border
            if (properties.PhotoBorderWidth > 0) {
                processedPhoto = AddBorder(processedPhoto, properties.PhotoBorderWidth, properties.PhotoBorderColor);
            }
            #endregion

            #region Photo drop shadow
            if (properties.DropShadowDepth > 0) {
                processedPhoto = DropShadow(processedPhoto, properties.DropShadowColor,
                    properties.DropShadowDepth, properties.DropShadowMaxOpacity,
                    properties.DropShadowLocation);
            }
            #endregion

            #region Save processed photo
            //check file existence
            if (File.Exists(destinationPicture)) {
                File.Delete(destinationPicture);
            }

            //save new image to file system.
            if (format == ImageFormat.Jpeg && properties.JpegCompressionRatio > 0 && properties.JpegCompressionRatio <= 100) {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
			    //find the encoder with the image/jpeg mime-type
			    ImageCodecInfo imageCodec = null;
			    foreach (ImageCodecInfo codec in codecs) {
				    if (codec.MimeType == "image/jpeg") {
					    imageCodec = codec;
				    }
			    }

			    //Create a collection of encoder parameters (we only need one in the collection)
			    EncoderParameters encoder = new EncoderParameters();

			    //We'll save images with 25%, 50%, 75% and 100% quality as compared with the original
                encoder.Param[0] = new EncoderParameter(Encoder.Quality, (long)properties.JpegCompressionRatio);

                processedPhoto.Save(destinationPicture, imageCodec, encoder);
            } else {
                processedPhoto.Save(destinationPicture, format);
            }
            processedPhoto.Dispose();
            #endregion
        }

        private ImageFormat getImageFormat (string fileName) {
            Console.WriteLine(fileName.Substring(fileName.LastIndexOf(".")));
            switch (fileName.Substring(fileName.LastIndexOf(".")).ToLower()) {
                case ".bmp": return ImageFormat.Bmp;
                case ".jpg": return ImageFormat.Jpeg;
                case ".jpeg": return ImageFormat.Jpeg;
                default: return ImageFormat.Jpeg;
            }
        }

        /*private Image shrinkImage (Image originalPhoto, int newSize) {
            int originalWidth, originalHeight;
            originalWidth = originalPhoto.Width;
            originalHeight = originalPhoto.Height;

            if (shrink && shrinkPixelSize > 0 && shrinkPixelSize < originalWidth && shrinkPixelSize < originalHeight) {
                float ratio = (float)originalWidth / (float)originalHeight;

                // create an image in memory
                System.Drawing.Image newImage;
                if (originalWidth >= originalHeight) {
                    // wider picture
                    newImage = originalPhoto.GetThumbnailImage(newSize, (int)((float)newSize / ratio), null, new System.IntPtr());
                } else {
                    // taller picture
                    newImage = originalPhoto.GetThumbnailImage((int)((float)newSize * ratio), newSize, null, new System.IntPtr());
                }

                return newImage;
            } else {
                return originalPhoto;
            }
        }*/

        public Image ShrinkImage (Image original, int newSize) {
            int originalWidth, originalHeight;
            originalWidth = original.Width;
            originalHeight = original.Height;

            float ratio = (float)originalWidth / (float)originalHeight;
            int newWidth, newHeight;
            if (originalWidth >= originalHeight) {
                // wider picture
                newWidth = newSize;
                newHeight = (int)((float)newSize / ratio);
            } else {
                // taller picture
                newWidth = (int)((float)newSize * ratio);
                newHeight = newSize;
            }
            Bitmap newImage = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(newImage);

            //experiment with this...
            g.InterpolationMode = InterpolationMode.Bilinear;
            g.DrawImage(original, new Rectangle(0, 0, newImage.Width, newImage.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel);
            g.Dispose();
            return (Image)newImage;
        }

        public Image ConvertToGreyScale (Image original) {
            Bitmap shadowBmp = new Bitmap(original.Width, original.Height);
            Graphics shadowGraphic = Graphics.FromImage(shadowBmp);

            /*ColorMatrix matrix = new ColorMatrix(new float[][]{
                new float[]{0.5f,0.5f,0.5f,0,0},
                new float[]{0.5f,0.5f,0.5f,0,0},
                new float[]{0.5f,0.5f,0.5f,0,0},
                new float[]{0,0,0,1,0,0},
                new float[]{0,0,0,0,1,0},
                new float[]{0,0,0,0,0,1}}
            );*/

            //Gilles Khouzams colour corrected grayscale shear
            ColorMatrix matrix = new ColorMatrix(new float[][]{
				new float[]{0.3f,0.3f,0.3f,0,0},
				new float[]{0.59f,0.59f,0.59f,0,0},
				new float[]{0.11f,0.11f,0.11f,0,0},
				new float[]{0,0,0,1,0,0},
				new float[]{0,0,0,0,1,0},
				new float[]{0,0,0,0,0,1}}
            );

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix);
            shadowGraphic.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            shadowGraphic.Dispose();
            return (Image)shadowBmp;
        }

        public Image RevertImage (Image original) {
            Bitmap shadowBmp = new Bitmap(original.Width, original.Height);
            ImageAttributes attributes = new ImageAttributes();
            ColorMatrix matrix = new ColorMatrix();

            matrix.Matrix00 = matrix.Matrix11 = matrix.Matrix22 = -1;

            //matrix.Matrix00 = matrix.Matrix11 = matrix.Matrix22 = 0.99f;
            //matrix.Matrix33 = matrix.Matrix44 = 1;
            //matrix.Matrix40 = matrix.Matrix41 = matrix.Matrix42 = .04f;

            attributes.SetColorMatrix(matrix);

            Graphics shadowGraphics = Graphics.FromImage(shadowBmp);
            shadowGraphics.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            shadowGraphics.Dispose();
            original.Dispose();
            return (Image)shadowBmp;
        }

        public Image AddBorder (Image original, int borderWidth, Color borderColor) {
            int innerBorderWidth = 1;
            Bitmap bm = new Bitmap(original.Width + (borderWidth + innerBorderWidth) * 2, original.Height + (borderWidth + innerBorderWidth) * 2);
            Graphics g = Graphics.FromImage(bm);
            SolidBrush brush = new SolidBrush(borderColor);
            g.FillRectangle(brush, 0, 0, bm.Width, bm.Height);

            brush = new SolidBrush(Color.White);
            g.FillRectangle(brush, borderWidth, borderWidth,
                original.Width + innerBorderWidth * 2, original.Height + innerBorderWidth * 2);

            g.DrawImage(original, borderWidth + innerBorderWidth, borderWidth + innerBorderWidth);
            g.Dispose();
            return (Image)bm;
        }

        public Image DropShadow (Image original, Color shadowColor, int depth, int maxOpacity,
            DropShadowLocations location) {
            int originalWidth = original.Width;
            int originalHeight = original.Height;
            Bitmap tn = new Bitmap(originalWidth + depth, originalHeight + depth);
            Graphics tg = Graphics.FromImage(tn);

            Rectangle rc = new Rectangle(0, 0, originalWidth + depth, originalHeight + depth);

            //calculate the opacities 
            Color darkShadow = Color.FromArgb(maxOpacity, shadowColor);
            Color lightShadow = Color.FromArgb(0, shadowColor);

            //Create a brush that will create a softshadow circle 
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, 2 * depth, 2 * depth);
            PathGradientBrush pgb = new PathGradientBrush(gp);
            pgb.CenterColor = darkShadow;
            pgb.SurroundColors = new Color[] { lightShadow };

            //generate a softshadow pattern that can be used to paint the shadow 
            Bitmap patternbm = new Bitmap(2 * depth, 2 * depth);
            Graphics g = Graphics.FromImage(patternbm);
            g.FillEllipse(pgb, 0, 0, 2 * depth, 2 * depth);
            g.Dispose();
            pgb.Dispose();

            SolidBrush sb = new SolidBrush(properties.BackgroundColor);
            tg.FillRectangle(sb, rc);
            sb.Dispose();

            switch (location) {
                case DropShadowLocations.BottomLeft:
                    //bottom side 
                    tg.DrawImage(patternbm, new Rectangle(rc.Left + depth, rc.Bottom - depth, rc.Width - (2 * depth), depth),
                        depth, depth, 1, depth, GraphicsUnit.Pixel);

                    //bottom left corner 
                    tg.DrawImage(patternbm, new Rectangle(rc.Left, rc.Bottom - depth, depth, depth),
                        0, depth, depth, depth, GraphicsUnit.Pixel);

                    //left side 
                    tg.DrawImage(patternbm, new Rectangle(rc.Left, rc.Top + depth, depth, rc.Height - (2 * depth)),
                        0, depth, depth, 1, GraphicsUnit.Pixel);

                    // draw original photo
                    tg.DrawImage(original, depth, 0, originalWidth, originalHeight);

                    break;
                case DropShadowLocations.BottomRight:
                    //right side 
                    tg.DrawImage(patternbm, new Rectangle(rc.Right - depth, rc.Top + depth,
                        depth, rc.Height - (2 * depth)), depth, depth, depth, 1,
                        GraphicsUnit.Pixel);

                    //bottom right corner 
                    tg.DrawImage(patternbm, new Rectangle(rc.Right - depth, rc.Bottom - depth, depth, depth),
                        depth, depth, depth, depth, GraphicsUnit.Pixel);

                    //bottom side 
                    tg.DrawImage(patternbm, new Rectangle(rc.Left + depth, rc.Bottom - depth, rc.Width - (2 * depth), depth),
                        depth, depth, 1, depth, GraphicsUnit.Pixel);

                    // draw original photo
                    tg.DrawImage(original, 0, 0, originalWidth, originalHeight);

                    break;
                case DropShadowLocations.TopLeft:
                    //top left corner 
                    tg.DrawImage(patternbm, new Rectangle(rc.Left, rc.Top, depth, depth), 0, 0, depth, depth,
                        GraphicsUnit.Pixel);

                    //top side 
                    tg.DrawImage(patternbm, new Rectangle(rc.Left + depth, rc.Top, rc.Width - (2 * depth), depth),
                        depth, 0, 1, depth, GraphicsUnit.Pixel);

                    //left side 
                    tg.DrawImage(patternbm, new Rectangle(rc.Left, rc.Top + depth, depth, rc.Height - (2 * depth)),
                        0, depth, depth, 1, GraphicsUnit.Pixel);

                    // draw original photo
                    tg.DrawImage(original, depth, depth, originalWidth, originalHeight);

                    break;
                case DropShadowLocations.TopRight:
                    //top side 
                    tg.DrawImage(patternbm, new Rectangle(rc.Left + depth, rc.Top, rc.Width - (2 * depth), depth),
                        depth, 0, 1, depth, GraphicsUnit.Pixel);

                    //top right corner 
                    tg.DrawImage(patternbm, new Rectangle(rc.Right - depth, rc.Top, depth, depth),
                        depth, 0, depth, depth, GraphicsUnit.Pixel);

                    //right side 
                    tg.DrawImage(patternbm, new Rectangle(rc.Right - depth, rc.Top + depth,
                        depth, rc.Height - (2 * depth)), depth, depth, depth, 1,
                        GraphicsUnit.Pixel);

                    // draw original photo
                    tg.DrawImage(original, 0, depth, originalWidth, originalHeight);

                    break;
            }

            #region All 8 spots, commented
            /*
            //top left corner 
            tg.DrawImage(patternbm, new Rectangle(rc.Left, rc.Top, depth, depth), 0, 0, depth, depth,
                GraphicsUnit.Pixel);

            //top side 
            tg.DrawImage(patternbm, new Rectangle(rc.Left + depth, rc.Top, rc.Width - (2 * depth), depth),
                depth, 0, 1, depth, GraphicsUnit.Pixel);

            //top right corner 
            tg.DrawImage(patternbm, new Rectangle(rc.Right - depth, rc.Top, depth, depth),
                depth, 0, depth, depth, GraphicsUnit.Pixel);

            //right side 
            tg.DrawImage(patternbm, new Rectangle(rc.Right - depth, rc.Top + depth,
                depth, rc.Height - (2 * depth)), depth, depth, depth, 1,
                GraphicsUnit.Pixel);

            //bottom right corner 
            tg.DrawImage(patternbm, new Rectangle(rc.Right - depth, rc.Bottom - depth, depth, depth),
                depth, depth, depth, depth, GraphicsUnit.Pixel);

            //bottom side 
            tg.DrawImage(patternbm, new Rectangle(rc.Left + depth, rc.Bottom - depth, rc.Width - (2 * depth), depth),
                depth, depth, 1, depth, GraphicsUnit.Pixel);

            //bottom left corner 
            tg.DrawImage(patternbm, new Rectangle(rc.Left, rc.Bottom - depth, depth, depth),
                0, depth, depth, depth, GraphicsUnit.Pixel);

            //left side 
            tg.DrawImage(patternbm, new Rectangle(rc.Left, rc.Top + depth, depth, rc.Height - (2 * depth)),
                0, depth, depth, 1, GraphicsUnit.Pixel);*/
            #endregion

            patternbm.Dispose();

            //tn.Dispose();

            return (Image)tn;
        }
    }
}