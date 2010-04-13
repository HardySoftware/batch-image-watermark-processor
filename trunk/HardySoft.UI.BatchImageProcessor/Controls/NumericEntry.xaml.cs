using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HardySoft.UI.BatchImageProcessor.Controls {
	/// <summary>
	/// Interaction logic for NumericEntry.xaml,
	/// Code is from http://www.philosophicalgeek.com/2009/11/16/attr-wpf-numeric-entry-control/
	/// </summary>
	public partial class NumericEntry : UserControl {
		private int previousValue = 0;

		private DispatcherTimer timer = new DispatcherTimer();
		private static int delayRate = SystemParameters.KeyboardDelay;
		private static int repeatSpeed = Math.Max(1, SystemParameters.KeyboardSpeed);
		private bool isIncrementing = false;

		public NumericEntry() {
			InitializeComponent();

			timer.Tick += new EventHandler(timer_Tick);
		}

		void timer_Tick(object sender, EventArgs e) {
			if (isIncrementing) {
				incrementValue();
			} else {
				decrementValue();
			}
			timer.Interval = TimeSpan.FromMilliseconds(1000.0 / repeatSpeed);
		}

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
			typeof(Int32), typeof(NumericEntry), new PropertyMetadata(0));
		public Int32 Value {
			get {
				return (Int32)GetValue(ValueProperty);
			}
			set {
				SetValue(ValueProperty, value);
			}
		}

		public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue",
			typeof(Int32), typeof(NumericEntry), new PropertyMetadata(100));
		public Int32 MaxValue {
			get {
				return (Int32)GetValue(MaxValueProperty);
			}
			set {
				SetValue(MaxValueProperty, value);
			}
		}

		public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue",
			typeof(Int32), typeof(NumericEntry), new PropertyMetadata(0));
		public Int32 MinValue {
			get {
				return (Int32)GetValue(MinValueProperty);
			}
			set {
				SetValue(MinValueProperty, value);
			}
		}

		public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register("Increment",
			typeof(Int32), typeof(NumericEntry), new PropertyMetadata(1));
		public Int32 Increment {
			get {
				return (Int32)GetValue(IncrementProperty);
			}
			set {
				SetValue(IncrementProperty, value);
			}
		}

		public static readonly DependencyProperty LargeIncrementProperty = DependencyProperty.Register("LargeIncrement",
			typeof(Int32), typeof(NumericEntry), new PropertyMetadata(5));
		public Int32 LargeIncrement {
			get {
				return (Int32)GetValue(LargeIncrementProperty);
			}
			set {
				SetValue(LargeIncrementProperty, value);
			}
		}

		private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
			if (!HardySoft.CC.Validation.Validators.IsInteger(e.Text)) {
				e.Handled = true;
				return;
			}
		}

		private void NumericTextBox_GotFocus(object sender, RoutedEventArgs e) {
			previousValue = Value;
		}

		private void NumericTextBox_LostFocus(object sender, RoutedEventArgs e) {
			int newValue = 0;
			if (Int32.TryParse(NumericTextBox.Text, out newValue)) {
				if (newValue > MaxValue) {
					newValue = MaxValue;
				} else if (newValue < MinValue) {
					newValue = MinValue;
				}
			} else {
				newValue = previousValue;
			}
			NumericTextBox.Text = newValue.ToString();
		}

		private void NumericTextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
			switch (e.Key) {
				case Key.Up:
					incrementValue();
					break;
				case Key.Down:
					decrementValue();
					break;
				case Key.PageUp:
					Value = Math.Min(Value + LargeIncrement, MaxValue);
					break;
				case Key.PageDown:
					Value = Math.Max(Value - LargeIncrement, MinValue);
					break;
				default:
					//do nothing
					break;
			}
		}

		private void incrementValue() {
			Value = Math.Min(Value + Increment, MaxValue);
		}

		private void decrementValue() {
			Value = Math.Max(Value - Increment, MinValue);
		}

		private void btnIncrement_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			btnIncrement.CaptureMouse();
			timer.Interval = TimeSpan.FromMilliseconds(delayRate * 250);
			timer.Start();
			isIncrementing = true;
		}

		private void btnIncrement_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
			timer.Stop();
			btnIncrement.ReleaseMouseCapture();
			incrementValue();
		}

		private void btnDecrement_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			btnDecrement.CaptureMouse();
			timer.Interval = TimeSpan.FromMilliseconds(delayRate * 250);
			timer.Start();
			isIncrementing = false;
		}

		private void btnDecrement_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
			timer.Stop();
			btnDecrement.ReleaseMouseCapture();
			decrementValue();
		}
	}
}
