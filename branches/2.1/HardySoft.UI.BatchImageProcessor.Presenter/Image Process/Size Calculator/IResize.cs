using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	interface IResize {
		Size CalculateNewSize(Size originalSize, double newSize);
	}
}
