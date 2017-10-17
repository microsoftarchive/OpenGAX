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

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library
{
	/// <summary>
	/// Provides utility methods for working with the DTE.
	/// </summary>
	public class DteHelper
	{
		protected DteHelper() { }

		#region BuildPath

		/// <summary>
		/// Builds a path to the element, detecting automatically the 
		/// type of element and building an appropriate path to it.
		/// </summary>
		public static string BuildPath(object toElement)
		{
			Guard.ArgumentNotNull(toElement, "toElement");

			if (toElement is SelectedItem)
			{
				return BuildPath((SelectedItem)toElement);
			}
			else if (toElement is SolutionFolder)
			{
				return BuildPath(((SolutionFolder)toElement).Parent);
			}
			else if (toElement is Project)
			{
				return BuildPath((Project)toElement);
			}
			else if (toElement is ProjectItem)
			{
				return BuildPath((ProjectItem)toElement);
			}
			else
			{
				throw new NotSupportedException(toElement.ToString());
			}
		}

		/// <summary>
		/// Gets an string representation from the current selected item in the solution explorer window.
		/// </summary>
		/// <param name="toSelectedItem"></param>
		/// <returns></returns>
		public static string BuildPath(SelectedItem toSelectedItem)
		{
			Guard.ArgumentNotNull(toSelectedItem, "toSelectedItem");
			if (toSelectedItem.ProjectItem != null)
			{
				return BuildPath(toSelectedItem.ProjectItem);
			}
			else if (toSelectedItem.Project != null)
			{
				return BuildPath(toSelectedItem.Project);
			}

			return toSelectedItem.Name;
		}

		/// <summary>
		/// Gets an string representatio from <paramref name="toProject"/>
		/// </summary>
		public static string BuildPath(Project toProject)
		{
			Guard.ArgumentNotNull(toProject, "toProject");

			string path = "";

			// EnvDTE.Solution folders are exposed with the same kind as solution items. 
			// This eases compatibility with VS2003 in the future.
			if (toProject.Kind == EnvDTE.Constants.vsProjectKindSolutionItems)
			{
				string folder = "";
				foreach (Project project in toProject.DTE.Solution.Projects)
				{
					folder = project.Name;
					// Only build the path if it's not the same top-level project.
					if (project == toProject)
					{
						break;
					}
					else if (BuildPathToFolder(project, toProject, ref folder))
					{
						break;
					}
				}

				path = folder + path;
			}
			else
			{
				try
				{
					// Setup projects throw NotImplementedException when queried for ParentProjectItem :S
					if (toProject.ParentProjectItem == null)
					{
						return toProject.Name;
					}
				}
				catch (NotImplementedException) { }

				string folder = "";
				foreach (Project project in toProject.DTE.Solution.Projects)
				{
					folder = project.Name;
					// Only build the path if it's not the same top-level project.
					if (project == toProject)
					{
						break;
					}
					else if (BuildPathToFolder(project, toProject, ref folder))
					{
						break;
					}
				}

				path = folder + path;
			}

			return path;
		}

		/// <summary>
		/// Gets an string representation from <paramref name="toItem"/>
		/// </summary>
		/// <param name="toItem"></param>
		/// <returns></returns>
		public static string BuildPath(ProjectItem toItem)
		{
			Guard.ArgumentNotNull(toItem, "toItem");
			string path = "";

			if (toItem.ContainingProject != null)
			{
				if (!BuildPathFromCollection(toItem.ContainingProject.ProjectItems, toItem, ref path))
				{
					return "";
				}
				else
				{
					path = Path.Combine(BuildPath(toItem.ContainingProject),path);
				}
			}
			else
			{
				path = toItem.Name;
			}

			return path;
		}

		/// <summary>
		/// Builds a virtual path from an ancestor collection of items to 
		/// a particular instance, which can be any number of levels of nesting 
		/// down the hierarchy.
		/// </summary>
		/// <param name="items">The parent collection of items.</param>
		/// <param name="target">The item to locate in the hierarchy.</param>
		/// <param name="path">The path being built.</param>
		/// <returns>Whether the item was found in the hierarchy of items.</returns>
		public static bool BuildPathFromCollection(ProjectItems items, ProjectItem target, ref string path)
		{
			if (items == null) return false;
			Guard.ArgumentNotNull(target, "target");

			foreach (ProjectItem item in items)
			{
				if (item == target)
				{
					path = path + target.Name;
					return true;
				}
				else
				{
					string tmp = path + item.Name + Path.DirectorySeparatorChar;
					ProjectItems childitems = item.ProjectItems;
					if (childitems == null && item.Object is Project)
						childitems = ((Project)item.Object).ProjectItems;

					bool found = BuildPathFromCollection(childitems, target, ref tmp);
					if (found)
					{
						path = tmp;
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Recursively tries to build a path from the parent project to the child one. 
		/// Returns true if the path can be built.
		/// </summary>
		private static bool BuildPathToFolder(Project parent, Project target, ref string path)
		{
			if (parent == null || parent.ProjectItems == null) return false;

			foreach (ProjectItem item in parent.ProjectItems)
			{
				try
				{
					if (item.Object == target)
					{
						path = path + Path.DirectorySeparatorChar + target.Name;
						return true;
					}
					else if (item.Kind == EnvDTE.Constants.vsProjectItemKindSolutionItems)
					{
						string tmp = path + Path.DirectorySeparatorChar + item.Name;
						bool found = BuildPathToFolder(item.Object as Project, target, ref tmp);
						if (found)
						{
							path = tmp;
							return true;
						}
					}
				}
				catch
				{
					// This is for safety.
					// Sometimes there may be invalid items laying around in the solution explorer.
					continue;
				}
			}

			return false;
		}

		#endregion BuildPath

		#region Find methods

		/// <summary>
		/// Retrieves the first project in the solution that matches the specified criteria.
		/// </summary>
		/// <param name="vs">The VS instance.</param>
		/// <param name="match">The predicate condition.</param>
		/// <returns>The project found or <see langword="null"/>.</returns>
		public static Project FindProject(_DTE vs, Predicate<Project> match)
		{
			Guard.ArgumentNotNull(vs, "vs");
			Guard.ArgumentNotNull(match, "match");

			foreach (Project project in vs.Solution.Projects)
			{
				if (match(project))
				{
					return project;
				}
				else if (project.ProjectItems != null)
				{
					Project child = FindProjectInternal(project.ProjectItems, match);
					if (child != null)
					{
						return child;
					}
				}
			}

			return null;
		}

		private static Project FindProjectInternal(ProjectItems items, Predicate<Project> match)
		{
			foreach (ProjectItem item in items)
			{
				Project project = item.Object as Project;
				if (project != null && match(project))
				{
					return project;
				}
				else if (item.ProjectItems != null)
				{
					Project child = FindProjectInternal(item.ProjectItems, match);
					if (child != null)
					{
						return child;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Finds a project in the solution, given its output assembly name.
		/// </summary>
		/// <returns>A <see cref="Project"/> reference or <see langword="null" /> if 
		/// it doesn't exist. Project can be C# or VB.</returns>
		public static Project FindProjectByAssemblyName(_DTE vs, string name)
		{
			return FindProject(vs, delegate(Project project)
			{
				Property prop = project.Properties.Item("AssemblyName");
				return prop != null && prop.Value != null &&
					prop.Value.ToString() == name;
			});
		}

		/// <summary>
		/// Finds a project in the solution, given its name.
		/// </summary>
		/// <returns>A <see cref="Project"/> reference or <see langword="null" /> if 
		/// it doesn't exist. Project can be C# or VB.</returns>
		public static Project FindProjectByName(_DTE vs, string name)
		{
			return FindProject(vs, delegate(Project project)
			{
				return project.Name == name;
			});
		}

		/// <summary>
		/// Finds a solution folder in the solution hierarchy, given its 
		/// folder-like location path. 
		/// </summary>
		/// <returns>The solution folder or <see langword="null" /> if 
		/// it doesn't exist.</returns>
		/// <remarks>
		/// Note that this method performs the same work as <see cref="FindProjectByPath"/>, 
		/// but only returns an instance if the EnvDTE.Project.Object is actually 
		/// an <see cref="EnvDTE80.SolutionFolder"/>. This is because solution folders 
		/// are represented as <see cref="Project"/> elements in Visual Studio.
		/// </remarks>
        public static EnvDTE80.SolutionFolder FindSolutionFolderByPath(EnvDTE.Solution root, string path)
		{
			Guard.ArgumentNotNull(root, "root");
			Guard.ArgumentNotNull(path, "path");
			Project prj = FindProjectByPath(root, path);
			if (prj != null)
			{
				return prj.Object as EnvDTE80.SolutionFolder;
			}
			return null;
		}

		/// <summary>
		/// Finds a project in the solution hierarchy, given its 
		/// folder-like location path. Note that solution folders will 
		/// also be returned, as they are represented as <see cref="Project"/> elements 
		/// in Visual Studio, and the actual folder can be retrieved by casting  
		/// the returned Project.Object property to 
		/// <see cref="EnvDTE80.SolutionFolder"/>.
		/// </summary>
		/// <returns>The project or <see langword="null" /> if 
		/// it doesn't exist.</returns>
		public static Project FindProjectByPath(EnvDTE.Solution root, string path)
		{
			Guard.ArgumentNotNull(root, "root");
			Guard.ArgumentNotNull(path, "path");

			string[] allpaths = path.Split(System.IO.Path.DirectorySeparatorChar,
				System.IO.Path.AltDirectorySeparatorChar);

			if (allpaths.Length == 0)
			{
				return null;
			}

			// First path is the project/solution folder to look into.
			Project prj = null;
			foreach (Project p in root.Projects)
			{
				if (p.Name == allpaths[0])
				{
					prj = p;
					break;
				}
			}

			if (prj == null) return null;

			string[] paths = new string[allpaths.Length - 1];
			// If there are no child paths, we reached the end.
			if (paths.Length == 0)
			{
				return prj;
			}

			Array.Copy(allpaths, 1, paths, 0, paths.Length);

			ProjectItem item = FindInCollectionRecursive(prj.ProjectItems, paths, 0);
			if (item == null)
			{
				return null;
			}
			{
				return item.Object as Project;
			}
		}

		/// <summary>
		/// Finds a project item in the received collection, given its name.
		/// </summary>
		/// <param name="collection">The initial collection to start the search.</param>
		/// <param name="name">The name of the item to locate.</param>
		/// <param name="recursive">Specifies whether to search in items collections in turn.</param>
		/// <returns>A <see cref="Project"/> reference or <see langword="null" /> if 
		/// it doesn't exist. Project can be C# or VB.</returns>
		public static ProjectItem FindItemByName(ProjectItems collection, string name, bool recursive)
		{
			Guard.ArgumentNotNull(collection, "collection");
			Guard.ArgumentNotNull(name, "name");
			foreach (ProjectItem item in collection)
			{
				if (item.Name == name)
					return item;

				// Recurse if specified.
				if (recursive)
				{
					ProjectItem child = FindItemByName(item.ProjectItems, name, recursive);
					if (child != null)
						return child;
				}
			}

			return null;
		}

		/// <summary>
		/// Finds a project item in the solution hierarchy, given its 
		/// folder-like location path. 
		/// </summary>
		/// <returns>The project item or <see langword="null" /> if 
		/// it doesn't exist.</returns>
		/// <remarks>
		/// Note that even projects and solution folders are represented 
		/// as project items, if they are not directly under the solution root, 
		/// but this method checks for the ProjectItem.Object property 
		/// to discard matches in this case. If the object to retrieve is a 
		/// project (or solution folder, which is represented as a project too, 
		/// and the folder is retrieved through the Project.Object property), 
		/// the <see cref="FindProjectByPath"/> method must be used.
		/// </remarks>
		public static ProjectItem FindItemByPath(EnvDTE.Solution root, string path)
		{
			Guard.ArgumentNotNull(root, "root");
			Guard.ArgumentNotNull(path, "path");

			string[] allpaths = path.Split(
				new char[] {
					System.IO.Path.DirectorySeparatorChar,
					System.IO.Path.AltDirectorySeparatorChar },
				StringSplitOptions.RemoveEmptyEntries);

			if (allpaths.Length == 0)
			{
				return null;
			}

			// First path is the project/solution folder to look into.
			Project prj = null;
			foreach (Project p in root.Projects)
			{
				if (p.Name == allpaths[0])
				{
					prj = p;
					break;
				}
			}

			if (prj == null)
			{
				return null;
			}

			string[] paths = new string[allpaths.Length - 1];
			// If there are no child paths, this is not an item but the project itself.
			if (paths.Length == 0)
			{
				return null;
			}

			Array.Copy(allpaths, 1, paths, 0, paths.Length);

			ProjectItem item = FindInCollectionRecursive(prj.ProjectItems, paths, 0);
			if ((item != null) && !(item.Object is Project || item.Object is EnvDTE80.SolutionFolder))
			{
				// Only return the item if it's not a container for a Project or SolutionFolder.
				return item;
			}
			return null;
		}

		/// <summary>
		/// Finds an item in a collection using its path-like notation inside the hierarchy of 
		/// <see cref="ProjectItems"/>.
		/// </summary>
		public static ProjectItem FindInCollection(ProjectItems collection, string path)
		{
			string[] allpaths = path.Split(
				new char[] {
					System.IO.Path.DirectorySeparatorChar,
					System.IO.Path.AltDirectorySeparatorChar }, 
				StringSplitOptions.RemoveEmptyEntries);

			return FindInCollectionRecursive(collection, allpaths, 0);
		}

		private static ProjectItem FindInCollectionRecursive(ProjectItems collection, string[] paths, int index)
		{
			foreach (ProjectItem item in collection)
			{
				if (item.Name == paths[index] ||
					MatchesWebProjectName(item, paths[index]))
				{
					if (index == paths.Length - 1)
					{
						// We reached the item we were looking for.
						return item;
					}
					else
					{
						// Otherwise, keep processing.
						// If item is a project/solution folder, cast before moving on.
						if (item.Object is Project)
						{
							return FindInCollectionRecursive(
								((Project)item.Object).ProjectItems,
								paths, ++index);
						}
						else
						{
							return FindInCollectionRecursive(item.ProjectItems, paths, ++index);
						}
					}
				}
			}

			// Item wasn't found.
			return null;
		}

		private static bool MatchesWebProjectName(ProjectItem item, string name)
		{
			Project project = item.Object as Project;
			if (project != null && project.Kind == VsWebSite.PrjKind.prjKindVenusProject)
			{
				string simpleName = Path.GetDirectoryName(item.Name);
				simpleName = simpleName.Substring(simpleName.LastIndexOf(Path.DirectorySeparatorChar) + 1);
				if (name == simpleName)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// UIHierarchy.GetItem does not always work :S, so I do it by hand here.
		/// </summary>
		private static UIHierarchyItem FindHierarchyItemByPath(UIHierarchyItems items, string[] paths, int index, Dictionary<UIHierarchyItems, bool> expandedItems)
		{
			foreach (UIHierarchyItem item in items)
			{
				if (item.Name == paths[index])
				{
					if (index == paths.Length - 1)
					{
						// We reached the item we were looking for.
						return item;
					}
					else
					{
						// Otherwise, keep processing.
						return FindHierarchyItemByPath(item.UIHierarchyItems, paths, ++index, expandedItems);
					}
				}
			}

			expandedItems.Add(items, items.Expanded);

			// Item wasn't found.
			return null;
		}

		#endregion Find methods

		#region Get methods

		/// <summary>
		/// Retrieves the currently selected target in the solution explorer.
		/// </summary>
		/// <remarks>
		/// If there's only one item selected, then the corresponding EnvDTE.Solution, Project 
		/// or ProjectItem element is returned. Otherwise, the <see cref="EnvDTE.SelectedItems"/> 
		/// property is returned.
		/// </remarks>
		/// <exception cref="InvalidOperationException">There's no current selection in the solution explorer.</exception>
		/// <returns>The current selection.</returns>
		public static object GetTarget(_DTE vs)
		{
			Guard.ArgumentNotNull(vs, "vs");

			if (vs.SelectedItems != null && vs.SelectedItems.Count > 0)
			{
				if (vs.SelectedItems.Count == 1)
				{
					EnvDTE.SelectedItem item = vs.SelectedItems.Item(1);
					// Determine target to load.
					if (item.Project != null)
					{
						return item.Project;
					}
					else if (item.ProjectItem != null)
					{
						return item.ProjectItem;
					}
					else if (vs.Solution.Properties.Item("Name").Value.Equals(item.Name))
					{
						return vs.Solution;
					}
					return item;
				}
				else
				{
					//If more than one element is selected, then assert should deal with the DTE.SelectedItems object
					return vs.SelectedItems;
				}
			}
			throw new InvalidOperationException(Properties.Resources.DteHelper_NoSelection);
		}

		/// <summary>
		/// Retrieves the code file extension for the project.
		/// </summary>
		public static string GetDefaultExtension(Project project)
		{
			Guard.ArgumentNotNull(project, "project");

			if (project.Kind == PrjKind.prjKindCSharpProject)
			{
				return ".cs";
			}
			else if (project.Kind == PrjKind.prjKindVBProject)
			{
				return ".vb";
			}
			else
			{
				throw new NotSupportedException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.DteHelper_UnsupportedProjectKind,
					project.Name));
			}
		}

		/// <summary>
		/// Retrieves a default namespace to use for project items.
		/// </summary>
		public static string GetProjectNamespace(Project project)
		{
			Guard.ArgumentNotNull(project, "project");

			string ns = project.Properties.Item("DefaultNamespace").Value.ToString();
			if (string.IsNullOrEmpty(ns))
			{
				ns = project.Properties.Item("RootNamespace").Value.ToString();
			}
			if (string.IsNullOrEmpty(ns))
			{
				ns = project.Properties.Item("AssemblyName").Value.ToString();
			}
			return ns;
		}

		/// <summary>
		/// Retrieves the project currently selected, if any.
		/// </summary>
		public static Project GetSelectedProject(_DTE vs)
		{
			Guard.ArgumentNotNull(vs, "vs");

			foreach (object obj in (object[])vs.ActiveSolutionProjects)
			{
				if (obj is Project) return obj as Project;
			}

			return null;
		}

		/// <summary>
		/// Returns the item file name relative to the containing solution.
		/// </summary>
		public static string GetFilePathRelative(ProjectItem item)
		{
			Guard.ArgumentNotNull(item, "item");
			return GetFilePathRelative(item.DTE, item.get_FileNames(1));
		}

		/// <summary>
		/// Turns the file name received into a path relative to the containing solution.
		/// </summary>
		public static string GetFilePathRelative(_DTE vs, string file)
		{
			Guard.ArgumentNotNull(vs, "vs");
			Guard.ArgumentNotNull(file, "file");

			if (!file.StartsWith(Path.GetDirectoryName(vs.Solution.FullName)))
			{
				throw new ArgumentException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.DteHelper_PathNotRelativeToSln,
					file, vs.Solution.FullName));
			}
			string relative = file.Replace(Path.GetDirectoryName(vs.Solution.FullName), "");
			if (relative.StartsWith(Path.DirectorySeparatorChar.ToString()))
			{
				relative = relative.Substring(1);
			}
			return relative;
		}

		/// <summary>
		/// Turns the relative file name received into full path, based on the containing solution location.
		/// </summary>
		public static string GetPathFull(_DTE vs, string file)
		{
			Guard.ArgumentNotNull(vs, "vs");
			Guard.ArgumentNotNull(file, "file");

			if (Path.IsPathRooted(file) &&
				!file.StartsWith(Path.GetDirectoryName(vs.Solution.FullName)))
			{
				throw new ArgumentException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.DteHelper_PathNotRelativeToSln,
					file, vs.Solution.FullName));
			}
			return Path.Combine(Path.GetDirectoryName(vs.Solution.FullName), file);
		}

		#endregion Get methods

		#region Select methods

		/// <summary>
		/// Selects the solution in the solution explorer.
		/// </summary>
		/// <returns>Whether the selection was successful.</returns>
		public static bool SelectSolution(_DTE vs)
		{
			Guard.ArgumentNotNull(vs, "vs");

			UIHierarchy hier;
			UIHierarchyItem sol;

			try
			{
				GetAndSelectSolutionExplorerHierarchy(vs, out hier, out sol);

			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Selects a solution explorer item, based on 
		/// its relative path with regards to the solution.
		/// </summary>
		/// <remarks>
		/// If selection fails, returned object will be null.
		/// </remarks>
		public static UIHierarchyItem SelectItem(_DTE vs, string path)
		{
			Guard.ArgumentNotNull(vs, "vs");
			Guard.ArgumentNotNullOrEmptyString(path, "path");

			UIHierarchy hier;
			UIHierarchyItem sol;
			GetAndSelectSolutionExplorerHierarchy(vs, out hier, out sol);

			// Perform selection.
			UIHierarchyItem item = null;
			try
			{
				string slnpath = Path.Combine(sol.Name, path);
				item = hier.GetItem(slnpath);
			}
			catch (ArgumentException)
			{
				Dictionary<UIHierarchyItems, bool> expandedItems = new Dictionary<UIHierarchyItems, bool>();

				// Retry selection by name (slower)
				item = FindHierarchyItemByPath(sol.UIHierarchyItems,
					path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), 0, expandedItems);
				RestoreExpandedItems(expandedItems);
			}

			if (item != null)
			{
				item.UIHierarchyItems.Expanded = true;
				item.Select(vsUISelectionType.vsUISelectionTypeSelect);
			}

			return item;
		}

		/// <summary>
		/// Selects an item by unfolding , based on 
		/// its relative path with regards to the solution.
		/// </summary>
		/// <remarks>
		/// If selection fails, returned object will be null too.
		/// </remarks>
		internal static UIHierarchyItem SelectItem(_DTE vs, object target)
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

        private static bool IsVsSetupProject(UIHierarchyItem item)
        {
            ProjectItem prjItem = item.Object as ProjectItem;
            Project prj;
            if (prjItem != null)
            {
                prj = prjItem.Object as Project;
                if (prj != null)
                {
                    if (prj.Kind == "{54435603-DBB4-11D2-8724-00A0C9A8B90C}")
                    {
                        return true;
                    }
                }
            }
            prj = item.Object as Project;
            if (prj != null)
            {
                if (prj.Kind == "{54435603-DBB4-11D2-8724-00A0C9A8B90C}")
                {
                    return true;
                }
            }
            return false;
        }
        private static UIHierarchyItem LocateInUICollection(UIHierarchyItems items, object target, Dictionary<UIHierarchyItems, bool> expandedItems)
		{
			if (items == null) return null;

			foreach (UIHierarchyItem item in items)
			{
                if (IsVsSetupProject(item))
                {
                    continue;
                }
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

		/// <summary>
		/// Determines whether the project is a web project.
		/// </summary>
		public static bool IsWebProject(Project project)
		{
			Guard.ArgumentNotNull(project, "project");
			return project.Kind == VsWebSite.PrjKind.prjKindVenusProject;
		}

		/// <summary>
		/// Determines if the project item is a web reference.
		/// </summary>
		public static bool IsWebReference(ProjectItem item)
		{
			Guard.ArgumentNotNull(item, "item");

			if (item.ContainingProject.Object is VSProject)
			{
				ProjectItem webrefs = ((VSProject)item.ContainingProject.Object).WebReferencesFolder;
				if (webrefs != null && webrefs.ProjectItems != null)
				{
					foreach (ProjectItem webref in webrefs.ProjectItems)
					{
						if (webref == item)
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
			return false;
		}

		#endregion IsXXX methods

		#region IVsHierarchy related

		/// <summary>
		/// Gets the current selected IVsHierarchy.
		/// </summary>
		/// <returns></returns>
		public static IVsHierarchy GetCurrentSelection(System.IServiceProvider provider)
		{
			Guard.ArgumentNotNull(provider, "provider");
			uint pitemid = 0;
			return GetCurrentSelection(provider, out pitemid);
		}

		internal sealed class __VSITEMID
		{
			// Fields
			public const uint NIL = 0xFFFFFFFF;
			public const uint ROOT = 0xFFFFFFFE;
			public const uint SELECTION = 0xFFFFFFFD;
		}

		/// <summary>
		/// Gets the current selected IVsHierarchy.
		/// </summary>
		public static IVsHierarchy GetCurrentSelection(System.IServiceProvider provider, out uint pitemid)
		{
			Guard.ArgumentNotNull(provider, "provider");

			IVsMonitorSelection pSelection =
				(IVsMonitorSelection)provider.GetService(typeof(SVsShellMonitorSelection));
			IntPtr ptrHierchary = IntPtr.Zero;
			IVsMultiItemSelect ppMIS = null;
			IntPtr ppSC = IntPtr.Zero;
			pSelection.GetCurrentSelection(out ptrHierchary, out pitemid, out ppMIS, out ppSC);
			if (ptrHierchary != IntPtr.Zero)
			{
				return (IVsHierarchy)Marshal.GetObjectForIUnknown(ptrHierchary);
			}
			else // There is not selection, so let's return the solution
			{
				IVsHierarchy solution = (IVsHierarchy)provider.GetService(typeof(SVsSolution));
				pitemid = __VSITEMID.ROOT;
				return solution;
			}
		}

		/// <summary>
		/// Gets the IVsHierarchy from <paramref name="project"/>
		/// </summary>
		public static IVsHierarchy GetVsHierarchy(System.IServiceProvider provider, EnvDTE.Project project)
		{
			Guard.ArgumentNotNull(provider, "provider");
			Guard.ArgumentNotNull(project, "project");

			IVsSolution solution = (IVsSolution)provider.GetService(typeof(SVsSolution));
			Debug.Assert(solution != null, "couldn't get the solution service");
			if (solution != null)
			{
				if (project != null)
				{
					IVsHierarchy vsHierarchy = null;
					solution.GetProjectOfUniqueName(project.UniqueName, out vsHierarchy);
					return vsHierarchy;
				}
			}
			return null;
		}

		#endregion

		#region ILocalRegistry

		/// <summary>
		/// Creates an instance of a COM object declared in the local registry of VisualStudio
		/// </summary>
		public static object CoCreateInstance(System.IServiceProvider provider, Type type, Type interfaceType)
		{
			Guard.ArgumentNotNull(provider, "provider");
			Guard.ArgumentNotNull(type, "type");
			Guard.ArgumentNotNull(interfaceType, "interfaceType");
		
			ILocalRegistry localRegistry = (ILocalRegistry)provider.GetService(typeof(SLocalRegistry));
			if (localRegistry != null)
			{
				Guid interfaceGuid = interfaceType.GUID;
				IntPtr pObject = IntPtr.Zero;
				localRegistry.CreateInstance(type.GUID,
					null,
					ref interfaceGuid,
					(uint)CLSCTX.CLSCTX_INPROC_SERVER,
					out pObject);
				if (pObject != IntPtr.Zero)
				{
					return Marshal.GetObjectForIUnknown(pObject);
				}
			}
			return null;
		}


		#endregion

		/// <summary>
		/// Iterates all the projects in the solution, irrespective of their depth in the 
		/// solution hierarchy of items.
		/// </summary>
		/// <param name="solution">The solution to iterate looking for projects.</param>
		/// <param name="processAndBreak">Delegate to perform processing for each project found. 
		/// The return value signals whether further iteration should continue or not.</param>
		public static void ForEachProject(EnvDTE.Solution solution, Predicate<Project> processAndBreak)
		{
			Guard.ArgumentNotNull(solution, "solution");
			Guard.ArgumentNotNull(processAndBreak, "processAndBreak");
			
			foreach (Project project in solution.Projects)
			{
				bool shouldBreak = false;
				if (!(project.Object is SolutionFolder))
				{
					shouldBreak = processAndBreak(project);
					if (shouldBreak)
					{
						return;
					}
				}
				shouldBreak = ForEachProjectInternal(project.ProjectItems, processAndBreak);
				if (shouldBreak)
				{
					return;
				}
			}
		}

		private static bool ForEachProjectInternal(ProjectItems items, Predicate<Project> processAndBreak)
		{
			bool shouldBreak = false;
			if (items != null)
			{
				foreach (ProjectItem item in items)
				{
					Project project = item.Object as Project;
					if (project != null)
					{
						if (!(project.Object is SolutionFolder))
						{
							shouldBreak = processAndBreak(project);
							if (shouldBreak)
							{
								break;
							}
						}
					}

					shouldBreak = ForEachProjectInternal(item.ProjectItems, processAndBreak);
					if (shouldBreak)
					{
						break;
					}
				}

			}

			return shouldBreak;
		}

		/// <summary>
		/// Replaces parameters in the format used by VS templates ($ArgumentName$) 
		/// using the values in the replacement dictionary provided.
		/// </summary>
		/// <remarks>
		/// The replacement is performed for multiple parameters. However, if a 
		/// parameter doesn't have a value in the dictionary, replacement of 
		/// further parameters will stop and the resulting string returned as-is, 
		/// with the values replaced so far, and the remaining parameters.
		/// </remarks>
		public static string ReplaceParameters(string value, IDictionaryService dictionary)
		{
			if (dictionary != null)
			{
				int begin = value.IndexOf("$");
				int end = value.IndexOf("$", begin + 1);
				if (begin != -1 && end != -1 && begin != end)
				{
					string key = value.Substring(begin + 1, (end - begin) - 1);
					object newvalue = dictionary.GetValue(key);
					if (newvalue != null)
					{
						// Make it recursive to support multiple parameters.
						return ReplaceParameters(value.Replace("$" + key + "$", newvalue.ToString()), dictionary);
					}
				}
			}
			return value;
		}
	}
}
