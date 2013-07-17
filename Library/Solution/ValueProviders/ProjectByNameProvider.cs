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
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.ComponentModel;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Solution.ValueProviders
{
    /// <summary>
    /// Search for the project specified by the "Path" attributte in the XML configuration file
    /// </summary>
    /// <example>&lt;ValueProvider Type="ProjectByNameProvider" Path="BusinessLayer\SubProject1"/></example>
    [ServiceDependency(typeof(DTE))]
    public class ProjectByNameProvider : ValueProvider, IAttributesConfigurable
    {
        private string path; 

        #region Overrides

        /// <summary>
        /// Uses <see cref="DteHelper.FindProjectByPath"/> to search for the project specified by the "Path" attributte
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <seealso cref="ValueProvider.OnBeginRecipe"/>
        /// <seealso cref="DteHelper.FindProjectByPath"/>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            DTE dte = (DTE)GetService(typeof(DTE));
            if (currentValue == null )
            {
                newValue = DteHelper.FindProjectByPath(dte.Solution, this.path);
                if (newValue != null)
                {
                    return true;
                }
            }
            newValue = currentValue;
            return false;
        }

        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <seealso cref="ValueProvider.OnBeforeActions"/>
        /// <seealso cref="OnBeginRecipe"/>
        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
            return OnBeginRecipe(currentValue, out newValue);
        }

        #endregion

        #region IAttributesConfigurable Members

        void IAttributesConfigurable.Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            path = attributes["Path"];
        }

        #endregion
    }
}

