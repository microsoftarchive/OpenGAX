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

#region Using Directives

using System;
using System.Collections.Specialized;
using System.Windows.Forms.Design;
using Microsoft.Practices.RecipeFramework.Services;
using System.ComponentModel.Design;
using System.Reflection;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions
{
    /// <summary>
    /// The action creates an instance of an unbound reference and registers the
    /// reference with the Recipe Framework. The action has two input properties
    /// (a) AssetName - name of the recipe or template (depending on the reference 
    /// type) and (b)ReferenceType - type of the reference class.  
    /// </summary>
	[ServiceDependency(typeof(ITypeResolutionService))]
	[ServiceDependency(typeof(IAssetReferenceService))]
    public sealed class CreateUnboundReferenceAction : ConfigurableAction
    {
        IAssetReference reference;
        StringDictionary attributes;

        #region Input Properties

        /// <summary>
        /// The name of the recipe or template
        /// </summary>
        [Input]
        public string AssetName
        {
            get { return assetName; }
            set { assetName = value; }
        } string assetName;

        /// <summary>
        /// Type of the reference class
        /// </summary>
        [Input]
        public string ReferenceType
        {
            get { return referenceType; }
            set { referenceType = value; }
        } string referenceType;

        #endregion
        
        #region IAction Members

        /// <summary>
        /// Creates the unbound reference and registers
        /// the reference with the Recipe Framework
        /// </summary>
        public override void Execute()
		{
			ITypeResolutionService typeLoader = GetService<ITypeResolutionService>(true);
			Type assetType = typeLoader.GetType(this.referenceType, true);

            if (!typeof(IUnboundAssetReference).IsAssignableFrom(assetType))
            {
                throw new ArgumentException(String.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.CreateUnboundReferenceAction_WrongType,
                    assetType)); 
            }

            ConstructorInfo ctor = assetType.GetConstructor(new Type[] { typeof(string) });
            if (ctor == null)
            {
                throw new NotSupportedException(String.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.CreateUnboundReferenceAction_UnsupportedConstructor,
                    assetType)); 
            }

            reference = (IAssetReference)ctor.Invoke(new object[] { this.assetName });
            if (reference is IAttributesConfigurable)
            {
                ((IAttributesConfigurable)reference).Configure(this.attributes);
            }
			IAssetReferenceService service = GetService<IAssetReferenceService>(true);
            service.Add(reference);
		}

        /// <summary>
        /// Removes the previously added reference
        /// </summary>
        public override void Undo()
        {
			IAssetReferenceService service = GetService<IAssetReferenceService>(true);
            if (reference != null)
            {
                service.Remove(reference);
                reference = null;
            }
        }

		#endregion

        /// <summary>
        /// Configures the action before executing
        /// </summary>
        /// <param name="attributes"></param>
        public override void Configure(StringDictionary attributes)
        {
            base.Configure(attributes);
            // Store attributes in case the reference type is configurable in turn.
            this.attributes = attributes;
        }
	}
}
