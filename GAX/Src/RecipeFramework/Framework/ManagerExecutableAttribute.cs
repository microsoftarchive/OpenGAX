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

namespace Microsoft.Practices.RecipeFramework
{
    /// <summary>
    /// Attribute that allows control over execution of <see cref="IAssetReference"/> elements.
    /// in the Guidance Package Manager.
	/// This attribute is used for controlling behavior of references 
	/// that are displayed in the Guidance Package Manager. The attribute should 
	/// not be used by the Guidance Package authors.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
	public class ManagerExecutableAttribute : Attribute
	{
        /// <summary>
        /// Initializes the attribute.
        /// </summary>
        /// <param name="allowExecute">Specifies whether the kind of reference this 
        /// attribute is applied to can be executed from the Guidance Package Manager.</param>
        public ManagerExecutableAttribute(bool allowExecute)
        {
            allow = allowExecute;
        }

        bool allow;

        /// <summary>
        /// Gets whether the kind of reference this 
        /// attribute is applied to can be executed from the Guidance Package Manager.
        /// </summary>
        public bool AllowExecute
        {
            get { return allow; }
        }
	}
}
