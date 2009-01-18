using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HardySoft.UI.BatchImageProcessor.Model {
	[Serializable]
	public class PhotoItem {
		public PhotoItem() {
			this.selected = true;
		}

		private bool selected;
		public bool Selected {
			get {
				return selected;
			}
			set {
				selected = value;
			}
		}

		private string photoPath;
		public string PhotoPath {
			get {
				return photoPath;
			}
			set {
				if (File.Exists(value)) {
					photoPath = value;
				}
			}
		}
	}
}
