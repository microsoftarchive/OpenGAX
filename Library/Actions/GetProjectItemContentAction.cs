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
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Shell.Design.Serialization;
using EnvDTE;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions
{
    /// <summary>
    /// Sets the context of an project item, even if the project item is opened by Visual Studio
    /// </summary>
    [ServiceDependency(typeof(ILocalRegistry))]
    public sealed class GetProjectItemContentAction : Action
    {
        #region Input Properties

        /// <summary>
        /// The project item that is receiving the new content
        /// </summary>
        [Input(Required=true)]
        public ProjectItem ProjectItem
        {
            get { return projectItem; }
            set { projectItem = value; }
        } ProjectItem projectItem;

        #endregion

        #region Output Properties

        /// <summary>
        /// The content of the project item
        /// </summary>
        [Output]
        public string Content
        {
            get { return content; }
            set { content = value; }
        } string content;

        #endregion

        #region IAction Members

        /// <summary>
        /// Gets the content of the project item
        /// </summary>
        public override void Execute()
        {
            string fileName = this.ProjectItem.get_FileNames(0);
            using (StreamReader fileStream = new StreamReader(fileName))
            {
                this.Content = fileStream.ReadToEnd();
                return;
            }
        }

        /// <summary>
        /// Undoes the get
        /// </summary>
        public override void Undo()
        {
            this.Content = string.Empty;
        }

        #endregion
    }
}
