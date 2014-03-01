using System.ComponentModel;

namespace HardySoft.UI.BatchImageProcessor.Model.Exif {
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Reference http://msdn.microsoft.com/en-us/library/bb760501(VS.85).aspx
	/// </remarks>
	public enum MeteringMode : int {
		[Description(@"Enum_Average")]
		Average = 1,

		[Description(@"Enum_Center")]
		Centre = 2,

		[Description(@"Enum_Spot")]
		Spot = 3,

		[Description(@"Enum_MultiSpot")]
		MultiSpot = 4,

		[Description(@"Enum_MultiSegment")]
		MultiSegment = 5,

		[Description(@"Enum_Partial")]
		Partial = 6,

		[Description(@"Enum_Unknown")]
		Unknown = 0
	}
}
