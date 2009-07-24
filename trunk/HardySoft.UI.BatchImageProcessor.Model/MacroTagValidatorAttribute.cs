using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HardySoft.UI.BatchImageProcessor.Model {
	public class MacroTagValidatorAttribute : ValidatorAttribute {
		private string[] validMacros;

		public MacroTagValidatorAttribute(Type macrosContainerType) {
			PropertyInfo[] pi = macrosContainerType.GetProperties();
			List<string> tags = new List<string>();

			for (int i = 0; i < pi.Length; i++) {
				object[] attr = pi[i].GetCustomAttributes(true);

				for (int j = 0; j < attr.Length; j++) {
					if (attr[j] is DisplayAttribute) {
						DisplayAttribute exifAttri = (DisplayAttribute)attr[j];

						string propertyName = pi[i].Name;
						tags.Add(propertyName);
					}
				}
			}

			this.validMacros = tags.ToArray();
		}

		protected override Validator DoCreateValidator(Type targetType) {
			return new MacroTagValidator(validMacros, MessageTemplate, Tag);
		}
	}
}
