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

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel
{
    /// <summary>
    /// Common utility functions for the Code Model
    /// </summary>
	public sealed class CodeModelUtil
	{
        /// <summary>
        /// Get the project code model from the current selected project
        /// </summary>
        /// <param name="dte"></param>
        /// <returns></returns>
        public static EnvDTE.CodeModel GetCodeModel(DTE dte)
        {
            if (dte.SelectedItems.Count == 1)
            {
                Project currentProject = null;
                if (dte.SelectedItems.Item(1).ProjectItem != null)
                {
                    currentProject = dte.SelectedItems.Item(1).ProjectItem.ContainingProject;
                }
                else
                {
                    currentProject = dte.SelectedItems.Item(1).Project;
                }
                if (currentProject != null)
                {
                    return currentProject.CodeModel;
                }
            }
            return null;
        }

		/// <summary>
		/// Gets a CodeElement object from the full name string
		/// </summary>
		/// <param name="dte"></param>
		/// <param name="codeElementName"></param>
		/// <returns></returns>
		public static CodeElement ConvertFromString(EnvDTE.DTE dte, string codeElementName)
		{
			return ConvertFromString(GetCodeModel(dte), codeElementName);
		}

		/// <summary>
		/// Gets a CodeElement object from the full name string
		/// </summary>
		/// <param name="codeModel"></param>
		/// <param name="codeElementName"></param>
		/// <returns></returns>
		public static CodeElement ConvertFromString(EnvDTE.CodeModel codeModel, string codeElementName)
		{
			try
			{
				CodeType codeType = codeModel.CodeTypeFromFullName(codeElementName);
				return (CodeElement)codeType;
			}
			catch
			{
				int dotIndex = codeElementName.LastIndexOf('.');
				if (dotIndex != -1)
				{
					try
					{
						string className = codeElementName.Substring(0, dotIndex);
						string memberName = codeElementName.Substring(dotIndex + 1);
						CodeElement parentElement = (CodeElement)codeModel.CodeTypeFromFullName(className);
						return new CodeElementEx(parentElement).Members.Item(memberName);
					}
					catch
					{
						return null;
					}
				}
				return null;
			}
		}

	}
}
