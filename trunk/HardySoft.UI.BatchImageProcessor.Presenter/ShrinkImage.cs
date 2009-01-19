using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Threading;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class ShrinkImage : ResizeImage {
		public override Image ProcessImage(Image input, ProjectSetting ps) {
		
			return ResizeImageJob(input, ps.ShrinkLongSidePixelTo);
		}
	}
}
