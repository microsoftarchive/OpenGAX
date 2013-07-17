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
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using VSLangProj;
using Microsoft.Practices.RecipeFramework.VisualStudio;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Solution.Actions
{
    /// <summary>
    /// Finds a project item given its name and the parent project
    /// </summary>
    public sealed class GetProjectItemAction : ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// The parent project that own the project item
        /// </summary>
        [Input(Required=true)]
        public Project Project
        {
            get { return project; }
            set { project = value; }
        } Project project;

        /// <summary>
        /// The name of the item to search for
        /// </summary>
        [Input(Required=true)]
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        } string itemName;

        #endregion

        #region Output Properties

        /// <summary>
        /// The found project item
        /// </summary>
        [Output]
        public ProjectItem ProjectItem
        {
            get { return projectItem; }
            set { projectItem = value; }
        } ProjectItem projectItem;

        #endregion

        #region Action members

        /// <summary>
        /// <see cref="IAction.Execute"/>
        /// </summary>
        public override void Execute()
        {
            this.ProjectItem=
                DteHelper.FindItemByName(this.Project.ProjectItems, this.ItemName, true);
        }

        /// <summary>
        /// <see cref="IAction.Undo"/>
        /// </summary>
        public override void Undo()
        {
            this.ProjectItem = null;
        }

        #endregion
    }
}
