using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardySoft.UI.BatchImageProcessor.Controls {
	struct HsvColor {
		public double H;
		public double S;
		public double V;

		public HsvColor(double h, double s, double v) {
			this.H = h;
			this.S = s;
			this.V = v;
		}
	}
}
