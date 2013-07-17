using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using EnvDTE;

/// <summary>
/// Utility methods for working with the IDE while debugging.
/// </summary>
internal class DebugUtils
{
	public static void DumpUIHierarchy(UIHierarchy hierarchy)
	{
		StringWriter sw = new StringWriter();
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
		StringWriter sw = new StringWriter();
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

	public static void DumpProperties(Properties props)
	{
		StringWriter sw = new StringWriter();
		foreach (Property prop in props)
			sw.WriteLine("Name={0}, Value={1}", prop.Name, prop.Value.ToString());

		System.Diagnostics.Debugger.Log(0, "Debug", sw.ToString());
	}

	//	public static void DumpCommandBars(DTE dte)
	//	{
	//		StringWriter sw = new StringWriter();
	//		IndentedTextWriter tw = new IndentedTextWriter(sw);
	//
	//		CommandBars bars = (CommandBars) dte.CommandBars;
	//
	//		foreach (CommandBar bar in bars)
	//		{
	//			DumpCommandBar(bar, tw);
	//		}
	//
	//		tw.Flush();
	//		System.Diagnostics.Debugger.Log(0, "Debug", sw.ToString());
	//	}
	//
	//	public static void DumpCommandBar(CommandBar bar, IndentedTextWriter tw)
	//	{
	//		tw.WriteLine(bar.Name);
	//		tw.Indent++;
	//		DumpControls(bar.Controls, tw);
	//		tw.Indent--;
	//	}
	//
	//
	//	public static void DumpControls(CommandBarControls controls, IndentedTextWriter tw)
	//	{
	//		foreach (CommandBarControl control in controls)
	//		{
	//			if (control is CommandBar)
	//			{
	//				tw.Indent++;
	//				DumpCommandBar((CommandBar)control, tw);
	//				tw.Indent--;
	//			}
	//			else
	//			{
	//				tw.WriteLine("[" + control.Caption + "]");
	//			}
	//		}
	//	}
}