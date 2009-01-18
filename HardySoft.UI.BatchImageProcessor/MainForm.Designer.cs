namespace HardySoft.UI.BatchImageProcessor {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent () {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjecttoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.properties = new System.Windows.Forms.PropertyGrid();
            this.lstFileList = new System.Windows.Forms.CheckedListBox();
            this.cmbImageType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowseWorkingFolder = new System.Windows.Forms.Button();
            this.txtWorkingFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkPreview = new System.Windows.Forms.CheckBox();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseOutputFolder = new System.Windows.Forms.Button();
            this.picSource = new System.Windows.Forms.PictureBox();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.chkBatchFileRename = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabRenameDest = new System.Windows.Forms.TabPage();
            this.propertyRenameOutput = new System.Windows.Forms.PropertyGrid();
            this.btnMake = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.menuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSource)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabRenameDest.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(900, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjecttoolStripMenuItem,
            this.openProjectToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.saveProjectAsToolStripMenuItem,
            this.closeProjectToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newProjecttoolStripMenuItem
            // 
            this.newProjecttoolStripMenuItem.Name = "newProjecttoolStripMenuItem";
            this.newProjecttoolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newProjecttoolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.newProjecttoolStripMenuItem.Text = "&New Project";
            this.newProjecttoolStripMenuItem.Click += new System.EventHandler(this.newProjecttoolStripMenuItem_Click);
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.openProjectToolStripMenuItem.Text = "&Open Project";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.saveProjectToolStripMenuItem.Text = "&Save Project";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
            // 
            // saveProjectAsToolStripMenuItem
            // 
            this.saveProjectAsToolStripMenuItem.Name = "saveProjectAsToolStripMenuItem";
            this.saveProjectAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.saveProjectAsToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.saveProjectAsToolStripMenuItem.Text = "S&ave Project As";
            this.saveProjectAsToolStripMenuItem.Click += new System.EventHandler(this.saveProjectAsToolStripMenuItem_Click);
            // 
            // closeProjectToolStripMenuItem
            // 
            this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
            this.closeProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.closeProjectToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.closeProjectToolStripMenuItem.Text = "C&lose Project";
            this.closeProjectToolStripMenuItem.Click += new System.EventHandler(this.closeProjectToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.properties);
            this.groupBox1.Controls.Add(this.lstFileList);
            this.groupBox1.Controls.Add(this.cmbImageType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnBrowseWorkingFolder);
            this.groupBox1.Controls.Add(this.txtWorkingFolder);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chkPreview);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 610);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // properties
            // 
            this.properties.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.properties.Location = new System.Drawing.Point(12, 293);
            this.properties.Name = "properties";
            this.properties.Size = new System.Drawing.Size(416, 311);
            this.properties.TabIndex = 0;
            this.properties.ToolbarVisible = false;
            this.properties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.properties_PropertyValueChanged);
            // 
            // lstFileList
            // 
            this.lstFileList.Location = new System.Drawing.Point(10, 73);
            this.lstFileList.Name = "lstFileList";
            this.lstFileList.Size = new System.Drawing.Size(418, 214);
            this.lstFileList.TabIndex = 13;
            this.lstFileList.SelectedIndexChanged += new System.EventHandler(this.lstFileList_SelectedIndexChanged);
            this.lstFileList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstFileList_ItemCheck);
            // 
            // cmbImageType
            // 
            this.cmbImageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImageType.Items.AddRange(new object[] {
            "All Image Files",
            "JPEG Images (*.JPG, *.JPEG)",
            "Windows Bitmap Images (*.bmp)"});
            this.cmbImageType.Location = new System.Drawing.Point(103, 45);
            this.cmbImageType.Name = "cmbImageType";
            this.cmbImageType.Size = new System.Drawing.Size(176, 21);
            this.cmbImageType.TabIndex = 10;
            this.cmbImageType.SelectedIndexChanged += new System.EventHandler(this.cmbImageType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 14);
            this.label3.TabIndex = 12;
            this.label3.Text = "Image Type";
            // 
            // btnBrowseWorkingFolder
            // 
            this.btnBrowseWorkingFolder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowseWorkingFolder.Location = new System.Drawing.Point(406, 19);
            this.btnBrowseWorkingFolder.Name = "btnBrowseWorkingFolder";
            this.btnBrowseWorkingFolder.Size = new System.Drawing.Size(22, 20);
            this.btnBrowseWorkingFolder.TabIndex = 9;
            this.btnBrowseWorkingFolder.Text = "...";
            this.btnBrowseWorkingFolder.Click += new System.EventHandler(this.btnBrowseWorkingFolder_Click);
            // 
            // txtWorkingFolder
            // 
            this.txtWorkingFolder.Location = new System.Drawing.Point(103, 19);
            this.txtWorkingFolder.Name = "txtWorkingFolder";
            this.txtWorkingFolder.ReadOnly = true;
            this.txtWorkingFolder.Size = new System.Drawing.Size(295, 20);
            this.txtWorkingFolder.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 14);
            this.label1.TabIndex = 8;
            this.label1.Text = "Input Folder";
            // 
            // chkPreview
            // 
            this.chkPreview.Checked = true;
            this.chkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreview.Location = new System.Drawing.Point(283, 47);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(120, 20);
            this.chkPreview.TabIndex = 11;
            this.chkPreview.Text = "&Preview Image";
            this.chkPreview.CheckedChanged += new System.EventHandler(this.chkPreview_CheckedChanged);
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Location = new System.Drawing.Point(561, 367);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.ReadOnly = true;
            this.txtOutputFolder.Size = new System.Drawing.Size(299, 20);
            this.txtOutputFolder.TabIndex = 22;
            // 
            // btnBrowseOutputFolder
            // 
            this.btnBrowseOutputFolder.Enabled = false;
            this.btnBrowseOutputFolder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowseOutputFolder.Location = new System.Drawing.Point(866, 365);
            this.btnBrowseOutputFolder.Name = "btnBrowseOutputFolder";
            this.btnBrowseOutputFolder.Size = new System.Drawing.Size(22, 20);
            this.btnBrowseOutputFolder.TabIndex = 23;
            this.btnBrowseOutputFolder.Text = "...";
            this.btnBrowseOutputFolder.Click += new System.EventHandler(this.cmdBrowse3_Click);
            // 
            // picSource
            // 
            this.picSource.Location = new System.Drawing.Point(452, 27);
            this.picSource.Name = "picSource";
            this.picSource.Size = new System.Drawing.Size(436, 335);
            this.picSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSource.TabIndex = 3;
            this.picSource.TabStop = false;
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(0, 643);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(900, 23);
            this.progress.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(452, 371);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 14);
            this.label2.TabIndex = 28;
            this.label2.Text = "Output Folder";
            // 
            // chkBatchFileRename
            // 
            this.chkBatchFileRename.AutoSize = true;
            this.chkBatchFileRename.Location = new System.Drawing.Point(3, 6);
            this.chkBatchFileRename.Name = "chkBatchFileRename";
            this.chkBatchFileRename.Size = new System.Drawing.Size(181, 17);
            this.chkBatchFileRename.TabIndex = 4;
            this.chkBatchFileRename.Text = "Enable batch rename output files";
            this.chkBatchFileRename.UseVisualStyleBackColor = true;
            this.chkBatchFileRename.CheckedChanged += new System.EventHandler(this.chkBatchFileRename_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tabControl1);
            this.groupBox3.Location = new System.Drawing.Point(452, 392);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(436, 214);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output Options";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabRenameDest);
            this.tabControl1.Location = new System.Drawing.Point(3, 19);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(427, 189);
            this.tabControl1.TabIndex = 5;
            // 
            // tabRenameDest
            // 
            this.tabRenameDest.Controls.Add(this.propertyRenameOutput);
            this.tabRenameDest.Controls.Add(this.chkBatchFileRename);
            this.tabRenameDest.Location = new System.Drawing.Point(4, 22);
            this.tabRenameDest.Name = "tabRenameDest";
            this.tabRenameDest.Padding = new System.Windows.Forms.Padding(3);
            this.tabRenameDest.Size = new System.Drawing.Size(419, 163);
            this.tabRenameDest.TabIndex = 0;
            this.tabRenameDest.Text = "Rename Output Files";
            this.tabRenameDest.UseVisualStyleBackColor = true;
            // 
            // propertyRenameOutput
            // 
            this.propertyRenameOutput.Enabled = false;
            this.propertyRenameOutput.HelpVisible = false;
            this.propertyRenameOutput.Location = new System.Drawing.Point(7, 30);
            this.propertyRenameOutput.Name = "propertyRenameOutput";
            this.propertyRenameOutput.Size = new System.Drawing.Size(406, 130);
            this.propertyRenameOutput.TabIndex = 5;
            this.propertyRenameOutput.ToolbarVisible = false;
            this.propertyRenameOutput.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyRenameOutput_PropertyValueChanged);
            // 
            // btnMake
            // 
            this.btnMake.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMake.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMake.Location = new System.Drawing.Point(452, 612);
            this.btnMake.Name = "btnMake";
            this.btnMake.Size = new System.Drawing.Size(212, 25);
            this.btnMake.TabIndex = 29;
            this.btnMake.Text = "Make";
            this.btnMake.UseVisualStyleBackColor = true;
            this.btnMake.Click += new System.EventHandler(this.btnMake_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(676, 612);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(212, 25);
            this.btnStop.TabIndex = 30;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 662);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnMake);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnBrowseOutputFolder);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.picSource);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hardy Software Batch Image Processor";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSource)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabRenameDest.ResumeLayout(false);
            this.tabRenameDest.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox lstFileList;
        private System.Windows.Forms.ComboBox cmbImageType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowseWorkingFolder;
        private System.Windows.Forms.TextBox txtWorkingFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkPreview;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Button btnBrowseOutputFolder;
        private System.Windows.Forms.PictureBox picSource;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.PropertyGrid properties;
        private System.Windows.Forms.ToolStripMenuItem newProjecttoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkBatchFileRename;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnMake;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabRenameDest;
        private System.Windows.Forms.PropertyGrid propertyRenameOutput;
    }
}

