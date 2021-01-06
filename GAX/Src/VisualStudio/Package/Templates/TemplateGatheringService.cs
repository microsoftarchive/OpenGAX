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
using Microsoft.Practices.RecipeFramework.Services;
using System.ComponentModel.Design;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.TemplateWizard;
using System.ComponentModel;
using System.IO;
using System.Xml;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Templates
{
	[ServiceDependency(typeof(IValueGatheringService))]
	[ServiceDependency(typeof(IConfigurationService))]
	internal sealed class TemplateGatheringService : Microsoft.Practices.ComponentModel.ServiceContainer, IValueGatheringService
	{
		#region Fields and constructor

		IValueGatheringService gatheringService;
		IDictionaryService templateDictionary;
		GuidancePackage package;
		bool executeActions;

		public TemplateGatheringService(IDictionaryService templateDictionary, bool executeActions)
		{
			if (templateDictionary == null)
			{
				throw new ArgumentNullException("templateDictionary");
			}
			this.templateDictionary = templateDictionary;
			this.executeActions = executeActions;
			this.result = ExecutionResult.Finish;
		}

		#endregion

		#region Overrides

		protected override void OnSited()
		{
			base.OnSited();
			this.gatheringService = GetService<IValueGatheringService>(true);
		}

		#endregion

		#region Public properties

		public ExecutionResult Result
		{
			get { return result; }
		} ExecutionResult result;

		#endregion

		#region IArgumentGatheringService members

		ExecutionResult IValueGatheringService.Execute(XmlElement serviceData, bool allowSuspend)
		{
			package = (GuidancePackage)Site.Container;
			using (package.SetupTemporaryService(typeof(IDictionaryService), this.templateDictionary))
			{
				CopyDictionaryServiceToReplamentDictionary();
				if (executeActions)
				{
					ProcessT4Templates();
					result = ExecutionResult.Finish;
					return result;
				}
				if (serviceData == UnfoldTemplate.NopWizardConfig)
				{
					result = ExecutionResult.Finish;
				}
				else
				{
					result = gatheringService.Execute(serviceData, allowSuspend);
				}
				if (result == ExecutionResult.Finish)
				{
					//Always return ExecutionResult.Suspend to avoid executing the actions
					return ExecutionResult.Suspend;
				}
				else
				{
					result = ExecutionResult.Cancel;
				}
				return result;
			}
		}

		private void CopyDictionaryServiceToReplamentDictionary()
		{
			IConfigurationService configService = GetService<IConfigurationService>(true);
			if (configService.CurrentRecipe.Arguments != null)
			{
				foreach (Configuration.Argument argument in configService.CurrentRecipe.Arguments)
				{
					// Out implementation of GetValue will also copy data to the replacement dictionary
					this.templateDictionary.GetValue(argument.Name);
				}
			}
		}

		private void ProcessT4Templates()
		{
			T4UnfoldAction action = new T4UnfoldAction();
			UnfoldTemplate template = (UnfoldTemplate)UnfoldTemplate.UnfoldingTemplates.Peek();
			if (template.Template.VSKind == WizardRunKind.AsMultiProject)
			{
				// We dont process any T4 template for a multiproject template
				return;
			}
			try
			{
				package.Add(action);
				IConfigurationService configService = package.GetService<IConfigurationService>(true);
				IVsTemplate vsTemplate = template.Template;
				Configuration.Recipe recipe = null;
				if (vsTemplate != null && vsTemplate.ExtensionData != null && !string.IsNullOrEmpty(vsTemplate.ExtensionData.Recipe))
				{
					recipe = configService.CurrentPackage[vsTemplate.ExtensionData.Recipe];
				}
				if (recipe != null && recipe.Arguments != null)
				{
					IDictionaryService dictService = GetService<IDictionaryService>();
					PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(action);
					foreach (Configuration.Argument recipeArg in recipe.Arguments)
					{
						object value = dictService.GetValue(recipeArg.Name);
						properties[recipeArg.Name].SetValue(action, value);
					}
				}
				action.Project = template.generatedProject;
				action.ProjectItemCollection = template.generatedItems;
				action.Execute();
			}
			catch (Exception ex)
			{
				// We will swallow the exception, the erroneous references did not get added
				ErrorHelper.Show(this, ex);
			}
			finally
			{
				if (action != null)
				{
					package.Remove(action);
					action.Dispose();
					action = null;
				}
			}
		}

		#endregion
	}
}
