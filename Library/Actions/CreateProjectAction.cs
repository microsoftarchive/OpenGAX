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

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions
{
    /// <summary>
    /// The action creates a new project from a project template and makes the
    /// new project reference available to subsequent actions. The action has 
    /// three input properties (a) ProjectName - name of new project (b) Template -
    /// path to the project template, and (c) ProjectFolder - name of the project
    /// folder. 
    /// </summary>
    public class CreateProjectAction : Action
    {
        #region Input Properties

        /// <summary>
        /// Project name that will have the new project
        /// </summary>
        [Input]
        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        } string projectName;

        /// <summary>
        /// Template used to create the new project
        /// </summary>
        [Input]
        public string Template
        {
            get { return template; }
            set { template = value; }
        } string template;

        /// <summary>
        /// Project folder to add the new project
        /// </summary>
        [Input]
        public string ProjectFolder
        {
            get { return projectFolder; }
            set { projectFolder = value; }
        } string projectFolder;

        #endregion

        #region Output properties

        /// <summary>
        /// Output property that will have the new created project
        /// </summary>
        [Output]
        public EnvDTE.Project Project
        {
            get { return project; }
            set { project = value; }
        } EnvDTE.Project project;

        #endregion

        #region IAction Members

        /// <summary>
        /// Creates the new project and adds it to the solution
        /// </summary>
        public override void Execute()
        {
            DTE dte = GetService<DTE>(true);
            Project = dte.Solution.AddFromTemplate(Template, ProjectFolder, ProjectName, false);
            if (!Directory.Exists(ProjectFolder))
            {
                Directory.CreateDirectory(ProjectFolder);
            }
        }

        /// <summary>
        /// Deletes the new project created
        /// </summary>
        public override void Undo()
        {
            if (Project != null)
            {
                Project.Delete();
                Project = null;
            }
        }

        #endregion
    }
}
