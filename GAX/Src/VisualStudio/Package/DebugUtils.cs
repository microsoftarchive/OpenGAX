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

using System.CodeDom.Compiler;
using System.IO;
using EnvDTE;
using System.Globalization;

/// <summary>
/// Utility methods for working with the IDE while debugging.
/// </summary>
public class DebugUtils
{
	public static void DumpUIHierarchy(UIHierarchy hierarchy)
	{
		StringWriter sw = new StringWriter(CultureInfo.CurrentCulture);
		IndentedTextWriter tw = new IndentedTextWriter(sw);
		DumpUIHierarchyItems(hierarchy.UIHierarchyItems, tw);
		tw.Flush();

		System.Diagnostics.Debugger.Log(0, "Debug", sw.ToString());
	}

	private static void DumpUIHierarchyItems(UIHierarchyItems items, IndentedTextWriter tw)
	{
		tw.Indent++;
		foreach (UIHierarchyItem item in items)
		{
			tw.Write(item.Name);
			if (item.Object is ProjectItem)
				tw.WriteLine(" (ProjectItem)");
			else if (item.Object is EnvDTE80.SolutionFolder)
				tw.WriteLine(" (SolutionFolder)");
			else if (item.Object is Project)
				tw.WriteLine(" (Project)");
			else
				tw.WriteLine(" (Unknown)");

			if (item.UIHierarchyItems != null)
				DumpUIHierarchyItems(item.UIHierarchyItems, tw);
		}
		tw.Indent--;
	}

	public static void DumpProjectItems(DTE dte)
	{
        if (dte == null) return;
		StringWriter sw = new StringWriter(CultureInfo.CurrentCulture);
		IndentedTextWriter tw = new IndentedTextWriter(sw);

		tw.WriteLine(dte.Solution.FullName);
		System.Collections.IEnumerator en = dte.Solution.GetEnumerator();
		while (en.MoveNext())
		{
			if (en.Current is ProjectItem)
			{
				tw.Indent++;
				tw.WriteLine(((ProjectItem)en.Current).Name);
				DumpProjectItems(((ProjectItem)en.Current).ProjectItems, tw);
				tw.Indent--;
			}
			else if (en.Current is Project)
			{
				Project p = en.Current as Project;
				tw.Indent++;
				tw.WriteLine(p.Name + " (" + p.Kind + ")");
				DumpProjectItems(p.ProjectItems, tw);
				tw.Indent--;
			}
		}

		tw.Flush();

		System.Diagnostics.Debugger.Log(0, "Debug", sw.ToString());
	}

	private static void DumpProjectItems(ProjectItems items, IndentedTextWriter tw)
	{
		tw.Indent++;
		foreach (ProjectItem item in items)
		{
			tw.WriteLine(item.Name); // + "(" + item.Kind + ")");
			if (item.Object is Project)
			{
				DumpProjectItems(((Project)item.Object).ProjectItems, tw);
			}
//				else if (item.Object is EnvDTE80.SolutionFolder)
//				{
//					tw.WriteLine("**** SolutionFolder ****");
//				}
			else if (item.ProjectItems != null)
			{
				DumpProjectItems(item.ProjectItems, tw);
			}
		}
		tw.Indent--;
	}

	public static void DumpProperties(EnvDTE.Properties props)
	{
        if (props == null) return;
		StringWriter sw = new StringWriter(CultureInfo.CurrentCulture);
        foreach (Property prop in props)
        {
            try
            {
                sw.Write("Name={0}, ", prop.Name);
                sw.Write("Value={0}", prop.Value.ToString());
            }
            catch
            {
                sw.WriteLine();
            }
        }
		System.Diagnostics.Debugger.Log(0, "Debug", sw.ToString());
	}
}