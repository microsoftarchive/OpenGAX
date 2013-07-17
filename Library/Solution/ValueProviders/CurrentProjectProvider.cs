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
using System.Text;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework.Services;
using EnvDTE;
using Microsoft.Practices.ComponentModel;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Solution.ValueProviders
{
    /// <summary>
    /// Returns the current selected project in the solution explorer
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class CurrentProjectProvider : ValueProvider
    {
        #region Overrides

        /// <summary>
        /// Use the DTE.SelectedItems collection to get the current selected project
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <seealso cref="ValueProvider.OnBeforeActions"/>
        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
            if (currentValue == null)
            {
                DTE dte = (DTE)GetService(typeof(DTE));
                if (dte.SelectedItems.Count == 1 )
                {
                    SelectedItem selection = dte.SelectedItems.Item(1);
                    if (selection.Project != null)
                    {
                        newValue = selection.Project;
                        return true;
                    }
                }
            }
            newValue = null;
            return false;
        }

        #endregion

    }
}

