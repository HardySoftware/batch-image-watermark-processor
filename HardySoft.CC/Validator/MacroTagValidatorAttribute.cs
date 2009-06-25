using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HardySoft.CC.Validators {
	public class MacroTagValidatorAttribute : ValidatorAttribute {
		private string[] validMacros;

		public MacroTagValidatorAttribute(string[] validMacros) {
			this.validMacros = validMacros;
		}

		protected override Validator DoCreateValidator(Type targetType) {
			return new MacroTagValidator(validMacros);
		}
	}
}
