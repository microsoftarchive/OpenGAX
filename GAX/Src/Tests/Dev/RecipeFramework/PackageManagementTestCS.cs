#region Using directives

using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.XPath;

using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.MockServices;
using Microsoft.Practices.RecipeFramework.PackageManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Common.Services;
using System.Windows.Forms;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
	[TestClass]
	public class PackageManagementTestCS
	{
		RecipeManager Manager;
		// A set of flags to use during execution for several tests.
		internal static bool[] Flags = new bool[10];

		static readonly string InstallUtil = Path.Combine(Utils.GetClrInstallationDirectory(), "InstallUtil.exe");

		#region SetUp & TearDown

		[TestInitialize]
		public void SetUp()
		{
			Manager = new RecipeManager();
			Manager.AddService(typeof(IPersistenceService), new MockPersistenceService());
			Manager.AddService(typeof(EnvDTE._DTE), new MockServices.MockDte());
            Manager.AddService(typeof(IValueGatheringService), new WizardFramework.WizardGatheringService());
			Flags = new bool[10];

			Configuration.GuidancePackage package = GuidancePackage.ReadConfiguration(
				Utils.MakeTestRelativePath("PackageManagementTest.xml"));

			Manager.AddService(typeof(IHostService), new TestHost(package.Host));

			//// Install host
			//ManifestInstallerTest.InstallHost(package.Host, typeof(TestInstaller), false);
			//// Install package.
			//ManifestInstallerTest.InstallPackage(
			//    Utils.MakeTestRelativePath("PackageManagementTest.xml"), false);
		}

		[TestCleanup]
		public void TearDown()
		{
			//// Install package.
			//ManifestInstallerTest.UninstallPackage(
			//    Utils.MakeTestRelativePath("PackageManagementTest.xml"), false);
			//Configuration.GuidancePackage package = GuidancePackage.ReadConfiguration(
			//    Utils.MakeTestRelativePath("PackageManagementTest.xml"));
			//ManifestInstallerTest.UninstallHost(package.Host);
		}

		#endregion SetUp & TearDown

		bool closed = false;

		[TestMethod]
		// Remove ignore to test non-modal behavior
		[Ignore]
		public void TestNoModal()
		{
			closed = false;
			PackageManager form = new PackageManager(Manager, null);
			form.FormClosed += new System.Windows.Forms.FormClosedEventHandler(OnFormClosed);
			form.Show();

			while (!closed)
			{
				System.Windows.Forms.Application.DoEvents();
				System.Threading.Thread.Sleep(10);
			}

		}
		void OnFormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
		{
			closed = true;
		}

		[TestMethod]
		//"Talk with Ashish about what's wrong in this  code! Second window is never found :S"
		[Ignore]
		public void TestReferences()
		{
            //We need to move this test outside this assembly
            //PackageManager form = new PackageManager(Manager, null);
            //Maui.Core.App app = new Maui.Core.App(System.Diagnostics.Process.GetCurrentProcess().Id);
            //Maui.Core.Window mainwnd = new Maui.Core.Window(form.Handle);
            //form.Show();
            //mainwnd.WaitForResponse();

            //Maui.Core.WinControls.ListView references = new Maui.Core.WinControls.ListView(mainwnd, "lstRecipes");
            //Assert.AreEqual(0, references.Items.Count);

            //Maui.Core.WinControls.Button enabledisable = new Maui.Core.WinControls.Button(mainwnd, "packagesButton");
            //enabledisable.Click(Maui.Core.MouseClickType.SingleClick, Maui.Core.MouseFlags.LeftButton);
            //enabledisable.WaitForResponse();
            //mainwnd.WaitForResponse();
            //mainwnd.WaitForResponse();

            //Maui.Core.Window packageswnd = new Maui.Core.Window("Enable and Disable Packages", Maui.Core.Utilities.StringMatchSyntax.ExactMatch);
            //Maui.Core.WinControls.ListView packages = new Maui.Core.WinControls.ListView(packageswnd, "lstPackages");

            //Assert.AreEqual(1, packages.Items.Count);
            //Assert.IsFalse(packages.Items[0].Checked);

            //packages.Items[0].Select();
            //packageswnd.WaitForResponse();

            //Maui.Core.WinControls.Button explorebutton = new Maui.Core.WinControls.Button(packageswnd, "btnExplore");
            //explorebutton.Click(Maui.Core.MouseClickType.SingleClick, Maui.Core.MouseFlags.LeftButton);
            //explorebutton.WaitForResponse();

            //Maui.Core.Window browsewnd = new Maui.Core.Window("Package Viewer", Maui.Core.Utilities.StringMatchSyntax.ExactMatch);
            //browsewnd.WaitForResponse();

            //Maui.Core.WinControls.TreeView tree = new Maui.Core.WinControls.TreeView(browsewnd, "tvRecipes");
            //Assert.AreEqual(1, tree.ChildCount);

            //tree.Nodes[0].Expand();
            //browsewnd.WaitForResponse();

            //Assert.AreEqual(2, tree.Nodes[0].ChildCount);
            //tree.ExpandAll();
            //browsewnd.WaitForResponse();

            //tree.Nodes[0].Nodes[0].Nodes[2].Select();
            //browsewnd.WaitForResponse();

            //Assert.AreEqual("Creates the references required for this package.",
            //    new Maui.Core.WinControls.TextBox(browsewnd, "txtDescription").Text);

            //tree.Nodes[0].Nodes[1].Nodes[0].Select();
            //browsewnd.WaitForResponse();
            //Assert.AreEqual("This is a test asset added by the host service",
            //    new Maui.Core.WinControls.TextBox(browsewnd, "txtDescription").Text);

            //new Maui.Core.WinControls.Button(browsewnd, "CloseButton").Click();

            //packageswnd.WaitForResponse();
            //packages.Items[0].Checked = true;
            //packageswnd.WaitForResponse();
            //new Maui.Core.WinControls.Button(packageswnd, "btnOK").Click(Maui.Core.MouseClickType.SingleClick, Maui.Core.MouseFlags.LeftButton);

            //// Binding recipe executed.
            //Assert.IsTrue(Flags[0]);
            //// Not undone.
            //Assert.IsFalse(Flags[1]);
            //// 2 new references in place.
            //Assert.AreEqual(2, references.Items.Count);

            //references.Items[0].Select();
            //mainwnd.WaitForResponse();
            //new Maui.Core.WinControls.Button(mainwnd, "executeButton").Click(Maui.Core.MouseClickType.SingleClick, Maui.Core.MouseFlags.LeftButton);
            //mainwnd.WaitForResponse();

            //// Action executed.
            //Assert.IsTrue(Flags[2]);
            //// Not undone.
            //Assert.IsFalse(Flags[3]);
		}

		[ServiceDependency(typeof(IAssetReferenceService))]
		public class BindingAction : Action
		{
			#region IAction Members

			public override void Execute()
			{
				IAssetReferenceService refsvc = (IAssetReferenceService)
					GetService(typeof(IAssetReferenceService));
				refsvc.Add(new MockObjects.MockReference("ValidateSolution", "/"));
				refsvc.Add(new MockObjects.MockReference("PublishServices", "/Services/Accounting"));
				refsvc.Add(new MyUnboundRecipeReference("AddXmlSerializationClass"));

				Flags[0] = true;
			}

			public override void Undo()
			{
				Flags[1] = true;
			}

			#endregion

			private class MyUnboundRecipeReference : UnboundRecipeReference
			{
				public MyUnboundRecipeReference(string recipe)
					: base(recipe)
				{ 
				}

				public override string AppliesTo
				{
					get { return "All C# Classes"; }
				}

				public override bool IsEnabledFor(object target)
				{
					throw new Exception("The method or operation is not implemented.");
				}
			}

		}

		public class TestAction : IAction
		{
			#region IAction Members

			public void Execute()
			{
				Flags[2] = true;
			}

			public void Undo()
			{
				Flags[3] = true;
			}

			#endregion
		}

		private class TestHost : Services.IHostService
		{
			public TestHost(string hostName)
			{
				hostname = hostName;
			}

			#region IHostService Members

			string hostname;

			public string HostName
			{
				get { return hostname; }
			}

			public IAssetDescription[] GetHostAssets(string packagePath, 
				Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage packageConfiguration)
			{
				return new AssetDescription[] {
					new AssetDescription("Templates", "Test asset", "This is a test asset added by the host service"),
					new AssetDescription(@"Templates\Item Templates\Console Applications", "New Console", "Test console"),
					new AssetDescription(@"Templates\Project Templates\Web Applications", "New Console", "Test console"),
					new AssetDescription(@"Templates\Item Templates\Console Applications", "Another Console", "Test console")
				};
			}

			public bool SelectTarget(object target) { return true; }

            public bool SelectTarget(IWin32Window ownerWindow, IUnboundAssetReference forReference) { return true; }

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
