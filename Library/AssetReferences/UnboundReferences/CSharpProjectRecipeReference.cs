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
using EnvDTE;
using VSLangProj;
using System.Runtime.Serialization;

namespace Microsoft.Practices.RecipeFramework.Library.AssetReferences.UnboundReferences
{
    /// <summary>
    /// UnBoundRecipe that allows to be executed only on CSharp Projects
    /// </summary>
    [Serializable]
    public class CSharpProjectRecipeReference : UnboundRecipeReference
    {

        /// <summary>
        /// Constructor of the CSharpProjectRecipeReference that must specify the 
        /// recipe name that will be used by the reference
        /// </summary>
        /// <param name="recipe"></param>
        public CSharpProjectRecipeReference(string recipe)
            : base(recipe)
        { 
        }

        /// <summary>
        /// Returns a friendly name as Any C# Project
        /// </summary>
        public override string AppliesTo
        {
            get { return Properties.Resources.UnboundReferences_AnyCSharpProject; }
        }

        /// <summary>
        /// Performs the validation of the item passed as target
        /// Returns true if the reference is allowed to be executed in the target
        /// that is if the target is a C# project
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool IsEnabledFor(object target)
        {
            if (!(target is Project))
            {
                return false;
            }
            return (((Project)target).Kind == PrjKind.prjKindCSharpProject);
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected CSharpProjectRecipeReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion ISerializable Members
    }
}
