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
using EnvDTE80;
using VSLangProj;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework;
using System.Xml;
using System.IO;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.Common;

namespace Microsoft.Practices.RecipeFramework.Library.VisualStudio.References
{
	/// <summary>
	/// UnBoundRecipe that allows to be executed only on ProjectItem
	/// </summary>
	[Serializable]
    public class ProjectItemReference : UnboundRecipeReference, IAttributesConfigurable
	{
        private string fileExtension;

        /// <summary>
        /// UnBoundRecipe that allows to be executed only on ProjectItem file
        /// </summary>
        public ProjectItemReference(string recipe)
			: base(recipe)
		{
		}

		/// <summary>
		/// Returns a friendly name as Project Item 
		/// </summary>
		public override string AppliesTo
		{
            get { return "Any ProjectItem."; }
		}

		/// <summary>
		/// Performs the validation of the item passed as target
		/// Returns true if the reference is allowed to be executed in the target
        /// that is if the target is a ProjectItem file.
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public override bool IsEnabledFor(object target)
		{
            ProjectItem item = target as ProjectItem;
            if (item != null)
            {
                if (!string.IsNullOrEmpty(fileExtension))
                {
                    if (Path.GetExtension(item.Name).ToLower() == fileExtension)
                    {
                        return true;
                    }
                }
                else 
                { 
                    return true; 
                }
            }
            return false;
		}

		#region ISerializable Members

		/// <summary>
		/// Required constructor for deserialization.
		/// </summary>
        protected ProjectItemReference(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion ISerializable Members

        #region IAttributesConfigurable Members

        /// <summary>
        /// Stores the set of user defined attributes
        /// </summary>
        /// <param name="attributes"></param>
        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes.ContainsKey("FileExtension"))
            {
                fileExtension = attributes["FileExtension"];
            }
        }

        #endregion
}
}
