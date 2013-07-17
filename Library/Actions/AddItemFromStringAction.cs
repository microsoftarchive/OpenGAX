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

namespace Microsoft.Practices.RecipeFramework.Library.Actions
{
    /// <summary>
    /// The action creates a project item from a string passed to the action
    /// in the Content input property. The other two input properties of the
    /// action are (a) targetFileName - provides the name of the item file 
    /// and (b) Project - identifies the project where the item is created. 
    /// The action is designed to follow the T3Action. 
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddItemFromStringAction : ConfigurableAction
    {
        #region Input Properties

        private string content;
        /// <summary>
        /// The string with the content of the item. In most cases it is a class
        /// generated using the T4 engine.
        /// </summary>
        [Input(Required=true)]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private string targetFileName;
        /// <summary>
        /// Name of the file where the item is to be stored.
        /// </summary>
        [Input(Required=true)]
        public string TargetFileName
        {
            get { return targetFileName; }
            set { targetFileName = value; }
        }

        private Project project;
        /// <summary>
        /// Project where the item it to be inserted.
        /// </summary>
        [Input(Required=true)]
        public Project Project
        {
            get { return project; }
            set { project = value; }
        }

        private bool open = true;
        /// <summary>
        /// A flag to indicate if the newly created item should be shown
        /// in a window.
        /// </summary>
        [Input]
        public bool Open
        {
            get { return open; }
            set { open = value; }
        }
	

        #endregion Input Properties

        #region Output Properties

        private EnvDTE.ProjectItem projectItem;
        /// <summary>
        /// A property that can be used to pass the creted item to
        /// a following action.
        /// </summary>
        [Output]
        public EnvDTE.ProjectItem ProjectItem
        {
            get { return projectItem; }
            set { projectItem = value; }
        }

        #endregion Output Properties
        /// <summary>
        /// The method that creates a new item from the intput string.
        /// </summary>
        public override void Execute()
        {
            DTE vs = GetService<DTE>(true);
            string tempfile = Path.GetTempFileName();
            using (StreamWriter sw = new StreamWriter(tempfile, false))
            {
                sw.WriteLine(content);
            }

            projectItem = project.ProjectItems.AddFromTemplate(tempfile, targetFileName);
            if (open)
            {
                Window wnd = projectItem.Open(Constants.vsViewKindPrimary);
                wnd.Visible = true;
                wnd.Activate();
            }
            File.Delete(tempfile);
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
