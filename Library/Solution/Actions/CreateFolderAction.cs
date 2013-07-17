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
using Microsoft.Practices.RecipeFramework;
using EnvDTE;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Solution.Actions
{
    /// <summary>
    /// Action to create a Folder in a <see cref="Project"/>
    /// </summary>
	public sealed class CreateFolderAction: ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// The <see cref="ProjectItems"/> where the folder will be created
        /// </summary>
        [Input(Required=true)]
        public ProjectItems ProjectItems
        {
            get { return projectItems;  }
            set { projectItems = value; }
        } ProjectItems projectItems;

        /// <summary>
        /// The name of the newly created folder
        /// </summary>
        [Input(Required=true)]
        public string FolderName
        {
            get { return folderName; }
            set { folderName = value; }
        } string folderName;
	
        #endregion

        #region Output properties

        /// <summary>
        /// The created folder
        /// </summary>
        [Output]
        public ProjectItems Folder
        {
            get { return folder;  }
            set { folder = value;  } 
        } ProjectItems folder;

        #endregion

        #region Overrides

        /// <summary>
        /// Creates a new folder under an existing project
        /// </summary>
        public override void Execute()
        {
            ProjectItem prItem = 
                this.ProjectItems.AddFolder(
                    this.FolderName, "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}");
            this.Folder = prItem.ProjectItems;
        }

        /// <summary>
        /// Deletes the created folder
        /// </summary>
        public override void Undo()
        {
            ((ProjectItem)this.Folder.Parent).Delete();
            this.Folder = null;
        }

        #endregion
    }
}
