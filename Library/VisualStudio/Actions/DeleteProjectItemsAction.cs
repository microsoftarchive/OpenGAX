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
using System.Collections.Generic;
using System.Text;
using EnvDTE80;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using System.IO;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using VsWebSite;

namespace Microsoft.Practices.RecipeFramework.Library.VisualStudio.Actions
{
    /// <summary>
    /// Delete ProjectItems content filtering by the input kind guid
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class DeleteProjectItemsAction : ConfigurableAction 
    {
        #region Input Properties

        private ProjectItems projectItems;
        /// <summary>
        /// The SolutionFolder to ensure existence
        /// </summary>
        [Input(Required=false)]
        public ProjectItems ProjectItems
        {
            get { return projectItems; }
            set { projectItems = value; }
        }

        private string kind;
        /// <summary>
        /// The kind to apply in the condition.
        /// </summary>
        [Input(Required=false)]
        public string Kind
        {
            get { return kind; }
            set { kind = value; }
        }

        #endregion
        
        /// <summary>
        /// Delete ProjectItems content filtering by the input kind guid
        /// </summary>
        public override void Execute()
        {
            foreach (ProjectItem projectItem in projectItems)
            {
                if ((string.IsNullOrEmpty(kind) || (projectItem.Kind == kind)))
                {
                    DTE vs = (DTE)GetService(typeof(DTE));
                    string itemPath = DteHelper.BuildPath(projectItem);
                    itemPath = DteHelper.GetPathFull(vs, itemPath);

                    projectItem.Delete();

                    File.Delete(itemPath);
                }
            }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override void Undo()
        {
            // No undo supported as no Remove method exists on the VSProject.References property.
        }
    }
}
