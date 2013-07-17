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
using System.ComponentModel;
using System.Globalization;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Provides custom conversion to string for enums.
	/// </summary>
	internal class EnumerationConverter : EnumConverter
	{
		/// <summary>
		/// Initializes the converter.
		/// </summary>
		public EnumerationConverter(Type type)
			: base(type)
		{
		}

		/// <summary>
		/// Overrides default conversion rules for enums.
		/// </summary>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
				return value.ToString();

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
