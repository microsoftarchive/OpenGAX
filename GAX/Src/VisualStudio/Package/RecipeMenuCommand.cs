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
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.Practices.RecipeFramework.Services;
using System.Diagnostics;
using Microsoft.Practices.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
	internal sealed class RecipeMenuCommand : AssetMenuCommand
	{
		Configuration.Recipe recipe;

		public RecipeMenuCommand(Configuration.Recipe recipe, GuidancePackage guidancePackage, 
			Microsoft.Practices.ComponentModel.ServiceContainer serviceProvider, CommandID commandId)
			: base(guidancePackage, serviceProvider, commandId)
		{
			if (recipe == null)
			{
				throw new ArgumentNullException("recipe");
			}
			this.recipe = recipe;
            this.Text = recipe.Caption;
		}

		protected override void OnQueryStatus()
		{
                IAssetReferenceService referenceservice = ServiceProvider.GetService<IAssetReferenceService>(true);
                Visible = referenceservice.IsAssetEnabledFor(recipe.Name, GetTarget());
		}

		protected override void OnExec()
		{
            IAssetReferenceService referenceservice = ServiceProvider.GetService<IAssetReferenceService>(true);
            IAssetReference reference = referenceservice.GetReferenceFor(recipe.Name, GetTarget());
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }
            reference.Execute();
        }

	}
}
