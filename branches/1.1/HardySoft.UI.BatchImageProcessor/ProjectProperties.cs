using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace HardySoft.UI.BatchImageProcessor {
    [Serializable]
    class ProjectProperties {
        private WaterMarkProperties properties;
        private BatchRenameProperties renameProperties;
        private bool preview;
        private string imageFolder, outputFolder;
        private int imageTypeSelectIndex = 0;
        private bool batchRenameOutputFiles;
        private List<string> selectedSourceFiles = new List<string>();

        public ProjectProperties () {
            properties = new WaterMarkProperties();
            renameProperties = new BatchRenameProperties();
        }

        public List<string> SelectedSourceFiles {
            get {
                return selectedSourceFiles;
            }
        }

        public void AddSourceFile (string fileName) {
            if (selectedSourceFiles == null) {
                selectedSourceFiles = new List<string>();
            }
            if (! selectedSourceFiles.Contains(fileName)) {
                selectedSourceFiles.Add(fileName);
            }
        }

        public void RemoveSourceFile (string fileName) {
            if (selectedSourceFiles == null) {
                selectedSourceFiles = new List<string>();
            }
            selectedSourceFiles.Remove(fileName);
        }

        public void ClearAllSourceFiles () {
            if (selectedSourceFiles == null) {
                selectedSourceFiles = new List<string>();
            }
            selectedSourceFiles.Clear();
        }

        public WaterMarkProperties Properties {
            get {
                return properties;
            }
            set {
                properties = value;
            }
        }

        public bool BatchRenameOutputFiles {
            get { return batchRenameOutputFiles; }
            set { batchRenameOutputFiles = value; }
        }

        public BatchRenameProperties RenameProperties {
            get {
                return renameProperties;
            }
            set {
                renameProperties = value;
            }
        }

        public string ImageFolder {
            get {
                return imageFolder;
            }
            set {
                imageFolder = value;
            }
        }

        public bool Preview {
            get {
                return preview;
            }
            set {
                preview = value;
            }
        }

        public string OutputFolder {
            get { return outputFolder; }
            set { outputFolder = value; }
        }

        public int ImageTypeSelectIndex {
            get { return imageTypeSelectIndex; }
            set { imageTypeSelectIndex = value; }
        }
    }
}