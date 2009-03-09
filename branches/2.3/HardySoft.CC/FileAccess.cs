using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace HardySoft.CC.File {
	public class FileAccess {
		public static bool AppendFile(string fileName, string content) {
			Mutex m = new Mutex(false, @"Global\" + fileName.Replace(@"\", "_"));
			m.WaitOne();

			try {
				FileInfo fi = new FileInfo(fileName);
				if (! Directory.Exists(fi.DirectoryName)) {
					Directory.CreateDirectory(fi.DirectoryName);
				}
				System.IO.File.AppendAllText(fileName, "\r\n" + content);

				return true;
			} catch (Exception) {
				return false;
			} finally {
				m.ReleaseMutex();
			}
		}
	}
}
