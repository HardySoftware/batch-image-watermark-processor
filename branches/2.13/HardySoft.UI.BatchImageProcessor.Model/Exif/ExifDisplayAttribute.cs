using System;

namespace HardySoft.UI.BatchImageProcessor.Model.Exif {
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class ExifDisplayAttribute : DisplayAttribute {
		private string valueFormat;

		public ExifDisplayAttribute(string displayName)
			: base(displayName) {
			this.valueFormat = "";
		}

		public ExifDisplayAttribute(string displayName, string valueFormat)
			: base(displayName) {
			this.valueFormat = valueFormat;
		}

		public string ValueFormat {
			get {
				return this.valueFormat;
			}
		}
	}
}
