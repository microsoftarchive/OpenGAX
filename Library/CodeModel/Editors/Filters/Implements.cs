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
using Microsoft.Practices.ComponentModel;
using System.ComponentModel.Design;
using EnvDTE;

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel.Editors.Filters
{
    /// <summary>
	/// Filter for the CodeModelEditor, configure using the "Implements" parameter in 
    /// your xml configuration file
    /// </summary>
    [ServiceDependency(typeof(IDictionaryService))]
	public sealed class Implements: SitedComponent, ICodeModelEditorFilter 
	{
        #region Private Implementation

		private string implementsInterfaceName = string.Empty;

        private bool CheckImplements(CodeClass codeClass)
        {
			if (codeClass.FullName.Equals(this.implementsInterfaceName, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            foreach (CodeInterface codeInterface in codeClass.ImplementedInterfaces)
            {
                if (codeInterface.FullName.Equals(this.implementsInterfaceName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
			}
			//foreach(CodeClass codeBase in codeClass.Bases)
			//{
			//    if (CheckImplements(codeBase))
			//    {
			//        return true;
			//    }
			//}
            return false;
        }

        private bool HasClassThatImplements(CodeNamespace codeNamespace)
        {
            foreach (CodeElement codeElement in codeNamespace.Members)
            {
                if (codeElement is CodeClass)
                {
                    if (CheckImplements((CodeClass)codeElement))
                    {
                        return true;
                    }
                }
                else if (codeElement is CodeNamespace)
                {
                    if (HasClassThatImplements((CodeNamespace)codeElement))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region Overrides

        /// <summary>
		/// Get's the configured parameter "Implements"
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            IDictionaryService dictService =
                (IDictionaryService)GetService(typeof(IDictionaryService));
			implementsInterfaceName = (string)dictService.GetValue("Implements");
        }

        #endregion

        #region ICodeModelEditorFilter Members

        bool ICodeModelEditorFilter.Filter(EnvDTE.CodeElement codeElement)
        {
            if (codeElement is CodeClass)
            {
                return !CheckImplements((CodeClass)codeElement);
            }
            else if (codeElement is CodeNamespace)
            {
                return !HasClassThatImplements((CodeNamespace)codeElement);
            }
            // Filter everything else
            return true;
        }

        #endregion
	}
}
