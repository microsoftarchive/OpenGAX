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
using System.IO;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions
{
    /// <summary>
    /// The action renames a solution project. The action has two input properties 
    /// (a) ProjectPath - solution path to the renamed project and (b) NewProjectName
    /// - new name of the project. The action has one output property Project, which 
    /// can be used by subsequent action to obtain reference to th erenamed project. 
    /// </summary>
    public class RenameProjectAction : Action
    {
        string oldName;
        string oldAssembly;

        #region Input Properties

        /// <summary>
        /// Solution path to the project to rename
        /// </summary>
        [Input(Required=true)]
        public string ProjectPath
        {
            get { return projectPath; }
            set { projectPath = value; }
        } string projectPath;

        /// <summary>
        /// The new project name
        /// </summary>
        [Input(Required=true)]
        public string NewProjectName
        {
            get { return newProjectName; }
            set { newProjectName = value; }
        } string newProjectName;

        #endregion

        #region Output properties

        /// <summary>
        /// The project object that was renamed
        /// </summary>
        [Output]
        public Project Project
        {
            get { return project; }
            set { project = value; }
        } Project project;

        #endregion

        #region IAction Members

        /// <summary>
        /// Renames the project with the provided name
        /// </summary>
        public override void Execute()
        {
			DTE dte = GetService<DTE>(true);
            this.project = DteHelper.FindProjectByPath(dte.Solution, projectPath);
            if (this.project == null)
            {
                throw new ArgumentException(String.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.RenameProjectAction_CantFindProject,
                    projectPath));
            }
            else
            {
                this.oldName = this.project.Name;
                this.oldAssembly = this.project.Properties.Item("AssemblyName").Value.ToString();
                this.project.Name = newProjectName;
                this.project.Properties.Item("AssemblyName").Value = newProjectName;
            }
        }

        /// <summary>
        /// Undoes the renaming
        /// </summary>
        public override void Undo()
        {
            if (this.project != null)
            {
                this.project.Name = this.oldName;
                project.Properties.Item("AssemblyName").Value = this.oldAssembly;
            }
        }

        #endregion
    }
}

