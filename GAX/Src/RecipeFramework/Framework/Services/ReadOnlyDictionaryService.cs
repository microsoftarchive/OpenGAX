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

#region Using directives

using System;
using System.ComponentModel.Design;

#endregion

namespace Microsoft.Practices.RecipeFramework.Services
{
	/// <summary>
	/// Lighweight proxy around the <see cref="DictionaryService"/> that makes it readonly.
	/// </summary>
	internal class ReadOnlyDictionaryService : IDictionaryService
	{
		IDictionaryService innerService;

		public ReadOnlyDictionaryService(IDictionaryService innerService)
		{
			this.innerService = innerService;
		}

		#region IDictionaryService Members

		public object GetKey(object value)
		{
			return innerService.GetKey(value);
		}

		public object GetValue(object key)
		{
			return innerService.GetValue(key);
		}

		public void SetValue(object key, object value)
		{
			throw new NotSupportedException(Properties.Resources.IDictionaryService_ReadOnlyException);
		}

		#endregion
	}
}
