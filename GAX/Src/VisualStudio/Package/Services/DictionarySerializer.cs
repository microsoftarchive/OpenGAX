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
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Services
{
	/// <summary>
	/// Manages serialization of an arbitrary dictionary of 
	/// values to and from a simple string representation.
	/// </summary>
	internal class DictionarySerializer
	{
		private DictionarySerializer() { }

		/// <summary>
		/// Serializes a dictionary to a string representation.
		/// </summary>
		public static string Serialize(IDictionary dictionary)
		{
			if (dictionary == null)
			{
				return null;
			}
			return HttpUtility.UrlEncode(
				new ObjectStateFormatter().Serialize(dictionary));
		}

		/// <summary>
		/// Deserializes the dictionary from its string representation.
		/// </summary>
		public static IDictionary Deserialize(string value)
		{
			if (value == null)
			{
				return null;
			}
			return (IDictionary)new ObjectStateFormatter().Deserialize(
				HttpUtility.UrlDecode(value));
		}
	}
}
