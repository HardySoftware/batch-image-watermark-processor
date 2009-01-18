using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/*
 * Credit goes to Joel Neubeck's article at http://www.codeproject.com/csharp/watermark.asp
 * and Big D's article at http://www.codeproject.com/csharp/watermark_creator.asp
 * and Bob Powel's articles at http://www.bobpowell.net/faqmain.htm
 * */

namespace HardySoft.UI.BatchImageProcessor {
    public partial class MainForm : Form {
        private bool stop = false;
        private ProjectProperties pp;
        private string projectFileName = "";
        private string windowTitle = "Hardy Software Batch Image Processor";
        private bool isDirty = false;

        public MainForm () {
            InitializeComponent();
            pp = new ProjectProperties();
        }

        #region Initialize
        private void MainForm_Load (object sender, EventArgs e) {
            cmbImageType.SelectedIndex = 0;
            //setWMProperties();
            disableInput();
        }
        #endregion

        #region Controls status
        private void disableInput () {
            lstFileList.Items.Clear();
            picSource.Image = null;
            properties.Enabled = false;
            btnBrowseWorkingFolder.Enabled = false;
            btnBrowseOutputFolder.Enabled = false;
            btnMake.Enabled = false;
            btnStop.Enabled = false;
            btnBrowseOutputFolder.Enabled = false;
            cmbImageType.Enabled = false;
            chkPreview.Enabled = false;
            saveProjectAsToolStripMenuItem.Enabled = false;
            saveProjectToolStripMenuItem.Enabled = false;
            closeProjectToolStripMenuItem.Enabled = false;
            chkBatchFileRename.Enabled = false;
            properties.SelectedObject = null;
            propertyRenameOutput.SelectedObject = null;
            txtOutputFolder.Text = "";
            txtWorkingFolder.Text = "";
        }

        private void enableInput () {
            properties.Enabled = true;
            btnBrowseWorkingFolder.Enabled = true;
            btnBrowseOutputFolder.Enabled = true;
            cmbImageType.Enabled = true;
            chkPreview.Enabled = true;
            btnMake.Enabled = true;
            properties.SelectedObject = pp.Properties;
            chkBatchFileRename.Enabled = true;
            saveProjectAsToolStripMenuItem.Enabled = true;
            saveProjectToolStripMenuItem.Enabled = true;
        }

        private void conditionalEnableInput () {
            if (chkBatchFileRename.Checked) {
                propertyRenameOutput.Enabled = true;
                propertyRenameOutput.SelectedObject = pp.RenameProperties;
            }
        }
        #endregion

        #region Image file list
        private void btnBrowseWorkingFolder_Click (object sender, EventArgs e) {
            string oldWorkingFolder;
            try {
                if (txtWorkingFolder.Text.Substring(0, 1) == ".") {
                    oldWorkingFolder = txtWorkingFolder.Text.Replace(".", AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.Length - 1));
                } else {
                    oldWorkingFolder = txtWorkingFolder.Text;
                }

                folderBrowser.SelectedPath = oldWorkingFolder;
                //folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.Personal;
            } catch (Exception ex) {
                showError(ex);
            }

            try {
                if (folderBrowser.ShowDialog() == DialogResult.OK) {
                    txtWorkingFolder.Text = folderBrowser.SelectedPath;
                    pp.ImageFolder = folderBrowser.SelectedPath;
                    settingChanged();
                    getFiles(folderBrowser.SelectedPath);
                    previewImage(0);
                }
            } catch (Exception ex) {
                showError(ex);
            }
        }

        private void getFiles (string workingFolder) {
            string[] temp1 = null, temp2 = null, temp3 = null, fileEntries = null;
            int totalFiles = 0;

            try {
                if (!Directory.Exists(workingFolder)) {
                    return;
                }

                switch (cmbImageType.SelectedIndex) {
                    case 0:
                        temp1 = Directory.GetFiles(workingFolder, "*.jpg");
                        temp2 = Directory.GetFiles(workingFolder, "*.jpeg");
                        temp3 = Directory.GetFiles(workingFolder, "*.bmp");
                        totalFiles = temp1.Length + temp2.Length + temp3.Length;
                        fileEntries = new string[totalFiles];
                        Array.Copy(temp1, 0, fileEntries, 0, temp1.Length);
                        Array.Copy(temp2, 0, fileEntries, temp1.Length, temp2.Length);
                        Array.Copy(temp3, 0, fileEntries, temp1.Length + temp2.Length, temp3.Length);
                        break;
                    case 1:
                        temp1 = Directory.GetFiles(workingFolder, "*.jpg");
                        temp2 = Directory.GetFiles(workingFolder, "*.jpeg");
                        totalFiles = temp1.Length + temp2.Length;
                        fileEntries = new string[totalFiles];
                        Array.Copy(temp1, 0, fileEntries, 0, temp1.Length);
                        Array.Copy(temp2, 0, fileEntries, fileEntries.Length, temp2.Length);
                        break;
                    case 2:
                        fileEntries = Directory.GetFiles(workingFolder, "*.bmp");
                        totalFiles = fileEntries.Length;
                        break;

                }

                progress.Maximum = totalFiles;
                lstFileList.Items.Clear();

                foreach (string fileName in fileEntries) {
                    if (fileName != null) {
                        lstFileList.Items.Add(fileName.Replace(AppDomain.CurrentDomain.BaseDirectory, @".\"), CheckState.Checked);
                    }
                }
            } catch  (Exception ex) {
                showError(ex);
            }
        }

        private void lstFileList_SelectedIndexChanged (object sender, EventArgs e) {
            previewImage(lstFileList.SelectedIndex);
        }

        private void cmbImageType_SelectedIndexChanged (object sender, EventArgs e) {
            if (txtWorkingFolder.Text.Trim().Length > 0) {
                getFiles(txtWorkingFolder.Text);
                previewImage(0);
                pp.ImageTypeSelectIndex = cmbImageType.SelectedIndex;
                settingChanged();
            }
        }

        private void lstFileList_ItemCheck (object sender, ItemCheckEventArgs e) {
            if (e.CurrentValue == CheckState.Unchecked && e.NewValue == CheckState.Checked) {
                string fileName = lstFileList.Items[e.Index].ToString();
                //pp.AddSourceFile(fileName);
                settingChanged();
            } else if (e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked) {
                string fileName = lstFileList.Items[e.Index].ToString();
                //pp.RemoveSourceFile(fileName);
                settingChanged();
            }
        }

        private void chkPreview_CheckedChanged (object sender, EventArgs e) {
            pp.Preview = chkPreview.Checked;
            settingChanged();
        }

        private void previewImage (int lineInList) {
            if (chkPreview.Checked == true && lstFileList.Items.Count > lineInList) {
                lstFileList.SelectedIndex = lineInList;
                if (File.Exists(lstFileList.Items[lineInList].ToString())) {
                    System.Drawing.Image img = Image.FromFile(lstFileList.Items[lineInList].ToString());
                    int width = img.Width;
                    int height = img.Height;
                    /* Calculate the dimensions necessary for an image to fit. */
                    Size fitImageSize = this.getScaledImageDimensions(
                        width, height, this.picSource.Width, this.picSource.Height);
                    Bitmap imgOutput = new Bitmap(img, fitImageSize.Width, fitImageSize.Height);

                    /* Clear any existing image in the PictureBox. */
                    this.picSource.Image = null;

                    /* When fitting the image to the window, we want to keep it centered. */
                    this.picSource.SizeMode = PictureBoxSizeMode.CenterImage;

                    /* Finally, set the Image property to point to the new, resized image. */
                    this.picSource.Image = imgOutput;
                    //picSource.Image = Image.FromFile(lstFileList.Items[0].ToString());
                }
            } else {
                this.picSource.Image = null;
            }
        }

        private Size getScaledImageDimensions (
            int currentImageWidth,
            int currentImageHeight,
            int desiredImageWidth,
            int desiredImageHeight) {
            /* First, we must calculate a multiplier that will be used
             * to get the dimensions of the new, scaled image.
             */

            double scaleImageMultiplier = 0;

            /* This multiplier is defined as the ratio of the
             * Desired Dimension to the Current Dimension.
             * Specifically which dimension is used depends on the larger
             * dimension of the image, as this will be the constraining dimension
             * when we fit to the window.
             */

            /* Determine if Image is Portrait or Landscape. */
            if (currentImageHeight > currentImageWidth) {
                /* Image is Portrait */
                /* Calculate the multiplier based on the heights. */
                if (desiredImageHeight > desiredImageWidth) {
                    scaleImageMultiplier = (double)desiredImageWidth / (double)currentImageWidth;
                } else {
                    scaleImageMultiplier = (double)desiredImageHeight / (double)currentImageHeight;
                }
            } else {
                /* Image is Landscape */
                /* Calculate the multiplier based on the widths. */
                if (desiredImageHeight > desiredImageWidth) {
                    scaleImageMultiplier = (double)desiredImageWidth / (double)currentImageWidth;
                } else {
                    scaleImageMultiplier = (double)desiredImageHeight / (double)currentImageHeight;
                }
            }

            /* Generate and return the new scaled dimensions.
             * Essentially, we multiply each dimension of the original image
             * by the multiplier calculated above to yield the dimensions
             * of the scaled image. The scaled image can be larger or smaller
             * than the original.
             */

            return new Size(
                (int)(currentImageWidth * scaleImageMultiplier),
                (int)(currentImageHeight * scaleImageMultiplier));
        }

        private void properties_PropertyValueChanged (object s, PropertyValueChangedEventArgs e) {
            settingChanged();
        }
        #endregion

        #region Output options
        private void cmdBrowse3_Click (object sender, EventArgs e) {
            try {
                if (txtOutputFolder.Text.Trim().Length > 0) {
                    folderBrowser.SelectedPath = txtOutputFolder.Text;
                }
            } catch {
            }

            try {
                if (folderBrowser.ShowDialog() == DialogResult.OK) {
                    if (txtWorkingFolder.Text == folderBrowser.SelectedPath) {
                        MessageBox.Show("Input and output folder can not be same", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    } else {
                        txtOutputFolder.Text = folderBrowser.SelectedPath;
                        pp.OutputFolder = folderBrowser.SelectedPath;
                        settingChanged();
                    }
                }
            } catch {
            }
        }

        private void chkBatchFileRename_CheckedChanged (object sender, EventArgs e) {
            settingChanged();
            pp.BatchRenameOutputFiles = chkBatchFileRename.Checked;
            propertyRenameOutput.Enabled = chkBatchFileRename.Checked;
            if (propertyRenameOutput.Enabled) {
                if (pp.RenameProperties == null) {
                    pp.RenameProperties = new BatchRenameProperties();
                }
                propertyRenameOutput.SelectedObject = pp.RenameProperties;
            }
        }

        private void propertyRenameOutput_PropertyValueChanged (object s, PropertyValueChangedEventArgs e) {
            settingChanged();
        }
        #endregion

        #region Process Images
        private void btnMake_Click (object sender, EventArgs e) {
            if (! validate()) {
                return;
            }

            btnStop.Enabled = true;
            progress.Value = 0;
            try {
                Application.DoEvents();
                WaterMark wm = new WaterMark(txtWorkingFolder.Text, pp.Properties);

                for (int i = 0; i < lstFileList.CheckedItems.Count; i++) {
                    if (stop) {
                        stop = false;
                        return;
                    }
                    string sourcePicture, destinationPicture;
                    FileInfo fi = new FileInfo(lstFileList.Items[i].ToString());
                    string fileNameOnly = fi.Name;
                    sourcePicture = Utilities.FormalizeFolderName(txtWorkingFolder.Text) + fileNameOnly;
                    destinationPicture = Utilities.FormalizeFolderName(txtOutputFolder.Text) + getOutputFileName(fileNameOnly, i);

                    wm.MarkImage(sourcePicture, destinationPicture);
                    progress.Increment(1);
                    Application.DoEvents();
                }
                Application.DoEvents();
            } catch (Exception ex) {
                showError(ex);
                Application.DoEvents();
            } finally {
                progress.Value = 0;
                btnStop.Enabled = false;
            }
        }

        private string getOutputFileName (string fileName, int batchIndex) {
            if (pp.BatchRenameOutputFiles) {
                string fileNameWithoutExtension, extension, newFileName;
                int pos = fileName.LastIndexOf(".");
                if (pos > -1) {
                    fileNameWithoutExtension = fileName.Substring(0, pos);
                    extension = fileName.Substring(pos + 1, fileName.Length - pos - 1);
                } else {
                    fileNameWithoutExtension = fileName;
                    extension = "";
                }

                if (pp.RenameProperties.FileNamePrefix.Length > 0 || pp.RenameProperties.FileNameSuffix.Length > 0) {
                    newFileName = pp.RenameProperties.FileNamePrefix + (batchIndex + pp.RenameProperties.StartWithNumber).ToString("D" + pp.RenameProperties.NumberPadding) + pp.RenameProperties.FileNameSuffix;
                } else {
                    newFileName = fileNameWithoutExtension + (batchIndex + pp.RenameProperties.StartWithNumber).ToString("D" + pp.RenameProperties.NumberPadding);
                }
                newFileName = newFileName + "." + extension;
                return newFileName;
            } else {
                return fileName;
            }
        }

        private void btnStop_Click (object sender, EventArgs e) {
            stop = true;
        }
        #endregion

        #region Menu events
        private void aboutToolStripMenuItem_Click (object sender, EventArgs e) {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void newProjecttoolStripMenuItem_Click (object sender, EventArgs e) {
            if (isDirty) {
                if (MessageBox.Show("Do you want to save pending changes?", "Save Change",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    saveProjectToolStripMenuItem_Click(this, null);
                }

                isDirty = false;
            }

            pp = new ProjectProperties();
            loadSavedSettings();
            projectFileName = "";
            setWindowTitle("Untitled");
            disableInput();
            enableInput();
        }

        private void openProjectToolStripMenuItem_Click (object sender, EventArgs e) {
            openFile.Filter = "All Image Process Project Files (*." + Utilities.ProjectFileExtension + ")|*." + Utilities.ProjectFileExtension + ";";
            if (openFile.ShowDialog() == DialogResult.OK) {
                /*projectFileName = openFile.FileName;
                try {
                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(projectFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    pp = (ProjectProperties)formatter.Deserialize(stream);
                    stream.Close();
                    loadSavedSettings();
                    getFiles(pp.ImageFolder);
                    setSourceFileCheckedStatus();
                    previewImage(0);
                    settingSaved();
                } catch (Exception ex) {
                    showError(ex);
                }*/
                openProjectFile(openFile.FileName);
            }
        }

        private void openProjectFile (string projectFile) {
            projectFileName = projectFile;
            try {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(projectFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                pp = (ProjectProperties)formatter.Deserialize(stream);
                stream.Close();
                loadSavedSettings();
                getFiles(pp.ImageFolder);
                setSourceFileCheckedStatus();
                previewImage(0);
                settingSaved();
            } catch (Exception ex) {
                showError(ex);
            }
        }

        private void loadSavedSettings () {
            enableInput();
            saveProjectAsToolStripMenuItem.Enabled = true;
            saveProjectToolStripMenuItem.Enabled = true;
            closeProjectToolStripMenuItem.Enabled = true;
            txtWorkingFolder.Text = pp.ImageFolder;
            chkPreview.Checked = pp.Preview;
            txtOutputFolder.Text = pp.OutputFolder;
            cmbImageType.SelectedIndex = pp.ImageTypeSelectIndex;
            chkBatchFileRename.Checked = pp.BatchRenameOutputFiles;
            conditionalEnableInput();
            properties.SelectedObject = pp.Properties;
            propertyRenameOutput.SelectedObject = pp.RenameProperties;
        }

        private void setSourceFileCheckedStatus () {
            if (pp.SelectedSourceFiles != null) {
                for (int i = 0; i < lstFileList.Items.Count; i++) {
                    string fileName = lstFileList.Items[i].ToString();
                    if (pp.SelectedSourceFiles.Contains(fileName)) {
                        lstFileList.SetItemCheckState(i, CheckState.Checked);
                    } else {
                        lstFileList.SetItemCheckState(i, CheckState.Unchecked);
                    }
                }
            }
        }

        private void saveProjectToolStripMenuItem_Click (object sender, EventArgs e) {
            if (projectFileName.Length == 0) {
                saveProjectAsToolStripMenuItem_Click(sender, e);
            } else {
                saveProject();
            }
        }

        private void saveProjectAsToolStripMenuItem_Click (object sender, EventArgs e) {
            saveFile.Filter = "All Image Process Project Files (*." + Utilities.ProjectFileExtension + ")|*." + Utilities.ProjectFileExtension + ";";
            if (saveFile.ShowDialog() == DialogResult.OK) {
                projectFileName = saveFile.FileName;
                saveProject();
            }
        }

        private void saveProject () {
            try {
                pp.ClearAllSourceFiles();
                foreach (object item in lstFileList.CheckedItems) {
                    pp.AddSourceFile(item.ToString());
                }
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(projectFileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, pp);
                stream.Close();
                settingSaved();
            } catch (Exception ex) {
                showError(ex);
            }
        }

        private void closeProjectToolStripMenuItem_Click (object sender, EventArgs e) {
            if (isDirty) {
                if (MessageBox.Show("Do you want to save pending changes?", "Save Change",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    saveProjectToolStripMenuItem_Click(this, null);
                }

                isDirty = false;
            }

            pp = new ProjectProperties();
            disableInput();
            projectFileName = "";
            settingSaved();
        }

        private void exitToolStripMenuItem_Click (object sender, EventArgs e) {
            if (isDirty) {
                if (MessageBox.Show("Do you want to save pending changes?", "Save Change",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    saveProjectToolStripMenuItem_Click(this, null);
                }

                isDirty = false;
            }

            Application.Exit();
        }
        #endregion

        #region Helps
        private void showError (Exception ex) {
            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void setWindowTitle (string title) {
            if (title.Trim().Length == 0) {
                this.Text = this.windowTitle;
            } else {
                this.Text = this.windowTitle + " -- " + title;
            }
        }

        private void settingChanged () {
            if (projectFileName.Length > 0) {
                setWindowTitle("* " + projectFileName);
            } else {
                setWindowTitle("* Untitled");
            }
            isDirty = true;
        }

        private void settingSaved () {
            setWindowTitle(projectFileName);
            isDirty = false;
        }

        private bool validate () {
            if (txtWorkingFolder.Text.Trim().Length == 0) {
                MessageBox.Show("No input folder.", this.windowTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtOutputFolder.Text.Trim().Length == 0) {
                MessageBox.Show("No output folder.", this.windowTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtWorkingFolder.Text.Equals(txtOutputFolder.Text)) {
                MessageBox.Show("Output folder can not be same as input folder.", this.windowTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtWorkingFolder.Text.Trim().Length == 0) {
            }
            return true;
        }
        #endregion

        #region Drag and Drop support
        private void MainForm_DragEnter (object sender, DragEventArgs e) {
            // If file is dragged, show cursor "Drop allowed"
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                if (a.Length > 1) {
                    // accept one file at one time only
                    e.Effect = DragDropEffects.None;
                } else {
                    FileInfo fi = new FileInfo(a.GetValue(0).ToString());
                    if (fi.Extension.ToLower().EndsWith(Utilities.ProjectFileExtension)) {
                        e.Effect = DragDropEffects.Copy;
                    } else {
                        e.Effect = DragDropEffects.None;
                    }
                }
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainForm_DragDrop (object sender, DragEventArgs e) {
            try {
                // When file is dragged from Explorer to the form, IDataObject
                // contains array of file names. If one file is dragged,
                // array contains one element.
                Array a = (Array)e.Data.GetData(DataFormats.FileDrop);

                if (a != null) {
                    // Extract string from first array element
                    string s = a.GetValue(0).ToString();

                    // Call OpenFile.
                    openProjectFile(s);

                    // in the case Explorer overlaps this form
                    this.Activate();
                }
            } catch (Exception ex) {
                showError(ex);
                //Trace.WriteLine("Error in DragDrop function: " + ex.Message);
                // don't show MessageBox here - Explorer is waiting !
            }
        }
        #endregion
    }
}