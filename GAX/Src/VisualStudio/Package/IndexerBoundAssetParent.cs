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
using Microsoft.Practices.RecipeFramework.Services;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
    internal sealed class IndexerBoundAssetParent : AssetReferenceIndexer
    {
        IDictionary references = new Hashtable(CaseInsensitiveHashCodeProvider.Default,
            CaseInsensitiveComparer.Default);

        public override void Add(IAssetReference reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }
            if (reference is IBoundAssetReference)
            {
                IBoundAssetReference boundref = (IBoundAssetReference)reference;
                EnvDTE.Project parentProject = null;
                if (boundref.Target is EnvDTE.ProjectItem)
                {
                    EnvDTE.ProjectItem prItem = (EnvDTE.ProjectItem)boundref.Target;
                    parentProject = prItem.ContainingProject;
                }
                else if (boundref.Target is EnvDTE.Project)
                {
                    EnvDTE.Project project = (EnvDTE.Project)boundref.Target;
                    try
                    {
                        if (project.ParentProjectItem != null && project.ParentProjectItem.ContainingProject != null)
                        {
                            parentProject = project.ParentProjectItem.ContainingProject;
                        }
                    }
                    catch
                    {
                        parentProject = null;
                    }
                }
                if ( parentProject!=null )
                {
                    ArrayList reflist = null;
                    if (!references.Contains(parentProject))
                    {
                        reflist = new ArrayList();
                        references.Add(parentProject, reflist);
                    }
                    reflist = (ArrayList)references[parentProject];
                    reflist.Add(reference);
                }
            }
        }

        public override void Remove(IAssetReference reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }
            if (reference is IBoundAssetReference)
            {
                IBoundAssetReference boundref = (IBoundAssetReference)reference;
                if (boundref.Target is EnvDTE.ProjectItem)
                {
                    EnvDTE.ProjectItem prItem = (EnvDTE.ProjectItem)boundref.Target;
					EnvDTE.Project containingProject = null;
					try
					{
						containingProject = prItem.ContainingProject;
					}
					catch
					{
						containingProject = null;
					}
					if (containingProject != null)
					{
						ArrayList reflist = (ArrayList)references[containingProject];
						if (reflist != null)
						{
							reflist.Remove(reference);
						}
					}
                }
            }
        }

        public override IAssetReference[] Find(params object[] arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }
            CheckArgumentCount(arguments, 1);
            EnvDTE.Project project = (EnvDTE.Project)arguments[0];
            ArrayList refs = (ArrayList)references[project];
            if (refs == null)
            {
                return new IAssetReference[0];
            }
            return (IAssetReference[])refs.ToArray(typeof(IAssetReference));
        }
    }
}
