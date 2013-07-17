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

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Common
{
    /// <summary>
    /// Enum for each of the types of vstemplates supported
    /// </summary>
    public enum TemplateKind
    {
        /// <summary>
        /// A new item inside a project
        /// </summary>
        ProjectItem = 1,
        /// <summary>
        /// A new project
        /// </summary>
        Project = 2,
        /// <summary>
        /// A new solution
        /// </summary>
        Solution = 3,
    }
}
