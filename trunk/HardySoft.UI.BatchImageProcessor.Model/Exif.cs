using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
using HardySoft.CC.Mathematics;

namespace HardySoft.UI.BatchImageProcessor.Model {
	public enum ColorRepresentation {
		sRGB,

		[Description(@"Enum_Uncalibrated")]
		Uncalibrated
	}

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

	/*public enum WhiteBalanceMode : int {
		[Description(@"Enum_Auto")]
		Auto = 0,

		[Description(@"Enum_Manual")]
		Manual = 1,

		AutoBracket =2
	}*/

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// WIC (http://www.microsoft.com/downloads/details.aspx?familyid=8E011506-6307-445B-B950-215DEF45DDD8)
	/// is a very helpful tool to help develop Exif.
	/// </remarks>
	[Serializable]
	public class ExifMetadata {
		// more info http://blogs.technet.com/jamesone/archive/2007/07/13/exploring-photographic-exif-data-using-powershell-of-course.aspx
		private BitmapMetadata metadata;
		private BitmapFrame frame;
		private string fileName;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="imageUri"></param>
		/// <param name="forReading">
		/// To indicate if the operation is for reading or writing.
		/// </param>
		public ExifMetadata(Uri imageUri, bool forReading) {
			if (forReading) {
				this.frame = BitmapFrame.Create(imageUri, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
			} else {
				this.frame = BitmapFrame.Create(imageUri, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
			}
			//this.metadata = (BitmapMetadata)frame.Metadata;
			this.metadata = (BitmapMetadata)frame.Metadata.Clone();
			this.fileName = imageUri.LocalPath;
		}

		private decimal parseUnsignedRational(ulong exifValue) {
			// UInt64's in this case are actually two 32-bit numbers which form a fraction.
			// To decode, the high part is the numerator and the bottom is the denominator.
			return (decimal)(exifValue & 0xFFFFFFFFL) / (decimal)((exifValue & 0xFFFFFFFF00000000L) >> 32);
		}

		private decimal parseSignedRational(long exifValue) {
			//return (decimal)(exifValue & 0xFFFFFFFFL) / (decimal)((exifValue & 0x7FFFFFFF00000000L) >> 32);
			return (decimal)(new Fraction(BitConverter.ToInt32(BitConverter.GetBytes(exifValue), 0),
				BitConverter.ToInt32(BitConverter.GetBytes(exifValue), 4)).ToDouble());
		}

		private ulong createUnsignedRational(uint numerator, uint denominator) {
			return (ulong)((ulong)denominator << 32) + (ulong)numerator;
		}

		private long createSignedRational(long numerator, long denominator) {
			return (long)(denominator << 32) + numerator;
		}

		/// <summary>
		/// Convert a Decimal to Sexagesimal
		/// </summary>
		/// <param name="degree"></param>
		/// <returns></returns>
		/// <remarks>
		/// Reference http://geography.about.com/library/howto/htdegrees.htm
		/// </remarks>
		private int[] convertDecimalToMinuteSecondDegree(decimal degree) {
			// TODO when upgrade to .Net 4.0 change return type to Tuple.
			List<int> result = new List<int>();
			int hour = (int)Math.Truncate(degree);
			result.Add(hour);

			decimal remainder = Math.Abs(degree - hour);

			decimal minuteDecimal = remainder * 60;
			int minute = (int)Math.Truncate(minuteDecimal);
			result.Add(minute);

			remainder = minuteDecimal - minute;

			int second = (int)Math.Truncate(remainder * 60);
			result.Add(second);

			return result.ToArray();
		}

		private object queryMetadata(string query) {
			if (metadata.ContainsQuery(query)) {
				return metadata.GetQuery(query);
			} else {
				return null;
			}
		}

		private void writeMetatadata(string query, object data) {
			metadata.SetQuery(query, data);
		}

		/// <summary>
		/// Reference http://www.awaresystems.be/imaging/tiff/tifftags/privateifd/exif/exifversion.html
		/// </summary>
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
			set {

				// hard-coded 2.20
				byte[] version = new byte[] {
					48,
					50,
					50,
					48
				};
				BitmapMetadataBlob blob = new BitmapMetadataBlob(version);
				writeMetatadata("/app1/ifd/exif/subifd:{uint=36864}", blob);
			}
		}

		/*[ExifDisplay("Label_Title")]
		public string Title {
			get {
				object val = queryMetadata("/app1/ifd/exif:{uint=40091}");
				return (val != null ? (string)val : String.Empty);
			}
		}

		[ExifDisplay("Label_Author")]
		public string Author {
			get {
				object val = queryMetadata("/app1/ifd/exif:{uint=40093}");
				return (val != null ? (string)val : String.Empty);
			}
		}*/

		[ExifDisplay("Label_MeteringMode")]
		public MeteringMode MeteringMode {
			get {
				object val = queryMetadata("/app1/ifd/exif/subifd:{uint=37383}");
				if (val == null) {
					return MeteringMode.Unknown;
				} else {
					return (MeteringMode)(Convert.ToInt32(val));
					/*try {
						switch ((ushort)val) {
							case 1:
								return MeteringMode.Average;
							case 2:
								return MeteringMode.Centre;
							case 3:
								return MeteringMode.Spot;
							case 4:
								return MeteringMode.MultiSpot;
							case 5:
								return MeteringMode.MultiSegment;
							case 6:
								return MeteringMode.Partial;
							default:
								return MeteringMode.Unknown;
						}
					} catch (Exception ex) {
						Trace.TraceError(ex.ToString());
						return MeteringMode.Unknown;
					}*/
				}
			}
			set {
				if (value != MeteringMode.Unknown) {
					writeMetatadata("/app1/ifd/exif/subifd:{uint=37383}", (ushort)value);
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
			set {
				if (value.HasValue) {
					writeMetatadata("/app1/ifd/exif/subifd:{uint=40962}", value.Value);
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
			set {
				if (value.HasValue) {
					writeMetatadata("/app1/ifd/exif/subifd:{uint=40963}", value.Value);
				}
			}
		}

		[ExifDisplay("Label_HorizontalResolution", "Label_ExifValue_DPI")]
		public decimal? HorizontalResolution {
			get {
				object val = queryMetadata("/app1/ifd/exif:{uint=282}");
				if (val != null) {
					ulong value = Convert.ToUInt64(val);
					return parseUnsignedRational(value);
				} else {
					return null;
				}
			}
			set {
				if (value.HasValue) {
					writeMetatadata("/app1/ifd/exif:{uint=282}", createUnsignedRational((uint)value.Value, 1));
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
			set {
				if (value.HasValue) {
					writeMetatadata("/app1/ifd/exif:{uint=283}", createUnsignedRational((uint)value.Value, 1));
				}
			}
		}

		[ExifDisplay("Label_Manufacturer")]
		public string EquipmentManufacturer {
			get {
				object val = queryMetadata("/app1/ifd/exif:{uint=271}");
				return (val != null ? (string)val : String.Empty);
			}
			set {
				if (!string.IsNullOrEmpty(value)) {
					writeMetatadata("/app1/ifd/exif:{uint=271}", value);
				}
			}
		}

		[ExifDisplay("Label_Camera")]
		public string CameraModel {
			get {
				object val = queryMetadata("/app1/ifd/exif:{uint=272}");
				return (val != null ? (string)val : string.Empty);
			}
			set {
				if (!string.IsNullOrEmpty(value)) {
					writeMetatadata("/app1/ifd/exif:{uint=272}", value);
				}
			}
		}

		[ExifDisplay("Label_CreationSoftware")]
		public string CreationSoftware {
			get {
				object val = queryMetadata("/app1/ifd/exif:{uint=305}");
				return (val != null ? (string)val : string.Empty);
			}
			set {
				if (!string.IsNullOrEmpty(value)) {
					writeMetatadata("/app1/ifd/exif:{uint=305}", value);
				}
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
			set {
				writeMetatadata("/app1/ifd/exif/subifd:{uint=40961}", (ushort)value);
			}
		}

		/*[ExifDisplay("Label_ExposureTime", "Label_ExifValue_Second")]
		public string ExposureTime {
			get {
				object val = queryMetadata("/app1/ifd/exif/subifd:{uint=33434}");
				if (val != null) {
					decimal time = parseUnsignedRational((ulong)val);
					// investigate if all the EXIF decimal or float value has no localized number information.
					System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-us");
					Fraction f = new Fraction(time.ToString(), culture.NumberFormat);
					return f.ToString();
				} else {
					return string.Empty;
				}
			}
		}*/

		[ExifDisplay("Label_ExposureTime", "Label_ExifValue_Second")]
		public Fraction ExposureTime {
			get {
				object val = queryMetadata("/app1/ifd/exif/subifd:{uint=33434}");
				if (val != null) {
					decimal time = parseUnsignedRational((ulong)val);
					// investigate if all the EXIF decimal or float value has no localized number information.
					System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-us");
					return new Fraction(time.ToString(), culture.NumberFormat);
				} else {
					return null;
				}
			}
			set {
				if (value != null) {
					writeMetatadata("/app1/ifd/exif/subifd:{uint=33434}",
						createUnsignedRational((uint)value.Numerator, (uint)value.Denominator));
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
			set {
				if (value.HasValue) {
					Fraction frac = new Fraction(value.Value.ToString());
					writeMetatadata("/app1/ifd/exif/subifd:{uint=37380}",
						createSignedRational(frac.Numerator, frac.Denominator));
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
			set {
				if (value.HasValue) {
					Fraction frac = new Fraction(value.Value.ToString());
					writeMetatadata("/app1/ifd/exif/subifd:{uint=33437}",
						createUnsignedRational((uint)frac.Numerator, (uint)frac.Denominator));
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
			set {
				if (value.HasValue) {
					Fraction frac = new Fraction(value.Value.ToString());
					writeMetatadata("/app1/ifd/exif/subifd:{uint=37386}",
						createUnsignedRational((uint)frac.Numerator, (uint)frac.Denominator));
				}
			}
		}

		[ExifDisplay("Label_ISOSpeed")]
		public ushort? IsoSpeed {
			get {
				return (ushort?)queryMetadata("/app1/ifd/exif/subifd:{uint=34855}");
			}
			set {
				if (value.HasValue) {
					writeMetatadata("/app1/ifd/exif/subifd:{uint=34855}", value.Value);
				}
			}
		}

		[ExifDisplay("Label_FlashMode")]
		public FlashMode FlashMode {
			get {
				object o = queryMetadata("/app1/ifd/exif/subifd:{uint=37385}");
				if (o != null) {
					ushort mode;
					if (ushort.TryParse(o.ToString(), out mode)) {
						return (FlashMode)mode;
					} else {
						return FlashMode.Unknown;
					}
					/*if ((ushort)queryMetadata("/app1/ifd/exif/subifd:{uint=37385}") % 2 == 1) {
						return FlashMode.FlashFired;
					} else {
						return FlashMode.FlashDidNotFire;
					}*/
				} else {
					return FlashMode.Unknown;
				}
			}
			set {
				if (value != FlashMode.Unknown) {
					writeMetatadata("/app1/ifd/exif/subifd:{uint=37385}", (ushort)value);
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
					return (ExposureMode)mode;
				}
			}
			set {
				if (value != ExposureMode.Unknown) {
					writeMetatadata("/app1/ifd/exif/subifd:{uint=34850}", (ushort)value);
				}
			}
		}

		[ExifDisplay("Label_LightSource")]
		public LightSourceMode LightSource {
			get {
				ushort? mode = (ushort?)queryMetadata("/app1/ifd/exif/subifd:{uint=37384}");

				if (mode == null) {
					return LightSourceMode.Unknown;
				} else {
					return (LightSourceMode)((int)mode);
					/*switch ((int)mode) {
						case 0:
							return LightSourceMode.Auto;
						case 1:
							return LightSourceMode.Daylight;
						case 2:
							return LightSourceMode.Fluorescent;
						case 3:
							return LightSourceMode.Tungsten;
						case 4:
							return LightSourceMode.Flash;
						case 9:
							return LightSourceMode.FineWeather;
						case 10:
							return LightSourceMode.CloudyWeather;
						case 11:
							return LightSourceMode.Shade;
						case 12:
							return LightSourceMode.DaylightFluorescent;
						case 13:
							return LightSourceMode.DayWhiteFluorescent;
						case 14:
							return LightSourceMode.CoolWhiteFluorescent;
						case 15:
							return LightSourceMode.WhiteFluorescent;
						case 17:
							return LightSourceMode.StandardLightA;
						case 18:
							return LightSourceMode.StandardLightB;
						case 19:
							return LightSourceMode.StandardLightC;
						case 20:
							return LightSourceMode.D55;
						case 21:
							return LightSourceMode.D65;
						case 22:
							return LightSourceMode.D75;
						case 23:
							return LightSourceMode.D50;
						case 24:
							return LightSourceMode.ISOStudioTungsten;
						case 255:
							return LightSourceMode.Other;
						default:
							return LightSourceMode.Unknown;
					}*/
				}
			}
			set {
				if (value != LightSourceMode.Unknown) {
					writeMetatadata("/app1/ifd/exif/subifd:{uint=37384}", (ushort)value);
				}
			}
		}

		/*
		[ExifDisplay("Label_WhiteBalanceMode")]
		public WhiteBalanceMode WhiteBalance {
			get {
				ushort? mode = (ushort?)queryMetadata("/app1/ifd/exif/subifd:{uint=41986}");

				if (mode == null) {
					return WhiteBalanceMode.Auto;
				} else {
					return (WhiteBalanceMode)((int)mode);
				}
			}
		}*/

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
					} catch (FormatException ex1) {
						Trace.TraceError(ex1.ToString());
						return null;
					} catch (OverflowException ex2) {
						Trace.TraceError(ex2.ToString());
						return null;
					} catch (ArgumentNullException ex3) {
						Trace.TraceError(ex3.ToString());
						return null;
					} catch (NullReferenceException ex4) {
						Trace.TraceError(ex4.ToString());
						return null;
					}
				}
			}
			set {
				if (value.HasValue) {
					writeMetatadata("/app1/ifd/exif/subifd:{uint=36867}",
						value.Value.ToString("yyyy:MM:dd hh:mm:ss"));
				}
			}
		}

		private GPSExifLocation latitudeCache;

		[ExifDisplay("Label_Latitude")]
		public decimal? Latitude {
			get {
				if (LatitudeRaw != null) {
					decimal latitude = parseUnsignedRational(latitudeCache.RawCoordinate[0]) 
						+ (parseUnsignedRational(latitudeCache.RawCoordinate[1]) / 60) 
						+ (parseUnsignedRational(latitudeCache.RawCoordinate[2]) / 3600);

					if (latitudeCache.CoordinateRef == "S") {
						latitude *= -1;
					}

					return latitude;
				} else {
					return null;
				}
			}
		}

		public GPSExifLocation LatitudeRaw {
			get {
				GPSExifLocation location = null;
				ulong[] val = (ulong[])queryMetadata("/app1/ifd/gps/{uint=2}");
				if (val != null) {
					location = new GPSExifLocation();
					location.RawCoordinate = val;

					// N or S
					string longitudeRef = queryMetadata("/app1/ifd/gps/{uint=1}").ToString().ToUpper();
					location.CoordinateRef = longitudeRef;

					latitudeCache = location;
				}

				return location;
			}
			set {
				if (value != null) {
					writeMetatadata("/app1/ifd/gps/{uint=2}", value.RawCoordinate);
					writeMetatadata("/app1/ifd/gps/{uint=1}", value.CoordinateRef);
				}
			}
		}

		private GPSExifLocation longitudeCache;

		[ExifDisplay("Label_Longitude")]
		public decimal? Longitude {
			get {
				if (LongitudeRaw != null) {
					decimal longitude = parseUnsignedRational(longitudeCache.RawCoordinate[0])
						+ (parseUnsignedRational(longitudeCache.RawCoordinate[1]) / 60)
						+ (parseUnsignedRational(longitudeCache.RawCoordinate[2]) / 3600);

					if (longitudeCache.CoordinateRef == "W") {
						longitude *= -1;
					}

					return longitude;
				} else {
					return null;
				}
			}
		}

		public GPSExifLocation LongitudeRaw {
			get {
				GPSExifLocation location = null;
				ulong[] val = (ulong[])queryMetadata("/app1/ifd/gps/{uint=4}");
				if (val != null) {
					location = new GPSExifLocation();
					location.RawCoordinate = val;

					// N or S
					string longitudeRef = queryMetadata("/app1/ifd/gps/{uint=3}").ToString().ToUpper();
					location.CoordinateRef = longitudeRef;

					longitudeCache = location;
				}

				return location;
			}
			set {
				if (value != null) {
					writeMetatadata("/app1/ifd/gps/{uint=4}", value.RawCoordinate);
					writeMetatadata("/app1/ifd/gps/{uint=3}", value.CoordinateRef);
				}
			}
		}

		[ExifDisplay("Label_Altitude")]
		public decimal? Altitude {
			get {
				var val = queryMetadata("/app1/ifd/gps/{uint=6}");
				if (val == null) {
					return null;
				} else {
					decimal altitude = parseUnsignedRational(Convert.ToUInt64(val));

					var altitudeRef = queryMetadata("/app1/ifd/gps/{uint=5}");

					// 0 = Above Sea Level, 1 = Below Sea Level
					if (altitudeRef != null && altitudeRef.ToString() == "1") {
						altitude *= -1;
					}
					return altitude;
				}
			}
			set {
				if (value.HasValue) {
					if (value.Value >= 0) {
						writeMetatadata("/app1/ifd/gps/{uint=6}", createUnsignedRational((uint)value.Value, 1));
						writeMetatadata("/app1/ifd/gps/{uint=5}", "0");
					} else {
						writeMetatadata("/app1/ifd/gps/{uint=6}", createUnsignedRational((uint)(value.Value * -1), 1));
						writeMetatadata("/app1/ifd/gps/{uint=5}", "1");
					}
				}
			}
		}

		public bool SaveExif() {
			JpegBitmapEncoder encoder = new JpegBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(frame, frame.Thumbnail, metadata, frame.ColorContexts));
			try {
				using (Stream jpegStreamOut = File.Open(this.fileName,
					FileMode.Create, FileAccess.ReadWrite)) {
					encoder.Save(jpegStreamOut);
				}

				return true;
			} catch (Exception ex) {
				Trace.TraceError(ex.ToString());
				return false;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class ExifDisplayAttribute : DisplayAttribute {
		private string valueFormat;

		public ExifDisplayAttribute(string displayName)
			: base(displayName) {
			this.valueFormat = "";
		}

		public ExifDisplayAttribute(string displayName, string valueFormat)
			: base(displayName) {
			this.valueFormat = valueFormat;
		}

		public string ValueFormat {
			get {
				return this.valueFormat;
			}
		}
	}

	public class DisplayAttribute : Attribute {
		protected string displayName;

		public string DisplayName {
			get {
				return displayName;
			}
		}

		public DisplayAttribute(string displayName)
			: base() {
			this.displayName = displayName;
		}
	}

	/// <summary>
	/// A class used to represent a latitude or longitude data in GPS Exif.
	/// </summary>
	public class GPSExifLocation {
		/// <summary>
		/// "/app1/ifd/gps/{uint=2}" or "/app1/ifd/gps/{uint=4}" value
		/// </summary>
		public ulong[] RawCoordinate {
			get;
			set;
		}

		/// <summary>
		/// "/app1/ifd/gps/{uint=1}" or "/app1/ifd/gps/{uint=3}" value
		/// </summary>
		public string CoordinateRef {
			get;
			set;
		}
	}
}
