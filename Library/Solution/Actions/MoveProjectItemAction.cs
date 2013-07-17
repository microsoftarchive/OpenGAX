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
using EnvDTE;
using System.IO;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Solution.Actions
{
    /// <summary>
    /// Moves a ProjectItem from one Projecto to another
    /// </summary>
	public sealed class MoveProjectItemAction: ConfigurableAction
    {
        #region Input properties

        /// <summary>
        /// The <see cref="ProjectItem"/> to move
        /// </summary>
        [Input(Required=true)]
        public ProjectItem ProjectItem
        {
            get { return projectItem; }
            set { projectItem = value; }
        } ProjectItem projectItem;

        /// <summary>
        /// The destination project for the project item
        /// </summary>
        [Input(Required=true)]
        [Output]
        public ProjectItems ProjectItems
        {
            get { return projectItems; }
            set { projectItems = value; }
        } ProjectItems projectItems;
	
        #endregion

        #region Overrides

        /// <summary>
        /// Moves a <see cref="ProjectItem"/>
        /// </summary>
        public override void Execute()
        {
            FileInfo fi=new FileInfo(this.ProjectItem.get_FileNames(0));
            this.ProjectItems.AddFromFileCopy(fi.FullName);
            if (this.ProjectItem.Document != null)
            {
                this.ProjectItem.Document.Close(vsSaveChanges.vsSaveChangesNo);
            }
            this.ProjectItem.Delete();
            this.ProjectItem = null;
        }

        /// <summary>
        /// Restores the location of the <see cref="ProjectItem"/>
        /// Not implemented
        /// </summary>
        public override void Undo()
        {
        }

        #endregion
    }
}
