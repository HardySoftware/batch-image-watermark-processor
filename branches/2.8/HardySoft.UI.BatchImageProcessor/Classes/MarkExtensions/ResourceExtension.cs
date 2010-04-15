using System;
using System.Threading;
using System.Windows.Markup;

namespace HardySoft.UI.BatchImageProcessor.Classes.MarkExtensions {
	public class ResourceExtension : MarkupExtension {
		private readonly string key;

		public ResourceExtension(string key) {
			if (key == null) {
				throw new ArgumentNullException("key");
			}
			if (key == String.Empty) {
				throw new ArgumentException("Argument key cannot be empty.");
			}
			this.key = key;
		}

		public override object ProvideValue(IServiceProvider serviceProvider) {
			return Resources.LanguageContent.ResourceManager.GetString(key, Thread.CurrentThread.CurrentCulture);
		}
	}
}
