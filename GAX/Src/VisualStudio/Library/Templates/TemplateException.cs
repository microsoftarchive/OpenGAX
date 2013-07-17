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
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates
{
    /// <summary>
	/// Represents errors that happen during execution of the <see cref="TextTemplateAction"/>.
    /// </summary>
    [Serializable]
	public class TemplateException : Exception
    {
        private CompilerErrorCollection compilerErrors;
        string renderedErrors;

		/// <summary>
		/// Initializes the exception with the list of compilation errors.
		/// </summary>
        public TemplateException(CompilerErrorCollection compilerErrors)
        {
            this.compilerErrors = compilerErrors;
        }

		/// <summary>
		/// The compilation errors that caused this exception.
		/// </summary>
        public CompilerErrorCollection CompilerErrors
        {
            set { this.compilerErrors = value; }
            get { return this.compilerErrors; }
        }

		/// <summary>
		/// Provides a string representation of all the compilation errors in the exception.
		/// </summary>
        public override string ToString()
        {
            if (renderedErrors == null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (CompilerError error in this.compilerErrors)
                {
                    sb.Append(error.ErrorText);
                    sb.Append(Environment.NewLine);
                    sb.Append(new string('-', 100));
                    sb.Append(Environment.NewLine);
                }
                renderedErrors = sb.ToString();
            }

            return renderedErrors;
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        protected TemplateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
