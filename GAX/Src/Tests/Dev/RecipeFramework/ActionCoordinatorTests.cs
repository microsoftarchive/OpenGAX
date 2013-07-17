using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using System.ComponentModel.Design;
using Microsoft.Practices.Common.Services;
using System.Xml;
using System.Collections.Generic;

namespace Microsoft.Practices.RecipeFramework
{
	[TestClass]
	public class ActionCoordinatorTests
	{
		private static Microsoft.Practices.ComponentModel.ServiceContainer container;
		private static Configuration.Recipe recipeConfiguration;

		[TestInitialize]
		public void SetUp()
		{
			recipeConfiguration = new Microsoft.Practices.RecipeFramework.Configuration.Recipe();
			recipeConfiguration.Name = "Test";

			Configuration.Action first = new Configuration.Action();
			first.Name = "First";
			first.Type = typeof(FirstAction).FullName;

			Configuration.Action second = new Configuration.Action();
			second.Name = "Second";
			second.Type = typeof(SecondAction).FullName;

			recipeConfiguration.Actions = new Configuration.RecipeActions();
			recipeConfiguration.Actions.Action = new Configuration.Action[] { first, second };

			container = new Microsoft.Practices.ComponentModel.ServiceContainer();
			container.AddService(typeof(System.ComponentModel.Design.ITypeResolutionService), new MockResolutionService());
			container.AddService(typeof(System.ComponentModel.Design.IDictionaryService), new MockDictionary());
			container.AddService(typeof(Microsoft.Practices.RecipeFramework.Services.IConfigurationService), 
				new MockConfigrationService());
			container.AddService(typeof(IComponentChangeService), new MockChangeService());
			container.AddService(typeof(IValueInfoService), new MockValueInfoService());
		}

		[TestMethod]
		public void DefaultCoordinatorExecutesSequentially()
		{
			// Setup mock actions.
			Recipe recipe = new Recipe(recipeConfiguration);
			container.Add(recipe);

			recipe.Execute(false);

			Assert.IsTrue(FirstAction.Executed);
			Assert.IsTrue(SecondAction.Executed);
		}

		[TestMethod]
		public void CustomCoordinatorDoesNotExecuteByDefault()
		{
			recipeConfiguration.Actions.CoordinatorServiceType = typeof(MockCoordinator).FullName;
			recipeConfiguration.Actions.Any = new XmlDocument().CreateElement("Data");

			Recipe recipe = new Recipe(recipeConfiguration);
			container.Add(recipe);

			recipe.Execute(false);

			Assert.IsTrue(MockCoordinator.WasRun, "Coordinator was not run");
			Assert.IsNotNull(MockCoordinator.Configuration, "Configuration element was not passed to the coordinator");
			Assert.AreEqual(2, MockCoordinator.DeclaredActions.Count, "Didn't receive the two configured actions");
			Assert.AreEqual("Data", MockCoordinator.Configuration.LocalName, "Received configuration element is not the one specified in xml");
		}

		[TestMethod]
		public void DefaultCoordinatorExecutesAllActions()
		{
			Recipe recipe = new Recipe(recipeConfiguration);
			container.Add(recipe);

			recipe.Execute(false);

			Assert.IsTrue(FirstAction.Executed, "FirstAction not executed");
			Assert.IsTrue(SecondAction.Executed, "FirstAction not executed");
		}

		#region Mock classes

		public class MockCoordinator : IActionCoordinationService
		{
			public static bool WasRun = false;
			public static System.Xml.XmlElement Configuration = null;
			public static Dictionary<string, Configuration.Action> DeclaredActions;

			#region IActionCoordinator Members

			public void Run(System.Xml.XmlElement coordinationData)
			{
				WasRun = true;
				Configuration = coordinationData;
			}

			#endregion

			#region IActionCoordinationService Members

			public void Run(Dictionary<string, Configuration.Action> declaredActions, XmlElement coordinationData)
			{
				WasRun = true;
				Configuration = coordinationData;
				DeclaredActions = declaredActions;
			}

			#endregion
		}

		public class FirstAction : Action
		{
			public static bool Executed = false;
			public static bool Undone = false;

			public FirstAction()
			{
				Executed = false;
				Undone = false;
			}

			public override void Execute()
			{
				Executed = true;
			}

			public override void Undo()
			{
				Undone = true;
			}
		}

		public class SecondAction : FirstAction
		{
			public SecondAction()
			{
				Executed = false;
				Undone = false;
			}
		}

		public class MockValueInfoService : IValueInfoService
		{
			#region IValueInfoService Members

			public ValueInfo GetInfo(string valueName)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public string ComponentName
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			#endregion
		}

		public class MockChangeService : IComponentChangeService
		{
			#region IComponentChangeService Members

			public void OnComponentChanged(object component, System.ComponentModel.MemberDescriptor member, object oldValue, object newValue)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void OnComponentChanging(object component, System.ComponentModel.MemberDescriptor member)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion

			#region IComponentChangeService Members

			public event ComponentEventHandler ComponentAdded;

			public void FireComponentAdded()
			{
				ComponentAdded(this, new ComponentEventArgs(null));
			}

			public event ComponentEventHandler ComponentAdding;

			public void FireComponentAdding()
			{
				ComponentAdding(this, new ComponentEventArgs(null));
			}

			public event ComponentChangedEventHandler ComponentChanged;

			public void FireComponentChanged()
			{
				ComponentChanged(this, new ComponentChangedEventArgs(null,null,null,null));
			}

			public event ComponentChangingEventHandler ComponentChanging;

			public void FireComponentChanging()
			{
				ComponentChanging(this, new ComponentChangingEventArgs(null,null));
			}

			public event ComponentEventHandler ComponentRemoved;

			public void FireComponentRemoved()
			{
				ComponentRemoved(this, new ComponentEventArgs(null));
			}

			public event ComponentEventHandler ComponentRemoving;

			public void FireComponentRemoving()
			{
				ComponentRemoving(this, new ComponentEventArgs(null));
			}

			public event ComponentRenameEventHandler ComponentRename;

			public void FireComponentRename()
			{
				ComponentRename(this, new ComponentRenameEventArgs(null,null,null));
			}

			#endregion

		}

		public class MockConfigrationService : IConfigurationService
		{
			#region IConfigurationService Members

			public Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage CurrentPackage
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public Microsoft.Practices.RecipeFramework.Configuration.Recipe CurrentRecipe
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public object CurrentGatheringServiceData
			{
				get { return null; }
			}

			public string BasePath
			{
				get { return AppDomain.CurrentDomain.BaseDirectory; }
			}

			#endregion
		}

		public class MockDictionary : System.ComponentModel.Design.IDictionaryService
		{
			#region IDictionaryService Members

			public object GetKey(object value)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public object GetValue(object key)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void SetValue(object key, object value)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion
		}

		public class MockResolutionService : System.ComponentModel.Design.ITypeResolutionService
		{
			#region ITypeResolutionService Members

			public System.Reflection.Assembly GetAssembly(System.Reflection.AssemblyName name, bool throwOnError)
			{
				return Assembly.Load(name);
			}

			public System.Reflection.Assembly GetAssembly(System.Reflection.AssemblyName name)
			{
				return Assembly.Load(name);
			}

			public string GetPathOfAssembly(System.Reflection.AssemblyName name)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public Type GetType(string name, bool throwOnError, bool ignoreCase)
			{
				return Type.GetType(name, throwOnError, ignoreCase);
			}

			public Type GetType(string name, bool throwOnError)
			{
				return GetType(name, throwOnError, false);
			}

			public Type GetType(string name)
			{
				return GetType(name, false, false);
			}

			public void ReferenceAssembly(System.Reflection.AssemblyName name)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion
		}

		#endregion
	}
}
