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
using Microsoft.Practices.Common;

namespace Microsoft.Practices.RecipeFramework.Library.VisualStudio.References
{
	/// <summary>
	/// UnBoundRecipe that allows to be executed only on Solution Folders
	/// </summary>
	[Serializable]
    public class FolderReference : UnboundRecipeReference, IAttributesConfigurable
	{
        private string folderName;
        
        /// <summary>
		/// Constructor of the FolderReference that must specify the 
		/// recipe name that will be used by the reference
		/// </summary>
		/// <param name="recipe"></param>
		public FolderReference(string recipe)
			: base(recipe)
		{
		}

		/// <summary>
		/// Returns a friendly name as Any Folder
		/// </summary>
		public override string AppliesTo
		{
			get { return "Any Folder"; }
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
            Project folder = target as Project;
            if (folder != null)
			{
                if (!string.IsNullOrEmpty(folderName))
                {
                    if (folder.Name == folderName)
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
		protected FolderReference(SerializationInfo info, StreamingContext context)
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
            if (attributes.ContainsKey("FolderName"))
            {
                folderName = attributes["FolderName"];
            }
        }

        #endregion	
    }
}
