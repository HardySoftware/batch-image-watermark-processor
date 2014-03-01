using System;

namespace HardySoft.UI.BatchImageProcessor.Model.Exif {
	public class DisplayAttribute : Attribute {
		protected string displayName;

		public string DisplayName {
			get {
				return displayName;
			}
		}

		public DisplayAttribute(string displayName)
			: base() {
			this.displayName = displayName;
		}
	}
}
