using System.ComponentModel;

namespace HardySoft.UI.BatchImageProcessor.Model.Exif {
	/// <summary>
	/// A.k.a Exposure Program
	/// </summary>
	/// <remarks>
	/// Reference http://msdn.microsoft.com/en-us/library/bb760434(VS.85).aspx
	/// </remarks>
	public enum ExposureMode : ushort {
		[Description(@"Enum_Manual")]
		Manual = 1,

		[Description(@"Enum_NormalProgram")]
		NormalProgram = 2,

		[Description(@"Enum_AperturePriority")]
		AperturePriority = 3,

		[Description(@"Enum_ShutterPriority")]
		ShutterPriority = 4,

		/// <summary>
		/// biased toward depth of field
		/// </summary>
		[Description(@"Enum_Creative")]
		CreativeMode = 5,

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// Reference biased toward shutter speed
		/// </remarks>
		[Description(@"Enum_ActionMode")]
		ActionMode = 6,

		[Description(@"Enum_PortraitMode")]
		PortraitMode = 7,

		[Description(@"Enum_LandscapeMode")]
		LandscapeMode = 8,

		[Description(@"Enum_Unknown")]
		Unknown = 0
	}
}
