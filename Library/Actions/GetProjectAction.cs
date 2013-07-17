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
using EnvDTE;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions
{
    /// <summary>
    /// The action returns a reference to a solution project.The reference
    /// is returned in the output property called Project. The action has one input 
    /// property ProjecName - name of the project to find in the solution. 
    /// If the project with the given name is not found, the recipe returns Null
    /// reference. 
    /// </summary>
    public sealed class GetProjectAction: ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// Name of the project to find in the solution
        /// </summary>
        [Input(Required=true)]
        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        } string projectName;

        #endregion

        #region Output properties

        /// <summary>
        /// The object project element of the solution
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
        /// Sets the property output Project to the appropiate dte element
        /// </summary>
        public override void Execute()
        {
			DTE dte = GetService<DTE>(true);
            this.Project = DteHelper.FindProjectByName(dte, ProjectName);
        }

        /// <summary>
        /// Undoes the set
        /// </summary>
        public override void Undo()
        {
        }

        #endregion

    }
}
