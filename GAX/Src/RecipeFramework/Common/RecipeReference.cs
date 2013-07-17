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
using System.Runtime.Serialization;
using Microsoft.Practices.ComponentModel;
using System.Security.Permissions;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.Common;

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Represents the abstract base class for references that point to recipes.
	/// </summary>
	[Serializable]
	[CategoryResource(typeof(Properties.Resources), "RecipeReference_Category")]
	[ServiceDependency(typeof(IConfigurationService))]
    [ServiceDependency(typeof(IExecutionService))]
	public abstract class RecipeReference : AssetReference
	{
		/// <summary>
		/// Initializes the reference to the recipe.
		/// </summary>
		/// <param name="recipe">The recipe the reference points to.</param>
		protected RecipeReference(string recipe) : base(recipe)
		{
		}

		/// <summary>
		/// Gets the caption of the recipe associated with this reference.
		/// </summary>
		public override string Caption
		{
			get 
			{
				IConfigurationService config = (IConfigurationService)
					ServiceHelper.GetService(this, typeof(IConfigurationService));
				return config.CurrentPackage[base.AssetName].Caption;
			}
		}

		/// <summary>
		/// Gets the description of the recipe associated with this reference.
		/// </summary>
		public override string Description
		{
			get
			{
				IConfigurationService config = (IConfigurationService)
					ServiceHelper.GetService(this, typeof(IConfigurationService));
                return config.CurrentPackage[base.AssetName].Description;
			}
		}

		/// <summary>
		/// Executes the reference upon its parent package. Descendants can change this 
		/// behavior if something needs to be done before or after execution.
		/// </summary>
		/// <returns>An <see cref="ExecutionResult"/> that represents the result of the execution of the recipe.</returns>
		protected override ExecutionResult OnExecute()
		{
            IExecutionService executor = (IExecutionService)
                ServiceHelper.GetService(this, typeof(IExecutionService));
			return executor.Execute(this);
		}

        #region ISerializable Members

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeReference"/> with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected RecipeReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion ISerializable Members
	}
}
