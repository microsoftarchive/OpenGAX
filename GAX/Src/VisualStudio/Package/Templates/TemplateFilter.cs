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
using System.Collections;
using System.Globalization;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;
using System.Diagnostics;
using EnvDTE80;
using EnvDTE;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Templates
{
	[ServiceDependency(typeof(IVsTemplatesService))]
	[ServiceDependency(typeof(IUIService))]
	internal sealed class TemplateFilter : SitedComponent, IVsFilterAddProjectItemDlg, IVsFilterNewProjectDlg
	{
		Hashtable templatesInFolder;
		Hashtable itemTemplatesInFolder;
		public const string RegistryPath = @"NewProjectTemplates\TemplateDirs\{0}\";

        CommandEvents newProjectCommandEvents;
        internal static bool IsNewProjectDialog { get; private set; }

		public TemplateFilter()
		{
			templatesInFolder = new Hashtable(7);
			itemTemplatesInFolder = new Hashtable(7);
		}

		protected override void OnSited()
		{
			base.OnSited();
			IRecipeManagerService recipeManager =
				(IRecipeManagerService)GetService(typeof(IRecipeManagerService));
			foreach (Configuration.Manifest.GuidancePackage package in recipeManager.GetInstalledPackages("VisualStudio"))
			{
				Guid packageGuid = new Guid(package.Guid);
				string newProjectTemplateKey = string.Format(CultureInfo.InvariantCulture,
					RegistryPath,
					packageGuid.ToString("B"));

				RecipeManagerPackage gaxPackage = (RecipeManagerPackage)GetService(typeof(RecipeManagerPackage));
				if (package != null)
				{
					using (RegistryKey key = gaxPackage.ApplicationRegistryRoot.OpenSubKey(newProjectTemplateKey))
					{
						if (key != null)
						{
							foreach (var subKeyName in key.GetSubKeyNames())
							{
								using (RegistryKey subKey = key.OpenSubKey(subKeyName))
								{
#if DEBUG
									Debug.WriteLine(string.Format("Retrieving projects templates information from {0}", gaxPackage.ApplicationRegistryRoot.ToString()));
									Debug.WriteLine(string.Format("User Registry root is {0}", gaxPackage.UserRegistryRoot.ToString()));
#endif
									string templateDir = (string)subKey.GetValue("TemplatesDir");
									if (!templatesInFolder.ContainsKey(templateDir))
									{
										templatesInFolder.Add(templateDir, packageGuid);
									}
									else
									{
										this.TraceWarning(
											String.Format(
												CultureInfo.CurrentCulture,
												Properties.Resources.Template_MultipleTemplateDirs,
												templateDir));
									}
								}
							}
						}
					}
				}
			}

            var dte = (EnvDTE.DTE)this.GetService(typeof(EnvDTE.DTE));
            
            this.newProjectCommandEvents = dte.Events.get_CommandEvents(typeof(VSConstants.VSStd97CmdID).GUID.ToString("B"), (int)VSConstants.VSStd97CmdID.NewProject);
            this.newProjectCommandEvents.BeforeExecute += new _dispCommandEvents_BeforeExecuteEventHandler(newProjectCommandEvents_BeforeExecute);
            this.newProjectCommandEvents.AfterExecute += new _dispCommandEvents_AfterExecuteEventHandler(newProjectCommandEvents_AfterExecute);
        }

        void newProjectCommandEvents_AfterExecute(string Guid, int ID, object CustomIn, object CustomOut)
        {
            IsNewProjectDialog = false;
        }

        void newProjectCommandEvents_BeforeExecute(string Guid, int ID, object CustomIn, object CustomOut, ref bool CancelDefault)
        {
            IsNewProjectDialog = true;
        }

		private bool IsCustomTemplateFolder(string templateDir, Guid projectFactory)
		{
			if (projectFactory.Equals(Guid.Empty))
			{
				foreach (var dir in templatesInFolder.Keys)
				{
					if (templateDir.StartsWith(dir.ToString()))
						return true;
				}
				return templatesInFolder.ContainsKey(templateDir);
			}
			else
			{
				Hashtable itemTemplates = GetItemsTemplates(projectFactory);
				return itemTemplates.ContainsKey(templateDir);
			}
		}

		public const string ItemTemplatesRegistryPath = @"Projects\{0}\AddItemTemplates\TemplateDirs\{1}\/1";

		private Hashtable GetItemsTemplates(Guid projectFactory)
		{
			if (!itemTemplatesInFolder.ContainsKey(projectFactory))
			{
				Hashtable templatesInProject = new Hashtable(7);
				IRecipeManagerService recipeManager =
					(IRecipeManagerService)GetService(typeof(IRecipeManagerService));
				foreach (Configuration.Manifest.GuidancePackage package in recipeManager.GetInstalledPackages("VisualStudio"))
				{
					Guid packageGuid = new Guid(package.Guid);
					string itemTemplateKey = string.Format(CultureInfo.InvariantCulture,
						ItemTemplatesRegistryPath,
						projectFactory.ToString("B"),
						packageGuid.ToString("B"));
					DefaultRegistryRootAttribute registryRoot =
						(DefaultRegistryRootAttribute)Attribute.GetCustomAttribute(
							typeof(RecipeManagerPackage),
							typeof(DefaultRegistryRootAttribute));

					RecipeManagerPackage gaxPackage = (RecipeManagerPackage)GetService(typeof(RecipeManagerPackage));
					if (gaxPackage != null)
					{
#if DEBUG
						Debug.WriteLine(string.Format("Retrieving item templates information from {0}", gaxPackage.ApplicationRegistryRoot.ToString()));
#endif
						using (RegistryKey key = gaxPackage.ApplicationRegistryRoot.OpenSubKey(itemTemplateKey))
						{
							if (key != null)
							{
								string templateDir = (string)key.GetValue("TemplatesDir");
								templatesInProject.Add(templateDir, packageGuid);
							}
						}
					}
				}
				itemTemplatesInFolder.Add(projectFactory, templatesInProject);
			}
			return (Hashtable)itemTemplatesInFolder[projectFactory];
		}

		private bool IsTemplateDirVisible(string templateDir, Guid projectFactory)
		{
			if (IsCustomTemplateFolder(templateDir, projectFactory))
			{
				if (!Directory.Exists(templateDir))
				{
					return false;
				}
				bool visible = false;
				foreach (string vszFile in Directory.GetFiles(templateDir, "*.vsz"))
				{
					if (IsTemplateVisible(vszFile, projectFactory))
					{
						visible = true;
						break;
					}
				}
				return visible;
			}
			return true;
		}

		private bool IsTemplateVisible(string templateFileName, Guid projectFactory)
		{
			if (!File.Exists(templateFileName))
			{
				return false;
			}
			string folder = Path.GetDirectoryName(templateFileName);
			if (IsCustomTemplateFolder(folder, projectFactory))
			{
				try
				{
					IVsTemplatesService templatesService =
						(IVsTemplatesService)GetService(typeof(IVsTemplatesService));
					string[] vszParts = Path.GetFileNameWithoutExtension(templateFileName).Split('x');
					if (vszParts.Length == 2)
					{
						Guid package = new Guid(vszParts[0]);
						int iTemplate = int.Parse(vszParts[1], NumberStyles.HexNumber);
						IVsTemplate template = templatesService.GetTemplate(package, iTemplate);
						if (projectFactory.Equals(Guid.Empty))
						{
							return (template != null && template.IsVisibleInAddNewDialogBox);
						}
						else
						{
							return (template != null && template.IsVisibleInAddNewDialogBox && template.ProjectFactory.Equals(projectFactory));
						}
					}
				}
				catch
				{
					return false;
				}
				return false;
			}
			return true;
		}

		#region IVsFilterAddProjectItemDlg Members

		int IVsFilterAddProjectItemDlg.FilterListItemByLocalizedName(ref Guid rguidProjectItemTemplates, string pszLocalizedName, out int pfFilter)
		{
			pfFilter = 0;
			return VSConstants.S_OK;
		}

		int IVsFilterAddProjectItemDlg.FilterListItemByTemplateFile(ref Guid rguidProjectItemTemplates, string pszTemplateFile, out int pfFilter)
		{
			pfFilter = IsTemplateVisible(pszTemplateFile, rguidProjectItemTemplates) ? 0 : 1;
			return VSConstants.S_OK;
		}

		int IVsFilterAddProjectItemDlg.FilterTreeItemByLocalizedName(ref Guid rguidProjectItemTemplates, string pszLocalizedName, out int pfFilter)
		{
			pfFilter = 0;
			return VSConstants.S_OK;
		}

		int IVsFilterAddProjectItemDlg.FilterTreeItemByTemplateDir(ref Guid rguidProjectItemTemplates, string pszTemplateDir, out int pfFilter)
		{
			pfFilter = IsTemplateDirVisible(pszTemplateDir, rguidProjectItemTemplates) ? 0 : 1;
			return VSConstants.S_OK;
		}

		#endregion

		#region IVsFilterNewProjectDlg Members

		int IVsFilterNewProjectDlg.FilterListItemByLocalizedName(string pszLocalizedName, out int pfFilter)
		{
			pfFilter = 0;
			return VSConstants.S_OK;
		}

		int IVsFilterNewProjectDlg.FilterListItemByTemplateFile(string pszTemplateFile, out int pfFilter)
		{
			pfFilter = IsTemplateVisible(pszTemplateFile, Guid.Empty) ? 0 : 1;
			return VSConstants.S_OK;
		}

		int IVsFilterNewProjectDlg.FilterTreeItemByLocalizedName(string pszLocalizedName, out int pfFilter)
		{
			pfFilter = 0;
			return VSConstants.S_OK;
		}


		int IVsFilterNewProjectDlg.FilterTreeItemByTemplateDir(string pszTemplateDir, out int pfFilter)
		{
			pfFilter = IsTemplateDirVisible(pszTemplateDir, Guid.Empty) ? 0 : 1;
			return VSConstants.S_OK;
		}

		#endregion
	}
}
