using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media;

namespace HardySoft.CC.Converter {
	public class FontConverter {
		/*public static string ToBase64String(Font font) {
			try {
				using (MemoryStream stream = new MemoryStream()) {
					BinaryFormatter formatter = new BinaryFormatter();
					formatter.Serialize(stream, font);
					return Convert.ToBase64String(stream.ToArray());
				}
			} catch {
				return null;
			}
		}

		public static Font FromBase64String(string font) {
			try {
				using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(font))) {
					BinaryFormatter formatter = new BinaryFormatter();
					return (Font)formatter.Deserialize(stream);
				}
			} catch {
				return null;
			}
		} */
	}
}
