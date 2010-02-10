using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HardySoft.UI.BatchImageProcessor.Controls {
	public class SortableListView : ListView {
		SortableGridViewColumn lastSortedOnColumn = null;

		ListSortDirection lastDirection = ListSortDirection.Ascending;

		#region New Dependency Properties
		public string ColumnHeaderSortedAscendingTemplate {
			get {
				return (string)GetValue(ColumnHeaderSortedAscendingTemplateProperty);
			}
			set {
				SetValue(ColumnHeaderSortedAscendingTemplateProperty, value);
			}
		}

		// Using assembly DependencyProperty as the backing store for ColumnHeaderSortedAscendingTemplate.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ColumnHeaderSortedAscendingTemplateProperty =
			DependencyProperty.Register("ColumnHeaderSortedAscendingTemplate",
			typeof(string),
			typeof(SortableListView),
			new UIPropertyMetadata(""));

		public string ColumnHeaderSortedDescendingTemplate {
			get {
				return (string)GetValue(ColumnHeaderSortedDescendingTemplateProperty);
			}
			set {
				SetValue(ColumnHeaderSortedDescendingTemplateProperty, value);
			}
		}

		// Using assembly DependencyProperty as the backing store for ColumnHeaderSortedDescendingTemplate.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ColumnHeaderSortedDescendingTemplateProperty =
			DependencyProperty.Register("ColumnHeaderSortedDescendingTemplate",
			typeof(string),
			typeof(SortableListView),
			new UIPropertyMetadata(""));

		public string ColumnHeaderNotSortedTemplate {
			get {
				return (string)GetValue(ColumnHeaderNotSortedTemplateProperty);
			}
			set {
				SetValue(ColumnHeaderNotSortedTemplateProperty, value);
			}
		}

		// Using assembly DependencyProperty as the backing store for ColumnHeaderNotSortedTemplate.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ColumnHeaderNotSortedTemplateProperty =
			DependencyProperty.Register("ColumnHeaderNotSortedTemplate",
			typeof(string),
			typeof(SortableListView),
			new UIPropertyMetadata(""));
		#endregion

		/// 
		/// Executes when the control is initialized completely the first time through. Runs only once.
		/// 
		protected override void OnInitialized(EventArgs e) {
			// add the event handler to the GridViewColumnHeader. This strongly ties this ListView to assembly GridView.
			this.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(GridViewColumnHeaderClickedHandler));

			setInitialStatus();

			base.OnInitialized(e);
		}

		/// 
		/// Event Handler for the ColumnHeader Click Event.
		/// 
		private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e) {
			GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;

			// ensure that we clicked on the column header and not the padding that's added to fill the space.
			if (headerClicked != null && headerClicked.Role != GridViewColumnHeaderRole.Padding) {
				// attempt to cast to the sortableGridViewColumn object.
				SortableGridViewColumn sortableGridViewColumn = (headerClicked.Column) as SortableGridViewColumn;
				// ensure that the column header is the correct type and assembly sort property has been set.
				if (sortableGridViewColumn != null
					&& !String.IsNullOrEmpty(sortableGridViewColumn.SortPropertyName)) {
					ListSortDirection direction;
					bool newSortColumn = false;
					// determine if this is assembly new sort, or assembly switch in sort direction.
					if (lastSortedOnColumn == null
						|| String.IsNullOrEmpty(lastSortedOnColumn.SortPropertyName)
						|| !String.Equals(sortableGridViewColumn.SortPropertyName, lastSortedOnColumn.SortPropertyName, StringComparison.InvariantCultureIgnoreCase)) {
						newSortColumn = true;
						direction = ListSortDirection.Ascending;
					} else {
						if (lastDirection == ListSortDirection.Ascending) {
							direction = ListSortDirection.Descending;
						} else {
							direction = ListSortDirection.Ascending;
						}
					}

					// get the sort property name from the column's information.
					string sortPropertyName = sortableGridViewColumn.SortPropertyName;
					// sort the data.
					sort(sortPropertyName, direction);
					if (direction == ListSortDirection.Ascending) {
						if (!String.IsNullOrEmpty(this.ColumnHeaderSortedAscendingTemplate)) {
							sortableGridViewColumn.HeaderTemplate = this.TryFindResource(ColumnHeaderSortedAscendingTemplate) as DataTemplate;
						} else {
							sortableGridViewColumn.HeaderTemplate = null;
						}
					} else {
						if (!String.IsNullOrEmpty(this.ColumnHeaderSortedDescendingTemplate)) {
							sortableGridViewColumn.HeaderTemplate = this.TryFindResource(ColumnHeaderSortedDescendingTemplate) as DataTemplate;
						} else {
							sortableGridViewColumn.HeaderTemplate = null;
						}
					}

					// Remove arrow from previously sorted header
					if (newSortColumn && lastSortedOnColumn != null) {
						if (!String.IsNullOrEmpty(this.ColumnHeaderNotSortedTemplate)) {
							lastSortedOnColumn.HeaderTemplate = this.TryFindResource(ColumnHeaderNotSortedTemplate) as DataTemplate;
						} else {
							lastSortedOnColumn.HeaderTemplate = null;
						}
					}
					lastSortedOnColumn = sortableGridViewColumn;
				}
			}
		}

		protected override void OnItemsSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue) {
			setInitialStatus();

			base.OnItemsSourceChanged(oldValue, newValue);
		}

		private void setInitialStatus() {
			// cast the ListView's View to assembly GridView
			GridView gridView = this.View as GridView;

			if (gridView != null) {
				// determine which column is marked as IsDefaultSortColumn. Stops on the first column marked this way.
				SortableGridViewColumn sortableGridViewColumn = null;
				foreach (GridViewColumn gridViewColumn in gridView.Columns) {
					sortableGridViewColumn = gridViewColumn as SortableGridViewColumn;
					if (sortableGridViewColumn != null) {
						if (sortableGridViewColumn.IsDefaultSortColumn) {
							break;
						}
						sortableGridViewColumn = null;
					}
				}

				// if the default sort column is defined, sort the data and then update the templates as necessary.
				if (sortableGridViewColumn != null) {
					lastSortedOnColumn = sortableGridViewColumn;
					sort(sortableGridViewColumn.SortPropertyName, ListSortDirection.Ascending);

					if (!String.IsNullOrEmpty(this.ColumnHeaderSortedAscendingTemplate)) {
						sortableGridViewColumn.HeaderTemplate = this.TryFindResource(ColumnHeaderSortedAscendingTemplate) as DataTemplate;
					}

					this.SelectedIndex = 0;
				}
			}
		}

		/// 
		/// Helper method that sorts the data.
		/// 
		private void sort(string sortBy, ListSortDirection direction) {
			lastDirection = direction;
			ICollectionView dataView = CollectionViewSource.GetDefaultView(this.ItemsSource);

			if (dataView != null) {
				dataView.SortDescriptions.Clear();
				SortDescription sd = new SortDescription(sortBy, direction);
				dataView.SortDescriptions.Add(sd);
				dataView.Refresh();
			}
		}
	}
}
