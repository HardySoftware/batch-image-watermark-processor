using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace HardySoft.UI.BatchImageProcessor.Model {
	public enum DropShadowLocation {
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}

	public enum OutputFileNameCase {
		None,
		UpperCase,
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
		LongSide,
		Width,
		Height
	}
}
