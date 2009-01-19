using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;

using Microsoft.Practices.Unity;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	class ImageProcessorEngine {
		private object syncRoot = new object();
		private uint threadNumber;
		private volatile bool stopFlag = false;
		private Queue<string> jobQueue;
		private AutoResetEvent[] events;
		private ProjectSetting ps = null;

		public ImageProcessorEngine(uint threadNumber, AutoResetEvent[] events) {
			this.threadNumber = threadNumber;
			this.jobQueue = new Queue<string>();
			this.events = events;
		}

		public void StartProcess(ProjectSetting ps) {
			this.stopFlag = false;
			this.ps = ps;

			foreach (PhotoItem item in ps.Photos) {
				if (item.Selected) {
					jobQueue.Enqueue(item.PhotoPath);
				}
			}

			for (int i = 0; i < this.threadNumber; i++) {
				Thread t = new Thread(new ParameterizedThreadStart(processImage));
				t.Name = i.ToString();
				t.Start(i);
			}
		}

		private void processImage(object threadIndex) {
			int index = (int)threadIndex;

			IUnityContainer container = new UnityContainer();
			// register all supported image process classes
			container.RegisterType<IProcess, AddBorder>("AddBorder", new PerThreadLifetimeManager());
			container.RegisterType<IProcess, ApplyWatermarkImage>("WatermarkImage", new PerThreadLifetimeManager());
			container.RegisterType<IProcess, ApplyWatermarkText>("WatermarkText", new PerThreadLifetimeManager());
			container.RegisterType<IProcess, DropShadowImage>("DropShadow", new PerThreadLifetimeManager());
			container.RegisterType<IProcess, GenerateThumbnail>("ThumbImage", new PerThreadLifetimeManager());
			container.RegisterType<IProcess, GrayScale>("GrayscaleEffect", new PerThreadLifetimeManager());
			container.RegisterType<IProcess, NegativeImage>("NegativeEffect", new PerThreadLifetimeManager());
			container.RegisterType<IProcess, ShrinkImage>("ShrinkImage", new PerThreadLifetimeManager());
			
			string imagePath = string.Empty;

			lock(syncRoot) {
				if (jobQueue.Count > 0) {
					imagePath = jobQueue.Dequeue();
				} else {
					// nothing more to process, signal
					events[index].Set();
				}
			}

			if (stopFlag) {
				// stop requested, signal
				events[index].Set();
			} else {
				Image normalImage = null;
				Image thumb = null;
				try {
					normalImage = Image.FromFile(imagePath);

					ImageFormat format = getImageFormat(imagePath);

					IProcess process;

					// thumbnail operation
					if (ps.ThumbnailSetting.GenerateThumbnail && ps.ThumbnailSetting.ThumbnailSize > 0) {
						process = container.Resolve<IProcess>("ThumbImage");
						thumb = process.ProcessImage(normalImage, ps);
					}

					// shrink image operation
					if (ps.ShrinkImage && ps.ShrinkLongSidePixelTo > 0) {
						process = container.Resolve<IProcess>("ShrinkImage");
						normalImage = process.ProcessImage(normalImage, this.ps);
					}

					// extra image effect
					if (ps.ProcessType != ImageProcessType.None) {
						switch (ps.ProcessType) {
							case ImageProcessType.GrayScale:
								process = container.Resolve<IProcess>("GrayscaleEffect");
								normalImage = process.ProcessImage(normalImage, null);
								break;
							case ImageProcessType.NagativeImage:
								process = container.Resolve<IProcess>("NegativeEffect");
								normalImage = process.ProcessImage(normalImage, null);
								break;
							default:
								break;
						}
					}

					// text watermark operation
					if (!string.IsNullOrEmpty(ps.Watermark.WatermarkText)) {
						process = container.Resolve<IProcess>("WatermarkText");
						normalImage = process.ProcessImage(normalImage, this.ps);
					}

					// image watermark operation
					if (!string.IsNullOrEmpty(ps.Watermark.WatermarkImageFile)) {
						process = container.Resolve<IProcess>("WatermarkImage");
						normalImage = process.ProcessImage(normalImage, this.ps);
					}

					// border operation
					if (ps.BorderSetting.BorderWidth > 0) {
						process = container.Resolve<IProcess>("AddBorder");
						normalImage = process.ProcessImage(normalImage, this.ps);
					}

					// drop shadow operation
					if (ps.DropShadowSetting.ShadowDepth > 0) {
						process = container.Resolve<IProcess>("DropShadow");
						normalImage = process.ProcessImage(normalImage, this.ps);
					}

					saveProcessedImage(imagePath, normalImage, thumb);
				} catch (Exception ex) {
					// TODO add logic
				}
				finally {
					normalImage.Dispose();
				}

				processImage(threadIndex);
			}
		}

		public void StopProcess() {
			this.stopFlag = true;
		}

		private ImageFormat getImageFormat(string fileName) {
			FileInfo fi = new FileInfo(fileName);

			switch (fi.Extension.ToLower()) {
				case ".bmp":
					return ImageFormat.Bmp;
				case ".jpg":
					return ImageFormat.Jpeg;
				case ".jpeg":
					return ImageFormat.Jpeg;
				case ".gif":
					return ImageFormat.Gif;
				case ".png":
					return ImageFormat.Png;
				default:
					return ImageFormat.Jpeg;
			}
		}

		private bool saveProcessedImage(string originalFilename, Image normalImage, Image thumb) {
			return false;
		}
	}
}
