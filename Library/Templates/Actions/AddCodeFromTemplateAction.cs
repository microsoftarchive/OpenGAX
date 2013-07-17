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
using System.IO;
using System.Text;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Design.Serialization;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Templates.Actions
{
	/// <summary>
	/// Unfolds a T4 template and replaces a <see cref="CodeModel"/> element
	/// </summary>
	[ServiceDependency(typeof(ITypeResolutionService))]
	public class AddCodeFromTemplateAction : T4Action
	{
		#region Input Properties

		/// <summary>
		/// The <see cref="CodeElement"/> been replaced by the template
		/// </summary>
		[Input(Required=true)]
		public CodeElement CodeElement
		{
			get { return codeElement; }
			set { codeElement = value; }
		} CodeElement codeElement;

		/// <summary>
		/// The T4 template file name, must be placed under the Templates\CodeSnippets folder
		/// </summary>
		[Input(Required=true)]
		public string Template
		{
			get { return template; }
			set { template = value; }
		} string template;

		#endregion

		#region Output Properties

		#endregion

		#region Action Members

		private string GetTemplate(out string fileName)
		{
			// Path is always relative to package root folder.
			// WARNING: if we were to copy our files and zip them, this may not work.
			TypeResolutionService resolver = GetService(typeof(ITypeResolutionService))
				as TypeResolutionService;
			if (resolver == null)
			{
				throw new ArgumentNullException("TypeResolutionService");
			}
			if (resolver.BasePath == null)
			{
				throw new ArgumentNullException("TypeResolutionService.BasePath");
			}
			fileName = new FileInfo(Path.Combine(
				resolver.BasePath, Template)).FullName;
			return new StreamReader(fileName).ReadToEnd();
		}

		private EditPoint editStartPoint = null;
		private EditPoint editEndPoint = null;
		private string oldText = string.Empty;

		/// <summary>
		/// Unfolds the template
		/// </summary>
		public override void Execute()
		{
			TextPoint textStartPoint = codeElement.StartPoint;
			editStartPoint = textStartPoint.CreateEditPoint();
			TextPoint textEndPoint = codeElement.EndPoint;
			editEndPoint = textEndPoint.CreateEditPoint();
			oldText = editStartPoint.GetText(editEndPoint);
			string templateFileName = string.Empty;
			string templateContent = GetTemplate(out templateFileName);
			editStartPoint.ReplaceText(
				editEndPoint,
				Render(templateContent, templateFileName),
				(int)vsEPReplaceTextOptions.vsEPReplaceTextAutoformat);
		}

		/// <summary>
		/// Reverts the unfolding
		/// </summary>
		public override void Undo()
		{
			editStartPoint.ReplaceText(editEndPoint, oldText, (int)vsEPReplaceTextOptions.vsEPReplaceTextAutoformat);
			oldText = string.Empty;
			editStartPoint = null;
			editEndPoint = null;
		}

		#endregion

	}
}
