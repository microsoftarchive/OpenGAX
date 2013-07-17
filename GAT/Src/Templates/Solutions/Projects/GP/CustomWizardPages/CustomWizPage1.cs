using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;
using System.ComponentModel.Design;

namespace $PackageNamespace$.CustomWizardPages
{
	/// <summary>
	/// Example of a class that is a custom wizard page
	/// </summary>
	public partial class CustomWizPage1 : CustomWizardPage
    {
        public CustomWizPage1(WizardForm parent)
            : base(parent)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            // TODO: Add any initialization after the InitializeComponent call
        }

        [RecipeArgument]
        public string Argument1
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
            if (string.IsNullOrEmpty(textBox1.Text.ToString()))
                dictionaryService.SetValue("Argument1", null);
            else
                dictionaryService.SetValue("Argument1", textBox1.Text.ToString());
        }
    }
}