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
using System.Text;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using EnvDTE80;
using System.Collections.Generic;

namespace Microsoft.Practices.RecipeFramework.Library.VisualStudio.Actions
{
    /// <summary>
    /// The action adds a file to the project item or solution item passed 
    /// to the action in the Content input property. 
    /// The other input properties of the action are 
    /// (a) SourceFileName - provides the name of the file 
    /// (b) SourceFilePath - provides the path name of the file
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddSolutionItemFromDirectoryAction : ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// Name of the source file path
        /// </summary>
        [Input(Required=false)]
        public string SourceFilePath
        {
            get { return sourceFilePath; }
            set { sourceFilePath = value; }
        } private string sourceFilePath;

        /// <summary>
        /// The TargetProjectItems object where add the file
        /// </summary>
        [Input(Required=false)]
        public ProjectItems TargetProjectItems
        {
            get { return targetProjectItems; }
            set { targetProjectItems = value; }
        } private ProjectItems targetProjectItems;

        private string searchPattern = "*.*";
        /// <summary>
        /// The searchPattern used to filter the files from the directory.
        /// </summary>
        [Input(Required=false)]
        public string SearchPattern
        {
            get { return searchPattern; }
            set { searchPattern = value; }
        }

        private bool open = true;
        /// <summary>
        /// A flag to indicate if the newly created item should be shown
        /// in a window.
        /// </summary>
        [Input(Required=false)]
        public bool Open
        {
            get { return open; }
            set { open = value; }
        }
	
        #endregion Input Properties

        #region Output Properties

        private ProjectItem[] projectItems;
        /// <summary>
        /// Returns the ProjectItem collection added.
        /// </summary>
        [Output]
        public ProjectItem[] ProjectItems
        {
            get { return projectItems; }
            set { projectItems = value; }
        } 

        #endregion Output Properties


        /// <summary>
        /// The method that creates a new item from the intput string.
        /// </summary>
        public override void Execute()
		{
            // This method is not implemented.
            //projectItem = targetProjectItems.AddFromDirectory(sourceFilePath);

            List<ProjectItem> items = new List<ProjectItem>();
            string[] fileNames = Directory.GetFiles(sourceFilePath, searchPattern);
            foreach (string fileName in fileNames)
            {
                items.Add(targetProjectItems.AddFromFile(fileName));
            }
            projectItems = items.ToArray();
		}

        /// <summary>
        /// Deletes the created items
        /// </summary>
        public override void Undo()
        {
            foreach (ProjectItem projectItem in projectItems)
            {
                if (projectItem != null)
                {
                    projectItem.Delete();
                }
            }
        }
    }
}
