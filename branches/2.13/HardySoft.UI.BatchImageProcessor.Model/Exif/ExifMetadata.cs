using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using HardySoft.CC.Mathematics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace HardySoft.UI.BatchImageProcessor.Model.Exif {
	public class ExifMetadata {
		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// All property tags http://msdn.microsoft.com/en-us/library/ms534417%28v=vs.85%29.aspx
		/// </remarks>
		private PropertyItem[] propItems;
		private string fileName;

		private static readonly int UInt16Size = Marshal.SizeOf(typeof(ushort));
		private static readonly int Int32Size = Marshal.SizeOf(typeof(int));
		private static readonly int UInt32Size = Marshal.SizeOf(typeof(uint));
		private static readonly int RationalSize = 2 * Marshal.SizeOf(typeof(int));
		private static readonly int URationalSize = 2 * Marshal.SizeOf(typeof(uint));

		private string[] exifDateTimeFormats = new string[]{
					"yyyy:MM:dd HH:mm:ss",
					"yyyy:MM:dd   :  :  ",
					"    :  :   HH:mm:ss",
				};

		public ExifMetadata(Uri imageUri) {
			this.fileName = imageUri.AbsolutePath;
			using (FileStream stream = new FileStream(this.fileName, FileMode.Open)) {
				using (Image image = Image.FromStream(stream)) {
					this.propItems = image.PropertyItems;
				}
			}
		}

		/// <summary>
		/// Queries the image's property item list to get value of the designated property Id.
		/// </summary>
		/// <param name="propertyId">The property Id to query by.</param>
		/// <param name="typeOfValue">The type of the value should be.</param>
		/// <returns></returns>
		private object QueryMetadata(int propertyId, Type typeOfValue) {
			var property = (from p in this.propItems where p.Id == propertyId select p).FirstOrDefault();

			if (property != null) {
				return this.FromPropertyItem(property, typeOfValue);
			}

			return null;
		}

		/// <summary>
		/// Converts a property item to an object or array of objects.
		/// </summary>
		/// <param name="propertyItem">the property item to convert</param>
		/// <param name="typeOfValue">The type of the value should be.</param>
		/// <returns>the property value</returns>
		private object FromPropertyItem(PropertyItem propertyItem, Type typeOfValue) {
			if (propertyItem == null) {
				return null;
			}

			object data = null;

			ExifType exifType = (ExifType)propertyItem.Type;

			switch (exifType) {
				case ExifType.Ascii: {
						// The value represents an array of chars terminated with null ('\0') char
						data = Encoding.ASCII.GetString(propertyItem.Value).TrimEnd('\0');
						break;
					}
				case ExifType.Byte: {
						switch (propertyItem.Id) {
							case 40091: // Title
							case 40095: // Subject
							case 40093: // Author
							case 40094: // Keywords
							case 40092: // Comments
								// The value represents an array of unicode bytes terminated with null ('\0') char
								data = Encoding.Unicode.GetString(propertyItem.Value).TrimEnd('\0');
								break;
							default:
								// The value represents an array of bytes
								data = propertyItem.Value;
								break;
						}
						break;
					}
				case ExifType.Raw: {
						// The value represents an array of bytes
						data = propertyItem.Value;
						break;
					}
				case ExifType.UInt16: {
						// The value represents an array of unsigned 16-bit integers.
						int count = propertyItem.Len / UInt16Size;

						ushort[] result = new ushort[count];
						for (int i = 0; i < count; i++) {
							result[i] = BitConverter.ToUInt16(propertyItem.Value, i * UInt16Size);
						}
						data = result;
						break;
					}
				case ExifType.Int32: {
						// The value represents an array of signed 32-bit integers.
						int count = propertyItem.Len / Int32Size;

						int[] result = new int[count];
						for (int i = 0; i < count; i++) {
							result[i] = BitConverter.ToInt32(propertyItem.Value, i * Int32Size);
						}
						data = result;
						break;
					}
				case ExifType.UInt32: {
						// The value represents an array of unsigned 32-bit integers.
						int count = propertyItem.Len / UInt32Size;

						uint[] result = new uint[count];
						for (int i = 0; i < count; i++) {
							result[i] = BitConverter.ToUInt32(propertyItem.Value, i * UInt32Size);
						}
						data = result;
						break;
					}
				case ExifType.Rational: {
						// The value represents an array of signed rational numbers
						// Numerator is an Int32 value, denominator a UInt32 value.
						int count = propertyItem.Len / RationalSize;
						Fraction[] result = new Fraction[count];
						for (int i = 0; i < count; i++) {
							int numberator = BitConverter.ToInt32(propertyItem.Value, i * RationalSize);
							int denominator = BitConverter.ToInt32(propertyItem.Value, i * RationalSize + Int32Size);

							result[i] = new Fraction(numberator, denominator);
						}

						return result;
					}
				case ExifType.URational: {
						// The value represents an array of signed rational numbers
						// Numerator and denominator are UInt32 values.
						int count = propertyItem.Len / URationalSize;
						Fraction[] result = new Fraction[count];
						for (int i = 0; i < count; i++) {
							int numberator = BitConverter.ToInt32(propertyItem.Value, i * URationalSize);
							int denominator = BitConverter.ToInt32(propertyItem.Value, i * URationalSize + UInt32Size);

							result[i] = new Fraction(numberator, denominator);
						}

						return result;
					}
				default: {
						data = propertyItem.Value;
						break;
					}
			}

			return this.ConvertData(typeOfValue, data);
		}

		private object ConvertData(Type targetType, object value) {
			if (targetType == null || value == null) {
				return value;
			}

			if (value is Array) {
				if (targetType == typeof(string)) {
					return Encoding.ASCII.GetString((byte[])value);
				} else {
					int length = ((Array)value).Length;
					if (length < 1) {
						value = null;
					} else if (length == 1) {
						value = ((Array)value).GetValue(0);
					}
				}
			}

			if (value is String) {
				value = ((String)value).TrimEnd('\0').Trim();
			}

			if (targetType == typeof(DateTime) && value is String) {
				string date = value.ToString();
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

			if (targetType.IsEnum) {
				Type underlyingType = Enum.GetUnderlyingType(targetType);
				if (value.GetType() != underlyingType) {
					value = Convert.ChangeType(value, underlyingType);
				}

				if (Enum.IsDefined(targetType, value) || FlagsAttribute.IsDefined(targetType, typeof(FlagsAttribute))) {
					try {
						return Enum.ToObject(targetType, value);
					} catch { }
				}
			}

			if (targetType == typeof(UnicodeEncoding) && value is byte[]) {
				byte[] bytes = (byte[])value;
				if (bytes.Length <= 1) {
					return String.Empty;
				}

				return Encoding.Unicode.GetString(bytes, 0, bytes.Length - 1);
			}

			if (targetType == typeof(Bitmap) && value is byte[]) {
				byte[] bytes = (byte[])value;
				if (bytes.Length < 1) {
					return null;
				}

				using (MemoryStream stream = new MemoryStream(bytes)) {
					return Bitmap.FromStream(stream);
				}
			}

			return value;
		}

		private void writeMetadata(int propertyId, object value) {
			var property = (from p in this.propItems where p.Id == propertyId select p).FirstOrDefault();

			if (property != null) {
				byte[] buffer;

				switch (property.Type) {
					case 2:
					// ASCII
					case 7:
						// Raw
						buffer = Encoding.ASCII.GetBytes(Convert.ToString(value) + '\0');
						property.Len = buffer.Length;
						property.Value = buffer;
						break;
					case 3:
						if (value == null) {
							property.Len = 0;
							property.Value = new byte[0];
						} else if (value is Array) {
							Array array = value as Array;
							int count = array.Length;
							buffer = new byte[count * Marshal.SizeOf(typeof(uint))];

							for (int i = 0; i < count; i++) {
								byte[] item = BitConverter.GetBytes(Convert.ToUInt32(array.GetValue(i)));
								item.CopyTo(buffer, i * Marshal.SizeOf(typeof(uint)));
							}

							property.Len = buffer.Length;
							property.Value = buffer;
						} else if (value.GetType().IsValueType || value is IConvertible) {
							byte[] data = BitConverter.GetBytes(Convert.ToUInt32(value));

							// property.Len = buffer.Length;
							// property.Value = buffer;
						}
						break;
					case 9:
						// Int32
						if (value == null) {
							property.Len = 0;
							property.Value = new byte[0];
						} else if (value is Array) {
							Array array = value as Array;
							int count = array.Length;
							byte[] data = new byte[count * Int32Size];

							for (int i = 0; i < count; i++) {
								byte[] item = BitConverter.GetBytes(Convert.ToInt32(array.GetValue(i)));
								item.CopyTo(data, i * Int32Size);
							}

							// return data;
						}

						if (value.GetType().IsValueType || value is IConvertible) {
							// return BitConverter.GetBytes(Convert.ToInt32(value));
						}
						break;
					case 4:
						BitConverter.ToInt32(property.Value, 0);
						break;
					case 10:
					case 5:
						int numberator = BitConverter.ToInt32(property.Value, 0);
						int denominator = BitConverter.ToInt32(property.Value, 4);

						if (denominator != 0) {
							// return (double)numberator / (double)denominator;
						} else {
							// return 0;
						}
						break;
					default:
						throw new NotSupportedException("Property type " + property.Type + " is not supported.");
				}

			}
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
		/// Reference http://www.awaresystems.be/imaging/tiff/tifftags/privateifd/exif/exifversion.html
		/// </summary>
		public string ExifVersion {
			get {
				object val = this.QueryMetadata(36864, typeof(string));
				if (val == null) {
					return string.Empty;
				} else {
					return val.ToString();
				}
			}
			set {
				this.writeMetadata(36864, value);
			}
		}

		[ExifDisplay("Label_MeteringMode")]
		public MeteringMode MeteringMode {
			get {
				object val = this.QueryMetadata(37383, typeof(MeteringMode));
				if (val == null) {
					return MeteringMode.Unknown;
				} else {
					return (MeteringMode)(Convert.ToInt32(val));
				}
			}
			set {
				if (value != MeteringMode.Unknown) {
					this.writeMetadata(37383, ((ushort)value).ToString());
				}
			}
		}

		[ExifDisplay("Label_Width", "Label_ExifValue_Pixel")]
		public uint? Width {
			get {
				object val = this.QueryMetadata(40962, typeof(uint?));
				if (val == null) {
					return null;
				} else {
					if (val.GetType() == typeof(UInt32)) {
						return (uint?)val;
					} else {
						return Convert.ToUInt32(val);
					}
				}
			}
			set {
				if (value.HasValue) {
					this.writeMetadata(40962, value.Value.ToString());
				}
			}
		}

		[ExifDisplay("Label_Height", "Label_ExifValue_Pixel")]
		public uint? Height {
			get {
				object val = this.QueryMetadata(40963, typeof(uint?));
				if (val == null) {
					return null;
				} else {
					return Convert.ToUInt32(val);
				}
			}
			set {
				if (value.HasValue) {
					this.writeMetadata(40963, value.Value.ToString());
				}
			}
		}

		[ExifDisplay("Label_HorizontalResolution", "Label_ExifValue_DPI")]
		public decimal? HorizontalResolution {
			get {
				Fraction[] val = this.QueryMetadata(282, typeof(decimal?)) as Fraction[];
				if (val != null && val.Length > 0) {
					return Convert.ToDecimal(val[0].ToDouble());
				} else {
					return null;
				}
			}
			set {
				if (value.HasValue) {
					this.writeMetadata(282, this.createUnsignedRational((uint)value.Value, 1).ToString());
				}
			}
		}

		[ExifDisplay("Label_VerticalResolution", "Label_ExifValue_DPI")]
		public float? VerticalResolution {
			get {
				Fraction[] val = this.QueryMetadata(283, typeof(Fraction)) as Fraction[];
				if (val != null && val.Length > 0) {
					return val[0].ToFloat();
				} else {
					return null;
				}
			}
			set {
				if (value.HasValue) {
					this.writeMetadata(283, this.createUnsignedRational((uint)value.Value, 1).ToString());
				}
			}
		}

		[ExifDisplay("Label_Manufacturer")]
		public string EquipmentManufacturer {
			get {
				object val = this.QueryMetadata(271, typeof(string));
				if (val != null) {
					return val.ToString();
				} else {
					return string.Empty;
				}
			}
			set {
				this.writeMetadata(271, value);
			}
		}

		[ExifDisplay("Label_Camera")]
		public string CameraModel {
			get {
				object val = this.QueryMetadata(272, typeof(string));
				if (val != null) {
					return val.ToString();
				} else {
					return string.Empty;
				}
			}
			set {
				this.writeMetadata(272, value);
			}
		}

		[ExifDisplay("Label_CreationSoftware")]
		public string CreationSoftware {
			get {
				object val = this.QueryMetadata(305, typeof(string));
				if (val != null) {
					return val.ToString();
				} else {
					return string.Empty;
				}
			}
			set {
				this.writeMetadata(305, value);
			}
		}

		[ExifDisplay("Label_ColorRepresentation")]
		public ColorRepresentation ColorRepresentation {
			get {
				object val = this.QueryMetadata(40961, typeof(uint));
				if (val != null) {
					if (Convert.ToUInt16(val) == 1) {
						return ColorRepresentation.sRGB;
					} else {
						return ColorRepresentation.Uncalibrated;
					}
				} else {
					return ColorRepresentation.Uncalibrated;
				}
			}
			set {
				this.writeMetadata(40961, ((ushort)value).ToString());
			}
		}

		[ExifDisplay("Label_ExposureTime", "Label_ExifValue_Second")]
		public Fraction ExposureTime {
			get {
				Fraction[] val = this.QueryMetadata(33434, typeof(Fraction)) as Fraction[];
				if (val != null && val.Length > 0) {
					return val[0];
				} else {
					return null;
				}
			}
			set {
				if (value != null) {
					this.writeMetadata(33434, this.createUnsignedRational((uint)value.Numerator, (uint)value.Denominator).ToString());
				}
			}
		}

		[ExifDisplay("Label_ExposureCompensation")]
		public Fraction ExposureCompensation {
			get {
				Fraction[] val = this.QueryMetadata(37380, typeof(Fraction)) as Fraction[];
				if (val != null && val.Length > 0) {
					return val[0];
				} else {
					return null;
				}
			}
			set {
				if (value != null) {
					this.writeMetadata(37380, this.createSignedRational(value.Numerator, value.Denominator).ToString());
				}
			}
		}

		[ExifDisplay("Label_Aperture", "Label_ExifValue_Aperture")]
		public Fraction LensAperture {
			get {
				Fraction[] val = this.QueryMetadata(33437, typeof(Fraction)) as Fraction[];
				if (val != null && val.Length > 0) {
					return val[0];
				} else {
					return null;
				}
			}
			set {
				if (value != null) {
					this.writeMetadata(33437, this.createUnsignedRational((uint)value.Numerator, (uint)value.Denominator).ToString());
				}
			}
		}

		[ExifDisplay("Label_FocalLength", "Label_ExifValue_MM")]
		public Fraction FocalLength {
			get {
				Fraction[] val = this.QueryMetadata(37386, typeof(Fraction)) as Fraction[];
				if (val != null && val.Length > 0) {
					return val[0];
				} else {
					return null;
				}
			}
			set {
				if (value != null) {
					this.writeMetadata(37386, this.createUnsignedRational((uint)value.Numerator, (uint)value.Denominator).ToString());
				}
			}
		}

		[ExifDisplay("Label_ISOSpeed")]
		public ushort? IsoSpeed {
			get {
				object val = this.QueryMetadata(34855, typeof(ushort));
				if (val != null) {
					return Convert.ToUInt16(val);
				} else {
					return null;
				}
			}
			set {
				if (value.HasValue) {
					this.writeMetadata(34855, value.Value.ToString());
				}
			}
		}

		[ExifDisplay("Label_FlashMode")]
		public FlashMode FlashMode {
			get {
				object o = this.QueryMetadata(37385, typeof(ushort));
				if (o != null) {
					ushort mode;
					if (ushort.TryParse(o.ToString(), out mode)) {
						return (FlashMode)mode;
					} else {
						return FlashMode.Unknown;
					}
				} else {
					return FlashMode.Unknown;
				}
			}
			set {
				if (value != FlashMode.Unknown) {
					this.writeMetadata(37385, ((ushort)value).ToString());
				}
			}
		}

		[ExifDisplay("Label_ExposureMode")]
		public ExposureMode ExposureMode {
			get {
				object val = this.QueryMetadata(34850, typeof(ushort));
				if (val != null) {
					ushort mode = Convert.ToUInt16(val);
					return (ExposureMode)mode;
				} else {
					return ExposureMode.Unknown;
				}
			}
			set {
				if (value != ExposureMode.Unknown) {
					this.writeMetadata(34850, ((ushort)value).ToString());
				}
			}
		}

		[ExifDisplay("Label_LightSource")]
		public LightSourceMode LightSource {
			get {
				object val = this.QueryMetadata(37384, typeof(int));
				if (val != null) {
					return (LightSourceMode)(Convert.ToInt32(val));
				} else {
					return LightSourceMode.Unknown;
				}
			}
			set {
				if (value != LightSourceMode.Unknown) {
					this.writeMetadata(37384, ((ushort)value).ToString());
				}
			}
		}

		[ExifDisplay("Label_DateTaken")]
		public DateTime? DateImageTaken {
			get {
				object val = this.QueryMetadata(36867, typeof(DateTime));
				if (val == null) {
					return null;
				} else {
					return DateTime.Parse(val.ToString());
				}
			}
			set {
				if (value.HasValue) {
					this.writeMetadata(36867, value.Value.ToString("yyyy:MM:dd HH:mm:ss"));
				}
			}
		}

		private GpsLocation latitudeCache = null;

		[ExifDisplay("Label_Latitude")]
		public float? Latitude {
			get {
				if (this.LatitudeRaw != null) {
					return this.latitudeCache.DecimalDegree;
				} else {
					return null;
				}
			}
		}

		public GpsLocation LatitudeRaw {
			get {
				GpsLocation location = null;
				Fraction[] val = this.QueryMetadata(2, typeof(object)) as Fraction[];
				if (val != null && val.Length > 0) {
					uint degree;
					uint? minute = null;
					float? second = null;
					CoordinateDirection direction = CoordinateDirection.TheEquator;

					degree = Convert.ToUInt32(val[0].Numerator);
					if (val.Length > 1) {
						minute = Convert.ToUInt32(val[1].Numerator);
					}
					if (val.Length > 2) {
						second = val[2].ToFloat();
					}

					// N or S
					string latitudeRef = this.QueryMetadata(1, typeof(string)).ToString();
					if (string.Compare(latitudeRef, "S", StringComparison.OrdinalIgnoreCase) == 0) {
						direction = CoordinateDirection.South;
					} else if (string.Compare(latitudeRef, "N", StringComparison.OrdinalIgnoreCase) == 0) {
						direction = CoordinateDirection.North;
					}

					location = new GpsLocation(degree, minute, second, direction);
					latitudeCache = location;
				}

				return location;
			}
			set {
				if (value != null) {
					//this.writeMetadata(2, value.RawCoordinate.ToString());
					//this.writeMetadata(1, value.CoordinateDirection.ToString());
				}
			}
		}

		private GpsLocation longitudeCache;

		[ExifDisplay("Label_Longitude")]
		public float? Longitude {
			get {
				if (this.LongitudeRaw != null) {
					return this.longitudeCache.DecimalDegree;
				} else {
					return null;
				}
			}
		}

		public GpsLocation LongitudeRaw {
			get {
				GpsLocation location = null;
				Fraction[] val = this.QueryMetadata(4, typeof(object)) as Fraction[];
				if (val != null && val.Length > 0) {
					uint degree;
					uint? minute = null;
					float? second = null;
					CoordinateDirection direction = CoordinateDirection.PrimeMeridian;

					degree = Convert.ToUInt32(val[0].Numerator);
					if (val.Length > 1) {
						minute = Convert.ToUInt32(val[1].Numerator);
					}
					if (val.Length > 2) {
						second = val[2].ToFloat();
					}

					// E or W
					string longitudeRef = this.QueryMetadata(3, typeof(string)).ToString();
					if (string.Compare(longitudeRef, "E", StringComparison.OrdinalIgnoreCase) == 0) {
						direction = CoordinateDirection.East;
					} else if (string.Compare(longitudeRef, "W", StringComparison.OrdinalIgnoreCase) == 0) {
						direction = CoordinateDirection.West;
					}

					location = new GpsLocation(degree, minute, second, direction);
					longitudeCache = location;
				}

				return location;
			}
			set {
				if (value != null) {
					//this.writeMetadata(4, value.RawCoordinate.ToString());
					//this.writeMetadata(3, value.CoordinateDirection.ToString());
				}
			}
		}

		[ExifDisplay("Label_Altitude")]
		public decimal? Altitude {
			get {
				Fraction[] val = this.QueryMetadata(6, typeof(Fraction)) as Fraction[];
				if (val != null && val.Length > 0) {
					decimal altitude = Convert.ToDecimal(val[0].ToDouble());

					// 0 = Above Sea Level, 1 = Below Sea Level
					var altitudeRef = this.QueryMetadata(5, typeof(int));

					if (altitudeRef != null && altitudeRef.ToString() == "1") {
						altitude *= -1;
					}
					return altitude;
				} else {
					return null;
				}
			}
			set {
				if (value.HasValue) {
					if (value.Value >= 0) {
						this.writeMetadata(6, this.createUnsignedRational((uint)value.Value, 1).ToString());
						this.writeMetadata(5, "0");
					} else {
						this.writeMetadata(6, this.createUnsignedRational((uint)(value.Value * -1), 1).ToString());
						this.writeMetadata(5, "1");
					}
				}
			}
		}

		public void SaveExif() {
			FileInfo fi = new FileInfo(this.fileName);
			string randomName = Path.Combine(fi.DirectoryName, Guid.NewGuid().ToString() + ".jpg");
			File.Move(this.fileName, randomName);

			using (Image image = new Bitmap(randomName)) {
				foreach (var property in this.propItems) {
					image.SetPropertyItem(property);
				}

				image.Save(this.fileName);
			}

			File.Delete(randomName);
		}
	}
}
