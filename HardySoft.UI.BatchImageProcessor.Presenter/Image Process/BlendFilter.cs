using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	internal class BlendFilter : BaseInPlaceFilter2 {
		Func<int, int, int> blendMethod;
		Dictionary<PixelFormat, PixelFormat> formatTranslations;

		public BlendFilter(Func<int, int, int> blendMethod) {
			this.blendMethod = blendMethod;
			this.formatTranslations = new Dictionary<PixelFormat, PixelFormat>();
			this.InitFormatTranslations();
		}

		public BlendFilter(Func<int, int, int> blendMethod, UnmanagedImage unmanagedOverlayImage)
			: base(unmanagedOverlayImage) {
			this.blendMethod = blendMethod;
			this.formatTranslations = new Dictionary<PixelFormat, PixelFormat>();
			this.InitFormatTranslations();
		}

		public BlendFilter(Func<int, int, int> blendMethod, Bitmap overlayImage)
			: base(overlayImage) {

			this.blendMethod = blendMethod;
			this.formatTranslations = new Dictionary<PixelFormat, PixelFormat>();
			this.InitFormatTranslations();
		}

		void InitFormatTranslations() {
			this.formatTranslations[PixelFormat.Format8bppIndexed] = PixelFormat.Format8bppIndexed;
			this.formatTranslations[PixelFormat.Format24bppRgb] = PixelFormat.Format24bppRgb;
			this.formatTranslations[PixelFormat.Format32bppRgb] = PixelFormat.Format32bppRgb;
			this.formatTranslations[PixelFormat.Format32bppArgb] = PixelFormat.Format32bppArgb;
			this.formatTranslations[PixelFormat.Format16bppGrayScale] = PixelFormat.Format16bppGrayScale;
			this.formatTranslations[PixelFormat.Format48bppRgb] = PixelFormat.Format48bppRgb;
			this.formatTranslations[PixelFormat.Format64bppArgb] = PixelFormat.Format64bppArgb;
		}

		protected override unsafe void ProcessFilter(UnmanagedImage image, UnmanagedImage overlay) {
			PixelFormat pixelFormat = image.PixelFormat;
			int width = image.Width;
			int height = image.Height;

			switch (pixelFormat) {
				case PixelFormat.Format8bppIndexed:
				case PixelFormat.Format24bppRgb:
				case PixelFormat.Format32bppRgb:
				case PixelFormat.Format32bppArgb: {
						BlendMethod1(image, overlay, pixelFormat, width, height);
						return;
					}
			}

			BlendMethod2(image, overlay, pixelFormat, width, height);
		}

		unsafe void BlendMethod2(UnmanagedImage image, UnmanagedImage overlay, PixelFormat pixelFormat, int width, int height) {
			int bytesPerPixel = (pixelFormat == PixelFormat.Format16bppGrayScale) ? 1 : ((pixelFormat == PixelFormat.Format48bppRgb) ? 3 : 4);
			int widthBytes = width * bytesPerPixel;
			int stride = image.Stride;
			int overlayStride = overlay.Stride;
			int imageBase = (int)image.ImageData.ToPointer();
			int overlayBase = (int)overlay.ImageData.ToPointer();
			for (int i = 0; i < height; i++) {
				ushort* B = (ushort*)(imageBase + (i * stride));
				ushort* L = (ushort*)(overlayBase + (i * overlayStride));
				int currentByte = 0;
				while (currentByte < widthBytes) {
					int outputByte = blendMethod(B[0], L[0]);
					B[0] = (ushort)Math.Min(outputByte, 65535);
					currentByte++;
					B++;
					L++;
				}
			}
		}

		unsafe void BlendMethod1(UnmanagedImage image, UnmanagedImage overlay, PixelFormat pixelFormat, int width, int height) {
			int bytesPerPixel = (pixelFormat == PixelFormat.Format8bppIndexed) ? 1 : ((pixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4);
			int widthBytes = width * bytesPerPixel;
			int imagePaddingBytes = image.Stride - widthBytes;
			int overlayPaddingBytes = overlay.Stride - widthBytes;
			byte* B = (byte*)image.ImageData.ToPointer();
			byte* L = (byte*)overlay.ImageData.ToPointer();
			for (int j = 0; j < height; j++) {
				int currentByte = 0;
				while (currentByte < widthBytes) {
					int outputByte = blendMethod(B[0], L[0]);
					B[0] = (byte)Math.Min(outputByte, 255);
					currentByte++;
					B++;
					L++;
				}
				B += imagePaddingBytes;
				L += overlayPaddingBytes;
			}
		}

		public override Dictionary<PixelFormat, PixelFormat> FormatTranslations {
			get {
				return this.formatTranslations;
			}
		}
	}
}
