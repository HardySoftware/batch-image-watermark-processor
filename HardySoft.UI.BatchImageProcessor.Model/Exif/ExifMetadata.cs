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
		/// All propertyItem tags http://msdn.microsoft.com/en-us/library/ms534417%28v=vs.85%29.aspx
		/// </remarks>
		private PropertyItem[] propItems;
		private string fileName;

		private static readonly int UInt16Size = Marshal.SizeOf(typeof(ushort));
		private static readonly int Int32Size = Marshal.SizeOf(typeof(int));
		private static readonly int UInt32Size = Marshal.SizeOf(typeof(uint));
		private static readonly int RationalSize = 2 * Marshal.SizeOf(typeof(int));
		private static readonly int URationalSize = 2 * Marshal.SizeOf(typeof(uint));

		public ExifMetadata(Uri imageUri) {
			this.fileName = imageUri.AbsolutePath;
			using (FileStream stream = new FileStream(this.fileName, FileMode.Open)) {
				using (Image image = Image.FromStream(stream)) {
					this.propItems = image.PropertyItems;
				}
			}
		}

		/// <summary>
		/// Queries the image's propertyItem item list to get value of the designated propertyItem Id.
		/// </summary>
		/// <typeparam name="T">The type of the value should be.</typeparam>
		/// <param name="propertyId">The propertyItem Id to query by.</param>
		/// <returns></returns>
		private object QueryMetadata<T>(int propertyId) {
			var property = (from p in this.propItems where p.Id == propertyId select p).FirstOrDefault();

			if (property != null) {
				object o = this.FromPropertyItem(property);
				return this.ConvertData<T>(o);
			}

			return null;
		}

		/// <summary>
		/// Converts a propertyItem item to an object or array of objects.
		/// </summary>
		/// <param name="propertyItem">the propertyItem item to convert</param>
		/// <returns>the propertyItem value</returns>
		private object FromPropertyItem(PropertyItem propertyItem) {
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

			return data;
		}

		/// <summary>
		/// Convert data from propertyItem item into actual value.
		/// </summary>
		/// <typeparam name="T">The type of the value should be.</typeparam>
		/// <param name="targetType"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private object ConvertData<T>(object value) {
			if (typeof(T) == null || value == null) {
				return value;
			}

			if (typeof(T) == typeof(Fraction) || typeof(T) == typeof(Fraction[])) {
				return value;
			}

			if (value is Array) {
				if (typeof(T) == typeof(string)) {
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

			if (typeof(T) == typeof(DateTime) && value is String) {
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

			if (typeof(T).IsEnum) {
				Type underlyingType = Enum.GetUnderlyingType(typeof(T));
				if (value.GetType() != underlyingType) {
					value = Convert.ChangeType(value, underlyingType);
				}

				if (Enum.IsDefined(typeof(T), value) || FlagsAttribute.IsDefined(typeof(T), typeof(FlagsAttribute))) {
					try {
						return Enum.ToObject(typeof(T), value);
					} catch { }
				}
			}

			if (typeof(T) == typeof(UnicodeEncoding) && value is byte[]) {
				byte[] bytes = (byte[])value;
				if (bytes.Length <= 1) {
					return String.Empty;
				}

				return Encoding.Unicode.GetString(bytes, 0, bytes.Length - 1);
			}

			if (typeof(T) == typeof(Bitmap) && value is byte[]) {
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

		/// <summary>
		/// Convert actual value into byte array to save with image.
		/// </summary>
		/// <param name="dataType"></param>
		/// <param name="targetType"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private byte[] ConvertData<T>(ExifType targetType, object value) {
			if (value == null) {
				return new byte[0];
			}

			switch (targetType) {
				case ExifType.Ascii: {
						return Encoding.ASCII.GetBytes(Convert.ToString(value) + '\0');
					}
				case ExifType.Byte:
				case ExifType.Raw: {
						if (typeof(T) == typeof(UnicodeEncoding)) {
							return Encoding.Unicode.GetBytes(Convert.ToString(value) + '\0');
						}

						if (typeof(T) == typeof(string)) {
							return Encoding.ASCII.GetBytes(Convert.ToString(value));
						}

						if (value is Array) {
							Array array = value as Array;
							int count = array.Length;
							byte[] data = new byte[count];

							for (int i = 0; i < count; i++) {
								data[i] = Convert.ToByte(array.GetValue(i));
							}

							return data;
						}

						if (value.GetType().IsValueType || value is IConvertible) {
							return new byte[] { Convert.ToByte(value) };
						}

						throw new ArgumentException(String.Format("Error converting {0} to byte[].", value.GetType().Name));
					}
				case ExifType.Int32: {
						if (value is Array) {
							Array array = value as Array;
							int count = array.Length;
							byte[] data = new byte[count * Int32Size];

							for (int i = 0; i < count; i++) {
								byte[] item = BitConverter.GetBytes(Convert.ToInt32(array.GetValue(i)));
								item.CopyTo(data, i * Int32Size);
							}

							return data;
						}

						if (value.GetType().IsValueType || value is IConvertible) {
							return BitConverter.GetBytes(Convert.ToInt32(value));
						}

						throw new ArgumentException(String.Format("Error converting {0} to Int32[].", value.GetType().Name));
					}
				case ExifType.Rational: {
						byte[] data;

						if (value is Array) {
							Array array = value as Array;
							int count = array.Length;
							data = new byte[count * RationalSize];

							for (int i = 0; i < count; i++) {
								Fraction item = (Fraction)Convert.ChangeType(array.GetValue(i), typeof(Fraction));
								BitConverter.GetBytes(Convert.ToInt32(item.Numerator)).CopyTo(data, i * RationalSize);
								BitConverter.GetBytes(Convert.ToInt32(item.Denominator)).CopyTo(data, i * RationalSize + Int32Size);
							}

							return data;
						}

						Fraction singleItem = (Fraction)Convert.ChangeType(value, typeof(Fraction));
						data = new byte[RationalSize];
						BitConverter.GetBytes(Convert.ToInt32(singleItem.Numerator)).CopyTo(data, 0);
						BitConverter.GetBytes(Convert.ToInt32(singleItem.Denominator)).CopyTo(data, Int32Size);
						return data;
					}
				case ExifType.UInt16: {
						if (value is Array) {
							Array array = value as Array;
							int count = array.Length;
							byte[] data = new byte[count * UInt16Size];

							for (int i = 0; i < count; i++) {
								byte[] item = BitConverter.GetBytes(Convert.ToUInt16(array.GetValue(i)));
								item.CopyTo(data, i * UInt16Size);
							}

							return data;
						}

						if (value.GetType().IsValueType || value is IConvertible) {
							return BitConverter.GetBytes(Convert.ToUInt16(value));
						}

						throw new ArgumentException(String.Format("Error converting {0} to UInt16[].", value.GetType().Name));
					}
				case ExifType.UInt32: {
						if (value is Array) {
							Array array = value as Array;
							int count = array.Length;
							byte[] data = new byte[count * UInt32Size];

							for (int i = 0; i < count; i++) {
								byte[] item = BitConverter.GetBytes(Convert.ToUInt32(array.GetValue(i)));
								item.CopyTo(data, i * UInt32Size);
							}

							return data;
						}

						if (value.GetType().IsValueType || value is IConvertible) {
							return BitConverter.GetBytes(Convert.ToUInt32(value));
						}

						throw new ArgumentException(String.Format("Error converting {0} to UInt32[].", value.GetType().Name));
					}
				case ExifType.URational: {
						byte[] data;
						if (value is Array) {
							Array array = value as Array;
							int count = array.Length;
							data = new byte[count * URationalSize];

							for (int i = 0; i < count; i++) {
								Fraction item = (Fraction)Convert.ChangeType(array.GetValue(i), typeof(Fraction));
								BitConverter.GetBytes(Convert.ToInt32(item.Numerator)).CopyTo(data, i * URationalSize);
								BitConverter.GetBytes(Convert.ToInt32(item.Denominator)).CopyTo(data, i * URationalSize + UInt32Size);
							}

							return data;
						}

						Fraction singleItem = (Fraction)Convert.ChangeType(value, typeof(Fraction));

						data = new byte[RationalSize];
						BitConverter.GetBytes(Convert.ToInt32(singleItem.Numerator)).CopyTo(data, 0);
						BitConverter.GetBytes(Convert.ToInt32(singleItem.Denominator)).CopyTo(data, UInt32Size);

						return data;
					}
				default: {
						throw new NotImplementedException(String.Format("Encoding for EXIF type \"{0}\" has not yet been implemented.", targetType));
					}
			}
		}

		private string ConvertLatitudeLongitudeDirection(CoordinateDirection direction) {
			switch (direction) {
				case CoordinateDirection.East:
					return "E";
				case CoordinateDirection.North:
					return "N";
				case CoordinateDirection.South:
					return "S";
				case CoordinateDirection.West:
					return "W";
				default:
					return string.Empty;
			}
		}

		private void WriteMetadata<T>(int propertyId, object value) {
			var propertyItem = (from p in this.propItems where p.Id == propertyId select p).FirstOrDefault();

			if (propertyItem != null) {
				// the item is currently being used by the image.
				ExifType exifType = (ExifType)propertyItem.Type;

				byte[] buffer = this.ConvertData<T>(exifType, value);
				propertyItem.Len = buffer.Length;
				propertyItem.Value = buffer;
			} else {
				// it is a brand new item to the image.
				Trace.TraceWarning("Property Id {0} is not found in the image, the value setting will be ignored.", propertyId);
			}
		}

		/// <summary>
		/// Reference http://www.awaresystems.be/imaging/tiff/tifftags/privateifd/exif/exifversion.html
		/// </summary>
		public string ExifVersion {
			get {
				object val = this.QueryMetadata<string>(36864);
				if (val == null) {
					return string.Empty;
				} else {
					return val.ToString();
				}
			}
			set {
				this.WriteMetadata<string>(36864, value);
			}
		}

		[ExifDisplay("Label_MeteringMode")]
		public MeteringMode MeteringMode {
			get {
				object val = this.QueryMetadata<int>(37383);
				if (val == null) {
					return MeteringMode.Unknown;
				} else {
					return (MeteringMode)(Convert.ToInt32(val));
				}
			}
			set {
				if (value != MeteringMode.Unknown) {
					this.WriteMetadata<int>(37383, (int)value);
				} else {
					this.WriteMetadata<int>(37383, null);
				}
			}
		}

		[ExifDisplay("Label_Width", "Label_ExifValue_Pixel")]
		public uint? Width {
			get {
				object val = this.QueryMetadata<uint>(40962);
				if (val == null) {
					return null;
				} else {
					return Convert.ToUInt32(val);
				}
			}
			set {
				if (value.HasValue) {
					this.WriteMetadata<int?>(40962, value.Value);
				} else {
					this.WriteMetadata<int?>(40962, null);
				}
			}
		}

		[ExifDisplay("Label_Height", "Label_ExifValue_Pixel")]
		public uint? Height {
			get {
				object val = this.QueryMetadata<uint>(40963);
				if (val == null) {
					return null;
				} else {
					return Convert.ToUInt32(val);
				}
			}
			set {
				if (value.HasValue) {
					this.WriteMetadata<uint?>(40963, value.Value);
				} else {
					this.WriteMetadata<uint?>(40963, null);
				}
			}
		}

		[ExifDisplay("Label_HorizontalResolution", "Label_ExifValue_DPI")]
		public float? HorizontalResolution {
			get {
				Fraction[] val = this.QueryMetadata<Fraction[]>(282) as Fraction[];
				if (val != null && val.Length > 0) {
					return val[0].ToFloat();
				} else {
					return null;
				}
			}
			set {
				if (value.HasValue) {
					Fraction f = new Fraction(value.Value);
					this.WriteMetadata<Fraction>(282, f);
				} else {
					this.WriteMetadata<Fraction>(282, null);
				}
			}
		}

		[ExifDisplay("Label_VerticalResolution", "Label_ExifValue_DPI")]
		public float? VerticalResolution {
			get {
				Fraction[] val = this.QueryMetadata<Fraction[]>(283) as Fraction[];
				if (val != null && val.Length > 0) {
					return val[0].ToFloat();
				} else {
					return null;
				}
			}
			set {
				if (value.HasValue) {
					Fraction f = new Fraction(value.Value);
					this.WriteMetadata<Fraction>(283, f);
				} else {
					this.WriteMetadata<Fraction>(283, null);
				}
			}
		}

		[ExifDisplay("Label_Manufacturer")]
		public string EquipmentManufacturer {
			get {
				object val = this.QueryMetadata<string>(271);
				if (val != null) {
					return val.ToString();
				} else {
					return string.Empty;
				}
			}
			set {
				this.WriteMetadata<string>(271, value);
			}
		}

		[ExifDisplay("Label_Camera")]
		public string CameraModel {
			get {
				object val = this.QueryMetadata<string>(272);
				if (val != null) {
					return val.ToString();
				} else {
					return string.Empty;
				}
			}
			set {
				this.WriteMetadata<string>(272, value);
			}
		}

		[ExifDisplay("Label_CreationSoftware")]
		public string CreationSoftware {
			get {
				object val = this.QueryMetadata<string>(305);
				if (val != null) {
					return val.ToString();
				} else {
					return string.Empty;
				}
			}
			set {
				this.WriteMetadata<string>(305, value);
			}
		}

		[ExifDisplay("Label_ColorRepresentation")]
		public ColorRepresentation ColorRepresentation {
			get {
				object val = this.QueryMetadata<uint>(40961);
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
				uint val = value == ColorRepresentation.sRGB ? 1u : 0u;
				this.WriteMetadata<uint>(40961, val);
			}
		}

		[ExifDisplay("Label_ExposureTime", "Label_ExifValue_Second")]
		public Fraction ExposureTime {
			get {
				Fraction[] val = this.QueryMetadata<Fraction[]>(33434) as Fraction[];
				if (val != null && val.Length > 0) {
					return val[0];
				} else {
					return null;
				}
			}
			set {
				if (value != null) {
					this.WriteMetadata<Fraction>(33434, value);
				} else {
					this.WriteMetadata<Fraction>(33434, null);
				}
			}
		}

		[ExifDisplay("Label_ExposureCompensation")]
		public Fraction ExposureCompensation {
			get {
				Fraction[] val = this.QueryMetadata<Fraction[]>(37380) as Fraction[];
				if (val != null && val.Length > 0) {
					return val[0];
				} else {
					return null;
				}
			}
			set {
				if (value != null) {
					this.WriteMetadata<Fraction>(37380, value);
				} else {
					this.WriteMetadata<Fraction>(37380, null);
				}
			}
		}

		[ExifDisplay("Label_Aperture", "Label_ExifValue_Aperture")]
		public Fraction LensAperture {
			get {
				Fraction[] val = this.QueryMetadata<Fraction[]>(33437) as Fraction[];
				if (val != null && val.Length > 0) {
					return val[0];
				} else {
					return null;
				}
			}
			set {
				if (value != null) {
					this.WriteMetadata<Fraction>(33437, value);
				} else {
					this.WriteMetadata<Fraction>(33437, null);
				}
			}
		}

		[ExifDisplay("Label_FocalLength", "Label_ExifValue_MM")]
		public Fraction FocalLength {
			get {
				Fraction[] val = this.QueryMetadata<Fraction[]>(37386) as Fraction[];
				if (val != null && val.Length > 0) {
					return val[0];
				} else {
					return null;
				}
			}
			set {
				if (value != null) {
					this.WriteMetadata<Fraction>(37386, value);
				} else {
					this.WriteMetadata<Fraction>(37386, null);
				}
			}
		}

		[ExifDisplay("Label_ISOSpeed")]
		public ushort? IsoSpeed {
			get {
				object val = this.QueryMetadata<ushort>(34855);
				if (val != null) {
					return Convert.ToUInt16(val);
				} else {
					return null;
				}
			}
			set {
				if (value.HasValue) {
					this.WriteMetadata<ushort>(34855, value.Value);
				} else {
					this.WriteMetadata<ushort>(34855, null);
				}
			}
		}

		[ExifDisplay("Label_FlashMode")]
		public FlashMode FlashMode {
			get {
				object o = this.QueryMetadata<ushort>(37385);
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
					this.WriteMetadata<ushort>(37385, (ushort)value);
				} else {
					this.WriteMetadata<ushort>(37385, null);
				}
			}
		}

		[ExifDisplay("Label_ExposureMode")]
		public ExposureMode ExposureMode {
			get {
				object val = this.QueryMetadata<ushort>(34850);
				if (val != null) {
					ushort mode = Convert.ToUInt16(val);
					return (ExposureMode)mode;
				} else {
					return ExposureMode.Unknown;
				}
			}
			set {
				if (value != ExposureMode.Unknown) {
					this.WriteMetadata<ushort>(34850, (ushort)value);
				} else {
					this.WriteMetadata<ushort>(34850, null);
				}
			}
		}

		[ExifDisplay("Label_LightSource")]
		public LightSourceMode LightSource {
			get {
				object val = this.QueryMetadata<int>(37384);
				if (val != null) {
					return (LightSourceMode)(Convert.ToInt32(val));
				} else {
					return LightSourceMode.None;
				}
			}
			set {
				if (value != LightSourceMode.None) {
					this.WriteMetadata<int>(37384, (int)value);
				} else {
					this.WriteMetadata<int>(37384, null);
				}
			}
		}

		[ExifDisplay("Label_DateTaken")]
		public DateTime? DateImageTaken {
			get {
				object val = this.QueryMetadata<DateTime>(36867);
				if (val == null) {
					return null;
				} else {
					return DateTime.Parse(val.ToString());
				}
			}
			set {
				if (value.HasValue) {
					this.WriteMetadata<string>(36867, value.Value.ToString("yyyy:MM:dd HH:mm:ss"));
				} else {
					this.WriteMetadata<string>(36867, null);
				}
			}
		}

		[ExifDisplay("Label_Latitude")]
		public GeographicCoordinate Latitude {
			get {
				GeographicCoordinate location = null;
				Fraction[] val = this.QueryMetadata<Fraction[]>(2) as Fraction[];
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
					string latitudeRef = this.QueryMetadata<string>(1).ToString();
					if (string.Compare(latitudeRef, "S", StringComparison.OrdinalIgnoreCase) == 0) {
						direction = CoordinateDirection.South;
					} else if (string.Compare(latitudeRef, "N", StringComparison.OrdinalIgnoreCase) == 0) {
						direction = CoordinateDirection.North;
					}

					location = new GeographicCoordinate(degree, minute, second, direction);
				}

				return location;
			}
			set {
				if (value != null) {
					if (value.CoordinateDirection != CoordinateDirection.North
						&& value.CoordinateDirection != CoordinateDirection.South
						&& value.CoordinateDirection != CoordinateDirection.TheEquator) {
							throw new ArgumentException(string.Format("Invalid CoordinateDirection {0} provided", value.CoordinateDirection));
					}

					if (value.CoordinateType != CoordinateType.Latitude) {
						throw new ArgumentException(string.Format("Invalid CoordinateType {0} provided", value.CoordinateType));
					}

					Fraction[] values = new Fraction[3];
					values[0] = new Fraction(value.Degree, 1);
					values[1] = new Fraction(value.Minute.HasValue ? value.Minute.Value : 0, 1);
					values[2] = new Fraction(value.Second.HasValue ? value.Second.Value : 0);
					this.WriteMetadata<Fraction[]>(2, values);
					this.WriteMetadata<string>(1, this.ConvertLatitudeLongitudeDirection(value.CoordinateDirection));
				} else {
					this.WriteMetadata<Fraction[]>(2, null);
					this.WriteMetadata<string>(1, null);
				}
			}
		}

		[ExifDisplay("Label_Longitude")]
		public GeographicCoordinate Longitude {
			get {
				GeographicCoordinate location = null;
				Fraction[] val = this.QueryMetadata<Fraction[]>(4) as Fraction[];
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
					string longitudeRef = this.QueryMetadata<string>(3).ToString();
					if (string.Compare(longitudeRef, "E", StringComparison.OrdinalIgnoreCase) == 0) {
						direction = CoordinateDirection.East;
					} else if (string.Compare(longitudeRef, "W", StringComparison.OrdinalIgnoreCase) == 0) {
						direction = CoordinateDirection.West;
					}

					location = new GeographicCoordinate(degree, minute, second, direction);
				}

				return location;
			}
			set {
				if (value != null) {
					if (value.CoordinateDirection != CoordinateDirection.East
						&& value.CoordinateDirection != CoordinateDirection.West
						&& value.CoordinateDirection != CoordinateDirection.PrimeMeridian) {
						throw new ArgumentException(string.Format("Invalid CoordinateDirection {0} provided", value.CoordinateDirection));
					}

					if (value.CoordinateType != CoordinateType.Longitude) {
						throw new ArgumentException(string.Format("Invalid CoordinateType {0} provided", value.CoordinateType));
					}

					Fraction[] values = new Fraction[3];
					values[0] = new Fraction(value.Degree, 1);
					values[1] = new Fraction(value.Minute.HasValue ? value.Minute.Value : 0, 1);
					values[2] = new Fraction(value.Second.HasValue ? value.Second.Value : 0);
					this.WriteMetadata<Fraction[]>(4, values);
					this.WriteMetadata<string>(3, this.ConvertLatitudeLongitudeDirection(value.CoordinateDirection));
				} else {
					this.WriteMetadata<Fraction[]>(4, null);
					this.WriteMetadata<string>(3, null);
				}
			}
		}

		[ExifDisplay("Label_Altitude")]
		public float? Altitude {
			get {
				Fraction[] val = this.QueryMetadata<Fraction>(6) as Fraction[];
				if (val != null && val.Length > 0) {
					float altitude = val[0].ToFloat();

					// 0 = Above Sea Level, 1 = Below Sea Level
					var altitudeRef = this.QueryMetadata<int>(5);

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
						this.WriteMetadata<string>(5, "0");
					} else {
						this.WriteMetadata<string>(5, "1");
					}

					this.WriteMetadata<Fraction>(6, new Fraction(Math.Abs(value.Value)));
				} else {
					this.WriteMetadata<string>(5, null);
					this.WriteMetadata<Fraction>(6, null);
				}
			}
		}

		public ExifOrientation Orientation {
			get {
				object val = this.QueryMetadata<ushort>(274);
				if (val != null) {
					ushort mode = Convert.ToUInt16(val);
					return (ExifOrientation)mode;
				} else {
					return ExifOrientation.Unknown;
				}
			}
			set {
				if (value != ExifOrientation.Unknown) {
					this.WriteMetadata<ushort>(274, (ushort)value);
				} else {
					this.WriteMetadata<ushort>(274, null);
				}
			}
		}

		public void SaveExif() {
			FileInfo fi = new FileInfo(this.fileName);
			string randomName = Path.Combine(fi.DirectoryName, fi.Name + Guid.NewGuid().ToString() + ".jpg");
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
