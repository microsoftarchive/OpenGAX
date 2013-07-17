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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ComponentModel;
using System.ComponentModel.Design;
using EnvDTE80;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Library.CodeModel;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel.Editors.Filters
{
    /// <summary>
    /// Filters element containing the attribute specifed in "HasAttribute"
    /// </summary>
    [ServiceDependency(typeof(IDictionaryService))]
	public class ContainsAttribute: SitedComponent, ICodeModelEditorFilter 
	{
        #region Private Fields

        private Type attributeType = null;
        private AttributeTargets attributeUsage = (AttributeTargets)0;

        #endregion

        #region Overrides

        /// <summary>
        /// Get's the configured parameter "HasAttribute"
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            IDictionaryService dictService =
                (IDictionaryService)GetService(typeof(IDictionaryService));
            string attributeName = (string)dictService.GetValue("ContainsAttribute");
            ITypeResolutionService typeResService = 
                (ITypeResolutionService)GetService(typeof(ITypeResolutionService));
            attributeType = typeResService.GetType(attributeName,false);
            if (attributeType != null)
            {
                AttributeUsageAttribute[] usages=
                    (AttributeUsageAttribute[])Attribute.GetCustomAttributes(attributeType, typeof(AttributeUsageAttribute));
                foreach(AttributeUsageAttribute usage in usages)
                {
                    attributeUsage |= usage.ValidOn;
                }
            }
        }

        #endregion

        #region Private implementation

        private AttributeTargets GetTarget(CodeElement codeElement)
        {
            if (codeElement is CodeClass)
            {
                return AttributeTargets.Class;
            }
            else if (codeElement is CodeStruct)
            {
                return AttributeTargets.Struct;
            }
            else if (codeElement is CodeInterface)
            {
                return AttributeTargets.Interface;
            }
            else if (codeElement is CodeEvent)
            {
                return AttributeTargets.Event;
            }
            else if (codeElement is CodeEnum)
            {
                return AttributeTargets.Enum;
            }
            else if (codeElement is CodeDelegate)
            {
                return AttributeTargets.Delegate;
            }
            else if (codeElement is CodeFunction)
            {
                return AttributeTargets.Method;
            }
            else if (codeElement is CodeProperty)
            {
                return AttributeTargets.Property;
            }
            else if (codeElement is CodeVariable)
            {
                return AttributeTargets.Field;
            }
            return (AttributeTargets)0;
        }

        #endregion

        #region ICodeModelEditorFilter Members

        bool ICodeModelEditorFilter.Filter(EnvDTE.CodeElement codeElement)
        {
            AttributeTargets target=GetTarget(codeElement);
            if ( (target & this.attributeUsage) != 0 )
            {
                CodeElements attributes = new CodeElementEx(codeElement).Attributes;
                foreach (CodeElement attribute in attributes)
                {
                    try
                    {
                        if (attribute.FullName.Equals(
                                this.attributeType.FullName, 
                                StringComparison.InvariantCultureIgnoreCase))
                        {
                            return false;
                        }
                    }
                    catch
                    {
                    }
                }
                return true;
            }
            return false;
        }

        #endregion
}
}
