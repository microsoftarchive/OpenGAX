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
using System.ComponentModel.Design;
using System.Reflection;
using EnvDTE80;

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel.ValueProviders
{
    /// <summary>
    /// Obtains a parameter type in a Generic Type
    /// </summary>
	public sealed class GenericTypeProvider: ValueProvider, IAttributesConfigurable 
	{
		private string argumentName;
		private int parameter;

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
			IDictionaryService dictService = 
				(IDictionaryService)GetService(typeof(IDictionaryService));
            if ( dictService!=null && dictService.GetValue(argumentName)!=null )
            {
				DTE dte = (DTE)GetService(typeof(DTE));
				CodeClass childClass = (CodeClass)dictService.GetValue(argumentName);
				CodeClass codeClass = (CodeClass)childClass.Bases.Item(1);
				string fullName = codeClass.FullName;
				int leftParam = fullName.IndexOf('<');
				int rightParam = fullName.LastIndexOf('>');
				if (leftParam != -1 && rightParam != -1 && leftParam < rightParam)
				{
					fullName = fullName.Substring(leftParam + 1, rightParam - leftParam);
					string[] parameters = fullName.Split(',');
					if (parameter < parameters.Length && parameter >= 0)
					{
						string parameterName = parameters[parameter];
						try
						{
							newValue = CodeModelUtil.GetCodeModel(dte).CodeTypeFromFullName(parameterName);
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
				}
            }
            newValue = currentValue;
            return false;
        }

		private Type GetTypeFromCodeElement(CodeClass codeClass)
		{
			return null;
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
			argumentName = attributes["Argument"];
			parameter = int.Parse(attributes["Parameter"]);
        }

        #endregion
    }
}
