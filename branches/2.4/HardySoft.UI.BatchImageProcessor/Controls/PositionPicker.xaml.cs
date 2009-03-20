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
using System.Drawing;

namespace HardySoft.UI.BatchImageProcessor.Controls {
	/// <summary>
	/// Interaction logic for PositionPicker.xaml
	/// </summary>
	public partial class PositionPicker : UserControl {
		public PositionPicker() {
			InitializeComponent();
		}

		public static readonly DependencyProperty SelectedPositionProperty = DependencyProperty.Register("SelectedPosition",
			typeof(ContentAlignment), typeof(PositionPicker),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnPositionSelectionChanged)));

		public static readonly DependencyProperty CornerOnlyProperty = DependencyProperty.Register("CornerOnly",
			typeof(bool), typeof(PositionPicker),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnCornerOnlyChanged)));

		public ContentAlignment SelectedPosition {
			get {
				return (ContentAlignment)GetValue(SelectedPositionProperty);
			}
			set {
				SetValue(SelectedPositionProperty, value);
			}
		}

		public bool CornerOnly {
			get {
				return (bool)GetValue(CornerOnlyProperty);
			}
			set {
				SetValue(CornerOnlyProperty, value);
			}
		}

		private static void OnPositionSelectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			PositionPicker pp = (PositionPicker)sender;

			ContentAlignment oldPosition = (ContentAlignment)e.OldValue;
			ContentAlignment newPosition = (ContentAlignment)e.NewValue;

			pp.TopLeft.IsChecked = false;
			pp.TopCenter.IsChecked = false;
			pp.TopRight.IsChecked = false;
			pp.MiddleLeft.IsChecked = false;
			pp.MiddleCenter.IsChecked = false;
			pp.MiddleRight.IsChecked = false;
			pp.BottomLeft.IsChecked = false;
			pp.BottomCenter.IsChecked = false;
			pp.BottomRight.IsChecked = false;

			if (newPosition == ContentAlignment.TopLeft) {
				pp.TopLeft.IsChecked = true;
			} else if (newPosition == ContentAlignment.TopCenter) {
				pp.TopCenter.IsChecked = true;
			} else if (newPosition == ContentAlignment.TopRight) {
				pp.TopRight.IsChecked = true;
			} else if (newPosition == ContentAlignment.MiddleLeft) {
				pp.MiddleLeft.IsChecked = true;
			} else if (newPosition == ContentAlignment.MiddleCenter) {
				pp.MiddleCenter.IsChecked = true;
			} else if (newPosition == ContentAlignment.MiddleRight) {
				pp.MiddleRight.IsChecked = true;
			} else if (newPosition == ContentAlignment.BottomLeft) {
				pp.BottomLeft.IsChecked = true;
			} else if (newPosition == ContentAlignment.BottomCenter) {
				pp.BottomCenter.IsChecked = true;
			} else if (newPosition == ContentAlignment.BottomRight) {
				pp.BottomRight.IsChecked = true;
			}

			pp.OnPositionSelectionChanged(oldPosition, newPosition);
		}

		private static void OnCornerOnlyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			PositionPicker pp = (PositionPicker)sender;

			bool oldValue = (bool)e.OldValue;
			bool newValue = (bool)e.NewValue;

			pp.OnCornerOnlyChanged(oldValue, newValue);
		}

		public static readonly RoutedEvent PositionSelectionChangedEvent =
		   EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble,
		   typeof(RoutedPropertyChangedEventHandler<ContentAlignment>), typeof(PositionPicker));

		public static readonly RoutedEvent CornerOnlyChangedEvent =
		   EventManager.RegisterRoutedEvent("CornerOnlyChanged", RoutingStrategy.Bubble,
		   typeof(RoutedPropertyChangedEventHandler<bool>), typeof(PositionPicker));

		public event RoutedPropertyChangedEventHandler<ContentAlignment> PositionSelectionChanged {
			add {
				AddHandler(PositionSelectionChangedEvent, value);
			}
			remove {
				RemoveHandler(PositionSelectionChangedEvent, value);
			}
		}

		public event RoutedPropertyChangedEventHandler<bool> CornerOnlyChanged {
			add {
				AddHandler(CornerOnlyChangedEvent, value);
			}
			remove {
				RemoveHandler(CornerOnlyChangedEvent, value);
			}
		}

		private void OnPositionSelectionChanged(ContentAlignment oldValue, ContentAlignment newValue) {
			RoutedPropertyChangedEventArgs<ContentAlignment> args = new RoutedPropertyChangedEventArgs<ContentAlignment>(oldValue, newValue);
			args.RoutedEvent = PositionPicker.PositionSelectionChangedEvent;
			RaiseEvent(args);
		}

		private void OnCornerOnlyChanged(bool oldValue, bool newValue) {
			RoutedPropertyChangedEventArgs<bool> args = new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue);
			args.RoutedEvent = PositionPicker.CornerOnlyChangedEvent;
			RaiseEvent(args);
		}
	}
}