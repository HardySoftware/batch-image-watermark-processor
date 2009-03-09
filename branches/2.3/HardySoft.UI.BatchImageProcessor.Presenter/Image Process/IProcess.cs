using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public interface IProcess {
		/// <summary>
		/// Process image in memory.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="ps">Project setting used by process logic to look for detailed settings
		/// related to this module only. Even though application sends the entire settings,
		/// but each proceesing module may have interest in only 1 or limited section(s).</param>
		/// <returns></returns>
		Image ProcessImage(Image input, ProjectSetting ps);

		bool EnableDebug {
			get;
			set;
		}
	}
}