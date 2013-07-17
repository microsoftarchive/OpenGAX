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
using System.Diagnostics;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions.Vsd
{
    /// <summary>
    /// Action that excludes all dependencies from the setup project specified
    /// </summary>
    public sealed class ExcludeAllDependenciesAction: Action
    {
        #region Input Properties

        /// <summary>
        /// Setup project where the dependencies are going to be deleted
        /// </summary>
        [Input(Required=true)]
        public EnvDTE.Project SetupProject
        {
            get { return setupProject; }
            set { setupProject = value; }
        } EnvDTE.Project setupProject;

        #endregion

        #region Private Implementation

        /// <summary>
        /// Performs the exclusion / or inclusion of the dependencies
        /// </summary>
        /// <param name="exclude"></param>
        private void Execute(bool exclude)
        {
            IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
            IVsdCollectionSubset plugins = deployable.GetPlugIns();
            IVsdFilePlugIn filePlugin = plugins.Item("File") as IVsdFilePlugIn;
            if (filePlugin != null)
            {
                foreach (object fileObject in filePlugin.Items)
                {
                    if (fileObject is IVsdFile)
                    {
                        if (((IVsdFile)fileObject).IsDependency)
                        {
                            ((IVsdFile)fileObject).Exclude = exclude;
                        }
                    }
                    else if (fileObject is IVsdAssembly)
                    {
                        if (((IVsdAssembly)fileObject).IsDependency)
                        {
                            ((IVsdAssembly)fileObject).Exclude = exclude;
                        }
                    }
                }
            }
            IVsdMergeModulePlugIn modulePlugin = plugins.Item("MergeModule") as IVsdMergeModulePlugIn;
            if (modulePlugin != null)
            {
                foreach (object moduleObject in modulePlugin.Items)
                {
                    if (moduleObject is IVsdModule)
                    {
                        if (((IVsdModule)moduleObject).IsDependency)
                        {
                            ((IVsdModule)moduleObject).Exclude = exclude;
                        }
                    }
                }
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Executes the exclusion of the dependencies in the setup project
        /// </summary>
        public override void Execute()
        {
            Execute(true);
        }

        /// <summary>
        /// Executes the inclusion of the dependencies in the setup project
        /// </summary>
        public override void Undo()
        {
            Execute(false);
        }

        #endregion
    }
}
