using System.ComponentModel;

namespace HardySoft.UI.BatchImageProcessor.Model.Exif {
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Reference http://msdn.microsoft.com/en-us/library/bb760489(VS.85).aspx
	/// </remarks>
	public enum LightSourceMode : int {
		[Description(@"Enum_Auto")]
		Auto,

		[Description(@"Enum_Daylight")]
		Daylight = 1,

		[Description(@"Enum_Fluorescent")]
		Fluorescent = 2,

		[Description(@"Enum_Tungsten")]
		Tungsten = 3,

		[Description(@"Enum_Flash")]
		Flash = 4,

		[Description(@"Enum_FineWeather")]
		FineWeather = 9,

		[Description(@"Enum_CloudyWeather")]
		CloudyWeather = 10,

		[Description(@"Enum_Shade")]
		Shade = 11,

		[Description(@"Enum_DaylightFluorescent")]
		DaylightFluorescent = 12,

		[Description(@"Enum_DayWhiteFluorescent")]
		DayWhiteFluorescent = 13,

		[Description(@"Enum_CoolWhiteFluorescent")]
		CoolWhiteFluorescent = 14,

		[Description(@"Enum_WhiteFluorescent")]
		WhiteFluorescent = 15,

		[Description(@"Enum_StandardLightA")]
		StandardLightA = 17,

		[Description(@"Enum_StandardLightB")]
		StandardLightB = 18,

		[Description(@"Enum_StandardLightC")]
		StandardLightC = 19,

		D55 = 20,
		D65 = 21,
		D75 = 22,
		D50 = 23,

		[Description(@"Enum_Other")]
		Other = 255,

		[Description(@"Enum_ISOStudioTungsten")]
		ISOStudioTungsten = 24,

		[Description(@"Enum_Unknown")]
		Unknown = 0
	}
}
