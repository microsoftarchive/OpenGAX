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
using Microsoft.Win32;

namespace Microsoft.Practices.RecipeFramework.Library.ValueProviders
{
	/// <summary>
	/// It provides the default Author registered in the machine
	/// </summary>
	public class DefaultAuthorValueProvider : ValueProvider
	{
		/// <summary>
		/// When the recipe executes it will provide the registered Organization that will be our author by default
		/// </summary>
		/// <param name="currentValue"></param>
		/// <param name="newValue"></param>
		/// <returns></returns>
		public override bool OnBeginRecipe(object currentValue, out object newValue)
		{
			if (string.IsNullOrEmpty(currentValue as string))
			{
				string author = "";
				RegistryKey regKeyCurrentVersion = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
				if (regKeyCurrentVersion != null)
				{
					author = (string)regKeyCurrentVersion.GetValue("RegisteredOrganization", "");
				}
				if (string.IsNullOrEmpty(author))
				{
					author = (string)regKeyCurrentVersion.GetValue("RegisteredOwner", "");
				}
				newValue = author;
			}
			else
			{
				newValue = currentValue;
			}
			return true;
		}
	}
}
