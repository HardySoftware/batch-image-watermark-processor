using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace OpenXMLWriter {
	public class ApplicationCommand {
		private static RoutedUICommand viewXAML;

		static ApplicationCommand() {
			// initialize the command
			viewXAML = new RoutedUICommand("View XAML", "ViewXAML", typeof(ApplicationCommand));
		}

		public static RoutedUICommand ViewXAML {
			get {
				return viewXAML;
			}
		}
	}
}
