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

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Converters
{
    /// <summary>
    /// A converter that validates that the input value is a valid namespace name.
    /// </summary>
    public class NamespaceStringConverter : StringConverter
    {
        /// <summary>
        /// Validates that each of the parts of a namespace name (separated by a dot) is 
        /// a valid .NET identifier.
        /// </summary>
        /// <remarks>
        /// See <see cref="System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier"/>.
        /// </remarks>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            if (!(value is string))
            {
                return false;
            }
            string[] segments = ((string)value).Split('.');
            foreach (string segment in segments)
            {
                if (!System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(segment))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
