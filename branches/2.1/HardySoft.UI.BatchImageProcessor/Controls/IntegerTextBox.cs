using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace HardySoft.UI.BatchImageProcessor.Controls {
	public class IntegerTextBox : TextBox {
		protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e) {
			e.Handled = !areAllValidNumericChars(e.Text);
			base.OnPreviewTextInput(e);
		}

		private bool areAllValidNumericChars(string str) {
			bool ret = true;
			/*string[] allowedSigns = new string[] {
				System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator,
				System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator,
				System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol,
				System.Globalization.NumberFormatInfo.CurrentInfo.NegativeSign,
				System.Globalization.NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol,
				System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator,
				System.Globalization.NumberFormatInfo.CurrentInfo.NumberGroupSeparator,
				System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalSeparator,
				System.Globalization.NumberFormatInfo.CurrentInfo.PercentGroupSeparator,
				System.Globalization.NumberFormatInfo.CurrentInfo.PercentSymbol,
				System.Globalization.NumberFormatInfo.CurrentInfo.PerMilleSymbol,
				System.Globalization.NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol,
				System.Globalization.NumberFormatInfo.CurrentInfo.PositiveSign
			};
			if (str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator |
				str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator |
				str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol |
				str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeSign |
				str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol |
				str == System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator |
				str == System.Globalization.NumberFormatInfo.CurrentInfo.NumberGroupSeparator |
				str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalSeparator |
				str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentGroupSeparator |
				str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentSymbol |
				str == System.Globalization.NumberFormatInfo.CurrentInfo.PerMilleSymbol |
				str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol |
				str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveSign)
				return ret;*/

			int l = str.Length;
			for (int i = 0; i < l; i++) {
				char ch = str[i];
				ret &= Char.IsDigit(ch);
			}

			return ret;
		}
	}
}
