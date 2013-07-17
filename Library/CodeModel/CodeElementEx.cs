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
using EnvDTE;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel
{
    /// <summary>
    /// A <see cref="CodeElement"/> with dynamic invocation capabilities
    /// </summary>
	public class CodeElementEx: DispatchObject<CodeElement>
	{
        /// <summary>
        /// Required constructor
        /// </summary>
        /// <param name="ce"></param>
        public CodeElementEx(CodeElement ce)
            : base(ce)
        {
        }

        /// <summary>
        /// Gets the child elements of this CodeElement
        /// </summary>
        public CodeElements Members
        {
            get
            {
                return (CodeElements)this["Members"];
            }
        }

        /// <summary>
        /// DocComment property
        /// </summary>
        public string DocComment
        {
            get
            {
                return (string)this["DocComment"];
            }
            set
            {
                this["DocComment"] = value;
            }
        }

        /// <summary>
        /// Gets the attributes of this CodeElement
        /// </summary>
        public CodeElements Attributes
        {
            get
            {
                return (CodeElements)this["Attributes"];
            }
        }

		/// <summary>
		/// Gets the set of base classes of this CodeElement
		/// </summary>
		public CodeElements Bases
		{
			get
			{
				return (CodeElements)this["Bases"];
			}
		}

        /// <summary>
        /// Adds a <see cref="CodeAttribute"/>
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public CodeAttribute AddAttribute(string attributeName,string attributeValue,object position)
        {
            return (CodeAttribute)
                Invoke("AddAttribute", attributeName, attributeValue, position);
        }
	}
}
