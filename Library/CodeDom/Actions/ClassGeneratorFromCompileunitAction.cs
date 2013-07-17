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
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using Microsoft.Practices.RecipeFramework.Library.CodeDom.Helpers;

namespace Microsoft.Practices.RecipeFramework.Library.CodeDom.Actions
{
    /// <summary>
    /// Generates class from CodeCompileUnit.
    /// </summary>
    public class ClassGeneratorFromCompileunitAction : ConfigurableAction
    {
        #region input properties

        private CodeCompileUnit compileUnit;
        /// <summary>
        /// CodeCompileUnit to generate the class
        /// </summary>
        [Input(Required=true)]
        public CodeCompileUnit CompileUnit
        {
            get { return compileUnit; }
            set { compileUnit = value; }
        }

        private LanguageType language;
        /// <summary>
        /// Language for the code provider
        /// </summary>
        [Input(Required=true)]
        public LanguageType Language
        {
            get { return language; }
            set { language = value; }
        }

        #endregion

        #region output properties

        private string _class;
        /// <summary>
        /// Class generated from the CodeCompileUnit using the provided Language
        /// </summary>
        [Output]
        public string Class
        {
            get { return _class; }
            set { _class = value; }
        }
        #endregion

        #region Action Member
        /// <summary>
        /// Generates class from the CodeCompileUnit and Language specified
        /// </summary>
        public override void Execute()
        {
            CodeDomProvider provider = ProviderFactory.CreateProvider(this.language);

            CodeGeneratorOptions options = new CodeGeneratorOptions();

            using(StringWriter tw = new StringWriter())
            {
                if(this.compileUnit.Namespaces.Count == 1)
                {
                    provider.GenerateCodeFromNamespace(this.compileUnit.Namespaces[0], tw, options);

                    this._class = tw.ToString();
                }
            }

        }

        /// <summary>
        /// This method do nothing
        /// </summary>
        public override void Undo()
        {
            //Do Nothing
        }
        #endregion

        private static class ProviderFactory
        {
            public static CodeDomProvider CreateProvider(LanguageType language)
            {
                if(language.Equals(LanguageType.cs))
                {
                    return new CSharpCodeProvider();
                }
                else if(language.Equals(LanguageType.vb))
                {
                    return new VBCodeProvider();
                }

                //TODO: Use resources for the exception message
                throw new NotSupportedException(string.Format("Language {0} not supported.", language.ToString()));
            }
        }
    }

}
