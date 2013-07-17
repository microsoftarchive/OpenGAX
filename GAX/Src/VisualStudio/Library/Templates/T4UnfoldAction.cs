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
using System.Collections.Generic;
using System.Text;
using System.IO;
using EnvDTE;
using Microsoft.Practices.RecipeFramework;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates
{
	/// <summary>
	/// Unfolds a T4 template using the content of a <see cref="ProjectItem"/> as template
	/// The result is placed in the content of the same <see cref="ProjectItem"/>
	/// </summary>
	public sealed class T4UnfoldAction : T4Action
	{
		#region Input Properties

		/// <summary>
		/// The project item that contains the T# template in its contents
		/// </summary>
		[Input]
		public ProjectItem ProjectItem
		{
			get { return projectItem; }
			set { projectItem = value; }
		} ProjectItem projectItem;

		/// <summary>
		/// A collection of project items that contains the T# template in its contents
		/// </summary>
		[Input]
		public ICollection<ProjectItem> ProjectItemCollection
		{
			get { return projectItems; }
			set { projectItems = value; }
		} ICollection<ProjectItem> projectItems;

		/// <summary>
		/// The project that contains one or more project items with T4 templates in its contents
		/// </summary>
		[Input]
		public Project Project
		{
			get { return project; }
			set { project = value; }
		} Project project;

		#endregion

		#region Output Properties

		#endregion

		#region Private Implementation

		private bool IsTemplateFile(string filePath, ref string content)
		{
			using (StreamReader fileStream = new StreamReader(filePath))
			{
				string firstLine = fileStream.ReadLine();
				if (firstLine.StartsWith("<#@ Template Language=", StringComparison.InvariantCultureIgnoreCase))
				{
					StringBuilder sb = new StringBuilder();
					sb.AppendLine(firstLine);
					sb.Append(fileStream.ReadToEnd());
					content = sb.ToString();
					return true;
				}
			}
			content = string.Empty;
			return false;
		}

		private void ProcessFile(string filePath, ProjectItem prItem)
		{
			string templateString = string.Empty;
			if (IsTemplateFile(filePath, ref templateString))
			{
				string renderedText = this.Render(templateString, filePath);
                //using (SetProjectItemContentAction setContentAction = new SetProjectItemContentAction())
                //{
                //    this.Container.Add(setContentAction);
                //    setContentAction.ProjectItem = prItem;
                //    setContentAction.Content = renderedText;
                //    setContentAction.Execute();
                //}
			}
		}

		private void ProcessProjectItem(ProjectItem prItem)
		{
			// First, process the childs elements, e.g: Form1.Designer.cs
			ProjectItems childItems = prItem.ProjectItems;
			if (childItems != null)
			{
				foreach (ProjectItem subItem in childItems)
				{
					ProcessProjectItem(subItem);
				}
			}
			// Then the main file, e.g: Form1.cs
			// This must be done this way so that we dont interfere with the VS designer
			for (short i = 0; i < prItem.FileCount; i++)
			{
				string fileName = prItem.get_FileNames(i);
				if (System.IO.File.Exists(fileName))
				{
					ProcessFile(fileName, prItem);
				}
			}
		}

		#endregion

		#region IAction Members

		/// <summary>
		/// Performs the unfolding with the given parameters
		/// </summary>
		public override void Execute()
		{
			DTE dte = (DTE)GetService(typeof(DTE));
			if (project != null)
			{
				foreach (ProjectItem prItem in project.ProjectItems)
				{
					ProcessProjectItem(prItem);
				}
			}
			else if (projectItem != null)
			{
				ProcessProjectItem(projectItem);
			}
			else if (projectItems != null)
			{
				foreach (ProjectItem generatedItem in this.projectItems)
				{
					ProcessProjectItem(generatedItem);
				}
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		/// <summary>
		/// Undoes the unfolding, not implemented
		/// </summary>
		public override void Undo()
		{
			// No undo implemented.
		}

		#endregion
	}
}
