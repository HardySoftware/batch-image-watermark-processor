using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace HardySoft.UI.BatchImageProcessor.Model {
	[Serializable]
	public class PhotoItem : INotifyPropertyChanged {
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		private void notifiy(string propertyName) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public PhotoItem() {
			this.selected = true;
			this.processed = false;
		}

		private bool selected;
		public bool Selected {
			get {
				return selected;
			}
			set {
				if (this.selected != value) {
					selected = value;
					notifiy("Selected");
				}
			}
		}

		private string photoPath;
		public string PhotoPath {
			get {
				return photoPath;
			}
			set {
				if (File.Exists(value) && string.Compare(this.photoPath, value, true) != 0) {
					photoPath = value;
					notifiy("PhotoPath");
				}
			}
		}

		[NonSerialized]
		private bool processed;
		public bool Processed {
			get { return processed; }
			set { processed = value; }
		}
	}
}
