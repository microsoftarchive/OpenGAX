//===================================================================================
// Microsoft patterns & practices
// Guidance Automation Toolkit
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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

/// <summary>
/// Class that contains utility functions to be called in the guidance package
/// </summary>
public class Utils
{
	/// <summary>
	/// Gets the string of the CLR installation directory
	/// </summary>
	/// <returns></returns>
    public static string GetClrInstallationDirectory()
    {
        int MAX_PATH = 260;
        StringBuilder sb = new StringBuilder(MAX_PATH);
        GetCORSystemDirectory(sb, MAX_PATH, ref MAX_PATH);
        return sb.ToString();
    }

    [DllImport("mscoree.dll")]
    private static extern int GetCORSystemDirectory(
        [MarshalAs(UnmanagedType.LPWStr)]StringBuilder pbuffer,
        int cchBuffer, ref int dwlength);

	internal static void SetIncludeInVsix(IServiceProvider provider, ProjectItem projectItem, bool value)
	{
		string projectUniqueName = projectItem.ContainingProject.UniqueName;
		IVsSolution vsSolution = (IVsSolution)provider.GetService(typeof(SVsSolution));
		if (vsSolution != null)
		{
			IVsHierarchy hierarchy;
			uint projectItemId;
			vsSolution.GetProjectOfUniqueName(projectUniqueName, out hierarchy);
			hierarchy.ParseCanonicalName(projectItem.get_FileNames(0), out projectItemId);
			IVsBuildPropertyStorage storage = hierarchy as IVsBuildPropertyStorage;
			if (hierarchy != null)
			{
				storage.SetItemAttribute(projectItemId, "IncludeInVSIX", value.ToString().ToLower());
			}
		}
	}

	internal static IEnumerable<ProjectItem> FindProjectItems(ProjectItems items, Predicate<ProjectItem> condition)
	{
		if (items != null)
		{
			foreach (ProjectItem item in items)
			{		
				if (condition(item))
					yield return item;

				foreach (ProjectItem subItem in FindProjectItems(item.ProjectItems, condition))
					yield return subItem;
			}
		}

		yield break;
	}
}