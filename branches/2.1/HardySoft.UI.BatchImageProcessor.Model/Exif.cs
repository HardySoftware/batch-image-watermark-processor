using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace HardySoft.UI.BatchImageProcessor.Model {
	public enum ColorRepresentation {
		sRGB,
		Uncalibrated
	}

	public enum FlashMode {
		FlashFired,
		FlashDidNotFire,
		Unkown
	}

	public enum ExposureMode {
		Manual,
		NormalProgram,
		AperturePriority,
		ShutterPriority,
		LowSpeedMode,
		HighSpeedMode,
		PortraitMode,
		LandscapeMode,
		Unknown
	}

	public enum WhiteBalanceMode {
		Daylight,
		Fluorescent,
		Tungsten,
		Flash,
		StandardLightA,
		StandardLightB,
		StandardLightC,
		D55,
		D65,
		D75,
		Other,
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

		[ExifDisplay("Width")]
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

		[ExifDisplay("Height")]
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

		[ExifDisplay("Horizontal Resolution")]
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

		[ExifDisplay("Vertical Resolution")]
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

		[ExifDisplay("Manufacturer")]
		public string EquipmentManufacturer {
			get {
				object val = queryMetadata("/app1/ifd/exif:{uint=271}");
				return (val != null ? (string)val : String.Empty);
			}
		}

		[ExifDisplay("Camera")]
		public string CameraModel {
			get {
				object val = queryMetadata("/app1/ifd/exif:{uint=272}");
				return (val != null ? (string)val : String.Empty);
			}
		}

		[ExifDisplay("CreationSoftware")]
		public string CreationSoftware {
			get {
				object val = queryMetadata("/app1/ifd/exif:{uint=305}");
				return (val != null ? (string)val : String.Empty);
			}
		}

		[ExifDisplay("Color Representation")]
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

		[ExifDisplay("Exposure Time")]
		public decimal? ExposureTime {
			get {
				object val = queryMetadata("/app1/ifd/exif/subifd:{uint=33434}");
				if (val != null) {
					return parseUnsignedRational((ulong)val);
				} else {
					return null;
				}
			}
		}

		[ExifDisplay("Exposure Compensation")]
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

		[ExifDisplay("Aperture")]
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

		[ExifDisplay("Focal Length")]
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

		[ExifDisplay("ISO Speed")]
		public ushort? IsoSpeed {
			get {
				return (ushort?)queryMetadata("/app1/ifd/exif/subifd:{uint=34855}");
			}
		}

		[ExifDisplay("Flash Mode")]
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

		[ExifDisplay("Exposure Mode")]
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

		[ExifDisplay("White Balance Mode")]
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
						case 10:
							return WhiteBalanceMode.Flash;
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
						case 255:
							return WhiteBalanceMode.Other;
						default:
							return WhiteBalanceMode.Unknown;
					}
				}
			}
		}

		[ExifDisplay("Date Taken")]
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
		private string displayGroup;
		private string displayName;

		public ExifDisplayAttribute(string displayName) {
			this.displayName = displayName;
			this.displayGroup = "";
		}

		public ExifDisplayAttribute(string displayName, string displayGroup) {
			this.displayName = displayName;
			this.displayGroup = displayGroup;
		}

		public string DisplayGroup {
			get {
				return this.displayGroup;
			}
		}

		public string DisplayName {
			get {
				return displayName;
			}
		}
	}
}
