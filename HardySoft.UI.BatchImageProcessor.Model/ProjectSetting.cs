using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

using HardySoft.CC.Validators;

using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HardySoft.UI.BatchImageProcessor.Model {
	[Serializable]
	public class ProjectSetting : INotifyPropertyChanged {
		// TODO check to see if more image formats are supported
		private string[] supportedImageFormat = new string[] {
			"*.jpg",
			"*.jpeg",
			"*.bmp",
			"*.gif",
			"*.png"
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
			wireEvents();
		}

		public void Initialize() {
			this.isDirty = false;
			this.projectCreated = false;

			this.sourceDirectory = string.Empty;
			this.outputDirectory = string.Empty;

			this.photos = new ObservableCollection<PhotoItem>();
			this.processType = ImageProcessType.None;
			this.shrinkMode = ShrinkImageMode.LongSide;
			this.shrinkPixelTo = 800;
			this.jpgCompressionRatio = 75;

			this.watermark = new Watermark();
			this.dropShadowSetting = new DropShadow();
			this.borderSetting = new ImageBorder();
			this.thumbnailSetting = new Thumbnail();
			this.renamingSetting = new BatchRename();
		}

		private void wireEvents() {
			this.photos.CollectionChanged += new NotifyCollectionChangedEventHandler(photos_CollectionChanged);
			this.watermark.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);
			this.dropShadowSetting.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);
			this.borderSetting.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);
			this.thumbnailSetting.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);
			this.renamingSetting.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);
		}

		void photos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			this.isDirty = true;
			notify("Photos");
		}

		void subSetting_PropertyChanged(object sender, PropertyChangedEventArgs e) {
			this.isDirty = true;
			notify(e.PropertyName);
		}

		/// <summary>
		/// Recreate entire project.
		/// </summary>
		public void NewProject() {
			Initialize();
			wireEvents();

			this.projectCreated = true;

#if DEBUG
			//this.SourceDirectory = @"C:\Temp\1";
			//this.outputDirectory = @"C:\Temp\2";
#endif
			this.isDirty = false;
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
			// traverse through image files in the project to make sure they are still available.
			foreach (PhotoItem photo in photos) {
				if (!File.Exists(photo.PhotoPath)) {
					// TODO think about how to handle, remove from list or just uncheck
					photo.Selected = false;
				}
			}
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
		[StringLengthValidator(3, 500, MessageTemplate = "Validation_ReqSouceDirectory")]
		public string SourceDirectory {
			get {
				return sourceDirectory;
			}
			set {
				if (string.Compare(sourceDirectory, value, true) != 0) {
					this.isDirty = true;
					
					photos.Clear();

					// get photoInstance list with supported file format
					for (int i = 0; i < supportedImageFormat.Length; i++) {
						string[] photoFiles = Directory.GetFiles(value, supportedImageFormat[i]);

						for (int j = 0; j < photoFiles.Length; j++) {
							PhotoItem photoItem = new PhotoItem() {
								PhotoPath = photoFiles[j]
							};

							photoItem.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);

							photos.Add(photoItem);
						}
					}
					this.sourceDirectory = value;
					notify("SourceDirectory");
				}
			}
		}

		private string outputDirectory;
		[StringLengthValidator(3, 500, MessageTemplate = "Validation_ReqDestDirectory")]
		public string OutputDirectory {
			get {
				return outputDirectory;
			}
			set {
				if (string.Compare(outputDirectory, value, true) != 0) {
					this.outputDirectory = value;
					this.isDirty = true;
					notify("OutputDirectory");
				}
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
		[MacroTagValidator(null, MessageTemplate="", Tag="Warning")]
		public Watermark Watermark {
			get {
				return watermark;
			}
			set {
				// Xml Serialization requires a property to be public and can be set and read
				// do nothing
			}
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
					this.keepExif = value;
					this.isDirty = true;
					notify("KeepExif");
				}
			}
		}

		private long jpgCompressionRatio;
		[RangeValidator(0L, RangeBoundaryType.Inclusive, 100L, RangeBoundaryType.Inclusive,
			MessageTemplate = "Validation_JpgCompressionRatio")]
		public long JpgCompressionRatio {
			get {
				return jpgCompressionRatio;
			}
			set {
				if (this.jpgCompressionRatio != value) {
					this.jpgCompressionRatio = value;
					this.isDirty = true;
					notify("JpgCompressionRatio");
				}
			}
		}

		private DropShadow dropShadowSetting;
		[System.Xml.Serialization.XmlElement]
		public DropShadow DropShadowSetting {
			get {
				return dropShadowSetting;
			}
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
					this.shrinkImage = value;
					this.isDirty = true;
					notify("ShrinkImage");
				}
			}
		}

		private uint shrinkPixelTo;
		public uint ShrinkPixelTo {
			get {
				return shrinkPixelTo;
			}
			set {
				if (this.shrinkPixelTo != value) {
					this.shrinkPixelTo = value;
					this.isDirty = true;
					notify("ShrinkPixelTo");
				}
			}
		}

		private ShrinkImageMode shrinkMode;
		public ShrinkImageMode ShrinkMode {
			get {
				return shrinkMode;
			}
			set {
				if (this.shrinkMode != value) {
					this.shrinkMode = value;
					this.isDirty = true;
					notify("ShrinkMode");
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

		/// <summary>
		/// When deserialize from save file, all the event handlers will be lost.
		/// This solution is from http://www.codeproject.com/KB/cs/FixingBindingListDeserial.aspx
		/// </summary>
		/// <param name="context"></param>
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context) {
			wireEvents();

			foreach (PhotoItem photoItem in this.photos) {
				photoItem.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);
			}
		}
	}
}