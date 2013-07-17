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
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using System.Globalization;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library.Converters
{
    /// <summary>
    /// A converter that validates that the input value is a reference to the root solution, 
    /// that is, either a forward or a backward slash.
    /// </summary>
    internal class SolutionConverter : StringConverter
    {
        /// <summary>
        /// Validates that the received value is a path identifies the solution, 
        /// that is, it is either a forward or a backward slash.
        /// </summary>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            if (!(value is string))
            {
                return base.IsValid(context, value);
            }
            string path = (string)value;
            return (path.Length == 1 && (path == "\\" || path == "/") &&
                ((DTE)ServiceHelper.GetService(context, typeof(DTE), this)).Solution != null);
        }

        /// <summary>
        /// Converts a solution to its string representation.
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // Ensure only projects are converted and only to string.
            if (destinationType != typeof(string) || 
                !(value is Solution))
            {
                throw new ArgumentException(Properties.Resources.SolutionConverter_InvalidConversion);
            }
            return "\\";
        }

        /// <summary>
        /// Converts from a string to the solution instance.
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (IsValid(context, value))
            {
                return ((DTE)ServiceHelper.GetService(context, typeof(DTE), this)).Solution;
            }
            else
            {
                throw new ArgumentException(Properties.Resources.SolutionConverter_InvalidPath, "value");
            }
        }
    }
}
