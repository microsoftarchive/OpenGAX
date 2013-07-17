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
using Microsoft.Practices.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Represents a reference to an asset.
	/// </summary>
	/// <remarks>
	/// Implementations of this interface must be serializable to be 
	/// persisted.
	/// </remarks>
	public interface IAssetReference : IComponent
	{
		/// <summary>
		/// Gets a key that uniquely identifies this reference in a package. 
		/// </summary>
		string Key { get; }

		/// <summary>
		/// Gets a caption for the reference.
		/// </summary>
		string Caption { get; }

		/// <summary>
		/// Gets a description of the purpose of the asset being referenced.
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Gets a friendly description of the applicability of the reference.
		/// </summary>
		/// <remarks>
		/// For <see cref="IBoundAssetReference"/>, this is usually a string 
		/// representation of the <see cref="IBoundAssetReference.Target"/>. For 
		/// <see cref="IUnboundAssetReference"/>, this is a friendly description 
		/// of the condition coded in the <see cref="IUnboundAssetReference.IsEnabledFor"/> method.
		/// </remarks>
		string AppliesTo { get; }

        /// <summary>
        /// Gets the name of the asset being referenced.
        /// </summary>
        string AssetName { get; }

		/// <summary>
		/// Executes the asset.
		/// </summary>
		/// <returns>An <see cref="ExecutionResult"/> that represents the result of the execution.</returns>
		ExecutionResult Execute();
	}
}
