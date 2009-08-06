using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace HardySoft.UI.BatchImageProcessor.Model {
	/// <summary>
	/// A class represents a single Exif display item.
	/// </summary>
	public class ExifContainerItem {
		/// <summary>
		/// Display/Localized Label of the Exif item.
		/// </summary>
		public string DisplayLabel {
			get;
			set;
		}

		/// <summary>
		/// A helper property used to help the applciation to use reflection to retrieve value
		/// from real object.
		/// </summary>
		public PropertyInfo Property {
			get;
			set;
		}

		/// <summary>
		/// The format string to format display value.
		/// </summary>
		public string ValueFormat {
			get;
			set;
		}

		/// <summary>
		/// Display value of Exif item.
		/// </summary>
		public string DisplayValue {
			get;
			set;
		}

		/// <summary>
		/// Exif tag value.
		/// </summary>
		public string ExifTag {
			get;
			set;
		}

		/// <summary>
		/// If the Exif item is an Enum value, get all the resource key to display value translation.
		/// </summary>
		public Dictionary<string, string> EnumValueTranslation {
			get;
			set;
		}
	}
}
