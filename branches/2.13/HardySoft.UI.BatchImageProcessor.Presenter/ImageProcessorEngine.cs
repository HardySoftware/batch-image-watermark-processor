using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using HardySoft.UI.BatchImageProcessor.Model;
using HardySoft.UI.BatchImageProcessor.Model.Exif;
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

		public ImageProcessorEngine(ProjectSetting ps, uint threadNumber, string dateTimeStringFormat, AutoResetEvent[] events, List<ExifContainerItem> exifContainer) {
			Trace.WriteLine("Number of Thread " + threadNumber);
			this.ps = ps;
			this.threadNumber = threadNumber;

			// add all selected images to job queue.
			this.jobQueue = new Queue<JobItem>();
			this.events = events;
			this.threads = new Thread[this.threadNumber];
			this.exifContainer = exifContainer;
			this.dateTimeStringFormat = dateTimeStringFormat;

			uint index = 0;

			List<PhotoItem> photoList = new List<PhotoItem>();
			if (ps.RenamingSetting.EnableBatchRename) {
				// TODO improve this feature under large volume, it could take long time to get it sorted first.
				// do sort of photo items first if batch rename is enabled.
				if (ps.RenamingSetting.SortOption == OutputFileSortOption.ByDateTimeTaken) {
					var pl = (from p in ps.Photos
							  orderby (new ExifMetadata(new Uri(p.PhotoPath))).DateImageTaken
							  select p);
					photoList = pl.ToList();
				} else if (ps.RenamingSetting.SortOption == OutputFileSortOption.ByOriginalFileName) {
					photoList = (from p in ps.Photos orderby p.PhotoPath select p).ToList();
				} else {
					throw new InvalidOperationException("Sort method is not supported.");
				}
			} else {
				photoList = ps.Photos.ToList();
			}

			foreach (PhotoItem item in photoList) {
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
				.RegisterType<IProcess, OilPaint>("OilPaintEffect", new PerThreadLifetimeManager())
				.RegisterType<IProcess, PencilSketch>("PencilSketchEffect", new PerThreadLifetimeManager())
				.RegisterType<IProcess, Relief>("ReliefEffect", new PerThreadLifetimeManager())
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

			try {
				if (!Directory.Exists(ps.OutputDirectory)) {
					// if the directory does not exist, create first
					Directory.CreateDirectory(ps.OutputDirectory);
				}
			} catch (Exception ex) {
				Trace.TraceError(ex.ToString());
				return;
			}

			for (int i = 0; i < this.threadNumber; i++) {
				threads[i] = new Thread(new ParameterizedThreadStart(ProcessImage));
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

		private void ProcessImage(object threadIndex) {
			int index = (int)threadIndex;
			Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " is created to process indexed item [" + index + "] at " + DateTime.Now + ".");
			Trace.WriteLine("Current Thread " + Thread.CurrentThread.ManagedThreadId + " Culture " + Thread.CurrentThread.CurrentCulture.ToString() + " during processing.");

			string imagePath = string.Empty;
			uint imageIndex = 0;

			lock (syncRoot) {
				if (jobQueue.Count > 0) {
					JobItem item = jobQueue.Dequeue();
					imagePath = item.FileName;
					imageIndex = item.Index;
					Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " is handling " + imagePath);
				} else {
					// nothing more to process, signal
#if DEBUG
					Debug.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " is set because no more image to process at " + DateTime.Now + ".");
#endif
					events[index].Set();
					return;
				}
			}

			if (stopFlag) {
				// stop requested, signal
#if DEBUG
				Debug.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " is set because stop requested.");
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
						exif = new ExifMetadata(new Uri(imagePath));
						Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " obtained EXIF for " + imagePath + " at " + DateTime.Now + ".");
					}

					// this will lock image until entire application quits: normalImage = Image.FromFile(imagePath);
					// following code won't lock image.
					using (Stream stream = File.OpenRead(imagePath)) {
						normalImage = Image.FromStream(stream);
						Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " opened " + imagePath + " at " + DateTime.Now + ".");
					}

					ImageFormat format = getImageFormat(imagePath);

					IProcess process;

					// thumbnail operation
					if (ps.ThumbnailSetting.GenerateThumbnail && ps.ThumbnailSetting.ThumbnailSize > 0) {
						process = container.Resolve<IProcess>("ThumbImage");
						thumbImage = process.ProcessImage(normalImage, this.ps);
						Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " processed thumbnail for " + imagePath + " at " + DateTime.Now + ".");
					}

					// shrink image operation
					if (ps.ShrinkImage && ps.ShrinkPixelTo > 0) {
						process = container.Resolve<IProcess>("ShrinkImage");
						normalImage = process.ProcessImage(normalImage, this.ps);
						Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " shrank " + imagePath + " at " + DateTime.Now + ".");
					}

					// image process effect
					if (ps.ProcessType != ImageProcessType.None) {
						switch (ps.ProcessType) {
							case ImageProcessType.GrayScale:
								process = container.Resolve<IProcess>("GrayscaleEffect");
								normalImage = process.ProcessImage(normalImage, null);
								Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " applied GrayscaleEffect for " + imagePath + " at " + DateTime.Now + ".");
								break;
							case ImageProcessType.NagativeImage:
								process = container.Resolve<IProcess>("NegativeEffect");
								normalImage = process.ProcessImage(normalImage, null);
								Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " applied NegativeEffect for " + imagePath + " at " + DateTime.Now + ".");
								break;
							case ImageProcessType.OilPaint:
								process = container.Resolve<IProcess>("OilPaintEffect");
								normalImage = process.ProcessImage(normalImage, null);
								Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " applied OilPaintEffect for " + imagePath + " at " + DateTime.Now + ".");
								break;
							case ImageProcessType.PencilSketch:
								process = container.Resolve<IProcess>("PencilSketchEffect");
								normalImage = process.ProcessImage(normalImage, null);
								Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " applied PencilSketchEffect for " + imagePath + " at " + DateTime.Now + ".");
								break;
							case ImageProcessType.Relief:
								process = container.Resolve<IProcess>("ReliefEffect");
								normalImage = process.ProcessImage(normalImage, null);
								Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " applied ReliefEffect for " + imagePath + " at " + DateTime.Now + ".");
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
									Debug.WriteLine("Current Thread "
										+ Thread.CurrentThread.ManagedThreadId + " Culture "
										+ Thread.CurrentThread.CurrentCulture.ToString()
										+ " before ApplyWatermarkText.");

									Debug.WriteLine("Current Thread: "
										+ Thread.CurrentThread.ManagedThreadId + ";"
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

						Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " applied watermark(s) for " + imagePath + " at " + DateTime.Now + ".");
					}

					// border operation
					if (ps.BorderSetting.BorderWidth > 0) {
						process = container.Resolve<IProcess>("AddBorder");
						normalImage = process.ProcessImage(normalImage, this.ps);
						Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " added border for " + imagePath + " at " + DateTime.Now + ".");
					}

					// drop shadow operation
					if (ps.DropShadowSetting.ShadowDepth > 0) {
						process = container.Resolve<IProcess>("DropShadow");
						normalImage = process.ProcessImage(normalImage, this.ps);
						Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " added dropshadow for " + imagePath + " at " + DateTime.Now + ".");
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
				ProcessImage(threadIndex);
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