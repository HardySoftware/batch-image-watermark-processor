using System;
using System.Drawing;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HardySoft.UI.BatchImageProcessor.Model {
	[Serializable]
	public class WatermarkImage : WatermarkBase {
		public WatermarkImage() : base() {
			this.watermarkPosition = ContentAlignment.TopRight;
			this.watermarkImageOpacity = 1.0;
		}

		private string watermarkImageFile;
		public string WatermarkImageFile {
			get {
				return this.watermarkImageFile;
			}
			set {
				if (string.Compare(this.watermarkImageFile, value, true) != 0) {
					this.watermarkImageFile = value;
					notify("WatermarkImageFile");
				}
			}
		}

		private double watermarkImageOpacity;
		[RangeValidator(0.0, RangeBoundaryType.Inclusive, 1.0, RangeBoundaryType.Inclusive,
			MessageTemplate = "Validation_Opacity")]
		public double WatermarkImageOpacity {
			get {
				return watermarkImageOpacity;
			}
			set {
				if (this.watermarkImageOpacity != value) {
					this.watermarkImageOpacity = value;
					notify("WatermarkImageOpacity");
				}
			}
		}
	}
}
