using System;
using System.Threading;
using System.Windows.Markup;
using System.Windows.Media;

namespace HardySoft.UI.BatchImageProcessor.Classes.MarkExtensions {
	public class ResourceFontExtension : MarkupExtension {
		private readonly string key;

		public ResourceFontExtension(string key) {
			if (key == null) {
				throw new ArgumentNullException("key");
			}
			if (key == String.Empty) {
				throw new ArgumentException("Argument key cannot be empty.");
			}
			this.key = key;
		}

		public override object ProvideValue(IServiceProvider serviceProvider) {
			string fontName = Resources.LanguageContent.ResourceManager.GetString(key, Thread.CurrentThread.CurrentCulture);

			FontFamily font = new FontFamily(fontName);
			return font;
		}
	}
}
