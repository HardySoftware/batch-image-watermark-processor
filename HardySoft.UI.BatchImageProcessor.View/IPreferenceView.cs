using System;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.View {
	public interface IPreferenceView : IView {
		uint ThreadNumber {
			get;
			set;
		}

		Skin ApplicationSkin {
			get;
			set;
		}
	}
}
