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
using EnvDTE80;
using VSLangProj;
using System.Runtime.Serialization;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Templates
{
    /// <summary>
    /// UnBoundRecipe that allows to be executed only on SolutionFolders
    /// </summary>
    [Serializable]
    public class SolutionFolderTemplateReference : UnboundTemplateReference
    {
        /// <summary>
        /// Constructor of the SolutionFolderTemplateReference that must specify the 
        /// recipe name that will be used by the reference
        /// </summary>
        /// <param name="template"></param>
        public SolutionFolderTemplateReference(string template)
            : base(template)
        {
        }

        /// <summary>
        /// Performs the validation of the item passed as target
        /// Returns true if the reference is allowed to be executed in the target
        /// that is if the target is a solution folder
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool IsEnabledFor(object target)
        {
            return (target is SolutionFolder);
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected SolutionFolderTemplateReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion ISerializable Members
    }
}