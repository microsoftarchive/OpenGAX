using System;
using System.Windows.Forms.Design;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;

namespace $PackageNamespace$.Actions
{
	public class ShowCurrentSelection : Microsoft.Practices.RecipeFramework.Action
	{
		#region IAction Members

        public override void Execute()
        {
            IUIService uiService = GetService<IUIService>(true);
			string message = "Selection: " + GetService<DTE>(true).SelectedItems.Item(1).Name;
            if (uiService != null)
            {
                uiService.ShowMessage(message, "Selection");
            }
            else
            {
               System.Windows.Forms.MessageBox.Show(message, "Selection");
            }
        }

        public override void Undo()
        {
        }

		#endregion
    }
}