using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;

namespace $PackageNamespace$.CustomWizardPages
{
	partial class CustomWizPage1 : CustomWizardPage
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

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(24, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter value of Argument1 in the field below";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Chocolate;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(26, 133);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(196, 23);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "Custom Page #1";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // CustomWizPage1
            // 
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Headline = "First Headline";
            this.InfoRTBoxSize = new System.Drawing.Size(550, 50);
            this.InfoRTBoxText = "You can change the size of this info-box and put text in it by manipulating Info" +
                "RTBox element of the related Designer class";
            this.Name = "CustomWizPage1";
            this.ShowInfoPanel = true;
            this.Size = new System.Drawing.Size(796, 451);
            this.Skippable = true;
            this.StepTitle = "First Custom Step";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.textBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}