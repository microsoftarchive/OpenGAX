using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library
{
	internal partial class SolutionPickerForm
	{
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
            this.pnlContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContainer.Location = new System.Drawing.Point(4, 42);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(347, 304);
            this.pnlContainer.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cancelButton);
            this.panel2.Controls.Add(this.acceptButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(4, 345);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(347, 36);
            this.panel2.TabIndex = 2;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(267, 7);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.acceptButton.Location = new System.Drawing.Point(185, 7);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "&Accept";
            // 
            // messageText
            // 
            this.messageText.BackColor = System.Drawing.SystemColors.Control;
            this.messageText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageText.Dock = System.Windows.Forms.DockStyle.Top;
            this.messageText.Location = new System.Drawing.Point(4, 4);
            this.messageText.Multiline = true;
            this.messageText.Name = "messageText";
            this.messageText.Size = new System.Drawing.Size(347, 32);
            this.messageText.TabIndex = 3;
            this.messageText.Text = "Select the element to use as the target for the execution for reference that appl" +
                "ies to {0}.";
            // 
            // SolutionPickerForm
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(355, 385);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.messageText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SolutionPickerForm";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Target Element Selection";
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlContainer;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.TextBox messageText;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
	}
}
