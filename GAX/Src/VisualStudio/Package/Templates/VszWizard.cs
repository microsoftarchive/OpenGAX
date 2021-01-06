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

using EnvDTE;
using EnvDTE80;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Templates
{
    /// <summary>
    /// IDTWizard extension that handles invocation of a Visual Studio template from a .vsdir file
    /// </summary>
    [ComVisible(true)]
    [Guid("70BAB4E1-E21D-44d0-95D0-5B6312E494D1")]
    [System.ComponentModel.DesignerCategory("Code")]
    public class VszWizard: Component, IDTWizard
    {
        /// <summary>
        /// Provides access to the values passed initially to from the Add New dialog. May be null.
        /// </summary>
        internal static IDictionary ContextParams
        {
            get { return context; }
        } static IDictionary context;

        #region IDTWizard Members

		void EnvDTE.IDTWizard.Execute(object Application, int hwndOwner, ref object[] ContextParams, ref object[] CustomParams, ref EnvDTE.wizardResult retval)
        {
            IVsTemplate vsTemplate = null;
            try
            {
				DTE dte = new DTETemplate((DTE)Application);
				CustomParams[0] = Environment.ExpandEnvironmentVariables((string)CustomParams[0]);

				if (!Path.IsPathRooted((string)CustomParams[0]) && CustomParams.Length >= 2)
				{
					var guidancePackageName = (string)CustomParams[1];

					var guidancePackageConfigurationFile = RecipeManager.GetConfigurationFile(RecipeManagerPackage.Singleton, guidancePackageName);

					if (!string.IsNullOrEmpty(guidancePackageConfigurationFile))
					{
						var template = Path.Combine(Path.GetDirectoryName(guidancePackageConfigurationFile), (string)CustomParams[0]);

						if (File.Exists(template))
							CustomParams[0] = template;
					}
				}
                string templateFileName = (string)CustomParams[0];
                IVsTemplatesService templateService = (IVsTemplatesService)
                    ServiceHelper.GetService(
						(IRecipeManagerService)
						new VsServiceProvider(Application).GetService(typeof(IRecipeManagerService)), 
						typeof(IVsTemplatesService), this);
                vsTemplate = templateService.GetTemplate(templateFileName);
                string wizardKind = ((string)ContextParams[0]).ToUpper(CultureInfo.InvariantCulture);
                if (wizardKind == Constants.vsWizardNewProject)
                {
                    string destDir = (string)ContextParams[2];
                    //Web projects can pass in an empty directory, if so don't create the dest directory.
                    if ((new System.Uri(destDir)).IsFile)
                    {
                        Directory.CreateDirectory(destDir);
                    }

                    //If adding the project as exclusive, close the current solution then check to see if a
                    //  solution name is specified. If so, then create Ona solution with that name.
                    if (((bool)ContextParams[4]) == true)
                    {
                        vsPromptResult promptResult = dte.ItemOperations.PromptToSave;
                        if (promptResult == vsPromptResult.vsPromptResultCancelled)
                        {
                            retval = wizardResult.wizardResultCancel;
                            return;
                        }
                        dte.Solution.Close(false);
                        if (string.IsNullOrEmpty(((string)ContextParams[5])) == false)
                        {
                            dte.Solution.Create(destDir, ((string)ContextParams[5]));
                        }
                        ContextParams[4] = false;
                    }
                    // Create a new Solution Folder for the multiproject template
                    else if (vsTemplate.VSKind == WizardRunKind.AsMultiProject )
                    {
                        string folderName = (string)ContextParams[1];
                        if (dte.SelectedItems.Count == 1)
                        {
                            object item = DteHelper.GetTarget(dte);
                            if (item is Solution2)
                            {
                                ((Solution2)item).AddSolutionFolder(folderName);
                            }
                            else if (item is Project)
                            {
                                SolutionFolder slnFolder = (SolutionFolder)(((Project)item).Object);
                                slnFolder.AddSolutionFolder(folderName);
                            }
                        }
                    }
                }

                // Pre-fill state with context parameters.
                context = new System.Collections.Specialized.HybridDictionary();
                // See http://msdn.microsoft.com/library/en-us/vsintro7/html/vxlrfcontextparamsenum.asp
                string kind = ((string)ContextParams[0]).ToUpper();
                if (kind == Constants.vsWizardNewProject.ToUpper())
                {
                    FillNewProject(ContextParams, context);
                }
                else if (kind == Constants.vsWizardAddSubProject.ToUpper())
                {
                    FillAddSubProject(ContextParams, context);
                }
                else if (kind == Constants.vsWizardAddItem.ToUpper())
                {
                    FillAddItem(ContextParams, context);
                }

                IDTWizard wizard = new Microsoft.VisualStudio.TemplateWizard.Wizard();
                wizard.Execute(dte, hwndOwner, ref ContextParams, ref CustomParams, ref retval);
            }
            catch (Exception ex)
            {
                retval = wizardResult.wizardResultCancel;
				if (!(ex is COMException) || ((COMException)ex).ErrorCode != VSConstants.E_ABORT)
                {
					ErrorHelper.Show(this.Site, ex);
				}
            }
            finally
            {
                Debug.Assert(UnfoldTemplate.UnfoldingTemplates.Count == 0);
                UnfoldTemplate.UnfoldingTemplates.Clear();
            }
        }

		/// <summary>
		/// Derived classes can call and override this method 
		/// </summary>
		protected virtual void Execute(object Application, int hwndOwner, ref object[] ContextParams, ref object[] CustomParams, ref EnvDTE.wizardResult retval)
		{
			Execute(Application, hwndOwner, ref ContextParams, ref CustomParams, ref retval);
		}

        #endregion

        #region Fill params

        private void FillNewProject(object[] context, IDictionary state)
        {
            state["WizardType"] = context[0];
            state["ProjectName"] = context[1];
            state["LocalDirectory"] = context[2];
            state["InstallationDirectory"] = context[3];
            state["FExclusive"] = context[4];
            state["SolutionName"] = context[5];
            state["Silent"] = context[6];
        }

        private void FillAddSubProject(object[] context, IDictionary state)
        {
            state["WizardType"] = context[0];
            state["ProjectName"] = ((EnvDTE.ProjectItems)context[2]).ContainingProject.UniqueName;
            state["ProjectItems"] = context[2];
            state["LocalDirectory"] = context[3];
            state["ItemName"] = context[4];
            state["InstallationDirectory"] = context[5];
            state["Silent"] = context[6];
        }

        private void FillAddItem(object[] context, IDictionary state)
        {
            state["WizardType"] = context[0];
            state["ProjectName"] = ((EnvDTE.ProjectItems)context[2]).ContainingProject.UniqueName;
            state["ProjectItems"] = context[2];
            state["LocalDirectory"] = context[3];
            state["ItemName"] = context[4];
            state["InstallationDirectory"] = context[5];
            state["Silent"] = context[5];
        }

        #endregion Fill params
    }
}
