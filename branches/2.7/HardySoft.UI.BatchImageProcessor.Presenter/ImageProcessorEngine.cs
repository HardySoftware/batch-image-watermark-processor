using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

using HardySoft.UI.BatchImageProcessor.Model;

using Microsoft.Practices.Unity;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	class ImageProcessorEngine {
		private object syncRoot = new object();
		private uint threadNumber;
		private volatile bool stopFlag = false;
		private Queue<JobItem> jobQueue;
		private AutoResetEvent[] events;
		private ProjectSetting ps = null;
		private Thread[] threads;
		private IUnityContainer container = new UnityContainer();
		private List<ExifContainerItem> exifContainer;
		private string dateTimeStringFormat;

		public event ImageProcessedDelegate ImageProcessed;

		public ImageProcessorEngine(ProjectSetting ps,
			uint threadNumber, string dateTimeStringFormat,
			AutoResetEvent[] events, List<ExifContainerItem> exifContainer) {
			this.ps = ps;
			this.threadNumber = threadNumber;
			this.jobQueue = new Queue<JobItem>();
			this.events = events;
			this.threads = new Thread[this.threadNumber];
			this.exifContainer = exifContainer;
			this.dateTimeStringFormat = dateTimeStringFormat;

			// add all selected images to job queue.
			jobQueue = new Queue<JobItem>();
			uint index = 0;
			foreach (PhotoItem item in ps.Photos) {
				if (item.Selected) {
					jobQueue.Enqueue(new JobItem() {
						FileName = item.PhotoPath,
						Index = index
					});

					index++;
				}
			}

			//UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
			//section.Containers.Default.Configure(container);

			// register all supported image process classes
			/*container.RegisterType<IProcess, ApplyWatermarkText>("WatermarkText",
				new PerThreadLifetimeManager(),
				new InjectionConstructor(exifContainer, dateTimeStringFormat));*/
			container
				.RegisterType<IProcess, AddBorder>("AddBorder", new PerThreadLifetimeManager())
				.RegisterType<IProcess, ApplyWatermarkImage>("WatermarkImage", new TransientLifetimeManager())
				.RegisterType<IProcess, ApplyWatermarkText>("WatermarkText", new TransientLifetimeManager())
				.RegisterType<IProcess, DropShadowImage>("DropShadow", new PerThreadLifetimeManager())
				.RegisterType<IProcess, GenerateThumbnail>("ThumbImage", new PerThreadLifetimeManager())
				.RegisterType<IProcess, GrayScale>("GrayscaleEffect", new PerThreadLifetimeManager()/*,new InjectionProperty("EnableDebug")*/)
				.RegisterType<IProcess, NegativeImage>("NegativeEffect", new PerThreadLifetimeManager())
				.RegisterType<IProcess, OilPaint>("OilPaint", new PerThreadLifetimeManager())
				.RegisterType<IProcess, Relief>("Relief", new PerThreadLifetimeManager())
				.RegisterType<IProcess, ShrinkImage>("ShrinkImage", new PerThreadLifetimeManager())
				// file name classes
				.RegisterType<IFilenameProvider, ThumbnailFileName>("ThumbFileName", new PerThreadLifetimeManager())
				.RegisterType<IFilenameProvider, ProcessedFileName>("NormalFileName", new PerThreadLifetimeManager())
				.RegisterType<IFilenameProvider, BatchRenamedFileName>("BatchRenamedFileName", new PerThreadLifetimeManager())
				// register save image classes
				.RegisterType<ISaveImage, SaveNormalImage>("SaveNormalImage", new PerThreadLifetimeManager())
				.RegisterType<ISaveImage, SaveCompressedJPGImage>("SaveCompressedJpgImage", new PerThreadLifetimeManager(),
					new InjectionConstructor(ps.JpgCompressionRatio));
		}

		public void StartProcess() {
			this.stopFlag = false;

			for (int i = 0; i < this.threadNumber; i++) {
				threads[i] = new Thread(new ParameterizedThreadStart(processImage));
				threads[i].CurrentCulture = Thread.CurrentThread.CurrentCulture;
				threads[i].Name = i.ToString();
				threads[i].Start(i);
			}
		}

		public int JobSize {
			get {
				lock (syncRoot) {
					return jobQueue.Count;
				}
			}
		}

		protected void OnImageProcessed(ImageProcessedEventArgs args) {
			if (ImageProcessed != null) {
				ImageProcessed(args);
			}
		}

		private void processImage(object threadIndex) {
			int index = (int)threadIndex;
			System.Diagnostics.Debug.WriteLine("Thread " + index + " is created.");
			System.Diagnostics.Debug.WriteLine("Current Thread Culture " + Thread.CurrentThread.CurrentCulture.ToString() + " during processing.");

			string imagePath = string.Empty;
			uint imageIndex = 0;

			lock (syncRoot) {
				if (jobQueue.Count > 0) {
					JobItem item = jobQueue.Dequeue();
					imagePath = item.FileName;
					imageIndex = item.Index;
				} else {
					// nothing more to process, signal
#if DEBUG
					System.Diagnostics.Debug.WriteLine("Thread " + index + " is set because no more image to process.");
#endif
					events[index].Set();
					return;
				}
			}

			if (stopFlag) {
				// stop requested, signal
#if DEBUG
				System.Diagnostics.Debug.WriteLine("Thread " + index + " is set because stop requested.");
#endif
				events[index].Set();
				return;
			} else {
				ExifMetadata exif = null;

				Image normalImage = null;
				Image thumbImage = null;
				try {
					if (ps.KeepExif) {
						// keep exif information from original file
						exif = new ExifMetadata(new Uri(imagePath), true);
					}

					// this will lock image until entire application quits
					// normalImage = Image.FromFile(imagePath);
					// this won't lock image
					using (Stream stream = File.OpenRead(imagePath)) {
						normalImage = Image.FromStream(stream);
					}

					ImageFormat format = getImageFormat(imagePath);

					IProcess process;

					// thumbnail operation
					if (ps.ThumbnailSetting.GenerateThumbnail && ps.ThumbnailSetting.ThumbnailSize > 0) {
						process = container.Resolve<IProcess>("ThumbImage");
						thumbImage = process.ProcessImage(normalImage, ps);
					}

					// shrink image operation
					if (ps.ShrinkImage && ps.ShrinkPixelTo > 0) {
						process = container.Resolve<IProcess>("ShrinkImage");
						normalImage = process.ProcessImage(normalImage, this.ps);
					}

					// image process effect
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
							case ImageProcessType.OilPaint:
								process = container.Resolve<IProcess>("OilPaint");
								normalImage = process.ProcessImage(normalImage, null);
								break;
							case ImageProcessType.Relief:
								process = container.Resolve<IProcess>("Relief");
								normalImage = process.ProcessImage(normalImage, null);
								break;
							default:
								break;
						}
					}

					if (ps.WatermarkCollection != null && ps.WatermarkCollection.Count > 0) {
						IUnityContainer watermarkContainer;
						watermarkContainer = container.CreateChildContainer();
						watermarkContainer.RegisterInstance<List<ExifContainerItem>>(exifContainer)
							.RegisterInstance<string>(dateTimeStringFormat);

						for (int watermarkIndex = 0; watermarkIndex < ps.WatermarkCollection.Count; watermarkIndex++) {
							watermarkContainer.RegisterInstance<int>(watermarkIndex);

							if (ps.WatermarkCollection[watermarkIndex] is WatermarkText) {
								// text watermark operation
								WatermarkText wt = ps.WatermarkCollection[watermarkIndex] as WatermarkText;
								if (!string.IsNullOrEmpty(wt.Text) && wt.WatermarkTextColor.A > 0) {
#if DEBUG
									System.Diagnostics.Debug.WriteLine("Current Thread "
										+ System.Threading.Thread.CurrentThread.ManagedThreadId + " Culture "
										+ System.Threading.Thread.CurrentThread.CurrentCulture.ToString()
										+ " before ApplyWatermarkText.");

									System.Diagnostics.Debug.WriteLine("Current Thread: "
										+ System.Threading.Thread.CurrentThread.ManagedThreadId + ";"
										+ " Image File Name: " + imagePath + ","
										+ " Watermark Text index: " + watermarkIndex
										+ " before.");
#endif
									//process = container.Resolve<IProcess>("WatermarkText");
									process = watermarkContainer.Resolve<IProcess>("WatermarkText");
									process.ImageFileName = imagePath;
									normalImage = process.ProcessImage(normalImage, this.ps);
								}
							} else if (ps.WatermarkCollection[watermarkIndex] is WatermarkImage) {
								// image watermark operation
								WatermarkImage wi = ps.WatermarkCollection[watermarkIndex] as WatermarkImage;
								if (!string.IsNullOrEmpty(wi.WatermarkImageFile)
									&& File.Exists(wi.WatermarkImageFile)
									&& wi.WatermarkImageOpacity > 0) {
#if DEBUG
									System.Diagnostics.Debug.WriteLine("Current Thread: "
										+ System.Threading.Thread.CurrentThread.ManagedThreadId + ";"
										+ " Image File Name: " + imagePath + ","
										+ " Watermark Image index: " + watermarkIndex
										+ " before.");
#endif
									process = watermarkContainer.Resolve<IProcess>("WatermarkImage");
									process.ImageFileName = imagePath;
									normalImage = process.ProcessImage(normalImage, this.ps);
								}
							}
						}
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

					IFilenameProvider fileNameProvider;

					if (ps.RenamingSetting.EnableBatchRename) {
						fileNameProvider = container.Resolve<IFilenameProvider>("BatchRenamedFileName");
					} else {
						fileNameProvider = container.Resolve<IFilenameProvider>("NormalFileName");
					}
					fileNameProvider.PS = ps;
					fileNameProvider.ImageIndex = imageIndex;
					fileNameProvider.SourceFileName = imagePath;

					ISaveImage imageSaver;
					if (format == ImageFormat.Jpeg) {
						imageSaver = container.Resolve<ISaveImage>("SaveCompressedJpgImage");
						imageSaver.Exif = exif;
					} else {
						imageSaver = container.Resolve<ISaveImage>("SaveNormalImage");
					}
					imageSaver.SaveImageToDisk(normalImage, format, fileNameProvider);

					if (thumbImage != null) {
						// TODO think about applying thumbImage file name to batch renamed original file
						fileNameProvider = container.Resolve<IFilenameProvider>("ThumbFileName");
						fileNameProvider.PS = ps;
						fileNameProvider.ImageIndex = imageIndex;
						fileNameProvider.SourceFileName = imagePath;
						//saveImage(imagePath, thumbImage, format, fileNameProvider, imageSaver, imageIndex);
						imageSaver.SaveImageToDisk(thumbImage, format, fileNameProvider);
					}
				} catch (Exception ex) {
					Trace.TraceError(ex.ToString());
				} finally {
					normalImage.Dispose();
					normalImage = null;

					if (thumbImage != null) {
						thumbImage.Dispose();
						thumbImage = null;
					}

					ImageProcessedEventArgs args = new ImageProcessedEventArgs(imagePath);
					OnImageProcessed(args);
				}

				// recursively call itself to go back to check if there are more files waiting to be processed.
				processImage(threadIndex);
			}
		}

		public void StopProcess() {
			lock (syncRoot) {
				this.stopFlag = true;
			}
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

		/*private bool saveImage(string originalFilename, Image image, ImageFormat format,
			IFilenameProvider fileNameProvider, ISaveImage save, uint imageIndex) {
			if (image == null) {
				// nothing to save
				return true;
			}

			string filename = fileNameProvider.GetFileName(originalFilename, ps, imageIndex);
			return save.SaveImageToDisk(image, filename, format);
		}*/
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