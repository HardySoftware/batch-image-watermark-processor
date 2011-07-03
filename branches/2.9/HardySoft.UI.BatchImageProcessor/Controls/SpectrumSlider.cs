using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Ink;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Markup;

namespace HardySoft.UI.BatchImageProcessor.Controls {
	public class SpectrumSlider : Slider {
		static SpectrumSlider() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SpectrumSlider),
				new FrameworkPropertyMetadata(typeof(SpectrumSlider)));
		}

		public Color SelectedColor {
			get {
				return (Color)GetValue(SelectedColorProperty);
			}
			set {
				SetValue(SelectedColorProperty, value);
			}
		}

		public static readonly DependencyProperty SelectedColorProperty =
			DependencyProperty.Register
			("SelectedColor", typeof(Color), typeof(SpectrumSlider),
			new PropertyMetadata(System.Windows.Media.Colors.Transparent));

		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
			m_spectrumDisplay = GetTemplateChild(SpectrumDisplayName) as Rectangle;
			updateColorSpectrum();
			OnValueChanged(Double.NaN, Value);
		}

		protected override void OnValueChanged(double oldValue, double newValue) {
			base.OnValueChanged(oldValue, newValue);
			Color theColor = ColorUtilities.ConvertHsvToRgb(360 - newValue, 1, 1);
			SetValue(SelectedColorProperty, theColor);
		}

		private void updateColorSpectrum() {
			if (m_spectrumDisplay != null) {
				createSpectrum();
			}
		}

		private void createSpectrum() {
			pickerBrush = new LinearGradientBrush();
			pickerBrush.StartPoint = new Point(0.5, 0);
			pickerBrush.EndPoint = new Point(0.5, 1);
			pickerBrush.ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation;

			List<Color> colorsList = ColorUtilities.GenerateHsvSpectrum();
			double stopIncrement = (double)1 / colorsList.Count;

			int i;
			for (i = 0; i < colorsList.Count; i++) {
				pickerBrush.GradientStops.Add(new GradientStop(colorsList[i], i * stopIncrement));
			}

			pickerBrush.GradientStops[i - 1].Offset = 1.0;
			m_spectrumDisplay.Fill = pickerBrush;

		}

		private static string SpectrumDisplayName = "PART_SpectrumDisplay";
		private Rectangle m_spectrumDisplay;
		private LinearGradientBrush pickerBrush;
	}
}