using System;
using System.Text;
using System.Windows.Forms.Design;

namespace Microsoft.Practices.RecipeFramework.MockServices
{
	sealed class MockUIService: IUIService
	{
		public MockUIService()
		{

		}

		#region IUIService Members

		bool IUIService.CanShowComponentEditor(object component)
		{
			return false;
		}

		System.Windows.Forms.IWin32Window IUIService.GetDialogOwnerWindow()
		{
			return null;
		}

		void IUIService.SetUIDirty()
		{
		}

		bool IUIService.ShowComponentEditor(object component, System.Windows.Forms.IWin32Window parent)
		{
			return false;
		}

		System.Windows.Forms.DialogResult IUIService.ShowDialog(System.Windows.Forms.Form form)
		{
			return System.Windows.Forms.DialogResult.Cancel;
		}

		void IUIService.ShowError(Exception ex, string message)
		{
		}

		void IUIService.ShowError(Exception ex)
		{
		}

		void IUIService.ShowError(string message)
		{
		}

		System.Windows.Forms.DialogResult IUIService.ShowMessage(string message, string caption, System.Windows.Forms.MessageBoxButtons buttons)
		{
			return System.Windows.Forms.DialogResult.Cancel;
		}

		void IUIService.ShowMessage(string message, string caption)
		{
		}

		void IUIService.ShowMessage(string message)
		{
		}

		bool IUIService.ShowToolWindow(Guid toolWindow)
		{
			return false;
		}

		System.Collections.IDictionary IUIService.Styles
		{
			get { return null; }
		}

		#endregion
	}
}
