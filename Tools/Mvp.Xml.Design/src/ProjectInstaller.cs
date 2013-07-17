using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Mvp.Xml.Design
{
	/// <summary>
	/// Registers the project with COM.
	/// </summary>
	[RunInstaller(true)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class ProjectInstaller : System.Configuration.Install.Installer
	{
		public override void Install(IDictionary stateSaver)
		{
			base.Install(stateSaver);
			new RegistrationServices().RegisterAssembly(
				Assembly.GetExecutingAssembly(), 
				AssemblyRegistrationFlags.SetCodeBase);
		}

		public override void Rollback(IDictionary savedState)
		{
			base.Rollback (savedState);
			new RegistrationServices().UnregisterAssembly(
				Assembly.GetExecutingAssembly());
		}

		public override void Uninstall(IDictionary savedState)
		{
			base.Uninstall (savedState);
			new RegistrationServices().UnregisterAssembly(
				Assembly.GetExecutingAssembly());
		}
	}
}
