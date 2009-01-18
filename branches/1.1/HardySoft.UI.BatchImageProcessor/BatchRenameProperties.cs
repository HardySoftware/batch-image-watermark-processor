using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Reflection;

namespace HardySoft.UI.BatchImageProcessor {
    [Serializable]
    public class BatchRenameProperties {
        private bool replaceWhiteSpacesWithUnderscore;
        private bool lowerCaseFileName, upperCaseFileName;
        private string fileNamePrefix, fileNameSuffix;
        private int startWithNumber, numberPadding;

        public BatchRenameProperties () {
            setDefaultValues();
        }

        private void setDefaultValues () {
            PropertyInfo[] props = this.GetType().GetProperties();
            for (int i = 0; i < props.Length; i++) {
                object[] attrs = props[i].GetCustomAttributes(typeof(DefaultValueAttribute), false);
                if (attrs.Length > 0) {
                    DefaultValueAttribute attr = (DefaultValueAttribute)attrs[0];
                    props[i].SetValue(this, attr.Value, null);
                }
            }
        }

        [CategoryAttribute("Batch Rename"),
        DescriptionAttribute("Indicates whether to replace white spaces in file name with underscores."),
        DefaultValueAttribute(false)]
        public bool ReplaceWhiteSpacesWithUnderscore {
            get { return replaceWhiteSpacesWithUnderscore; }
            set { replaceWhiteSpacesWithUnderscore = value; }
        }

        [CategoryAttribute("Batch Rename"),
        DescriptionAttribute("Indicates whether to set file name to lower case."),
        DefaultValueAttribute(false)]
        public bool LowerCaseFileName {
            get { return lowerCaseFileName; }
            set { lowerCaseFileName = value; }
        }

        [CategoryAttribute("Batch Rename"),
        DescriptionAttribute("Indicates whether to set file name to lower case."),
        DefaultValueAttribute(false)]
        public bool UpperCaseFileName {
            get { return upperCaseFileName; }
            set { upperCaseFileName = value; }
        }

        [CategoryAttribute("Batch Rename"),
        DescriptionAttribute("Prefix of new file name.")
        ]
        public string FileNamePrefix {
            get {
                if (fileNamePrefix == null) {
                    return string.Empty;
                } else {
                    return fileNamePrefix.Trim();
                }
            }
            set { fileNamePrefix = value.Trim(); }
        }

        [CategoryAttribute("Batch Rename"),
        DescriptionAttribute("Suffix of new file name.")
        ]
        public string FileNameSuffix {
            get {
                if (fileNameSuffix == null) {
                    return string.Empty;
                } else {
                    return fileNameSuffix.Trim();
                }
            }
            set {
                fileNameSuffix = value.Trim();
            }
        }

        [CategoryAttribute("Batch Rename"),
        DescriptionAttribute("Indicates the start number of the new batch."),
        DefaultValueAttribute(0)]
        public int StartWithNumber {
            get { return startWithNumber; }
            set { startWithNumber = value; }
        }

        [CategoryAttribute("Batch Rename"),
        DescriptionAttribute("Indicates the padding of number in file name."),
        DefaultValueAttribute(3)]
        public int NumberPadding {
            get { return numberPadding; }
            set { numberPadding = value; }
        }
    }
}
