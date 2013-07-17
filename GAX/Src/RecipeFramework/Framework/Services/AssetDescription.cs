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

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework.Services
{
	/// <summary>
	/// Description of an asset in the framework.
	/// </summary>
	/// <remarks>
	/// Used by the <see cref="IHostService"/> service to provide additional 
	/// asset descriptions for a specific host.
	/// </remarks>
	public class AssetDescription: IAssetDescription 
	{
		/// <summary>
		/// Creates an asset description passing its complete information.
		/// </summary>
		/// <param name="category">The category the asset belongs to.</param>
		/// <param name="caption">The caption of the asset.</param>
		/// <param name="description">The description of the asset and its purpose.</param>
		public AssetDescription(string category, string caption, string description)
		{
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			if (category.Length == 0)
			{
				throw new ArgumentException(Properties.Resources.General_ArgumentEmpty, "category");
			}
			if (caption == null)
			{
				throw new ArgumentNullException("caption");
			}
			this.category = category;
			this.caption = caption;
			this.description = description;
		}

		private string category;

		/// <summary>
		/// Gets/sets category the asset belongs to.
		/// </summary>
		public string Category
		{
			get { return category; }
		}

		private string caption;

		/// <summary>
		/// Gets/sets the short caption of the asset.
		/// </summary>
		public string Caption
		{
			get { return caption; }
		}

		private string description = String.Empty;

		/// <summary>
		/// Gets/sets the description of the asset and its purpose.
		/// </summary>
		public string Description
		{
			get { return description; }
		}
	}
}
