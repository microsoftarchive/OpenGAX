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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using EnvDTE;
using Microsoft.Practices.ComponentModel;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Templates.ValueProviders
{
    /// <summary>
    /// Get the ProjectItem object of the item been unfolded
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public sealed class ProjectItemProvider: ValueProvider
    {
        #region Overides

        /// <summary>
        /// Uses <see cref="DteHelper.GetTarget"/> to obtain the new ProjectItem object
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
            if (currentValue == null)
            {
                DTE dte=(DTE)GetService(typeof(DTE));
                object target = DteHelper.GetTarget(dte);
                if (target is ProjectItem)
                {
                    newValue = target;
                    return true;
                }
            }
            newValue = currentValue;
            return false;
        }

        #endregion
    }
}
