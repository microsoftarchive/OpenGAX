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

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel.ValueProviders
{
    /// <summary>
    /// Provides the first defined class in the current selected project item in the solution explorer
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class CurrentClassProvider: ValueProvider 
    {
        private CodeClass GetCurrentClass(ProjectItem prItem)
        {
            foreach (CodeElement codeElement in prItem.FileCodeModel.CodeElements)
            {
                if (codeElement is CodeNamespace)
                {
                    CodeNamespace codeNamepace = (CodeNamespace)codeElement;
                    if (codeNamepace.Members.Count > 0 && codeNamepace.Members.Item(1) is CodeClass)
                    {
                        return (CodeClass)codeNamepace.Members.Item(1);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// <seealso cref="ValueProvider.OnBeginRecipe"/>
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue == null)
            {
                DTE dte = (DTE)GetService(typeof(DTE));
                if ( dte.SelectedItems.Count==1 )
                {
                    ProjectItem prItem = dte.SelectedItems.Item(1).ProjectItem;
                    if (prItem != null)
                    {
                        newValue = GetCurrentClass(prItem);
                        if (newValue != null)
                        {
                            return true;
                        }
                    }
                }
            }
            newValue = currentValue;
            return false;
        }
    }
}
