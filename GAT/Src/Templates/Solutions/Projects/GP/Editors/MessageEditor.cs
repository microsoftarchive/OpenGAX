using System;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace $PackageNamespace$.Editors
{
    public class MessageEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, 
            IServiceProvider provider, object value)
        {
            object svc = provider.GetService(typeof(IWindowsFormsEditorService));
            if (svc == null)
            {
                return base.EditValue(context, provider, value);
            }

            MessageEditorForm form = new MessageEditorForm();
            if (value != null)
            {
                form.messageText.Text = value.ToString();
            }
            if (((IWindowsFormsEditorService)svc).ShowDialog(form) == DialogResult.OK)
            {
                return form.messageText.Text;
            }
            else
            {
                return value;
            }
        }
    }
}
