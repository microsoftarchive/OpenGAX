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
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using EnvDTE80;
using VSLangProj;

namespace Microsoft.Practices.RecipeFramework.Library.Solution
{
    /// <summary>
    /// Utility functions to be used by Refrences
    /// </summary>
	public sealed class ReferenceUtil
	{
        /// <summary>
        /// Checks if the <paramref name="target"/> is under a folder name <paramref name="folderName"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static bool IsUnderFolderNested(object target, string folderName)
        {
            if (target is ProjectItem)
            {
                ProjectItem prItem = (ProjectItem)target;
                while (prItem != null && prItem.Kind.Equals(EnvDTE.Constants.vsProjectItemKindPhysicalFolder))
                {
                    if (prItem.Name.Equals(folderName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                    prItem = (ProjectItem)prItem.Collection.Parent;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if <paramref name="target"/> contains a <see cref="CodeClass"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="codeClass"></param>
        /// <returns></returns>
        public static bool HaveAClass(object target,out CodeClass codeClass)
        {
            ProjectItem prItem = null;
            if (target is SelectedItems)
            {
                SelectedItems items = (SelectedItems)target;
                if (items.Count > 1)
                {
                    if (items.Item(1).ProjectItem != null)
                    {
                        prItem = items.Item(1).ProjectItem;
                    }
                }
            }
            else if (target is ProjectItem)
            {
                prItem = (ProjectItem)target;
            }
            if (prItem!=null && prItem.FileCodeModel != null)
            {
                foreach (CodeElement codeElement in prItem.FileCodeModel.CodeElements)
                {
                    if (codeElement is CodeNamespace)
                    {
                        CodeNamespace codeNamespace = (CodeNamespace)codeElement;
                        if (codeNamespace.Members.Count > 0 && codeNamespace.Members.Item(1) is CodeClass)
                        {
                            codeClass = (CodeClass)codeNamespace.Members.Item(1);
                            return true;
                        }
                    }
                }
            }
            codeClass = null;
            return false;
        }

        /// <summary>
        /// Cheks if <paramref name="target"/> is located in CSharp project
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsCSharpProject(object target)
        {
            Project project=null;
            if ( target is Project )
            {
                project = (Project)target;
            }
            else if ( target is ProjectItem  )
            {
                project = ((ProjectItem)target).ContainingProject;
            }
            if (project != null)
            {
                return (project.Kind == PrjKind.prjKindCSharpProject);
            }
            return false;
        }

        /// <summary>
        /// Checks if the <paramref name="target"/> is under a folder name <paramref name="folderName"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static bool IsUnderFolder(object target, string folderName)
        {
            if (target is ProjectItem)
            {
                ProjectItem prItem = (ProjectItem)target;
                if (prItem != null && prItem.Kind.Equals(EnvDTE.Constants.vsProjectItemKindPhysicalFolder) &&
                    prItem.Name.Equals(folderName, StringComparison.InvariantCultureIgnoreCase))
                {
                   return true;
                }
            }
            return false;
        }

	}
}
