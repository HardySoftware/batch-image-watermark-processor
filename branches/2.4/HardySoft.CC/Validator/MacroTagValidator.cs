using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace HardySoft.CC.Validators {
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
		}

		protected override string DefaultMessageTemplate {
			get {
				//return "The value {0} is not divisible by {1}";
				return string.Empty;
			}
		}
	}
}
