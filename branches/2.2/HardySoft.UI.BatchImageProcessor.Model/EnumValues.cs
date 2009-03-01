using System.ComponentModel;

namespace HardySoft.UI.BatchImageProcessor.Model {
	public enum DropShadowLocation {
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}

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

	public enum WatermarkPositions {
		[Description(@"TopLeft")]
		TopLeft,

		[Description(@"TopCenter")]
		TopCenter,

		[Description(@"TopRight")]
		TopRight,

		[Description(@"MiddleLeft")]
		MiddleLeft,

		[Description(@"MiddleCenter")]
		MiddleCenter,

		[Description(@"MiddleRight")]
		MiddleRight,

		[Description(@"BottomLeft")]
		BottomLeft,

		[Description(@"BottomCenter")]
		BottomCenter,

		[Description(@"BottomRight")]
		BottomRight
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
