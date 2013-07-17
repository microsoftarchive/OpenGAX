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
    /// A converter that validates that the input value is a path to a solution element, 
    /// relative to the solution root (itself represented by the "\"), 
    /// and provides conversion to/from a string with 
    /// that format for all element kinds (Solution, Project, SolutionFolder and ProjectItem).
    /// </summary>
    /// <remarks>
    /// The string representation of an element always starts with a slash, and can optionally 
    /// be only that character, if the value points to the solution itself.
    /// </remarks>
    public abstract class DteElementConverter : StringConverter
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
            object element = ConvertFromInternal(context, CultureInfo.CurrentCulture, value, false);
            return (element != null);
        }

        /// <summary>
        /// Converts from the path-like representation of an element to the actual element.
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return ConvertFromInternal(context, culture, value, true);
        }

        /// <summary>
        /// Converts an element into its path location relative to the solution.
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // We only support special conversion to string and from our known types.
            if (destinationType != typeof(string) ||
                !(value is EnvDTE.Solution ||
                value is EnvDTE80.SolutionFolder ||
                value is Project ||
                value is ProjectItem))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            // The helper requests the service and ensures that it exists.
            if (value is EnvDTE.Solution)
            {
                return "\\";
            }
            else 
            {
                return "\\" + DteHelper.BuildPath(value);
            }
        }

        /// <summary>
        /// Converts from a given solution relative path to a corresponding EnvDTE.ProjecItem
		/// in case it exists in the current solution
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="throwErrors"></param>
        /// <returns></returns>
		protected virtual internal object ConvertFromInternal(ITypeDescriptorContext context, CultureInfo culture, object value, bool throwErrors)
        {
            if (value is string)
            {
                // The helper requests the service and ensures that it exists.
                DTE vs = (DTE)ServiceHelper.GetService(context, typeof(DTE), this);
                if (vs.Solution == null)
                {
                    if (throwErrors)
                    {
                        throw new InvalidOperationException(Properties.Resources.DteElementConverter_NoSolutionOpened);
                    }
                    else
                    {
                        return null;
                    }
                }
                string path = (string)value;
                // First determine solution reference.
                if (path.Length == 0)
                {
                    return null;
                }
                if (path.Length == 1)
                {
                    if (path == "\\" || path == "/")
                    {
                        return vs.Solution;
                    }
                    else
                    {
                        if (throwErrors)
                        {
                            throw new FormatException(Properties.Resources.DteElementConverter_MustStartWithSlash);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    // Remove slash and try to locate project.
                    if (path.StartsWith("/") || path.StartsWith("\\"))
                    {
                        path = path.Substring(1);
                    }
                    Project project = DteHelper.FindProjectByPath(vs.Solution, path);
                    if (project != null)
                    {
                        return project;
                    }
                    else
                    {
                        // Try with item. Must find something to be valid.
                        ProjectItem item = DteHelper.FindItemByPath(vs.Solution, path);
                        if (item != null)
                        {
                            return item;
                        }
                        else
                        {
                            if (throwErrors)
                            {
                                throw new FormatException(Properties.Resources.DteElementConverter_CantFindElement);
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            else
            {
                try
                {
                    return base.ConvertFrom(context, culture, value);
                }
                catch
                {
                    if (throwErrors)
                    {
                        throw;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
