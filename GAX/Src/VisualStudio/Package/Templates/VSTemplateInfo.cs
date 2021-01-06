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
using Microsoft.Win32;
using Microsoft.Practices.Common;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Design;
using System.Collections;
using Microsoft.Practices.ComponentModel;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.Services;
using System.IO;
using Microsoft.Practices.RecipeFramework.VisualStudio.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using System.Diagnostics;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Templates
{
    internal sealed class VSTemplatesService : ContainerComponent, IVsTemplatesService
    {
        #region Fields & Constructor
        RegistryKey currentVsRegistryKey;
        Hashtable registryData;

        private class RegistryData
        {
            public int Command;
            public Guid PackageGuid;
            public CommandID CommandID
            {
                get { return new CommandID(PackageGuid, Command); }
            }
            public string PackageName;
            public RegistryData(int command, string PackageName, Guid Package)
            {
                this.Command = command;
                this.PackageGuid = Package;
                this.PackageName = PackageName;
            }
        }

        public VSTemplatesService()
            : this(null)
        {
        }

        /// <summary>
        /// Use this ctor to use the templates services in a specific vs hive.
        /// </summary>
        /// <param name="currentVsRegistryKey">The current vs registry key.</param>
        internal VSTemplatesService(RegistryKey currentVsRegistryKey)
        {
            this.currentVsRegistryKey = currentVsRegistryKey;
            registryData = new Hashtable(7, CaseInsensitiveHashCodeProvider.Default,
                CaseInsensitiveComparer.Default);
            ReadRegistryData();
        }

        #endregion

        #region Private implmentation

        private static string GetRegistryKeyName(Guid PackageGuid)
        {
            if (PackageGuid.Equals(Guid.Empty))
            {
                return String.Format(CultureInfo.CurrentCulture, @"Packages\{0}\Templates",
                    typeof(RecipeManagerPackage).GUID.ToString("B"));
            }
            else
            {
                return String.Format(CultureInfo.CurrentCulture, @"Packages\{0}\Templates\{1}",
                    typeof(RecipeManagerPackage).GUID.ToString("B"),
                    PackageGuid.ToString("B"));
            }
        }

        private RegistryKey GetRegistryKey(Guid PackageGuid, bool registering)
        {
            // RegRoot is specified @ registration time, when a would be registered in another hive
            // than the current or regular, if regRoot is not specified the currenthive will be used.

            if (currentVsRegistryKey == null)
            {
                Package package = GetService<Package>();

                if (package != null)
                {
                    currentVsRegistryKey = package.ApplicationRegistryRoot;
                }
                else
                {
					currentVsRegistryKey = RegistryHelper.GetCurrentVsRegistryKey(registering);
                }
            }

            if (registering)
            {
                return currentVsRegistryKey.CreateSubKey(GetRegistryKeyName(PackageGuid));
            }
            else
            {
                return currentVsRegistryKey.OpenSubKey(GetRegistryKeyName(PackageGuid));
            }
        }

        private void GetTemplate(string templateFile, ArrayList templates, Configuration.GuidancePackage guidancePackage)
        {
            IVsTemplate template = null;

            // HACK: we don't want to process VB templates if VB package isn't installed

            if (IsVbTemplate(templateFile) && !IsVsIsInstalled()) 
            {
				this.TraceInformation("The VS template '{0}' uses VB and VB isn't installed, skipping it...", templateFile);
                return;
            }

            if (guidancePackage != null)
            {
                RegisterTemplate(templateFile, guidancePackage.Name, new Guid(guidancePackage.Guid));
            }

            template = ((IVsTemplatesService)this).GetTemplate(templateFile);

            if (template != null)
            {
                templates.Add(template);
            }

            //IVsTemplate template = null;
            //if (guidancePackage != null)
            //{
            //    RegisterTemplate(templateFile, guidancePackage.Name, new Guid(guidancePackage.Guid));
            //}
            //template = ((IVsTemplatesService)this).GetTemplate(templateFile);
            //if (template != null)
            //{
            //    templates.Add(template);
            //}
        }

        private bool IsVbTemplate(string templateFilename)
        {

            // we could use an XmlDocument for this but performance of guidance package registration is already poor
            // and the checking below should work most of the time

            string templateText = File.ReadAllText(templateFilename).ToLower();

            // Check both, <ProjectType> and <ProjectSubType> so we catch Web Projects templates in VB too.
            if (templateText.Contains("<projecttype>visualbasic</projecttype>")
            || templateText.Contains("<projectsubtype>visualbasic</projectsubtype>"))
            {
                return true;
            }

            return false;

        }

        private bool IsVsIsInstalled()
        {
            if (currentVsRegistryKey != null)
            {
                using(RegistryKey vbKey = currentVsRegistryKey.OpenSubKey(@"InstalledProducts\Microsoft Visual Basic"))
                {
                    return vbKey != null;
                }
            }
            return false;
        }

        private void PopulateTemplates(string basePath, ArrayList templates, Configuration.GuidancePackage guidancePackage)
        {
            GetTemplates(basePath + @"\Templates\Items", templates, guidancePackage);
            GetTemplates(basePath + @"\Templates\Projects", templates, guidancePackage);
            GetTemplates(basePath + @"\Templates\Solutions", templates, guidancePackage);
            GetTemplates(basePath + @"\Templates\Solutions\Projects", templates, guidancePackage);
        }

        private void GetTemplates(string directory, ArrayList templates, Configuration.GuidancePackage guidancePackage)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }
            foreach (string templateFile in Directory.GetFiles(directory, "*.vstemplate"))
            {
                GetTemplate(templateFile, templates, guidancePackage);
            }
            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                foreach (string templateFile in Directory.GetFiles(subDirectory, "*.vstemplate"))
                {
                    GetTemplate(templateFile, templates, guidancePackage);
                }
            }
        }

        private RegistryData GetTemplateRegistryData(string templateFileName)
        {
            // Normalize path as it may contain double back slash which usually shouldn't hurt but will break the .ContainsKey check below
            // this is due to Uri.LocalPath behaving differently under Vista -- reported as VS bug #
            //templateFileName = templateFileName.Replace(@"\\", @"\");
            if (!registryData.ContainsKey(templateFileName))
            {
                throw new InvalidOperationException(String.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resources.Templates_NotRegistered,
                    templateFileName));
            }
            return (RegistryData)registryData[templateFileName];
        }

        #endregion

        #region IVsTemplatesService Members

        private void ReadRegistryData()
        {
            using (RegistryKey templatesKey = this.GetRegistryKey(Guid.Empty, false))
            {
                if (templatesKey == null)
                {
                    return;
                }
                foreach (string PackageKeyName in templatesKey.GetSubKeyNames())
                {
                    if (String.IsNullOrEmpty(PackageKeyName))
                    {
                        continue;
                    }
                    Guid PackageGuid = new Guid(PackageKeyName);
                    using (RegistryKey templateKey = this.GetRegistryKey(PackageGuid, false))
                    {
                        if (templateKey == null)
                        {
                            continue;
                        }
                        string PackageName = (string)templateKey.GetValue("PackageName");
                        int n = (int)templateKey.GetValue("LastTemplate", 0);
                        if (string.IsNullOrEmpty(PackageName) || n == 0)
                        {
                            continue;
                        }
                        for (int i = 1; i <= n; i++)
                        {
                            string templateFileName = (string)templateKey.GetValue(i.ToString());
                            if (!string.IsNullOrEmpty(templateFileName))
                            {
                                registryData.Add(templateFileName, new RegistryData(i, PackageName, new Guid(PackageKeyName)));
                            }
                        }
                    }
                }
            }
        }

        private int RegisterTemplate(string templateFileName, string guidancePackage, Guid Package)
        {
            using (RegistryKey keyPackage = this.GetRegistryKey(Package, true))
            {
                int templateCounter = (int)keyPackage.GetValue("LastTemplate");
                if (templateCounter >= 255)
                {
                    throw new InvalidOperationException(Properties.Resources.Templates_MaximumExceeded);
                }
                ++templateCounter;
                keyPackage.SetValue(templateCounter.ToString(), templateFileName);
                if (registryData.ContainsKey(templateFileName))
                {
                    registryData.Remove(templateFileName);
                }
                registryData.Add(templateFileName, new RegistryData(templateCounter, guidancePackage, Package));
                keyPackage.SetValue("LastTemplate", templateCounter);
                return templateCounter;
            }
        }

        IAssetDescription[] IVsTemplatesService.RegisterTemplates(string basePath, Configuration.GuidancePackage guidancePackage)
        {
            Guid PackageGuid = new Guid(guidancePackage.Guid);
            using (RegistryKey templatesKey = this.GetRegistryKey(PackageGuid, true))
            {
                templatesKey.SetValue("LastTemplate", 0);
                templatesKey.SetValue("PackageName", guidancePackage.Name);
            }
            ArrayList templates = new ArrayList();
            PopulateTemplates(basePath, templates, guidancePackage);
            foreach (IVsTemplate template in templates)
            {
                ((TemplateMetaData)template).Register(true);
            }
            return (IAssetDescription[])templates.ToArray(typeof(IAssetDescription));
        }

        void IVsTemplatesService.UnregisterTemplates(string basePath, Configuration.GuidancePackage guidancePackage)
        {
            foreach (TemplateMetaData template in Components)
            {
                template.Register(false);
            }

            string dirItemsCache = Path.Combine(basePath, @"Templates\Items.Cache");
            if (Directory.Exists(dirItemsCache))
            {
                Directory.Delete(dirItemsCache, true);
            }
            string dirProjectsCache = Path.Combine(basePath, @"Templates\Projects.Cache");
            if (Directory.Exists(dirProjectsCache))
            {
                Directory.Delete(dirProjectsCache, true);
            }
            using (RegistryKey templatesKey = this.GetRegistryKey(Guid.Empty, true))
            {
                templatesKey.DeleteSubKey(new Guid(guidancePackage.Guid).ToString("B"));
            }
        }

        IVsTemplate IVsTemplatesService.GetCurrentTemplate()
        {
            if (UnfoldTemplate.UnfoldingTemplates.Count > 0)
            {
                return (IVsTemplate)UnfoldTemplate.UnfoldingTemplates.Peek();
            }
            return null;
        }

        IVsTemplate IVsTemplatesService.GetTemplate(Guid Package, int iTemplate)
        {
            using (RegistryKey keyPackage = this.GetRegistryKey(Package, false))
            {
                if (keyPackage != null)
                {
                    string templateFileName = (string)keyPackage.GetValue(iTemplate.ToString());
                    return ((IVsTemplatesService)this).GetTemplate(templateFileName);
                }
            }
            return null;
        }

        IVsTemplate IVsTemplatesService.GetTemplate(string templateFileName)
        {
            if (this.Components[templateFileName] == null)
            {
                RegistryData regData = GetTemplateRegistryData(templateFileName);
                this.Add(
                    new TemplateMetaData(
                        templateFileName,
                        regData.CommandID,
                        regData.PackageName, this.currentVsRegistryKey),
                        templateFileName);
            }
            return (IVsTemplate)this.Components[templateFileName];
        }

        IAssetDescription[] IVsTemplatesService.GetHostAssets(string basePath)
        {
            ArrayList templates = new ArrayList();
            PopulateTemplates(basePath, templates, null);
            return (IAssetDescription[])templates.ToArray(typeof(IAssetDescription));
        }

        #endregion

    }
}
