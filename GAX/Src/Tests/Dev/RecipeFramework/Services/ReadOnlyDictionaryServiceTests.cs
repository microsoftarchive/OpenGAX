#region Using directives

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Xml;

using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.MockServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Common.Services;
using Microsoft.VisualStudio.ExtensionManager;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
    [TestClass]
	[DeploymentItem(@"Services\", @"Services\")]
	public class ReadOnlyDictionaryServiceTests
	{
		RecipeManager Manager;

		#region SetUp

		[TestInitialize]
		public void SetUp()
		{
			var extensionManager = new MockServices.MockExtensionManager();
			extensionManager.AddGuidancePackage(Utils.MakeTestRelativePath(@"Services\\ReadOnlyDictionaryServiceTests.xml"));

			Manager = new RecipeManager();
			Manager.AddService(typeof(IPersistenceService), new MockPersistenceService());
			Manager.AddService(typeof(EnvDTE._DTE), new MockServices.MockDte());
			Manager.AddService(typeof(SVsExtensionManager), extensionManager);
		}

		#endregion SetUp

		private void EnablePackage(GuidancePackage guidancePackage)
		{
			EnablePackage(guidancePackage,false);
		}

		private void EnablePackage(GuidancePackage guidancePackage,bool UseMock)
		{
			Manager.EnablePackage(guidancePackage);
			if (UseMock)
			{
                guidancePackage.AddService(typeof(IValueGatheringService), new MockServices.MockArgumentGatheringService());
			}
			else
			{
                guidancePackage.AddService(typeof(IValueGatheringService), new WizardFramework.WizardGatheringService());
			}
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void TryChangeValueProviderBefore()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(new XmlTextReader(
				Utils.MakeTestRelativePath("Services\\ReadOnlyDictionaryServiceTests.xml")));

			EnablePackage(package);
            try
            {
                package.Execute(new MockObjects.MockReference("TryChangeValueProviderBefore", ""));
            }
            catch (ValueProviderException aex)
            {
                throw aex.InnerException;
            }
		}

		[TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TryChangeValueProviderAfter()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(new XmlTextReader(
				Utils.MakeTestRelativePath("Services\\ReadOnlyDictionaryServiceTests.xml")));

			EnablePackage(package,true);
            try
            {
                package.Execute(new MockObjects.MockReference("TryChangeValueProviderAfter", ""));
            }
            catch (ValueProviderException aex)
            {
                throw aex.InnerException;
            }
        }

		[TestMethod]
		public void TryChangeValueProviderOnChange()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(new XmlTextReader(
				Utils.MakeTestRelativePath("Services\\ReadOnlyDictionaryServiceTests.xml")));

			EnablePackage(package);
			package.Execute(new MockObjects.MockReference("TryChangeValueProviderOnChange", ""));
		}

		[TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TryChangeAction()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(new XmlTextReader(
				Utils.MakeTestRelativePath("Services\\ReadOnlyDictionaryServiceTests.xml")));

			EnablePackage(package);
			try
			{
				package.Execute(new MockObjects.MockReference("TryChangeAction", ""));
			}
			catch (ActionExecutionException aex)
			{
				throw aex.InnerException;
			}
		}

		#region ChangeAction

		[System.ComponentModel.DesignerCategory("Code")]
		public class ChangeAction : Component, IAction
		{
			#region IAction Members

			public void Execute()
			{
				IDictionaryService dictionary = (IDictionaryService)GetService(typeof(IDictionaryService));
				dictionary.SetValue("A", "Hi there");
			}

			public void Undo()
			{
			}

			#endregion
		}

		#endregion ChangeAction

		#region TryChangeDictionaryValueProvider
		public class TryChangeDictionaryValueProviderBefore : ValueProvider
		{
			public override bool OnBeginRecipe(object currentValue, out object newValue)
			{
                newValue = null;
                IDictionaryService dictionary = (IDictionaryService)GetService(typeof(IDictionaryService));
				dictionary.SetValue("A", "Hi there");
				return true;
			}
		}

		public class TryChangeDictionaryValueProviderAfter : ValueProvider
		{
			public override bool OnBeforeActions(object currentValue, out object newValue)
			{
                newValue = null;
                IDictionaryService dictionary = (IDictionaryService)GetService(typeof(IDictionaryService));
				dictionary.SetValue("A", "Hi there");
				return true;
			}
		}

		public class TryChangeDictionaryValueProviderOnChange : ValueProvider
		{
			public override bool OnArgumentChanged(string changedArgumentName, object changedArgumentValue,
				object currentValue, out object newValue)
			{
                newValue = null;
                IDictionaryService dictionary = (IDictionaryService)GetService(typeof(IDictionaryService));
				dictionary.SetValue("A", "Hi there");
				return true;
			}
		}
		#endregion TryChangeDictionaryValueProvider

		#region ChangeValueWizard class

		internal class ChangeValueWizard : MoveToNextPage 
		{
			public ChangeValueWizard(WizardFramework.WizardForm parent)
				:base(parent)
			{
			}
			protected override void Execute()
			{
				bool ok = false;
				try
				{
					IDictionaryService dictionary = (IDictionaryService)
						GetService(typeof(IDictionaryService));
					dictionary.SetValue("A", "Bye");
				}
				catch (NotSupportedException)
				{
					ok = true;
				}

				if (!ok) Assert.Fail("Should have thrown NotSupportedException");
			}
		}

		#endregion ChangeValueWizard class
	}
}