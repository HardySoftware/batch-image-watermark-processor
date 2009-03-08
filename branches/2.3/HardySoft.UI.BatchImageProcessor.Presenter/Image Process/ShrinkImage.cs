using System.Drawing;
using System.Drawing.Drawing2D;

using Microsoft.Practices.Unity;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class ShrinkImage : ResizeImage {
		public override Image ProcessImage(Image input, ProjectSetting ps) {
			if (ps.ShrinkPixelTo > 0) {
				IUnityContainer container = new UnityContainer();
				// TODO register component in config file
				container.RegisterType<IResize, ResizeByLongSide>("LongSide", new PerThreadLifetimeManager());
				container.RegisterType<IResize, ResizeByWidth>("Width", new PerThreadLifetimeManager());
				container.RegisterType<IResize, ResizeByHeight>("Height", new PerThreadLifetimeManager());

				IResize resize = null;
				switch (ps.ShrinkMode) {
					case ShrinkImageMode.Height:
						resize = container.Resolve<IResize>("Height");
						break;
					case ShrinkImageMode.Width:
						resize = container.Resolve<IResize>("Width");
						break;
					case ShrinkImageMode.LongSide:
						resize = container.Resolve<IResize>("LongSide");
						break;
				}

				container.Dispose();

				if (resize != null) {
					System.Windows.Size currentSize = new System.Windows.Size((double)input.Width, (double)input.Height);
					System.Windows.Size newSize = resize.CalculateNewSize(currentSize, (double)ps.ShrinkPixelTo);
					return ResizeImageJob(input, newSize, InterpolationMode.HighQualityBicubic);
				} else {
					return input;
				}
			} else {
				// if no new size is specified, don't do the job.
				return input;
			}
		}
	}
}
