using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace HardySoft.UI.BatchImageProcessor.Classes.Commands {
	public class ApplicationCommand {
		private static RoutedUICommand make;
		private static RoutedUICommand exit;
		private static RoutedUICommand preference;
		private static RoutedUICommand about;
		private static RoutedUICommand discuss;
		private static RoutedUICommand addWatermark;
		private static RoutedUICommand deleteWatermark;

		static ApplicationCommand() {
			// initialize the command
			InputGestureCollection inputs = new InputGestureCollection();
			inputs.Add(new KeyGesture(Key.M, ModifierKeys.Control, "Ctrl+M"));
			make = new RoutedUICommand("Make", "Make", typeof(ApplicationCommand), inputs);

			exit = new RoutedUICommand("Exit", "Exit", typeof(ApplicationCommand));

			preference = new RoutedUICommand("Preference", "Preference", typeof(ApplicationCommand));

			about = new RoutedUICommand("About", "About", typeof(ApplicationCommand));

			discuss = new RoutedUICommand("Discuss", "Discuss", typeof(ApplicationCommand));

			addWatermark = new RoutedUICommand("AddWatermark", "AddWatermark", typeof(ApplicationCommand));

			deleteWatermark = new RoutedUICommand("DeleteWatermark", "DeleteWatermark", typeof(ApplicationCommand));
		}

		public static RoutedUICommand Make {
			get {
				return make;
			}
		}

		public static RoutedUICommand Exit {
			get {
				return exit;
			}
		}

		public static RoutedUICommand Preference {
			get {
				return preference;
			}
		}

		public static RoutedUICommand About {
			get {
				return about;
			}
		}

		public static RoutedUICommand Discuss {
			get {
				return discuss;
			}
		}

		public static RoutedUICommand AddWatermark {
			get {
				return addWatermark;
			}
		}

		public static RoutedUICommand DeleteWatermark {
			get {
				return deleteWatermark;
			}
		}
	}
}
