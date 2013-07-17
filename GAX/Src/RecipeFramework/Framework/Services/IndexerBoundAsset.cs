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
	/// Indexes bound asset references by asset name.
	/// </summary>
	/// <remarks>
	/// The <see cref="Find"/> method will potentially retrieve several 
	/// bound references because there may be multiple bound references to the 
	/// same asset from diferent targets. 
    /// <para>
    /// The <see cref="Find"/> method receives the name of the asset to locate, 
    /// and optionally the type of the references to retrieve, to filter them.
    /// </para>
	/// </remarks>
    public sealed class IndexerBoundAsset : AssetReferenceIndexer
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
			if (reference is IBoundAssetReference)
			{
                IBoundAssetReference boundref = (IBoundAssetReference)reference;
                ArrayList reflist = (ArrayList)references[boundref.AssetName];
				if (reflist == null)
				{
					reflist = new ArrayList();
                    references.Add(boundref.AssetName, reflist);
				}
				reflist.Add(reference);
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
				ArrayList reflist = (ArrayList)references[((IBoundAssetReference)reference).AssetName];
				if (reflist != null)
				{
					reflist.Remove(reference);
				}
			}
		}

		/// <summary>
		/// See <see cref="IAssetReferenceIndexer.Find"/>.
		/// </summary>
        /// <returns>
        /// Returns all the bound references for a given asset (recipe or template for example).
        /// </returns>
        public override IAssetReference[] Find(params object[] arguments)
		{
			if (arguments == null)
			{
				throw new ArgumentNullException("arguments");
			}
			CheckArgumentCount(arguments, 1);
			CheckArgumentType(arguments, 0, typeof(string));

			string asset = (string)arguments[0];
            ArrayList reflist = (ArrayList)references[asset];
			if (reflist == null)
			{
				return new IAssetReference[0];
			}

            if (arguments.Length > 1)
            {
                CheckArgumentType(arguments, 1, typeof(Type));
                // Filter by type.
                Type filtertype = (Type)arguments[1];
                ArrayList filtered = new ArrayList(reflist.Count);
                foreach (object reference in reflist)
                {
                    if (filtertype.IsAssignableFrom(reference.GetType()))
                    {
                        filtered.Add(reference);
                    }
                }
                return (IAssetReference[])filtered.ToArray(typeof(IAssetReference));
            }

			return (IAssetReference[])reflist.ToArray(typeof(IAssetReference));
		}

		#endregion
	}
}
