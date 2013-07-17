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
	/// Indexes bound references by target.
	/// </summary>
	/// <remarks>
	/// The <see cref="Find"/> method will potentially retrieve several 
	/// bound references, because as there may be multiple bound references to the 
    /// same item for diferent recipes. It can optionally receive the type 
    /// of the references to retrieve, to filter them.
	/// </remarks>
	public sealed class IndexerBoundTarget : AssetReferenceIndexer
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
				ArrayList reflist = (ArrayList)references[boundref.Target];
				if (reflist == null)
				{
					reflist = new ArrayList();
					references.Add(boundref.Target, reflist);
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
                IBoundAssetReference boundRef = (IBoundAssetReference)reference;
                if (boundRef.Target != null)
                {
                    ArrayList reflist = (ArrayList)references[boundRef.Target];
                    if (reflist != null)
                    {
                        reflist.Remove(reference);
                    }
                }
			}
		}

		/// <summary>
		/// See <see cref="IAssetReferenceIndexer.Find"/>.
		/// </summary>
        /// <returns>
        /// Returns all the references for a given target (first argument in the <paramref name="arguments"/> 
        /// list). Optionally the method can receive the type of references being searched for and filter 
        /// those based on a <see cref="Type"/> argument passed as the second one in <paramref name="arguments"/>.
        /// </returns>
        public override IAssetReference[] Find(params object[] arguments)
		{
			if (arguments == null)
			{
				throw new ArgumentNullException("arguments");
			}
			CheckArgumentCount(arguments, 1);
			CheckArgumentNull(arguments, 0);

			ArrayList reflist = (ArrayList)references[arguments[0]];
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
