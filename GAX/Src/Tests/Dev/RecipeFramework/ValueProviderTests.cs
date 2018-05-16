#region Using directives

using System;
using System.Collections;
using System.ComponentModel.Design;

using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.MockServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.Common;
using Microsoft.VisualStudio.ExtensionManager;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
    [TestClass]
	[DeploymentItem("ValueProviderTests.xml")]
	[DeploymentItem("ValueProviderTests-Circular.xml")]
	public class ValueProviderTests
	{
		RecipeManager Manager;
		// A set of flags to use during execution for several tests.
		internal static bool[] Flags = new bool[10];
		// Arbitrary list of values for tests
		internal static IDictionary Values;

		static ValueProviderTests()
		{
			System.Diagnostics.Trace.Listeners.Add(new
				System.Diagnostics.TextWriterTraceListener(Console.Out));
		}

		#region SetUp

		[TestInitialize]
		public void SetUp()
		{
			var extensionManager = new MockExtensionManager();
			extensionManager.AddGuidancePackage("ValueProviderTests.xml");
			extensionManager.AddGuidancePackage("ValueProviderTests-Circular.xml");

			Manager = new RecipeManager();
			Manager.AddService(typeof(IPersistenceService), new MockPersistenceService());
			Manager.AddService(typeof(EnvDTE._DTE), new MockServices.MockDte());
			Manager.AddService(typeof(SVsExtensionManager), extensionManager);
			
			Flags = new bool[10];
			Values = new System.Collections.Specialized.HybridDictionary();
		}

		#endregion SetUp

		private void EnablePackage(GuidancePackage guidancePackage)
		{
			Manager.EnablePackage(guidancePackage);
            guidancePackage.AddService(typeof(IValueGatheringService), new WizardFramework.WizardGatheringService());
		}

		[TestMethod]
		public void ValueProviderLifeCycle()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(Utils.MakeTestRelativePath("ValueProviderTests.xml"));

			EnablePackage(package);

			package.Execute(new MockObjects.MockReference("TestLifeCycle", ""));

			Assert.IsTrue(Flags[0]);
			Assert.IsTrue(Flags[1]);
		}

		[TestMethod]
		public void WizardChangeMonitoring()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(Utils.MakeTestRelativePath("ValueProviderTests.xml"));

			EnablePackage(package);

			package.Execute(new MockObjects.MockReference("TestChangeNotification", ""));

			Assert.IsTrue(Flags[0]);
			Assert.AreEqual("A new value-B", Values["B"]);
		}

		[TestMethod]
		//[Ignore("Test display a dialog and waits for input, Daniel to fix it")]
		public void ValueEditorChangeMonitoring()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(Utils.MakeTestRelativePath("ValueProviderTests.xml"));

			EnablePackage(package);

			// Set a value for the FirstValueProvider class to pick.
			Values["FirstValueProvider"] = "Hi there";
			Values["ConcatValueProvider"] = " from B";

			package.Execute(new MockObjects.MockReference("ValueEditorChangeMonitoring", ""));
			Assert.IsTrue(Flags[0]);
			Assert.AreEqual("Bye Hi there from B", Values["ConcatValueProvider"]);
		}

		[TestMethod]
		[Ignore]
		public void CircularDependenciesMessage()
		{
			bool passed = false;

			try
			{
				// Load the package from the config.
				GuidancePackage package = new GuidancePackage(Utils.MakeTestRelativePath("ValueProviderTests-Circular.xml"));
			}
			catch (System.Configuration.ConfigurationException cex)
			{
				if (cex.InnerException.Message.IndexOf(@"
A->B->C->E->C
B->C->E->C->E
B->D->A->B
C->E->C
D->A->B->C->E->C
D->B->C->E->C
E->C->E") != -1)
				{
					passed = true;
				}
			}

			if (!passed)
			{
				Assert.Fail("Should have thrown a configuration exception with a specific message containing invalid monitoring chain. Keep in sync with config.");
			}
		}

		[TestMethod]
		[ExpectedException(typeof(System.Configuration.ConfigurationException))]
		public void CircularDependencies()
		{
				// Load the package from the config.
				GuidancePackage package = new GuidancePackage(Utils.MakeTestRelativePath("ValueProviderTests-Circular.xml"));
		}

		[TestMethod]
		public void ConfigurableEditor()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(Utils.MakeTestRelativePath("ValueProviderTests.xml"));

			EnablePackage(package);

			package.Execute(new MockObjects.MockReference("ValueEditorConfig", ""));
			Assert.IsTrue(Flags[0]);
		}

		#region LifeCycleProvider class

		public class LifeCycleProvider : ValueProvider
		{
			public override bool OnBeforeActions(object currentValue, out object newValue)
			{
				ValueProviderTests.Flags[0] = true;
				// Specify a value so the recipe finishes.
				newValue = "kzu";
				return true;
			}

			public override bool OnBeginRecipe(object currentValue, out object newValue)
			{
                newValue = null;
				ValueProviderTests.Flags[1] = true;
				return true;
			}
		}

		#endregion LifeCycleProvider class

		#region ChangeMonitoringStep class

		[System.ComponentModel.DesignerCategory("Code")]
		internal class ChangeMonitoringStep : Services.MoveToNextPage
		{
			public ChangeMonitoringStep(WizardFramework.WizardForm parent)
				: base(parent)
			{
			}

			public override void EndInit()
			{
				base.EndInit();

				IComponentChangeService changes = (IComponentChangeService)
					GetService(typeof(IComponentChangeService));
				if (changes!=null)
				{
					changes.ComponentChanged += new ComponentChangedEventHandler(OnArgumentChanged);
				}
			}

			protected override void Execute()
			{
				IDictionaryService dictionary = (IDictionaryService)
					GetService(typeof(IDictionaryService));
				dictionary.SetValue("A", "A new value");
			}

			void OnArgumentChanged(object sender, ComponentChangedEventArgs e)
			{
                ValueInfo argument = (ValueInfo)e.Component;
				IDictionaryService dictionary = (IDictionaryService)
					GetService(typeof(IDictionaryService));

				if (argument.Name == "A")
				{
					ValueProviderTests.Flags[0] = true;
					dictionary.SetValue("B", e.NewValue.ToString() + "-B");

					this.Wizard.DefaultButton = Microsoft.WizardFramework.ButtonType.Finish;
					this.Wizard.AcceptButton.PerformClick();
				}

				// Keep the value to perform assertions.
				ValueProviderTests.Values[argument.Name] = e.NewValue;
			}
		}

		#endregion ChangeMonitoringStep class

		#region FirstValueProvider

		public class FirstValueProvider : ValueProvider
		{
			public override bool OnBeginRecipe(object currentValue, out object newValue)
			{
				ValueProviderTests.Flags[0] = true;
				newValue = ValueProviderTests.Values["FirstValueProvider"];
				// Returning true at this point will cause ConcatValueProvider to be called before gathering,
				// meaning the argument will have a value. Therefore I added another argument without 
				// provider, and Nullable=true, so that the wizard is called actually.
				return true;
			}
		}

		#endregion FirstValueProvider

		#region ConcatValueProvider
		public class ConcatValueProvider : ValueProvider
		{
			public override bool OnArgumentChanged(string changedArgumentName, object changedArgumentValue,
				object currentValue, out object newValue)
			{
				newValue = changedArgumentValue.ToString() + ValueProviderTests.Values["ConcatValueProvider"].ToString();
				ValueProviderTests.Values["ConcatValueProvider"] = newValue;
				return true;
			}
		}
		#endregion ConcatValueProvider

		#region ValueEditorChangeMonitoringWizard class

		[System.ComponentModel.DesignerCategory("Code")]
		internal class ValueEditorChangeMonitoringWizard : Services.MoveToNextPage
		{
			public ValueEditorChangeMonitoringWizard(WizardFramework.WizardForm parent)
				: base(parent)
			{
			}

			protected override void Execute()
			{
				IDictionaryService dictionary = (IDictionaryService)
					GetService(typeof(IDictionaryService));
				// Test that the two values are set to the expected values.
				Assert.AreEqual("Hi there", dictionary.GetValue("A"));
				Assert.AreEqual("Hi there from B", dictionary.GetValue("B"));

				// Cause a change in the second value.
				dictionary.SetValue("A", "Bye ");
				Assert.AreEqual("Bye Hi there from B", dictionary.GetValue("B"));
			}
		}

		#endregion ValueEditorChangeMonitoringWizard class

		#region ValueEditorConfigurable class

		public class ValueEditorConfigurable : ValueProvider, IAttributesConfigurable
		{
			public override bool OnBeginRecipe(object currentValue, out object newValue)
			{
				newValue = "Hi There";
				return true;
			}

			#region IAttributesConfigurable Members

			public void Configure(System.Collections.Specialized.StringDictionary attributes)
			{
				Flags[0] = true;
				Assert.AreEqual(attributes["Value1"], "TheValue");
			}

			#endregion
		}

		#endregion

		#region ValueEditorDefault class

		public class ValueEditorDefault : ValueProvider, IAttributesConfigurable
		{
			string value;

			public override bool OnBeginRecipe(object currentValue, out object newValue)
			{
				newValue = value;
				return true;
			}

			#region IAttributesConfigurable Members

			public void Configure(System.Collections.Specialized.StringDictionary attributes)
			{
				value = attributes["Value"];
			}

			#endregion
		}

		#endregion
	}
}
