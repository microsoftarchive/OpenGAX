using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.WizardFramework;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Services
{
    /// <summary>
    /// Summary description for CustomArgumentPanel
    /// </summary>
    [DesignerCategory("Form")]
	[ServiceDependency(typeof(IValueInfoService))]
	public class CustomArgumentPanel : ArgumentPanelTypeEditor, ISupportInitialize
    {
        #region Designer Stuff

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.valueEditor)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.invalidValuePictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// argumentLabel
			// 
			this.argumentLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.argumentLabel.Size = new System.Drawing.Size(420, 18);
			// 
			// valueEditor
			// 
			this.valueEditor.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.valueEditor.Size = new System.Drawing.Size(420, 18);
			this.valueEditor.TextBox.KeyPress +=new KeyPressEventHandler(TextBox_KeyPress);
			// 
			// invalidValuePictureBox
			// 
			this.invalidValuePictureBox.Location = new System.Drawing.Point(300, 19);
			this.invalidValuePictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
			// 
			// checkBox1
			// 
			this.checkBox1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.checkBox1.AutoSize = true;
			this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.checkBox1.Location = new System.Drawing.Point(173, 0);
			this.checkBox1.MinimumSize = new System.Drawing.Size(47, 35);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(47, 35);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Remove";
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// CustomArgumentPanel
			// 
			this.Controls.Add(this.checkBox1);
			this.Controls.SetChildIndex(this.checkBox1, 0);
			this.Controls.SetChildIndex(this.argumentLabel, 0);
			this.Controls.SetChildIndex(this.valueEditor, 0);
			this.Controls.SetChildIndex(this.invalidValuePictureBox, 0);
			((System.ComponentModel.ISupportInitialize)(this.valueEditor)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.invalidValuePictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		void TextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (this.checkBox1.Checked)
			{
				this.checkBox1.Checked = false;
			}
		}

        #endregion

		private System.Windows.Forms.CheckBox checkBox1;

        #endregion Designer Stuff

        #region Default Constructor

        /// <summary>
		/// Default constructor
		/// </summary>
		public CustomArgumentPanel()
		{
			InitializeComponent();
		}

		#endregion

		#region ISupportInitialize Members

		/// <summary>
		/// <see cref="ISupportInitialize.BeginInit"/>
		/// </summary>
		public override void BeginInit()
		{
			base.BeginInit();
		}

		/// <summary>
		/// <see cref="ISupportInitialize.BeginInit"/>
		/// </summary>
		public override void EndInit()
		{
			base.EndInit();
			if (this.FieldConfig != null)
			{
				this.toolTip.SetToolTip(this.checkBox1, string.Format(CultureInfo.CurrentCulture,
					Properties.Resources.ReferenceRestoreService_FieldCheckTooltip, this.FieldConfig.Label));
			}
		}

		#endregion

		#region Overrides

		/// <summary>
		/// <see cref="UpdateValue"/>
		/// </summary>
		/// <param name="newValue"></param>
		protected override void UpdateValue(object newValue)
		{
			base.UpdateValue(newValue);
			if ((newValue != null) && !(newValue is DummyDTE.EmptyDteElement))
			{
				if (this.checkBox1 != null)
				{
					this.checkBox1.Checked = false;
				}
			}
		}

		#endregion

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox1.Checked)
			{
				SetValue(DummyDTE.EmptyDteElement.EmptyValue());
			}
			else
			{
				SetValue(null);
			}
		}
	}
}
