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
using VSLangProj;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Converters
{
    /// <summary>
    /// A converter that validates that the input value is a path to a reference in a project. 
    /// </summary>
    /// <remarks>
    /// The value must follow the same rules as the <see cref="DteElementConverter"/>.
    /// </remarks>
    public class ReferenceConverter : DteElementConverter
    {
        /// <summary>
        /// Validates that the received value is a path expression that identifies a reference 
        /// in a project in the logical treeview of the solution explorer, starting at the solution.
        /// </summary>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            object reference = ConvertFromInternal(context, CultureInfo.CurrentCulture, value, false);
            return reference != null;
        }

        /// <summary>
        /// Converts an element into its path location relative to the solution.
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // Ensure only projects are converted and only to string.
            if (destinationType != typeof(string) || 
                !(value is Reference))
            {
                throw new ArgumentException(Properties.Resources.ReferenceConverter_InvalidConversion);
            }
            Reference r = (Reference)value;
            return DteHelper.BuildPath(r.ContainingProject) + "\\" +
                Properties.Resources.ReferenceConverter_ReferencesNode + "\\" + r.Name;
        }

        /// <summary>
        /// Converts from a path location to a project item instance.
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return ConvertFromInternal(context, culture, value, true);
        }
        /// <summary>
        /// Internal method that performs the conversion from a path location to a project item instance
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="throwErrors"></param>
        /// <returns></returns>
        protected internal override object ConvertFromInternal(ITypeDescriptorContext context, CultureInfo culture, object value, bool throwErrors)
        {
            if (!(value is string))
            {
                return base.ConvertFromInternal(context, culture, value, throwErrors);
            }
            string path = (string)value;
            string refsnode = "\\" + Properties.Resources.ReferenceConverter_ReferencesNode;
            string projectpath = path.Substring(0, path.IndexOf(refsnode));
            string referencename = path.Substring(path.IndexOf(refsnode) + refsnode.Length + 1);

            VSProject project = base.ConvertFromInternal(context, CultureInfo.CurrentCulture, projectpath, throwErrors) as VSProject;
            if (project == null)
            {
                if (throwErrors)
                {
                    throw new ArgumentException(Properties.Resources.ReferenceConverter_InvalidPath);
                }
                else
                {
                    return null;
                }
            }
            foreach (Reference prjref in project.References)
            {
                if (prjref.Name == referencename)
                {
                    return prjref;
                }
            }
            if (throwErrors)
            {
                throw new ArgumentException(Properties.Resources.ReferenceConverter_InvalidPath);
            }
            else
            {
                return null;
            }
        }
    }
}
