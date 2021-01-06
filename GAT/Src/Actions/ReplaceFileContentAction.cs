//===================================================================================
// Microsoft patterns & practices
// Guidance Automation Toolkit
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
using System.Linq;
using System.Collections.Generic;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using System.Diagnostics;
using Microsoft.Practices.RecipeFramework.Library;
using System.IO;

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.Actions
{
	/// <summary>
	/// Performs a search and replace action over a file
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	public class ReplaceFileContentAction : ConfigurableAction
	{
		/// <summary>
		/// The target project
		/// </summary>
		[Input(Required = true)]
		public Project Project { get; set; }

		[Input]
		public string Path { get; set; }

		[Input]
		public string OldText { get; set; }

		[Input]
		public string NewText { get; set; }

		/// <summary>
		/// Does nothing, as un-registration must be done explicitly.
		/// </summary>
		public override void Undo()
		{
			// Must un-register to undo.
		}

		/// <summary>
		/// Performs a search and replace action over a file
		/// </summary>
		public override void Execute()
		{
			// TODO: Move to resources
			TraceUtil.TraceInformation(this, string.Format("Replacing '{0}' by '{1}' on path '{2}'",
				this.OldText,
				this.NewText,
				string.IsNullOrEmpty(this.Path) ? string.Empty : this.Path));

			foreach (ProjectItem item in Utils.FindProjectItems(this.Project.ProjectItems, 
				p => string.IsNullOrEmpty(this.Path) || p.Name.EndsWith(this.Path, StringComparison.InvariantCultureIgnoreCase)))
			{
				if(EnsureWritable(item.get_FileNames(1)))
				{
					var content = File.ReadAllText(item.get_FileNames(1));

					content = content.Replace(OldText, NewText);

					File.WriteAllText(item.get_FileNames(1), content);						
				}
			}
		}

		bool EnsureWritable(String filePath)
		{
			DTE vs = (DTE)GetService(typeof(DTE));
			if (File.Exists(filePath))
			{
				if (vs.SourceControl.IsItemUnderSCC(filePath) &&
					!vs.SourceControl.IsItemCheckedOut(filePath))
				{
					bool checkout = vs.SourceControl.CheckOutItem(filePath);
					if (!checkout)
					{
						TraceUtil.TraceWarning(this, string.Format("Cannot check out {0} file.", filePath));

						return false;
					}
				}
				else
				{
					// perform an extra check if the file is read only.
					if (IsReadOnly(filePath))
					{
						ResetReadOnly(filePath);
					}
				}
			}

			return true;
		}

		bool IsReadOnly(string filePath)
		{
			return (File.GetAttributes(filePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
		}

		void ResetReadOnly(string filePath)
		{
			File.SetAttributes(filePath, File.GetAttributes(filePath) ^ FileAttributes.ReadOnly);
		}
	}
}