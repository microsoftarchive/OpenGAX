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
using System.Runtime.Serialization;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using System.Security.Permissions;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
	/// <summary>
    /// Represents a recipe reference that points to the solution item.
	/// </summary>
	/// <seealso cref="RecipeReference"/>
	[Serializable]
	[ServiceDependency(typeof(DTE))]
	[System.Drawing.ToolboxBitmap(typeof(SolutionReference))]
	public class SolutionReference : VsBoundReference
	{
		private class SolutionReferenceStrategy : IBoundReferenceLocatorStrategy
		{
			#region IReferenceStrategy Members

			public string GetAppliesTo(object target)
			{
				return Properties.Resources.SolutionReference_AppliesTo;
			}

			public string GetSerializationData(object target)
			{
				return string.Empty;
			}

			public object LocateTarget(IServiceProvider serviceProvider, string serializedData)
			{
				DTE vs = (DTE)serviceProvider.GetService(typeof(DTE));
				return vs.Solution;
			}

			#endregion
		}

		#region Field & Constructor

		SolutionReferenceStrategy strategy;

		/// <summary>
		/// Initializes an instance of the <see cref="SolutionReference"/> class.
		/// </summary>
		/// <seealso cref="RecipeReference"/>
		public SolutionReference(string recipe, Solution solution)
			: base(recipe, solution)
		{
			this.strategy = new SolutionReferenceStrategy();
		}

		#endregion Field & Constructor

		#region Overrides

		/// <summary>
		/// Initializes tracking of the associated item.
		/// </summary>
		protected override void OnSited()
		{
			base.OnSited();
			IServiceProvider vs = GetService<IServiceProvider>(true);
			SetTarget(this.strategy.LocateTarget(vs, string.Empty));
		}

		/// <summary>
		/// Provides a string representation of the reference.
		/// </summary>
		public override string AppliesTo
		{
			get
			{
				return strategy.GetAppliesTo(null);
			}
		}

		/// <summary>
		/// Gets the recipe name, that uniquely identifies this reference, as only 
		/// a single instance of a recipe can be attached to the solution, and only one solution 
		/// can be opened in a given moment in VS.
		/// </summary>
		public override string Key
		{
			get { return base.AssetName; }
		}

		/// <summary>
		/// Returns an <see cref="IBoundReferenceLocatorStrategy"/> object
		/// </summary>
		public override IBoundReferenceLocatorStrategy Strategy
		{
			get { return strategy; }
		}

		#endregion Overrides

		#region ISerializable Members

		/// <summary>
		/// Initializes an instance of the <see cref="SolutionReference"/> class.
		/// </summary>
		/// <seealso cref="IAssetReference"/>
		protected SolutionReference(SerializationInfo info, StreamingContext context) 
			: base(info, context)
		{
			this.strategy = new SolutionReferenceStrategy();
		}

		#endregion ISerializable Members
	}
}
