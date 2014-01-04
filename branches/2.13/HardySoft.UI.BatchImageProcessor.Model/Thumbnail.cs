using System;
using System.ComponentModel;

namespace HardySoft.UI.BatchImageProcessor.Model {
	[Serializable]
	public class Thumbnail : INotifyPropertyChanged {
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

		public Thumbnail() {
			this.thumbnailFileNamePrefix = "thumb_";
			this.thumbnailFileNameSuffix = "_thumb";
			this.thumbnailSize = 100;
		}

		private bool generateThumbnail;
		public bool GenerateThumbnail {
			get {
				return generateThumbnail;
			}
			set {
				if (this.generateThumbnail != value) {
					generateThumbnail = value;
					notify("GenerateThumbnail");
				}
			}
		}

		private string thumbnailFileNamePrefix;
		public string ThumbnailFileNamePrefix {
			get {
				return thumbnailFileNamePrefix;
			}
			set {
				if (string.Compare(this.thumbnailFileNamePrefix, value, true) != 0) {
					thumbnailFileNamePrefix = value;
					notify("ThumbnailFileNamePrefix");
				}
			}
		}

		private string thumbnailFileNameSuffix;
		public string ThumbnailFileNameSuffix {
			get {
				return thumbnailFileNameSuffix;
			}
			set {
				if (string.Compare(this.thumbnailFileNameSuffix, value, true) != 0) {
					thumbnailFileNameSuffix = value;
					notify("ThumbnailFileNameSuffix");
				}
			}
		}

		private uint thumbnailSize;
		public uint ThumbnailSize {
			get {
				return thumbnailSize;
			}
			set {
				if (this.thumbnailSize != value) {
					thumbnailSize = value;
					notify("ThumbnailSize");
				}
			}
		}
	}
}