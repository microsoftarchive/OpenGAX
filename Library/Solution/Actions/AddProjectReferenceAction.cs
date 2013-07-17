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
using VsWebSite;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Solution.Actions
{
    /// <summary>
    /// Adds a reference to a project pointing to another 
    /// project in the same solution. 
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    [ServiceDependency(typeof(DTE))]
    public class AddProjectReferenceAction : Action
    {
        #region Inputs

        private Project referringProject;

        /// <summary>
        /// The project refrence been added
        /// </summary>
        [Input(Required=false)]
        public Project ReferringProject
        {
            get { return referringProject; }
            set { referringProject = value; }
        }

        private Project referencedProject;

        /// <summary>
        /// The project receiving the new reference
        /// </summary>
        [Input(Required=false)]
        public Project ReferencedProject
        {
            get { return referencedProject; }
            set { referencedProject = value; }
        }

        #endregion

        private const string WebProjectKind = "{E24C65DC-7377-472B-9ABA-BC803B73C61A}";

        /// <summary>
        /// Adds the project reference to the <see cref="ReferencedProject"/>
        /// </summary>
        public override void Execute()
        {
            if (this.ReferencedProject.UniqueName.Equals(this.ReferringProject.UniqueName, StringComparison.InvariantCultureIgnoreCase))
            {
                // Do nothing.
                return;
            }
            // If reference already exists, nothing happens.
            // If referringProject is a VSProject
            VSProject vsProject = referringProject.Object as VSProject;
            if (vsProject != null)
            {
                if (referencedProject.Kind.Equals(WebProjectKind, StringComparison.InvariantCultureIgnoreCase))
                {
                    VsWebSite.VSWebSite vsWebSite = (VsWebSite.VSWebSite)(referencedProject.Object);
                    foreach (VsWebSite.WebService webService in vsWebSite.WebServices)
                    {
                        vsProject.AddWebReference(webService.URL);
                    }
                }
                else
                {
                    // No error handling needed here. See documentation for AddProject.
                    vsProject.References.AddProject(referencedProject);
                }
            }
            else
            {
                // If the Project recived is not a VsProject
                // Try it with a webProject
                VSWebSite webProject = referringProject.Object as VSWebSite;
                if (webProject != null)
                {
                    if (referencedProject.Kind.Equals(WebProjectKind, StringComparison.InvariantCultureIgnoreCase))
                    {
                        VSWebSite vsWebSite = (VSWebSite)(referencedProject.Object);
                        foreach (VsWebSite.WebService webService in vsWebSite.WebServices)
                        {
                            webProject.WebReferences.Add(webService.URL, webService.ClassName);
                        }
                    }
                    else
                    {
                        // Check if the reference already exists in the WebProject
                        if(!IsAlreadyReferenced(webProject, referencedProject))
                        {
                            webProject.References.AddFromProject(referencedProject);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override void Undo()
        {
            // No undo supported as no Remove method exists on the VSProject.References property.
        }

        #region Private Implementation
        private bool IsAlreadyReferenced(VSWebSite webProject, Project referencedProject)
        {
            foreach(AssemblyReference reference in webProject.References)
            {
                if(reference.Name.Equals(ReferencedProject.Name))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
