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
using System.Runtime.Serialization;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Represents a reference to an asset that contains a condition to 
	/// determine enablement of the reference.
	/// </summary>
	public interface IUnboundAssetReference : IAssetReference
	{
		/// <summary>
		/// Determines whether the reference is enabled for a particular target item, 
		/// based on the condition contained in the reference.
		/// </summary>
        /// <param name="target">The <see cref="Object"/> to check for references.</param>
        /// <returns>
        /// <see langword="true"/> if the reference is enabled for the given <paramref name="target"/>.
        /// Otherwise, <see langword="false"/>.
        /// </returns>
		bool IsEnabledFor(object target);
	}
}
