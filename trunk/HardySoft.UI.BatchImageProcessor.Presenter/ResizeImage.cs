using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Diagnostics;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public abstract class ResizeImage : IProcess {
		public abstract Image ProcessImage(Image input, ProjectSetting ps);

		protected Image ResizeImageJob(Image input, uint size) {
			Random rnd = new Random(DateTime.Now.Millisecond);
			Thread.Sleep(rnd.Next(500, 1500));
			string message = string.Format("Thread name: {0}", Thread.CurrentThread.Name);
			Debug.WriteLine(message);

			return input;
		}
	}
}
