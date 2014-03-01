using System.ComponentModel;

namespace HardySoft.UI.BatchImageProcessor.Model.Exif {
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Reference http://msdn.microsoft.com/en-us/library/bb760445(VS.85).aspx
	/// </remarks>
	public enum FlashMode : ushort {
		[Description(@"Enum_FlashMode00")]
		FlashDidNotFire = 0,

		[Description(@"Enum_FlashMode01")]
		FlashFired = 1,

		[Description(@"Enum_FlashMode05")]
		StrobeReturnNotDetected = 5,

		[Description(@"Enum_FlashMode07")]
		StrobeReturnDetected = 7,

		[Description(@"Enum_FlashMode09")]
		CompulsoryFlashMode = 9,

		[Description(@"Enum_FlashMode13")]
		CompulsoryFlashModeReturnLightNotDetected = 13,

		[Description(@"Enum_FlashMode15")]
		CompulsoryFlashModeReturnLightDetected = 15,

		[Description(@"Enum_FlashMode16")]
		FlashNotFiredCompulsoryFlashMode = 16,

		[Description(@"Enum_FlashMode24")]
		NoFlashAuto = 24,

		[Description(@"Enum_FlashMode25")]
		FlashAuto = 25,

		[Description(@"Enum_FlashMode29")]
		FlashAutoNoStrobeReturn = 29,

		[Description(@"Enum_FlashMode31")]
		FlashAutoStrobeReturn = 31,

		[Description(@"Enum_FlashMode32")]
		NoFlashFunction = 32,

		[Description(@"Enum_FlashMode65")]
		FlashRedEye = 65,

		[Description(@"Enum_FlashMode69")]
		FlashRedEyeNoStrobeReturn = 69,

		[Description(@"Enum_FlashMode71")]
		FlashRedEyeStrobeReturn = 71,

		[Description(@"Enum_FlashMode73")]
		FlashCcompulsoryRedEye = 73,

		[Description(@"Enum_FlashMode77")]
		FlashCcompulsoryRedEyeNoStrobeReturn = 77,

		[Description(@"Enum_FlashMode79")]
		FlashCcompulsoryRedEyeStrobeReturn = 79,

		[Description(@"Enum_FlashMode89")]
		FlashAutoRedEye = 89,

		[Description(@"Enum_FlashMode93")]
		FlashAutoNoStrobeReturnRedEye = 93,

		[Description(@"Enum_FlashMode95")]
		FlashAutoStrobeReturnRedEye = 95,

		[Description(@"Enum_Unknown")]
		Unknown
	}
}
