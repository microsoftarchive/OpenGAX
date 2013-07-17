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
using System.Configuration;
using Config = System.Configuration.Configuration;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using System.IO;
using Microsoft.Win32;
using Microsoft.Practices.ComponentModel;
using System.ComponentModel.Design;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.RecipeFramework.Library.Templates.Actions;
using Microsoft.VisualStudio.Shell.Interop;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Configuration.Actions
{
    /// <summary>
    /// Given an <see cref="EnvDTE.Project"/> object, obtains the <see cref="System.Configuration.Configuration"/> object
    /// </summary>
    [ServiceDependency(typeof(ITypeResolutionService))]
    public class GetConfigurationAction : ConfigurableAction
    {
        const string VisualStudioRoot = @"Software\Microsoft\VisualStudio\";

        #region Input Properties

        /// <summary>
        /// The project that owns the configuration file
        /// </summary>
        [Input(Required=true)]
        public EnvDTE.Project Project
        {
            get { return project; }
            set { project = value; }
        } EnvDTE.Project project;

        /// <summary>
        /// The template to unfold in case that the project does not have a configuration file
        /// </summary>
        [Input(Required=false)]
        public string Template
        {
            get 
            {
                if (!File.Exists(template))
                {
                    if (string.IsNullOrEmpty(template))
                    {
                        string vsHive = @"8.0";

                        ILocalRegistry3 localRegistry = GetService<SLocalRegistry>() as ILocalRegistry3;

                        if (localRegistry != null)
                        {
                            string regRoot = string.Empty;
                            localRegistry.GetLocalRegistryRoot(out regRoot);
                            regRoot = regRoot.Replace(VisualStudioRoot, string.Empty);
                            vsHive = regRoot.Split('\\')[0];
                        }
                      
                        //    using (RegistryKey vsSelectedHive = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Guidance Automation Extensions\Hosts\VisualStudio"))
                        //    {
                        //        if (vsSelectedHive != null)
                        //        {
                        //            vsHive = vsSelectedHive.GetValue("CurrentHive", vsHive).ToString();
                        //        }
                        //    }
                      

                        vsHive = Path.Combine(@"SOFTWARE\Microsoft\VisualStudio\Setup\VS", vsHive);

                        using (RegistryKey vsSetup = Registry.LocalMachine.OpenSubKey(vsHive))
                        {
                            template = (string)vsSetup.GetValue("EnvironmentDirectory");
                        }
                        if (Project.Kind.Equals(CSharpLanguage, StringComparison.InvariantCultureIgnoreCase))
                        {
                            template += @"ItemTemplatesCache\CSharp\1033\AppConfig.zip\App.vstemplate";
                        }
                        else if (Project.Kind.Equals(VBLanguage, StringComparison.InvariantCultureIgnoreCase))
                        {
                            template += @"ItemTemplatesCache\VisualBasic\1033\AppConfiguration.zip\AppConfiguration.vstemplate";
                        }
                        else if (Project.Kind.Equals(JSharpLanguage, StringComparison.InvariantCultureIgnoreCase))
                        {
                            template += @"ItemTemplatesCache\JSharp\1033\AppConfig.zip\App.vstemplate";
                        }
                        else
                        {
                            throw new InvalidOperationException("Language not supported");
                        }
                    }
                    else
                    {
                        TypeResolutionService resolutionService =
                            (TypeResolutionService)GetService(typeof(ITypeResolutionService));
                        template =
                            new FileInfo(
                                Path.Combine(resolutionService.BasePath + @"\Templates\",
                                    template)).FullName;
                    }
                }
                return template;  
            }
            set { template = value;  }
        } string template = string.Empty;
       
        #endregion

        #region Output Properties

        /// <summary>
        /// The output <see cref="System.Configuration.Configuration"/> object
        /// </summary>
        [Output]
        public Config Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        } Config configuration;

        #endregion

        #region Action members

        private string GetAppConfig()
        {
            foreach(EnvDTE.ProjectItem item in Project.ProjectItems)
            {
                FileInfo fileInfo = new FileInfo(item.get_FileNames(0));
                if (fileInfo.Name.Equals("App.config", StringComparison.InvariantCultureIgnoreCase) )
                {
                    return fileInfo.FullName;
                }
            }
            return string.Empty;
        }

        private const string CSharpLanguage = "{FAE04EC0-301F-11d3-BF4B-00C04F79EFBC}";
        private const string VBLanguage = "{F184B08F-C81C-45f6-A57F-5ABD9991F28F}";
        private const string JSharpLanguage = "{E6FDF86B-F3D1-11D4-8576-0002A516ECE8}";

        /// <summary>
        /// <see cref="IAction.Execute"/>
        /// </summary>
        public override void Execute()
        {
            string appConfigFileName = GetAppConfig();
            if (string.IsNullOrEmpty(appConfigFileName))
            {
				if (string.IsNullOrEmpty(this.Template))
				{
					return;
				}
                using(UnfoldTemplateAction unfoldAction=new UnfoldTemplateAction())
                {
                    this.Container.Add(unfoldAction);
                    unfoldAction.Template = this.Template;
                    unfoldAction.ItemName = "App.config";
                    unfoldAction.Root = this.Project;
                    unfoldAction.Execute();
                }
                appConfigFileName = GetAppConfig();
            }
            ExeConfigurationFileMap exeFileMap = new ExeConfigurationFileMap();
            exeFileMap.ExeConfigFilename = appConfigFileName;
            this.Configuration = ConfigurationManager.OpenMappedExeConfiguration(
                exeFileMap,ConfigurationUserLevel.None);
        }

        /// <summary>
        /// <see cref="IAction.Undo"/>
        /// </summary>
        public override void Undo()
        {
            this.Configuration = null;
        }

        #endregion
    }
}
