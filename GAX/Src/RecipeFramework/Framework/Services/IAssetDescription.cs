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

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework.Services
{
    /// <summary>
    /// Description of an asset in the framework.
    /// </summary>
    /// <remarks>
    /// Used by the <see cref="IHostService"/> service to provide additional 
    /// asset descriptions for a specific host.
    /// </remarks>
    public interface IAssetDescription
    {
        /// <summary>
        /// Gets/sets category the asset belongs to.
        /// </summary>
        string Category { get; }

        /// <summary>
        /// Gets/sets the short caption of the asset.
        /// </summary>
        string Caption { get; }

        /// <summary>
        /// Gets/sets the description of the asset and its purpose.
        /// </summary>
        string Description { get; }
    }
}
