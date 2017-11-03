#region Using directives

using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Xml;

using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.MockServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms.Design;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.ExtensionManager;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
	[TestClass]
	[DeploymentItem("RecipeTest.xml")]
	[DeploymentItem("RecipeTest-Binding.xml")]
	[DeploymentItem("RecipeTest-BindingInvalid.xml")]	
	public class RecipeTests
	{
		RecipeManager Manager;
		// A set of flags to use during execution for several tests.
		internal static bool[] Flags = new bool[10];

		#region SetUp

		[TestInitialize]
		public void SetUp()
		{
			var extensionManager = new MockExtensionManager();
			extensionManager.AddGuidancePackage("RecipeTest.xml");
			extensionManager.AddGuidancePackage("RecipeTest-Binding.xml");
			extensionManager.AddGuidancePackage("RecipeTest-BindingInvalid.xml");
			
			Manager = new RecipeManager();
			Manager.AddService(typeof(IPersistenceService), new MockPersistenceService());
			Manager.AddService(typeof(EnvDTE._DTE), new MockServices.MockDte());
			Manager.AddService(typeof(IValueGatheringService), new WizardFramework.WizardGatheringService());
			Manager.AddService(typeof(SVsExtensionManager), extensionManager);

			Flags = new bool[10];
		}

		#endregion SetUp

        [TestMethod]
        public void EmbeddedResourceTest()
        {
            Stream resource = this.GetType().Assembly.GetManifestResourceStream("Microsoft.Practices.RecipeFramework.RecipeTest.xml");
            Assert.IsNotNull(resource);
            resource.Close();

            resource = this.GetType().Assembly.GetManifestResourceStream("Microsoft.Practices.RecipeFramework.RecipeTest.xml");
            using (StreamReader sr = new StreamReader(resource))
            {
                string data = sr.ReadToEnd();
                // Console.WriteLine(data);
            }

            resource = this.GetType().Assembly.GetManifestResourceStream("Microsoft.Practices.RecipeFramework.RecipeTest.xml");
            using (StreamReader sr = new StreamReader(resource))
            {
                string data = sr.ReadToEnd();
                // Console.WriteLine(data);
            }
        }

		//[TestMethod]
		//public void ExecuteCallsOnBeginRecipeAlways()
		//{
		//    Configuration.GuidancePackage package = new Configuration.GuidancePackage();
		//    package.Name = "FooPackage";
		//    package.Guid = Guid.NewGuid().ToString();
		//    Configuration.Recipe config = new Configuration.Recipe();
		//    config.Name = "Test";
		//    package.Recipes = new Configuration.Recipe[] { config };

		//    Configuration.Argument argument = new Configuration.Argument("Foo", typeof(string));
		//    argument.ValueProvider = new Configuration.ValueProvider(typeof(MockValueProvider));
		//    config.Arguments = new Configuration.Argument[] { argument };

		//    MockValueInfoService infoService = new MockValueInfoService();
		//    infoService.Infos.Add("Foo", new ValueInfo("Foo", true, typeof(string), TypeDescriptor.GetConverter(typeof(string))));

		//    Recipe recipe = new Recipe(config);
		//    recipe.AddService(typeof(ITypeResolutionService), new MockTypeResolutionService());
		//    recipe.AddService(typeof(IDictionaryService), new MockDictionaryService());
		//    recipe.AddService(typeof(IConfigurationService), new MockConfigurationService(package));
		//    recipe.AddService(typeof(IValueGatheringService), new MockArgumentGatheringService());
		//    recipe.AddService(typeof(IValueInfoService), infoService);

		//    recipe.Execute(false);

		//    Assert.IsTrue(MockValueProvider.BeginRecipeCalled);

		//}

		public class MockValueProvider : ValueProvider
		{
			public static bool BeginRecipeCalled;
			public static bool BeforeActionsCalled;

			public override bool OnBeginRecipe(object currentValue, out object newValue)
			{
				BeginRecipeCalled = true;
				return base.OnBeginRecipe(currentValue, out newValue);
			}

			public override bool OnBeforeActions(object currentValue, out object newValue)
			{
				BeforeActionsCalled = true;
				return base.OnBeforeActions(currentValue, out newValue);
			}
		}

        [TestMethod]
        public void ExecuteRecipeTypeAlias()
        {
            // Load the package from the config.
            GuidancePackage package = new GuidancePackage(Utils.MakeTestRelativePath("RecipeTest.xml"));
            EnablePackage(package);
            package.Execute(new MockObjects.MockReference("TypeAliasArgument", ""));
        }

        #region ExecuteRecipeTypeAlias helper class

        [System.ComponentModel.DesignerCategory("Code")]
        internal class ExecuteRecipeTypeAliasStep : Services.MoveToNextPage
        {
            public ExecuteRecipeTypeAliasStep(WizardFramework.WizardForm parent)
                : base(parent)
            {
            }

            protected override void Execute()
            {
                base.OnActivated();
                IDictionaryService dictionary = (IDictionaryService)
                    GetService(typeof(IDictionaryService));
                dictionary.SetValue("AnInt", 64);
                dictionary.SetValue("ABigInt", (long)64);
            }
        }

        #endregion RecipeTestStep class

		private void EnablePackage(GuidancePackage package)
		{
			Manager.EnablePackage(package);
            //package.AddService(typeof(IValueGatheringService), new MockServices.MockArgumentGatheringService());
            package.AddService(typeof(IValueGatheringService), new WizardFramework.WizardGatheringService());
		}

		[TestMethod]
		[ExpectedException(typeof(RecipeExecutionException))]
		public void ExecuteNullNoWizard()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(Utils.MakeTestRelativePath("RecipeTest.xml"));

			EnablePackage(package);

			package.Execute(new MockObjects.MockReference("NullNoWizard", ""));
		}

		[TestMethod]
        [ExpectedException(typeof(ServiceMissingException))]
		public void ExecuteNullWizardNoGathering()
        {
            // Load the package from the config.
            GuidancePackage package = new GuidancePackage(Utils.MakeTestRelativePath("RecipeTest.xml"));

            // Don't add the gathering service to the package.
            Manager.EnablePackage(package);
            Manager.AddService(typeof(IUIService), new MockServices.MockUIService());
            try
            {
                ExecutionResult result = package.Execute(new MockObjects.MockReference("TestRecipe", ""));
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    throw e.InnerException;
                else
                    throw e;
            }
        }

		[TestMethod]
		public void ExecuteBindingRecipe()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(Utils.MakeTestRelativePath("RecipeTest-Binding.xml"));

			EnablePackage(package);

			Assert.IsTrue(Flags[0]);
		}
		
		[TestMethod]
		[ExpectedException(typeof(System.Configuration.ConfigurationException))]
		public void ExecuteInvalidBindingRecipe()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(Utils.MakeTestRelativePath("RecipeTest-BindingInvalid.xml"));

			EnablePackage(package);
		}

		[TestMethod]
		public void CanReplaceExecutionService()
		{
			MockActionExecutionService exec = new MockActionExecutionService();
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(Utils.MakeTestRelativePath("RecipeTest.xml"));
			EnablePackage(package);
			package.AddService(typeof(IActionExecutionService), exec);

			package.Execute(new MockObjects.MockReference("TestRecipe", ""));

			Assert.IsTrue(exec.ExecuteCalled, "Custom execution service was not called");
		}

		class MockActionExecutionService : IActionExecutionService
		{
			public bool ExecuteCalled = false;

			#region IActionExecutionService Members

			public void Execute(string actionName)
			{
				Execute(actionName, new Dictionary<string, object>());
			}

			public void Execute(string actionName, Dictionary<string, object> inputValues)
			{
				ExecuteCalled = true;
			}

			#endregion
		}

		#region ExecuteBindingAction

		public class ExecuteBindingAction : IAction
		{
			#region IAction Members

			public void Execute()
			{
				RecipeTests.Flags[0] = true;
			}

			public void Undo()
			{
				throw new NotImplementedException();
			}

			#endregion
		}

		#endregion ExecuteBindingAction

		#region RecipeTestStep class

		[System.ComponentModel.DesignerCategory("Code")]
		internal class RecipeTestStep : Services.MoveToNextPage
		{
			public RecipeTestStep(WizardFramework.WizardForm parent)
			: base(parent)
			{
			}

			protected override void Execute()
			{
				base.OnActivated();
				IDictionaryService dictionary = (IDictionaryService)
					GetService(typeof(IDictionaryService));
				dictionary.SetValue("AValue", "IPG");
			}
		}

		#endregion RecipeTestStep class
	}
}
