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
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.ComponentModel;
using System.Windows.Forms.Design;
using Microsoft.Practices.Common;
using System.Diagnostics;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.TaskList
{
	[ServiceDependency(typeof(IConfigurationService))]
	[ServiceDependency(typeof(IVsTaskProvider3))]
	internal sealed class RecipeTaskItem : SitedComponent, IVsTaskItem, IVsTaskItem3
	{
		#region Fields

		private RecipeReference recipeReference;
		private Configuration.Recipe recipeConfig;
		private IRecipeManagerService managerService;

		#endregion

		#region Constructor

		internal RecipeTaskItem(RecipeReference recipeReference)
		{
			if (recipeReference == null)
			{
				throw new ArgumentNullException("recipeReference");
			}
			this.recipeReference = recipeReference;
		}

		#endregion

		#region Overrides

		protected override void OnSited()
		{
			base.OnSited();
			IConfigurationService configService = GetService<IConfigurationService>(true);
			this.recipeConfig =
				(Configuration.Recipe)configService.CurrentPackage[recipeReference.AssetName];
			managerService = GetService<IRecipeManagerService>(true);
		}

		#endregion

		#region IVsTaskItem members
		  
		int IVsTaskItem.CanDelete(out int pfCanDelete)
		{
			pfCanDelete = VSConstantsEx.FALSE;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.Category(Microsoft.VisualStudio.Shell.Interop.VSTASKCATEGORY[] pCat)
		{
			pCat[0] = Microsoft.VisualStudio.Shell.Interop.VSTASKCATEGORY.CAT_ALL;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.Column(out int piCol)
		{
			piCol = -1;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.Document(out string pbstrMkDocument)
		{
			pbstrMkDocument = "";
			// WORKAROUND: v-oscca: We must return E_NOTIMPL, otherwise the TaskItem will not appear in our task list but in the "Error" TaskList
			// I don't know why this behavior? 
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.HasHelp(out int pfHasHelp)
		{
			pfHasHelp = VSConstantsEx.FALSE;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.ImageListIndex(out int pIndex)
		{
			pIndex = -1;
			//v-oscca: Important, we must return E_NOTIMPL, otherwise the TaskItem will not appear in our task list but in the "Error" TaskList
			// I don't know why this behavior? 
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.IsReadOnly(Microsoft.VisualStudio.Shell.Interop.VSTASKFIELD field, out int pfReadOnly)
		{
			pfReadOnly = 1;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.Line(out int piLine)
		{
			piLine = -1;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.NavigateTo()
		{
			GuidancePackage package = null;
			try
			{
				package = (GuidancePackage)ServiceHelper.GetService(this.recipeReference, typeof(IExecutionService));
				package.TurnOnOutput();
				// Execute the recipe through the reference.
				this.recipeReference.Execute();
			}
			catch (Exception e)
			{
				ErrorHelper.Show(this.Site, e);
			}
			finally
			{
				if (package != null)
				{
					package.TurnOffOutput();
				}
			}
            return VSConstants.S_OK;
		}

		int IVsTaskItem.NavigateToHelp()
		{
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.OnDeleteTask()
		{
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.OnFilterTask(int fVisible)
		{
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.SubcategoryIndex(out int pIndex)
		{
			pIndex = 0;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.get_Checked(out int pfChecked)
		{
			pfChecked = VSConstantsEx.FALSE;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.get_Priority(Microsoft.VisualStudio.Shell.Interop.VSTASKPRIORITY[] ptpPriority)
		{
			ptpPriority[0] = Microsoft.VisualStudio.Shell.Interop.VSTASKPRIORITY.TP_NORMAL;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.get_Text(out string pbstrName)
		{
			pbstrName = this.recipeReference.Key;
			return VSConstants.S_OK;
		}

		int IVsTaskItem.put_Checked(int fChecked)
		{
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.put_Priority(Microsoft.VisualStudio.Shell.Interop.VSTASKPRIORITY tpPriority)
		{
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem.put_Text(string bstrName)
		{
			return VSConstants.E_NOTIMPL;
		}
		
		#endregion

		#region IVsTaskItem3 members

		int IVsTaskItem3.GetTaskProvider(out IVsTaskProvider3 ppProvider)
		{
			ppProvider = (IVsTaskProvider3)GetService(typeof(IVsTaskProvider3));
			if (ppProvider == null)
			{
				return VSConstants.E_UNEXPECTED;
			}
			return VSConstants.S_OK;
		}

		int IVsTaskItem3.GetTaskName(out string pbstrName)
		{
			pbstrName = this.recipeReference.Key;
			return VSConstants.S_OK;
		}

		int IVsTaskItem3.GetColumnValue(int iField, out uint ptvtType, out uint ptvfFlags, out object pvarValue, out string pbstrAccessibilityName)
		{
			ptvfFlags = 0;
			pvarValue = null;
			pbstrAccessibilityName = null;
			ptvtType = (uint)__VSTASKVALUETYPE.TVT_NULL;
			switch ((RecipeTaskProvider.TaskColumns.Columns)iField)
			{
				case RecipeTaskProvider.TaskColumns.Columns.Recipe:
					{
						ptvtType = (uint)__VSTASKVALUETYPE.TVT_TEXT;
						pvarValue = this.recipeConfig.Caption;
						break;
					}
				case RecipeTaskProvider.TaskColumns.Columns.Project:
					{
						ptvtType = (uint)__VSTASKVALUETYPE.TVT_TEXT;
                        pvarValue = this.recipeReference.AppliesTo;
						break;
					}
				case RecipeTaskProvider.TaskColumns.Columns.Description:
					{
						ptvtType = (uint)__VSTASKVALUETYPE.TVT_TEXT;
						pvarValue = this.recipeConfig.Description;
						break;
					}
				default:
					return VSConstants.E_INVALIDARG;
			}
			return VSConstants.S_OK;
		}

		int IVsTaskItem3.GetTipText(int iField, out string pbstrTipText)
		{
			pbstrTipText = "";
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem3.SetColumnValue(int iField, ref object pvarValue)
		{
			pvarValue = null;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem3.GetEnumCount(int iField, out int pnValues)
		{
			pnValues = 0;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem3.GetEnumValue(int iField, int iValue, out object pvarValue, out string pbstrAccessibilityName)
		{
			pvarValue = null;
			pbstrAccessibilityName = null;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem3.OnLinkClicked(int iField, int iLinkIndex)
		{
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem3.GetNavigationStatusText(out string pbstrText)
		{
			pbstrText = "";
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem3.GetDefaultEditField(out int piField)
		{
			piField = -1;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskItem3.GetSurrogateProviderGuid(out System.Guid pguidProvider)
		{
			object service = GetService(typeof(IVsTaskProvider3));
			if (service != null)
			{
				pguidProvider = ((RecipeTaskProvider)service).Guid;
				return VSConstants.S_OK;
			}
			pguidProvider = Guid.Empty;
			return VSConstants.E_UNEXPECTED;
		}

		int IVsTaskItem3.IsDirty(out int pfDirty)
		{
			pfDirty = VSConstantsEx.TRUE;
			return VSConstants.S_OK;
		}
		
		#endregion
	}
}
