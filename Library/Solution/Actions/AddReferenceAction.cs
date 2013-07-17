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
using System.Collections;
using System.ComponentModel;
using System.IO;
using EnvDTE;
using VSLangProj;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.ComponentModel;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Solution.Actions
{
	/// <summary>
	/// Adds a reference to a project pointing to another 
	/// project in the same solution. 
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ServiceDependency(typeof(DTE))]
    public class AddReferenceAction : Action
	{
		#region Inputs

        /// <summary>
        /// The project where the reference is been added
        /// </summary>
		[Input(Required=true)]
		public Project ReferringProject
		{
			get { return referringProject; }
			set { referringProject = value; }
		} Project referringProject;

        /// <summary>
        /// The file name reference
        /// </summary>
		[Input(Required=true)]
		public string ReferenceName
		{
            get { return referenceName; }
            set { referenceName = value; }
        } string referenceName;

		#endregion

        /// <summary>
        /// Adds the reference to the project
        /// </summary>
		public override void Execute()
		{
            VSProject vsProject = ((VSProject)referringProject.Object);
            vsProject.References.Add(this.ReferenceName);
		}

        /// <summary>
        /// Not implemented
        /// </summary>
		public override void Undo()
		{
			// No undo supported as no Remove method exists on the VSProject.References property.
		}
    }
}

