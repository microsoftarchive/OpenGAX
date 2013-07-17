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
	/// Base class with helper methods to use for custom indexers of asset references.
	/// </summary>
	public abstract class AssetReferenceIndexer : IAssetReferenceIndexer
	{
		#region IAssetReferenceIndexer Members

        /// <summary>
        /// Adds a reference to the index.
        /// </summary>
        /// <param name="reference">The reference to add to the indexer.</param>
        public abstract void Add(IAssetReference reference);

        /// <summary>
        /// Removes a reference from the index.
        /// </summary>
        /// <param name="reference">The reference to remove from the indexer.</param>
        public abstract void Remove(IAssetReference reference);

        /// <summary>
        /// Finds the references that satisfy the arguments.
        /// </summary>
        /// <param name="arguments">An indexer-specific list of arguments to use for the search.</param>
        /// <returns>An array of references that satisfy the criteria.</returns>
        public abstract IAssetReference[] Find(params object[] arguments);

		#endregion

		/// <summary>
		/// Ensures that the argument count is at least the required one.
		/// </summary>
		/// <exception cref="ArgumentException">The arguments length is less than the required count.</exception>
        /// <param name="arguments">The arguments to check.</param>
        /// <param name="requiredCount">The minimum number of values that <paramref name="arguments"/> should contain.</param>
		protected void CheckArgumentCount(object[] arguments, int requiredCount)
		{
			if (arguments.Length < requiredCount)
			{
				throw new ArgumentException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.Indexer_RequiredArgCount,
					this, requiredCount),
					"arguments");
			}
		}

		/// <summary>
		/// Ensures that the element at a given index is not null.
		/// </summary>
		/// <exception cref="ArgumentNullException">The element at the given index is null.</exception>
		/// <exception cref="IndexOutOfRangeException">The index is outside of the bounds of the array. A call to <see cref="CheckArgumentCount"/> should be issued previously.</exception>
        /// <param name="arguments">The arguments to check.</param>
        /// <param name="index">The index of the argument to check for null in <paramref name="arguments"/>.</param>
		static protected void CheckArgumentNull(object[] arguments, int index)
		{
			if (arguments[index] == null)
			{
				throw new ArgumentNullException("arguments[" + index + "]");
			}
		}

		/// <summary>
		/// Ensures that the argument at the given index is not null and is of the appropriate type.
		/// </summary>
		/// <exception cref="ArgumentNullException">The element at the given index is null.</exception>
		/// <exception cref="ArgumentException">The element at the given index is not of the required type.</exception>
		/// <exception cref="IndexOutOfRangeException">The index is outside of the bounds of the array. A call to <see cref="CheckArgumentCount"/> should be issued previously.</exception>
        /// <param name="arguments">The arguments to check.</param>
        /// <param name="index">The index of the argument to check for compatibility with <paramref name="requiredType"/>.</param>
        /// <param name="requiredType">The type that the value in <paramref name="arguments"/> at the given <paramref name="index"/> 
        /// should be compatible to (as implemented by the <see cref="Type.IsAssignableFrom"/> method).</param>
        protected void CheckArgumentType(object[] arguments, int index, Type requiredType)
		{
			CheckArgumentNull(arguments, index);
			if (!requiredType.IsAssignableFrom(arguments[index].GetType()))
			{
				throw new ArgumentException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.General_InvalidArgumentType,
					"arguments[" + index + "]", requiredType));
			}

		}
	}
}
