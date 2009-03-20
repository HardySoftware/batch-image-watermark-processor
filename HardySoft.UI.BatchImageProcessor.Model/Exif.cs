using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using HardySoft.CC.Mathematics;

namespace HardySoft.UI.BatchImageProcessor.Model {
	public enum ColorRepresentation {
		sRGB,

		[Description(@"Enum_Uncalibrated")]
		Uncalibrated
	}

	public enum FlashMode {
		[Description(@"Enum_FlashFired")]
		FlashFired,

		[Description(@"Enum_FlashDidNotFire")]
		FlashDidNotFire,

		[Description(@"Enum_Unkown")]
		Unkown
	}

	public enum ExposureMode {
		[Description(@"Enum_Manual")]
		Manual,

		[Description(@"Enum_NormalProgram")]
		NormalProgram,

		[Description(@"Enum_AperturePriority")]
		AperturePriority,

		[Description(@"Enum_ShutterPriority")]
		ShutterPriority,

		[Description(@"Enum_LowSpeedMode")]
		LowSpeedMode,

		[Description(@"Enum_HighSpeedMode")]
		HighSpeedMode,

		[Description(@"Enum_PortraitMode")]
		PortraitMode,

		[Description(@"Enum_LandscapeMode")]
		LandscapeMode,

		[Description(@"Enum_Unknown")]
		Unknown
	}

	public enum WhiteBalanceMode {
		[Description(@"Enum_Daylight")]
		Daylight,

		[Description(@"Enum_Fluorescent")]
		Fluorescent,

		[Description(@"Enum_Tungsten")]
		Tungsten,

		[Description(@"Enum_FineWeather")]
		FineWeather,

		[Description(@"Enum_CloudyWeather")]
		CloudyWeather,

		[Description(@"Enum_Shade")]
		Shade,

		[Description(@"Enum_DaylightFluorescent")]
		DaylightFluorescent,

		[Description(@"Enum_DayWhiteFluorescent")]
		DayWhiteFluorescent,

		[Description(@"Enum_CoolWhiteFluorescent")]
		CoolWhiteFluorescent,

		[Description(@"Enum_WhiteFluorescent")]
		WhiteFluorescent,

		[Description(@"Enum_Flash")]
		Flash,

		[Description(@"Enum_StandardLightA")]
		StandardLightA,

		[Description(@"Enum_StandardLightB")]
		StandardLightB,

		[Description(@"Enum_StandardLightC")]
		StandardLightC,

		D55,
		D65,
		D75,
		D50,

		[Description(@"Enum_Other")]
		Other,

		[Description(@"Enum_ISOStudioTungsten")]
		ISOStudioTungsten,

		[Description(@"Enum_Unknown")]
		Unknown
	}

	[Serializable]
	public class ExifMetadata {
		BitmapMetadata _metadata;

		public ExifMetadata(Uri imageUri) {
			BitmapFrame frame = BitmapFrame.Create(imageUri, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
			_metadata = (BitmapMetadata)frame.Metadata;
		}

		private decimal parseUnsignedRational(ulong exifValue) {
			return (decimal)(exifValue & 0xFFFFFFFFL) / (decimal)((exifValue & 0xFFFFFFFF00000000L) >> 32);
		}

		private decimal parseSignedRational(long exifValue) {
			return (decimal)(exifValue & 0xFFFFFFFFL) / (decimal)((exifValue & 0x7FFFFFFF00000000L) >> 32);
		}

		private object queryMetadata(string query) {
			if (_metadata.ContainsQuery(query)) {
				return _metadata.GetQuery(query);
			} else {
				return null;
			}
		}

		public string ExifVersion {
			get {
				object val = queryMetadata("/app1/ifd/exif/subifd:{uint=36864}");
				if (val == null) {
					return string.Empty;
				} else if (val.GetType() == typeof(BitmapMetadataBlob)) {
					BitmapMetadataBlob blob = (BitmapMetadataBlob)val;
					byte[] version = blob.GetBlobValue();
					string s = System.Text.Encoding.ASCII.GetString(version);
					return s;
				} else {
					return string.Empty;
				}
			}
		}

		[ExifDisplay("Label_Width", "Label_ExifValue_Pixel")]
		public uint? Width {
			get {
				object val = queryMetadata("/app1/ifd/exif/subifd:{uint=40962}");
				if (val == null) {
					return null;
				} else {
					if (val.GetType() == typeof(UInt32)) {
						return (uint?)val;
					} else {
						return System.Convert.ToUInt32(val);
					}
				}
			}
		}

		[ExifDisplay("Label_Height", "Label_ExifValue_Pixel")]
		public uint? Height {
			get {
				object val = queryMetadata("/app1/ifd/exif/subifd:{uint=40963}");
				if (val == null) {
					return null;
				} else {
					if (val.GetType() == typeof(UInt32)) {
						return (uint?)val;
					} else {
						return System.Convert.ToUInt32(val);
					}
				}
			}
		}

		[ExifDisplay("Label_HorizontalResolution", "Label_ExifValue_DPI")]
		public decimal? HorizontalResolution {
			get {
				object val = queryMetadata("/app1/ifd/exif:{uint=282}");
				if (val != null) {
					return parseUnsignedRational((ulong)val);
				} else {
					return null;
				}
			}
		}

		[ExifDisplay("Label_VerticalResolution", "Label_ExifValue_DPI")]
		public decimal? VerticalResolution {
			get {
				object val = queryMetadata("/app1/ifd/exif:{uint=283}");
				if (val != null) {
					return parseUnsignedRational((ulong)val);
				} else {
					return null;
				}
			}
		}

		[ExifDisplay("Label_Manufacturer")]
		public string EquipmentManufacturer {
			get {
				object val = queryMetadata("/app1/ifd/exif:{uint=271}");
				return (val != null ? (string)val : String.Empty);
			}
		}

		[ExifDisplay("Label_Camera")]
		public string CameraModel {
			get {
				object val = queryMetadata("/app1/ifd/exif:{uint=272}");
				return (val != null ? (string)val : String.Empty);
			}
		}

		[ExifDisplay("Label_CreationSoftware")]
		public string CreationSoftware {
			get {
				object val = queryMetadata("/app1/ifd/exif:{uint=305}");
				return (val != null ? (string)val : String.Empty);
			}
		}

		[ExifDisplay("Label_ColorRepresentation")]
		public ColorRepresentation ColorRepresentation {
			get {
				object o = queryMetadata("/app1/ifd/exif/subifd:{uint=40961}");
				if (o != null) {
					if ((ushort)o == 1) {
						return ColorRepresentation.sRGB;
					} else {
						return ColorRepresentation.Uncalibrated;
					}
				} else {
					return ColorRepresentation.Uncalibrated;
				}
			}
		}

		/*[ExifDisplay("Label_ExposureTime")]
		public decimal? ExposureTime {
			get {
				object val = queryMetadata("/app1/ifd/exif/subifd:{uint=33434}");
				if (val != null) {
					return parseUnsignedRational((ulong)val);
				} else {
					return null;
				}
			}
		}*/
		[ExifDisplay("Label_ExposureTime", "Label_ExifValue_Second")]
		public string ExposureTime {
			get {
				object val = queryMetadata("/app1/ifd/exif/subifd:{uint=33434}");
				if (val != null) {
					decimal time = parseUnsignedRational((ulong)val);
					// TODO investigate if all the EXIF decimal or float value has no localized number information.
					System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-us");
					Fraction f = new Fraction(time.ToString(), culture.NumberFormat);
					return f.ToString();
				} else {
					return string.Empty;
				}
			}
		}

		[ExifDisplay("Label_ExposureCompensation")]
		public decimal? ExposureCompensation {
			get {
				object val = queryMetadata("/app1/ifd/exif/subifd:{uint=37380}");
				if (val != null) {
					return parseSignedRational((long)val);
				} else {
					return null;
				}
			}
		}

		[ExifDisplay("Label_Aperture", "Label_ExifValue_Aperture")]
		public decimal? LensAperture {
			get {
				object val = queryMetadata("/app1/ifd/exif/subifd:{uint=33437}");
				if (val != null) {
					return parseUnsignedRational((ulong)val);
				} else {
					return null;
				}
			}
		}

		[ExifDisplay("Label_FocalLength", "Label_ExifValue_MM")]
		public decimal? FocalLength {
			get {
				object val = queryMetadata("/app1/ifd/exif/subifd:{uint=37386}");
				if (val != null) {
					return parseUnsignedRational((ulong)val);
				} else {
					return null;
				}
			}
		}

		[ExifDisplay("Label_ISOSpeed")]
		public ushort? IsoSpeed {
			get {
				return (ushort?)queryMetadata("/app1/ifd/exif/subifd:{uint=34855}");
			}
		}

		[ExifDisplay("Label_FlashMode")]
		public FlashMode FlashMode {
			get {
				object o = queryMetadata("/app1/ifd/exif/subifd:{uint=37385}");
				if (o != null) {
					if ((ushort)queryMetadata("/app1/ifd/exif/subifd:{uint=37385}") % 2 == 1) {
						return FlashMode.FlashFired;
					} else {
						return FlashMode.FlashDidNotFire;
					}
				} else {
					return FlashMode.Unkown;
				}
			}
		}

		[ExifDisplay("Label_ExposureMode")]
		public ExposureMode ExposureMode {
			get {
				ushort? mode = (ushort?)queryMetadata("/app1/ifd/exif/subifd:{uint=34850}");

				if (mode == null) {
					return ExposureMode.Unknown;
				} else {
					switch ((int)mode) {
						case 1:
							return ExposureMode.Manual;
						case 2:
							return ExposureMode.NormalProgram;
						case 3:
							return ExposureMode.AperturePriority;
						case 4:
							return ExposureMode.ShutterPriority;
						case 5:
							return ExposureMode.LowSpeedMode;
						case 6:
							return ExposureMode.HighSpeedMode;
						case 7:
							return ExposureMode.PortraitMode;
						case 8:
							return ExposureMode.LandscapeMode;
						default:
							return ExposureMode.Unknown;
					}
				}
			}
		}

		[ExifDisplay("Label_WhiteBalanceMode")]
		public WhiteBalanceMode WhiteBalanceMode {
			get {
				ushort? mode = (ushort?)queryMetadata("/app1/ifd/exif/subifd:{uint=37384}");

				if (mode == null) {
					return WhiteBalanceMode.Unknown;
				} else {
					switch ((int)mode) {
						case 1:
							return WhiteBalanceMode.Daylight;
						case 2:
							return WhiteBalanceMode.Fluorescent;
						case 3:
							return WhiteBalanceMode.Tungsten;
						case 4:
							return WhiteBalanceMode.Flash;
						case 9:
							return WhiteBalanceMode.FineWeather;
						case 10:
							return WhiteBalanceMode.CloudyWeather;
						case 11:
							return WhiteBalanceMode.Shade;
						case 12:
							return WhiteBalanceMode.DaylightFluorescent;
						case 13:
							return WhiteBalanceMode.DayWhiteFluorescent;
						case 14:
							return WhiteBalanceMode.CoolWhiteFluorescent;
						case 15:
							return WhiteBalanceMode.WhiteFluorescent;
						case 17:
							return WhiteBalanceMode.StandardLightA;
						case 18:
							return WhiteBalanceMode.StandardLightB;
						case 19:
							return WhiteBalanceMode.StandardLightC;
						case 20:
							return WhiteBalanceMode.D55;
						case 21:
							return WhiteBalanceMode.D65;
						case 22:
							return WhiteBalanceMode.D75;
						case 23:
							return WhiteBalanceMode.D50;
						case 24:
							return WhiteBalanceMode.ISOStudioTungsten;
						case 255:
							return WhiteBalanceMode.Other;
						default:
							return WhiteBalanceMode.Unknown;
					}
				}
			}
		}

		[ExifDisplay("Label_DateTaken")]
		public DateTime? DateImageTaken {
			get {
				object val = queryMetadata("/app1/ifd/exif/subifd:{uint=36867}");
				if (val == null) {
					return null;
				} else {
					string date = (string)val;
					try {
						return new DateTime(
							int.Parse(date.Substring(0, 4)),    // year
							int.Parse(date.Substring(5, 2)),    // month
							int.Parse(date.Substring(8, 2)),    // day
							int.Parse(date.Substring(11, 2)),   // hour
							int.Parse(date.Substring(14, 2)),   // minute
							int.Parse(date.Substring(17, 2))    // second
						);
					} catch (FormatException) {
						return null;
					} catch (OverflowException) {
						return null;
					} catch (ArgumentNullException) {
						return null;
					} catch (NullReferenceException) {
						return null;
					}
				}
			}
		}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class ExifDisplayAttribute : Attribute {
		private string valueFormat;
		private string displayName;

		public ExifDisplayAttribute(string displayName) {
			this.displayName = displayName;
			this.valueFormat = "";
		}

		public ExifDisplayAttribute(string displayName, string valueFormat) {
			this.displayName = displayName;
			this.valueFormat = valueFormat;
		}

		public string ValueFormat {
			get {
				return this.valueFormat;
			}
		}

		public string DisplayName {
			get {
				return displayName;
			}
		}
	}
}
