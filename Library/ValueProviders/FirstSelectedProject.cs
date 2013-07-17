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

namespace Microsoft.Practices.RecipeFramework.Library.ValueProviders
{
    /// <summary>
    /// ValueProvider that returns the first selected project
    /// in the solution explorer
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class FirstSelectedProject : ValueProvider
    {
        /// <summary>
        /// Sets the newValue to the first selected project
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            DTE vs = (DTE)GetService(typeof(DTE));
            newValue = ((object[])vs.ActiveSolutionProjects)[0] as Project;
            Debug.Assert(newValue != null, "There's no selected project.");
            return true;
        }
    }
}