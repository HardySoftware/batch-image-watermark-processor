using System.Globalization;

namespace HardySoft.UI.BatchImageProcessor.View {
	public interface IConfiguration {
		CultureInfo ApplicationLanguage {
			get;
			set;
		}
	}
}
