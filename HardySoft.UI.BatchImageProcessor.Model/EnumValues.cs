using System.ComponentModel;
using System;

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
		[Description(@"Enum_None")]
		None,
		[Description(@"Enum_GrayScale")]
		GrayScale,
		[Description(@"Enum_NagativeImage")]
		NagativeImage,
		[Description(@"Enum_OilPaint")]
		OilPaint,
		[Description(@"Enum_PencilSketch")]
		PencilSketch,
		[Description(@"Enum_Relief")]
		Relief
	}

	public enum Skin {
		[Description(@"Enum_Skin_DarkCool")]
		DarkCool,
		[Description(@"Enum_Skin_ShinnyBlue")]
		ShinnyBlue
	}

	public enum ShrinkImageMode {
		[Description(@"Enum_LongSide")]
		LongSide,

		[Description(@"Enum_Width")]
		Width,

		[Description(@"Enum_Height")]
		Height
	}

	public enum OutputFileSortOption {
		[Description(@"Enum_ByDateTimeTaken")]
		ByDateTimeTaken,

		[Description(@"Enum_ByOriginalFileName")]
		ByOriginalFileName
	}
}
