using System;
using System.IO;
using System.IO.Packaging;
using System.Printing;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms; //.Net 2.0
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Xps;
using System.Xml;

using OXMLWriter;

namespace OpenXMLWriter {
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>

	public partial class Window1 : System.Windows.Window {
		private int updatingUI = 0;
		private Boolean spellcheck = true;
		private string fileName = string.Empty;
		private bool isDirty = false;
		private static double[] AvailableFontSizes = new double[] {
                3.0d,    4.0d,    5.0d,    6.0d,   7.0d,   8.0d,   
                9.0d,    10.0d,   11.0d,   12.0d,   13.0d,
                14.0d,   15.0d,  16.0d,  17.0d,  18.0d,
                19.0d,  20.0d,  21.0d,  22.0d,  24.0d,
                25.0d,  26.0d,  28.0d,  30.0d,  32.0d,
                34.0d,  35.0d,  36.0d,  38.0d,  40.0d,  42.0d,  44.0d,  45.0d,  46.0d,  48.0d,
                50.0d,  52.0d,  56.0d,  60.0d,  62.0d, 64.0d,  66.0d, 68.0d, 70.0d, 72.0d, 74.0d, 76.0d,
                78.0d,  80.0d,  82.0d,  84.0d, 86.0d, 88.0d, 90.0d, 92.0d, 94.0d, 96.0d, 100.0d,  102.0d, 
                104.0d, 106,0d, 108.0d, 112.0d, 120.0d, 124.0d, 128.0d,132.0d, 136.0d,140.0d, 144.0d, 152.0d,
               160.0d, 168.0d, 176.0d, 180.0d, 186.0d, 192.0d, 1960.0d, 200.0d, 204.0d, 208.0d,212.0d, 216.0d, 220.0d, 224.0d, 232.0d, 240.0d, 250.0d, 260.0d, 270.0d, 280.0d, 290.0d,
               300.0d, 320.0d, 360.0d, 384.0d, 400.0d, 420.0d, 448.0d, 460.0d, 480.0d, 500.0d, 512.0d, 576.0d, 600.0d,  640.0d   };

		Color[] fontColors = {                
                Colors.Black,  
                Colors.White,                                                       
                Colors.Blue,  
                Colors.Red, 
                Colors.Green,
                Colors.Yellow, 
                Colors.Purple,                
                Colors.Brown,        
                Colors.Gray,  
                Colors.DarkGray,
                Colors.DarkBlue,                                
                Colors.DarkGreen,                
                Colors.DarkMagenta,
                Colors.DarkOliveGreen,
                Colors.DarkOrange,
                Colors.DarkOrchid,
                Colors.DarkRed,                
                Colors.DarkTurquoise,                
                Colors.Gold,                
                Colors.Cyan,
                Colors.Violet,                             
                Colors.Aqua,                
                Colors.Beige,               
                Colors.GreenYellow,                
                Colors.Indigo,
                Colors.Ivory,                
                Colors.LightBlue,                
                Colors.Lime,
                Colors.Magenta,
                Colors.Maroon,
                Colors.MediumBlue,
                Colors.Navy, 
                Colors.Olive,                
                Colors.Orange,
                Colors.OrangeRed,                
                Colors.Pink,                                                            
                Colors.Tan,
                Colors.Teal,                
                Colors.Turquoise,                             
                Colors.YellowGreen
                };

		public Window1() {
			InitializeComponent();

			foreach (FontFamily family in System.Windows.Media.Fonts.SystemFontFamilies) {
				cb.Items.Add(family.Source);
			}

			for (int i = 0; i < AvailableFontSizes.Length; i++) {
				cbsize.Items.Add(AvailableFontSizes[i]);
			}

			for (int i = 0; i < fontColors.Length; i++) {
				SolidColorBrush bx = new SolidColorBrush(fontColors[i]);
				Rectangle colorRect = new Rectangle();
				colorRect.Margin = new Thickness(1, 1, 1, 1);
				colorRect.Width = 25;
				colorRect.Height = 10;
				colorRect.Fill = bx;
				colorRect.Stroke = Brushes.DarkGray;
				colorRect.StrokeThickness = 1;
				cbcolor.Items.Add(colorRect);
			}

			mainRichTextBox.Document.PagePadding = new Thickness(12);
			mainRichTextBox.Focus();

			mainRichTextBox_SelectionChanged(null, null);

			setSpellCheck();
		}

		private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			FlowDocument flowdoc = null;

			OpenFileDialog openFile = new OpenFileDialog();
			//openFile.Filter = "FlowDocument Files (*.xaml)|*.xaml|All Files (*.*)|*.*";
			openFile.Filter = "FlowDocument Files (*.xaml)|*.xaml|Text Files - Unicode (*.txt)|*.txt|All Files (*.*)|*.*";

			if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				FileStream xamlFile = openFile.OpenFile() as FileStream;

				if (xamlFile == null) {
					return;
				} else {
					try {
						if (openFile.FilterIndex == 2) {
							//Load text Text
							using (StreamReader streamReader = new StreamReader(xamlFile)) {
								//Paragraph paragraph = new Paragraph();
								String textdata = streamReader.ReadToEnd();
								Run run = new Run(textdata);
								mainRichTextBox.Document.Blocks.Clear();
								mainRichTextBox.Document.Blocks.Add(new Paragraph(run));
							}
						} else {
							flowdoc = XamlReader.Load(xamlFile) as FlowDocument;
							if (flowdoc == null) {
								throw (new XamlParseException("Unable to load FlowDocument."));
							}
							mainRichTextBox.Document = flowdoc;
						}

						this.fileName = openFile.FileName;
						this.isDirty = false;
						setTitle();
					} catch (Exception ex) {
						System.Windows.MessageBox.Show(ex.Message);
						return;
					}
				}
			}
		}

		private void NewFile(Object sender, RoutedEventArgs args) {
			if (this.isDirty) {
				MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to save file first?",
					"File not Saved",
					MessageBoxButton.YesNoCancel,
					MessageBoxImage.Question);

				if (result == MessageBoxResult.Cancel) {
					return;
				} else if (result == MessageBoxResult.Yes) {
					save();
				}
			}

			mainRichTextBox.Document = new FlowDocument();

			this.isDirty = false;
			this.fileName = string.Empty;
			setTitle();
		}

		private void Exit(Object sender, RoutedEventArgs args) {
			if (this.isDirty) {
				save();
			}

			this.Close();
		}

		private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			if (string.IsNullOrEmpty(this.fileName)) {
				SaveFileDialog saveFileDialog = new SaveFileDialog();
				//saveFileDialog.Title = "Save File";

				//saveFileDialog.Filter = "FlowDocument Files (*.xaml)|*.xaml|All Files (*.*)|*.*";
				saveFileDialog.Filter = "FlowDocument Files (*.xaml)|*.xaml|Text Files - Unicode (*.txt)|*.txt|OpenXML Files (*.docx)|*.docx";
				if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					this.fileName = saveFileDialog.FileName;
				} else {
					return;
				}
			}

			save();
		}

		private void SaveFileAs(object sender, RoutedEventArgs e) {
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			//saveFileDialog.Title = "Save File As";

			saveFileDialog.Filter = "FlowDocument Files (*.xaml)|*.xaml|Text Files - Unicode (*.txt)|*.txt|OpenXML Files (*.docx)|*.docx";
			if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				this.fileName = saveFileDialog.FileName;
			} else {
				return;
			}

			save();
		}

		private void save() {
			FileStream xamlFile = null;
			try {
				xamlFile = File.Open(this.fileName, FileMode.Create);

				if (xamlFile == null) {
					return;
				} else {
					FileInfo fi = new FileInfo(this.fileName);
					if (string.Compare(fi.Extension, ".txt", true) == 0) {
						//Save as Text
						//The bullets in unicode text file may appear as a few characters when opened with a normal editor 
						FlowDocument flowDoc = mainRichTextBox.Document;
						TextPointer contentstart = flowDoc.ContentStart;
						TextPointer contentend = flowDoc.ContentEnd;
						TextRange tr = new TextRange(contentstart, contentend);
						tr.Save(xamlFile, System.Windows.DataFormats.Text);
					} else if (string.Compare(fi.Extension, ".docx", true) == 0) {
						FlowDocument flowDoc = mainRichTextBox.Document;
						flowDocToDocx(xamlFile, flowDoc);
					} else {
						//Save as XAML
						XamlWriter.Save(mainRichTextBox.Document, xamlFile);
					}

					this.isDirty = false;
					setTitle();
				}
			} catch (Exception e) {
				System.Windows.MessageBox.Show(e.Message);
				return;
			} finally {
				if (xamlFile != null) {
					xamlFile.Close();
				}
			}
		}

		private void flowDocToDocx(FileStream xamlFile, FlowDocument flowDoc) {
			TextPointer contentstart = flowDoc.ContentStart;
			TextPointer contentend = flowDoc.ContentEnd;
			if (contentstart == null) {
				throw new ArgumentNullException("ContentStart");
			}
			if (contentend == null) {
				throw new ArgumentNullException("ContentEnd");
			}

			//Create document

			// document package container
			Package zippackage = null;
			zippackage = Package.Open(xamlFile, FileMode.Create, FileAccess.ReadWrite);

			// main document.xml 
			Uri uri = new Uri("/word/document.xml", UriKind.Relative);
			PackagePart partDocumentXML = zippackage.CreatePart(uri, "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml");

			//ver 1.2 Numbering
			Uri uriNumbering = new Uri("/word/numbering.xml", UriKind.Relative);
			Uri uriNumberingRelationship = new Uri("numbering.xml", UriKind.Relative);
			PackagePart partNumberingXML = zippackage.CreatePart(uriNumbering, "application/vnd.openxmlformats-officedocument.wordprocessingml.numbering+xml");
			partDocumentXML.CreateRelationship(uriNumberingRelationship, TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/numbering", "rId1");

			using (XmlTextWriter openxmlwriter = new XmlTextWriter(partDocumentXML.GetStream(FileMode.Create, FileAccess.Write), System.Text.Encoding.UTF8)) {
				openxmlwriter.Formatting = Formatting.Indented;
				openxmlwriter.Indentation = 2;
				openxmlwriter.IndentChar = ' ';

				//ver 1.2 
				using (XmlTextWriter numberingwriter = new XmlTextWriter(partNumberingXML.GetStream(FileMode.Create, FileAccess.Write), System.Text.Encoding.UTF8)) {
					numberingwriter.Formatting = Formatting.Indented;
					numberingwriter.Indentation = 2;
					numberingwriter.IndentChar = ' ';

					//Actual Writing
					new OpenXmlWriter().Write(contentstart, contentend, openxmlwriter, numberingwriter);
				}
			}

			zippackage.Flush();

			// relationship 
			zippackage.CreateRelationship(uri, TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument", "rId1");
			zippackage.Flush();
			zippackage.Close();
		}

		void PrintContent(Object sender, RoutedEventArgs args) {
			//Printing with Paginator

			//set pagination with printer's margins and size
			PrintDocumentImageableArea area = null;
			XpsDocumentWriter xdwriter = PrintQueue.CreateXpsDocumentWriter(ref area);

			//make a copy to print with pagginator w/o crashing
			TextPointer position1 = mainRichTextBox.Document.ContentStart;
			TextPointer position2 = mainRichTextBox.Document.ContentEnd;
			TextRange sourceDocumentRange = new TextRange(position1, position2);

			MemoryStream tempstream = new MemoryStream();
			sourceDocumentRange.Save(tempstream, System.Windows.DataFormats.Xaml);

			FlowDocument sourceDocumentCopy = new FlowDocument();
			TextPointer position3 = sourceDocumentCopy.ContentStart;
			TextPointer position4 = sourceDocumentCopy.ContentEnd;
			TextRange copyDocumentRange = new TextRange(position3, position4);
			copyDocumentRange.Load(tempstream, System.Windows.DataFormats.Xaml);

			if ((xdwriter != null) && (area != null)) {
				DocumentPaginator paginator = ((IDocumentPaginatorSource)sourceDocumentCopy).DocumentPaginator;
				paginator.PageSize = new Size(area.MediaSizeWidth, area.MediaSizeHeight);

				Thickness pageBounds = sourceDocumentCopy.PagePadding;

				double leftmargin, topmargin, rightmargin, bottommargin;
				if (area.OriginWidth > pageBounds.Left)
					leftmargin = area.OriginWidth;
				else
					leftmargin = pageBounds.Left;

				if (area.OriginHeight > pageBounds.Top)
					topmargin = area.OriginHeight;
				else
					topmargin = pageBounds.Top;

				double printerRightMargin = area.MediaSizeWidth - (area.OriginWidth + area.ExtentWidth);
				if (printerRightMargin > pageBounds.Right)
					rightmargin = printerRightMargin;
				else
					rightmargin = pageBounds.Right;

				double printerBottomMargin = area.MediaSizeHeight - (area.OriginHeight + area.ExtentHeight);
				if (printerBottomMargin > pageBounds.Bottom)
					bottommargin = printerBottomMargin;
				else
					bottommargin = pageBounds.Bottom;

				sourceDocumentCopy.PagePadding = new Thickness(leftmargin, topmargin, rightmargin, bottommargin);

				//can be used to set columns
				sourceDocumentCopy.ColumnWidth = double.PositiveInfinity;

				xdwriter.Write(paginator);
			}
		}

		private double pointsToPixels(double value) {
			return value * 96.0d / 72.0d;
		}

		private double pixelsToPoints(double value) {
			return Math.Round(value * 72.0 / 96.0);
		}

		void DoCut(Object sender, RoutedEventArgs args) {
			mainRichTextBox.Cut();
		}

		void DoUndo(Object sender, RoutedEventArgs args) {
			mainRichTextBox.Undo();
		}

		void DoRedo(Object sender, RoutedEventArgs args) {
			mainRichTextBox.Redo();
		}

		void DoPaste(Object sender, RoutedEventArgs args) {
			mainRichTextBox.Paste();
		}

		void DoCopy(Object sender, RoutedEventArgs args) {
			mainRichTextBox.Copy();
		}

		void DoSelectAll(Object sender, RoutedEventArgs args) {
			mainRichTextBox.SelectAll();
		}

		void DoAbout(Object sender, RoutedEventArgs args) {
			System.Windows.MessageBox.Show("OpenXML Writer 1.2\n\nCopyright 2007 openxml.biz", "About", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		void ToggleSpellCheck(Object sender, RoutedEventArgs args) {
			if (spellcheck) {
				spellcheck = false;
			} else {
				spellcheck = true;
			}

			setSpellCheck();
		}

		private void setSpellCheck() {
			mainRichTextBox.SpellCheck.IsEnabled = spellcheck;
			SpellCheckMenuItem.IsChecked = spellcheck;
		}

		void FontChanged(Object sender, RoutedEventArgs args) {
			if (updatingUI == 1)
				return;

			TextRange range = mainRichTextBox.Selection;
			range.ApplyPropertyValue(TextElement.FontFamilyProperty, cb.SelectedValue.ToString());

			//ver 1.2
			mainRichTextBox.Focus();
		}

		void FontSizeChanged(Object sender, RoutedEventArgs args) {
			if (updatingUI == 1) {
				return;
			}

			TextRange range = mainRichTextBox.Selection;
			double chosenSize = 8;

			if (double.TryParse(cbsize.SelectedValue.ToString(), out chosenSize)) {
				range.ApplyPropertyValue(TextElement.FontSizeProperty, pointsToPixels(chosenSize));
				//range.ApplyPropertyValue(TextElement.FontSizeProperty, chosenSize);
			}

			//ver 1.2
			mainRichTextBox.Focus();
		}

		void FontColorChanged(Object sender, RoutedEventArgs args) {
			if (updatingUI == 1) {
				return;
			}

			TextRange range = mainRichTextBox.Selection;
			int selIndex = cbcolor.SelectedIndex;
			SolidColorBrush bx = new SolidColorBrush(fontColors[selIndex]);
			range.ApplyPropertyValue(TextElement.ForegroundProperty, bx);

			//ver 1.2
			mainRichTextBox.Focus();
		}

		#region Rich Textbox events
		private void mainRichTextBox_SelectionChanged(Object sender, RoutedEventArgs args) {
			updatingUI = 1;

			TextRange range = mainRichTextBox.Selection;
			object fontFamily = range.GetPropertyValue(TextElement.FontFamilyProperty);

			if (cb.Items.Contains(fontFamily.ToString())) {
				cb.SelectedItem = fontFamily.ToString();
			} else {
				cb.SelectedValue = "";
			}

			object fontSize = range.GetPropertyValue(TextElement.FontSizeProperty);
			double fs = 8;
			if (double.TryParse(fontSize.ToString(), out fs)) {
				fs = pixelsToPoints(fs);
				if (cbsize.Items.Contains(fs)) {
					cbsize.SelectedItem = fs;
				} else {
					cbsize.SelectedValue = fs;
				}
			}

			object fontColor = range.GetPropertyValue(TextElement.ForegroundProperty);

			SolidColorBrush px = null;
			try {
				px = fontColor as SolidColorBrush;
				if (px != null) {
					Color cz = px.Color;

					int found = -1;
					for (int i = 0; i < fontColors.Length; i++) {
						if (fontColors[i] == cz) {
							found = i;
							break;
						}
					}

					if (found >= 0) {
						cbcolor.SelectedIndex = found;
					} else {
						cbcolor.SelectedValue = "";
					}
				}
			} catch (Exception e) {
				string strx = e.ToString();
			}

			updatingUI = 0;
		}

		private void mainRichTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
			this.isDirty = true;
			setTitle();
		} 
		#endregion

		protected void ViewXamlCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			/* this is complete version of xaml
			TextRange range;

			range = new TextRange(mainRichTextBox.Document.ContentStart, mainRichTextBox.Document.ContentEnd);

			MemoryStream stream = new MemoryStream();
			range.Save(stream, System.Windows.DataFormats.Xaml);
			stream.Position = 0;

			StreamReader r = new StreamReader(stream);

			txtFlowDocumentMarkup.Text = r.ReadToEnd();
			r.Close();
			stream.Close();*/

			using (MemoryStream xaml = new MemoryStream()) {
				XamlWriter.Save(mainRichTextBox.Document, xaml);
				xaml.Seek(0, 0);

				using (StreamReader reader = new StreamReader(xaml)) {
					XamlView xamlWindow = new XamlView();
					xamlWindow.Owner = this;
					// Just read to the end.
					xamlWindow.XamlString = reader.ReadToEnd();
					xamlWindow.ShowDialog();
				}
			}
		}

		private void setTitle() {
			string title = "OpenXML Writer";
			if (!string.IsNullOrEmpty(this.fileName)) {
				title += " -- ";
				if (this.isDirty) {
					title += "*";
				}

				title += this.fileName;
			}

			this.Title = title;
		}

		

		
	}
}