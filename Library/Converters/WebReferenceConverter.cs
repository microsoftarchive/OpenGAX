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
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Converters
{
    /// <summary>
    /// A converter that validates that the input value is a path to a web reference in a project, 
    /// with the same rules as the <see cref="DteElementConverter"/>, but ensuring 
    /// the target is always a web reference.
    /// </summary>
    public class WebReferenceConverter : DteElementConverter
    {
        /// <summary>
        /// Validates that the received value is a path expression that identifies a 
        /// web reference in a project based on the logical treeview of the solution explorer, 
        /// starting at the solution.
        /// </summary>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            if (!(value is string))
            {
                return base.IsValid(context, value);
            }
            object element = base.ConvertFromInternal(context, CultureInfo.CurrentCulture, value, false);
            return (element != null && element is ProjectItem &&
                DteHelper.IsWebReference((ProjectItem)element));
        }

        /// <summary>
        /// Converts an element into its path location relative to the solution.
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // Ensure only projects are converted and only to string.
            if (destinationType != typeof(string) || 
                !(value is ProjectItem) ||
                !DteHelper.IsWebReference((ProjectItem)value))
            {
                throw new ArgumentException(Properties.Resources.WebReferenceConverter_InvalidConversion);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Converts from a path location to a project item instance that points to a project item 
        /// that represents a web reference.
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            object element = base.ConvertFrom(context, culture, value);
            if (!(element is ProjectItem))
            {
                throw new ArgumentException(Properties.Resources.WebReferenceConverter_InvalidPath, "value");
            }
            return element;
        }
    }
}
