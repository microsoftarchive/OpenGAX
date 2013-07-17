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
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.Specialized;
using Microsoft.Practices.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Converters
{
	/// <summary>
	/// A converter that validates that a string matches with a pattern of regular expression
	/// </summary>
    public class RegexMatchStringConverter : StringConverter, IAttributesConfigurable
    {
        /// <summary>
        /// Key that must exist in the dictionary passed to the <see cref="Configure"/> method, 
        /// which must be the string "Expression".
        /// </summary>
        public const string ExpressionKey = "Expression";

        /// <summary>
        /// The expression used to evaluate the validity of the value.
        /// </summary>
        Regex expression;

        /// <summary>
        /// Validates that the received value is a regular expression 
        /// that matches the expression we are using
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            if (expression == null)
            {
                throw new InvalidOperationException(Properties.Resources.RegexStringConverter_ExpressionMissing);
            }
            if (!(value is string))
            {
                return false;
            }
            return expression.IsMatch((string)value);
        }

        /// <summary>
        /// Configures the converter with the expression to use to determine validity, 
        /// which may be a value with the key "Expression".
        /// </summary>
        /// <param name="attributes">List of attributes to configure the converter. Must 
        /// contain an entry with the key "Expression".</param>
        public void Configure(StringDictionary attributes)
        {
            if (!attributes.ContainsKey(ExpressionKey))
            {
                throw new ArgumentException(Properties.Resources.RegexStringConverter_ExpressionMissing);
            }
            expression = new Regex(attributes[ExpressionKey]);
        }
    }
}
