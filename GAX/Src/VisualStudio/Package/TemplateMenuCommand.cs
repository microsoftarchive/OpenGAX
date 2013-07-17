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
using System.Text;
using EnvDTE;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Services;
using System.Diagnostics;
using Microsoft.Practices.RecipeFramework.VisualStudio.Services;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;
using Microsoft.VisualStudio;
using System.IO;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
	internal sealed class TemplateMenuCommand: AssetMenuCommand, IVsBrowseProjectLocation 
	{
        IVsTemplate template;

		public TemplateMenuCommand(IVsTemplate template, GuidancePackage guidancePackage, Microsoft.Practices.ComponentModel.ServiceContainer serviceProvider)
            : base(guidancePackage, serviceProvider, template.Command )
		{
            this.template = template;
            string templateMenuName = String.Format(
                CultureInfo.CurrentCulture,
                Microsoft.Practices.RecipeFramework.VisualStudio.Properties.Resources.Templates_ContextMenu,
                this.template.Name);
            this.Text = templateMenuName;
        }

		protected override void OnQueryStatus()
		{
            Visible = this.template.IsVisibleFromContextMenu;
		}

		protected override void OnExec()
		{
            if (template.IsVisibleFromContextMenu)
            {
                IConfigurationService configService=
                    (IConfigurationService)ServiceProvider.GetService(typeof(IConfigurationService),true);
                if (template.Kind == TemplateKind.Project)
                {
                    IVsSolution3 solution = (IVsSolution3)ServiceProvider.GetService(typeof(SVsSolution), true);
					IVsBrowseProjectLocation browseProjectLocation = null;
					uint cnpvdeFlags = (uint)(__VSCREATENEWPROJVIADLGEXFLAGS.VNPVDE_ALWAYSADDTOSOLUTION | __VSCREATENEWPROJVIADLGEXFLAGS.VNPVDE_ADDNESTEDTOSELECTION);
					//browseProjectLocation = this;
					//cnpvdeFlags |= (uint)(__VSCREATENEWPROJVIADLGEXFLAGS.VNPVDE_OVERRIDEBROWSEBUTTON);
                    solution.CreateNewProjectViaDlgEx(Microsoft.Practices.RecipeFramework.VisualStudio.Properties.Resources.IDD_ADDPROJECTDLG,
                        null,
                        configService.CurrentPackage.Caption,
                        this.template.Name,
                        null,
						cnpvdeFlags,
						browseProjectLocation);
                }
                else if (template.Kind== TemplateKind.ProjectItem )
                {
                    uint itemid = 0;
                    IVsProject vsProject = DteHelper.GetCurrentSelection(this.ServiceProvider, out itemid) as IVsProject;
                    IVsAddProjectItemDlg2 addProjectItemDlg =
                        (IVsAddProjectItemDlg2)ServiceProvider.GetService(typeof(SVsAddProjectItemDlg),true);
                    __VSADDITEMFLAGS addItemsFlags = __VSADDITEMFLAGS.VSADDITEM_AddNewItems | __VSADDITEMFLAGS.VSADDITEM_SuggestTemplateName;
                    Guid projectGuid = this.template.ProjectFactory;
                    string location = String.Empty;
                    string filter = String.Empty;
                    int dontShowAgain = VSConstantsEx.TRUE;
                    addProjectItemDlg.AddProjectItemDlgTitled(itemid,
                        ref projectGuid,
                        vsProject,
                        (uint)addItemsFlags,
                        null,
                        configService.CurrentPackage.Caption,
                        this.template.Name,
                        ref location,
                        ref filter,
                        out dontShowAgain);
                }
            }
		}

		private string DefaultLocation
		{
			get
			{
				string relativePath = string.Empty;
				DTE dte = this.ServiceProvider.GetService<DTE>();
				if (dte.SelectedItems.Count > 0)
				{
					relativePath = DteHelper.BuildPath(dte.SelectedItems.Item(1));
				}
				string slnDir = new FileInfo(dte.Solution.FullName).Directory.FullName;
				return Path.Combine(slnDir, relativePath);
			}
		}

		#region IVsBrowseProjectLocation Members

		int IVsBrowseProjectLocation.BrowseProjectLocation(string pszStartDirectory, out string pbstrProjectLocation)
		{
			pbstrProjectLocation = this.DefaultLocation;
			return VSConstants.S_OK;
		}

		#endregion
	}
}
