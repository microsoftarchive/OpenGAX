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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Common;
using EnvDTE;

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel.ValueProviders
{
    /// <summary>
    /// Obtains the <see cref="CodeClass"/> for the paramter specified in "ClassName"
    /// </summary>
	public sealed class CodeClassProvider: ValueProvider, IAttributesConfigurable 
	{
        private string className;

        #region Overrides

        /// <summary>
        /// Obtains the <see cref="CodeClass"/> for the specified paramter "ClassName"
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <seealso cref="ValueProvider.OnBeginRecipe"/>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue == null)
            {
                DTE dte = (DTE)GetService(typeof(DTE));
                try
                {
                    newValue = CodeModelUtil.GetCodeModel(dte).CodeTypeFromFullName(this.className);
                }
                catch
                {
                    newValue = null;
                }
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
            className = attributes["ClassName"];
        }

        #endregion
    }
}
