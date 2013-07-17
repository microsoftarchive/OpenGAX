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
using EnvDTE;
using Microsoft.Practices.ComponentModel;

namespace Microsoft.Practices.RecipeFramework.Library.Actions
{
    /// <summary>
    /// The action adds a build dependency from one solution project to another solution 
    /// project. The action has two input properties (a) Project - the project that will
    /// have the dependency and (b) dependencyProject - the project that the Project project
    /// depends on. 
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddProjectDependencyAction : ConfigurableAction
    {
        private EnvDTE.Project parent;

        /// <summary>
        /// Parent project that will contain the dependency to the <see cref="DependencyProject"/>.
        /// </summary>
        [Input]
        public EnvDTE.Project Project
        {
            get { return parent; }
            set { parent = value; }
        }

        private EnvDTE.Project dependencyProject;

        /// <summary>
        /// Project that the <see cref="Project"/> depends on.
        /// </summary>
        [Input]
        public EnvDTE.Project DependencyProject
        {
            get { return dependencyProject; }
            set { dependencyProject = value; }
        }

        /// <summary>
        /// Attaches the registration recipes to the package project.
        /// </summary>
        public override void Execute()
        {
            DTE vs = GetService<DTE>(true);
            vs.Solution.SolutionBuild.BuildDependencies.Item(parent).AddProject(dependencyProject.UniqueName);
        }

        /// <summary>
        /// Removes the added references.
        /// </summary>
        public override void Undo()
        {
        }
    }
}
