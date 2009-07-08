using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Collections.Generic;
using System.Linq;

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

			string watermarkText = objectToValidate.ToString();

			// parse all tags in text
			List<string> tags = getAllTags(watermarkText);
		}

		private List<string> getAllTags(string input) {
			Stack<char> stack = new Stack<char>();
			List<string> tags = new List<string>();

			bool prefixFound = false, suffixFound = false;
			for (int i = 0; i < input.Length; i++) {

				if (i < input.Length - 1) {
					string tagDelimiter = input.Substring(i, 2);

					if (string.Compare(tagDelimiter, "[[", false) == 0) {
						// found a opening tag
						prefixFound = true;
						suffixFound = false;
					}

					if (string.Compare(tagDelimiter, "]]", false) == 0) {
						// found a closing tag
						suffixFound = true;
					}
				}

				if (prefixFound && !suffixFound) {
					stack.Push(input[i]);
				}

				if (prefixFound && suffixFound) {
					// prepare to find next tag
					prefixFound = false;
					suffixFound = false;

					string tag = new string(stack.Reverse<char>().ToArray());
					// remove tag prefix
					tag = tag.Substring(2).Trim();
					tags.Add(tag);

					stack.Clear();
				}
			}

			return tags;
		}

		protected override string DefaultMessageTemplate {
			get {
				//return "The value {0} is not divisible by {1}";
				return string.Empty;
			}
		}
	}
}
