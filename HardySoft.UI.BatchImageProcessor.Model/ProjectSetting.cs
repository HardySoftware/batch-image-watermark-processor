using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

using HardySoft.UI.BatchImageProcessor.Model.ModelValidators;

using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HardySoft.UI.BatchImageProcessor.Model
{
    [Serializable]
    public class ProjectSetting : INotifyPropertyChanged
    {
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

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                propertyChanged += value;
            }
            remove
            {
                propertyChanged -= value;
            }
        }

        private void Notify(string propName)
        {
            /*if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}*/

            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public ProjectSetting()
        {
            Initialize();
            WireEvents();
        }

        public void Initialize()
        {
            this.isDirty = false;
            this.projectCreated = false;

            this.sourceDirectory = string.Empty;
            this.outputDirectory = string.Empty;

            this.Photos = new ObservableCollection<PhotoItem>();
            this.watermarkCollection = new ObservableCollection<WatermarkBase>();
#if DEBUG
            //this.WatermarkCollection.Add(new WatermarkImage());
            //this.WatermarkCollection.Add(new WatermarkText());
#endif
            this.processType = ImageProcessType.None;
            this.shrinkMode = ShrinkImageMode.LongSide;
            this.shrinkPixelTo = 800;
            this.jpgCompressionRatio = 75;

            this.dropShadowSetting = new DropShadow();
            this.borderSetting = new ImageBorder();
            this.thumbnailSetting = new Thumbnail();
            this.renamingSetting = new BatchRename();
        }

        private void WireEvents()
        {
            this.Photos.CollectionChanged += new NotifyCollectionChangedEventHandler(Photos_CollectionChanged);
            this.watermarkCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(watermarkCollection_CollectionChanged);
            this.dropShadowSetting.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);
            this.borderSetting.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);
            this.thumbnailSetting.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);
            this.renamingSetting.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);
        }

        protected void watermarkCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.isDirty = true;
            Notify("WatermarkCollection");
        }

        void Photos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.isDirty = true;
            Notify("PhotoCollection");
        }

        void subSetting_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.isDirty = true;
            Notify(e.PropertyName);
        }

        /// <summary>
        /// Recreate entire project.
        /// </summary>
        public void NewProject()
        {
            Initialize();
            WireEvents();

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
        public void SaveProject()
        {
            this.isDirty = false;
        }

        /// <summary>
        /// After project is opened, reset some flags of the project.
        /// </summary>
        public void OpenProject()
        {
            // traverse through image files in the project to make sure they are still available.
            foreach (PhotoItem photo in this.Photos)
            {
                if (!File.Exists(photo.PhotoPath))
                {
                    // TODO think about how to handle, remove from list or just uncheck
                    photo.Selected = false;
                }
            }

            if (Directory.Exists(this.SourceDirectory))
            {
                // check if there are any new files in the same folder but not in project, list them but don't select them,
                // TODO in .net 4.0 and up, replace the code with Directory.EnumerateFiles to avoid loading all files first.
                var allNewPhotoFiles = Directory.GetFiles(this.SourceDirectory, "*.*")
                    .Where(s => this.supportedImageFormat.Contains("*" + (new FileInfo(s).Extension).ToLower()) && !this.Photos.Any(x => string.Compare(x.PhotoPath, s, true) == 0))
                    .ToArray();

                foreach (string newPhoto in allNewPhotoFiles)
                {
                    PhotoItem pi = new PhotoItem()
                    {
                        PhotoPath = newPhoto,
                        Selected = false,
                    };
                    pi.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);
                    this.Photos.Add(pi);
                }
            }

            this.isDirty = false;
            this.projectCreated = true;
        }

        private bool isDirty;
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }
        }

        private bool projectCreated;
        public bool ProjectCreated
        {
            get
            {
                return projectCreated;
            }
            set
            {
                projectCreated = value;
            }
        }

        private string sourceDirectory;
        [StringLengthValidator(3, 500, MessageTemplate = "Validation_ReqSouceDirectory")]
        public string SourceDirectory
        {
            get
            {
                return sourceDirectory;
            }
            set
            {
                if (string.Compare(sourceDirectory, value, true) == 0)
                {
                    return;
                }
                this.isDirty = true;

                Photos.Clear();

                // get photoInstance list with supported file format
                for (int i = 0; i < supportedImageFormat.Length; i++)
                {
                    string[] photoFiles = Directory.GetFiles(value, supportedImageFormat[i]);

                    for (int j = 0; j < photoFiles.Length; j++)
                    {
                        PhotoItem photoItem = new PhotoItem()
                        {
                            PhotoPath = photoFiles[j]
                        };

                        photoItem.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);

                        Photos.Add(photoItem);
                    }
                }
                this.sourceDirectory = value;
                Notify("SourceDirectory");
            }
        }

        private string outputDirectory;
        [StringLengthValidator(3, 500, MessageTemplate = "Validation_ReqDestDirectory")]
        [PropertyComparisonValidator("SourceDirectory", ComparisonOperator.NotEqual,
            MessageTemplate = "Validation_SourceDestFolderSame")]
        public string OutputDirectory
        {
            get
            {
                return outputDirectory;
            }
            set
            {
                if (string.Compare(outputDirectory, value, true) == 0)
                {
                    return;
                }
                this.outputDirectory = value;
                this.isDirty = true;
                Notify("OutputDirectory");
            }
        }

        public ObservableCollection<PhotoItem> Photos
        {
            get;
            set;
        }

        private ObservableCollection<WatermarkBase> watermarkCollection;

        [ObjectCollectionValidatorExt(typeof(WatermarkBase))]
        [OverlappingWatermarkPositionValdiator(MessageTemplate = "Validation_OverlapPosition||{0}", Tag = "Warning")]
        public ObservableCollection<WatermarkBase> WatermarkCollection
        {
            get
            {
                return this.watermarkCollection;
            }
            //set;
        }

        public void AddWatermark(WatermarkBase watermark)
        {
            watermark.PropertyChanged += new PropertyChangedEventHandler(watermark_PropertyChanged);
            this.watermarkCollection.Add(watermark);
        }

        protected void watermark_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.isDirty = true;
            Notify("Watermark");
        }

        /// <summary>
        /// remove watermark from list
        /// </summary>
        /// <param name="selectedIndex"></param>
        /// <returns>
        /// Returns true if there is at least one watermark remaining; otherwise false.
        /// </returns>
        public bool RemoveWatermark(int selectedIndex)
        {
            if (this.watermarkCollection != null
                && this.watermarkCollection.Count >= selectedIndex + 1)
            {

                WatermarkBase watermark = this.watermarkCollection[selectedIndex];
                watermark.PropertyChanged -= watermark_PropertyChanged;

                this.watermarkCollection.RemoveAt(selectedIndex);

                if (this.watermarkCollection.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private ImageProcessType processType;
        public ImageProcessType ProcessType
        {
            get
            {
                return this.processType;
            }
            set
            {
                if (this.processType != value)
                {
                    this.processType = value;
                    this.isDirty = true;
                    Notify("ProcessType");
                }
            }
        }

        private bool keepExif;
        public bool KeepExif
        {
            get
            {
                return keepExif;
            }
            set
            {
                if (keepExif != value)
                {
                    this.keepExif = value;
                    this.isDirty = true;
                    Notify("KeepExif");
                }
            }
        }

        private long jpgCompressionRatio;
        [RangeValidator(0L, RangeBoundaryType.Inclusive, 100L, RangeBoundaryType.Inclusive,
            MessageTemplate = "Validation_JpgCompressionRatio")]
        public long JpgCompressionRatio
        {
            get
            {
                return jpgCompressionRatio;
            }
            set
            {
                if (this.jpgCompressionRatio != value)
                {
                    this.jpgCompressionRatio = value;
                    this.isDirty = true;
                    Notify("JpgCompressionRatio");
                }
            }
        }

        private DropShadow dropShadowSetting;
        [System.Xml.Serialization.XmlElement]
        public DropShadow DropShadowSetting
        {
            get
            {
                return dropShadowSetting;
            }
        }

        private ImageBorder borderSetting;
        [ObjectValidator()]
        public ImageBorder BorderSetting
        {
            get
            {
                return borderSetting;
            }
        }

        private bool shrinkImage;
        public bool ShrinkImage
        {
            get
            {
                return shrinkImage;
            }
            set
            {
                if (this.shrinkImage != value)
                {
                    this.shrinkImage = value;
                    this.isDirty = true;
                    Notify("ShrinkImage");
                }
            }
        }

        private uint shrinkPixelTo;
        public uint ShrinkPixelTo
        {
            get
            {
                return shrinkPixelTo;
            }
            set
            {
                if (this.shrinkPixelTo != value)
                {
                    this.shrinkPixelTo = value;
                    this.isDirty = true;
                    Notify("ShrinkPixelTo");
                }
            }
        }

        private ShrinkImageMode shrinkMode;
        public ShrinkImageMode ShrinkMode
        {
            get
            {
                return shrinkMode;
            }
            set
            {
                if (this.shrinkMode != value)
                {
                    this.shrinkMode = value;
                    this.isDirty = true;
                    Notify("ShrinkMode");
                }
            }
        }

        private Thumbnail thumbnailSetting;
        public Thumbnail ThumbnailSetting
        {
            get
            {
                return thumbnailSetting;
            }
        }

        private BatchRename renamingSetting;
        public BatchRename RenamingSetting
        {
            get
            {
                return renamingSetting;
            }
        }

        /// <summary>
        /// When deserialize from save file, all the event handlers will be lost.
        /// This solution is from http://www.codeproject.com/KB/cs/FixingBindingListDeserial.aspx
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            WireEvents();

            foreach (PhotoItem photoItem in this.Photos)
            {
                photoItem.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);
            }

            foreach (WatermarkBase watermark in this.WatermarkCollection)
            {
                watermark.PropertyChanged += new PropertyChangedEventHandler(subSetting_PropertyChanged);
            }
        }
    }
}