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
using System.Xml;
using System.Collections;
using System.Globalization;
using System.ComponentModel.Design;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Actions
{
	/// <summary>
	/// Adds all bound references specified in the input <see cref="Template"/> 
	/// to the current context.
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	[ServiceDependency(typeof(IAssetReferenceService))]
	public sealed class AddTemplateReferencesAction : Action
	{
		#region Input Properties

		/// <summary>
		/// The template been unfolded
		/// </summary>
		[Input(true)]
		public IVsTemplate Template
		{
			get { return template; }
			set { template = value; }
		} IVsTemplate template;

		/// <summary>
		/// The root in the solution explorer where a multiproject template is been unfolded
		/// </summary>
		[Input]
		public Project Root
		{
			get { return root; }
			set { root = value; }
		} Project root;

		/// <summary>
		/// The project where a project template is been unfolded.
		/// </summary>
		[Input]
		public Project Project
		{
			get { return project; }
			set { project = value; }
		} Project project;


		/// <summary>
		/// The item where an item template is been unfolded.
		/// </summary>
		[Input]
		public ProjectItem Item
		{
			get { return item; }
			set { item = value; }
		} ProjectItem item;

		/// <summary>
		/// The replacement dictionary passed in the <see cref="IWizard.RunStarted"/>
		/// </summary>
		[Input]
		public IDictionaryService ReplacementDictionary
		{
			get { return replacements; }
			set { replacements = value; }
		} IDictionaryService replacements;

		/// <summary>
		/// The package where the template lives.
		/// </summary>
		[Input]
		public GuidancePackage Package
		{
			get { return package; }
			set { package = value; }
		} GuidancePackage package;

		#endregion

		#region Action members

		private ArrayList referencesAdded;

		/// <summary>
		/// <seealso cref="IAction.Execute"/>
		/// </summary>
		public override void Execute()
		{
			if (Template.ExtensionData.References == null)
			{
				return;
			}
			int length = 0;
			if (Template.ExtensionData.References.RecipeReference != null)
			{
				length = Template.ExtensionData.References.RecipeReference.Length;
			}
			if (Template.ExtensionData.References.TemplateReference != null)
			{
				length += Template.ExtensionData.References.TemplateReference.Length;
			}

			referencesAdded = new ArrayList(length);

			IAssetReferenceService referenceService = GetService<IAssetReferenceService>(true);
			StringBuilder errorList = new StringBuilder();
			if (this.Template.ExtensionData.References.RecipeReference != null)
			{
				foreach (Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference reference in Template.ExtensionData.References.RecipeReference)
				{
					Hashtable hashInitialState;
					try
					{
						hashInitialState = ReadReferenceState(reference.InitialState);
					}
					catch (Exception ex)
					{
						errorList.AppendFormat(
							CultureInfo.CurrentCulture,
							Properties.Resources.Templates_InitialStateError,
							reference.Name, 
							DteHelper.ReplaceParameters(reference.Target, this.ReplacementDictionary), 
							ex);
						// Don't add the offending reference.
						continue;
					}
					VsBoundReference vsTarget = this.GetReferenceTarget(reference.Target, reference.Name);
					if (vsTarget != null)
					{
						try
						{
							referenceService.Add(vsTarget, hashInitialState);
							referencesAdded.Add(vsTarget);
						}
						catch (Exception ex)
						{
							errorList.AppendFormat(ex.Message).AppendLine();
						}
					}
					else
					{
						errorList.AppendFormat(
							CultureInfo.CurrentCulture,
							Properties.Resources.Templates_CantFindTarget,
							DteHelper.ReplaceParameters(reference.Target, this.ReplacementDictionary),
							reference.Name).AppendLine();
					}
				}
			}
			if (this.Item != null && this.Template.ExtensionData.References.TemplateReference != null)
			{
				throw new ArgumentException(String.Format(
					CultureInfo.CurrentCulture,
					Properties.Resources.Templates_ItemCantHaveTemplates,
					Path.GetFileName(this.Template.FileName)),
					"TemplateReference");
			}
			if (this.Template.ExtensionData.References.TemplateReference != null)
			{
				string basePath = this.Package.BasePath;
				foreach (VsTemplate.AssetReference reference in this.Template.ExtensionData.References.TemplateReference)
				{
					Hashtable hashInitialState;
					try
					{
						hashInitialState = ReadReferenceState(reference.InitialState);
					}
					catch (Exception ex)
					{
						errorList.AppendFormat(
							CultureInfo.CurrentCulture,
							Properties.Resources.Templates_InitialStateError,
							reference.Name, reference.Target, ex);
						continue;
					}
					string templateFileName = basePath + @"\Templates\" + reference.Name;
                    templateFileName = new CompatibleUri(templateFileName).LocalPath;
                    // Normalize path as it may contain double back slashes which usually shouldn't hurt but will break some of the checkings GAX does against registry keys
                    // this is necessary due to Uri.LocalPath behaving differently under Vista -- reported as VS bug #
                    //templateFileName = WinXpUriLocalPathBehavior(
                    //templateFileName = templateFileName.Replace(@"\\", @"\");
					VsBoundReference vsTarget = this.GetReferenceTarget(reference.Target, templateFileName);
					if (File.Exists(templateFileName) && templateFileName.EndsWith(".vstemplate", StringComparison.InvariantCultureIgnoreCase) && vsTarget != null)
					{
						BoundTemplateReference tmpref = new BoundTemplateReference(templateFileName, vsTarget);
						referencesAdded.Add(tmpref);
						referenceService.Add(tmpref, hashInitialState);
					}
					else if (vsTarget == null)
					{
						errorList.Append(String.Format(
							CultureInfo.CurrentCulture,
							Properties.Resources.Templates_CantFindTarget,
							DteHelper.ReplaceParameters(reference.Target, this.ReplacementDictionary),
							reference.Name));
						errorList.Append(Environment.NewLine);
					}
					else
					{
						errorList.Append(String.Format(
							CultureInfo.CurrentCulture,
							Properties.Resources.Template_DoesntExist,
							reference.Name));
						errorList.Append(Environment.NewLine);
					}
				}
			}
			if (errorList.Length > 0)
			{
				// Enclose full list of errors in a simpler message for display in the dialog.
				throw new RecipeFrameworkException(string.Format(CultureInfo.CurrentCulture,
					Properties.Resources.Templates_ErrorsProcessingReferences,
					this.Template.Name),
					new ArgumentException(errorList.ToString()));
			}
		}

		/// <summary>
		/// <seealso cref="IAction.Undo"/>
		/// </summary>
		public override void Undo()
		{
			IAssetReferenceService referenceService = GetService<IAssetReferenceService>(true);
			foreach (IAssetReference asset in this.referencesAdded)
			{
				referenceService.Remove(asset);
			}
			referencesAdded.Clear();
		}

		#endregion

		private Hashtable ReadReferenceState(StateEntry[] initalState)
		{
			Hashtable hashInitialState = null;
			if (initalState != null)
			{
				hashInitialState = new Hashtable();
				foreach (StateEntry entry in initalState)
				{
					entry.Key = NormalizeTextNodes(entry.Key);
					entry.Value = NormalizeTextNodes(entry.Value);

					object key = entry.Key;
					object value = entry.Value;
					// Both default to string if no other type is provided.
					if (key is XmlText)
					{

					}
					if (key is string)
					{
						key = DteHelper.ReplaceParameters((string)entry.Key, this.ReplacementDictionary);
					}
					if (value is string)
					{
						value = DteHelper.ReplaceParameters((string)entry.Value, this.ReplacementDictionary);
					}

					hashInitialState.Add(key, value);
				}
			}
			return hashInitialState;
		}

		private object NormalizeTextNodes(object data)
		{
			if (data is XmlText)
			{
				return ((XmlText)data).Value;
			}
			else if (data is XmlNode[])
			{
				StringBuilder value = new StringBuilder();
				foreach (XmlNode node in (XmlNode[])data)
				{
					// Need to convert everything to string, 
					// otherwise the value will not serialize.
					value.Append(node.OuterXml);
				}

				return value.ToString();
			}

			return data;
		}

		private VsBoundReference GetReferenceTarget(string target, string recipeName)
		{
			VsBoundReference vsTarget = null;
			DTE dte = GetService<DTE>(true);
			target = DteHelper.ReplaceParameters(target, this.ReplacementDictionary);
			// Special case for "/" value.
			if (target.Equals("/") || target.Equals("\\"))
			{
				// "/" is the solution if we're in a solution template, the 
				// project it this is a project template or the item if it's an item template.
				if (this.Project != null)
				{
					vsTarget = new ProjectReference(recipeName, this.Project);
				}
				else if (this.Item != null)
				{
					vsTarget = new ProjectItemReference(recipeName, this.Item);
				}
				else
				{
					vsTarget = new SolutionReference(recipeName, dte.Solution);
				}
			}
			else if (template.VSKind == WizardRunKind.AsMultiProject)
			{
				string pathToTarget = target.Substring(1);
				if (root != null && root.Object is SolutionFolder)
				{
					pathToTarget = DteHelper.BuildPath(root) + target;
				}
				ProjectItem prItem = DteHelper.FindItemByPath(dte.Solution, pathToTarget);
				if (prItem != null)
				{
					vsTarget = new ProjectItemReference(recipeName, prItem);
				}
				else
				{
					// Try finding a project.
					Project prj = DteHelper.FindProjectByPath(dte.Solution, pathToTarget);
					if (prj != null)
					{
						vsTarget = new ProjectReference(recipeName, prj);
					}
				}
			}
			else if (template.VSKind == WizardRunKind.AsNewProject && this.Project != null)
			{
				ProjectItem prItem = DteHelper.FindItemByPath(dte.Solution,
					DteHelper.BuildPath(this.Project) + target);
				// Can only refer to items.
				if (prItem != null)
				{
					vsTarget = new ProjectItemReference(recipeName, prItem);
				}
			}
			else
			{
				// We got here because there's an Item template that contains an assetname other than "\".
				throw new ArgumentException(String.Format(
					CultureInfo.CurrentCulture,
					Properties.Resources.Templates_ItemTargetInvalid,
					Path.GetFileName(template.FileName), target), "Target");
			}
			return vsTarget;
		}
	}
}
