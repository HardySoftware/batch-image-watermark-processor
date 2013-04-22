using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	public class WatermarkCollectionToSelectConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is ObservableCollection<WatermarkBase>) {
				if (value != null) {
					List<WatermarkSelection> watermarks = new List<WatermarkSelection>();
					for (int i = 0; i < ((ObservableCollection<WatermarkBase>)value).Count; i++) {
						WatermarkBase watermark = ((ObservableCollection<WatermarkBase>)value)[i];
						WatermarkSelection ws = new WatermarkSelection {
							Index = i
						};

						if (watermark is WatermarkImage) {
							ws.WatermarkTypeDisplayText = Resources.LanguageContent.Label_Image;
							ws.WatermarkType = "Image";
						} else if (watermark is WatermarkText) {
							ws.WatermarkTypeDisplayText = Resources.LanguageContent.Label_Text;
							ws.WatermarkType = "Text";
						}

						watermarks.Add(ws);
					}

					return watermarks;
				} else {
					return null;
				}
			} else {
				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException("This method should never be called");
		}
	}
}
