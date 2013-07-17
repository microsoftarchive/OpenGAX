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

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel.Actions
{
    /// <summary>
    /// Adds a new method into the specified code element class
    /// </summary>
    public class AddMethodAction : ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// The class where the new will be added
        /// </summary>
        [Input(Required=true)]
        public CodeClass Class
        {
            get { return codeClass; }
            set { codeClass = value; }
        } CodeClass codeClass;

        /// <summary>
        /// The kind of method to add
        /// </summary>
        /// <seealso cref="vsCMFunction"/>
        [Input(Required=false)]
        public vsCMFunction MethodKind
        {
            get { return methodKind; }
            set { methodKind = value; }
        } vsCMFunction methodKind = (vsCMFunction)vsCMFunction.vsCMFunctionFunction;

        /// <summary>
        /// The name of the new method
        /// </summary>
        [Input(Required=true)]
        public string MethodName
        {
            get { return name; }
            set { name = value; }
        } string name;

        /// <summary>
        /// The return type of the new method
        /// </summary>
        [Input(Required=false)]
        public object ReturnType
        {
            get { return retType; }
            set { retType = value; }
        } object retType = vsCMTypeRef.vsCMTypeRefVoid;

        /// <summary>
        /// The position in the current class where the new method is going to be added
        /// </summary>
        [Input(Required=false)]
        public object Position
        {
            get { return position;  }
            set { position = value;  }
        } object position = 0;

        /// <summary>
        /// The access kind of the new method
        /// </summary>
        /// <seealso cref="vsCMAccess"/>
        [Input(Required=false)]
        public vsCMAccess Access
        {
            get { return access; }
            set { access = value; }
        } vsCMAccess access = vsCMAccess.vsCMAccessPublic;

        #endregion

        #region Output Properties

        /// <summary>
        /// The code element of the new method
        /// </summary>
        [Output]
        public CodeFunction Method
        {
            get { return method; }
            set { method = value; }
        } CodeFunction method;

        #endregion

        #region Action members

        private const string CSharpComment = "//TODO: Add your code here";
        private const string VBComment = "Rem Add your code here";

        internal static string GetComment(string language)
        {
            if (language.Equals(CodeModelLanguageConstants.vsCMLanguageCSharp, StringComparison.InvariantCultureIgnoreCase))
            {
                return CSharpComment;
            }
            else if (language.Equals(CodeModelLanguageConstants.vsCMLanguageVB, StringComparison.InvariantCultureIgnoreCase))
            {
                return VBComment;
            }
            else
            {
                throw new NotSupportedException("Language not supported");
            }
        }

        internal static void AddExpansionComment(CodeFunction codeFunction)
        {
            EditPoint edPoint = codeFunction.EndPoint.CreateEditPoint();
            edPoint.LineUp(2);
            if (edPoint.GreaterThan(codeFunction.StartPoint))
            {
                edPoint.Insert(GetComment(codeFunction.Language));
            }
        }

        /// <summary>
        /// <see cref="IAction.Execute"/>
        /// </summary>
        public override void Execute()
        {
            this.Method = this.Class.AddFunction(
                this.MethodName,
                this.MethodKind,
                this.ReturnType,
                this.Position,
                this.Access,
                null);
            AddExpansionComment(this.Method);
        }

        /// <summary>
        /// <see cref="IAction.Undo"/>
        /// </summary>
        public override void Undo()
        {
            this.Class.RemoveMember(this.MethodName);
        }

        #endregion
    }
}
