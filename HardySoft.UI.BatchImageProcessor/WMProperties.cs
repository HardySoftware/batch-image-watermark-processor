using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Reflection;
using System.Runtime.Serialization;

namespace HardySoft.UI.BatchImageProcessor {
    /// <summary>
    /// Customer class to be displayed in the property grid
    /// </summary>
    [Serializable]
    [DefaultPropertyAttribute("CopyrightText")]
    public class WaterMarkProperties {
        //private string waterMarkImageFile;
        private bool useImageWaterMark;
        private string copyrightText;
        private System.Drawing.ContentAlignment copyrightTextPosition;
        private System.Drawing.Image copyrightImage;
        private System.Drawing.ContentAlignment copyrightImagePosition;
        private System.Drawing.Font copyrightTextFont;
        //private bool _bestfitfontsize;
        private bool shrinkImage;
        private int shrinkLongSidePixelTo;
        private int jpegCompressionRatio;
        private ImageProcessTypes imageProcessType;
        private Color photoBorderColor;
        private int photoBorderWidth;
        private int dropShadowDepth;
        private Color dropShadowColor;
        private DropShadowLocations dropShadowLocation;
        private int dropShadowMaxOpacity;
        private Color backgroundColor;
        private bool generateThumbNail;
        private int thumbNailSize;
        private string thumbNailFileSuffix;
        private string thumbNailFilePrefix;

        public WaterMarkProperties () {
            //shrinkLongSidePixel = 800;
            setDefaultValues();
        }

        #region Photo settings
        [CategoryAttribute("Photo Settings"),
        DescriptionAttribute("Indicates the background color of the entire photo, including border and shadow."),
        DefaultValueAttribute(typeof(Color), "White")]
        public Color BackgroundColor {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        [CategoryAttribute("Photo Settings"),
        DescriptionAttribute("Adds border to photo with designated color."),
        DefaultValueAttribute(typeof(Color), "Black")]
        public Color PhotoBorderColor {
            get { return photoBorderColor; }
            set { photoBorderColor = value; }
        }

        [CategoryAttribute("Photo Settings"),
        DescriptionAttribute("Adds border to photo with designated width."),
        DefaultValueAttribute(0)]
        public int PhotoBorderWidth {
            get { return photoBorderWidth; }
            set { photoBorderWidth = value; }
        }

        [CategoryAttribute("Photo Settings"),
        DescriptionAttribute("Indicates process type to original image."),
        DefaultValueAttribute(ImageProcessTypes.None)]
        public ImageProcessTypes ImageProcessType {
            get { return imageProcessType; }
            set { imageProcessType = value; }
        }

        [CategoryAttribute("Photo Settings"),
        DescriptionAttribute("Shrinks long side of original image to certain pixel. If the input value is greater than long side of original image, then nothing happens. If input value is 0 then nothing happens."),
        DefaultValueAttribute(800)]
        public int ShrinkLongSidePixelTo {
            get { return shrinkLongSidePixelTo; }
            set {
                if (value <= 0) {
                    shrinkLongSidePixelTo = 0;
                } else {
                    shrinkLongSidePixelTo = value;
                }
            }
        }

        [CategoryAttribute("Photo Settings"),
        DescriptionAttribute("Indicates if to shrink original image."),
        DefaultValueAttribute(false)]
        public bool ShrinkImage {
            get { return shrinkImage; }
            set { shrinkImage = value; }
        }

        [CategoryAttribute("Photo Settings"),
        DescriptionAttribute("Indicates depth of drop shadow."),
        DefaultValueAttribute(0)]
        public int DropShadowDepth {
            get { return dropShadowDepth; }
            set { dropShadowDepth = value; }
        }

        [CategoryAttribute("Photo Settings"),
        DescriptionAttribute("Indicates color of drop shadow."),
        DefaultValueAttribute(typeof(Color), "Black")]
        public Color DropShadowColor {
            get { return dropShadowColor; }
            set { dropShadowColor = value; }
        }

        [CategoryAttribute("Photo Settings"),
        DescriptionAttribute("Indicates location of drop shadow."),
        DefaultValueAttribute(DropShadowLocations.BottomRight)]
        public DropShadowLocations DropShadowLocation {
            get { return dropShadowLocation; }
            set { dropShadowLocation = value; }
        }

        [CategoryAttribute("Photo Settings"),
        DescriptionAttribute("Indicates max opacity of drop shadow. The value should be an integer 0 through 255."),
        DefaultValueAttribute(255),
        Editor(typeof(ElementColorValueRangeEditor), typeof(UITypeEditor))]
        public int DropShadowMaxOpacity {
            get { return dropShadowMaxOpacity; }
            set {
                if (value < 0) {
                    dropShadowMaxOpacity = 0;
                } else if (dropShadowMaxOpacity > 255) {
                    dropShadowMaxOpacity = 255;
                } else {
                    dropShadowMaxOpacity = value;
                }
            }
        }
        #endregion

        #region Copyright Text
        [CategoryAttribute("Copyright Text Settings"),
        DescriptionAttribute("Your copyright text ..."),
        DefaultValueAttribute("Copyright © "),
        Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string CopyrightText {
            get {
                return copyrightText;
            }
            set {
                copyrightText = value.Trim();
            }
        }

        [CategoryAttribute("Copyright Text Settings"),
        DescriptionAttribute("Indicates copyright text position in the image."),
        DefaultValueAttribute(ContentAlignment.BottomCenter)]
        public ContentAlignment CopyrightTextPosition {
            get {
                return copyrightTextPosition;
            }
            set {
                copyrightTextPosition = value;
            }
        }

        [CategoryAttribute("Copyright Text Settings"),
        DescriptionAttribute("Indicates copyright text font."),
        DefaultValueAttribute(typeof(Font), "Arial, 8.25pt, style=Bold")]
        public System.Drawing.Font CopyrightTextFont {
            get {
                return copyrightTextFont;
            }
            set {
                copyrightTextFont = value;
            }
        }

        /*
        [CategoryAttribute("Copyright Text Settings"),
        DescriptionAttribute("Copyright text font."),
        DefaultValueAttribute(true)]
        [ReadOnly(true)]
        public bool FontSizeBestFit {
            get {
                return _bestfitfontsize;
            }
            set {
                _bestfitfontsize = value;
            }
        }*/
        #endregion

        #region Water mark image
        [CategoryAttribute("Copyright Image Settings"),
        DescriptionAttribute("Copyright image."),
        DefaultValueAttribute(null)]
        public System.Drawing.Image CopyrightImage {
            get {
                return copyrightImage;
            }
            set {
                copyrightImage = value;
            }
        }

        [CategoryAttribute("Copyright Image Settings"),
        DescriptionAttribute("Indicates copyright image position in the main Image."),
        DefaultValueAttribute(ContentAlignment.TopRight)]
        public ContentAlignment CopyrightImagePosition {
            get {
                return copyrightImagePosition;
            }
            set {
                copyrightImagePosition = value;
            }
        }

        [CategoryAttribute("Copyright Image Settings"),
        DescriptionAttribute("Indicates if image is used as copyright water mark."),
        DefaultValueAttribute(false)]
        public bool UseImageWaterMark {
            get {
                return useImageWaterMark;
            }
            set {
                useImageWaterMark = value;
            }
        }

        /*
        [CategoryAttribute("Copyright Image Settings"),
        DescriptionAttribute("Image file location of copyright water mark."),
        Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string WaterMarkImageFile {
            get {
                return waterMarkImageFile;
            }
            set {
                waterMarkImageFile = value;
            }
        }*/
        #endregion

        #region Save Options
        [CategoryAttribute("Save Options"),
        DescriptionAttribute("Indicates the compression ratio of JPEG file, the value should be 0 through 100. 0 means default."),
        DefaultValueAttribute(0),
       Editor(typeof(CompressionRatioValueRangeEditor), typeof(UITypeEditor))]
        public int JpegCompressionRatio {
            get {
                return jpegCompressionRatio;
            }
            set {
                if (value > 100 || value < 0) {
                    jpegCompressionRatio = 0;
                } else {
                    jpegCompressionRatio = value;
                }
            }
        }
        #endregion

        #region Thumbmail
        [CategoryAttribute("Thumbnail Image Options"),
        DescriptionAttribute("Indicates whether to generate thumbnail images at same time."),
        DefaultValueAttribute(false)]
        public bool GenerateThumbNail {
            get { return generateThumbNail; }
            set { generateThumbNail = value; }
        }

        [CategoryAttribute("Thumbnail Image Options"),
        DescriptionAttribute("Indicates long side size of thumbnail image. If value is 0, then no thumn nail image will be gerenated."),
        DefaultValueAttribute(100)]
        public int ThumbNailSize {
            get { return thumbNailSize; }
            set {
                if (value < 0) {
                    thumbNailSize = 0;
                } else {
                    thumbNailSize = value;
                }
            }
        }

        [CategoryAttribute("Thumbnail Image Options"),
        DescriptionAttribute("Indicates suffix of thumbnail image file."),
        DefaultValueAttribute("_thumb")]
        public string ThumbNailFileSuffix {
            get {
                if (thumbNailFileSuffix == null) {
                    return string.Empty;
                } else {
                    return thumbNailFileSuffix;
                }
            }
            set {
                thumbNailFileSuffix = value.Trim();
            }
        }

        [CategoryAttribute("Thumbnail Image Options"),
        DescriptionAttribute("Indicates prefix of thumbnail image file."),
        DefaultValueAttribute("thumb_")]
        public string ThumbNailFilePrefix {
            get {
                if (thumbNailFilePrefix == null) {
                    return string.Empty;
                } else {
                    return thumbNailFilePrefix;
                }
            }
            set {
                thumbNailFilePrefix = value.Trim();
            }
        }
        #endregion

        private void setDefaultValues () {
            PropertyInfo[] props = this.GetType().GetProperties();
            for (int i = 0; i < props.Length; i++) {
                object[] attrs = props[i].GetCustomAttributes(typeof(DefaultValueAttribute), false);
                if (attrs.Length > 0) {
                    DefaultValueAttribute attr = (DefaultValueAttribute)attrs[0];
                    props[i].SetValue(this, attr.Value, null);
                }
            }
        }
    }

    public enum ImageProcessTypes {
        None,
        GrayScale,
        NegativeImage
    }

    public enum DropShadowLocations {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}