using System;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;

namespace HardySoft.UI.BatchImageProcessor {
    public abstract class IntegerRangeEditor : UITypeEditor {
        protected abstract int MinValue {
            get;
        }

        protected abstract int MaxValue {
            get;
        }

        public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider,
            object value) {
            //use IWindowsFormsEditorService object to display a control in the dropdown area
            IWindowsFormsEditorService formService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (formService == null) {
                return null;
            }
            TrackBar bar = new TrackBar();
            bar.Orientation = Orientation.Horizontal;
            bar.LargeChange = (MaxValue - MinValue) / 20;
            bar.SmallChange = (MaxValue - MinValue) / 30;
            //bar.Size = new Size(120, 60);
            bar.TickFrequency = 10;
            bar.SetRange(MinValue, MaxValue);
            bar.Value = (int)value;
            formService.DropDownControl(bar);
            return bar.Value;
        }
    }

    public class ElementColorValueRangeEditor : IntegerRangeEditor {
        protected override int MinValue {
            get { return 0; }
        }

        protected override int MaxValue {
            get { return 255; }
        }
    }

    public class CompressionRatioValueRangeEditor : IntegerRangeEditor {
        protected override int MinValue {
            get { return 0; }
        }

        protected override int MaxValue {
            get { return 100; }
        }
    }
}