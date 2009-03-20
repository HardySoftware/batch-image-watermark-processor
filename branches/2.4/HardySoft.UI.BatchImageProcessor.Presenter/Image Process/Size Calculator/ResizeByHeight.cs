using System.Windows;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class ResizeByHeight : IResize {
		public Size CalculateNewSize(Size originalSize, double newSize) {
			double originalWidth = originalSize.Width;
			double originalHeight = originalSize.Height;

			double ratio = originalWidth / originalHeight;

			double newWidth, newHeight;

			if (newSize > originalHeight) {
				newWidth = originalWidth;
				newHeight = originalHeight;
			} else {
				newWidth = newSize * ratio;
				newHeight = newSize;
			}

			return new Size(newWidth, newHeight);
		}
	}
}
