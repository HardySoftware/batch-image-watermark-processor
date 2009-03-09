using System;

using HardySoft.UI.BatchImageProcessor.View;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class PreferenceWindow_Presenter : Presenter<IPreferenceView> {
		public override void SetView(IPreferenceView view) {
			this.View = view;
		}
	}
}
