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
using System.Collections;
using System.Collections.Specialized;

namespace Microsoft.Practices.RecipeFramework.Services
{
	/// <summary>
	/// Indexes bound references by asset name and target.
	/// </summary>
	/// <remarks>
	/// The <see cref="Find"/> method will return either one or no reference
	/// at all, because there can be at most one bound reference that point to a
	/// given asset and target.
	/// </remarks>
    public sealed class IndexerBoundAssetTarget : AssetReferenceIndexer
	{
        IDictionary references = new Hashtable(CaseInsensitiveHashCodeProvider.Default,
            CaseInsensitiveComparer.Default);

		#region IAssetReferenceIndexer Members

		/// <summary>
		/// See <see cref="IAssetReferenceIndexer.Add"/>.
		/// </summary>
		public override void Add(IAssetReference reference)
		{
			if (reference == null)
			{
				throw new ArgumentNullException("reference");
			}
			if ((reference is IBoundAssetReference) && (((IBoundAssetReference)reference).Target != null))
			{
                IBoundAssetReference boundref = (IBoundAssetReference)reference;
                IDictionary targets = (IDictionary)references[boundref.AssetName];
				if (targets == null)
				{
					targets = new HybridDictionary();
                    references.Add(boundref.AssetName, targets);
				}
                targets.Add(boundref.Target, reference);
			}
		}

		/// <summary>
		/// See <see cref="IAssetReferenceIndexer.Remove"/>.
		/// </summary>
		public override void Remove(IAssetReference reference)
		{
			if (reference == null)
			{
				throw new ArgumentNullException("reference");
			}
			if (reference is IBoundAssetReference)
			{
				IDictionary targets = (IDictionary)references[reference.AssetName];
				if ((targets != null) && (((IBoundAssetReference)reference).Target != null))
				{
					targets.Remove(((IBoundAssetReference)reference).Target);
				}
			}
		}

		/// <summary>
		/// See <see cref="IAssetReferenceIndexer.Find"/>.
		/// </summary>
        /// <returns>
        /// Returns a reference for a given asset (recipe or template for example) and 
        /// target (solution element for example). 
        /// </returns>
        public override IAssetReference[] Find(params object[] arguments)
		{
			if (arguments == null)
			{
				throw new ArgumentNullException("arguments");
			}
			CheckArgumentCount(arguments, 2);
			CheckArgumentType(arguments, 0, typeof(string));
			CheckArgumentNull(arguments, 1);

			string asset = (string)arguments[0];
            IDictionary targets = (IDictionary)references[asset];
			if (targets == null || targets[arguments[1]] == null)
			{
				return new IAssetReference[0];
			}

			return new IAssetReference[] { (IAssetReference)targets[arguments[1]] };
		}

		#endregion
	}
}
