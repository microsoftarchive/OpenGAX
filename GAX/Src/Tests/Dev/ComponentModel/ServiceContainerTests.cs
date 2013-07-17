//===================================================================================
// Microsoft patterns & practices
// Guidance Automation Extensions
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// License: MS-LPL
//===================================================================================

#region Using directives

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Microsoft.Practices.ComponentModel
{
	[TestClass]
	public class ServiceContainerTests
	{
		#region Simple service registration and retrieval

		[TestMethod]
		public void TestGetServiceGeneric()
		{
			IServiceContainer c = new ServiceContainer();
			c.AddService(this.GetType(), this);
			
			ServiceContainerTests st = (ServiceContainerTests) 
				c.GetService(typeof(ServiceContainerTests));
			Assert.AreEqual("Hello", st.SayHello());
		}

		private string SayHello()
		{
			return "Hello";
		}

		#endregion Simple service registration and retrieval

		#region Siting components

		[TestMethod]
		public void TestSitedComponent()
		{
			ServiceContainer c = new ServiceContainer();
			c.AddService(typeof(HelloWorldService), new HelloWorldService());

			c.Add(new MyComponent(), "MyTest");
			MyComponent mc = c.Components["MyTest"] as MyComponent;
			Assert.IsTrue(mc.Run());
		}

		private class MyComponent : SitedComponent
		{
			public bool Run()
			{
				return ((HelloWorldService)GetService(typeof(HelloWorldService))).SayHello() == "Hello";
			}
		}

		private class HelloWorldService
		{
			public string SayHello()
			{
				return "Hello";
			}
		}

		#endregion Siting components

		#region Nested containers and services

		[TestMethod]
		public void DetectContainerOnSited()
		{
			ServiceContainer parent = new ServiceContainer();
			DetectOnSitedChildContainer child = new DetectOnSitedChildContainer();
			parent.Add(child);

			Assert.IsTrue(child.HasDetected, "Child OnSited method not called or Site is null.");
		}

		[TestMethod]
		public void NestedContainers()
		{
			IServiceContainer parent = new ServiceContainer();
			IServiceContainer child = new ServiceContainer();
			// Site into parent container.
			((IContainer)parent).Add((IComponent)child);
			IServiceContainer grandchild = new ServiceContainer();
			// Site into parent container.
			((IContainer)child).Add((IComponent)grandchild);

			parent.AddService(typeof(TopMostService), new TopMostService());
			child.AddService(typeof(FirstChildService), new FirstChildService());
			grandchild.AddService(typeof(SecondChildService), new SecondChildService());

			Assert.IsNotNull(grandchild.GetService(typeof(SecondChildService)));
			Assert.IsNotNull(grandchild.GetService(typeof(FirstChildService)));
			Assert.IsNotNull(grandchild.GetService(typeof(TopMostService)));

			Assert.IsNull(child.GetService(typeof(SecondChildService)));
			Assert.IsNull(parent.GetService(typeof(FirstChildService)));
			Assert.IsNull(parent.GetService(typeof(SecondChildService)));

			Assert.IsNotNull(child.GetService(typeof(FirstChildService)));
			Assert.IsNotNull(child.GetService(typeof(TopMostService)));
		}

		private class TopMostService
		{
		}

		private class FirstChildService
		{
		}

		private class SecondChildService
		{
		}

		private class DetectOnSitedChildContainer : ServiceContainer
		{
			public bool HasDetected = false;

			protected override void OnSited()
			{
				base.OnSited();
				HasDetected = this.Site != null;
			}
		}

		#endregion Nested containers and services

		#region Component publishes service

		[TestMethod]
		public void ComponentPublishesService()
		{
			ServiceContainer c = new ServiceContainer(true);
			c.Add(new PublishingComponent());
			c.Add(new ConsumingComponent());
		}

		private class PublishingComponent : SitedComponent
		{
			protected override void OnSited()
			{
				base.OnSited();
				IServiceContainer c = (IServiceContainer)GetService(typeof(IServiceContainer));
				Assert.IsNotNull(c, "IServiceContainer service not found!");
				c.AddService(typeof(TopMostService), new TopMostService());
			}
		}

		private class ConsumingComponent : SitedComponent
		{
			protected override void OnSited()
			{
				base.OnSited();
				Assert.IsNotNull(GetService(typeof(TopMostService)), "Couldn't find published service!");
			}
		}

		#endregion Component publishes service

		#region Component publishes promoted service

		[TestMethod]
		public void ComponentPublishesPromotedService()
		{
			ServiceContainer parent = new ServiceContainer(true);
			ServiceContainer child = new ServiceContainer(true);
			parent.Add(child);
			// Publishing component added to the child container.
			child.Add(new PromotedPublishingComponent());
			// Consuming component on the parent container should see the promoted service.
			parent.Add(new ConsumingComponent());
		}

		private class PromotedPublishingComponent : SitedComponent
		{
			protected override void OnSited()
			{
				base.OnSited();
				IServiceContainer c = (IServiceContainer)GetService(typeof(IServiceContainer));
				Assert.IsNotNull(c, "IServiceContainer service not found!");
				c.AddService(typeof(TopMostService), new TopMostService(), true);
			}
		}

		#endregion Component publishes service

		#region Service dependency checks

		[TestMethod]
		[ExpectedException(typeof(ServiceMissingException))]
		public void CheckedDependenciesGenericException()
		{
			ServiceContainer c = new ServiceContainer();
			c.Add(new GenericDependencyExceptionComponent());
		}

		[TestMethod]
		[ExpectedException(typeof(CantWorkWithoutItException))]
		public void CheckedDependenciesSpecificException()
		{
			ServiceContainer c = new ServiceContainer();
			c.Add(new SpecificDependencyExceptionComponent());
		}

		[ServiceDependency(typeof(TopMostService))]
		private class SpecificDependencyExceptionComponent : SitedComponent
		{
			protected override void OnMissingServiceDependency(Type missingService)
			{
				throw new CantWorkWithoutItException();
			}
		}

		private class CantWorkWithoutItException : ApplicationException
		{
		}

		[ServiceDependency(typeof(TopMostService))]
		private class GenericDependencyExceptionComponent : IComponent
		{
			#region IComponent Members
			event EventHandler IComponent.Disposed
			{
				add { throw new global::System.NotImplementedException(); }
				remove { throw new global::System.NotImplementedException(); }
			}

			ISite IComponent.Site
			{
				get { return _site; }
				set { _site = value; }
			} ISite _site;

			#endregion

			#region IDisposable Members

			void IDisposable.Dispose()
			{
				throw new NotImplementedException();
			}

			#endregion
		}

		#endregion Service dependency checks
	}
}