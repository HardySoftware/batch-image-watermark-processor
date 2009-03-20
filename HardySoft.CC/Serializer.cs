using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace HardySoft.CC.Transformer {
	public class Serializer {
		public static string Serialize<T>(T obj) {
			try {
				String s = null;

				MemoryStream memoryStream = new MemoryStream();
				XmlSerializer xs = new XmlSerializer(typeof(T));
				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
				xs.Serialize(xmlTextWriter, obj);
				memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
				s = Converter.StringConverter.UTF8ByteArrayToString(memoryStream.ToArray());

				return s;
			} catch (Exception) {
				return string.Empty;
			}
		}

		public static T DeSerialize<T>(string xml) {
			try {
				XmlSerializer xs = new XmlSerializer(typeof(T));
				MemoryStream memoryStream = new MemoryStream(Converter.StringConverter.StringToUTF8ByteArray(xml));
				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
				return (T)xs.Deserialize(memoryStream);
			} catch (Exception) {
				return default(T);
			}
		}
	}
}