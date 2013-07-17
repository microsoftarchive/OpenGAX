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
    /// A converter that validates that the input value is a path to a project item, 
    /// with the same rules as the <see cref="DteElementConverter"/>, but ensuring 
    /// the target is always an item.
    /// </summary>
    internal class ProjectItemConverter : DteElementConverter
    {
        /// <summary>
        /// Validates that the received value is a path expression that identifies an item in the 
        /// logical treeview of the solution explorer, starting at the solution.
        /// </summary>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            if (!(value is string))
            {
                return base.IsValid(context, value);
            }
            object element = base.ConvertFromInternal(context, CultureInfo.CurrentCulture, value, false);
            return (element != null && element is ProjectItem);
        }

        /// <summary>
        /// Converts an element into its path location relative to the solution.
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // Ensure only projects are converted and only to string.
            if (destinationType != typeof(string) || 
                !(value is ProjectItem))
            {
                throw new ArgumentException(Properties.Resources.ProjectItemConverter_InvalidConversion);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Converts from a path location to a project item instance.
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            object element = base.ConvertFrom(context, culture, value);
            if (!(element is ProjectItem))
            {
                throw new ArgumentException(Properties.Resources.ProjectItemConverter_InvalidPath, "value");
            }
            return element;
        }
    }
}
