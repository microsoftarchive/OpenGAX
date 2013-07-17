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
    public class AddSolutionItemFromFileAction : ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// Name of the source file
        /// </summary>
        [Input(Required=true)]
        public string SourceFileName
        {
            get { return sourceFileName; }
            set { sourceFileName = value; }
        } private string sourceFileName;

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
        /// The TargetProject object where add the file
        /// </summary>
        [Input(Required=true)]
        public Project TargetProject
        {
            get { return targetProject; }
            set { targetProject = value; }
        } private Project targetProject;

        /// <summary>
        /// A flag to indicate if the newly created item should be shown
        /// in a window.
        /// </summary>
        [Input(Required=false)]
        public bool Open
        {
            get { return open; }
            set { open = value; }
        } private bool open = true;
	

        #endregion Input Properties

        #region Output Properties

        /// <summary>
        /// A property that can be used to pass the creted item to
        /// a following action.
        /// </summary>
        [Output]
        public ProjectItem ProjectItem
        {
            get { return projectItem; }
            set { projectItem = value; }
        } private ProjectItem projectItem;

        #endregion Output Properties


        /// <summary>
        /// The method that creates a new item from the intput string.
        /// </summary>
        public override void Execute()
		{
            string fileName = Path.Combine(sourceFilePath, sourceFileName);

            projectItem = targetProject.ProjectItems.AddFromFile(fileName);
		}

        /// <summary>
        /// Undoes the creation of the item, then deletes the item
        /// </summary>
        public override void Undo()
        {
            if (projectItem != null)
            {
                projectItem.Delete();
            }
        }
    }
}
