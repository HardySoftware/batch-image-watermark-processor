using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace HardySoft.UI.BatchImageProcessor.Model.ModelValidators {
	public class OverlappingWatermarkPositionValdiatorAttribute : ValidatorAttribute {
		public OverlappingWatermarkPositionValdiatorAttribute() {
		}

		protected override Validator DoCreateValidator(Type targetType) {
			return new OverlappingWatermarkPositionValdiator(MessageTemplate, Tag);
		}
	}
}
