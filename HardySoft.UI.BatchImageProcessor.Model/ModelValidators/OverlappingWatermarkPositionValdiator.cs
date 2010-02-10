using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Drawing;
using System.Collections.ObjectModel;

namespace HardySoft.UI.BatchImageProcessor.Model.ModelValidators {
	public class OverlappingWatermarkPositionValdiator : Validator<ObservableCollection<WatermarkBase>> {
		public OverlappingWatermarkPositionValdiator(string messageTemplate)
			: this(messageTemplate, null) {
		}

		public OverlappingWatermarkPositionValdiator(string messageTemplate, string tag)
			: base(messageTemplate, tag) {
		}

		protected override string DefaultMessageTemplate {
			get {
				//return "The value {0} is not divisible by {1}";
				return string.Empty;
			}
		}

		protected override void DoValidate(ObservableCollection<WatermarkBase> objectToValidate,
			object currentTarget, string key, ValidationResults validationResults) {
			if (objectToValidate != null && objectToValidate.Count > 1) {
				IEnumerable<IGrouping<ContentAlignment, int>> overlapQuery = objectToValidate.GroupBy(w => w.WatermarkPosition,
					w => objectToValidate.IndexOf(w)).Where(g => g.Count() > 1);

				foreach (IGrouping<ContentAlignment, int> group in overlapQuery) {
#if DEBUG
					System.Diagnostics.Debug.WriteLine("position " + group.Key + " is overlapping.");

					foreach (int index in group) {
						System.Diagnostics.Debug.WriteLine("    object index " + index + ".");
					}
#endif
					string message = string.Format(this.MessageTemplate, group.Key);
					this.LogValidationResult(validationResults, message, currentTarget, key);
				}
			}
		}
	}
}
