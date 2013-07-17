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
	/// Provides a category of a component that is localizable.
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public sealed class CategoryResourceAttribute : CategoryAttribute
	{
        Type resourceBase;

		/// <summary>
		/// Constructs the category attribute for localization.
		/// </summary>
		/// <param name="resourceBase">See <see cref="ResourceManager(Type)"/> for the 
		/// purpose of this parameter, which is the same passed to that constructor.</param>
		/// <param name="resourceName">Name of the resource to use for the category.</param>
		public CategoryResourceAttribute(Type resourceBase, string resourceName) : 
            base(resourceName)
		{
            this.resourceBase = resourceBase;
		}

        /// <summary>
        /// See <see cref="CategoryAttribute.GetLocalizedString"/>.
        /// </summary>
        protected override string GetLocalizedString(string value)
        {
            return new ResourceManager(resourceBase).GetString(
                value, System.Globalization.CultureInfo.CurrentCulture);
        }
	}
}
