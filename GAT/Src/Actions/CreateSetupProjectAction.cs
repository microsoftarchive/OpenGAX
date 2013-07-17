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

#region Using Directives

using System;
using System.Text;
using Microsoft.Practices.ComponentModel;
using Microsoft.Win32;
using EnvDTE;
using System.IO;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.Common;
using Microsoft.VisualStudio.Shell.Interop;

#endregion

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.Actions
{
    [ServiceDependency(typeof(EnvDTE.DTE))]
    internal class CreateSetupProjectAction: Action
    {
        #region Input Properties

        [Input]
        public string PackageName
        {
            get { return packageName; }
			set { packageName = value; }
		} string packageName;

        #endregion

        #region Output properties

        [Output]
        public string ContentFiles
        {
            get { return "ContentFiles"; }
        }

        [Output]
        public string Built
        {
            get { return "Built"; }
        }

        [Output]
        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        } string projectName;

        [Output]
        public string ProjectFolder
        {
            get { return projectFolder; }
            set { projectFolder = value; }
        } string projectFolder;

        [Output]
        public string Template
        {
            get { return template; }
            set { template = value; }
        } string template;

        [Output]
        public string MergeModuleFileName
        {
            get
            {
                return ReflectionHelper.GetAssemblyFolder(this.GetType().Assembly)+"\\Microsoft.Practices.VisualStudio.MergeModule.msm";
            }
        }

        [Output]
        public string CustomActionData
        {
            get
            {
                return CurrentHiveParameter + " /Configuration=\"[TARGETDIR]" + PackageName + ".xml\"";
            }
        }

        [Output]
        public string InstallerProjectName
        {
            get
            {
				return PackageName + "Installer";
            }
        }

        internal string CurrentHiveParameter
        {
            get
            {
                string regRoot = string.Empty;

                ILocalRegistry3 registryService = GetService<SLocalRegistry>() as ILocalRegistry3;
                if (registryService != null)
                {
                    registryService.GetLocalRegistryRoot(out regRoot);
                }

                if (!string.IsNullOrEmpty(regRoot))
                {
                    return string.Format("/Hive={0}", Path.GetFileName(regRoot));
                }

                return string.Empty;
            }
        }

        #endregion

        #region IAction Members

        public override void Execute()
        {
            string vsHive = @"SOFTWARE\Microsoft\VisualStudio\8.0";

            ILocalRegistry3 localRegistry = GetService<SLocalRegistry>() as ILocalRegistry3;
            if (localRegistry != null)
            {
                localRegistry.GetLocalRegistryRoot(out vsHive);
            }

            using (RegistryKey vsdTemplates = Registry.LocalMachine.OpenSubKey(Path.Combine(vsHive, @"Projects\{54435603-DBB4-11D2-8724-00A0C9A8B90C}\AddItemTemplates\TemplateDirs\{66D6F801-B1EC-40A4-BB15-318617E96DAD}")))
            {
                RegistryKey vsdTemplateDir = vsdTemplates.OpenSubKey("/1");
                Template = ((string)vsdTemplateDir.GetValue("TemplatesDir")) + @"\Setup.vdproj";
                ProjectName = PackageName + "Setup.vdproj";
                EnvDTE.DTE dte = GetService<EnvDTE.DTE>(true);
                string slnFolder = (string)(dte.Solution.Properties.Item("Path").Value);
                slnFolder = slnFolder.Substring(0, slnFolder.LastIndexOf("\\"));
                ProjectFolder = slnFolder + @"\" + PackageName + "Setup";
            }
        }

        public override void Undo()
        {
        }

        #endregion
    }
}
