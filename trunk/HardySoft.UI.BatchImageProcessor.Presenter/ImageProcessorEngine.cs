using System;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Microsoft.Practices.Unity;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	class ImageProcessorEngine {
		private object syncRoot = new object();
		private uint threadNumber;
		private volatile bool stopFlag = false;
		private Queue<JobItem> jobQueue;
		private AutoResetEvent[] events;
		private ProjectSetting ps = null;

		public ImageProcessorEngine(uint threadNumber, AutoResetEvent[] events) {
			this.threadNumber = threadNumber;
			this.jobQueue = new Queue<JobItem>();
			this.events = events;
		}

		public void StartProcess(ProjectSetting ps) {
			this.stopFlag = false;
			this.ps = ps;

			// add all selected images to job queue.
			uint index = 0;
			foreach (PhotoItem item in ps.Photos) {
				if (item.Selected) {
					jobQueue.Enqueue(new JobItem()
					{
						FileName = item.PhotoPath,
						Index = index
					});

					index++;
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
			// register generate file name classes
			container.RegisterType<IFilenameProvider, ThumbnailFileName>("ThumbFileName", new PerThreadLifetimeManager());
			container.RegisterType<IFilenameProvider, ProcessedFileName>("NormalFileName", new PerThreadLifetimeManager());
			container.RegisterType<IFilenameProvider, BatchRenamedFileName>("BatchRenamedFileName", new PerThreadLifetimeManager());
			// register save image classes
			container.RegisterType<ISaveImage, SaveNormalImage>("SaveNormalImage", new PerThreadLifetimeManager());
			container.RegisterType<ISaveImage, SaveCompressedJPGImage>("SaveCompressedJpgImage", new PerThreadLifetimeManager());
			
			string imagePath = string.Empty;
			uint imageIndex = 0;

			lock(syncRoot) {
				if (jobQueue.Count > 0) {
					JobItem item = jobQueue.Dequeue();
					imagePath = item.FileName;
					imageIndex = item.Index;
				} else {
					// nothing more to process, signal
					events[index].Set();
					return;
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
					if (!string.IsNullOrEmpty(ps.Watermark.WatermarkImageFile) 
						&& ! string.IsNullOrEmpty(ps.Watermark.WatermarkImageFile)) {
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

					ISaveImage imageSaver;

					if (format == ImageFormat.Jpeg
						&& ps.JpgCompressionRatio > 0
						&& ps.JpgCompressionRatio < 100) {
						imageSaver = container.Resolve<ISaveImage>("SaveCompressedJPGImage");
					} else {
						imageSaver = container.Resolve<ISaveImage>("SaveNormalImage");
					}

					IFilenameProvider fileNameProvider;

					if (ps.RenamingSetting.EnableBatchRename) {
						fileNameProvider = container.Resolve<IFilenameProvider>("BatchRenamedFileName");
					} else {
						fileNameProvider = container.Resolve<IFilenameProvider>("NormalFileName");
					}
					saveImage(imagePath, normalImage, format, fileNameProvider, imageSaver);

					fileNameProvider = container.Resolve<IFilenameProvider>("ThumbFileName");
					fileNameProvider.ImageIndex = imageIndex;
					saveImage(imagePath, thumb, format, fileNameProvider, imageSaver);
				} catch (Exception ex) {
					// TODO add logic
				}
				finally {
					normalImage.Dispose();

					if (thumb != null) {
						thumb.Dispose();
					}
				}

				// go back to check if there are more files waiting to be processed.
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

		private bool saveImage(string originalFilename, Image image, ImageFormat format,
			IFilenameProvider fileNameProvider, ISaveImage save) {
			if (image == null) {
				// nothing to save
				return true;
			}

			string filename = fileNameProvider.GetFileName(originalFilename, ps);
			return save.SaveImageToDisk(image, filename, format);
		}
	}

	class JobItem {
		public string FileName {
			get;
			set;
		}

		public uint Index {
			get;
			set;
		}
	}
}