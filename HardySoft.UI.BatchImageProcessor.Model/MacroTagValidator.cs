using System.Collections.Generic;
using System.Linq;

using HardySoft.CC;

using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace HardySoft.UI.BatchImageProcessor.Model {
	public class MacroTagValidator : Validator<string> {
		private string[] validMacros;

		public MacroTagValidator(string[] validMacros)
			: this(validMacros, null, null) {
		}

		public MacroTagValidator(string[] validMacros, string messageTemplate)
			: this(validMacros, messageTemplate, null) {
		}

		public MacroTagValidator(string[] validMacros, string messageTemplate, string tag)
			: base(messageTemplate, tag) {
			this.validMacros = validMacros;
		}

		protected override void DoValidate(string objectToValidate, object currentTarget,
			string key, ValidationResults validationResults) {
			/*if (objectToValidate % divisor != 0) {
				string message = string.Format(this.MessageTemplate, objectToValidate, divisor);
				this.LogValidationResult(validationResults, message, currentTarget, key);
			}*/

			if (! string.IsNullOrEmpty(objectToValidate)) {
				string watermarkText = objectToValidate.ToString();

				// parse all tags in text
				string[] tags = getAllTags(watermarkText);

				IEnumerable<string> invalidTags = tags.Except(this.validMacros);

				foreach (string invalidTag in invalidTags) {
					string message = string.Format(this.MessageTemplate, objectToValidate, invalidTag);
					this.LogValidationResult(validationResults, message, currentTarget, key);
				}
			}
		}

		private string[] getAllTags(string input) {
			return Parser.TagParser(input, "[[", "]]");
		}

		protected override string DefaultMessageTemplate {
			get {
				//return "The value {0} is not divisible by {1}";
				return string.Empty;
			}
		}
	}
}
