using System;

using HardySoft.UI.BatchImageProcessor.View;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class PreferenceWindow_Presenter {
		private IPreference view;

		public void SetView(IPreference view) {
			this.view = view;
		}
	}
}
