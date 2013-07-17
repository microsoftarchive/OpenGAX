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
	public class ContainerComponentTests
	{
		#region Simple containment

		[TestMethod]
		public void ProviderContainment()
		{
			ServiceContainer sc = new ServiceContainer();
			sc.AddService(typeof(ContainerComponentTests), this);

			Assert.AreSame(this,
				((IServiceProvider)sc).GetService(typeof(ContainerComponentTests)));

			ContainerComponent cc = (ContainerComponent)sc;

			Assert.AreSame(this,
				((IServiceProvider)cc).GetService(typeof(ContainerComponentTests)));
		}


		[TestMethod]
		public void SimpleContainment()
		{
			ContainerComponent cc = new ContainerComponent();
			cc.Add(new Component());
			SimpleComponent sc = new SimpleComponent();
			cc.Add(sc);

			Assert.IsTrue(sc.sited);
		}

		private class SimpleComponent : SitedComponent
		{
			internal bool sited = false;

			protected override void OnSited()
			{
				base.OnSited();
				sited = true;
			}
		}

		#endregion Simple containment

		#region Service containment

		[TestMethod]
		public void ServiceContainment()
		{
			ServiceContainer parent = new ServiceContainer();
			parent.AddService(typeof(ContainerComponentTests), this);

			ContainerComponent child = new ContainerComponent();
			SimpleComponent sc = new SimpleComponent();
			child.Add(sc);

			Assert.IsNull(((IServiceProvider)child).GetService(
				typeof(ContainerComponentTests)));

			parent.Add(child);

			Assert.AreSame(this, ((IServiceProvider)parent).GetService(
				typeof(ContainerComponentTests)));
		}

		#endregion Service containment
	}
}