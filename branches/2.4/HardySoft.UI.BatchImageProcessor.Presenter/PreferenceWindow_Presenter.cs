using System;
using System.Collections.Generic;

using HardySoft.UI.BatchImageProcessor.View;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class PreferenceWindow_Presenter : Presenter<IPreferenceView> {
		public override void SetView(IPreferenceView view) {
			this.View = view;

			this.View.ValidDateTimeFormatStrings = getDateTimeFormatStrings();
		}
	}
}