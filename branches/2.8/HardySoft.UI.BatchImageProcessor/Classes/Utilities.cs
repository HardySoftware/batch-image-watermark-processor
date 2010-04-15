using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

using HardySoft.UI.BatchImageProcessor.Model;
using HardySoft.UI.BatchImageProcessor.Resources;

namespace HardySoft.UI.BatchImageProcessor.Classes {
	class Utilities {
		/// <summary>
		/// Scans value's original data type's DescriptionAttribute to get resource key
		/// and use it to get displayed value from resource.
		/// </summary>
		/// <param name="objectValue">
		/// Original object value.
		/// </param>
		/// <returns>
		/// If DescriptionAttribute is found and the key is valid, it returns display value
		/// from resource; otherwise original value is returned.
		/// </returns>
		/// <remarks>
		/// This function is especially useful to display user friendly Enum values.
		/// </remarks>
		public static string GetEnumItemDisplayValue(object objectValue) {
			FieldInfo fi = objectValue.GetType().GetField(objectValue.ToString());

			// look for Enum member localizedValue(resource key) attribute
			DescriptionAttribute[] attributes =
				(DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes.Length > 0) {
				string description = attributes[0].Description;

				if (!string.IsNullOrEmpty(description)) {
					description = Resources.LanguageContent.ResourceManager.GetString(description,
						Thread.CurrentThread.CurrentCulture);
					if (!string.IsNullOrEmpty(description)) {
						return description;
					} else {
						return objectValue.ToString();
					}
				} else {
					return objectValue.ToString();
				}
			} else {
				return objectValue.ToString();
			}
		}

		public static List<ExifContainerItem> GetExifContainer() {
			return GetExifContainer(false);
		}

		public static List<ExifContainerItem> GetExifContainer(bool includeEnumValueTranslation) {
			List<ExifContainerItem> container = new List<ExifContainerItem>();

			Type t = typeof(ExifMetadata);
			PropertyInfo[] pi = t.GetProperties();

			for (int i = 0; i < pi.Length; i++) {
				ExifContainerItem containerItem = new ExifContainerItem();

				object[] attr = pi[i].GetCustomAttributes(true);

				for (int j = 0; j < attr.Length; j++) {
					if (attr[j] is ExifDisplayAttribute) {
						ExifDisplayAttribute exifAttri = (ExifDisplayAttribute)attr[j];

						string displayName = Resources.LanguageContent.ResourceManager.GetString(exifAttri.DisplayName,
							Thread.CurrentThread.CurrentCulture);
						containerItem.DisplayLabel = displayName;
						containerItem.ValueFormat = Resources.LanguageContent.ResourceManager.GetString(exifAttri.ValueFormat,
							Thread.CurrentThread.CurrentCulture);
						containerItem.Property = pi[i];
						containerItem.ExifTag = pi[i].Name;

						if (includeEnumValueTranslation) {
							if (pi[i].PropertyType.IsEnum) {
								containerItem.EnumValueTranslation = new Dictionary<string, string>();

								Array enumValues = Enum.GetValues(pi[i].PropertyType);

								for (int k = 0; k < enumValues.Length; k++) {
									// TODO think about how to handle same enum value in different enum with different display.
									if (! containerItem.EnumValueTranslation.ContainsKey(enumValues.GetValue(k).ToString())) {
										containerItem.EnumValueTranslation.Add(enumValues.GetValue(k).ToString(),
											GetEnumItemDisplayValue(enumValues.GetValue(k)));
									}
								}
							}
						}

						container.Add(containerItem);
					}
				}
			}

			return container;
		}

		/// <summary>
		/// Due to the restriction of this project that UI resource sits outside of model project,
		/// the localized validation message returns as assembly resource key. But when wen need to embed
		/// some validation value from result there is no default property we can use, so the idea
		/// is to make returned message like "resource_key||value1||value2".
		/// </summary>
		/// <param name="input">"resource_key||value1||value2"</param>
		/// <returns></returns>
		public static string ParseResource(string input) {
			string[] s = Regex.Split(input, Regex.Escape("||"));

			if (s != null && s.Length > 0) {
				// the first part is base resource key
				string resourceKey = s[0].Trim();

				string message = LanguageContent.ResourceManager.GetString(resourceKey,
							Thread.CurrentThread.CurrentCulture);

				if (string.IsNullOrEmpty(message)) {
					return input;
				}

				if (s.Length > 1) {
					// the rest parts are values used to merge into display text
					List<string> parameters = new List<string>();
					parameters.AddRange(s);
					parameters.RemoveAt(0);
					message = string.Format(message, parameters.ToArray());
				}

				return message;
			} else {
				return input;
			}
		}
	}
}
