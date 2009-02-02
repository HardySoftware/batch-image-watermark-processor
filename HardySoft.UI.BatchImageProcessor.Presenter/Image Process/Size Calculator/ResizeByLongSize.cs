using System.Windows;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class ResizeByLongSide : IResize {
		public Size CalculateNewSize(Size originalSize, double newSize) {
			double originalWidth = originalSize.Width;
			double originalHeight = originalSize.Height;

			double ratio = originalWidth / originalHeight;

			double newWidth, newHeight;
			if (originalWidth >= originalHeight) {
				// wider picture

				if (newSize > originalWidth) {
					// we don't enlarge image
					newWidth = originalWidth;
					newHeight = originalHeight;
				} else {
					newWidth = newSize;
					newHeight = newSize / ratio;
				}
			} else {
				// taller picture

				if (newSize > originalHeight) {
					newWidth = originalWidth;
					newHeight = originalHeight;
				} else {
					newWidth = newSize * ratio;
					newHeight = newSize;
				}
			}

			return new Size(newWidth, newHeight);
		}
	}
}
