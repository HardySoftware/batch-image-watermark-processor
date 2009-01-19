using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class GenerateThumbnail : ResizeImage {
		public override Image ProcessImage(Image input, ProjectSetting ps) {
			return ResizeImageJob(input, ps.ThumbnailSetting.ThumbnailSize);
		}
	}
}
