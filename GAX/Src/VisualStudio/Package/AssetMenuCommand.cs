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

#region Using Directives

using System;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
	internal abstract class AssetMenuCommand : OleMenuCommand
	{
		protected GuidancePackage GuidancePackage
		{
			get { return guidancePackage; }
		} GuidancePackage guidancePackage;

		protected Microsoft.Practices.ComponentModel.ServiceContainer ServiceProvider
		{
			get { return serviceProvider; }
		} Microsoft.Practices.ComponentModel.ServiceContainer serviceProvider;

		public AssetMenuCommand(GuidancePackage guidancePackage,
			Microsoft.Practices.ComponentModel.ServiceContainer serviceProvider, CommandID commandId)
			: base(null, commandId)
		{
			if (serviceProvider == null || guidancePackage == null || commandId == null)
			{
				throw new ArgumentNullException("RecipeMenuCommand");
			}
			this.serviceProvider = serviceProvider;
			this.guidancePackage = guidancePackage;
			this.Supported = true;
			this.Visible = true;
			this.Enabled = true;
			this.BeforeQueryStatus += new EventHandler(OnBeforeQueryStatus);
		}

		protected object GetTarget()
		{
            return DteHelper.GetTarget(ServiceProvider.GetService<DTE>(true));
		}

		protected abstract void OnQueryStatus();

		protected abstract void OnExec();

		private void OnBeforeQueryStatus(object sender, EventArgs eventArgs)
		{
			try
			{
				this.GuidancePackage.TurnOnOutput();
				Visible = false;
				OnQueryStatus();
			}
			catch (Exception e)
			{
				ErrorHelper.Show(ServiceProvider, e);
			}
			finally
			{
				this.GuidancePackage.TurnOffOutput();
			}
		}

		public override void Invoke()
		{
			try
			{
				this.GuidancePackage.TurnOnOutput();
				OnExec();
			}
			catch (Exception e)
			{
				ErrorHelper.Show(ServiceProvider, e);
			}
			finally
			{
				this.GuidancePackage.TurnOffOutput();
			}
		}

		public override void Invoke(object inArg)
		{
			this.Invoke();
		}

		public override void Invoke(object inArg, IntPtr outArg)
		{
			this.Invoke();
		}

		public override void Invoke(object inArg, System.IntPtr outArg, Microsoft.VisualStudio.OLE.Interop.OLECMDEXECOPT options)
		{
			this.Invoke();
		}
	}
}