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
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Collections;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.TextTemplating;

using Microsoft.Practices.Common;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.ComponentModel;
using System.CodeDom.Compiler;
using System.Collections.Generic;


namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates
{
	/// <summary>
	/// The action unfolds a T4 template and returns the result as a string
	/// in the output property called Content. The action has one input property
	/// called Template - path to the template file.
	/// </summary>
	/// <remarks>
	/// Receives a <see cref="Template"/> input , and leaves the rendered 
	/// output in the <see cref="Content"/> output parameter.
	/// <para>
	/// The <see cref="Template"/> parameter path must resolve to a location 
	/// under the package root folder. The "Templates" subfolder is automatically 
	/// appended to the beginning of the path if it's not a full path, as templates 
	/// should be placed in that subfolder under the package, alongside the VS templates.
	/// </para>
	/// </remarks>
	[ServiceDependency(typeof(ITypeResolutionService))]
	public sealed class TextTemplateAction : T4Action
	{
		#region Input Properties

		/// <summary>
		/// Template input file name
		/// </summary>
		[Input(true)]
		public string Template
		{
			get { return template; }
			set { template = value; }
		} string template;

		#endregion

		#region Output Properties

		/// <summary>
		/// Rendered output file
		/// </summary>
		[Output]
		public string Content
		{
			get { return rendered; }
			set { rendered = value; }
		} string rendered;

		#endregion

		#region IAction Members

		/// <summary>
		/// Performs the unfolding with the given parameters
		/// </summary>
		public override void Execute()
		{
            // fields
            string templateFile = this.Template;
            string templateCode = string.Empty;

            // Get the template code.
            if (templateFile == null)
            {
                throw new ArgumentNullException("Template");
            }

            // Get the package root folder.
            string basePath = this.GetTemplateBasePath();

            // if templateFile is rooted use it
            // else combine it with the templatePath.
            if (!Path.IsPathRooted(templateFile))
            {
                templateFile = Path.Combine(basePath, templateFile);
            }

            // Resolve relative path movements (..\..\..)
            templateFile = new FileInfo(templateFile).FullName;

            // Validate if the templateFile was in
            // the package root folder
            if (!templateFile.StartsWith(basePath))
            {
                throw new ArgumentException(
                    Properties.Resources.TextTemplateAction_TemplatePathNotInPackageFolder);
            }
            templateCode = File.ReadAllText(templateFile);

            this.Content = Render(templateCode, templateFile);
        }

		/// <summary>
		/// Undoes the unfolding, not implemented
		/// </summary>
		public override void Undo()
		{
			// No undo implemented.
		}

		#endregion
	}
}
