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
using System.Runtime.InteropServices;
using Microsoft.Practices.RecipeFramework.Library.Actions;
using Microsoft.Practices.RecipeFramework.Library.CodeModel;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel.Actions
{
    /// <summary>
    /// Sets the DocComment property of a <see cref="CodeElement"/> object
    /// </summary>
    public class SetDocCommentAction : ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// The <see cref="CodeElement"/> object whose property is been set
        /// </summary>
        [Input(Required=true)]
        public CodeElement CodeElement
        {
            get { return codeElement.RealObject; }
            set { codeElement = new CodeElementEx(value); }
        } CodeElementEx codeElement;

        /// <summary>
        /// The value of the DocComment
        /// </summary>
        [Input(Required=true)]
        public string DocComment
        {
            get { return docComment; }
            set { docComment = value; }
        } string docComment;

        #endregion

        #region Output Properties

        #endregion

        #region Action members

        private string oldValue =string.Empty;

        /// <summary>
        /// Sets the DocComment property
        /// </summary>
        public override void Execute()
        {
            oldValue = this.codeElement.DocComment;
            this.codeElement.DocComment = this.DocComment;
        }

        /// <summary>
        /// Set the old DocComment value
        /// </summary>
        public override void Undo()
        {
            if (!string.IsNullOrEmpty(oldValue))
            {
                this.codeElement.DocComment = oldValue;
            }
        }

        #endregion
    }
}

