using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class ImageProcessedEventArgs : EventArgs {
		private string imageFileName;

		public ImageProcessedEventArgs(string imageFileName)
			: base() {
			this.imageFileName = imageFileName;
		}

		public string ImageFileName {
			get {
				return this.imageFileName;
			}
		}
	}

	public delegate void ImageProcessedDelegate(ImageProcessedEventArgs args);
}
