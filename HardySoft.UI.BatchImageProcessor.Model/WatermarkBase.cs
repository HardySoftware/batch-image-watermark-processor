using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HardySoft.UI.BatchImageProcessor.Model {
	[Serializable]
	public abstract class WatermarkBase : INotifyPropertyChanged {
		protected ContentAlignment watermarkPosition;
		public ContentAlignment WatermarkPosition {
			get {
				return this.watermarkPosition;
			}
			set {
				if (this.watermarkPosition != value) {
					watermarkPosition = value;
					notify("WatermarkPosition");
				}
			}
		}

		protected int watermarkRotateAngle;
		[RangeValidator(0, RangeBoundaryType.Inclusive, 360, RangeBoundaryType.Inclusive,
			MessageTemplate = "Validation_RotationAngle")]
		public int WatermarkRotateAngle {
			get {
				return watermarkRotateAngle;
			}
			set {
				if (this.watermarkRotateAngle != value) {
					this.watermarkRotateAngle = value;
					notify("WatermarkeRotateAngle");
				}
			}
		}

		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		protected void notify(string propName) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}

		/// <summary>
		/// The space of text box or image box between edge of main image.
		/// </summary>
		public int Padding {
			get {
				// TODO add UI selection to let user input distance to edge as padding
				return 10;
			}
		}
	}
}
