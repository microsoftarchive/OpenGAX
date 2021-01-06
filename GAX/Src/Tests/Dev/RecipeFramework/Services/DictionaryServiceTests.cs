#region Using directives

using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Xml;

using Microsoft.Practices.RecipeFramework.MockServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms.Design;
using Microsoft.Practices.Common.Services;
using Microsoft.VisualStudio.ExtensionManager;

#endregion

namespace Microsoft.Practices.RecipeFramework.Services
{
    [TestClass]
	[DeploymentItem(@"Services\", @"Services\")]
	public class DictionaryServiceTests
	{
		[TestMethod]
		public void TestWizardValues()
		{
			var extensionManager = new MockExtensionManager();
			extensionManager.AddGuidancePackage(@"Services\DictionaryServiceTest.xml");

			IRecipeManagerService manager = new RecipeManager();
			((IServiceContainer)manager).AddService(typeof(IPersistenceService),
				new MockServices.MockPersistenceService());
			((IServiceContainer)manager).AddService(typeof(IUIService),
				new MockServices.MockUIService());

			manager.AddService(typeof(SVsExtensionManager), extensionManager);

			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(new XmlTextReader(Utils.MakeTestRelativePath(@"Services\DictionaryServiceTest.xml")));

			manager.EnablePackage(package);
			
			// Add dependant mock services.
			package.AddService(typeof(EnvDTE._DTE), new MockServices.MockDte());
            package.AddService(typeof(IValueGatheringService), new WizardFramework.WizardGatheringService());
			package.AddService(typeof(SVsExtensionManager), extensionManager);

			// DictionaryTestStep class is the one that performs the actual checks on the values.
			package.Execute(new MockObjects.MockReference("TestDictionary", "/Services"));
		}
	}

	#region DictionaryTestStep class

	[System.ComponentModel.DesignerCategory("Code")]
	internal class DictionaryTestStep : Services.MoveToNextPage 
	{
		public DictionaryTestStep(WizardFramework.WizardForm parent)
			: base(parent)
		{
		}

		protected override void Execute()
		{
			IDictionaryService dictionary = (IDictionaryService)
				GetService(typeof(IDictionaryService));

			dictionary.SetValue("AColor", Color.Black);
			dictionary.SetValue("ABoolean", true);
			dictionary.SetValue("AnEnum", PlatformID.WinCE);
			dictionary.SetValue("ADate", new DateTime(2004, 8, 24));
		}

		public override void OnDeactivated()
		{
			base.OnDeactivated();

			IDictionaryService dictionary = (IDictionaryService)
				GetService(typeof(IDictionaryService));

			Assert.AreEqual(
				Color.Black,
				(Color)dictionary.GetValue("AColor"));

			Assert.IsTrue((bool)dictionary.GetValue("ABoolean"));

			Assert.AreEqual(
				PlatformID.WinCE,
				(PlatformID)dictionary.GetValue("AnEnum"));

			Assert.AreEqual(
				new DateTime(2004, 8, 24),
				(DateTime)dictionary.GetValue("ADate"));
		}
	}

	#endregion DictionaryTestStep class
}