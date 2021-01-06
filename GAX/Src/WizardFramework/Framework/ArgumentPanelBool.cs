using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Microsoft.Practices.Common.Services;
using System.ComponentModel.Design;

namespace Microsoft.Practices.WizardFramework
{
	/// <summary>
	/// <see cref="ArgumentPanel"/> to edit a boolean value
	/// </summary>
	public class ArgumentPanelBool : ArgumentPanel, ISupportInitialize
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
			this.checkBox = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.invalidValuePictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// invalidValuePictureBox
			// 
			this.invalidValuePictureBox.Location = new System.Drawing.Point(202, 1);
			// 
			// checkBox
			// 
			this.checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox.Location = new System.Drawing.Point(0, 0);
			this.checkBox.Name = "checkBox";
			this.checkBox.Size = new System.Drawing.Size(200, 18);
			this.checkBox.TabIndex = 0;
			this.checkBox.Text = "checkBox1";
			this.checkBox.CheckedChanged += new System.EventHandler(this.CheckedChanged);
			// 
			// ArgumentPanelBool
			// 
			this.Controls.Add(this.checkBox);
			this.InvalidValueMessage = "Invalid Value";
			this.Size = new System.Drawing.Size(218, 22);
			this.Controls.SetChildIndex(this.checkBox, 0);
			this.Controls.SetChildIndex(this.invalidValuePictureBox, 0);
			((System.ComponentModel.ISupportInitialize)(this.invalidValuePictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.CheckBox checkBox;

        #endregion Designer Stuff

        #region Constructor

        /// <summary>
		/// Default constructor
		/// </summary>
		public ArgumentPanelBool()
		{
			InitializeComponent();
		}

		#endregion

		#region Events

		private void CheckedChanged(object sender,EventArgs e)
		{
			SetValue(checkBox.Checked);			
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
		/// <see cref="ISupportInitialize.EndInit"/>
		/// </summary>
		public override void EndInit()
		{
			base.EndInit();
			//If we are still initializing , then do nothing
			if (IsInitializing)
			{
				return;
			}
			//OK: The base class is now initialized
			this.checkBox.Text = this.FieldConfig.Label;
			this.ToolTip.SetToolTip(checkBox, this.FieldConfig.Tooltip);

			IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
			if (dictionaryService != null)
			{
				object val = dictionaryService.GetValue(this.FieldConfig.ValueName);
				if (val == null)
				{
					SetValue(false);
				}
			}
            if (this.FieldConfig.ReadOnly)
            {
                this.checkBox.Enabled = false;
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
			//For boolean values, assume that a null value if false
			if (newValue == null)
			{
				checkBox.Checked = false;
			}
			else if (newValue is Nullable<bool>)
			{
				checkBox.Checked = ((Nullable<bool>)newValue).Value;
			}
			else
			{
				checkBox.Checked = (bool)newValue;
			}
		}

		#endregion
	}
}
