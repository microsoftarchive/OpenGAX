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
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Globalization;
using System.ComponentModel.Design;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System.Text;
using Microsoft.Win32;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;
using Microsoft.Practices.RecipeFramework.VisualStudio.Properties;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Templates
{
	//We don't need these dependecies, we need to run without at installation time
	//[ServiceDependency(typeof(DTE))]
	//[ServiceDependency(typeof(IVsResourceManager))]
	//[ServiceDependency(typeof(IAssetReferenceService))]
	internal sealed class TemplateMetaData : SitedComponent, IVsTemplate, IAssetDescription
    {
        private RegistryKey regRoot;

        private string GetStringFromCommandID(CommandID commandID)
        {
            if (commandID == null)
            {
                throw new ArgumentNullException("commandID");
            }
            //v-oscca:
            //The services resourceManager and dte will be null during installation phase, this is becuase the component is not sited.
            //This is by design.
            IVsResourceManager resourceManager = (IVsResourceManager)GetService(typeof(SVsResourceManager));
            if (resourceManager != null)
            {
                int localeId = CultureInfo.CurrentCulture.LCID;
                DTE dte = (DTE)GetService(typeof(DTE));
                if (dte != null)
                {
                    localeId = dte.LocaleID;
                }
                string valueAsString = null;
                Guid guid = commandID.Guid;
                resourceManager.LoadResourceString(ref guid, localeId, "#" + commandID.ID, out valueAsString);
                return valueAsString;
            }
            return null;
        }

        #region Services.IVsTemplate members

        public string Name
        {
            get
            {
                if (name == null)
                {
                    name = GetStringFromCommandID(commandIdName);
                }
                return name;
            }
        } string name;

        public CommandID CommandIdName
        {
            get
            {
                return commandIdName;
            }
        } CommandID commandIdName;

        public string Description
        {
            get
            {
                if (description == null)
                {
                    description = GetStringFromCommandID(commandIdDescription);
                }
                return description;
            }
        } string description;

        public CommandID CommandIdDescription
        {
            get { return commandIdDescription; }
        }  CommandID commandIdDescription;

        public CommandID Icon
        {
            get { return icon; }
        } CommandID icon;

        public string IconFileName
        {
            get { return iconFileName; }
        } string iconFileName;

        public TemplateKind Kind
        {
            get { return kind; }
        } TemplateKind kind;

        public WizardRunKind VSKind
        {
            get { return vsKind; }
        } WizardRunKind vsKind;

        public VsTemplate.Template ExtensionData
        {
            get { return extensionData; }
        } VsTemplate.Template extensionData;

        public CommandID Command
        {
            get { return command; }
        } CommandID command;

        public string FileName
        {
            get { return templateFileName; }
        } string templateFileName;

        public string PackageName
        {
            get { return packageName; }
        } string packageName;

        public int SortPriority
        {
            get { return sortPriority; }
        } int sortPriority;

        public string SuggestedBaseName
        {
            get { return suggestedBaseName; }
        } string suggestedBaseName;

        public bool CreateNewFolder
        {
            get { return createNewFolder; }
        } bool createNewFolder = false;

        public bool ProvideDefaultName
        {
            get { return provideDefaultName; }
        } bool provideDefaultName = true;

        public bool EnableLocationBrowseButton
        {
            get { return enableLocationBrowseButton; }
        } bool enableLocationBrowseButton = true;

        public bool EnableEditOfLocationField
        {
            get { return enableEditOfLocationField; }
        } bool enableEditOfLocationField = true;

        public LocationField LocationField
        {
            get { return locationField; }
        } LocationField locationField = LocationField.Enabled;

        public bool AppendDefaultFileExtension
        {
            get { return appendDefaultFileExtension; }
        } bool appendDefaultFileExtension = true;

        public bool SupportsLanguageDropDown
        {
            get { return supportsLanguageDropDown; }
        } bool supportsLanguageDropDown = false;

        public bool SupportsMasterPage
        {
            get { return supportsMasterPage; }
        } bool supportsMasterPage = false;

        internal sealed class NativeMethods
        {
            private const uint RT_STRING = 0x00000006;
            private const uint LOAD_LIBRARY_AS_DATAFILE = 0x00000002;
            private const string VsResourceAssemblyFileName = "msenvui.dll";

            public const uint NewProjectResourceId = 13114;
            public const uint AddNewProjectResourceId = 13310;

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool FreeLibrary(IntPtr hModule);

            [DllImport("user32.dll")]
            public static extern int LoadString(IntPtr hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);

            [DllImport("user32.dll")]
            static public extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

            [DllImport("user32.dll")]
            static public extern IntPtr FindWindow(string lpszClass, string lpszWindow);

            [DllImport("user32.dll")]
            static public extern IntPtr GetParent(IntPtr hWnd);

            public static string GetVSResourcesAssemblyLocation()
            {
                string lcid = RecipeManagerPackage.Singleton.GetProviderLocale().ToString();
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, lcid);
                string fileName = Path.Combine(path, VsResourceAssemblyFileName);
                return fileName;
            }

            /// <summary>
            /// Get a string resource from an unmanaged assembly based on the resource id specified.
            /// </summary>
            /// <param name="nativeAssemblyName">The unmanaged assembly file name fullpath.</param>
            /// <param name="resourceId">The resource id.</param>
            /// <returns>string</returns>
            public static string GetStringResource(string nativeAssemblyName, uint resourceId)
            {
                IntPtr hModule = IntPtr.Zero;
                try
                {
                    hModule = LoadLibraryEx(nativeAssemblyName, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);
                    StringBuilder stringBuilder = new StringBuilder(255);
                    LoadString(hModule, resourceId, stringBuilder, stringBuilder.Capacity + 1);
                    return stringBuilder.ToString();
                }
                catch (Exception exception)
                {
                    throw new Exception(string.Format(Resources.NativeMethods_GetStringResourceException, resourceId.ToString(), nativeAssemblyName), exception);
                }
                finally
                {
                    FreeLibrary(hModule);
                }
            }
        }

        public bool IsVisibleInAddNewDialogBox
        {
            get
            {
				if (this.Kind == TemplateKind.Solution)
				{
					return true;
				}
                else if (this.Kind == TemplateKind.Project && TemplateFilter.IsNewProjectDialog)
                {
                    return false;
                }
                
				return IsVisibleFromContextMenu;
            }
        }

        public string Language
        {
            get { return language; }
        } string language;

        public Guid ProjectFactory
        {
            get
            {
                if (projectFactories.Count == 0)
                {
                    if (regRoot == null)
                    {
						regRoot = RegistryHelper.GetCurrentVsRegistryKey(false);
                    }

                    using (RegistryKey projectsKey = regRoot.OpenSubKey("Projects"))
                    {
                        foreach (string projectKeyName in projectsKey.GetSubKeyNames())
                        {
                            using (RegistryKey projectKey = projectsKey.OpenSubKey(projectKeyName))
                            {
                                string lang = projectKey.GetValue("Language(VSTemplate)") as string;
                                // Important, the factory must not be a sub group
                                if (lang != null && projectKey.GetValue("TemplateGroupIDs(VsTemplate)") == null && projectKey.GetValue("TemplateIDs(VsTemplate)") == null)
                                {
                                    Guid projectGuid = Guid.Empty;
                                    try
                                    {
                                        projectGuid = new Guid(projectKeyName);
                                    }
                                    catch
                                    {
                                        this.TraceWarning(Properties.Resources.Templates_InvalidRegistry, projectKeyName);
                                    }
                                    if (projectGuid != Guid.Empty)
                                    {
                                        if (projectFactories.ContainsKey(lang))
                                        {
                                            this.TraceWarning(Properties.Resources.Templates_CorruptMultipleFactories, lang);
                                        }
                                        else
                                        {
                                            projectFactories.Add(lang, projectGuid);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!projectFactories.Contains(Language))
                {
                    //Upps the project facotry is not present, we need to throw.
                    return Guid.Empty;
                }
                return (Guid)projectFactories[Language];
            }
        } static Hashtable projectFactories;

        #endregion

        #region Enabling/Disabling templates

        public bool IsVisibleFromContextMenu
        {
            get
            {
                try
                {
                    if (UnfoldTemplate.UnfoldingTemplates.Count > 0)
                    {
                        // A multi-project template is been unfolded, allow any other template to be unfolded
                        return true;
                    }
                    DTE dte = (DTE)GetService(typeof(DTE));
                    if (dte.SelectedItems != null && dte.SelectedItems.Count > 0)
                    {
                        object target = DteHelper.GetTarget(dte);
                        IAssetReferenceService referenceService =
                            (IAssetReferenceService)GetService(typeof(IAssetReferenceService));
                        if (referenceService != null)
                        {
                            if (referenceService.IsAssetEnabledFor(this.FileName, target))
                            {
                                return true;
                            }
                            else if (VSKind == WizardRunKind.AsMultiProject && target is Project) // If the template is been unfolded, then check the parent folder
                            {
                                Project parentProject = (target as Project).ParentProjectItem.ContainingProject;
                                if (parentProject != null) // Parent folder exist, check the reference to the template
                                {
                                    return referenceService.IsAssetEnabledFor(this.FileName, parentProject);
                                }
                                else // The parent folder is the solution root, check the reference in the solutioin root
                                {
                                    return referenceService.IsAssetEnabledFor(this.FileName, dte.Solution);
                                }
                            }
                        }
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        protected override object GetService(Type serviceType)
        {
            object serviceInstance = base.GetService(serviceType);
            if (serviceInstance == null)
            {
                if (serviceType != typeof(IRecipeManagerService))
                {
                    IRecipeManagerService recipeManager = (IRecipeManagerService)GetService(typeof(IRecipeManagerService));
                    if (recipeManager != null)
                    {
                        IServiceProvider Package = (IServiceProvider)recipeManager.GetPackage(this.PackageName);
                        if (Package != null)
                        {
                            serviceInstance = Package.GetService(serviceType);
                        }
                    }
                }
            }
            return serviceInstance;
        }

        public string CacheBaseBath
        {
            get
            {
                string basePath = FileName.ToLowerInvariant();
                switch (kind)
                {

                    case TemplateKind.Solution:
                        basePath = basePath.Substring(0, basePath.LastIndexOf(@"templates\solutions\")) + @"Templates\Solutions.Cache";
                        break;
                    case TemplateKind.Project:
                        if (basePath.LastIndexOf(@"templates\solutions\") != -1)
                        {
                            basePath = basePath.Substring(0, basePath.LastIndexOf(@"templates\solutions\")) + @"Templates\Solutions.Cache";
                        }
                        else
                        {
                            basePath = basePath.Substring(0, basePath.LastIndexOf(@"templates\projects\")) + @"Templates\Projects.Cache";
                        }
                        break;
                    case TemplateKind.ProjectItem:
                        basePath = basePath.Substring(0, basePath.LastIndexOf(@"templates\items\")) + @"Templates\Items.Cache";
                        break;
                }
                return basePath;
            }
        }

        private int VSDIRFlags
        {
            get
            {
                int vsFlags = 0;
                // This value is never used
                //vsFlags |= (int)(__VSDIRFLAGS.VSDIRFLAG_DisableNameField);
                if (Kind == TemplateKind.Solution)
                {
                    vsFlags |= (int)(__VSDIRFLAGS2.VSDIRFLAG_SolutionTemplate);
                }
                if (!this.provideDefaultName)
                {
                    vsFlags |= (int)(__VSDIRFLAGS.VSDIRFLAG_DontInitNameField);
                }
                if (this.createNewFolder)
                {
                    vsFlags |= (int)(__VSDIRFLAGS2.VSDIRFLAG_RequiresNewFolder);
                }
                if (!this.enableLocationBrowseButton)
                {
                    vsFlags |= (int)(__VSDIRFLAGS.VSDIRFLAG_DisableBrowseButton);
                }
                if (this.locationField == LocationField.Hidden)
                {
                    vsFlags |= (int)(__VSDIRFLAGS2.VSDIRFLAG_DontShowNameLocInfo);
                }
                else if (this.LocationField == LocationField.Disabled)
                {
                    vsFlags |= (int)(__VSDIRFLAGS.VSDIRFLAG_DisableLocationField);
                }
                if (!this.enableEditOfLocationField)
                {
                    vsFlags |= (int)(__VSDIRFLAGS.VSDIRFLAG_DisableLocationField);
                }
                if (!this.appendDefaultFileExtension)
                {
                    vsFlags |= (int)(__VSDIRFLAGS.VSDIRFLAG_DontAddDefExtension);
                }
                if (this.supportsLanguageDropDown)
                {
                    vsFlags |= (int)(__VSDIRFLAGS2.VSDIRFLAG_EnableLangDropdown);
                }
                if (this.supportsMasterPage)
                {
                    vsFlags |= (int)(__VSDIRFLAGS2.VSDIRFLAG_EnableMasterPage);
                }
                return vsFlags;
            }
        }

        private string VSDir
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(VSZName);
                sb.Append(".vsz");
                sb.Append("|"); // RelPathName
                if (CommandIdName != null)
                {
                    sb.Append(CommandIdName.Guid.ToString("B"));
                }
                else
                {
                    sb.Append(Command.Guid.ToString("B"));
                }
                sb.Append("|"); // {clsidPAckage}
                if (Name != null)
                {
                    sb.Append(Name);
                }
                else if (CommandIdName != null)
                {
                    sb.Append("#");
                    sb.Append(CommandIdName.ID);
                }
                else
                {
                    sb.Append("#0");
                }
                sb.Append("|"); // Localized Name
                sb.Append(SortPriority);
                sb.Append("|"); // SortPriority
                if (Description != null)
                {
                    sb.Append(Description);
                }
                else if (CommandIdName != null && CommandIdDescription != null && CommandIdName.Guid.Equals(CommandIdDescription.Guid))
                {
                    sb.Append("#");
                    sb.Append(CommandIdDescription.ID);
                }
                else
                {
                    sb.Append("#0");
                }
                sb.Append("|"); // Description
                if (Icon != null)
                {
                    sb.Append(Icon.Guid.ToString("B"));
                }
                else
                {
                    sb.Append(Command.Guid.ToString("B"));
                }
                sb.Append("|"); // Icon {clsidPackage}
                if (Icon != null)
                {
                    sb.Append("#");
                    sb.Append(Icon.ID);
                }
                else
                {
                    sb.Append("#0");
                }
                sb.Append("|"); // IconResourceId
                sb.Append(VSDIRFlags.ToString());
                sb.Append("|"); // Flags
                if (SuggestedBaseName != null)
                {
                    sb.Append(SuggestedBaseName);
                }
                // SuggestedBaseName
                return sb.ToString();
            }
        }

        private void CreateVSZFile()
        {
            using (StreamWriter textWriter = new StreamWriter(Path.Combine(CacheBaseBath, VSZName + ".vsz")))
            {
                textWriter.WriteLine("VSWizard 7.0");
                textWriter.WriteLine(String.Format(CultureInfo.CurrentCulture, "Wizard={0}", typeof(VszWizard).GUID.ToString("B")));
                textWriter.WriteLine(String.Format(CultureInfo.CurrentCulture, "Param=\"{0}\"", FileName));
				textWriter.WriteLine(String.Format(CultureInfo.CurrentCulture, "Param=\"{0}\"", PackageName));
            }
        }

        private void DeleteVSZFile()
        {
            string vszFile = Path.Combine(CacheBaseBath, VSZName + ".vsz");
            if (File.Exists(vszFile))
            {
                File.Delete(vszFile);
            }
        }

        private string VSZName
        {
            get
            {
                return Command.Guid.ToString() + "x" + Command.ID.ToString("X4");
            }
        }

        public void Register(bool register)
        {
            string basePath = CacheBaseBath;
            string name = VSZName;
            if (register)
            {
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                CreateVSZFile();
                using (StreamWriter textWriter = new StreamWriter(Path.Combine(basePath, name + ".vsdir")))
                {
                    textWriter.WriteLine(VSDir);
                }
                if (IconFileName != null)
                {
                    string sourceIcoFile = Path.Combine(Path.GetDirectoryName(FileName), IconFileName);
                    string ext = Path.GetExtension(IconFileName);
                    string targetIcoFile = Path.Combine(basePath, VSZName + ext);
                    if (File.Exists(sourceIcoFile))
                    {
                        File.Copy(sourceIcoFile, targetIcoFile, true);
                    }
                }
            }
            else
            {
                DeleteVSZFile();
                string vsdirFile = Path.Combine(basePath, name + ".vsdir");
                if (File.Exists(vsdirFile))
                {
                    File.Delete(vsdirFile);
                }
                string ext = Path.GetExtension(IconFileName);
                string icoFile = Path.Combine(basePath, VSZName + ext);
                if (File.Exists(icoFile))
                {
                    File.Delete(icoFile);
                }
            }
        }

        #endregion

        #region IAssetDescription members

        public string Caption
        {
            get { return Name; }
        }

        public string Category
        {
            get { return Properties.Resources.Templates_Category; }
        }

        #endregion

        #region Template File Reading

        VsTemplate.Template GetWizardData(XmlNode wizardData)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ConformanceLevel = ConformanceLevel.Auto;

            // Pull the XSD from the config assembly (it's an embedded resource).
            using (Stream xsdstream = typeof(VsTemplate.Template).Assembly.GetManifestResourceStream(
                VsTemplate.Template.SchemaResourceName))
            {
                Debug.Assert(xsdstream != null, "XSD not embedded in config assembly");
                // If the schema is not valid (we must check that at design-time), this will throw.
                XmlSchema xsd = XmlSchema.Read(xsdstream, null);
                settings.Schemas.Add(xsd);
            }
            using (XmlReader xr = XmlReader.Create(new XmlNodeReader(wizardData), settings))
            {
                VsTemplate.Template template = (VsTemplate.Template)new VsTemplate.TemplateSerializer().Deserialize(xr);
                return template;
            }
        }

        private void ReadTemplate()
        {
            try
            {
                #region Start reading Xml .vstemplate file
                XmlDocument templateDoc = new XmlDocument();
                templateDoc.Load(templateFileName);
                XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(templateDoc.NameTable);
                xmlNamespaceManager.AddNamespace("vstemplns", "http://schemas.microsoft.com/developer/vstemplate/2005");
                #endregion
                #region Set VsTemplate data
                XmlNodeList templateData = templateDoc.SelectNodes("/vstemplns:VSTemplate/vstemplns:TemplateData", xmlNamespaceManager);
                foreach (XmlNode xmlNode in templateData[0].ChildNodes)
                {
                    CommandID valueAsCommandID = null;
                    string valueAsString = null;
                    XmlAttribute xmlPackage = xmlNode.Attributes["Package"];
                    if (xmlPackage != null)
                    {
                        XmlAttribute xmlId = xmlNode.Attributes["ID"];
                        Guid guidPackage = new Guid(xmlPackage.Value);
                        if (xmlId != null)
                        {
                            valueAsCommandID = new CommandID(guidPackage, int.Parse(xmlId.Value));
                        }
                    }
                    else
                    {
                        valueAsString = xmlNode.InnerText;
                    }
                    switch (xmlNode.Name)
                    {
                        case "Name":
                            this.name = valueAsString;
                            this.commandIdName = valueAsCommandID;
                            break;
                        case "Description":
                            this.description = valueAsString;
                            this.commandIdDescription = valueAsCommandID;
                            break;
                        case "Icon":
                            this.icon = valueAsCommandID;
                            if (valueAsString != null)
                            {
                                this.iconFileName = valueAsString;
                            }
                            break;
                        case "CreateNewFolder":
                            this.createNewFolder = bool.Parse(valueAsString);
                            break;
                        case "AppendDefaultFileExtension":
                            this.appendDefaultFileExtension = bool.Parse(valueAsString);
                            break;
                        case "SupportsLanguageDropDown":
                            this.supportsLanguageDropDown = bool.Parse(valueAsString);
                            break;
                        case "SupportsMasterPage":
                            this.supportsMasterPage = bool.Parse(valueAsString);
                            break;
                        case "ProvideDefaultName":
                            this.provideDefaultName = bool.Parse(valueAsString);
                            break;
                        case "SortOrder":
                            this.sortPriority = int.Parse(valueAsString);
                            break;
                        case "ProjectType":
                            this.language = valueAsString;
                            break;
                        case "DefaultName":
                            this.suggestedBaseName = valueAsString;
                            break;
                        case "EnableLocationBrowseButton":
                            this.enableLocationBrowseButton = bool.Parse(valueAsString);
                            break;
                        case "EnableEditOfLocationField":
                            this.enableEditOfLocationField = bool.Parse(valueAsString);
                            break;
                        case "LocationField":
                            this.locationField = (LocationField)Enum.Parse(typeof(LocationField), valueAsString);
                            break;
                        default:
                            break;
                    }
                }
                #endregion
                #region Set vsKind
                XmlNode projectTypeNode = templateDoc.SelectSingleNode("/vstemplns:VSTemplate/@Type", xmlNamespaceManager);
                if (projectTypeNode == null)
                {
                    throw new RecipeFrameworkException(Properties.Resources.Templates_MissingType);
                }
                else if (string.Compare(projectTypeNode.InnerText, "Project", true, CultureInfo.InvariantCulture) == 0)
                {
                    this.vsKind = WizardRunKind.AsNewProject;
                }
                else if (string.Compare(projectTypeNode.InnerText, "Item", true, CultureInfo.InvariantCulture) == 0)
                {
                    this.vsKind = WizardRunKind.AsNewItem;
                }
                else if (string.Compare(projectTypeNode.InnerText, "ProjectGroup", true, CultureInfo.InvariantCulture) == 0)
                {
                    this.vsKind = WizardRunKind.AsMultiProject;
                }
                #endregion
                #region Set Kind
                string folderName = Path.GetDirectoryName(this.FileName);
                string[] folders = folderName.Split('\\');
                string[] foldersArray = new string[2] { folders[folders.Length - 1], folders[folders.Length - 2] };
                foreach (string folder in foldersArray)
                {
                    if (folder.Equals("Items", StringComparison.InvariantCultureIgnoreCase))
                    {
                        kind = TemplateKind.ProjectItem;
                        break;
                    }
                    else if (folder.Equals("Projects", StringComparison.InvariantCultureIgnoreCase))
                    {
                        kind = TemplateKind.Project;
                        if (folders[0].Equals("Solutions"))
                        {
                        }
                        break;
                    }
                    else if (folder.Equals("Solutions", StringComparison.InvariantCultureIgnoreCase))
                    {
                        kind = TemplateKind.Solution;
                        break;
                    }
                }
                #endregion
                #region Set extension data
                XmlNode wizardData = templateDoc.SelectSingleNode("/vstemplns:VSTemplate/vstemplns:WizardData", xmlNamespaceManager);
                if (wizardData == null)
                {
                    this.extensionData = new VsTemplate.Template();
                }
                else
                {
                    this.extensionData = this.GetWizardData(wizardData.ChildNodes[0]);
                }
                #endregion
                #region Make sure that the there is an custom UI specified:
                try
                {
                    XmlNode wizardUINode = templateDoc.SelectSingleNode(
                        "/vstemplns:VSTemplate/vstemplns:WizardExtension", xmlNamespaceManager);
                    XmlNode wizardAsmNode = wizardUINode.SelectSingleNode(
                        "vstemplns:Assembly", xmlNamespaceManager);
                    XmlNode wizardClassNode = wizardUINode.SelectSingleNode(
                        "vstemplns:FullClassName", xmlNamespaceManager);

                    string extensionFullName = wizardClassNode.InnerText + ", " + wizardAsmNode.InnerText;
                    Type extensionType = ReflectionHelper.LoadType(extensionFullName);
                    if (extensionType == null)
                    {
                        throw new TypeLoadException(extensionFullName);
                    }
                    if (!(typeof(UnfoldTemplate).IsAssignableFrom(extensionType)))
                    {
                        throw new ArgumentException(Properties.Resources.Templates_NoExtension);
                    }
                }
                catch
                {
                    throw new ArgumentException(Properties.Resources.Templates_NoExtension);
                }
                #endregion
                #region Additional Checks
                CheckTemplateKinds();
                CheckTemplateReferences();
                #endregion
            }
            catch (Exception e)
            {
                throw new RecipeFrameworkException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resources.Templates_InvalidTemplateFile,
                        templateFileName), e);
            }
        }

        private void CheckTemplateKinds()
        {
            if (
                 !(
                    (Kind == TemplateKind.ProjectItem && VSKind == WizardRunKind.AsNewItem) ||
                    (Kind == TemplateKind.Project && (VSKind == WizardRunKind.AsMultiProject || VSKind == WizardRunKind.AsNewProject)) ||
                    (Kind == TemplateKind.Solution && (VSKind == WizardRunKind.AsMultiProject || VSKind == WizardRunKind.AsNewProject))
                  )
                )
            {
                throw new RecipeFrameworkException(String.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resources.Templates_KindMismatch,
                    Kind.ToString(), VSKind.ToString()));
            }
        }

        private void CheckTemplateReferences()
        {
            if (this.ExtensionData == null || this.ExtensionData.References == null)
            {
                return;
            }
            if (this.ExtensionData.References.RecipeReference != null)
            {
                foreach (VsTemplate.AssetReference reference in ExtensionData.References.RecipeReference)
                {
                    if (Kind == TemplateKind.ProjectItem)
                    {
                        if (!reference.Target.Equals("\\") && !reference.Target.Equals("/"))
                        {
                            // We got here because there's an Item template that contains an assetname other than "\".
                            throw new ArgumentException(String.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resources.Templates_ItemTargetInvalid,
                                Path.GetFileName(this.FileName), reference.Target), "Target");
                        }
                    }
                }
            }
            if (this.ExtensionData.References.TemplateReference != null)
            {
                if (Kind == TemplateKind.ProjectItem)
                {
                    throw new ArgumentException(String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resources.Templates_ItemCantHaveTemplates,
                        Path.GetFileName(this.FileName)),
                        "TemplateReference");
                }
                foreach (VsTemplate.AssetReference reference in this.ExtensionData.References.TemplateReference)
                {
                }
            }
        }

        #endregion

        #region Constructor

        public TemplateMetaData(string templateFileName, CommandID command, string packageName, RegistryKey registryRoot)
        {
            if (packageName == null)
            {
                throw new ArgumentNullException("PackageName");
            }
            if (projectFactories == null)
            {
                projectFactories = new Hashtable(7);
            }

            this.name = null;
            this.description = null;
            this.icon = null;
            this.templateFileName = templateFileName;
            this.command = command;
            this.packageName = packageName;
            this.ReadTemplate();
			this.regRoot = registryRoot;
            if (this.ProjectFactory.Equals(Guid.Empty))
            {
                // In case there is no project factory for this template then we must halt the installation.
                // We cannot use GUID_NULL, otherwise a corrupt registry entry will be entered and later on
                // devenv.exe /setup will fail with an AV exception.
                throw new RecipeFrameworkException(
                    String.Format(CultureInfo.CurrentCulture,
                        Properties.Resources.Templates_ProjectFactoryNotFound,
                        this.Language,
                        this.FileName));
            }
        }

        #endregion
    }
}
