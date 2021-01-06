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

#region Using directives

using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Templates
{
	/// <summary>
	/// Generic Wizard Extension for a VS template file (.vstemplate)
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	[ServiceDependency(typeof(IVsTemplatesService))]
	[System.ComponentModel.DesignerCategory("Code")]
	public class UnfoldTemplate : Component, IWizard
	{
		#region Private Fields

		internal IVsTemplate Template
		{
			get { return template; }
		} IVsTemplate template;
		GuidancePackage Package;
		bool didEnable = false;

		internal Project generatedProject;
		internal List<ProjectItem> generatedItems;
		Project unfoldingRoot;
		List<ProjectItem> openedItems;

		#endregion

		#region Default Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public UnfoldTemplate()
		{
			this.openedItems = new List<ProjectItem>();
			this.generatedItems = new List<ProjectItem>();
		}

		#endregion

		#region Properties

		// WORKAROUND: need to disable checks for valid references for subtemplates during 
		// child template expansion (i.e. during solution, for projects, and during project, for items).
		internal bool IsUnfolding
		{
			// It's set to true by the parent template, and reset by the VszWizard.
			get { return unfolding; }
			set { unfolding = value; }
		} bool unfolding = false;

		internal static Stack UnfoldingTemplates
		{
			get
			{
				if (unfoldingTemplates == null)
				{
					unfoldingTemplates = new Stack();
				}
				return unfoldingTemplates;
			}
		} static Stack unfoldingTemplates;

		internal IDictionaryService ReplacementDictionary
		{
			get { return templateDictionary; }
		}

		TemplateDictionaryService templateDictionary;

		internal ProjectItem FirstGeneratedItem
		{
			get
			{
				if (this.generatedItems.Count > 0)
				{
					return this.generatedItems[0];
				}
				return null;
			}
		}

		#endregion

		#region Implementation

		private void LoadRecipeManager(object automationObject)
		{
			IRecipeManagerService recipeManager = null;
			try
			{
				recipeManager = (IRecipeManagerService)ServiceHelper.GetService(
					new VsServiceProvider(automationObject), 
					typeof(IRecipeManagerService), this);
				recipeManager.Add(this);
			}
			catch (Exception e)
			{
				ErrorHelper.Show(this.Site, e, Properties.Resources.Templates_CannotLoadRecipeManager);
				throw new WizardCancelledException();
			}
		}

		private void LoadTemplateData(Dictionary<string, string> replacementsDictionary, string templateFileName)
		{
			try
			{
				templateFileName = new CompatibleUri(templateFileName).LocalPath;
				this.templateDictionary = new TemplateDictionaryService(replacementsDictionary);
				IVsTemplatesService templatesService =
					(IVsTemplatesService)GetService(typeof(IVsTemplatesService));
				this.template = templatesService.GetTemplate(templateFileName);
			}
			catch (Exception e)
			{
				ErrorHelper.Show(this.Site, e, Properties.Resources.Templates_InvalidWizardData);
				throw new WizardCancelledException();
			}
		}

		private void LoadPackage()
		{
			IRecipeManagerService recipeManager = (IRecipeManagerService)
				GetService(typeof(IRecipeManagerService));
			if (template.Kind == TemplateKind.Solution)
			{
				Package = recipeManager.GetPackage(template.PackageName);
				if (Package == null)
				{
					Package = recipeManager.EnablePackage(template.PackageName);
					didEnable = true;
				}
			}
			else
			{
				Package = recipeManager.GetPackage(template.PackageName);
			}
			if (Package == null)
			{
				string messageText = messageText = String.Format(
					CultureInfo.CurrentCulture,
					Properties.Resources.Templates_PackageNotActive,
					template.PackageName);
				ErrorHelper.Show((IServiceProvider)recipeManager, messageText);
				throw new WizardCancelledException();
			}
		}

		private void LoadInitialState()
		{
			if (this.template.ExtensionData.Recipe == null)
				return;
			DTE dte = (DTE)GetService(typeof(DTE));
			IAssetReferenceService referenceService = (IAssetReferenceService)
				this.Package.GetService(typeof(IAssetReferenceService), false);
			if (referenceService != null)
			{
				IAssetReference templateAsset = null;
				try
				{
					object target = DteHelper.GetTarget(dte);
					templateAsset = referenceService.GetReferenceFor(this.Template.FileName, target);
					if (templateAsset == null)
					{
						if (this.Template.Kind == TemplateKind.Project && target is Project) // If the template is been unfolded, then check the parent folder
						{
							Project parentProject = (target as Project).ParentProjectItem.ContainingProject;
							if (parentProject != null) // Parent folder exist, check the reference to the template
							{
								templateAsset = referenceService.GetReferenceFor(this.Template.FileName, parentProject);
							}
							else // The parent folder is the solution root, check the reference in the solutioin root
							{
								templateAsset = referenceService.GetReferenceFor(this.Template.FileName, dte.Solution);
							}
						}
					}
				}
				catch (Exception)
				{
					templateAsset = null;
				}
				if (templateAsset != null)
				{
					IPersistenceService persistenceService = (IPersistenceService)this.Package.GetService(typeof(IPersistenceService), false);
					if (persistenceService != null)
					{
						IDictionary hashStored = persistenceService.LoadState(this.Template.PackageName, templateAsset);
						if (hashStored != null)
						{
							ArrayList toremove = new ArrayList(hashStored.Count);
							foreach (DictionaryEntry keyValuePair in hashStored)
							{
								if (this.templateDictionary.GetValue(keyValuePair.Key) == null)
								{
									try
									{
										this.templateDictionary.SetValue(keyValuePair.Key, keyValuePair.Value);
									}
									catch
									{
										string appliesTo;
										try
										{
											appliesTo = templateAsset.AppliesTo;
										}
										catch (Exception e)
										{
											this.TraceWarning(e.Message);
											appliesTo = Properties.Resources.Reference_AppliesToThrew;
										}
										// Invalid initial state should just be ignored.
										this.TraceWarning(Properties.Resources.Template_IgnoreKeyInitialState, keyValuePair.Key, templateAsset.AppliesTo);
										toremove.Add(keyValuePair.Key);
									}
								}
							}
							foreach (object key in toremove)
							{
								hashStored.Remove(key);
							}
							if (toremove.Count != 0)
							{
								// Save the updated one without the offending values.
								persistenceService.SaveState(this.template.PackageName, templateAsset, hashStored);
							}
						}
					}
				}
			}
		}

		private void CheckConflictingArguments(UnfoldTemplate parentTemplate)
		{
			Configuration.Recipe parentRecipe = parentTemplate.RecipeConfig;
			if (RecipeConfig != null && parentRecipe != null && RecipeConfig.Arguments != null)
			{
				foreach (Configuration.Argument arg in RecipeConfig.Arguments)
				{
					foreach (Configuration.Argument argParent in parentRecipe.Arguments)
					{
						if (arg.Name.Equals(argParent.Name))
						{
							throw new RecipeFrameworkException(
								String.Format(CultureInfo.CurrentCulture,
									Properties.Resources.Templates_ArgumentsShared,
									arg.Name,
									RecipeConfig.Name,
									parentRecipe.Name));
						}
					}
				}
			}
		}

		private void LoadDictionary()
		{
			if (UnfoldingTemplates.Count > 0)
			{
				UnfoldTemplate parentTemplate = (UnfoldTemplate)UnfoldingTemplates.Peek();
				CheckConflictingArguments(parentTemplate);
				foreach (DictionaryEntry keyValuePair in parentTemplate.templateDictionary.State)
				{
					if (this.templateDictionary.GetValue(keyValuePair.Key) == null)
					{
						this.templateDictionary.SetValue(keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
			// After we load the Dictionary, now push ourselves into the unfolding templates stack
			UnfoldTemplate.UnfoldingTemplates.Push(this);
		}

		internal readonly static XmlElement NopWizardConfig = new XmlDocument().CreateElement("NopWizard");

		Configuration.Recipe RecipeConfig
		{
			get
			{
				if (recipeConfig == null)
				{
					if (this.template.ExtensionData.Recipe != null && Package != null)
					{
						IConfigurationService configService =
							(IConfigurationService)Package.GetService(typeof(IConfigurationService), true);
						recipeConfig = configService.CurrentPackage[this.template.ExtensionData.Recipe];
					}
				}
				return recipeConfig;
			}
		} Configuration.Recipe recipeConfig = null;

		private ExecutionResult ExecuteRecipe(bool executeActions)
		{
			if (RecipeConfig != null)
			{
				TemplateGatheringService templatesvc = new TemplateGatheringService(templateDictionary, executeActions);
				using (Package.SetupTemporaryService(typeof(IValueGatheringService), templatesvc))
				{
					try
					{
						if (RecipeConfig.GatheringServiceData == null)
						{
							RecipeConfig.GatheringServiceData = new Configuration.RecipeGatheringServiceData();
							RecipeConfig.GatheringServiceData.Any = NopWizardConfig;
						}
						//Package.Execute(this.template.ExtensionData.Recipe, this.templateDictionary.State);
                        Package.ExecuteFromTemplate(this.template.ExtensionData.Recipe, this.templateDictionary.State);
					}
					finally
					{
						if (RecipeConfig.GatheringServiceData != null && RecipeConfig.GatheringServiceData.Any == NopWizardConfig)
						{
                            RecipeConfig.GatheringServiceData = null;
							//RecipeConfig.GatheringServiceData.Any = null;
						}
					}
				}
				return templatesvc.Result;
			}
			return ExecutionResult.Failed;
		}

		private void ProcessReferences()
		{
			if (this.template.ExtensionData.References != null)
			{
				Actions.AddTemplateReferencesAction action = new Actions.AddTemplateReferencesAction();
				try
				{
					Package.Add(action);
					action.Package = this.Package;
					action.Root = this.unfoldingRoot;
					action.Project = this.generatedProject;
					action.Item = this.FirstGeneratedItem;
					action.Template = this.template;
					action.ReplacementDictionary = this.ReplacementDictionary;
					action.Execute();
				}
				catch (Exception ex)
				{
					// We will swallow the exception, the erroneous references did not get added
					ErrorHelper.Show(this.Site, ex);
				}
				finally
				{
					if (action != null)
					{
						Package.Remove(action);
						action.Dispose();
						action = null;
					}
				}
			}
		}

		#endregion

		#region IWizard Members

		/// <summary>
		/// See <see cref="IWizard.BeforeOpeningFile"/>.
		/// </summary>
		public virtual void BeforeOpeningFile(ProjectItem projectItem)
		{
			openedItems.Add(projectItem);
		}

		/// <summary>
		/// See <see cref="IWizard.ProjectFinishedGenerating(Project)"/>.
		/// </summary>
		public virtual void ProjectFinishedGenerating(Project project)
		{
			this.generatedProject = project;
		}

		/// <summary>
		/// See <see cref="IWizard.ProjectItemFinishedGenerating(ProjectItem)"/>.
		/// </summary>
		public virtual void ProjectItemFinishedGenerating(ProjectItem projectItem)
		{
			foreach (ProjectItem generatedItem in this.generatedItems)
			{
				if (generatedItem != null && generatedItem.ProjectItems != null &&
					 generatedItem.ProjectItems.Count > 0)
				{
					foreach (ProjectItem subItem in generatedItem.ProjectItems)
					{
						if (subItem == projectItem)
						{
							// projectItem is already contained in generatedItem, so dont update generatedItems
							return;
						}
					}
				}
			}
			// Assumes that the first element in the template becomes the "root" project item.
			this.generatedItems.Add(projectItem);
		}

		internal class Interop
		{
			public static bool Canceled(int hr) { return (hr == VSConstants.E_ABORT) || (hr == VSConstants.OLE_E_PROMPTSAVECANCELLED) || (hr == VSConstants.E_FAIL); }
			public static void ThrowAbort()
			{
				Marshal.ThrowExceptionForHR(VSConstants.E_ABORT);
			}
		}

		/// <summary>
		/// See <see cref="IWizard.RunStarted"/>.
		/// </summary>
		public virtual void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			try
			{
				if (automationObject == null)
				{
					throw new ArgumentNullException("automationObject");
				}
				if (replacementsDictionary == null)
				{
					throw new ArgumentNullException("replacementsDictionary");
				}
				LoadRecipeManager(automationObject);
				LoadTemplateData(replacementsDictionary, (string)customParams[0]);
				LoadPackage();
				LoadDictionary();
				LoadInitialState();
				if (!IsUnfolding)
				{
					IsUnfolding = true;
				}
				SetUnfoldingRoot();
				Package.TurnOnOutput();
				ExecutionResult result = ExecuteRecipe(false);
				if (result == ExecutionResult.Cancel)
				{
					throw new WizardCancelledException();
				}
			}
			// The TemplateWizard will catch the cancel exception and will not re-throw an exception
			// so the shell will not get failed hr and will assume that the wizard completed succesfully
			// and a project is opened
			// To really cancel the creation we will throw E_ABORT insted of WizardCancelledException
			catch (WizardCancelledException)
			{
				RestoreTemplateState();
				Interop.ThrowAbort();
			}
			catch (COMException e)
			{
				RestoreTemplateState();
				if (Interop.Canceled(e.ErrorCode))
				{
					Interop.ThrowAbort();
				}
				else
				{
					ErrorHelper.Show(this.Site, e);
					Interop.ThrowAbort();
				}
			}
			catch (Exception e)
			{
				RestoreTemplateState();
				ErrorHelper.Show(this.Site, e);
				Interop.ThrowAbort();
			}
			finally
			{
				if (Package != null)
				{
					Package.TurnOffOutput();
				}
			}
		}

		private void SetUnfoldingRoot()
		{
			if (template.VSKind == WizardRunKind.AsMultiProject)
			{
				EnvDTE.DTE dte = (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE));
				if (dte.SelectedItems != null && dte.SelectedItems.Count == 1)
				{
					this.unfoldingRoot = DteHelper.GetTarget(dte) as Project;
				}
			}
		}

		/// <summary>
		/// See <see cref="IWizard.RunFinished"/>.
		/// </summary>
		public virtual void RunFinished()
		{
			Debug.Assert(((DTE)GetService(typeof(DTE))).Solution.IsOpen, "There's no opened solution!");
			try
			{
				Package.TurnOnOutput();
				ProcessReferences();
				ExecuteRecipe(true);
			}
			catch (WizardCancelledException)
			{
				RestoreTemplateState();
				Interop.ThrowAbort();
			}
			catch (COMException e)
			{
				RestoreTemplateState();
				if (Interop.Canceled(e.ErrorCode))
				{
					Interop.ThrowAbort();
				}
				else
				{
					ErrorHelper.Show(this.Site, e);
					Interop.ThrowAbort();
				}
			}
			catch (Exception e)
			{
				RestoreTemplateState();
				ErrorHelper.Show(this.Site, e);
				Interop.ThrowAbort();
			}
			finally
			{
				if (Package != null)
				{
					Package.TurnOffOutput();
				}
			}
			try
			{
				this.Dispose();
			}
			catch (Exception e)
			{
				ErrorHelper.Show(this.Site, e);
				Interop.ThrowAbort();
			}
		}

		private void RestoreTemplateState()
		{
			// Let's restore the stack, we are 
			if (UnfoldingTemplates.Count > 0 && UnfoldingTemplates.Peek() == this)
			{
				UnfoldingTemplates.Pop();
			}
			foreach (ProjectItem prItem in openedItems)
			{
				if (prItem.Document != null)
				{
					prItem.Document.Close(vsSaveChanges.vsSaveChangesNo);
				}
			}
			openedItems.Clear();
			if (didEnable)
			{
				IRecipeManagerService recipeManager = (IRecipeManagerService)
					GetService(typeof(IRecipeManagerService));
				if (recipeManager != null)
				{
					recipeManager.DisablePackage(Package);
				}
			}
		}

		/// <summary>
		/// See <see cref="IDisposable.Dispose"/>.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (this.template != null)
			{
				this.template = null;
			}
			if (this.templateDictionary != null)
			{
				this.templateDictionary = null;
			}
			if (UnfoldTemplate.UnfoldingTemplates.Count > 0 && UnfoldTemplate.UnfoldingTemplates.Peek() == this)
			{
				UnfoldTemplate.UnfoldingTemplates.Pop();
			}
			if (openedItems != null)
			{
				openedItems.Clear();
				openedItems = null;
			}
			// Will automatically remove this component from its container.
			base.Dispose(disposing);
		}

		/// <summary>
		/// See <see cref="IWizard.ShouldAddProjectItem"/>.
		/// </summary>
		public virtual bool ShouldAddProjectItem(string filePath)
		{
			return true;
		}

		#endregion
	}
}