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
using System.ComponentModel;
using System.Globalization;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Converters
{
    /// <summary>
    /// A converter that validates that the input value is a valid .NET identifier (i.e. valid method, 
    /// property, class name, etc.)
    /// </summary>
    /// <remarks>
    /// See <see cref="System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier"/>.
    /// </remarks>
    public class CodeIdentifierStringConverter : StringConverter
    {
        /// <summary>
        /// Validates that the received value is a code identifier 
        /// for the language that is using
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            if (!(value is string))
            {
                return false;
            }
			if (value is string)
			{
				value = ((string)value).Replace(' ', '_');
			}
            return System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(
                (string)value);
        }

		/// <summary>
		/// Converts and string from string, if the string contains spaces then are replaced by underscore
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				value = ((string)value).Replace(' ','_');
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>
		/// Converts and string from string, if the string contains spaces then are replaced by underscore
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <param name="destinationType"></param>
		/// <returns></returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is string)
			{
				value = ((string)value).Replace(' ', '_');
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
