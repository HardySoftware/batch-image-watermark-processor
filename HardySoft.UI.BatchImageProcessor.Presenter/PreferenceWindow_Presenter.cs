using System;
using System.Collections.Generic;

using HardySoft.UI.BatchImageProcessor.View;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class PreferenceWindow_Presenter : Presenter<IPreferenceView> {
		public override void SetView(IPreferenceView view) {
			this.View = view;

			this.View.ValidDateTimeFormatStrings = getDateTimeFormatStrings();
		}

		private Dictionary<string, string> getDateTimeFormatStrings() {
			Dictionary<string, string> d = new Dictionary<string, string>();
			d.Add("d", "Label_DatePattern_ShortDatePattern");
			d.Add("D", "Label_DatePattern_LongDatePattern");
			d.Add("f", "Label_DatePattern_FullDatetimeShortTimePattern");
			d.Add("F", "Label_DatePattern_FullDatetimeLongTimePattern");
			d.Add("g", "Label_DatePattern_GeneralDatetimeShortTimePattern");
			d.Add("G", "Label_DatePattern_GeneralDatetimeLongTimePattern");
			d.Add("M", "Label_DatePattern_MonthDayPattern");
			d.Add("t", "Label_DatePattern_ShortTimePattern");
			d.Add("T", "Label_DatePattern_LongTimePattern");
			d.Add("Y", "Label_DatePattern_YearMonthPattern");

			return d;
		}
	}
}