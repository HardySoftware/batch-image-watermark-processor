using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	class SaveCompressedJPGImage : ISaveImage {
		#region ISaveImage Members

		public bool SaveImageToDisk(System.Drawing.Image image, string fileName, System.Drawing.Imaging.ImageFormat format) {
			throw new NotImplementedException();
		}

		#endregion
	}
}
