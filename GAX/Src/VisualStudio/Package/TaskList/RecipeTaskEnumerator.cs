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
using System.Collections;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.ComponentModel;
using Microsoft.VisualStudio.Shell.Interop;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.TaskList
{
	[ServiceDependency(typeof(IAssetReferenceService))]
	[ServiceDependency(typeof(IConfigurationService))]
	[ServiceDependency(typeof(IVsTaskProvider3))]
	internal sealed class RecipeTaskEnumerator : ContainerComponent,IVsEnumTaskItems
	{
		#region Fields

		IEnumerator recipeReferencesEnumerator;

		#endregion

		#region Constructor

		internal RecipeTaskEnumerator()
		{
		}

		#endregion

		#region Overrides

		protected override void OnSited()
		{
			base.OnSited();
			IAssetReferenceService referenceService = (IAssetReferenceService)
				this.GetService(typeof(IAssetReferenceService),true);
			foreach (IAssetReference reference in referenceService.GetAll())
			{
				if (reference is RecipeReference && reference is IBoundAssetReference)
				{
					RecipeTaskItem recipeTaskItem = new RecipeTaskItem((RecipeReference)reference);
					this.Add(recipeTaskItem, reference.Key);
				}
			}
			((IVsEnumTaskItems)this).Reset();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		#endregion

		#region IVsEnumTaskItems members

		int IVsEnumTaskItems.Clone(out Microsoft.VisualStudio.Shell.Interop.IVsEnumTaskItems ppenum)
		{
			ppenum = null;
			return VSConstants.E_NOTIMPL;
		}

		int IVsEnumTaskItems.Next(uint celt, Microsoft.VisualStudio.Shell.Interop.IVsTaskItem[] rgelt, uint[] pceltFetched)
		{
			if (pceltFetched == null)
			{
				if (celt != 1)
				{
					return VSConstants.E_POINTER;
				}
			}
			if (null == rgelt)
			{
				return VSConstants.S_FALSE;
			}
			uint cReturn = 0;
			int iRgelt = 0;
			while (celt > 0 && recipeReferencesEnumerator.MoveNext())
			{
				RecipeTaskItem recipeTaskItem = recipeReferencesEnumerator.Current as RecipeTaskItem;
				rgelt[iRgelt] = recipeTaskItem;
				iRgelt++;
				cReturn++;
				--celt;
			}
			if (null != pceltFetched)
				pceltFetched[0] = cReturn;
			return VSConstants.S_OK;
		}

		int IVsEnumTaskItems.Reset()
		{
			recipeReferencesEnumerator = this.Components.GetEnumerator();
			recipeReferencesEnumerator.Reset();
			return VSConstants.S_OK;
		}

		int IVsEnumTaskItems.Skip(uint celt)
		{
			while (celt > 0 && recipeReferencesEnumerator.MoveNext())
			{
				celt--;
			}
			return VSConstants.S_OK;
		}

		#endregion
	}
}
