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
using System.IO;
using EnvDTE;
using VSLangProj;
using System.Collections;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.VisualStudio.OLE.Interop;
using System.ComponentModel.Design;
using EnvDTE80;
using System.Collections.Generic;
using Microsoft.Build.Evaluation;
using Project = EnvDTE.Project;
using ProjectItem = EnvDTE.ProjectItem;
using BProject = Microsoft.Build.Evaluation.Project;

namespace Microsoft.Practices.RecipeFramework.Library
{
	/// <summary>
	/// Provides utility methods for working with the DTE.
	/// </summary>
	public sealed class DteHelper: Microsoft.Practices.RecipeFramework.VisualStudio.Library.DteHelper
	{
		private DteHelper() { }

		#region Select methods
		
		/// <summary>
		/// Selects an item by unfolding , based on 
		/// its relative path with regards to the solution.
		/// </summary>
		/// <remarks>
		/// If selection fails, returned object will be null too.
		/// </remarks>
		public static UIHierarchyItem SelectItem(_DTE vs, object target)
		{
			Guard.ArgumentNotNull(vs, "vs");
			Guard.ArgumentNotNull(target, "target");

			UIHierarchy hier;
			UIHierarchyItem sol;
			GetAndSelectSolutionExplorerHierarchy(vs, out hier, out sol);

			Dictionary<UIHierarchyItems, bool> expandedItems = new Dictionary<UIHierarchyItems, bool>();

			UIHierarchyItem locatedItem = LocateInUICollection(sol.UIHierarchyItems, target, expandedItems);
			RestoreExpandedItems(expandedItems);

			return locatedItem;
		}

		private static void RestoreExpandedItems(Dictionary<UIHierarchyItems, bool> expandedItems)
		{
			foreach (KeyValuePair<UIHierarchyItems, bool> pair in expandedItems)
			{
				pair.Key.Expanded = pair.Value;
			}
		}

		private static UIHierarchyItem LocateInUICollection(UIHierarchyItems items, object target, Dictionary<UIHierarchyItems, bool> expandedItems)
		{
			if (items == null) return null;

			foreach (UIHierarchyItem item in items)
			{
				ProjectItem prjItem = item.Object as ProjectItem;
				if (item.Object == target ||
					(prjItem != null && prjItem.Object == target))
				{
					item.Select(vsUISelectionType.vsUISelectionTypeSelect);
					return item;
				}

				UIHierarchyItem child = LocateInUICollection(item.UIHierarchyItems, target, expandedItems);
				if (child != null) return child;
			}

			expandedItems.Add(items, items.Expanded);

			return null;
		}

		private static void GetAndSelectSolutionExplorerHierarchy(_DTE vs, out UIHierarchy hier, out UIHierarchyItem sol)
		{
			if (vs == null)
			{
				throw new ArgumentNullException("vs");
			}
			// Select the parent folder to add the project to it.
			Window win = vs.Windows.Item(EnvDTE.Constants.vsWindowKindSolutionExplorer);
			if (win == null)
			{
				// Can't select as there's no solution explorer open.
				throw new InvalidOperationException(Properties.Resources.DteHelper_NoSolutionExplorer);
			}
			win.Activate();
			win.SetFocus();
			hier = win.Object as UIHierarchy;
			sol = hier.UIHierarchyItems.Item(1);
			if (sol == null)
			{
				// No solution is opened.
				throw new InvalidOperationException(Properties.Resources.DteHelper_NoSolutionExplorer);
			}
			sol.Select(vsUISelectionType.vsUISelectionTypeSelect);
		}

		#endregion Select methods

		#region IsXXX methods

		private static string[] webprojguids = {
				"{349C5851-65DF-11DA-9384-00065B846F21}", // web app and mvc 5
				"{8BB2217D-0F2D-49D1-97BC-3654ED321F3B}", // asp.net 5
				"{603C0E0B-DB56-11DC-BE95-000D561079B0}", // ASP.NET MVC 1
				"{F85E285D-A4E0-4152-9332-AB1D724D3325}", // ASP.NET MVC 2	
				"{E53F8FEA-EAE0-44A6-8774-FFD645390401}", // ASP.NET MVC 3
				"{E3E379DF-F4C6-4180-9B81-6769533ABE47}", // ASP.NET MVC 4
				"{E24C65DC-7377-472b-9ABA-BC803B73C61A}" // web site 
			};

		/// <summary>
		/// Determines whether the project is a web project.
		/// </summary>
		public static new bool IsWebProject(Project project)
		{
			Guard.ArgumentNotNull(project, "project");

			System.IServiceProvider sp = GetServiceProvider(project);
			string[] guids = GetProjectTypeGuids(project, sp);

			if(guids == null)
				return project.Kind == VsWebSite.PrjKind.prjKindVenusProject;

			for (int i = 0; i < guids.Length; i++)
			{
				guids[i] = guids[i].ToUpper();
			}

			for (int i = 0; i < webprojguids.Length; i++)
			{
				for (int j = 0; j < guids.Length; j++)
				{
					if (webprojguids[i] == guids[j])
						return true;
				}
			}
			return false;
		}

		#endregion IsXXX methods

		public static System.IServiceProvider GetServiceProvider(EnvDTE.Project project)
		{
			System.IServiceProvider sp = new Microsoft.VisualStudio.Shell.ServiceProvider(project.DTE as
				Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
			return sp;
		}

		public static IVsHierarchy GetVsHierarchy(EnvDTE.Project project)
		{
			return GetVsHierarchy(GetServiceProvider(project), project);
		}

		/// <summary>
		/// use IVsBuildPropertyStorage to get project properties.
		/// </summary>
		/// <param name="prj"></param>
		/// <param name="provider"></param>
		/// <returns>if failed, return null</returns>
		public static string[] GetProjectTypeGuids(Project project, System.IServiceProvider provider)
		{
			string sv;
			IVsHierarchy pHierarchy = GetVsHierarchy(provider, project);
			if (pHierarchy != null)
			{
				IVsBuildPropertyStorage bs = pHierarchy as IVsBuildPropertyStorage;
				if (bs == null)
				{
					System.Diagnostics.Debugger.Log(0, "Debug", "IVsBuildPropertyStorage not implemented on IVsHierarchy");
					return null;
				}

				int hr =
					bs.GetPropertyValue("ProjectTypeGuids", string.Empty, (uint)_PersistStorageType.PST_PROJECT_FILE, out sv);
				if (hr == 0)
				{
					return sv.Split(';');
				}
			}

			return null;
		}

		/// <summary>
		/// use msbuild assembly to get it.
		/// </summary>
		/// <param name="project"></param>
		public static string[] GetProjectTypeGuids(Project project)
		{
			ICollection<BProject> bprjs = ProjectCollection.GlobalProjectCollection.LoadedProjects;

			foreach (BProject bp in bprjs)
			{
				if (string.Compare(bp.FullPath, project.FileName) == 0)
				{
					string sv = bp.GetPropertyValue("ProjectTypeGuids");
					if (!string.IsNullOrEmpty(sv))
					{
						return sv.Split(';');
					}
				}
			}
			return null;
		}
	}
}
