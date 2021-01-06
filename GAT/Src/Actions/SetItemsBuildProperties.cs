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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.RecipeFramework.MetaGuidancePackage;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.Actions
{
	/// <summary>
	/// Sets all the items inside folder Template with the appropiate settings that is
	/// Copy To Output Directory = Copy If Newer
	/// Build Action = Content
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	public class SetItemsBuildProperties : Action, IServiceProvider
	{
		/// <summary>
		/// It sets all the items with the appropiate values
		/// </summary>
		public override void Execute()
		{
			if (MessageBox.Show(Properties.Resources.Actions_ItemsBuildProps_Confirmation,
				Properties.Resources.Actions_ItemsBuildProps_Title, MessageBoxButtons.YesNo,
				MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			DTE vs = (DTE)GetService(typeof(DTE));
			ProjectItem item = (ProjectItem)DteHelper.GetTarget(vs);
			vs.StatusBar.Highlight(true);
			if (item.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFolder)
			{
				SetPropertiesOfFolder(item);
			}
			if (applied)
			{
				TraceUtil.TraceInformation(this, Properties.Resources.Actions_ItemsBuildProps_Success);
				vs.StatusBar.Text = Properties.Resources.Actions_ItemsBuildProps_Success;
			}
			else
			{
				TraceUtil.TraceInformation(this, Properties.Resources.Actions_ItemsBuildProps_Fail);
				vs.StatusBar.Text = Properties.Resources.Actions_ItemsBuildProps_Fail;
			}
			vs.StatusBar.Highlight(false);
		}

		private void SetPropertiesOfFolder(ProjectItem pi)
		{
			foreach (ProjectItem item in pi.ProjectItems)
			{
				if (item.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFolder)
				{
					TraceUtil.TraceInformation(this, string.Format(CultureInfo.CurrentCulture,
						Properties.Resources.Actions_ItemsBuildProps_Progress, item.Name));
					SetPropertiesOfFolder(item);
				}
				else if (item.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
				{
					applied = true;
                    SetBuildProperties(item);
					Utils.SetIncludeInVsix(this, item, true);

                    if (item.ProjectItems != null)
                    {
                        foreach (ProjectItem subItem in item.ProjectItems)
                        {
                            SetBuildProperties(subItem);
                            Utils.SetIncludeInVsix(this, subItem, true);
                        }
                    }
				}
			}
		}

        private static void SetBuildProperties(ProjectItem item)
        {
            item.Properties.Item("BuildAction").Value = 2;
            item.Properties.Item("CopyToOutputDirectory").Value = 2;
            item.Properties.Item("ItemType").Value = "Content";
        }


		private bool applied = false;

		/// <summary>
		/// Undo of the set of the properties not supported
		/// </summary>
		public override void Undo()
		{
		}

		object IServiceProvider.GetService(Type serviceType)
		{
			return this.GetService(serviceType);
		}
	}
}
