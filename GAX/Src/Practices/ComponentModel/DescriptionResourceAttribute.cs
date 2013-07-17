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
using System.ComponentModel;
using System.Resources;
using System.Text;

namespace Microsoft.Practices.ComponentModel
{
	/// <summary>
	/// Provides a description of a component that is localizable.
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public sealed class DescriptionResourceAttribute : DescriptionAttribute
	{
		/// <summary>
		/// Constructs the description attribute for localization.
		/// </summary>
		/// <param name="resourceBase">See <see cref="ResourceManager(Type)"/> for the 
		/// purpose of this parameter, which is the same passed to that constructor.</param>
		/// <param name="resourceName">Name of the resource to use for the description.</param>
		public DescriptionResourceAttribute(Type resourceBase, string resourceName) :
			base(new ResourceManager(resourceBase).GetString(
			resourceName, System.Globalization.CultureInfo.CurrentCulture))
		{
		}
	}
}
