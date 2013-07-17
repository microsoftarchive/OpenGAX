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
using System.Drawing;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Collections.Generic;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Templates
{
    /// <summary>
    /// Base class for Unbound references to a template
    /// </summary>
	[Serializable]
	[ToolboxBitmap(typeof(UnboundTemplateReference))]
	public abstract  class UnboundTemplateReference : TemplateReference, IUnboundAssetReference
	{
        /// <summary>
        /// Constructor from the template file name
        /// </summary>
        /// <param name="template"></param>
        protected UnboundTemplateReference(string template)
            : base(template)
		{
		}

		#region ISerializable members

        /// <summary>
        /// Required constructor for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
		protected UnboundTemplateReference(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

        /// <summary>
        /// <seealso cref="ISerializable.GetObjectData"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Unbound implementations usually don't need to store data.
        }

		#endregion

		#region IUnboundAssetReference Members

        /// <summary>
        /// <seealso cref="IUnboundAssetReference.IsEnabledFor"/>
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
		public abstract bool IsEnabledFor(object target);

		#endregion

        /// <summary>
        /// <seealso cref="IAssetReference.Key"/>
        /// </summary>
        public override string Key
        {
            get { return this.GetType().ToString() + "::" + base.AssetName; }
        }

        /// <summary>
        /// <seealso cref="IAssetReference.AppliesTo"/>
        /// </summary>
        public override string AppliesTo
		{
			get { return Properties.Resources.UnboundReference_AllItems; }
		}
	}
}
