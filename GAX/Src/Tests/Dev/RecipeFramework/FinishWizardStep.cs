#region Using directives

using System;
using System.Text;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// A step that causes the wizard to be finished inmediately.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	internal class FinishWizardStep : Services.MoveToNextPage 
	{
		public FinishWizardStep(WizardFramework.WizardForm parent)
			: base(parent)
		{
		}

		protected override void Execute()
		{
		}
	}
}
