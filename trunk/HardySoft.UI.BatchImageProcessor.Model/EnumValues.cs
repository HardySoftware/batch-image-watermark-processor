using System.ComponentModel;

namespace HardySoft.UI.BatchImageProcessor.Model {
	public enum OutputFileNameCase {
		[Description(@"Enum_None")]
		None,

		[Description(@"Enum_UpperCase")]
		UpperCase,

		[Description(@"Enum_LowerCase")]
		LowerCase
	}

	public enum ImageProcessType {
		None,
		GrayScale,
		NagativeImage
	}

	public enum Skin {
		DarkCool
	}

	public enum ShrinkImageMode {
		[Description(@"Enum_LongSide")]
		LongSide,

		[Description(@"Enum_Width")]
		Width,

		[Description(@"Enum_Height")]
		Height
	}
}
