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
using System.Windows.Forms;

using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.Common;

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework.PackageManagement
{
	/// <summary>
	/// Base class for recipe management forms.
	/// </summary>
	/// <remarks>
	/// Descendant forms must override <see cref="Site"/> property 
	/// and call <see cref="InitServices"/> to initialize the required service 
	/// variables.
	/// </remarks>
	[System.ComponentModel.DesignerCategory("Code")]
	public class ManagerForm : Form
	{
		/// <summary>
		/// Initializes the form, which must be sited at a later time.
		/// </summary>
		public ManagerForm()
		{
		}

		/// <summary>
		/// Initializes the form using a provider, which will be used to site the form. 
		/// </summary>
		public ManagerForm(IServiceProvider provider)
		{
			// Site ourselves with the provider.
			this.Site = new ComponentModel.Site(provider, this, null);
            ServiceHelper.CheckDependencies(this);
            //container.Add(this);
		}

		/// <summary>
		/// It is called when the form is closed.
		/// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!e.Cancel)
            {
                this.Container.Remove(this);
            }
        }

		/// <summary>
		/// Implements service retrieval from the provider.
		/// </summary>
		protected void InitServices(IServiceProvider provider)
		{
			if (provider == null)
			{
				// It's being unsited actually.
				return;
			}
			manager = (IRecipeManagerService)provider.GetService(typeof(IRecipeManagerService));
			host = (IHostService)provider.GetService(typeof(IHostService));
			if (manager == null)
			{
				throw new InvalidOperationException(
					Configuration.Resources.PackageManager_NoRecipeManagerService);
			}
		}

		private IRecipeManagerService manager;

		/// <summary>
		/// Gets the service that allows administrative operations on recipes and Packages.
		/// </summary>
		public IRecipeManagerService RecipeManagerService
		{
			get { return manager; }
		}

		private IHostService host;

		/// <summary>
		/// Gets the host where the framework is running.
		/// </summary>
		public IHostService HostService
		{
			get { return host; }
		}
	}
}
