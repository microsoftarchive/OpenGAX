#region Using Directives

using System;
using System.Windows.Forms;
using EnvDTE;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Editors
{
	internal sealed class SolutionPickerForm : Form
	{
        #region Designer Stuff

        private Panel pnlContainer;
        private Panel panel2;
        private Button cancelButton;
        private Button acceptButton;
        private TextBox messageText;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.pnlContainer = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.cancelButton = new System.Windows.Forms.Button();
			this.acceptButton = new System.Windows.Forms.Button();
			this.messageText = new System.Windows.Forms.TextBox();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlContainer
			// 
			this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlContainer.Location = new System.Drawing.Point(6, 55);
			this.pnlContainer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.pnlContainer.Name = "pnlContainer";
			this.pnlContainer.Size = new System.Drawing.Size(427, 551);
			this.pnlContainer.TabIndex = 1;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.cancelButton);
			this.panel2.Controls.Add(this.acceptButton);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(6, 551);
			this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(427, 55);
			this.panel2.TabIndex = 2;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(307, 11);
			this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(112, 35);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "&Cancel";
			// 
			// acceptButton
			// 
			this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.acceptButton.Location = new System.Drawing.Point(184, 11);
			this.acceptButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(112, 35);
			this.acceptButton.TabIndex = 0;
			this.acceptButton.Text = "&Accept";
			// 
			// messageText
			// 
			this.messageText.BackColor = System.Drawing.SystemColors.Control;
			this.messageText.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.messageText.Dock = System.Windows.Forms.DockStyle.Top;
			this.messageText.Location = new System.Drawing.Point(6, 6);
			this.messageText.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.messageText.Multiline = true;
			this.messageText.Name = "messageText";
			this.messageText.Size = new System.Drawing.Size(427, 49);
			this.messageText.TabIndex = 3;
			this.messageText.Text = "Select the element to use as the target for the execution for reference that appl" +
    "ies to {0}.";
			// 
			// SolutionPickerForm
			// 
			this.AcceptButton = this.acceptButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(439, 612);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.pnlContainer);
			this.Controls.Add(this.messageText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "SolutionPickerForm";
			this.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " Target Element Selection";
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        #endregion Designer Stuff

        SolutionPickerControl picker;

        public SolutionPickerForm()
        {
            InitializeComponent();
        }

        public SolutionPickerForm(DTE dte, IUnboundAssetReference reference)
        {
            InitializeComponent();
            this.picker = new SolutionPickerControl(dte, reference, DteHelper.GetTarget(dte), null);
            this.picker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picker.SelectionChanged += new SelectionChangedEventHandler(OnSelectionChanged);
            this.pnlContainer.Controls.Add(this.picker);
			string appliesTo;
			try
			{
				appliesTo = reference.AppliesTo;
			}
			catch (Exception)
			{
				appliesTo = Properties.Resources.Reference_AppliesToThrew;
				//It's better to continue executing the action and not stop because of the caption in the label
				//throw new RecipeExecutionException(reference.AssetName,
				//	string.Format(CultureInfo.CurrentCulture,
				//	Properties.Resources.Reference_InvalidAttributes, "AppliesTo"), e);
			}
			this.messageText.Text = String.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                this.messageText.Text,
				appliesTo);
        }

        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.acceptButton.Enabled = e.IsValid;
        }

        /// <summary>
        /// Gets the target selected in the treeview.
        /// </summary>
        public object SelectedTarget
        {
            get { return this.picker.SelectedTarget; }
        }
    }
}
