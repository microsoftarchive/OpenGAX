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

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.ValueProviders
{
    /// <summary>
    /// Provides a value this global to the Visual Studio session
    /// </summary>
    /// <remarks>Use the "Key" property in your XML configuration file to specify the slot for the global value.</remarks>
    /// <example>&lt;ValueProvider Type="GlobalValueProvider" Key="GlobalSlotName"/></example>
    public class GlobalValueProvider : ValueProvider, IAttributesConfigurable
    {
        private string keyName; 

        #region Overrides

        /// <summary>Checks if the value exist in the dte.Solution.Globals collection, if so it returns it</summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <seealso cref="ValueProvider.OnBeginRecipe"/>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            DTE dte = (DTE)GetService(typeof(DTE));
            if (currentValue == null && dte.Solution.Globals.get_VariableExists(keyName))
            {
                newValue = dte.Solution.Globals[keyName];
                return true;
            }
            newValue = currentValue;
            return false;
        }

        /// <summary>Stores the current value in the specified slot name in the dte.Solution.Globals collection</summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <seealso cref="ValueProvider.OnBeforeActions"/>
        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
            if (currentValue != null)
            {
                DTE dte = (DTE)GetService(typeof(DTE));
                dte.Solution.Globals[keyName] = currentValue;
            }
            newValue = currentValue;
            return false;
        }

        #endregion

        #region IAttributesConfigurable Members

        void IAttributesConfigurable.Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            keyName = attributes["Key"];
        }

        #endregion
    }
}
