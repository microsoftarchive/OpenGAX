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
using System.Collections;

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework.Services
{
	/// <summary>
	/// Persists state associated with a recipe.
	/// </summary>
	public interface IPersistenceService
	{
        /// <summary>
        /// Clears all information belonging to a package from the service.
        /// </summary>
        /// <param name="packageName">The name of the package whose state should be cleared.</param>
        void ClearState(string packageName);

		#region Reference State

		/// <summary>
		/// Loads the state of the <paramref name="packageName"/> belonging 
		/// to the <paramref name="packageName"/> from a persistent medium.
		/// </summary>
		/// <param name="packageName">Name of the package containing the reference.</param>
		/// <param name="reference">The reference that identifies the state.</param>
		/// <returns>The persisted state or <see langword="null"/> if none exists.</returns>
		/// <remarks>
		/// The <see cref="IAssetReference.Key"/> is used to perform the state lookup. Concrete 
		/// reference implementations must implement this property and provide a string value  
		/// that can be used to uniquely identify its associated state.
		/// </remarks>
		IDictionary LoadState(string packageName, IAssetReference reference);

		/// <summary>
		/// Removes the persisted state for the <paramref name="reference"/> belonging 
		/// to the <paramref name="packageName"/> from a persistent medium, if it is found.
		/// </summary>
		/// <param name="packageName">Name of the package containing the recipe.</param>
		/// <param name="reference">The reference that identifies the state.</param>
		/// <remarks>
		/// The <see cref="IAssetReference.Key"/> is used to perform the state lookup. Concrete 
		/// reference implementations must implement this property and provide a string value  
		/// that can be used to uniquely identify its associated state.
		/// </remarks>
        /// <returns>The existing state or <see langword="null"/>.</returns>
		IDictionary RemoveState(string packageName, IAssetReference reference);

		/// <summary>
		/// Stores the <paramref name="state"/> for the <paramref name="reference"/> belonging 
		/// to the <paramref name="packageName"/> on a persistent medium.
		/// </summary>
		/// <param name="packageName">Name of the package containing the recipe.</param>
		/// <param name="reference">The reference that identifies the state.</param>
		/// <param name="state">Arbitrary state associated with the recipe.</param>
		/// <remarks>
		/// The <see cref="IAssetReference.Key"/> is used to perform the state lookup. Concrete 
		/// reference implementations must implement this property and provide a string value  
		/// that can be used to uniquely identify its associated state.
		/// </remarks>
		void SaveState(string packageName, IAssetReference reference, IDictionary state);

		#endregion Reference State

		#region Asset References

		/// <summary>
		/// Loads a set of asset references for a package.
		/// </summary>
		/// <param name="packageName">Name of the package whose references should be loaded.</param>
		/// <returns>An array of zero or more elements depending on the persisted state.</returns>
		IAssetReference[] LoadReferences(string packageName);

        /// <summary>
		/// Removes all asset references belonging to a package.
        /// </summary>
        /// <param name="packageName">Name of the package whose references should be removed.</param>
		void RemoveReferences(string packageName);

		/// <summary>
		/// Stores a set of asset references for a package.
		/// </summary>
		/// <param name="packageName">Name of the package the references belong to.</param>
		/// <param name="references">The list of references to persist.</param>
		void SaveReferences(string packageName, IAssetReference[] references);

		#endregion Asset References
	}
}
