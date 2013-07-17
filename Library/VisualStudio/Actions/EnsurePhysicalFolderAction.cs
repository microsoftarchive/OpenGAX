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

namespace Microsoft.Practices.RecipeFramework.Library.VisualStudio.Actions
{
    /// <summary>
    /// Ensures Physical Folder existence
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class EnsurePhysicalFolderAction : ConfigurableAction 
    {
        #region Inputs

        /// <summary>
        /// The SolutionFolder to ensure existence
        /// </summary>
        [Input(Required=true)]
        public SolutionFolder SolutionFolder
        {
            get { return solutionFolder; }
            set { solutionFolder = value; }
        } SolutionFolder solutionFolder;

        #endregion
        
        #region Output properties

        /// <summary>
        /// Returns the full path string of the input Folder
        /// </summary>
        [Output]
        public string Path
        {
            get { return path; }
            set { path = value; }
        } string path;

        #endregion


        /// <summary>
        /// Ensures Physical Folder existence
        /// </summary>
        public override void Execute()
        {
            EnvDTE.DTE vs = (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE));
            path = "";

            // TODO: this code should be in DteHelper
            // In some case vs.Solution.FullName == ""
            if (vs.Solution.FullName != "")
            {
                path = DteHelper.GetPathFull(vs, DteHelper.BuildPath(solutionFolder));
            }
            else
            {
                Property property = vs.Solution.Properties.Item("Path");
                path = System.IO.Path.GetDirectoryName(property.Value.ToString());

                path = System.IO.Path.Combine(path, DteHelper.BuildPath(solutionFolder));
            }

            if (!string.IsNullOrEmpty(path))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            else
            {
                // throw an exception.
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
