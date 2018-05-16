#region Using directives

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Xml;

using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.MockServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.ExtensionManager;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
    /// <summary>
    /// By convention, the actions add themselves to the tracker with their type as the key.
    /// </summary>
    [TestClass]
	[DeploymentItem("Action-DuplicateParam.xml")]
	[DeploymentItem("ActionExecutionTests.xml")]
	public class ActionExecutionTests
	{
		#region SetUp

		RecipeManager Manager;
		ActionTracker Tracker;

		[TestInitialize]
		public void SetUp()
		{
			var extensionManager = new MockExtensionManager();
			extensionManager.AddGuidancePackage("ActionExecutionTests.xml");
			extensionManager.AddGuidancePackage("Action-DuplicateParam.xml");

			Manager = new RecipeManager();
			Tracker = new ActionTracker();
			Manager.AddService(Tracker.GetType(), Tracker);
			Manager.AddService(typeof(IPersistenceService), new MockPersistenceService());
			Manager.AddService(typeof(EnvDTE._DTE), new MockServices.MockDte());
			Manager.AddService(typeof(SVsExtensionManager), extensionManager);
		}

		#endregion SetUp

        [TestMethod]
        [ExpectedException(typeof(System.Configuration.ConfigurationException))]
        public void DuplicateActionParameter()
        {
            // Load the package from the config.
            GuidancePackage package = new GuidancePackage(new XmlTextReader(
                Utils.MakeTestRelativePath("Action-DuplicateParam.xml")));
        }

		[TestMethod]
		public void SuccessfullAction()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(new XmlTextReader(Utils.MakeTestRelativePath("ActionExecutionTests.xml")));

			Manager.EnablePackage(package);

			package.Execute(new MockObjects.MockReference("SuccessfullAction", ""));

			// Check the "SuccessAction" action was executed.
			Assert.IsNotNull(Tracker.Executed[typeof(SuccessAction)]);
		}

		[TestMethod]
		public void FailAction()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(new XmlTextReader(Utils.MakeTestRelativePath("ActionExecutionTests.xml")));

			Manager.EnablePackage(package);

			try
			{
				// Must fail.
				package.Execute(new MockObjects.MockReference("FailAction", ""));
			}
			catch
			{
				// We passed.
				return;
			}

			Assert.Fail("Execution should have failed.");
		}

		[TestMethod]
		public void FailedActionIsNotUndone()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(new XmlTextReader(Utils.MakeTestRelativePath("ActionExecutionTests.xml")));

			Manager.EnablePackage(package);

			try
			{
				// Must fail.
				package.Execute("FailAction");
			}
			catch (ActionExecutionException)
			{
				Assert.IsNull(Tracker.Undone[typeof(FailAction)]);

				// We passed.
				return;
			}

			Assert.Fail("Execution should have failed.");
		}

		[TestMethod]
		public void SuccessAndFailActions()
		{
			// Load the package from the config.
			GuidancePackage package = new GuidancePackage(new XmlTextReader(Utils.MakeTestRelativePath("ActionExecutionTests.xml")));

			Manager.EnablePackage(package);

			try
			{
				// Must fail.
				package.Execute(new MockObjects.MockReference("SuccessAndFailActions", ""));
			}
			catch (ActionExecutionException)
			{
				Assert.IsNotNull(Tracker.Executed[typeof(SuccessAction)]);
				Assert.IsNotNull(Tracker.Executed[typeof(FailAction)]);
				Assert.IsNull(Tracker.Undone[typeof(FailAction)]);
				Assert.IsNotNull(Tracker.Undone[typeof(SuccessAction)]);

				// We passed.
				return;
			}

			Assert.Fail("Execution should have failed.");
		}
	}

	public class ActionTracker
	{
		public Hashtable Executed = new Hashtable();
		public Hashtable Undone = new Hashtable();
	}

	#region Action classes

	[System.ComponentModel.DesignerCategory("Code")]
	public abstract class TestAction : Component, IAction
	{
		protected ActionTracker Tracker
		{
			get { return (ActionTracker)GetService(typeof(ActionTracker)); }
		}

		#region IAction Members

		public virtual void Execute()
		{
			Tracker.Executed[this.GetType()] = true;
		}

		public virtual void Undo()
		{
			Tracker.Undone[this.GetType()] = true;
		}

		#endregion
	}

	[System.ComponentModel.DesignerCategory("Code")]
	public class SuccessAction : TestAction
	{
	}

	[System.ComponentModel.DesignerCategory("Code")]
	public class FailAction : TestAction
	{
		public override void Execute()
		{
			base.Execute();
			throw new ArgumentException("Fail");
		}
	}

	[System.ComponentModel.DesignerCategory("Code")]
	public class FailUndoAction : TestAction
	{
		public override void Undo()
		{
			base.Undo();
			throw new ArgumentException("Fail");
		}
	}

	[System.ComponentModel.DesignerCategory("Code")]
	public class FailExecuteAndUndoAction : TestAction
	{
		public override void Execute()
		{
			base.Execute();
			throw new ArgumentException("Fail");
		}

		public override void Undo()
		{
			base.Undo();
			throw new ArgumentException("Fail");
		}
	}

	#endregion Action classes

	#region ActionStep class

	[System.ComponentModel.DesignerCategory("Code")]
	internal class ActionStep :  MoveToNextPage
	{
		public ActionStep(WizardFramework.WizardForm parent)
			: base(parent)
		{
		}

		protected override void Execute()
		{
			IDictionaryService dictionary = (IDictionaryService)
				GetService(typeof(IDictionaryService));
			dictionary.SetValue("AValue", "IPG");
		}
	}

	#endregion ActionStep class
}
