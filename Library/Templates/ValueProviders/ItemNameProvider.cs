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
using Microsoft.Practices.ComponentModel;
using System.ComponentModel.Design;
using System.IO;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Templates.ValueProviders
{
    /// <summary>
    /// Provides the name of an item been unfolded
    /// </summary>
    [ServiceDependency(typeof(IDictionaryService))]
    public class ItemNameProvider : ValueProvider
	{
        #region Overrides

        /// <summary>Provides the name of an item been unfolded</summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <seealso cref="ValueProvider.OnBeginRecipe"/>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            IDictionaryService dictionaryService = (IDictionaryService)GetService(typeof(IDictionaryService));
            string name = dictionaryService.GetValue("rootname") as string;
            if (name != null)
            {
				name = Path.GetFileNameWithoutExtension(name);
            }
            else
            {
                name = dictionaryService.GetValue("projectname") as string;
            }
            if (name != null)
            {
                newValue = name;
                return true;
            }
            else
            {
                newValue = currentValue;
                return false;
            }
        }

        #endregion
    }
}
