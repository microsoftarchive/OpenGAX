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

namespace Microsoft.Practices.RecipeFramework.Services
{
	/// <summary>
	/// Provides fast retrieval of asset references held in the <see cref="IAssetReferenceService"/>.
	/// </summary>
	public interface IAssetReferenceIndexer
	{
		/// <summary>
		/// Adds a reference to the index.
		/// </summary>
        /// <param name="reference">The <see cref="IAssetReference"/> to add to the indexer.</param>
		void Add(IAssetReference reference);

		/// <summary>
		/// Removes a reference from the index.
		/// </summary>
       /// <param name="reference">The <see cref="IAssetReference"/> to remove from the indexer.</param>
        void Remove(IAssetReference reference);
		
        /// <summary>
		/// Finds and returns the references that satisfy the arguments.
		/// </summary>
		/// <param name="arguments">An array of type <see cref="Object"/> that contains an indexer-specific 
		/// list of arguments to use for the search.</param>
		/// <returns>An array of type <see cref="IAssetReference"/> that contains the references that satisfy 
		/// the criteria.</returns>
		IAssetReference[] Find(params object[] arguments);
	}
}
