using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class ResizeByWidth : IResize {
		public Size CalculateNewSize(Size originalSize, double newSize) {
			double originalWidth = originalSize.Width;
			double originalHeight = originalSize.Height;

			double ratio = originalWidth / originalHeight;

			double newWidth, newHeight;

			if (newSize > originalWidth) {
				// we don't enlarge image
				newWidth = originalWidth;
				newHeight = originalHeight;
			} else {
				newWidth = newSize;
				newHeight = newSize / ratio;
			}

			return new Size(newWidth, newHeight);
		}
	}
}
