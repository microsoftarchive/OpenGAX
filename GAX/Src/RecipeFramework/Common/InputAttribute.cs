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

#region Using Directives

using System;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
    /// <summary>
    /// Specifies an input property for an action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class InputAttribute : System.Attribute
    {
        /// <summary>
        /// Initializes the attribute as a non-required input.
        /// </summary>
        public InputAttribute() { }

        /// <summary>
        /// Initializes the attribute, specifying whether it is required 
        /// for the execution of the action.
        /// </summary>
        /// <param name="required"><see langword="true"/> to indicate that the input is 
        /// required; otherwise, <see langword="false"/>.</param>
		[Obsolete("Set the Required property explicitly instead of using the constructor parameter.", false)]
        public InputAttribute(bool required)
        {
            this.required = required;
        }

        private bool required = false;

        /// <summary>
        /// Gets a value indicating whether the input property of the action is required 
        /// for it to execute correctly. 
        /// </summary>
        /// <remarks>
        /// If <see langword="true"/>, the annotated property must be non-null and 
        /// non-empty (if it is of type <see cref="System.String"/>). Otherwise, 
        /// an <see cref="ArgumentNullException"/> will be thrown.
        /// </remarks>
        public bool Required
        {
            get { return required; }
            set { required = value; }
        }
    }
}
