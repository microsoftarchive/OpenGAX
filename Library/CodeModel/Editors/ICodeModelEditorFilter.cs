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
using System.Collections.Generic;
using System.Text;
using EnvDTE;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel.Editors
{
    /// <summary>
    /// Interface implemeted by a custom filter object used in a <see cref="CodeModelEditor"/>
    /// </summary>
	public interface ICodeModelEditorFilter
	{
        /// <summary>
        /// Filters a <see cref="CodeElement"/> object
        /// </summary>
        /// <param name="codeElement"></param>
        /// <returns>Returns True is the object is been filtered out, false if not</returns>
        bool Filter(CodeElement codeElement);
	}
}
