using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.ComponentModel
{
	internal sealed partial class ErrorForm
	{
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorForm));
			this.labelMessage = new System.Windows.Forms.Label();
			this.exceptionDetails = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.buttonException = new System.Windows.Forms.Button();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// labelMessage
			// 
			this.labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelMessage.Location = new System.Drawing.Point(80, 20);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
			this.labelMessage.Size = new System.Drawing.Size(560, 97);
			this.labelMessage.TabIndex = 0;
			// 
			// exceptionDetails
			// 
			this.exceptionDetails.Location = new System.Drawing.Point(16, 191);
			this.exceptionDetails.Multiline = true;
			this.exceptionDetails.Name = "exceptionDetails";
			this.exceptionDetails.ReadOnly = true;
			this.exceptionDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.exceptionDetails.Size = new System.Drawing.Size(723, 142);
			this.exceptionDetails.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(163, 134);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(141, 30);
			this.button1.TabIndex = 2;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(323, 134);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(141, 30);
			this.button2.TabIndex = 3;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(483, 134);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(141, 30);
			this.button3.TabIndex = 4;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(19, 20);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(51, 47);
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			// 
			// buttonException
			// 
			this.buttonException.ImageIndex = 0;
			this.buttonException.ImageList = this.imageList1;
			this.buttonException.Location = new System.Drawing.Point(19, 143);
			this.buttonException.Name = "buttonException";
			this.buttonException.Size = new System.Drawing.Size(26, 21);
			this.buttonException.TabIndex = 6;
			this.buttonException.Click += new System.EventHandler(this.buttonException_Click);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "down.bmp");
			this.imageList1.Images.SetKeyName(1, "up.bmp");
			// 
			// toolTip1
			// 
			this.toolTip1.AutomaticDelay = 10;
			this.toolTip1.AutoPopDelay = 5000;
			this.toolTip1.InitialDelay = 10;
			this.toolTip1.ReshowDelay = 2;
			// 
			// ErrorForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(8, 19);
			this.ClientSize = new System.Drawing.Size(656, 176);
			this.ControlBox = false;
			this.Controls.Add(this.buttonException);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.exceptionDetails);
			this.Controls.Add(this.labelMessage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "ErrorForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ErrorDialog";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

        #endregion

        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.TextBox exceptionDetails;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonException;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip1;
	}
}
