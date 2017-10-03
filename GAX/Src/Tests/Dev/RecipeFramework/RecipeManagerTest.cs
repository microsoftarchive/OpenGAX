#region Using directives

using System;
using System.IO;
using System.Xml;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.MockServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Common.Services;
using System.Windows.Forms;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
    [TestClass]
	[Ignore]
	public class RecipeManagerTest
	{
		RecipeManager Manager;
		// A set of flags to use during execution for several tests.
		internal static bool[] Flags = new bool[10];

		static readonly string InstallUtil = Path.Combine(Utils.GetClrInstallationDirectory(), "InstallUtil.exe");

		#region SetUp

		[TestInitialize]
		public void SetUp()
		{
			Manager = new RecipeManager();
			Manager.AddService(typeof(IPersistenceService), new MockPersistenceService());
			Manager.AddService(typeof(EnvDTE._DTE), new MockServices.MockDte());
            Manager.AddService(typeof(IValueGatheringService), new WizardFramework.WizardGatheringService());
			Flags = new bool[10];
		}

		#endregion SetUp

		[TestMethod]
		public void LoadPackage()
		{
			RecipeManager mgr = new RecipeManager();
			// Setup dependent services.
			mgr.RemoveService(typeof(System.ComponentModel.Design.ITypeResolutionService));
			mgr.AddService(typeof(System.ComponentModel.Design.ITypeResolutionService), new MockServices.MockTypeResolutionService());
			mgr.AddService(typeof(Services.IPersistenceService), new MockServices.MockPersistenceService());
			Configuration.GuidancePackage config = new Configuration.GuidancePackage();
			config.Name = "Test package";
            config.SchemaVersion = "1.0";
			config.Caption = "Test package caption";
			GuidancePackage package = new GuidancePackage(config);
			mgr.Add(package);
		}

		[TestMethod]
		public void AddPackage()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(new XmlTextReader(Utils.MakeTestRelativePath("RecipeTest.xml")));

			Manager.Add(package);
			Assert.IsNotNull(Manager.GetPackage(package.Configuration.Name));
		}

		[TestMethod]
		public void EnablePackage()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(new XmlTextReader(Utils.MakeTestRelativePath("RecipeTest.xml")));

			Manager.EnablePackage(package);
			Assert.IsNotNull(Manager.GetPackage(package.Configuration.Name));
		}

		//[TestMethod]
		//public void InstallAndEnablePackage()
		//{
		//    Configuration.GuidancePackage package = GuidancePackage.ReadConfiguration(
		//        Utils.MakeTestRelativePath("RecipeTest.xml"));

		//    // Ensure host
		//    ManifestInstallerTest.InstallHost(package.Host, typeof(TestInstaller), false);
		//    try
		//    {
		//        // Install previously registered package.
		//        ManifestInstallerTest.UninstallPackage(Utils.MakeTestRelativePath("RecipeTest.xml"), false);

		//        // Now install it.
		//        string output = ManifestInstallerTest.InstallPackage(Utils.MakeTestRelativePath("RecipeTest.xml"), false);

		//        // Check that the installer class was called.
		//        if (output.IndexOf(typeof(TestInstaller).AssemblyQualifiedName) == -1)
		//        {
		//            Console.Write(output);
		//            Assert.Fail(string.Format("Installer for package was not run. When try to find \"{0}\"", typeof(TestInstaller).AssemblyQualifiedName));
		//        }
		//        RecipeManager.ResetMainManifest();
		//        Manager.EnablePackage(package.Name);
		//    }
		//    finally
		//    {
		//        ManifestInstallerTest.UninstallPackage(Utils.MakeTestRelativePath("RecipeTest.xml"), false);
		//        ManifestInstallerTest.UninstallHost(package.Host);
		//    }
		//}

		[TestMethod]
		public void EnableDisablePackage()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(new XmlTextReader(Utils.MakeTestRelativePath("RecipeTest.xml")));

			Manager.EnablePackage(package);
			package.Disposed += new EventHandler(OnPackageDisposed);
			Manager.DisablePackage(package.Configuration.Name);

			Assert.IsTrue(Flags[0], "Package was not disposed");
		}

		[TestMethod]
		public void ExecuteHost()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(new XmlTextReader(Utils.MakeTestRelativePath("RecipeTest.xml")));

			Manager.AddService(typeof(Services.IHostService),
				new TestHost(Manager, false, false, false));

			Manager.EnablePackage(package);
			Assert.IsTrue(Flags[0], "Host was not called at package enabling time");
			Manager.DisablePackage(package.Configuration.Name);
			Assert.IsTrue(Flags[1], "Host was not called at package disabling time");
		}

		[TestMethod]
		public void EnableCancel()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(Utils.MakeTestRelativePath("RecipeTest.xml"));

			Manager.AddService(typeof(Services.IHostService),
				new TestHost(Manager, true, false, false));

			Manager.EnablePackage(package);
			Assert.IsNull(Manager.GetPackage(package.Configuration.Name),
				"Host shouldn't have let package to get enabled");
		}

		[TestMethod]
		public void DisableCancel()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(Utils.MakeTestRelativePath("RecipeTest.xml"));

			Manager.AddService(typeof(Services.IHostService),
				new TestHost(Manager, false, true, false));

			Manager.EnablePackage(package);
			Assert.IsNotNull(Manager.GetPackage(package.Configuration.Name),
				"Package was not enabled");
			Manager.DisablePackage(package.Configuration.Name);
			Assert.IsNotNull(Manager.GetPackage(package.Configuration.Name),
				"Host shouldn't have let package to get disabled");
		}

		void OnPackageDisposed(object sender, EventArgs e)
		{
			Flags[0] = true;
		}

		public class ExecutedByNameAction : IAction
		{
			#region IAction Members

			public void Execute()
			{
				Flags[0] = true;
			}

			public void Undo()
			{
				Flags[1] = true;
			}

			#endregion
		}

		private class TestHost : Services.IHostService
		{
			bool cancelEnable;
			bool cancelDisable;
			bool cancelExecute;

			public TestHost(RecipeManager manager, bool cancelEnable, bool cancelDisable, bool cancelExecute)
			{
				this.cancelDisable = cancelDisable;
				this.cancelEnable = cancelEnable;
				this.cancelExecute = cancelExecute;
				manager.EnablingPackage += new CancelPackageEventHandler(EnablingPackage);
				manager.DisablingPackage += new CancelPackageEventHandler(DisablingPackage);
			}

			public void EnablingPackage(object sender, CancelPackageEventArgs args)
			{
				Flags[0] = true;
				args.Cancel = cancelEnable;
			}

			public void DisablingPackage(object sender, CancelPackageEventArgs args)
			{
				Flags[1] = true;
				args.Cancel = cancelDisable;
			}

			#region IHostService Members

			public string HostName
			{
				get { return "TestHost"; }
			}

			public IAssetDescription[] GetHostAssets(string packagePath, 
				Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage packageConfiguration)
			{
				return null;
			}

            public bool SelectTarget(object target)
            {
                return true;
            }

            public bool SelectTarget(IWin32Window ownerWindow, IUnboundAssetReference forReference)
            {
                return true;
            }

			#endregion
		}

		public class TestInstaller : IHostInstaller
		{
			#region IHostInstaller Members

			public void InstallPackage(System.Configuration.Install.InstallContext context, Configuration.GuidancePackage packageConfig)
			{
				context.LogMessage(this.GetType().AssemblyQualifiedName);
			}

			public void UninstallPackage(System.Configuration.Install.InstallContext context, Configuration.GuidancePackage packageConfig)
			{
				context.LogMessage(this.GetType().AssemblyQualifiedName);
			}

			public void InstallHost(System.Configuration.Install.InstallContext context)
			{
			}

			public void UninstallHost(System.Configuration.Install.InstallContext context)
			{
			}

			#endregion
		}
	}
}