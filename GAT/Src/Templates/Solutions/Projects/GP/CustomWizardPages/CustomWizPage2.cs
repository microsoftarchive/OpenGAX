using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;
using System.ComponentModel.Design;

namespace $PackageNamespace$.CustomWizardPages
{
	/// <summary>
	/// Example of a class that is a custom wizard page
	/// </summary>
	public partial class CustomWizPage2 : CustomWizardPage
    {
        public CustomWizPage2(WizardForm parent)
            : base(parent)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            // TODO: Add any initialization after the InitializeComponent call
        }


        [RecipeArgument]
        public string Argument2
        {
            set
            {
                if (value != null)
                {
                    this.textBox1.Text = value.ToString();
                }
                else
                {
                    this.textBox1.Text = "";
                }
            }
        }

        [RecipeArgument]
        public Font Argument3
        {
            set
            {
                this.valueEditor1.Value = value;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
            if (string.IsNullOrEmpty(textBox1.Text.ToString()))
                dictionaryService.SetValue("Argument2", null);
            else
                dictionaryService.SetValue("Argument2", textBox1.Text.ToString());
        }

        private void valueEditor1_ValueChanged(object sender, System.ComponentModel.Design.ComponentChangedEventArgs e)
        {
            IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
            dictionaryService.SetValue("Argument3", valueEditor1.Value);
            if (valueEditor1.Value == null)
            {
                this.label3.Visible = false;
            }
            else
            {
                this.label3.Font = (Font)valueEditor1.Value;
                this.label3.Visible = true;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
