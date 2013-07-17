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
using System.Collections.Generic;

namespace Microsoft.Practices.RecipeFramework.Services
{
    /// <summary>
    /// Indexes unbound asset references by asset name.
    /// </summary>
    /// <remarks>
    /// The <see cref="Find"/> method can only retrieve one unbound reference, 
    /// because the asset can only be pointed by only one unbound reference of 
    /// a given type.
    /// <para>
    /// The <see cref="Find"/> method receives the name of the asset to locate.
    /// </para>
    /// </remarks>
    public sealed class IndexerUnboundAsset : AssetReferenceIndexer
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
            if (reference is IUnboundAssetReference)
            {

                if (references.Contains(reference.Key))
                {
                    throw new ArgumentException(String.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
                        Properties.Resources.Indexer_AtMostOnUnboundPerAsset,
                        reference.Key));
                }
                references.Add(reference.Key, reference);

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
            if (reference is IUnboundAssetReference)
            {
                references.Remove(reference.Key);
            }
        }

        /// <summary>
        /// See <see cref="IAssetReferenceIndexer.Find"/>.
        /// </summary>
        /// <returns>
        /// Returns all the unbound references for a given asset (recipe or template for example).
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

            List<IAssetReference> foundAssetReferences = new List<IAssetReference>();

            foreach (string key in references.Keys)
            {
                string[] referenceKeyArray = key.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                string assetReference = string.Empty;
                if (referenceKeyArray.Length > 1)
                {
                    assetReference = referenceKeyArray[1];
                }
                else
                {
                    assetReference = referenceKeyArray[0];
                }

                if (asset == assetReference)
                {
                    foundAssetReferences.Add((IAssetReference)references[key]);
                }
            }

            return foundAssetReferences.ToArray();


        }



        #endregion
    }
}
