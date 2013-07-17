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
    /// A converter that validates that the input value is a path to a solution folder, 
    /// with the same rules as the <see cref="DteElementConverter"/>, but ensuring 
    /// the target is always a solution folder.
    /// </summary>
    internal class SolutionFolderConverter : DteElementConverter
    {
        /// <summary>
        /// Validates that the received value is a path expression that identifies a solution folder in the 
        /// logical treeview of the solution explorer, starting at the solution.
        /// </summary>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            if (!(value is string))
            {
                return base.IsValid(context, value);
            }
            object element = base.ConvertFromInternal(context, CultureInfo.CurrentCulture, value, false);
            return (element != null && 
				(element is EnvDTE80.SolutionFolder ||
				(element is Project && ((Project)element).Object is EnvDTE80.SolutionFolder)));
        }

        /// <summary>
        /// Converts a solution folder into its path location relative to the solution.
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // Ensure only projects are converted and only to string.
			if (destinationType == typeof(string) &&
				((value is EnvDTE80.SolutionFolder) || ((value is Project) && (((Project)value).Object is EnvDTE80.SolutionFolder))))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			throw new ArgumentException(Properties.Resources.SolutionFolderConverter_InvalidConversion);
        }

        /// <summary>
        /// Converts from a path location to a solution folder instance.
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            object element = base.ConvertFrom(context, culture, value);
            if (element is EnvDTE80.SolutionFolder) 
            {
				return element;
			}
			else if ((element is Project) && (((Project)element).Object is EnvDTE80.SolutionFolder))
			{
				return ((Project)element).Object;
            }
            throw new ArgumentException(Properties.Resources.SolutionFolderConverter_InvalidPath, "value");
        }
    }
}
