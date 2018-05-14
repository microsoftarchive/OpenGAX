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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common;
using System.Windows.Forms.Design;
#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.TaskList
{
	[ServiceDependency(typeof(IAssetReferenceService))]
	[ServiceDependency(typeof(IConfigurationService))]
	[ComVisible(true), Guid("7DA12822-CA24-47b1-B65A-7F41333B20C2")]
	internal sealed class RecipeTaskProvider : ContainerComponent, IVsTaskProvider2, IVsTaskProvider3
	{
		#region Fields

		private uint cookie;
		private Microsoft.VisualStudio.Shell.Interop.IVsTaskList taskList;
		private IAssetReferenceService referenceService;
		private RecipeTaskEnumerator recipeTaskEnumerator;

		public Guid Guid
		{
			get { return guid; }
		} Guid guid;

		#endregion

		#region Constructor

		public RecipeTaskProvider(Guid guid)
		{
			this.guid = guid;
		}

		#endregion

		#region Overrides

		protected override object GetService(Type serviceType)
		{
			if ( serviceType == typeof(IVsTaskProvider3) )
			{
				return this;
			}
			return base.GetService(serviceType);
		}

		protected override void OnSited()
		{
			base.OnSited();
			this.referenceService = (IAssetReferenceService)
				GetService(typeof(IAssetReferenceService),true);
			ReBuildTaskEnumerator();
			RegisterChangeEvent(true);
			cookie = Register();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (recipeTaskEnumerator != null)
				{
					Remove(recipeTaskEnumerator);
					recipeTaskEnumerator.Dispose();
					recipeTaskEnumerator = null;
				}
				RegisterChangeEvent(false);
				Unregister();
			}
			base.Dispose(disposing);
		}

		private void ReBuildTaskEnumerator()
		{
			if (recipeTaskEnumerator != null)
			{
				Remove(recipeTaskEnumerator);
				recipeTaskEnumerator.Dispose();
				recipeTaskEnumerator = null;
			}
			recipeTaskEnumerator = new RecipeTaskEnumerator();
			Add(recipeTaskEnumerator);
		}

		private uint Register()
		{
			if (taskList == null)
			{
				Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider =
					(Microsoft.VisualStudio.OLE.Interop.IServiceProvider)Site.GetService(typeof(Microsoft.VisualStudio.OLE.Interop.IServiceProvider));
				Guid SID_SVsTaskList = typeof(Microsoft.VisualStudio.Shell.Interop.SVsTaskList).GUID;
				Guid IID_IVsTaskList = typeof(Microsoft.VisualStudio.Shell.Interop.IVsTaskList).GUID;
				System.IntPtr objectTaskList = IntPtr.Zero;
				serviceProvider.QueryService(ref SID_SVsTaskList, ref IID_IVsTaskList, out objectTaskList);
				taskList = Marshal.GetObjectForIUnknown(objectTaskList) as IVsTaskList;
			}
			if (taskList!=null )
			{
				taskList.RegisterTaskProvider(this, out cookie);
			}
			else
			{
                throw new InvalidOperationException(Properties.Resources.TaskList_CannotRegister);
			}
			return cookie;
		}

		private void Unregister()
		{
			if (taskList!=null)
			{
				taskList.RefreshTasks(cookie);
				taskList.UnregisterTaskProvider(cookie);
				taskList = null;
			}
		}

		#endregion

		#region Event registration and handlers
		
		private void RegisterChangeEvent(bool register)
		{
			if (register)
			{
				this.referenceService.Changed += new EventHandler(OnReferencesChanged);
			}
			else
			{
				this.referenceService.Changed -= new EventHandler(OnReferencesChanged);
			}
		}

		private void OnReferencesChanged(object sender, EventArgs args)
		{
            try
            {
                ReBuildTaskEnumerator();
                this.taskList.RefreshTasks(cookie);
            }
            catch (Exception e)
            {
                ErrorHelper.Show(this, e);
            }
        }

		#endregion

		#region IVsTaskProvider Members

		int IVsTaskProvider.EnumTaskItems(out IVsEnumTaskItems ppenum)
		{
			return ((IVsTaskProvider2)this).EnumTaskItems(out ppenum);
		}

		int IVsTaskProvider.ImageList(out IntPtr phImageList)
		{
			return ((IVsTaskProvider2)this).ImageList(out phImageList);
		}

		int IVsTaskProvider.OnTaskListFinalRelease(IVsTaskList pTaskList)
		{
			return ((IVsTaskProvider2)this).OnTaskListFinalRelease(pTaskList);
		}

		int IVsTaskProvider.ReRegistrationKey(out string pbstrKey)
		{
			return ((IVsTaskProvider2)this).ReRegistrationKey(out pbstrKey);
		}

		int IVsTaskProvider.SubcategoryList(uint cbstr, string[] rgbstr, out uint pcActual)
		{
			return ((IVsTaskProvider2)this).SubcategoryList(cbstr, rgbstr, out pcActual);
		}


		#endregion

		#region IVsTaskProvider2 members

		int IVsTaskProvider2.EnumTaskItems(out Microsoft.VisualStudio.Shell.Interop.IVsEnumTaskItems ppenum)
		{
			ppenum = recipeTaskEnumerator;
			return VSConstants.S_OK;
		}

		int IVsTaskProvider2.ImageList(out System.IntPtr phImageList)
		{
			phImageList = IntPtr.Zero;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskProvider2.OnTaskListFinalRelease(Microsoft.VisualStudio.Shell.Interop.IVsTaskList pTaskList)
		{
			Dispose(true);
			return VSConstants.S_OK;
		}

		int IVsTaskProvider2.ReRegistrationKey(out string pbstrKey)
		{
			pbstrKey = "TODO";
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskProvider2.SubcategoryList(uint cbstr, string[] rgbstr, out uint pcActual)
		{
			pcActual = 0;
			return VSConstants.E_NOTIMPL;
		}
		
		int IVsTaskProvider2.MaintainInitialTaskOrder(out int bMaintainOrder)
		{
			bMaintainOrder = 1;
			return VSConstants.S_OK;
		}
		
		#endregion
		
		#region IVsTaskProvider3 members

		int IVsTaskProvider3.GetProviderFlags(out uint tpfFlags)
		{
			tpfFlags = 1; // Always visible
			return VSConstants.S_OK;
		}

		int IVsTaskProvider3.GetProviderGuid(out System.Guid pguidProvider)
		{
			pguidProvider = this.Guid;
			return VSConstants.S_OK;
		}

		int IVsTaskProvider3.GetProviderName(out string pbstrName)
		{
			IConfigurationService configService=
				(IConfigurationService)GetService(typeof(IConfigurationService));
			if (configService != null)
			{
				pbstrName = configService.CurrentPackage.Caption;
				return VSConstants.S_OK;
			}
			pbstrName = "Unknown";
			return VSConstants.E_UNEXPECTED;
		}

		int IVsTaskProvider3.GetProviderToolbar(out System.Guid pguidGroup, out uint pdwID)
		{
			pdwID = 0;
			pguidGroup = Guid.Empty;
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskProvider3.GetSurrogateProviderGuid(out System.Guid pguidProvider)
		{
			pguidProvider = this.Guid;
			return VSConstants.S_OK;
		}

		int IVsTaskProvider3.OnBeginTaskEdit(Microsoft.VisualStudio.Shell.Interop.IVsTaskItem pItem)
		{
			return VSConstants.E_NOTIMPL;
		}

		int IVsTaskProvider3.OnEndTaskEdit(Microsoft.VisualStudio.Shell.Interop.IVsTaskItem pItem, int fCommitChanges, out int pfAllowChanges)
		{
			pfAllowChanges = VSConstantsEx.FALSE;
			return VSConstants.E_NOTIMPL;
		}
		
		internal class TaskColumns
		{
			public const int Count = 3;
			public const int MinColWidth = 14;
			public const int NameColWidth = 20;
			public enum Columns
			{
				Recipe = 0,
				Project = 1,
				Description = 2,
			};
		}

		int IVsTaskProvider3.GetColumnCount(out int pnColumns)
		{
			pnColumns = TaskColumns.Count;
			return VSConstants.S_OK;
		}

		int IVsTaskProvider3.GetColumn(int iColumn, Microsoft.VisualStudio.Shell.Interop.VSTASKCOLUMN[] pColumn)
		{
			if (pColumn == null || iColumn < 0 || iColumn > TaskColumns.Count)
			{
				return VSConstants.E_INVALIDARG;
			}
			pColumn[0].iImage = -1;
			pColumn[0].fShowSortArrow = VSConstantsEx.TRUE;
			pColumn[0].fAllowUserSort = VSConstantsEx.TRUE;
			pColumn[0].fVisibleByDefault = VSConstantsEx.TRUE;
			pColumn[0].fAllowHide = VSConstantsEx.TRUE;
			pColumn[0].fSizeable = VSConstantsEx.TRUE;
			pColumn[0].fMoveable = VSConstantsEx.TRUE;
			pColumn[0].iDefaultSortPriority = 1;
			pColumn[0].fDescendingSort = VSConstantsEx.FALSE;
			pColumn[0].cxMinWidth = TaskColumns.MinColWidth;
			pColumn[0].cxDefaultWidth = TaskColumns.NameColWidth;
			pColumn[0].fDynamicSize = VSConstantsEx.TRUE;
			pColumn[0].bstrTip = null;
			pColumn[0].iField = iColumn;
			switch ((TaskColumns.Columns)iColumn)
			{
				case TaskColumns.Columns.Recipe:
				{
					pColumn[0].bstrHeading = Properties.Resources.TaskList_ColumnName;
					pColumn[0].bstrCanonicalName = "Recipe";
					pColumn[0].bstrLocalizedName = Properties.Resources.TaskList_ColumnName;
					break;
				}
				case TaskColumns.Columns.Project:
				{
					pColumn[0].bstrHeading = Properties.Resources.TaskList_ProjectItem;
					pColumn[0].bstrCanonicalName = "Project";
					pColumn[0].bstrLocalizedName = Properties.Resources.TaskList_ProjectItem;
					break;
				}
				case TaskColumns.Columns.Description:
				{
					pColumn[0].bstrHeading = Properties.Resources.TaskList_Description;
					pColumn[0].bstrCanonicalName = "Description";
					pColumn[0].bstrLocalizedName = Properties.Resources.TaskList_Description;
					break;
				}
			}
			return VSConstants.S_OK;
		}
		
		#endregion
	}
}
