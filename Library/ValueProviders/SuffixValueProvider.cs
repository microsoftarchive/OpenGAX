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
using System.Collections.Generic;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;

namespace Microsoft.Practices.RecipeFramework.Library.ValueProviders
{
	/// <summary>
	/// Removes a suffix (if it exist) from a string value
	/// </summary>
	public class SuffixValueProvider : ValueProvider, IAttributesConfigurable
	{
		private string argumentName;
		private string suffix;

		#region Overrides

		/// <summary>Removes the suffix and returns the new value</summary>
		/// <param name="currentValue"></param>
		/// <param name="newValue"></param>
		/// <returns></returns>
		/// <seealso cref="ValueProvider.OnBeginRecipe"/>
		public override bool OnBeginRecipe(object currentValue, out object newValue)
		{
			IDictionaryService dictService = (IDictionaryService)GetService(typeof(IDictionaryService));
			if (dictService != null && dictService.GetValue(argumentName)!=null )
			{
				string value = dictService.GetValue(argumentName).ToString();
				int iSuffix = value.LastIndexOf(suffix);
				if (iSuffix != -1 && iSuffix>0)
				{
					value = value.Substring(0, iSuffix);
				}
				newValue = value;
				return true;
			}
			newValue = currentValue;
			return false;
		}

		/// <summary>Removes the suffix and returns the new value</summary>
		/// <param name="currentValue"></param>
		/// <param name="newValue"></param>
		/// <returns></returns>
		/// <seealso cref="ValueProvider.OnBeforeActions"/>
		public override bool OnBeforeActions(object currentValue, out object newValue)
		{
			return OnBeginRecipe(currentValue, out newValue);
		}

		#endregion

		#region IAttributesConfigurable Members

		void IAttributesConfigurable.Configure(System.Collections.Specialized.StringDictionary attributes)
		{
			argumentName = attributes["Argument"];
			suffix = attributes["Suffix"];
		}

		#endregion
	}
}
