using System.Drawing;
using System.Drawing.Imaging;
using AForge.Imaging.Filters;
using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class PencilSketch : IProcess {
		public string ImageFileName {
			get;
			set;
		}

		public Image ProcessImage(Image input, ProjectSetting ps) {
			Bitmap image = new Bitmap(input);

			if (input.PixelFormat != PixelFormat.Format24bppRgb 
				&& input.PixelFormat != PixelFormat.Format16bppGrayScale
				&& input.PixelFormat != PixelFormat.Format8bppIndexed) {
				// Aforge library only support certian bpp, need to do a convert first
					image = this.ChangePixelFormat(image, PixelFormat.Format24bppRgb);
			}

			var layerA = new SaturationCorrection(-71).Apply(image);

			var layerB = new Invert().Apply(layerA);
			layerB = new GaussianBlur().Apply(layerB);

			layerA = new BlendFilter(BlendMode.ColorDodge, layerB).Apply(layerA);
			layerA = new GammaCorrection(-5).Apply(layerA);

			//layerA = new BlendFilter(BlendMode.Overlay, layerA).Apply(image);
			return (Image)layerA;
		}

		private Bitmap ChangePixelFormat(Bitmap inputOriginalImage, PixelFormat newPixelFormat) {
			Bitmap resultingImage = new Bitmap(inputOriginalImage.Width, inputOriginalImage.Height, newPixelFormat);
			using (Graphics g = Graphics.FromImage(resultingImage)) {
				g.DrawImage(inputOriginalImage, 0, 0);
			}
			return resultingImage;
		}
	}
}
