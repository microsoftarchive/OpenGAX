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
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Represents the abstract base class for unbound recipe references that express a 
	/// condition by implementing the <see cref="IsEnabledFor"/> method 
	/// and do not need any data being serialized.
	/// </summary>
	[Serializable]
	public abstract class UnboundRecipeReference : RecipeReference, IUnboundAssetReference
	{
		/// <summary>
		/// Initializes the unbound reference.
		/// </summary>
        /// <param name="recipe">The name of the recipe pointed by the reference.</param>
		protected UnboundRecipeReference(string recipe)
			: base(recipe)
		{ 
		}

        /// <summary>
        /// Determines whether the reference is enabled for a particular target item, 
        /// based on the condition contained in the reference.
        /// </summary>
        /// <param name="target">The <see cref="Object"/> to check for references.</param>
        /// <returns>
        /// <see langword="true"/> if the reference is enabled for the given <paramref name="target"/>.
        /// Otherwise, <see langword="false"/>.
        /// </returns>
        public abstract bool IsEnabledFor(object target);

		/// <summary>
		/// Gets the name of the recipe to use as a key to identify the reference. 
		/// Only one unbound reference can point to a recipe at a given time.
		/// </summary>
		public override string Key
		{
			get { return base.AssetName; }
		}

        #region ISerializable Members

        /// <summary>
        /// Initializes a new instance of the <see cref="UnboundRecipeReference"/> with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected UnboundRecipeReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Provides a serialization member that derived classes can override to add 
        /// data to be stored for serialization.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Unbound implementations usually don't need to store data.
        }

        #endregion ISerializable Members

	}
}
