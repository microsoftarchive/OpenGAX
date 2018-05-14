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
using System.Xml;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using EnvDTE;
using EnvDTE80;

using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;
using Microsoft.Practices.RecipeFramework.VisualStudio.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;
using System.Xml.Serialization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Properties;
using Microsoft.Practices.RecipeFramework.VisualStudio.ToolWindow;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
	[ServiceDependency(typeof(IRecipeManagerService))]
    [ServiceDependency(typeof(IVsSolution))]
    [ServiceDependency(typeof(DTE))]
    [ServiceDependency(typeof(IVsTemplatesService))]
    internal sealed class SolutionPackagesContainer : Microsoft.Practices.ComponentModel.ServiceContainer, IHostService, IVsSolutionEvents, IVsSolutionEvents4, IPersistenceService, IOleComponent, IVsSolutionLoadEvents
    {
        [Serializable]
        private struct PackageHeader
        {
            public PackageHeader(String name, String version, bool bindingRecipeRun)
            {
                Name = name;
                Version = version;
                BindingRecipeRun = bindingRecipeRun;
            }
            public String Name;
            public String Version;
            public bool BindingRecipeRun;
        }

        #region Fields

        IRecipeManagerService recipeManager;
        OleMenuCommand guidancePackageManagerMenuCmd;
        OleMenuCommand guidanceNavigatorMenuCmd;
        uint componentID;
        IOleComponentManager oleComponentManager;

        const String GaxExternalStateFileExtension = ".gpState";
        List<GuidancePackagesStatePackage> preservedPackages;

        #endregion

        #region Event handlers

        // See VSWhidbey bug #445930 on why we keep these references.
        SolutionEvents solutionEvents;
        ProjectItemsEvents projectItemsEvents;
        ProjectItemsEvents solutionItemsEvents;

        #endregion

        #region Default constructor

        public SolutionPackagesContainer()
            : base(true)
        {
            this.oleComponentManager = null;
            this.componentID = 0;
            this.packagePersistenceData = new Dictionary<string, PackageState>(7);
            preservedPackages = new List<GuidancePackagesStatePackage>();
            AddService(typeof(IPersistenceService), this);
        }

        #endregion

        #region Overrides

        protected override void OnSited()
        {
            base.OnSited();

            //Setup DTE Events
            try
            {
                oleComponentManager =
                    (IOleComponentManager)GetService(typeof(SOleComponentManager));
                if (oleComponentManager != null)
                {
                    OLECRINFO crinfo = new OLECRINFO();
                    crinfo.cbSize = (uint)Marshal.SizeOf(crinfo);
                    crinfo.grfcrf = (uint)(_OLECRF.olecrfNeedIdleTime | _OLECRF.olecrfNeedPeriodicIdleTime);
                    crinfo.grfcadvf = (uint)(_OLECADVF.olecadvfModal | _OLECADVF.olecadvfRedrawOff | _OLECADVF.olecadvfWarningsOff);
                    crinfo.uIdleTimeInterval = 1000;
                    oleComponentManager.FRegisterComponent(this,
                        new OLECRINFO[] { crinfo },
                        out this.componentID);
                }

                // Add GAX commands
                OleMenuCommandService mcs = GetService(typeof(IMenuCommandService), true) as OleMenuCommandService;
                if (mcs != null)
                {
                    // Add command handlers for Guidance Package Manager menu (commands must exist in the .ctc file)
                    CommandID cmd = new CommandID(CTC.guidRecipeManagerCmdSet, CTC.icmdRecipeManagerCommand);
                    guidancePackageManagerMenuCmd = new OleMenuCommand(new EventHandler(OnRecipeManagerCommand), cmd);
                    guidancePackageManagerMenuCmd.Supported = true;
                    guidancePackageManagerMenuCmd.Visible = true;
                    guidancePackageManagerMenuCmd.Enabled = true;
                    guidancePackageManagerMenuCmd.BeforeQueryStatus += new EventHandler(OnRecipeManagerQueryStatus);
                    mcs.AddCommand(guidancePackageManagerMenuCmd);

                    // Add command handlers for Guidance Navigator Window menu (commands must exist in the .ctc file)
                    CommandID toolWindowCmd = new CommandID(CTC.guidRecipeManagerCmdSet, (int)CTC.icmdNavigatorWindowCommand);
                    guidanceNavigatorMenuCmd = new OleMenuCommand(new EventHandler(OnGuidanceNavigatorCommand), toolWindowCmd);
                    guidanceNavigatorMenuCmd.Supported = true;
                    guidanceNavigatorMenuCmd.Visible = true;
                    guidanceNavigatorMenuCmd.Enabled = true;
                    guidanceNavigatorMenuCmd.BeforeQueryStatus += new EventHandler(OnGuidanceNavigatorQueryStatus);
                    mcs.AddCommand(guidanceNavigatorMenuCmd);
                }

                //Setup recipe manager events
                recipeManager = GetService<IRecipeManagerService>(true);
                recipeManager.EnabledPackage += new PackageEventHandler(OnEnabledPackage);
                recipeManager.DisablingPackage += new CancelPackageEventHandler(OnDisablingPackage);
                recipeManager.EnablingPackage += new CancelPackageEventHandler(OnEnablingPackage);

                DTE dte = GetService<DTE>(true);
                DTE2 dte2 = (DTE2)dte;

                this.solutionEvents = dte.Events.SolutionEvents;
                solutionEvents.ProjectRemoved += new _dispSolutionEvents_ProjectRemovedEventHandler(OnProjectRemoved);
                solutionEvents.ProjectRenamed += new _dispSolutionEvents_ProjectRenamedEventHandler(OnProjectRenamed);
                solutionEvents.ProjectAdded += new _dispSolutionEvents_ProjectAddedEventHandler(OnProjectAdded);

                this.projectItemsEvents = ((Events2)dte2.Events).ProjectItemsEvents;
                projectItemsEvents.ItemRenamed += new _dispProjectItemsEvents_ItemRenamedEventHandler(OnProjectItemRenamed);
                projectItemsEvents.ItemRemoved += new _dispProjectItemsEvents_ItemRemovedEventHandler(OnProjectItemRemoved);

                this.solutionItemsEvents = dte.Events.SolutionItemsEvents;
                solutionItemsEvents.ItemRemoved += new _dispProjectItemsEvents_ItemRemovedEventHandler(OnProjectItemRemoved);
                solutionItemsEvents.ItemRenamed += new _dispProjectItemsEvents_ItemRenamedEventHandler(OnProjectItemRenamed);

            }
            catch (Exception ex)
            {
                this.TraceError(ex.ToString());
                throw new RecipeFrameworkException(
                    Properties.Resources.Package_FailedToLoad, ex);
            }
        }

        void OnGuidanceNavigatorCommand(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.

            RecipeManagerPackage pkg = RecipeManagerPackage.Singleton;
            ToolWindowPane window = pkg.FindToolWindow(typeof(GuidanceNavigatorWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new COMException(Resources.GuidanceNavigatorWindow_CannotCreate);
            }
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        /// <summary>
        /// Decides wether the option to show the Guidance Navigator tool window should be enabled or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnGuidanceNavigatorQueryStatus(object sender, EventArgs e)
        {
            DTE dte = GetService<DTE>(true);
            guidanceNavigatorMenuCmd.Visible = dte.Solution.IsOpen;
        }

        void OnRecipeManagerQueryStatus(object sender, EventArgs e)
        {
            DTE dte = GetService<DTE>(true);
            guidancePackageManagerMenuCmd.Visible = dte.Solution.IsOpen;
        }

        protected override void Dispose(bool disposing)
        {
            if (preservedPackages != null)
            {
                preservedPackages.Clear();
                preservedPackages = null;
            }
            if (packagePersistenceData != null)
            {
                // Let's remove the bag of state too.
                packagePersistenceData.Clear();
                packagePersistenceData = null;
            }
            if (recipeManager != null)
            {
                recipeManager.EnabledPackage -= new PackageEventHandler(OnEnabledPackage);
                recipeManager.EnablingPackage -= new CancelPackageEventHandler(OnEnablingPackage);
                recipeManager.DisablingPackage -= new CancelPackageEventHandler(OnDisablingPackage);
                recipeManager = null;
            }
            // The DTE object is invalid when exiting VS.
            // Any access to it will throw an AV exception
            //if (solutionEvents != null)
            //{
            //    solutionEvents.ProjectRemoved -= new _dispSolutionEvents_ProjectRemovedEventHandler(OnProjectRemoved);
            //    solutionEvents.ProjectRenamed -= new _dispSolutionEvents_ProjectRenamedEventHandler(OnProjectRenamed);
            //    this.solutionEvents = null;
            //}
            //if ( projectItemsEvents != null )
            //{
            //    projectItemsEvents.ItemRemoved -= new _dispProjectItemsEvents_ItemRemovedEventHandler(OnProjectItemRemoved);
            //    projectItemsEvents.ItemRenamed -= new _dispProjectItemsEvents_ItemRenamedEventHandler(OnProjectItemRenamed);
            //    this.projectItemsEvents = null;
            //}
            //if (solutionItemsEvents != null)
            //{
            //    solutionItemsEvents.ItemRemoved -= new _dispProjectItemsEvents_ItemRemovedEventHandler(OnProjectItemRemoved);
            //    solutionItemsEvents.ItemRenamed -= new _dispProjectItemsEvents_ItemRenamedEventHandler(OnProjectItemRenamed);
            //    this.solutionItemsEvents = null;
            //}
            //if (menuCmd != null)
            //{
            //    menuCmd.BeforeQueryStatus -= new EventHandler(OnRecipeManagerQueryStatus);
            //    OleMenuCommandService mcs = GetService(typeof(IMenuCommandService), false) as OleMenuCommandService;
            //    if (mcs != null)
            //    {
            //        mcs.RemoveCommand(menuCmd);
            //    }
            //    menuCmd = null;
            //}

            if (oleComponentManager != null)
            {
                oleComponentManager.FRevokeComponent(this.componentID);
                this.oleComponentManager = null;
                this.componentID = 0;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Standard (Non-guidancepackage) Command Handlers

        internal class CTC
        {
            public static readonly Guid guidRecipeManagerPkg = typeof(RecipeManagerPackage).GUID;

            public static readonly Guid guidRecipeManagerCmdSet = new Guid("{96119584-8C4B-4910-9211-71D48FA59FAF}");
            public const int icmdRecipeManagerCommand = 0x100;
            public const int icmdNavigatorWindowCommand = 0x101;

        };

        private void OnRecipeManagerCommand(object sender, EventArgs eventArgs)
        {
            try
            {
                IUIService uiService = GetService<IUIService>();
                IRecipeManagerService manager = GetService<IRecipeManagerService>();
                if (manager == null)
                {
                    this.TraceError(
                        Properties.Resources.Package_CantGetManagerService);
                    if (uiService != null)
                    {
                        uiService.ShowError(Properties.Resources.Package_CantGetManagerService);
                    }
                    return;
                }
                EnvDTE.DTE vs = GetService<EnvDTE.DTE>();
                if (vs != null && vs.Solution != null)
                {
                    object currentSelection = null;
                    if (vs.SelectedItems.Count != 0)
                    {
                        SelectedItem item = vs.SelectedItems.Item(1);
                        // Determine target.
                        if (item.Project != null)
                        {
                            currentSelection = item.Project;
                        }
                        else if (item.ProjectItem != null)
                        {
                            currentSelection = item.ProjectItem;
                        }
                        else
                        {
                            currentSelection = vs.Solution;
                        }
                    }
                    using (PackageManagement.PackageManager form = new PackageManagement.PackageManager(
                        manager as System.IServiceProvider, currentSelection))
                    {
                        if (uiService == null)
                        {
                            form.ShowDialog();
                        }
                        else
                        {
                            uiService.ShowDialog(form);
                        }
                    }
                }
                else
                {
                    if (uiService != null)
                    {
                        uiService.ShowMessage(Properties.Resources.Package_NoSolutionOpened);
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHelper.Show(this, e);
            }
        }

        #endregion

        #region IVsSolutionEvents members

        int IVsSolutionEvents.OnAfterCloseSolution(object pUnkReserved)
        {
            try
            {
                // hide & disable the "Guidance Package Manager" option
                guidancePackageManagerMenuCmd.Enabled = false;
                guidancePackageManagerMenuCmd.Visible = false;
                // hide & disable the "Guidance Package Navigator" show toolwindow command
                guidanceNavigatorMenuCmd.Enabled = false;
                guidanceNavigatorMenuCmd.Visible = false;
                // close the "Guidance Package Navigator" toolwindow if it is open


                packagePersistenceData.Clear();
                preservedPackages.Clear();
                return VSConstants.S_OK;
            }
            catch (Exception e)
            {
                ErrorHelper.Show(this, e);
                return VSConstants.E_FAIL;
            }
        }

        int IVsSolutionEvents.OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsSolutionEvents.OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsSolutionEvents.OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsSolutionEvents.OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsSolutionEvents.OnBeforeCloseSolution(object pUnkReserved)
        {
            try
            {
                // save packages bindings and state for the current solution
                SavePackagesData();

                //Unload every single package
                IRecipeManagerService recipeManager = GetService<IRecipeManagerService>(true);
                IContainer packages = (IContainer)recipeManager;
                foreach (IComponent component in packages.Components)
                {
                    if (component is GuidancePackage)
                    {
                        recipeManager.DisablePackage((GuidancePackage)component);
                    }
                }
                return VSConstants.S_OK;
            }
            catch (Exception e)
            {
                ErrorHelper.Show(this, e);
                return VSConstants.E_FAIL;
            }
        }

        int IVsSolutionEvents.OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsSolutionEvents.OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsSolutionEvents.OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsSolutionEvents.OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return VSConstants.E_NOTIMPL;
        }


        #endregion

        #region IVsSolutionEvents4 Members

        int IVsSolutionEvents4.OnAfterAsynchOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsSolutionEvents4.OnAfterChangeProjectParent(IVsHierarchy pHierarchy)
        {
            //Handle Drag and Drop event
            if (pHierarchy == null)
            {
                return VSConstants.E_INVALIDARG;
            }
            object hierarchyObject = null;
            pHierarchy.GetProperty(DteHelper.__VSITEMID.ROOT,
                (int)__VSHPROPID.VSHPROPID_ExtObject,
                out hierarchyObject);
            if (hierarchyObject != null)
            {
                if (hierarchyObject is Project)
                {
                    OnProjectRenamed(hierarchyObject as Project);
                }
                else if (hierarchyObject is ProjectItem)
                {
                    OnProjectItemRenamed(hierarchyObject as ProjectItem);
                }
            }
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents4.OnAfterRenameProject(IVsHierarchy pHierarchy)
        {
            // This one is handled by the event in DTE.Events
            return VSConstants.E_NOTIMPL;
        }

        int IVsSolutionEvents4.OnQueryChangeProjectParent(IVsHierarchy pHierarchy, IVsHierarchy pNewParentHier, ref int pfCancel)
        {
            return VSConstants.E_NOTIMPL;
        }

        #endregion

        #region IRecipeManagerService events

        void OnEnablingPackage(object sender, CancelPackageEventArgs e)
        {
            try
            {
                // Only allow execution of the binding recipe if the package is not already attached,
                // or if the version of the package differs from the installed one
                // This will only happen if there was no package-related persisted state in the .suo file in the beginning.
                // See RecipeManagerPackage.cs, OnLoadOptions method, around line 237. 
                PackageState packageState = GetPackageState(e.Package.Configuration.Name);
                bool differentVersion = (packageState.Version.Length > 0) && (packageState.Version != e.Package.Configuration.Version);

                if (differentVersion)
                {
                    MessageBox.Show(string.Format(CultureInfo.CurrentCulture,
                        Properties.Resources.SolutionPackagesContainer_PackageVersionDiffers,
                        e.Package.Configuration.Name),
                        Properties.Resources.SolutionPackagesContainer_PackageVersionDiffersTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                e.ExecuteBindingRecipe = !packageState.BindingRecipeRun || differentVersion;
                if (e.ExecuteBindingRecipe)
                {
                    IAssetReferenceService referenceService = e.Package.GetService<IAssetReferenceService>(true);
                    if (referenceService != null)
                    {
                        foreach (IAssetReference reference in referenceService.GetAll())
                        {
                            referenceService.Remove(reference);
                        }
                    }
                }
                //Add the IArgumentGatheringService service
                e.Package.AddService(typeof(IValueGatheringService), new WizardFramework.WizardGatheringService());
                // Service will be disposed if it fails to load.
                e.Package.AddService(typeof(IOutputWindowService), new OutputWindowService(
                    e.Package.Configuration.Caption, e.Package.SourceSwitch));
			}
			catch (Exception ex)
            {
                ErrorHelper.Show(this, ex);
            }
        }

        void OnEnabledPackage(object sender, PackageEventArgs e)
        {
            try
            {
                VsGuidancePackage vsPackage = new VsGuidancePackage(e.Package);
                Add(vsPackage, e.Package.Configuration.Name);
                PackageState packageState = GetPackageState(e.Package.Configuration.Name);
                packageState.BindingRecipeRun = true;
                packageState.Version = e.Package.Configuration.Version;
            }
            catch (Exception ex)
            {
                ErrorHelper.Show(this, ex);
            }
        }

        void OnDisablingPackage(object sender, CancelPackageEventArgs e)
        {
            try
            {
                VsGuidancePackage vsPackage = (VsGuidancePackage)this.Components[e.Package.Configuration.Name];
                if (vsPackage != null)
                {
                    Remove(vsPackage);
                    IPersistenceService persist = GetService<IPersistenceService>();
                    if (persist != null)
                    {
                        persist.ClearState(e.Package.Configuration.Name);
                    }
                    vsPackage.Dispose();
                    vsPackage = null;
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.Show(this, ex);
            }
        }

        #endregion

        #region IHostService Members

        /// <summary>
        /// Returns the string "VisualStudio" which is the name of the 
        /// host implemented by this class.
        /// </summary>
        public string HostName
        {
            get
            {
                //return InstallerHelper.GetCurrentHostName();
                return "VisualStudio";
            }
        }

        /// <summary>
        /// Retrieves the templates exposed by a VS package.
        /// </summary>
        public IAssetDescription[] GetHostAssets(string packagePath, Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage packageConfiguration)
        {
            IVsTemplatesService templatesService = GetService<IVsTemplatesService>(true);
            return templatesService.GetHostAssets(packagePath);
        }

        /// <summary>
        /// Selects the target of an <see cref="IBoundAssetReference"/>, usually 
        /// done prior to execution.
        /// </summary>
        public bool SelectTarget(object target)
        {
            DTE vs = GetService<DTE>(true);
            Solution solution = target as Solution;
            SolutionFolder folder = target as SolutionFolder;

            if (solution != null)
            {
                return DteHelper.SelectSolution(vs);
            }
            else if (folder != null)
            {
                return DteHelper.SelectItem(vs, folder.Parent) != null;
            }
            else
            {
                return DteHelper.SelectItem(vs, target) != null;
            }
        }

        /// <summary>
        /// Performs selection of a target for the given reference.
        /// </summary>
        public bool SelectTarget(IWin32Window ownerWindow, IUnboundAssetReference forReference)
        {
            DTE dte = GetService<DTE>(true);
            SolutionPickerForm picker = new SolutionPickerForm(dte, forReference);
            DialogResult result = picker.ShowDialog(ownerWindow);
            if (result == DialogResult.OK)
            {
                return SelectTarget(picker.SelectedTarget);
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region DTE Event Handlers

        private void OnReferenceRemoved(object target)
        {
            try
            {
                foreach (object obj in this.Components)
                {
                    if (obj is VsGuidancePackage)
                    {
                        Practices.ComponentModel.ServiceContainer vsPackage = (Practices.ComponentModel.ServiceContainer)obj;
                        IAssetReferenceService referenceService = vsPackage.GetService<IAssetReferenceService>(true);
                        if (referenceService == null)
                        {
                            continue;
                        }
                        IAssetReference[] references = referenceService.Find(typeof(IndexerBoundTarget), target);
                        if (references != null)
                        {
                            foreach (IAssetReference reference in references)
                            {
                                referenceService.Remove(reference);
                                reference.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHelper.Show(this, e);
            }
        }

        private void OnReferenceRenamed(object target)
        {
            try
            {
                foreach (object obj in this.Components)
                {
                    if (obj is VsGuidancePackage)
                    {
                        Practices.ComponentModel.ServiceContainer vsPackage = (Practices.ComponentModel.ServiceContainer)obj;
                        IAssetReferenceService referenceService = vsPackage.GetService<IAssetReferenceService>(true);
                        if (referenceService == null)
                        {
                            continue;
                        }
                        IAssetReference[] references = referenceService.Find(typeof(IndexerBoundTarget), target);
                        if (references != null)
                        {
                            foreach (IAssetReference reference in references)
                            {
                                if (reference is IBoundAssetReference)
                                {
                                    IDictionary state = referenceService.Remove(reference);
                                    if (reference is VsBoundReference)
                                    {
                                        ((VsBoundReference)reference).SetTarget(target);
                                    }
                                    else if (reference is BoundTemplateReference)
                                    {
                                        ((BoundTemplateReference)reference).BoundReference.SetTarget(target);
                                    }
                                    referenceService.Add(reference, state);
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHelper.Show(this, e);
            }
        }

        void RemoveProjectReferences(Project project)
        {
            //Check references in the items of the project first an delete those
            try
            {
                foreach (object obj in this.Components)
                {
                    if (obj is VsGuidancePackage)
                    {
                        Practices.ComponentModel.ServiceContainer vsPackage = (Practices.ComponentModel.ServiceContainer)obj;
                        IAssetReferenceService referenceService = vsPackage.GetService<IAssetReferenceService>(true);
                        if (referenceService == null)
                        {
                            continue;
                        }
                        IAssetReference[] references = referenceService.Find(typeof(IndexerBoundAssetParent), project);
                        if (references != null)
                        {
                            foreach (IAssetReference reference in references)
                            {
                                referenceService.Remove(reference);
                                reference.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHelper.Show(this, e);
            }
        }

        void UpdateProjectReferences(Project project)
        {
            //Check references in the items of the project update those
            try
            {
                foreach (object obj in this.Components)
                {
                    if (obj is VsGuidancePackage)
                    {
                        Practices.ComponentModel.ServiceContainer vsPackage = (Practices.ComponentModel.ServiceContainer)obj;
                        IAssetReferenceService referenceService = vsPackage.GetService<IAssetReferenceService>(true);
                        if (referenceService == null)
                        {
                            continue;
                        }
                        IAssetReference[] references = referenceService.Find(typeof(IndexerBoundAssetParent), project);
                        if (references != null)
                        {
                            foreach (IBoundAssetReference reference in references)
                            {
                                object target = reference.Target;
                                IDictionary state = referenceService.Remove(reference);
                                if (reference is VsBoundReference)
                                {
                                    ((VsBoundReference)reference).SetTarget(target);
                                }
                                else if (reference is BoundTemplateReference)
                                {
                                    ((BoundTemplateReference)reference).BoundReference.SetTarget(target);
                                }
                                referenceService.Add(reference, state);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHelper.Show(this, e);
            }
        }

        internal void RefreshReferences()
        {
            foreach (object obj in this.Components)
            {
                if (obj is VsGuidancePackage)
                {
                    Practices.ComponentModel.ServiceContainer vsPackage = (Practices.ComponentModel.ServiceContainer)obj;
                    IAssetReferenceService referenceService = vsPackage.GetService<IAssetReferenceService>(true);
                    IConfigurationService configservice = vsPackage.GetService<IConfigurationService>(true);
                    if (referenceService == null)
                    {
                        continue;
                    }
                    int nRefs = 0;
                    foreach (IAssetReference reference in referenceService.GetAll())
                    {
                        if (reference is IBoundAssetReference)
                        {
                            if (((IBoundAssetReference)reference).Target != null)
                            {
                                nRefs++;
                                object target = ((IBoundAssetReference)reference).Target;
                                IDictionary state = referenceService.Remove(reference);
                                if (reference is VsBoundReference)
                                {
                                    ((VsBoundReference)reference).SetTarget(target);
                                }
                                else if (reference is BoundTemplateReference)
                                {
                                    ((BoundTemplateReference)reference).BoundReference.SetTarget(target);
                                }
                                referenceService.Add(reference, state);
                            }
                        }
                    }
                    if (nRefs == 0)
                    {
                        IPersistenceService persistence = GetService<IPersistenceService>(true);
                        persistence.SaveReferences(configservice.CurrentPackage.Name, referenceService.GetAll());
                    }
                }
            }
        }

        void OnProjectItemRemoved(ProjectItem projectItem)
        {
            try
            {
                if (projectItem.Object != null && projectItem.Object is Project)
                {
                    RemoveProjectReferences((Project)projectItem.Object);
                }
                // Process the childs elements, e.g: Form1.Designer.cs
                ProjectItems childItems = projectItem.ProjectItems;
                if (childItems != null)
                {
                    foreach (ProjectItem subItem in childItems)
                    {
                        OnProjectItemRemoved(subItem);
                    }
                }
            }
            catch (Exception ex)
            {
                // This is meant to catch "Project unavailable" exception
                this.TraceError(ex.ToString());
            }
            finally
            {
                OnReferenceRemoved(projectItem);
            }
        }

        void OnProjectItemRenamed(ProjectItem projectItem, string oldname)
        {
            OnProjectItemRenamed(projectItem);
        }

        void OnProjectItemRenamed(ProjectItem projectItem)
        {
            OnReferenceRenamed(projectItem);
            if (projectItem.Object != null && projectItem.Object is Project)
            {
                UpdateProjectReferences((Project)projectItem.Object);
            }
            // Process the childs elements, e.g: Form1.Designer.cs
            ProjectItems childItems = projectItem.ProjectItems;
            if (childItems != null)
            {
                foreach (ProjectItem subItem in childItems)
                {
                    OnProjectItemRenamed(subItem);
                }
            }
        }

        void OnProjectRemoved(Project project)
        {
            try
            {
                RemoveProjectReferences(project);
            }
            catch (Exception ex)
            {
                // This is meant to catch "Project unavailable" exception
                this.TraceError(ex.ToString());
            }
            finally
            {
                OnReferenceRemoved(project);
            }
        }

        void OnProjectRenamed(Project project, string oldname)
        {
            OnProjectRenamed(project);
        }

        void OnProjectRenamed(Project project)
        {
            OnReferenceRenamed(project);
            UpdateProjectReferences(project);
        }

        void OnProjectAdded(Project Project)
        {
            //Trace.WriteLine(Project.Name);
        }

        void OnAssetRemoved(object asset)
        {
            if (asset is Project)
            {
                OnProjectRemoved((Project)asset);
            }
            else if (asset is ProjectItem)
            {
                OnProjectItemRemoved((ProjectItem)asset);
            }
        }

        void OnAssetRenamed(object asset)
        {
            if (asset is Project)
            {
                OnProjectRenamed((Project)asset);
            }
            else if (asset is ProjectItem)
            {
                OnProjectItemRenamed((ProjectItem)asset);
            }
        }

        #endregion

        #region IPersistenceService Members

        Dictionary<string, PackageState> packagePersistenceData;
        //ArrayList packagesToPreserve;

        private IDictionary CopyDictionary(IDictionary state)
        {
            // The only real way of getting a full copy of the entire state is to serialize it.
            // We know it's serializable because otherwise it will never make it into the 
            // persisted format.
            MemoryStream mem = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(mem, state);
            mem.Position = 0;
            return (IDictionary)formatter.Deserialize(mem);
        }

        IDictionary IPersistenceService.LoadState(string packageName, IAssetReference reference)
        {
            if (packageName == null)
            {
                throw new ArgumentNullException("packageName");
            }
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }
            PackageState state = GetPackageState(packageName);
            if (!state.States.Contains(reference.Key))
            {
                return null;
            }
            // Return a copy of the dictionary.
            return CopyDictionary((IDictionary)state.States[reference.Key]);
        }

        IDictionary IPersistenceService.RemoveState(string packageName, IAssetReference reference)
        {
            if (packageName == null)
            {
                throw new ArgumentNullException("packageName");
            }
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }
            PackageState state = GetPackageState(packageName);
            if (!state.States.Contains(reference.Key))
            {
                return null;
            }
            IDictionary statedata = (IDictionary)state.States[reference.Key];
            state.States.Remove(reference.Key);
            // Don't copy as we don't care about it anymore.
            return statedata;
        }

        void IPersistenceService.ClearState(string packageName)
        {
            packagePersistenceData.Remove(packageName);
        }

        PackageState GetPackageState(string packageName)
        {
            PackageState packagestate = null;
            if (!packagePersistenceData.ContainsKey(packageName))
            {
                packagestate = new PackageState();
                packagePersistenceData[packageName] = packagestate;
            }
            else
            {
                packagestate = packagePersistenceData[packageName];
            }
            return packagestate;
        }

        void IPersistenceService.SaveState(string packageName, IAssetReference reference, IDictionary state)
        {
#if DEBUG
            Debug.WriteLine(String.Format("IPersistenceService.SaveState for reference {0} of package {1}", reference.Caption, packageName));
#endif

            if (packageName == null)
            {
                throw new ArgumentNullException("packageName");
            }
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }
            PackageState packagestate = GetPackageState(packageName);
            // Copy the dictionary.
            packagestate.States[reference.Key] = CopyDictionary(state);
        }

        IAssetReference[] IPersistenceService.LoadReferences(string packageName)
        {
            if (packageName == null)
            {
                throw new ArgumentNullException("packageName");
            }
            PackageState state = GetPackageState(packageName);
            return state.References;
        }

        void IPersistenceService.RemoveReferences(string packageName)
        {
            if (packageName == null)
            {
                throw new ArgumentNullException("packageName");
            }
            if (!packagePersistenceData.ContainsKey(packageName))
            {
                return;
            }
            GetPackageState(packageName).References = new IAssetReference[0];
        }

        void IPersistenceService.SaveReferences(string packageName, IAssetReference[] references)
        {
#if DEBUG
            Debug.WriteLine(String.Format("IPersistenceService.SaveReferences for package {0}", packageName));
#endif
            if (packageName == null)
            {
                throw new ArgumentNullException("packageName");
            }
            if (references == null)
            {
                throw new ArgumentNullException("references");
            }
            PackageState state = GetPackageState(packageName);
            state.References = references;
        }

        #endregion

        #region PackageState class

        /// <summary>
        /// Holds the state of each package in the in-memory persistence data. 
        /// This is the representation used at runtime, once deserialization 
        /// has been performed.
        /// </summary>
        [Serializable]
        private class PackageState
        {
            /// <summary>
            /// Flag that specifies whether a solution user options file (.suo) contained package-related 
            /// information when the solution was first opened. This will signal that all packages must 
            /// be re-bound (by executing their binding recipes) as part of the SolutionPackagesContainer.OnEnablingPackage 
            /// event handler, and that any bound references from templates should be restored, as part of the 
            /// IVsSolutionEvents.OnAfterOpenSolution event in SolutionPackagesContainer (around line 400).
            /// </summary>
            public bool BindingRecipeRun = false;

            /// <summary>
            /// Version string that specifies the version used for the package when it was attached to the current
            /// solution file. It is used to check differencies in version when the user opens the solution again.
            /// </summary>
            public string Version = string.Empty;
            public IDictionary States = new Hashtable();
            public IAssetReference[] References = new IAssetReference[0];
        }

        #endregion PackageState class

        #region Package State Load/Save

        /// <summary>
        /// Holds serialization information for a package, such as
        /// the references and their initial state. This class is 
        /// nothing more than a bucket of binary data for the package, 
        /// which ultimately will be deserialized into a PackageState class.
        /// </summary>
        [Serializable]
        private class PackageData
        {
            public PackageData(string name, byte[] data)
            {
                this.name = name;
                this.data = data;
            }

            private string name;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            private byte[] data;

            public byte[] Data
            {
                get { return data; }
                set { data = value; }
            }
        }

        internal void LoadPackagesData()
        {
            preservedPackages = new List<GuidancePackagesStatePackage>();
            //packagePersistenceData = new Dictionary<string, PackageState>();

            String solutionStateFilename = GetSolutionStateFilename();
            if (solutionStateFilename == null || !File.Exists(solutionStateFilename))
            {
                return;
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GuidancePackagesState));

            using (FileStream fs = File.OpenRead(solutionStateFilename))
            {
                GuidancePackagesState gps = (GuidancePackagesState)xmlSerializer.Deserialize(fs);
                foreach (GuidancePackagesStatePackage package in gps.Packages)
                {
                    // add the deserialized package
                    DeserializePackage(package);
                }
            }
        }

        //private void DeserializePackage(ITypeResolutionService mainresolution, GuidancePackagesStatePackage package)
        private void DeserializePackage(GuidancePackagesStatePackage package)
        {
            // Retrieve the top-most resolution service that lives in the manager.
            object manager = GetService<IRecipeManagerService>();
            // The manager is the service provider itself that hosts the resolution service.
            ITypeResolutionService mainresolution = (ITypeResolutionService)
                ServiceHelper.GetService((System.IServiceProvider)manager,
                    typeof(ITypeResolutionService), this);

            PackageState packageState = new PackageState();
            PackageHeader packageHeader;
            try
            {
                // deserialize package header
                packageHeader = DeserializePackageHeader(package.Blob);
            }
            catch (Exception e)
            {
                throw new SerializationException(
                    string.Format(CultureInfo.CurrentCulture,
                        Properties.Resources.SolutionPackagesContainer_SerializationError,
                        package.Name),
                    e);
            }

            packageState.BindingRecipeRun = packageHeader.BindingRecipeRun;
            packageState.Version = packageHeader.Version;

            XmlReader creader = null;
            try
            {
                // Retrieve package base path.
                creader = RecipeManager.GetConfigurationReader(this, package.Name);
                string configfile = new CompatibleUri(creader.BaseURI).LocalPath;
                creader.Close();
                using (TypeResolutionResolver resolver = new TypeResolutionResolver(
                    new TypeResolutionService(Path.GetDirectoryName(configfile), mainresolution)))
                {
                    // deserialize the recipes state
                    Hashtable stateTable = new Hashtable();
                    foreach (GuidancePackagesStatePackageState state in package.States)
                    {
                        try
                        {
                            DictionaryEntry stateEntry = (DictionaryEntry)DeserializeFromBinary(state.Blob);
                            stateTable.Add(stateEntry.Key, stateEntry.Value);
                        }
                        catch (Exception e)
                        {
                            throw new SerializationException(
                                string.Format(CultureInfo.CurrentCulture,
                                    Properties.Resources.SolutionPackagesContainer_SerializationError,
                                    package.Name),
                                e);
                        }
                    }
                    packageState.States = stateTable;

                    // deserialize the references
                    List<IAssetReference> referenceTable = new List<IAssetReference>();
                    foreach (GuidancePackagesStatePackageReference reference in package.References)
                    {
                        try
                        {
                            referenceTable.Add((IAssetReference)DeserializeFromBinary(reference.Blob));
                        }
                        catch (Exception e)
                        {
                            throw new SerializationException(
                                string.Format(CultureInfo.CurrentCulture,
                                    Properties.Resources.SolutionPackagesContainer_SerializationError,
                                    package.Name),
                                e);
                        }
                    }
                    packageState.References = referenceTable.ToArray();

                    packagePersistenceData[package.Name] = packageState;
                }
            }
            // if the package is not installed, or if there is an error loading the xml
            // ask if should we remove the data from the state file
            catch (Exception ex)
            {
                DialogResult result = ErrorHelper.Show(this, ex, string.Format(CultureInfo.CurrentUICulture,
                            Properties.Resources.SolutionPackagesContainer_PackageRemovePersistence,
                            ex.Message), MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    // the user doesn't want to skip the package
                    // keep track of it so we can serialize it back at solution closing time
                    preservedPackages.Add(package);
                }
                if (creader != null)
                {
                    creader.Close();
                }
            }
        }

        internal void SavePackagesData()
        {
            String solutionStateFilename = GetSolutionStateFilename();
            if (solutionStateFilename == null)
            {
                return;
            }

            object hostservice = GetService(typeof(IHostService));
            Debug.Assert(hostservice is SolutionPackagesContainer, "Wrong implementation of IHostService found");
            RefreshReferences();

            ArrayList data = new ArrayList(packagePersistenceData.Count +
                preservedPackages.Count);

            List<GuidancePackagesStatePackage> packages = new List<GuidancePackagesStatePackage>();

            foreach (KeyValuePair<string, PackageState> entry in packagePersistenceData)
            {
                GuidancePackagesStatePackage pkg = SerializePackage(entry.Key, entry.Value);
                packages.Add(pkg);
            }

            // add packages that failed to load but the user didn't want to remove from the solution
            packages.AddRange(preservedPackages);

            GuidancePackagesState gps = new GuidancePackagesState();
            gps.Packages = packages.ToArray();

            // don't create a file if there are no packages to persist
            if (gps.Packages.Length > 0)
            {
                XmlSerializer stateFile = new XmlSerializer(typeof(GuidancePackagesState));
                XmlWriterSettings writerSettings = new XmlWriterSettings();
                writerSettings.Indent = true;

                // make sure we checkout the file (if its under SCC) or reset its read-only bit in order to be able to write to it
                EnsureWritable(solutionStateFilename);

                using (XmlWriter gpStateFile = XmlWriter.Create(solutionStateFilename, writerSettings))
                {
                    stateFile.Serialize(gpStateFile, gps);
                    gpStateFile.Close();
                }
            }
        }

        #endregion

        #region SCC helper
        void EnsureWritable(String filePath)
        {
            DTE vs = (DTE)GetService(typeof(DTE));
            if (File.Exists(filePath))
            {
                if (vs.SourceControl.IsItemUnderSCC(filePath) &&
                    !vs.SourceControl.IsItemCheckedOut(filePath))
                {
                    bool checkout = vs.SourceControl.CheckOutItem(filePath);
                    if (!checkout)
                    {
                        throw new CheckoutException(string.Format(Resources.Checkout_Failed, filePath));
                    }
                }
                else
                {
                    // perform an extra check if the file is read only.
                    if (IsReadOnly(filePath))
                    {
                        ResetReadOnly(filePath);

                    }
                }
            }
        }

        bool IsReadOnly(string filePath)
        {
            return (File.GetAttributes(filePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
        }

        void ResetReadOnly(string filePath)
        {
            File.SetAttributes(filePath, File.GetAttributes(filePath) ^ FileAttributes.ReadOnly);
        }
        #endregion

        #region New Persistence Storage support

        private GuidancePackagesStatePackage SerializePackage(String name, PackageState state)
        {
            GuidancePackagesStatePackage package = new GuidancePackagesStatePackage();
            package.Name = name;
            package.Version = state.Version;
            package.BindingRecipeRun = state.BindingRecipeRun;

            // serialize package states
            List<GuidancePackagesStatePackageState> states = new List<GuidancePackagesStatePackageState>();
            foreach (DictionaryEntry stateEntry in state.States)
            {
                GuidancePackagesStatePackageState gpState = new GuidancePackagesStatePackageState();
                gpState.Key = stateEntry.Key.ToString();
                gpState.Blob = SerializeToBinary(stateEntry);
                states.Add(gpState);
            }
            package.States = states.ToArray();

            // serialize package references
            List<GuidancePackagesStatePackageReference> references = new List<GuidancePackagesStatePackageReference>();
            foreach (AssetReference referenceEntry in state.References)
            {
                GuidancePackagesStatePackageReference reference = new GuidancePackagesStatePackageReference();
                reference.AssetName = referenceEntry.AssetName;
                reference.AppliesTo = referenceEntry.AppliesTo;
                reference.Blob = SerializeToBinary(referenceEntry);
                references.Add(reference);
            }
            package.References = references.ToArray();

            // Serialize package header
            package.Blob = SerializePackageHeader(new PackageHeader(package.Name, package.Version, package.BindingRecipeRun));

            return package;
        }

        private PackageHeader DeserializePackageHeader(byte[] header)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return (PackageHeader)binaryFormatter.Deserialize(new MemoryStream(header));
        }

        private byte[] SerializePackageHeader(PackageHeader packageHeader)
        {
            return SerializeToBinary(packageHeader);
        }

        private byte[] SerializeToBinary(object o)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(ms, o);
                return ms.GetBuffer();
            }
        }

        private object DeserializeFromBinary(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return binaryFormatter.Deserialize(ms);
            }
        }

        private String GetSolutionStateFilename()
        {
            DTE dte = GetService<DTE>();
            if (dte.Solution != null)
            {
                String solutionFullname = null;

                try
                {
                    solutionFullname = (String)dte.Solution.Properties.Item("Path").Value;
                }
                catch
                {

                }
                if (!String.IsNullOrEmpty(solutionFullname))
                {
                    String folder = Path.GetDirectoryName(solutionFullname);
                    String filenameWithoutExtension = Path.GetFileNameWithoutExtension(solutionFullname);
                    return Path.Combine(folder, filenameWithoutExtension + GaxExternalStateFileExtension);
                }
            }
            return null;
        }

        #endregion

        #region IOleComponent Members

        int IOleComponent.FContinueMessageLoop(uint uReason, IntPtr pvLoopData, MSG[] pMsgPeeked)
        {
            return VSConstantsEx.FALSE;
        }

        IEnumerator<KeyValuePair<string, PackageState>> packageEnumerator = null;
        IEnumerator referenceEnumerator = null;

        private bool FixReference(string packageName, IBoundAssetReference reference, object actualTarget)
        {
            DTE dte = GetService<DTE>();
            try
            {
                dte.StatusBar.Text = String.Format(CultureInfo.CurrentCulture,
                    Properties.Resources.SolutionPackagesContainer_UpdatingReference,
                    reference.SubPath,
                    packageName);
                string newSerializationData = string.Empty;
                try
                {
                    newSerializationData = reference.Strategy.GetSerializationData(actualTarget);
                }
                catch
                {
                    // This will catch "Project unavailible exception
                    newSerializationData = string.Empty;
                }
                if (string.IsNullOrEmpty(newSerializationData))
                {
                    OnAssetRemoved(actualTarget);
                }
                else
                {
                    OnAssetRenamed(actualTarget);
                }
                return true;
            }
            finally
            {
                dte.StatusBar.Clear();
                reference = null;
                referenceEnumerator = null;
                packageEnumerator = null;
            }
        }

        private bool CheckReference(string packageName, IBoundAssetReference reference)
        {
            object actualTarget = reference.Target;
            if (actualTarget == null)
            {
                return false;
            }
            string serializedData = reference.SubPath;
            if (serializedData == null)
            {
                return FixReference(packageName, reference, actualTarget);
            }
            object testedTarget = reference.Strategy.LocateTarget(this, serializedData);
            if (testedTarget != actualTarget)
            {
                return FixReference(packageName, reference, actualTarget);
            }
            return false;
        }

        private int ReferenceLoop(string packageName)
        {
            if (referenceEnumerator == null)
            {
                return VSConstantsEx.FALSE;
            }
            while (referenceEnumerator.MoveNext())
            {
                if (referenceEnumerator.Current is IBoundAssetReference)
                {
                    IBoundAssetReference reference = referenceEnumerator.Current as IBoundAssetReference;
                    if (reference != null && CheckReference(packageName, reference))
                    {
                        referenceEnumerator = null;
                        packageEnumerator = null;
                        return VSConstantsEx.TRUE;
                    }
                }
                if (oleComponentManager.FContinueIdle() == VSConstantsEx.FALSE)
                {
                    return VSConstantsEx.TRUE;
                }
            }
            referenceEnumerator = null;
            return VSConstantsEx.FALSE;
        }

        int IOleComponent.FDoIdle(uint _grfidlef)
        {
            _OLEIDLEF grfidlef = (_OLEIDLEF)_grfidlef;
            if (referenceEnumerator != null && packageEnumerator != null)
            {
                if (ReferenceLoop(packageEnumerator.Current.Key) == VSConstantsEx.TRUE)
                {
                    return VSConstantsEx.TRUE;
                }
            }
            if (packageEnumerator == null)
            {
                packageEnumerator = this.packagePersistenceData.GetEnumerator();
            }
            while (packageEnumerator.MoveNext())
            {
                referenceEnumerator =
                    packageEnumerator.Current.Value.References.GetEnumerator();
                if (ReferenceLoop(packageEnumerator.Current.Key) == VSConstantsEx.TRUE || oleComponentManager.FContinueIdle() == VSConstantsEx.FALSE)
                {
                    return VSConstantsEx.TRUE;
                }
            }
            referenceEnumerator = null;
            packageEnumerator = null;
            return VSConstantsEx.FALSE;
        }

        int IOleComponent.FPreTranslateMessage(MSG[] pMsg)
        {
            return VSConstantsEx.FALSE;
        }

        int IOleComponent.FQueryTerminate(int fPromptUser)
        {
            return VSConstantsEx.TRUE;
        }

        int IOleComponent.FReserved1(uint dwReserved, uint message, IntPtr wParam, IntPtr lParam)
        {
            return VSConstantsEx.TRUE;
        }

        IntPtr IOleComponent.HwndGetWindow(uint dwWhich, uint dwReserved)
        {
            return IntPtr.Zero;
        }

        void IOleComponent.OnActivationChange(IOleComponent pic, int fSameComponent, OLECRINFO[] pcrinfo, int fHostIsActivating, OLECHOSTINFO[] pchostinfo, uint dwReserved)
        {
        }

        void IOleComponent.OnAppActivate(int fActive, uint dwOtherThreadID)
        {
        }

        void IOleComponent.OnEnterState(uint uStateID, int fEnter)
        {
        }

        void IOleComponent.OnLoseActivation()
        {
        }

        void IOleComponent.Terminate()
        {
        }

        #endregion

        #region IVsSolutionLoadEvents Members

        int IVsSolutionLoadEvents.OnAfterBackgroundSolutionLoadComplete()
        {
            // enable and show the Guidance Package Manager menu
            guidancePackageManagerMenuCmd.Enabled = true;
            guidancePackageManagerMenuCmd.Visible = true;

            // enable and show the Guidance Navigator Window menu
            guidanceNavigatorMenuCmd.Enabled = true;
            guidanceNavigatorMenuCmd.Visible = true;

            IRecipeManagerService recipeManager = GetService<IRecipeManagerService>(true);

            // load list and state for all binded packages to the current solution
            LoadPackagesData();

            // iterate over all packages binded to the current solution
            foreach (string packageName in packagePersistenceData.Keys)
            {
                try
                {
                    // try to get to the package
                    if (recipeManager.GetPackage(packageName) == null)
                    {
                        // now try to enable the package
                        recipeManager.EnablePackage(packageName);
                    }
                }
                catch (Exception ex)
                {
                    // for some reason we couldn't get to the package OR the loading failed
                    // ask the user if he wants to permanently remove the package binding from the solution
                    DialogResult result = ErrorHelper.Show(this, ex, String.Format(CultureInfo.CurrentUICulture,
                                Properties.Resources.SolutionPackagesContainer_CannotLoadPackage,
                                packageName), MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            // try to get to the package
                            if (recipeManager.GetPackage(packageName) != null)
                            {
                                // we got it, now try to remove the binding from the solution.
                                recipeManager.DisablePackage(packageName);
                            }
                        }
                        finally
                        {
                            // we need to make sure we always remove the binding for the current run
                            IPersistenceService persist = GetService<IPersistenceService>();
                            if (persist != null)
                            {
                                persist.RemoveReferences(packageName);
                            }
                            packagePersistenceData.Remove(packageName);
                        }
                    }
                }
            }
            if (UnfoldTemplate.UnfoldingTemplates.Count == 0)
            {
                IReferenceRestoreService restoreService = GetService<IReferenceRestoreService>();
                bool useBuiltIn = restoreService == null;
                try
                {
                    if (useBuiltIn)
                    {
                        restoreService = new ReferenceRestoreService();
                        Add(restoreService);
                    }
                    restoreService.PerformValidation();
                }
                finally
                {
                    if (useBuiltIn && restoreService != null)
                    {
                        Remove(restoreService);
                    }
                }
            }

            return VSConstants.S_OK;
        }

        int IVsSolutionLoadEvents.OnAfterLoadProjectBatch(bool fIsBackgroundIdleBatch)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsSolutionLoadEvents.OnBeforeBackgroundSolutionLoadBegins()
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsSolutionLoadEvents.OnBeforeLoadProjectBatch(bool fIsBackgroundIdleBatch)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsSolutionLoadEvents.OnBeforeOpenSolution(string pszSolutionFilename)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsSolutionLoadEvents.OnQueryBackgroundLoadProjectBatch(out bool pfShouldDelayLoadToNextIdle)
        {
            pfShouldDelayLoadToNextIdle = false;
            return VSConstants.E_NOTIMPL;
        }

        #endregion
    }
}