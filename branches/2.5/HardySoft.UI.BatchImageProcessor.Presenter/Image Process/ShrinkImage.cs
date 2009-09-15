using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

using HardySoft.UI.BatchImageProcessor.Model;

using Microsoft.Practices.Unity;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class ShrinkImage : ResizeImage {
		public override Image ProcessImage(Image input, ProjectSetting ps) {
			try {
				if (ps.ShrinkPixelTo > 0) {
					IUnityContainer container = new UnityContainer();
					//container.RegisterType<IResize, ResizeByLongSide>("LongSide", new PerThreadLifetimeManager());
					//container.RegisterType<IResize, ResizeByWidth>("Width", new PerThreadLifetimeManager());
					//container.RegisterType<IResize, ResizeByHeight>("Height", new PerThreadLifetimeManager());
					container.RegisterType<IResize, ResizeByLongSide>("LongSide", new ContainerControlledLifetimeManager());
					container.RegisterType<IResize, ResizeByWidth>("Width", new ContainerControlledLifetimeManager());
					container.RegisterType<IResize, ResizeByHeight>("Height", new ContainerControlledLifetimeManager());

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
			} catch (Exception ex) {
				Trace.TraceError(ex.ToString());
				return input;
			}
		}
	}
}