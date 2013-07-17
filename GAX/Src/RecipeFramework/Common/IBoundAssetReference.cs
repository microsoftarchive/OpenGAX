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
	/// Represents a reference to an asset that is bound to a target item.
	/// </summary>
	/// <remarks>
	/// Implementations of this interface must be serializable to be 
	/// persisted. This means that the target must be properly expressed in the 
	/// serialized data so it can be relocated upon deserialization.
	/// <para>
	/// Implementations should provide a friendly representation of the reference target 
	/// through the <see cref="IAssetReference.AppliesTo"/> property implementation.
	/// </para>
	/// </remarks>
	public interface IBoundAssetReference : IAssetReference
	{
		/// <summary>
		/// Gets the target object the recipe is attached to.
		/// </summary>
		object Target { get; }

		/// <summary>
		/// Gets the string that represents the logical path to the target in the solution
		/// </summary>
		string SubPath { get; }

		/// <summary>
		/// Returns an <see cref="IBoundReferenceLocatorStrategy"/> object
		/// </summary>
		IBoundReferenceLocatorStrategy Strategy { get; }
	}
}
