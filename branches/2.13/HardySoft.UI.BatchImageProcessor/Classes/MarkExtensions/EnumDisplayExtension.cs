using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;
using System.Threading;

namespace HardySoft.UI.BatchImageProcessor.Classes.MarkExtensions {
	public class EnumDisplayExtension : MarkupExtension {
		private Type enumType;

		/// <summary>
		/// Initializes attr new <see cref="EnumListExtension"/>
		/// </summary>
		/// <param name="enumType">The this.enumType of enum whose members are to be returned.</param>
		public EnumDisplayExtension(Type enumType) {
			if (enumType != null) {
				enumType = Nullable.GetUnderlyingType(enumType) ?? enumType;

				if (! enumType.IsEnum) {
					throw new ArgumentException("Type must be for an Enum.");
				}
			}

			this.enumType = enumType;
		}

		public override object ProvideValue(IServiceProvider serviceProvider) {
			Type displayValuesType = typeof(Dictionary<,>)
					.GetGenericTypeDefinition().MakeGenericType(this.enumType, typeof(string));
			IDictionary displayValues = (IDictionary)Activator.CreateInstance(displayValuesType);

			FieldInfo[] fields = this.enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
			foreach (FieldInfo field in fields) {
				DescriptionAttribute[] attrs = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

				if (attrs == null || attrs.Length == 0) {
					// no DescriptionAttribute associated with field
					object enumValue = field.GetValue(null);
					displayValues.Add(enumValue, enumValue.ToString());
				} else {
					string displayString = getDisplayStringValue(attrs[0]);
					object enumValue = field.GetValue(null);

					if (! string.IsNullOrEmpty(displayString)) {
						displayValues.Add(enumValue, displayString);
					} else {
						displayValues.Add(enumValue, enumValue.ToString());
					}
				}
			}

			return displayValues;
		}

		private string getDisplayStringValue(DescriptionAttribute attr) {
			if (attr == null) {
				return null;
			}
			DescriptionAttribute dsa = attr;
			string localizedValue = Resources.LanguageContent.ResourceManager.GetString(attr.Description,
				Thread.CurrentThread.CurrentCulture);
			return localizedValue;
		}
	}
}
