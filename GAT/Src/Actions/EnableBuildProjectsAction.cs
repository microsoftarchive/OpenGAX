//===================================================================================
// Microsoft patterns & practices
// Guidance Automation Toolkit
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// License: MS-LPL
//===================================================================================

#region using
using System;
using System.Collections;
using System.Text;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using System.Diagnostics;
using System.IO;
#endregion

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.Actions
{
	/// <summary>
	/// Enables the build mark of all the projects of the solution
	/// </summary>
    [ServiceDependency(typeof(DTE))]
    public class EnableBuildProjectsAction : Action
    {
		/// <summary>
		/// Performs the checking "Build" to all the projects that have the solution in all configurations
		/// </summary>
        public override void Execute()
        {
            DTE vs = GetService<DTE>(true);
            foreach (SolutionConfiguration config in vs.Solution.SolutionBuild.SolutionConfigurations)
            {
				foreach (SolutionContext context in config.SolutionContexts)
				{
					context.ShouldBuild = true;
				}
            }
        }

		/// <summary>
		/// Undo the setting mark to build.  Not supported
		/// </summary>
        public override void Undo()
        {
        }
    }
}
