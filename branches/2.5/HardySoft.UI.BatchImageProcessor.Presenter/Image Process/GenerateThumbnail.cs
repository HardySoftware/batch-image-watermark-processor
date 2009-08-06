using System.Drawing;
using System.Drawing.Drawing2D;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class GenerateThumbnail : ResizeImage {
		public override Image ProcessImage(Image input, ProjectSetting ps) {
			ResizeByLongSide resize = new ResizeByLongSide();
			System.Windows.Size currentSize = new System.Windows.Size((double)input.Width, (double)input.Height);
			System.Windows.Size newSize = resize.CalculateNewSize(currentSize, (double)ps.ThumbnailSetting.ThumbnailSize);
			return ResizeImageJob(input, newSize, InterpolationMode.Bicubic);
		}
	}
}