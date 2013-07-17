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
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using EnvDTE80;
using Microsoft.Practices.RecipeFramework.VisualStudio;

namespace Microsoft.Practices.RecipeFramework.Library.VisualStudio.Converters
{
    /// <summary>
    /// Converter that returns a SolutionFolder object
    /// </summary>
    public class SolutionFolderConverter : TypeConverter
    {
        /// <summary>
        /// Returns a boolean value specifing if the type can be converted
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns>Returns true only if <paramref name="destinationType"/> is <see cref="SolutionFolder"/></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(SolutionFolder))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
		/// Converts <paramref name="value"/> to type <paramref name="destinationType"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value">A <see cref="string"/> object</param>
        /// <param name="destinationType"></param>
        /// <returns>The SolutionFolder of a <see cref="string"/></returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value is string)
            {
                EnvDTE.DTE vs = (EnvDTE.DTE)context.GetService(typeof(EnvDTE.DTE));
                return DteHelper.FindSolutionFolderByPath(vs.Solution, (string)value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
