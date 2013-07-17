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
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using System.Runtime.Serialization;
using System.Collections;
using Microsoft.Practices.Common;
using System.Security.Permissions;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
	/// <summary>
	/// Represents a reference to a visual studio command that launches a recipe.
	/// </summary>
	[Serializable]
	[ServiceDependency(typeof(DTE))]
	public abstract class VsBoundReference : RecipeReference, IBoundAssetReference
	{
        /// <summary>
        /// It will be used to store the subpath in SolutionExplorer of the reference
        /// </summary>
        protected string serializedData;

		/// <summary>
		/// Initializes an instance of the <see cref="VsBoundReference"/> class.
		/// </summary>
		protected VsBoundReference(string recipe, object target)
			: base(recipe)
		{
			this.target = target;
		}

		/// <summary>
		/// Disposes the base class and the item being tracked.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this.target = null;
		}

		/// <summary>
		/// Builds a unique key that identifies the reference and its target.
		/// </summary>
		public override string Key
		{
			get { return base.AssetName + "::" + this.AppliesTo; }
		}

		/// <summary>
		/// Sets the target to a new object.
		/// </summary>
		protected internal virtual void SetTarget(object target)
		{
			this.target = target;
			this.serializedData = this.Strategy.GetSerializationData(this.target);
		}

		/// <summary>
		/// Performs selection of the target item before executing the recipe 
		/// if it's not the same as the target.
		/// </summary>
		protected override ExecutionResult OnExecute()
		{
			DTE vs = (DTE)ServiceHelper.GetService(this, typeof(DTE));
			object selection = null;
			if (vs.SelectedItems != null && vs.SelectedItems.Count > 0)
			{
				IEnumerator enumerator = vs.SelectedItems.GetEnumerator();
				enumerator.MoveNext();
				SelectedItem item = (SelectedItem)enumerator.Current;
				// Determine current target selection.
				if (item.Project != null)
				{
					selection = item.Project;
				}
				else if (item.ProjectItem != null)
				{
					selection = item.ProjectItem;
				}
				else
				{
					selection = vs.Solution;
				}
			}
			if (this.target != selection)
			{
				IHostService host = (IHostService)ServiceHelper.GetService(this, typeof(IHostService));
				// Perform selection of reference target.
				host.SelectTarget(this.target);
			}
			return base.OnExecute();
		}

		/// <summary>
		/// Returns an <see cref="IBoundReferenceLocatorStrategy"/> object
		/// </summary>
		public abstract IBoundReferenceLocatorStrategy Strategy { get; }

		#region IBoundAssetReference Members

		[NonSerialized]
		object target;

		/// <summary>
		/// See <see cref="IBoundAssetReference.Target"/>.
		/// </summary>
		public object Target
		{
			get { return target; }
		}

        /// <summary>
		/// <see cref="IBoundAssetReference.SubPath"/> 
        /// </summary>
        public string SubPath
        {
            get { return serializedData; }
        }

		#endregion

		#region ISerializable Members

		/// <summary>
		/// Required constructor for deserialization.
		/// </summary>
		protected VsBoundReference(SerializationInfo info, StreamingContext context) 
			: base(info, context)
		{
			this.serializedData = info.GetString("data");
		}

		/// <summary>
		/// <seealso cref="ISerializable.GetObjectData"/>
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("data", this.serializedData);
		}

		#endregion ISerializable Members
	}
}
