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
	/// Handles asset references.
	/// </summary>
	public interface IAssetReferenceService
	{
		/// <summary>
		/// Adds a reference to the service, ignoring duplicate references.
		/// </summary>
        /// <param name="reference">The <see cref="IAssetReference"/> to add to the service.</param>
		void Add(IAssetReference reference);

        /// <summary>
        /// Adds a reference to the service with initial state information, 
        /// ignoring duplicate references.
        /// </summary>
        /// <param name="reference">The <see cref="IAssetReference"/> to add to the service.</param>
		/// <param name="initialState">The <see cref="System.Collections.IDictionary"/> containing the initial state information
        /// to assign to the reference.</param>
        void Add(IAssetReference reference, System.Collections.IDictionary initialState);

        /// <summary>
        /// Adds a reference to the service, specifying whether an exception should be thrown if 
        /// the reference already exists in the service.
        /// </summary>
        /// <param name="reference">The <see cref="IAssetReference"/> to add to the service.</param>
        /// <param name="throwIfDuplicate"><see langword="true"/> to indicate that an exception should be thrown if 
        /// the reference already exists in the service; otherwise, <see langword="false"/>.</param>
        void Add(IAssetReference reference, bool throwIfDuplicate);

        /// <summary>
        /// Adds a reference to the service with initial state information, 
        /// specifying whether an exception should be thrown if 
        /// the reference already exists in the service.
        /// </summary>
        /// <param name="reference">The <see cref="IAssetReference"/> to add to the service.</param>
		/// <param name="initialState">The <see cref="System.Collections.IDictionary"/> containing the initial state information
        /// to assign to the reference.</param>
        /// <param name="throwIfDuplicate"><see langword="true"/> to indicate that an exception should be thrown if 
        /// the reference already exists in the service; otherwise, <see langword="false"/>.</param>
        void Add(IAssetReference reference, System.Collections.IDictionary initialState, bool throwIfDuplicate);
        
        /// <summary>
        /// Adds an indexer to the service.
        /// </summary>
        /// <param name="indexerType">The <see cref="Type"/> to use as a key to identity the 
        /// indexer when the <see cref="Find"/> method is called.</param>
        /// <param name="indexerInstance">An <see cref="IAssetReferenceIndexer"/> to use as the indexer instance.</param>
        void AddIndexer(Type indexerType, IAssetReferenceIndexer indexerInstance);
        
        /// <summary>
        /// Adds a set of references to the service, ignoring duplicates.
        /// </summary>
        /// <param name="references">An array of type <see cref="IAssetReference"/> containing a list 
        /// of references to add.</param>
		void AddRange(IAssetReference[] references);
        
        /// <summary>
        /// Adds a set of references to the service, 
        /// specifying whether an exception should be thrown if 
        /// the reference already exists in the service.
        /// </summary>
        /// <param name="references">An array of type <see cref="IAssetReference"/> containing a list 
        /// of references to add.</param>
        /// <param name="throwIfDuplicate"><see langword="true"/> to indicate that an exception should be thrown if 
        /// the reference already exists in the service; otherwise, <see langword="false"/>.</param>
        void AddRange(IAssetReference[] references, bool throwIfDuplicate);

        /// <summary>
        /// Occurs when the list of references is modified.
        /// </summary>
        event EventHandler Changed;

        /// <summary>
        /// Finds and returns all references that match the specified criteria.
        /// </summary>
        /// <param name="indexerType">The <see cref="Type"/> of the indexer to use. This must be the 
        /// same argument passed to the <see cref="AddIndexer"/> method.</param>
        /// <param name="conditions">An array of type <see cref="object"/> that contains a list of optional conditions 
        /// the indexer will use to locate the references.</param>
        /// <returns>An array of type <see cref="IAssetReference"/> containing the references that match the criteria.</returns>
        IAssetReference[] Find(Type indexerType, params object[] conditions);

        /// <summary>
        /// Finds the first reference that matches the specified criteria.
        /// </summary>
        /// <param name="indexerType">The type of the indexer to use. This must be the 
        /// same argument passed to the <see cref="AddIndexer"/> method.</param>
        /// <param name="conditions">Optional conditions the indexer will use to locate the references.</param>
        /// <returns>The first reference (if there are many satisfying the criteria) or <see langword="null"/> if none is found.</returns>
        IAssetReference FindOne(Type indexerType, params object[] conditions);

        /// <summary>
        /// Returns all the references in the service.
        /// </summary>
        /// <returns>An array of type <see cref="IAssetReference"/> that contains all the references in the service.</returns>
        IAssetReference[] GetAll();

        /// <summary>
        /// Retrieves a reference for a given asset and target.
        /// </summary>
        /// <returns>An <see cref="IAssetReference"/> containing a reference, or <see langword="null"/> if none is found.</returns>
        /// <remarks>
        /// The returned reference can be either an <see cref="IBoundAssetReference"/> or 
        /// <see cref="IUnboundAssetReference"/>. In the latter case, it will be a reference to 
        /// the asset only, because no target is involved in the definition of an unbound reference.
        /// <para>
        /// The target will be searched first. Returning an unbound reference is the fallback 
        /// behavior.
        /// </para>
        /// </remarks>
        IAssetReference GetReferenceFor(string asset, object target);

        /// <summary>
        /// Retrieves all references for a given asset and target.
        /// </summary>
        /// <returns>An array of <see cref="IAssetReference"/> containing a references.</returns>
        /// <remarks>
        /// The returned reference can be either an <see cref="IBoundAssetReference"/> or 
        /// <see cref="IUnboundAssetReference"/>. In the latter case, it will be a reference to 
        /// the asset only, because no target is involved in the definition of an unbound reference.
        /// <para>
        /// The target will be searched first. Returning an unbound reference is the fallback 
        /// behavior.
        /// </para>
        /// </remarks>
        IAssetReference[] GetReferencesFor(string asset, object target);
 
        /// <summary>
        /// Determines whether the asset is enabled for the given target.
        /// </summary>
        /// <param name="asset">The asset the reference should point to.</param>
        /// <param name="target">The target the reference should be bound to.</param>
        /// <returns><see langword="true"/> if there is a reference bound to the 
        /// given <paramref name="target"/> for the specified <paramref name="asset"/>. 
        /// Otherwise, <see langword="false"/>.
        /// </returns>
        bool IsAssetEnabledFor(string asset, object target);

		/// <summary>
		/// Removes a reference and returns any saved state that it may have persisted.
		/// </summary>
        /// <param name="reference">An <see cref="IAssetReference"/> representing the reference to remove.</param>
		/// <returns>An <see cref="System.Collections.IDictionary"/> containing the state of the reference prior to removal.</returns>
		System.Collections.IDictionary Remove(IAssetReference reference);
	}
}
