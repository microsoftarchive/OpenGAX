using System;
using System.Text;
using Microsoft.Practices.WizardFramework;
using System.Windows.Forms;

namespace Microsoft.Practices.RecipeFramework.Services
{
	[System.ComponentModel.DesignerCategory("Code")]
	internal abstract class MoveToNextPage : WizardPageFromConfig
	{
		public MoveToNextPage(WizardForm parent)
			:base(parent)
		{
		}

		public override void EndInit()
		{
			base.EndInit();
			//this.Load += new EventHandler(OnLoad);
			this.VisibleChanged += new EventHandler(OnVisibleChanged);
		}

		protected abstract void Execute();

		void OnLoad(object sender, EventArgs e)
		{
			Execute();
		}

		bool executed = false;

		void OnVisibleChanged(object sender, EventArgs e)
		{
			if (!executed)
			{
				Execute();
				executed = true;
			}
			this.Wizard.DefaultButton = Microsoft.WizardFramework.ButtonType.Next;
			Button acceptButton = (Button)this.Wizard.AcceptButton;
			if (acceptButton == null || !acceptButton.Enabled)
			{
				// If the AcceptoButton is null, them click on the Finish Button
				this.Wizard.DefaultButton = Microsoft.WizardFramework.ButtonType.Finish;
				acceptButton = (Button)this.Wizard.AcceptButton;
			}
			acceptButton.PerformClick();
		}
	}
}
