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

using System;
using System.Collections;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;

using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio.TaskList;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
    [ServiceDependency(typeof(IMenuCommandService))]
	[ServiceDependency(typeof(IAssetReferenceService))]
	internal sealed class VsGuidancePackage : Microsoft.Practices.ComponentModel.ServiceContainer
	{
		#region Fields

		RecipeTaskProvider taskList;
		GuidancePackage guidancePackage;
		ArrayList commands;

		#endregion

		#region Properties
		internal GuidancePackage GuidancePackage
		{
			get
			{
				return guidancePackage;
			}
		}
		#endregion

		#region Constructor

		public VsGuidancePackage(GuidancePackage guidancePackage)
		{
			this.guidancePackage = guidancePackage;
		}

		#endregion

		#region Overrides

		protected override void OnSited()
		{
			base.OnSited();

			//Setup TaskList provider
			taskList = new RecipeTaskProvider(new Guid(guidancePackage.Configuration.Guid));
			Add(taskList);

            IAssetReferenceService referenceService = GetService<IAssetReferenceService>();
            referenceService.AddIndexer(typeof(IndexerBoundAssetParent), new IndexerBoundAssetParent());

			// Initialize all commands according to loaded configuration.
			InitializeCommands();

        }

		protected override object GetService(Type serviceType)
		{
			object service = base.GetService(serviceType);
			if (service == null)
			{
				service = ((IServiceProvider)guidancePackage).GetService(serviceType);
			}
			return service;
		}

		protected override void Dispose(bool disposing)
		{
			if ( taskList!=null )
			{
				Remove(taskList);
				taskList.Dispose();
				taskList = null;
			}
            DeinitializeCommands();
			base.Dispose(disposing);
		}

		#endregion

		#region Private

		private void InitializeTemplateCommands()
		{
            IMenuCommandService mcs = GetService<IMenuCommandService>(true);
            IVsTemplatesService templatesService = GetService<IVsTemplatesService>(true);
            Guid guidancePackageGuid = new Guid(guidancePackage.Configuration.Guid);
            IAssetDescription[] templates = templatesService.GetHostAssets(guidancePackage.BasePath);
            foreach (IVsTemplate template in templates)
            {
                if (template.Kind != TemplateKind.Solution)
                {
                    MenuCommand menuCmd = new TemplateMenuCommand(template, this.guidancePackage, this);
                    mcs.AddCommand(menuCmd);
                    commands.Add(menuCmd);
                }
            }
        }

		private void InitializeRecipeCommands()
		{
			IMenuCommandService mcs = GetService<IMenuCommandService>(true);
			Guid guidancePackageGuid = new Guid(guidancePackage.Configuration.Guid);
            if (guidancePackage.Configuration.Recipes == null)
            {
                return;
            }
            if (guidancePackage.Configuration.Recipes.Length >= 255)
            {
                throw new InvalidOperationException(Properties.Resources.Recipes_MaximumExceeded);
            }
			for (int iRecipe = 0; iRecipe < guidancePackage.Configuration.Recipes.Length; iRecipe++)
			{
				Configuration.Recipe recipe = guidancePackage.Configuration.Recipes[iRecipe];
				if (recipe.HostData != null && recipe.HostData.CommandBar != null)
				{
					CommandID cmd = new CommandID(guidancePackageGuid, (iRecipe + 1) * 0x100);
					MenuCommand menuCmd = new RecipeMenuCommand(recipe, this.guidancePackage, this, cmd);
					mcs.AddCommand(menuCmd);
					commands.Add(menuCmd);
				}
			}
		}

		private void InitializeCommands()
		{
			if (commands == null)
			{
				commands = new ArrayList();
			}
            InitializeRecipeCommands();
		    InitializeTemplateCommands();
		}

		private void DeinitializeCommands()
		{
			if (commands != null)
			{
                IMenuCommandService mcs = GetService<IMenuCommandService>(true);
				foreach (MenuCommand command in commands)
				{
					mcs.RemoveCommand(command);
				}
				commands = null;
			}
		}

		#endregion
    }
}
