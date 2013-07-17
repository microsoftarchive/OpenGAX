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
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Library.CodeModel;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel.Actions
{
    /// <summary>
    /// Searches for a <see cref="CodeElement"/> child in a <see cref="CodeElement"/> container
    /// </summary>
    public class GetMemberAction : ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// The <see cref="CodeElement"/> container
        /// </summary>
        [Input(Required=true)]
        public CodeElement ParentElement
        {
            get { return parentElement; }
            set { parentElement = value; }
        } CodeElement parentElement;

        /// <summary>
        /// The name of the element to search for
        /// </summary>
        [Input(Required=true)]
        public string ElementName
        {
            get { return name; }
            set { name = value; }
        } string name;

        #endregion

        #region Output Properties

        /// <summary>
        /// The found <see cref="CodeElement"/> object
        /// </summary>
        [Output]
        public CodeElement CodeElement
        {
            get { return element; }
            set { element = value; }
        } CodeElement element;

        #endregion

        #region Action members

        /// <summary>
        /// Searches for the <see cref="CodeElement"/>
        /// </summary>
        public override void Execute()
        {
            CodeElements elements = new CodeElementEx(this.ParentElement).Members;
            foreach (CodeElement codeElement in elements)
            {
                if (codeElement.Name.Equals(this.ElementName, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.CodeElement = codeElement;
                    return;
                }
            }
        }

        /// <summary>
        /// Sets the found element to null
        /// </summary>
        public override void Undo()
        {
            this.CodeElement = null;
        }

        #endregion
    }
}

