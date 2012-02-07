using System;
using System.Collections.Generic;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.View {
	public interface IPreferenceView : IView {
		Skin ApplicationSkin {
			get;
			set;
		}

		string DateTimeFormatString {
			get;
			set;
		}

		Dictionary<string, string> ValidDateTimeFormatStrings {
			get;
			set;
		}
	}
}
