using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;

namespace $PackageNamespace$.CustomWizardPages
{
	partial class CustomWizPage2
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Drawing.FontConverter fontConverter1 = new System.Drawing.FontConverter();
            System.Drawing.Design.FontEditor fontEditor1 = new System.Drawing.Design.FontEditor();
            this.valueEditor1 = new Microsoft.Practices.WizardFramework.ValueEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.valueEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // valueEditor1
            // 
            this.valueEditor1.BackColor = System.Drawing.SystemColors.Info;
            this.valueEditor1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.valueEditor1.ConverterInstance = fontConverter1;
            this.valueEditor1.EditorInstance = fontEditor1;
            this.valueEditor1.EditorType = typeof(System.Drawing.Design.FontEditor);
            this.valueEditor1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.valueEditor1.Location = new System.Drawing.Point(29, 150);
            this.valueEditor1.Name = "valueEditor1";
            this.valueEditor1.ReadOnly = false;
            this.valueEditor1.Size = new System.Drawing.Size(328, 18);
            this.valueEditor1.TabIndex = 0;
            this.valueEditor1.ToolTip = "Tooltip for argument";
            this.valueEditor1.ValueRequired = true;
            this.valueEditor1.ValueType = typeof(System.Drawing.Font);
            this.valueEditor1.ValueChanged += new System.ComponentModel.Design.ComponentChangedEventHandler(this.valueEditor1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(24, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter value of Argument2 in the field below";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Aquamarine;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Blue;
            this.textBox1.Location = new System.Drawing.Point(28, 50);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(329, 38);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "Custom Page 2";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(28, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(279, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Specify Font (value of Argument3)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(27, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(351, 25);
            this.label3.TabIndex = 11;
            this.label3.Text = "Sample of the font you have chosen";
            // 
            // CustomWizPage2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.valueEditor1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Headline = "Second Headline";
            this.InfoRTBoxSize = new System.Drawing.Size(500, 50);
            this.InfoRTBoxText = "You can change the size of this info-box and put text in it by manipulating Info" +
                "RTBox element of the related Designer class";
            this.Name = "CustomWizPage2";
            this.ShowInfoPanel = true;
            this.Size = new System.Drawing.Size(527, 350);
            this.Skippable = true;
            this.StepTitle = "Second custom step";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.textBox1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.valueEditor1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.valueEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Microsoft.Practices.WizardFramework.ValueEditor valueEditor1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private Label label2;
        private Label label3;
    }
}