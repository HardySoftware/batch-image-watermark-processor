using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;

using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HardySoft.UI.BatchImageProcessor.Model {
	[Serializable]
	public class ProjectSetting : INotifyPropertyChanged {
		// TODO check to see if more file formats are supported
		private string[] supportedImageFormat = new string[] {
			"*.jpg",
			"*.jpeg",
			"*.bmp"
		};

		//public event PropertyChangedEventHandler PropertyChanged;
		[NonSerialized]
		private PropertyChangedEventHandler propertyChanged;

		public event PropertyChangedEventHandler PropertyChanged {
			add {
				propertyChanged += value;
			}
			remove {
				propertyChanged -= value;
			}
		}
		
		private void notify(string propName) {
			/*if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}*/

			if (propertyChanged != null) {
				propertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}

		public ProjectSetting() {
			Initialize();
		}

		public void Initialize() {
			this.isDirty = false;
			this.projectCreated = false;

			this.sourceDirectory = string.Empty;
			this.outputDirectory = string.Empty;

			this.photos = new ObservableCollection<PhotoItem>();
			this.processType = ImageProcessType.None;
			this.shrinkLongSidePixelTo = 800;
			this.jpgCompressionRatio = 100;

			this.watermark = new Watermark();
			this.watermark.PropertyChanged += new PropertyChangedEventHandler(watermark_PropertyChanged);

			this.dropShadowSetting = new DropShadow();
			this.dropShadowSetting.PropertyChanged += new PropertyChangedEventHandler(dropShadowSetting_PropertyChanged);

			this.borderSetting = new ImageBorder();
			this.borderSetting.PropertyChanged += new PropertyChangedEventHandler(borderSetting_PropertyChanged);

			this.thumbnailSetting = new Thumbnail();
			this.thumbnailSetting.PropertyChanged += new PropertyChangedEventHandler(thumbnailSetting_PropertyChanged);

			this.renamingSetting = new BatchRename();
			this.renamingSetting.PropertyChanged += new PropertyChangedEventHandler(renamingSetting_PropertyChanged);
		}

		void renamingSetting_PropertyChanged(object sender, PropertyChangedEventArgs e) {
			this.isDirty = true;
		}

		void thumbnailSetting_PropertyChanged(object sender, PropertyChangedEventArgs e) {
			this.isDirty = true;
		}

		void borderSetting_PropertyChanged(object sender, PropertyChangedEventArgs e) {
			this.isDirty = true;
		}

		void dropShadowSetting_PropertyChanged(object sender, PropertyChangedEventArgs e) {
			this.isDirty = true;
		}

		void watermark_PropertyChanged(object sender, PropertyChangedEventArgs e) {
			this.isDirty = true;
		}

		/// <summary>
		/// Recreate entire project.
		/// </summary>
		public void NewProject() {
			Initialize();

			this.projectCreated = true;

#if DEBUG
			this.SourceDirectory = @"C:\Temp\1";
			this.outputDirectory = @"C:\Temp\2";
#endif
		}

		/// <summary>
		/// After project is saved, reset some flags of the project.
		/// </summary>
		public void SaveProject() {
			this.isDirty = false;
		}

		/// <summary>
		/// After project is opened, reset some flags of the project.
		/// </summary>
		public void OpenProject() {
			this.isDirty = false;
			this.projectCreated = true;
		}

		private bool isDirty;
		public bool IsDirty {
			get {
				return isDirty;
			}
		}

		private bool projectCreated;
		public bool ProjectCreated {
			get {
				return projectCreated;
			}
			set {
				projectCreated = value;
			}
		}

		private string sourceDirectory;
		[StringLengthValidator(3, 500, MessageTemplate = "ReqSouceDirectory")]
		public string SourceDirectory {
			get {
				return sourceDirectory;
			}
			set {
				if (string.Compare(sourceDirectory, value, true) != 0) {
					isDirty = true;
					
					photos.Clear();

					// get photoInstance list with supported file format
					for (int i = 0; i < supportedImageFormat.Length; i++) {
						string[] photoFiles = Directory.GetFiles(value, supportedImageFormat[i]);

						for (int j = 0; j < photoFiles.Length; j++) {
							photos.Add(new PhotoItem()
							{
								PhotoPath = photoFiles[j]
							});
						}
					}

					notify("SourceDirectory");
				}

				sourceDirectory = value;
			}
		}

		private string outputDirectory;
		[StringLengthValidator(3, 500, MessageTemplate = "ReqDestDirectory")]
		public string OutputDirectory {
			get {
				return outputDirectory;
			}
			set {
				if (string.Compare(outputDirectory, value, true) != 0) {
					isDirty = true;
					notify("OutputDirectory");
				}

				outputDirectory = value;
			}
		}

		private ObservableCollection<PhotoItem> photos;
		public ObservableCollection<PhotoItem> Photos {
			get {
				return photos;
			}
			set {
				photos = value;
			}
		}

		private Watermark watermark;
		public Watermark Watermark {
			get {
				return watermark;
			}
			/*set {
				this.watermark = value;
				notify("Watermark");
			}*/
		}

		private ImageProcessType processType;
		public ImageProcessType ProcessType {
			get {
				return this.processType;
			}
			set {
				if (this.processType != value) {
					this.processType = value;
					this.isDirty = true;
					notify("ProcessType");
				}
			}
		}

		// TODO whenever write EXIF is ready, enable the checkbox on UI.
		private bool keepExif;
		public bool KeepExif {
			get {
				return keepExif;
			}
			set {
				if (keepExif != value) {
					keepExif = value;
					this.isDirty = true;
					notify("KeepExif");
				}
			}
		}

		private int jpgCompressionRatio;
		[RangeValidator(0, RangeBoundaryType.Inclusive, 100, RangeBoundaryType.Inclusive,
			MessageTemplate = "ValJpgCompressionRatio")]
		public int JpgCompressionRatio {
			get {
				return jpgCompressionRatio;
			}
			set {
				if (this.jpgCompressionRatio != value) {
					jpgCompressionRatio = value;
					notify("JpgCompressionRatio");
				}
			}
		}

		private DropShadow dropShadowSetting;
		public DropShadow DropShadowSetting {
			get {
				return dropShadowSetting;
			}
			/*set {
				dropShadowSetting = value;
				notify("DropShadowSetting");
			}*/
		}

		private ImageBorder borderSetting;
		public ImageBorder BorderSetting {
			get {
				return borderSetting;
			}
		}

		private bool shrinkImage;
		public bool ShrinkImage {
			get {
				return shrinkImage;
			}
			set {
				if (this.shrinkImage != value) {
					shrinkImage = value;
					this.isDirty = true;
					notify("ShrinkImage");
				}
			}
		}

		private uint shrinkLongSidePixelTo;
		public uint ShrinkLongSidePixelTo {
			get {
				return shrinkLongSidePixelTo;
			}
			set {
				if (this.shrinkLongSidePixelTo != value) {
					shrinkLongSidePixelTo = value;
					this.isDirty = true;
					notify("ShrinkLongSidePixelTo");
				}
			}
		}

		private Thumbnail thumbnailSetting;
		public Thumbnail ThumbnailSetting {
			get {
				return thumbnailSetting;
			}
		}

		private BatchRename renamingSetting;
		public BatchRename RenamingSetting {
			get {
				return renamingSetting;
			}
		}
	}
}