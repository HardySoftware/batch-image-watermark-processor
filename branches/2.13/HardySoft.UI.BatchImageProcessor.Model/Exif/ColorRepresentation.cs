using System.ComponentModel;

namespace HardySoft.UI.BatchImageProcessor.Model.Exif {
	public enum ColorRepresentation {
		sRGB,

		[Description(@"Enum_Uncalibrated")]
		Uncalibrated
	}
}
