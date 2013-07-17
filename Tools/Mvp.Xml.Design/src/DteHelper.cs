using System;
using System.IO;
using EnvDTE;
using VSLangProj;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel.Design;
using EnvDTE80;

namespace Mvp.Xml.Design
{
	/// <summary>
	/// Provides utility methods for working with the DTE.
	/// </summary>
	public sealed class DteHelper
	{
        #region BuildPath

        /// <summary>
        /// Builds a path to the element, detecting automatically the 
        /// type of element and building an appropriate path to it.
        /// </summary>
        public static string BuildPath(object toElement)
        {
            if (toElement == null)
            {
                throw new ArgumentNullException("toElement");
            }
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
            if (toSelectedItem == null)
            {
                throw new ArgumentNullException("toSelectedItem");
            }
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
        /// <param name="toProject"></param>
        /// <returns></returns>
		public static string BuildPath(Project toProject)
		{
            if (toProject == null)
            {
                throw new ArgumentNullException("toProject");
            }
            string path = "";

			// Solution folders are exposed with the same kind as solution items. 
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
            if (toItem == null)
            {
                throw new ArgumentNullException("toItem");
            }
            string path = "";

			if (toItem.ContainingProject != null)
			{
                if (!BuildPathFromCollection(toItem.ContainingProject.ProjectItems, toItem, ref path))
                {
                    return "";
                }
                else
                {
                    path = BuildPath(toItem.ContainingProject) + path;
                }
			}
			else
			{
				path = toItem.Name;
			}

			return path;
		}

		private static bool BuildPathFromCollection(ProjectItems items, ProjectItem target, ref string path)
		{
			if (items == null) return false;

			foreach (ProjectItem item in items)
			{
				if (item == target)
				{
					path = path + Path.DirectorySeparatorChar + target.Name;
					return true;
				}
				else
				{
					string tmp = path + Path.DirectorySeparatorChar + item.Name;
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
		/// Finds a project in the solution, given its output assembly name.
		/// </summary>
		/// <returns>A <see cref="Project"/> reference or <see langword="null" /> if 
		/// it doesn't exist. Project can be C# or VB.</returns>
		public static Project FindProjectByAssemblyName(_DTE vs, string name)
		{
            if (vs == null)
            {
                throw new ArgumentNullException("vs");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            try
            {
                foreach (Project p in (Projects)vs.GetObject("CSharpProjects"))
                {
                    if (p.Properties.Item("AssemblyName").Value.ToString() == name) return p;
                }
            }
            catch { } // Late-bound collection of "CSharpProject" throws if C# is not installed.
            try
            {
                foreach (Project p in (Projects)vs.GetObject("VBProjects"))
                {
                    if (p.Properties.Item("AssemblyName").Value.ToString() == name) return p;
                }
            }
            catch { } // Late-bound collection of "VBProjects" throws if VB is not installed.

			return null;
		}

		/// <summary>
		/// Finds a project in the solution, given its name.
		/// </summary>
		/// <returns>A <see cref="Project"/> reference or <see langword="null" /> if 
		/// it doesn't exist. Project can be C# or VB.</returns>
		public static Project FindProjectByName(_DTE vs, string name)
		{
            if (vs == null)
            {
                throw new ArgumentNullException("vs");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
			// Try the quick ones first.
			try
			{
				foreach (Project p in (Projects)vs.GetObject("CSharpProjects"))
				{
					if (p.Name == name) return p;
				}
			}
            catch { } // Late-bound collection of "CSharpProject" throws if C# is not installed.

			try
			{
				foreach (Project p in (Projects)vs.GetObject("VBProjects"))
				{
					if (p.Name == name) return p;
				}
			}
            catch { } // Late-bound collection of "VBProjects" throws if VB is not installed.

			// We've no option but to iterate everything now.
			foreach (Project p in vs.Solution.Projects)
			{
				if (p.Name == name)
				{
					return p;
				}
				else if (p.ProjectItems != null)
				{
					Project inner = FindProjectByName(p.ProjectItems, name);
					if (inner != null)
					{
						return inner;
					}
				}
			}

			return null;
		}

		private static Project FindProjectByName(ProjectItems items, string name)
		{
			foreach (ProjectItem item in items)
			{
				if (item.Object is Project && ((Project)item.Object).Name == name)
				{
					return item.Object as Project;
				}
				else if (item.ProjectItems != null)
				{
					Project p = FindProjectByName(item.ProjectItems, name);
					if (p != null)
					{
						return p;
					}
				}
			}

			return null;
		}

        /// <summary>
        /// Finds a solution folder in the solution hierarchy, given its 
        /// folder-like location path. 
        /// </summary>
        /// <returns>The solution folder or <see langword="null" /> if 
        /// it doesn't exist.</returns>
        /// <remarks>
        /// Note that this method performs the same work as <see cref="FindProjectByPath"/>, 
        /// but only returns an instance if the <see cref="Project.Object"/> is actually 
        /// an <see cref="EnvDTE80.SolutionFolder"/>. This is because solution folders 
        /// are represented as <see cref="Project"/> elements in Visual Studio.
        /// </remarks>
        public static EnvDTE80.SolutionFolder FindSolutionFolderByPath(Solution root, string path)
        {
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
        /// the returned project <see cref="Project.Object"/> property to 
        /// <see cref="EnvDTE80.SolutionFolder"/>.
        /// </summary>
        /// <returns>The project or <see langword="null" /> if 
        /// it doesn't exist.</returns>
        public static Project FindProjectByPath(Solution root, string path)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            string[] allpaths = path.Split(System.IO.Path.DirectorySeparatorChar,
                System.IO.Path.AltDirectorySeparatorChar);

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
        /// but this method checks for the <see cref="ProjectItem.Object"/> property 
        /// to discard matches in this case. If the object to retrieve is a 
        /// project (or solution folder, which is represented as a project too, 
        /// and the folder is retrieved through the <see cref="Project.Object"/> property), 
        /// the <see cref="FindProjectByPath"/> method must be used.
        /// </remarks>
		public static ProjectItem FindItemByPath(Solution root, string path)
		{
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
			string[] allpaths = path.Split(System.IO.Path.DirectorySeparatorChar, 
				System.IO.Path.AltDirectorySeparatorChar);
            
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

		private static ProjectItem FindInCollectionRecursive(ProjectItems collection, string[] paths, int index)
		{
			foreach (ProjectItem item in collection)
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

		#endregion Find methods

		#region Get methods

        /// <summary>
        /// Retrieves the currently selected target in the solution explorer.
        /// </summary>
        /// <remarks>
        /// If there's only one item selected, then the corresponding Solution, Project 
        /// or ProjectItem element is returned. Otherwise, the <see cref="EnvDTE.SelectedItems"/> 
        /// property is returned.
        /// </remarks>
        /// <exception cref="InvalidOperationException">There's no current selection in the solution explorer.</exception>
        /// <returns>The current selection.</returns>
        public static object GetTarget(_DTE vs)
        {
            if (vs.SelectedItems != null && vs.SelectedItems.Count > 0)
            {
                if (vs.SelectedItems.Count == 1)
                {
                    IEnumerator enumerator = vs.SelectedItems.GetEnumerator();
                    enumerator.MoveNext();
                    EnvDTE.SelectedItem item = (EnvDTE.SelectedItem)enumerator.Current;
                    // Determine target to load.
                    if (item.Project != null)
                    {
                        return item.Project;
                    }
                    else if (item.ProjectItem != null)
                    {
                        return item.ProjectItem;
                    }
                    else if ( vs.Solution.Properties.Item("Name").Value.Equals(item.Name) )
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
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }
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
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }
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
            if (vs == null)
            {
                throw new ArgumentNullException("vs");
            }
			foreach (object obj in (object[]) vs.ActiveSolutionProjects)
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
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return GetFilePathRelative(item.DTE, item.get_FileNames(1));
		}

		/// <summary>
		/// Turns the file name received into a path relative to the containing solution.
		/// </summary>
		public static string GetFilePathRelative(_DTE vs, string file)
		{
            if (vs == null)
            {
                throw new ArgumentNullException("vs");
            }
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
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
            if (vs == null)
            {
                throw new ArgumentNullException("vs");
            }
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
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
            UIHierarchy hier = win.Object as UIHierarchy;
            UIHierarchyItem sol = hier.UIHierarchyItems.Item(1);
            if (sol == null)
            {
                // No solution is opened.
                return false;
            }
            sol.Select(vsUISelectionType.vsUISelectionTypeSelect);
            return true;
        }

        #region Removed feature - not working quite good

        /*
 
        /// <summary>
		/// Selects a project in the solution explorer.
		/// </summary>
		public static bool SelectProject(Project project)
		{
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }
			// Select the parent folder to add the project to it.
			Window win = project.DTE.Windows.Item(Constants.vsWindowKindSolutionExplorer);
            if (win == null)
            {
                // Can't select as there's no solution explorer open.
				throw new InvalidOperationException(Properties.Resources.DteHelper_NoSolutionExplorer);
            }
			win.Activate();
			win.SetFocus();
			UIHierarchy hier = win.Object as UIHierarchy;
			UIHierarchyItem sol = hier.UIHierarchyItems.Item(1);
            if (sol == null)
            {
                // No solution is opened.
                return false;
            }
			sol.Select(vsUISelectionType.vsUISelectionTypeSelect);

			// Remove project file name from path.
			string name = Path.GetDirectoryName(project.UniqueName);

			// Web projects can't be located through UniqueName.
			if (IsWebProject(project))
			{
				// Locate by folder relative to solution one. 
				// WARNING: this will not work if project is NOT inside solution!
				name = Path.GetDirectoryName(project.Properties.Item("FullPath").Value.ToString());
				string slnpath = Path.GetDirectoryName(project.DTE.Solution.FullName);
				name = name.Substring(name.IndexOf(slnpath) + slnpath.Length + 1);
			}

			// Perform selection.
			UIHierarchyItem item = null;
			try
			{
				item = hier.GetItem(Path.Combine(sol.Name, name));
			}
			catch (ArgumentException)
			{
				// Retry selection by name (much slower!)
				item = FindProjectByName(project.Name, hier.UIHierarchyItems);
			}
            if (item != null)
            {
                item.UIHierarchyItems.Expanded = true;
                item.Select(vsUISelectionType.vsUISelectionTypeSelect);
                return true;
            }
            else
            {
                return false;
            }
		}

		private static UIHierarchyItem FindProjectByName(string name, UIHierarchyItems items)
		{
			foreach (UIHierarchyItem item in items)
			{
				if (item.Name == name)
				{
					ProjectItem pi = item.Object as ProjectItem;
					// Check the project name or the subproject name (for ETP).
					if (pi.ContainingProject.Name == name || 
						(pi.SubProject != null && pi.SubProject.Name == name))
						return item;
				}
				
				if (item.UIHierarchyItems != null)
				{
					UIHierarchyItem uii = FindProjectByName(name, item.UIHierarchyItems);
					if (uii != null) return uii;
				}
			}

			return null;
		}

        */

        #endregion Removed feature - not working quite good

		/// <summary>
		/// Selects a solution explorer item, based on 
		/// its relative path with regards to the solution.
		/// </summary>
        /// <remarks>
        /// If selection fails, returned object will be null too.
        /// </remarks>
		public static UIHierarchyItem SelectItem(_DTE vs, string path)
		{
            if (vs == null)
            {
                throw new ArgumentNullException("vs");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
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
			UIHierarchy hier = win.Object as UIHierarchy;
			UIHierarchyItem sol = hier.UIHierarchyItems.Item(1);
            if (sol == null)
            {
                // No solution is opened.
				throw new InvalidOperationException(Properties.Resources.DteHelper_NoSolutionExplorer);
			}
            sol.Select(vsUISelectionType.vsUISelectionTypeSelect);
			// Perform selection.
            UIHierarchyItem item = null;
            try
            {
                string slnpath = Path.Combine(sol.Name, path);
                item = hier.GetItem(slnpath);
            }
            catch (ArgumentException)
            {
                // Retry selection by name (slower)
                item = FindHierarchyItemByPath(sol.UIHierarchyItems,
                    path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), 0);
            }

			if (item != null)
			{
                item.UIHierarchyItems.Expanded = true;
                item.Select(vsUISelectionType.vsUISelectionTypeSelect);
            }

            return item;
		}

		#region Find manually

		/// <summary>
		/// UIHierarchy.GetItem does not always work :S, so I do it by hand here.
		/// </summary>
		private static UIHierarchyItem FindHierarchyItemByPath(UIHierarchyItems items, string[] paths, int index)
		{
            // Expand everything as we go.
            items.Expanded = true;
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
                        return FindHierarchyItemByPath(item.UIHierarchyItems, paths, ++index);
                    }
				}
			}

			// Item wasn't found.
			return null;
		}

		#endregion Find manually

		#endregion Select methods

		#region IsXXX methods

		/// <summary>
		/// Determines whether the project is a web project.
		/// </summary>
		public static bool IsWebProject(Project project)
		{
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }
            try
            {
                return project.Properties.Item("WebServerVersion") != null &&
                    project.Properties.Item("WebServerVersion").Value != null &&
                    !string.IsNullOrEmpty(project.Properties.Item("WebServerVersion").Value.ToString());
            }
            catch
            {
                return false;
            }
		}

        /// <summary>
        /// Determines if the project item is a web reference.
        /// </summary>
        public static bool IsWebReference(ProjectItem item)
        {
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
    }
}
