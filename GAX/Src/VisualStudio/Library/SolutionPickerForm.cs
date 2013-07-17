#region Using Directives

using System;
using System.Windows.Forms;
using EnvDTE;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library
{
    /// <summary>
    /// Form that uses the <see cref="SolutionPickerControl"/> to allow selection of a valid target.
    /// </summary>
	internal partial class SolutionPickerForm : Form
	{
        SolutionPickerControl picker;

		/// <summary>
		/// Initializes an instance of the form.
		/// </summary>
        public SolutionPickerForm()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Initializes an instance of the form.
		/// </summary>
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
