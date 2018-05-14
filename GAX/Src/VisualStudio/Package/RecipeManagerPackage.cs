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
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms.Design;
using System.Xml;

using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.VisualStudio.Services;
using Microsoft.Practices.Common;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;
using Microsoft.Practices.RecipeFramework.VisualStudio.Properties;
using Microsoft.Practices.RecipeFramework.VisualStudio.ToolWindow;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.VisualStudio.Registration;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
    /// <summary>
    /// The RecipeFramework Package, this class implements a VS Package
    /// This Package also provides UI for Guidance Package management
    /// </summary>
    [Guid("77D93A80-73FC-40f8-87DB-ACD3482964B2")]
    [ProvideService(typeof(IRecipeManagerService))]
    [ProvideBindingPath]
    [ProvideObject(typeof(VszWizard))]
    [Registration.ProvidePseudoFolder(typeof(RecipeManagerPackage.GuidancePackagesPseudoFolder), "#102", 1000)]
    [Registration.ProvideVsTemplateInfo()]
    [ProvideMenuResource(1000, 1)]
    //TODO:Change VsHive
    [InstalledProductRegistration(false, "#100", "#101", "8.0", IconResourceID = 400)]
    [ServiceDependency(typeof(IServiceContainer))]
    [ServiceDependency(typeof(IMenuCommandService))]
    [ServiceDependency(typeof(SVsRegisterNewDialogFilters))]
    [ProvideToolWindow(typeof(GuidanceNavigatorWindow))]
    [PackageRegistration(UseManagedResourcesOnly = false, SatellitePath = "$PackageFolder$")]
    [RegisterAutoLoad(UIContextGuids.NoSolution)]
#if DEBUG
    [ProvidePackageSetting("SourceLevels", "Verbose")]
#else
    [ProvidePackageSetting("SourceLevels", "Verbose")]
#endif
	[ProvidePackageSetting("AutoShowGuidanceNavigator", 1)]
    internal sealed class RecipeManagerPackage : Package, IVsSolutionEvents, IVsSolutionEvents4, IVsTrackProjectDocumentsEvents2, IVsSolutionLoadEvents
    {
        #region Fields

        [Guid("C72B8446-B872-4d94-BD13-B0891CBCB4FF")]
        internal sealed class GuidancePackagesPseudoFolder { };
        uint projectsFilterCookie;
        uint itemsFilterCookie;

        static RecipeManagerPackage singleton;
        const String GaxUserDataPersistenceKey = "GaxUserData";
        const String GNavAutoClosedKey = "gNavAutoClosed";
        ToolWindow.GuidanceNavigatorManager guidanceNavigatorManager = null;

        OutputWindowService recipeFrameworkOutputWindow;

        #endregion

        #region Default Constructor

        public RecipeManagerPackage()
        {
            try
            {
                singleton = this;
                projectsFilterCookie = 0;
                itemsFilterCookie = 0;
                IServiceContainer container = (IServiceContainer)this;
                ServiceCreatorCallback callback = new ServiceCreatorCallback(this.OnCreateService);
                container.AddService(typeof(IRecipeManagerService), callback, true);
                container.AddService(typeof(IHostService), callback);
                container.AddService(typeof(IPersistenceService), callback);
                container.AddService(typeof(IVsTemplatesService), callback);
                container.AddService(typeof(IVsFilterAddProjectItemDlg), callback);
            }
            catch (Exception e)
            {
                ErrorHelper.Show(this, e);
            }
        }

        #endregion

        #region Overrides

        protected override void Initialize()
        {
            base.Initialize();
            try
            {
                Environment.SetEnvironmentVariable("RecipeFrameworkPath", Path.GetDirectoryName(this.GetType().Assembly.Location));
				LoadSourceLevelsSetting();

                AdviseSolutionEvents();
                AdviseTrackProjectDocumentsEvents();

                IVsRegisterNewDialogFilters filterService =
                    (IVsRegisterNewDialogFilters)GetService(typeof(SVsRegisterNewDialogFilters));
                filterService.RegisterAddNewItemDialogFilter(
                    (IVsFilterAddProjectItemDlg)GetService(typeof(IVsFilterAddProjectItemDlg)), out itemsFilterCookie);
                filterService.RegisterNewProjectDialogFilter(
                    (IVsFilterNewProjectDlg)GetService(typeof(IVsFilterAddProjectItemDlg)), out projectsFilterCookie);

                AddOptionKey(GaxUserDataPersistenceKey);

                ServiceHelper.CheckDependencies(this, (System.IServiceProvider)this);

                guidanceNavigatorManager = new GuidanceNavigatorManager(this as System.IServiceProvider);

                IRecipeManagerService rms = (IRecipeManagerService)GetService(typeof(IRecipeManagerService));
                rms.EnablingPackage += OnEnablingPackage;
                rms.EnabledPackage += OnEnabledPackage;
            }
            catch (Exception e)
            {
                ErrorHelper.Show(this, e);
                throw;
            }
        }

        protected override int QueryClose(out bool canClose)
        {
            canClose = true;
            // Remove and dispose the output window if appropriate.
            IServiceContainer container = base.GetService(typeof(IServiceContainer)) as IServiceContainer;
            IDisposable outwindow = GetService(typeof(IOutputWindowService)) as IDisposable;
            if (outwindow != null)
            {
                container.RemoveService(typeof(IOutputWindowService));
                outwindow.Dispose();
            }
            return VSConstants.S_OK;
        }

        protected override void Dispose(bool disposing)
        {
            IVsRegisterNewDialogFilters filterService =
                (IVsRegisterNewDialogFilters)GetService(typeof(SVsRegisterNewDialogFilters));
            if (itemsFilterCookie != 0)
            {
                if (filterService != null)
                {
                    filterService.UnregisterAddNewItemDialogFilter(itemsFilterCookie);
                }
                itemsFilterCookie = 0;
            }
            if (projectsFilterCookie != 0)
            {
                if (filterService != null)
                {
                    filterService.UnregisterNewProjectDialogFilter(projectsFilterCookie);
                }
                projectsFilterCookie = 0;
            }

            UnadviseSolutionEvents();
            UnadviseTrackProjectDocumentsEvents();
            base.Dispose(disposing);
        }

        #endregion

        #region Service Creation

        private object OnCreateService(IServiceContainer container, Type serviceType)
        {
            object newService = null;
            if (serviceType == typeof(IRecipeManagerService))
            {
                newService = new RecipeManager();
            }
            else if (serviceType == typeof(IHostService))
            {
                newService = new SolutionPackagesContainer();
            }
            else if (serviceType == typeof(IPersistenceService))
            {
                // REVIEW (v-vaprea): this assumes the type implementing IHostService will always
                // implements IPersistenceService, not really nice...
                return GetService(typeof(IHostService));
            }
            else if (serviceType == typeof(IVsTemplatesService))
            {
                newService = new VSTemplatesService();
            }
            else if (serviceType == typeof(IVsFilterAddProjectItemDlg))
            {
                newService = (IVsFilterAddProjectItemDlg)(new Templates.TemplateFilter());
            }
            // Now, let's site the new service
            if (newService != null && newService is IComponent)
            {
                // Site it so VS acts as the container.
                ((IComponent)newService).Site =
                    new Site(this as System.IServiceProvider, newService as IComponent, newService.GetType().Name);
                ServiceHelper.CheckDependencies((IComponent)newService);
            }
            return newService;
        }

        #endregion

        #region Solution User Options (.suo) handling

        Hashtable solutionUserOptions = new Hashtable();

        internal void AddSolutionUserOption(String key, object value)
        {
            solutionUserOptions.Add(key, value);
        }

        internal object GetSolutionUserOption(String key, object value)
        {
            return solutionUserOptions[key];
        }

        internal void RemoveSolutionUserOption(String key, object value)
        {
            solutionUserOptions.Remove(key);
        }

        protected override void OnLoadOptions(string key, Stream stream)
        {
            try
            {
                if (key.Equals(GaxUserDataPersistenceKey))
                {
                    guidanceNavigatorManager.LoadState(new BinaryFormatter().Deserialize(stream));
                }
            }
            catch (Exception e)
            {
                ErrorHelper.Show(this, e);
            }
        }

        protected override void OnSaveOptions(string key, Stream stream)
        {
            try
            {
                if (key.Equals(GaxUserDataPersistenceKey))
                {
                    new BinaryFormatter().Serialize(stream, guidanceNavigatorManager.SaveState());
                }
            }
            catch (Exception e)
            {
                ErrorHelper.Show(this, e);
            }
        }

        #endregion

        #region Solution Events

        uint solutionEventsCookie = 0;

        internal void AdviseSolutionEvents()
        {
            //Setup IVsSolutionEvents sink
            if (solutionEventsCookie == 0)
            {
                IVsSolution vsSolution = (IVsSolution)GetService(typeof(IVsSolution));
                vsSolution.AdviseSolutionEvents(this, out solutionEventsCookie);
            }
        }

        internal void UnadviseSolutionEvents()
        {
            if (solutionEventsCookie != 0)
            {
                IVsSolution vsSolution = (IVsSolution)GetService(typeof(IVsSolution));
                vsSolution.UnadviseSolutionEvents(solutionEventsCookie);
                solutionEventsCookie = 0;
            }
        }

        #endregion

        #region Tracking Project Documents changes
        uint trackProjectDocumentsCookie = 0;
        internal void AdviseTrackProjectDocumentsEvents()
        {
            if (trackProjectDocumentsCookie == 0)
            {
                IVsTrackProjectDocuments2 service = (IVsTrackProjectDocuments2)GetService(typeof(SVsTrackProjectDocuments));
                service.AdviseTrackProjectDocumentsEvents(this, out trackProjectDocumentsCookie);
            }
        }

        internal void UnadviseTrackProjectDocumentsEvents()
        {
            if (trackProjectDocumentsCookie != 0)
            {
                IVsTrackProjectDocuments2 service = (IVsTrackProjectDocuments2)GetService(typeof(SVsTrackProjectDocuments));
                service.UnadviseTrackProjectDocumentsEvents(trackProjectDocumentsCookie);
            }
        }
        #endregion

        #region IVsSolutionEvents Members

        int IVsSolutionEvents.OnAfterCloseSolution(object pUnkReserved)
        {
            IRecipeManagerService rms = (IRecipeManagerService)GetService(typeof(IRecipeManagerService));
            rms.EnabledPackage -= new PackageEventHandler(OnEnabledPackage);
            //rms.EnablingPackage -= new PackageEventHandler(rms_EnabledPackage);

            guidanceNavigatorManager.Reset();

            if (IsGuidanceNavigatorToolWindowVisible())
            {
                SetGNavAutoClosedKeyValue(true);
                HideGuidanceNavigatorWindow();
            }
            else
            {
                SetGNavAutoClosedKeyValue(false);
            }

            IVsSolutionEvents hostService =
                (IVsSolutionEvents)GetService(typeof(IHostService));
            return hostService.OnAfterCloseSolution(pUnkReserved);
        }

        void SetGNavAutoClosedKeyValue(bool value)
        {
            DTE dte = (DTE)this.GetService(typeof(DTE));
            dte.Globals[GNavAutoClosedKey] = value;
            dte.Globals.set_VariablePersists(GNavAutoClosedKey, true);
        }

        int IVsSolutionEvents.OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            guidanceNavigatorManager.OnSolutionExplorerChanged();

            IVsSolutionEvents hostService =
                (IVsSolutionEvents)GetService(typeof(IHostService));
            return hostService.OnAfterLoadProject(pStubHierarchy, pRealHierarchy);
        }

        int IVsSolutionEvents.OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            IVsSolutionEvents hostService =
                (IVsSolutionEvents)GetService(typeof(IHostService));
            return hostService.OnAfterOpenProject(pHierarchy, fAdded);
        }

        bool isOpeningSolution = false;

        int IVsSolutionEvents.OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            if (fNewSolution == 0)
            {
                isOpeningSolution = true;
            }

            IVsSolutionEvents hostService =
                (IVsSolutionEvents)GetService(typeof(IHostService));
            return hostService.OnAfterOpenSolution(pUnkReserved, fNewSolution);
        }


        int IVsSolutionEvents.OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            IVsSolutionEvents hostService =
                (IVsSolutionEvents)GetService(typeof(IHostService));
            return hostService.OnBeforeCloseProject(pHierarchy, fRemoved);
        }

        int IVsSolutionEvents.OnBeforeCloseSolution(object pUnkReserved)
        {
            IVsSolutionEvents hostService =
                (IVsSolutionEvents)GetService(typeof(IHostService));
            return hostService.OnBeforeCloseSolution(pUnkReserved);
        }

        int IVsSolutionEvents.OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            guidanceNavigatorManager.OnSolutionExplorerChanged();

            IVsSolutionEvents hostService =
                (IVsSolutionEvents)GetService(typeof(IHostService));
            return hostService.OnBeforeUnloadProject(pRealHierarchy, pStubHierarchy);
        }

        int IVsSolutionEvents.OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            IVsSolutionEvents hostService =
                (IVsSolutionEvents)GetService(typeof(IHostService));
            return hostService.OnQueryCloseProject(pHierarchy, fRemoving, ref pfCancel);
        }

        int IVsSolutionEvents.OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            IVsSolutionEvents hostService =
                (IVsSolutionEvents)GetService(typeof(IHostService));
            return hostService.OnQueryCloseSolution(pUnkReserved, ref pfCancel);
        }

        int IVsSolutionEvents.OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            guidanceNavigatorManager.OnSolutionExplorerChanged();

            IVsSolutionEvents hostService =
                (IVsSolutionEvents)GetService(typeof(IHostService));
            return hostService.OnQueryUnloadProject(pRealHierarchy, ref pfCancel);
        }

        #endregion

        #region IVsSolutionEvents4 Members

        int IVsSolutionEvents4.OnAfterAsynchOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            IVsSolutionEvents4 hostService =
                (IVsSolutionEvents4)GetService(typeof(IHostService));
            return hostService.OnAfterAsynchOpenProject(pHierarchy, fAdded);
        }

        int IVsSolutionEvents4.OnAfterChangeProjectParent(IVsHierarchy pHierarchy)
        {
            // REVIEW (v-vaprea): this doesn't look good -- the code is asking for the IHostService only to then
            // cast the returned object to an IVsSolutionEvents4 interface thus assuming that the type implementing
            // IHostService also implements this other interface... not nice at all... (the same 'pattern' is used on other
            // methods in this class too.
            IVsSolutionEvents4 hostService =
                (IVsSolutionEvents4)GetService(typeof(IHostService));
            return hostService.OnAfterChangeProjectParent(pHierarchy);
        }

        int IVsSolutionEvents4.OnAfterRenameProject(IVsHierarchy pHierarchy)
        {
            guidanceNavigatorManager.OnSolutionExplorerChanged();

            IVsSolutionEvents4 hostService =
                (IVsSolutionEvents4)GetService(typeof(IHostService));
            return hostService.OnAfterRenameProject(pHierarchy);
        }

        int IVsSolutionEvents4.OnQueryChangeProjectParent(IVsHierarchy pHierarchy, IVsHierarchy pNewParentHier, ref int pfCancel)
        {
            IVsSolutionEvents4 hostService =
                (IVsSolutionEvents4)GetService(typeof(IHostService));
            return hostService.OnQueryChangeProjectParent(pHierarchy, pNewParentHier, ref pfCancel);
        }

        #endregion

        static internal RecipeManagerPackage Singleton
        {
            get
            {
                return singleton;
            }
        }

        internal GuidanceNavigatorManager GuidanceNavigatorManager
        {
            get
            {
                return this.guidanceNavigatorManager;
            }
        }

        void OnEnabledPackage(object sender, PackageEventArgs e)
        {
            //// if a new solution is being created this means packages are enabled for the first time
            //// so gNav should show by default
            if (!isOpeningSolution)
            {
                // if the user explicitly added a 'AutoShowGuidanceNavigator' set to 0 in the registry
                // the following condition won't match and then we'll refrain from showing
                if (AutoShowGuidanceNavigator())
                {
                    ShowGuidanceNavigatorWindow();
                }
            }
            // if a solution is being opened
            else
            {
                DTE dte = (DTE)GetService(typeof(DTE));
                if (dte.Globals.get_VariableExists(GNavAutoClosedKey))
                {
                    bool autoClosed = (bool)dte.Globals[GNavAutoClosedKey];
                    if (autoClosed)
                    {
                        ShowGuidanceNavigatorWindow();
                    }
                }
            }
        }

        void OnEnablingPackage(object sender, CancelPackageEventArgs e)
        {
            if (recipeFrameworkOutputWindow == null)
            {
                recipeFrameworkOutputWindow = TraceUtil.GaxOutputWindowService;
                recipeFrameworkOutputWindow.Site = new Site((IServiceContainer)this, recipeFrameworkOutputWindow, recipeFrameworkOutputWindow.WindowName);
                ((IServiceContainer)this).AddService(typeof(IOutputWindowService), recipeFrameworkOutputWindow);
			}	

            // if a new solution is being created this means packages are enabled for the first time
            // so gNav should show by default
            if (!isOpeningSolution)
            {
                // if the user explicitly added a 'AutoShowGuidanceNavigator' set to 0 in the registry
                // the following condition won't match and then we'll refrain from showing
                if (AutoShowGuidanceNavigator())
                {
                    ShowGuidanceNavigatorWindow();
                }
            }
            // if a solution is being opened
            else
            {
                DTE dte = (DTE)GetService(typeof(DTE));
                if (dte.Globals.get_VariableExists(GNavAutoClosedKey))
                {
                    bool autoClosed = (bool)dte.Globals[GNavAutoClosedKey];
                    if (autoClosed)
                    {
                        ShowGuidanceNavigatorWindow();
                    }
                }
            }
        }

        #region Guidance Navigator

        internal void ShowGuidanceNavigatorWindow()
        {
            IVsWindowFrame windowFrame = GetGuidanceNavigatorWindowFrame();
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        internal bool IsGuidanceNavigatorToolWindowVisible()
        {
            IVsWindowFrame windowFrame = GetGuidanceNavigatorWindowFrame();
            // yes, IsVisible will return 0 when the window is visible and 1 when it's not...
            if (windowFrame.IsVisible() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void HideGuidanceNavigatorWindow()
        {
            IVsWindowFrame windowFrame = GetGuidanceNavigatorWindowFrame();
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Hide());
        }

        private IVsWindowFrame GetGuidanceNavigatorWindowFrame()
        {
            ToolWindowPane window = FindToolWindow(typeof(GuidanceNavigatorWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new COMException(Resources.GuidanceNavigatorWindow_CannotCreate);
            }

            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            return windowFrame;
        }


        void ResetGuidanceNavigatorManager()
        {
            if (guidanceNavigatorManager != null)
            {
                guidanceNavigatorManager.Reset();
            }
        }

        bool AutoShowGuidanceNavigator()
        {
            bool shouldAutoShow = true;

            using (var vsRegistryKey = RegistryHelper.GetCurrentVsRegistryKey(false))
            {
                using (var regKey = vsRegistryKey.OpenSubKey(ProvidePackageSetting.GetRegistryKey(typeof(RecipeManagerPackage))))
                {

                    if (regKey != null)
                    {
                        object value = regKey.GetValue("AutoShowGuidanceNavigator");
                        if (value != null)
                        {
                            if ((int)value == 1)
                            {
                                shouldAutoShow = true;
                            }
                            else
                            {
                                shouldAutoShow = false;
                            }
                        }
                    }
                }
            }

            return shouldAutoShow;
        }

        #endregion

        #region IVsTrackProjectDocumentsEvents2 Members

        int IVsTrackProjectDocumentsEvents2.OnAfterAddDirectoriesEx(int cProjects, int cDirectories, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSADDDIRECTORYFLAGS[] rgFlags)
        {
            guidanceNavigatorManager.OnSolutionExplorerChanged();
            return VSConstants.S_OK;
        }

        int IVsTrackProjectDocumentsEvents2.OnAfterAddFilesEx(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSADDFILEFLAGS[] rgFlags)
        {
            guidanceNavigatorManager.OnSolutionExplorerChanged();
            return VSConstants.S_OK;
        }

        int IVsTrackProjectDocumentsEvents2.OnAfterRemoveDirectories(int cProjects, int cDirectories, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSREMOVEDIRECTORYFLAGS[] rgFlags)
        {
            guidanceNavigatorManager.OnSolutionExplorerChanged();
            return VSConstants.S_OK;
        }

        int IVsTrackProjectDocumentsEvents2.OnAfterRemoveFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSREMOVEFILEFLAGS[] rgFlags)
        {
            guidanceNavigatorManager.OnSolutionExplorerChanged();
            return VSConstants.S_OK;
        }

        int IVsTrackProjectDocumentsEvents2.OnAfterRenameDirectories(int cProjects, int cDirs, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgszMkOldNames, string[] rgszMkNewNames, VSRENAMEDIRECTORYFLAGS[] rgFlags)
        {
            guidanceNavigatorManager.OnSolutionExplorerChanged();
            return VSConstants.S_OK;
        }

        int IVsTrackProjectDocumentsEvents2.OnAfterRenameFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgszMkOldNames, string[] rgszMkNewNames, VSRENAMEFILEFLAGS[] rgFlags)
        {
            guidanceNavigatorManager.OnSolutionExplorerChanged();
            return VSConstants.S_OK;
        }

        int IVsTrackProjectDocumentsEvents2.OnAfterSccStatusChanged(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, uint[] rgdwSccStatus)
        {
            return VSConstants.S_OK;
        }

        int IVsTrackProjectDocumentsEvents2.OnQueryAddDirectories(IVsProject pProject, int cDirectories, string[] rgpszMkDocuments, VSQUERYADDDIRECTORYFLAGS[] rgFlags, VSQUERYADDDIRECTORYRESULTS[] pSummaryResult, VSQUERYADDDIRECTORYRESULTS[] rgResults)
        {
            return VSConstants.S_OK;
        }

        int IVsTrackProjectDocumentsEvents2.OnQueryAddFiles(IVsProject pProject, int cFiles, string[] rgpszMkDocuments, VSQUERYADDFILEFLAGS[] rgFlags, VSQUERYADDFILERESULTS[] pSummaryResult, VSQUERYADDFILERESULTS[] rgResults)
        {
            return VSConstants.S_OK;
        }

        int IVsTrackProjectDocumentsEvents2.OnQueryRemoveDirectories(IVsProject pProject, int cDirectories, string[] rgpszMkDocuments, VSQUERYREMOVEDIRECTORYFLAGS[] rgFlags, VSQUERYREMOVEDIRECTORYRESULTS[] pSummaryResult, VSQUERYREMOVEDIRECTORYRESULTS[] rgResults)
        {
            return VSConstants.S_OK;
        }

        int IVsTrackProjectDocumentsEvents2.OnQueryRemoveFiles(IVsProject pProject, int cFiles, string[] rgpszMkDocuments, VSQUERYREMOVEFILEFLAGS[] rgFlags, VSQUERYREMOVEFILERESULTS[] pSummaryResult, VSQUERYREMOVEFILERESULTS[] rgResults)
        {
            return VSConstants.S_OK;
        }

        int IVsTrackProjectDocumentsEvents2.OnQueryRenameDirectories(IVsProject pProject, int cDirs, string[] rgszMkOldNames, string[] rgszMkNewNames, VSQUERYRENAMEDIRECTORYFLAGS[] rgFlags, VSQUERYRENAMEDIRECTORYRESULTS[] pSummaryResult, VSQUERYRENAMEDIRECTORYRESULTS[] rgResults)
        {
            return VSConstants.S_OK;
        }

        int IVsTrackProjectDocumentsEvents2.OnQueryRenameFiles(IVsProject pProject, int cFiles, string[] rgszMkOldNames, string[] rgszMkNewNames, VSQUERYRENAMEFILEFLAGS[] rgFlags, VSQUERYRENAMEFILERESULTS[] pSummaryResult, VSQUERYRENAMEFILERESULTS[] rgResults)
        {
            return VSConstants.S_OK;
        }

		#endregion

		static bool sourceSwitchLoaded = false;

		private static void LoadSourceLevelsSetting()
        {
			if (sourceSwitchLoaded)
				return;
			sourceSwitchLoaded = true;
			// Set the default
			SourceLevels level = SourceLevels.Error;
            try
            {
                using (var vsRegistryKey = RegistryHelper.GetCurrentVsRegistryKey(false))
                {
                    using (var settingsKey = vsRegistryKey.OpenSubKey(ProvidePackageSetting.GetRegistryKey(typeof(RecipeManagerPackage))))
                    {
                        if (settingsKey != null)
                        {
                            var sourceLevelsValue = settingsKey.GetValue("SourceLevels", "Error") as string;

							SourceLevels result;
                            if (Enum.TryParse<SourceLevels>(sourceLevelsValue, out result))
                            {
								level = result;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Set the default
                level = SourceLevels.Error;
            }

            TraceUtil.GaxSourceSwitch.Level = level;
        }

        #region IVsSolutionLoadEvents Members

        int IVsSolutionLoadEvents.OnAfterBackgroundSolutionLoadComplete()
        {
            if (guidanceNavigatorManager == null)
            {
                guidanceNavigatorManager = new GuidanceNavigatorManager(this as System.IServiceProvider);
            }

            IRecipeManagerService rms = (IRecipeManagerService)GetService(typeof(IRecipeManagerService));
            rms.EnabledPackage += new PackageEventHandler(OnEnabledPackage);

            IVsSolutionLoadEvents hostService =
                (IVsSolutionLoadEvents)GetService(typeof(IHostService));
            int result = hostService.OnAfterBackgroundSolutionLoadComplete();

            isOpeningSolution = false;
            return result;
        }

        int IVsSolutionLoadEvents.OnAfterLoadProjectBatch(bool fIsBackgroundIdleBatch)
        {
            IVsSolutionLoadEvents hostService =
                (IVsSolutionLoadEvents)GetService(typeof(IHostService));
            return hostService.OnAfterLoadProjectBatch(fIsBackgroundIdleBatch);
        }

        int IVsSolutionLoadEvents.OnBeforeBackgroundSolutionLoadBegins()
        {
            IVsSolutionLoadEvents hostService =
                (IVsSolutionLoadEvents)GetService(typeof(IHostService));
            return hostService.OnBeforeBackgroundSolutionLoadBegins();
        }

        int IVsSolutionLoadEvents.OnBeforeLoadProjectBatch(bool fIsBackgroundIdleBatch)
        {
            IVsSolutionLoadEvents hostService =
                (IVsSolutionLoadEvents)GetService(typeof(IHostService));
            return hostService.OnBeforeLoadProjectBatch(fIsBackgroundIdleBatch);
        }

        int IVsSolutionLoadEvents.OnBeforeOpenSolution(string pszSolutionFilename)
        {
            IVsSolutionLoadEvents hostService =
                (IVsSolutionLoadEvents)GetService(typeof(IHostService));
            return hostService.OnBeforeOpenSolution(pszSolutionFilename);
        }

        int IVsSolutionLoadEvents.OnQueryBackgroundLoadProjectBatch(out bool pfShouldDelayLoadToNextIdle)
        {
            IVsSolutionLoadEvents hostService =
                (IVsSolutionLoadEvents)GetService(typeof(IHostService));
            return hostService.OnQueryBackgroundLoadProjectBatch(out pfShouldDelayLoadToNextIdle);
        }

        #endregion
    }
}