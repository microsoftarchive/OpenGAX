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
using Microsoft.VisualStudio.TextTemplating;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Globalization;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates
{
	/// <summary>
	/// Implementation of <see cref="DirectiveProcessor"/> that generates
	/// properties code and returns the result as a string.
	/// </summary>
	/// <remarks>
	/// The method ProcessDirective() can be invoked several times between 
	/// StartProcessingRun() and FinishProcessingRun().
	/// The method GetClassCodeForProcessingRun() returns the properties code for
	/// all the ProcessDirective() calls.
	/// </remarks>
	internal class PropertiesDirectiveProcessor : DirectiveProcessor
	{
		private const string PropertyDirectiveProcessorName = "PropertyProcessor";
		private const string PropertyDirectiveName = "property";
		private const string NameAttribute = "name";
		private const string TypeAttribute = "type";
		private const string ConverterAttribute = "converter";
		private const string EditorAttribute = "editor";

		private TemplateHost host;
		private System.CodeDom.Compiler.CodeDomProvider languageProvider;
		private StringWriter codeWriter;

        /// <summary>
        /// Initialize the host.
        /// </summary>
        /// <param name="host"></param>
        public override void Initialize(ITextTemplatingEngineHost host)
        {
            base.Initialize(host);

            if (host == null)
            {
                throw new ArgumentNullException("host");
            }

            this.host = (TemplateHost)host;
        }

        /// <summary>
        /// Return if the directive is supported.
        /// </summary>
        /// <param name="directiveName"></param>
        /// <returns></returns>
		public override bool IsDirectiveSupported(string directiveName)
		{
			if (directiveName == null)
			{
				throw new ArgumentNullException("directiveName");
			}
			if (string.Compare(directiveName, PropertyDirectiveName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return true;
			}
			return false;
		}

        /// <summary>
        /// Initializes the properties code writer.
        /// </summary>
        public override void StartProcessingRun(CodeDomProvider languageProvider, string templateContents, CompilerErrorCollection errors)
        {
            base.StartProcessingRun(languageProvider, templateContents, errors);

             if (languageProvider == null)
             {
                 throw new ArgumentNullException("languageProvider");
             }
             this.codeWriter = new StringWriter(CultureInfo.CurrentCulture);
             this.languageProvider = languageProvider;
        }

		/// <summary>
		/// Generates the property code (with his getter) for the processed directive.
		/// The return value of the property is obtained from the <see cref="TemplateHost.Arguments"/>.
		/// </summary>
		public override void ProcessDirective(string directiveName, IDictionary<string, string> arguments)
		{
			Guard.ArgumentNotNull(directiveName, "directiveName");
			Guard.ArgumentNotNull(arguments, "arguments");

			if (String.Compare(directiveName, PropertyDirectiveName, true) != 0)
			{
				// this class do not know the directiveName requested.
				return;
			}

			if (this.host.Arguments.ContainsKey(arguments[NameAttribute]) == false)
			{
				throw new ArgumentNullException(arguments[NameAttribute], String.Format(
					CultureInfo.CurrentCulture,
					Properties.Resources.PropertiesDirectiveProcessor_UndefinedValue,
					NameAttribute));
			}

			// Create the property code generator
			CodeMemberProperty property = new CodeMemberProperty();
			property.Name = arguments[NameAttribute];

			// Type attribute in the template always overrides the type defined in the recipe.
			// Types should be assignable or this will throw when the template is rendered.
			// We cannot check that they are assignable here, because Imports and Assembly references 
			// can exist in the template that we do not have at this point and cannot load into 
			// this domain.
			if (arguments.ContainsKey(TypeAttribute))
			{
				property.Type = new CodeTypeReference(arguments[TypeAttribute]);
			}
			else
			{
				property.Type = new CodeTypeReference(this.host.Arguments[property.Name].Type);
			}

			// Emit converter and editor attributes if specified. This will allow integration with VS to 
			// test templates by setting their property values with the Property Browser.
			if (arguments.ContainsKey(ConverterAttribute))
			{
				property.CustomAttributes.Add(
					new CodeAttributeDeclaration(
						new CodeTypeReference(typeof(TypeConverterAttribute)),
						new CodeAttributeArgument(
							new CodeTypeOfExpression(
								arguments[ConverterAttribute]))));
			}
			if (arguments.ContainsKey(EditorAttribute))
			{
				property.CustomAttributes.Add(
					new CodeAttributeDeclaration(
						new CodeTypeReference(typeof(EditorAttribute)),
						new CodeAttributeArgument(
							new CodeTypeOfExpression(
								arguments[EditorAttribute]))));
			}

			// Make property public so that VS integration can inspect the template class for 
			// public properties that can be set.
			property.Attributes = MemberAttributes.Public | MemberAttributes.Final;

			// Mark the property as auto-generated from the template. Allows improvement to the VS integration 
			// with the Property Browser.
			property.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeTypeReference(typeof(TemplatePropertyAttribute))));

			property.GetStatements.Add(
				new CodeMethodReturnStatement(
					new CodeCastExpression(property.Type,
						new CodePropertyReferenceExpression(
							new CodeIndexerExpression(
								new CodePropertyReferenceExpression(
									new CodePropertyReferenceExpression(
										new CodeTypeReferenceExpression(typeof(TemplateHost)),
										"CurrentHost"),
									"Arguments"),
								new CodePrimitiveExpression(property.Name)),
							"Value"))));


			CodeGeneratorOptions opt = new CodeGeneratorOptions();
			opt.BracingStyle = "C";
			this.languageProvider.GenerateCodeFromMember(property, this.codeWriter, opt);
		}

		public override void FinishProcessingRun()
		{
		}

		/// <summary>
		/// Returns the properties code for all the ProcessDirective() calls.
		/// </summary>
		public override string GetClassCodeForProcessingRun()
		{
			if (codeWriter == null)
			{
				throw new InvalidOperationException(Properties.Resources.PropertiesDirectiveProcessor_StartProcessingRunNotInvoked);
			}
			else
			{
				return this.codeWriter.ToString();
			}
		}

		public override string[] GetReferencesForProcessingRun()
		{
			return new string[] { this.GetType().Assembly.Location };
		}

		public override string[] GetImportsForProcessingRun()
		{
			return null;
		}

		public override string GetPostInitializationCodeForProcessingRun()
		{
			return null;
		}

		public override string GetPreInitializationCodeForProcessingRun()
		{
			return null;
		}
	}
}
