using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Services;
using RecipeFrameworkConfiguration = Microsoft.Practices.RecipeFramework.Configuration;
using Microsoft.Practices.RecipeFramework.Configuration;
using System.IO;
using Microsoft.Practices.RecipeFramework.VisualStudio.Properties;
using System.Diagnostics;
using Microsoft.Practices.RecipeFramework.VisualStudio.ToolWindow;
using System.Collections;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using Microsoft.Practices.Common;
using System.Windows.Forms.Design;
using Microsoft.Practices.ComponentModel;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.ToolWindow
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GuidanceNavigatorControl : UserControl
    {
        #region Fields
        IServiceProvider serviceProvider;
        Dictionary<String, GuidancePackage> guidancePackagesList;
        GuidanceNavigatorManager guidanceNavigatorManager;
        GuidancePackage selectedGuidancePackage;

        ExtendedTreeView historyRecipesTreeView;
        ExtendedTreeView availableRecipesTreeView;

        Panel[] panelGroup;

        private const int ImageRecipeIndex = 0;
        private const int ImageDocumentationIndex = 1;
        private const int ImageExecuteIndex = 2;
        private const int ImageTemplateIndex = 3;

        private const string ExecuteRecipeProtocolHandler = "recipe://";
        private Dictionary<string, IAssetReference> nextStepReferences;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GuidanceNavigatorControl"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public GuidanceNavigatorControl(IServiceProvider serviceProvider)
        {
            guidancePackagesList = new Dictionary<string, GuidancePackage>();
            nextStepReferences = new Dictionary<string, IAssetReference>();
            this.serviceProvider = serviceProvider;
            
            InitializeComponent();

            historyRecipesTreeView = new ExtendedTreeView();
            historyRecipesTreeView.ShowNodeToolTips = true;
            availableRecipesTreeView = new ExtendedTreeView();
            availableRecipesTreeView.ShowNodeToolTips = true;
        }

        /// <summary>
        /// Cleanup code for GuidanceNavigatorControl
        /// </summary>
        ~GuidanceNavigatorControl()
        {
            UnsubscribeToGaxEvents();
            UnsubscribeToSolutionExplorerChanges();
        }

        #region OnResize handler
        /// <summary>
        /// Raises the resize event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (toolStripGuidancePackages.Items.Count > 0)
            {
                // Adjust the width of guidance packages dropdownlist.
                this.cbGuidancePackages.Size = new Size(this.toolStripGuidancePackages.Width - (this.toolStripGuidancePackages.Items[0].Bounds.X + this.toolStripGuidancePackages.Items[0].Bounds.Width + this.toolStripGuidancePackages.Padding.Right) - 35, this.cbGuidancePackages.Height);
            }
        }
        #endregion

        /// <summary>
        /// Handles the Load event of the GuidanceNavigatorControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void GuidanceNavigatorControl_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            panelOverview.Dock = DockStyle.Fill;
            panelOverview.Visible = true;

            historyRecipesTreeView.ImageList = this.imageList;
            historyRecipesTreeView.Dock = DockStyle.Fill;
            historyRecipesTreeView.BorderStyle = BorderStyle.None;
            historyRecipesTreeView.NodeClose += new TreeNodeMouseClickEventHandler(completedRecipesTreeView_NodeClose);
            historyRecipesTreeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(historyRecipesTreeView_NodeMouseClick);

            panelCompletedRecipes.Controls.Add(historyRecipesTreeView);
            panelCompletedRecipes.Dock = DockStyle.Fill;
            panelCompletedRecipes.Visible = false;

            availableRecipesTreeView.Dock = DockStyle.Fill;
            availableRecipesTreeView.BorderStyle = BorderStyle.None;
            availableRecipesTreeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(availableRecipesTreeView_NodeMouseClick);
            availableRecipesTreeView.ImageList = this.imageList;

            panelAvailableRecipes.Controls.Add(availableRecipesTreeView);
            panelAvailableRecipes.Dock = DockStyle.Fill;
            panelAvailableRecipes.Visible = false;

            panelGroup = new Panel[] { panelOverview, panelCompletedRecipes, panelAvailableRecipes };

            // Disable Recipe ToolStrip
            this.toolStripPackage.Enabled = false;
            this.ResumeLayout();
        }

        #region Available Recipes & History treeview's event handlers
        /// <summary>
        /// Handles the NodeMouseClick event of the historyRecipesTreeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
        private void historyRecipesTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null)
                return;

            if (e.Node.Tag is Link)
            {
                Link documentationLink = e.Node.Tag as Link;

                if (documentationLink.KindSpecified)
                {
                    if (documentationLink.Kind == DocumentationKind.Documentation)
                    {
                        ShowHelp(documentationLink.Url);
                    }
                    else if (documentationLink.Kind == DocumentationKind.NextStep)
                    {
                        if (documentationLink.Url.StartsWith(ExecuteRecipeProtocolHandler))
                        {
                            string referenceName = documentationLink.Url.Replace(ExecuteRecipeProtocolHandler, string.Empty);
                            IAssetReference assetReference = GetAssetReferenceByName(referenceName);
                            
                            if (assetReference != null)
                            {
                                ExecuteReference(assetReference);
                            }
                        }
                        else
                        {
                            ShowHelp(documentationLink.Url);
                        }
                    }
                }
            }
            else if (e.Node.Tag is IAssetReference)
            {
                ExecuteReference(e.Node.Tag as IAssetReference);

                // If this is a non recurrent recipe, the 'run' node will be removed.
                Recipe recipe = e.Node.Parent.Tag as Recipe;
                if (recipe != null)
                {
                    if (!recipe.Recurrent)
                    {
                        e.Node.Parent.Remove();
                    }
                }
            }

        }

        private IAssetReference GetAssetReferenceByName(string referenceName)
        {
            //Dictionary<string, IAssetReference> references = this.guidanceNavigatorManager.GetAvailableRecipes(selectedGuidancePackage);

            Dictionary<string, IAssetReference> references = GetAvailableReferencesCache();

            IAssetReference assetReference = null;

            foreach (IAssetReference reference in references.Values)
            {
                if (reference.AssetName == referenceName)
                {
                    assetReference = reference;
                    break;
                }
            }
            return assetReference;
        }

        /// <summary>
        /// Handles the NodeMouseClick event of the availableRecipesTreeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
        private void availableRecipesTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null)
                return;

            IAssetReference executeReference = e.Node.Tag as IAssetReference;

            if (e.Node.Tag is Link)
            {
                Link documentationLink = e.Node.Tag as Link;

                if (documentationLink.KindSpecified && documentationLink.Kind == DocumentationKind.Documentation)
                {
                    ShowHelp(documentationLink.Url);
                }
            }
            else if (executeReference != null)
            {
                ExecuteReference(executeReference);
            }
            else if (e.Node.Tag is Recipe)
            {
                // nothing todo... 
            }
        }

        /// <summary>
        /// Executes the reference.
        /// </summary>
        /// <param name="reference">The IAssetReference that will be execute.</param>
        private void ExecuteReference(IAssetReference reference)
        {
            if (reference == null)
                return;

            try
            {
                IHostService host = (IHostService)ServiceHelper.GetService(this, typeof(IHostService));

                bool execute = false;
                if (reference is IBoundAssetReference)
                {
                    IBoundAssetReference boundReferece = reference as IBoundAssetReference;
                    if (boundReferece.Target == null)
                    {
                        // this is a stale bound reference: tell the user and refresh the list of available guidance
                        MessageBox.Show(Properties.Resources.Navigator_StaleReference, Properties.Resources.Navigator_StaleReferenceTitle);
                        // invalidate the cache to remove stale references
                        this.UpdateAvailableGuidance(true);
                        return;
                    }
                    execute = host.SelectTarget(((IBoundAssetReference)reference).Target);
                }
                else if (reference is IUnboundAssetReference)
                {
                    execute = host.SelectTarget(this, (IUnboundAssetReference)reference);
                }

                if (execute)
                {
                    selectedGuidancePackage.TurnOnOutput();
                    reference.Execute();
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.Show(this.Site, ex);
            }
            finally
            {
                if (selectedGuidancePackage != null)
                {
                    selectedGuidancePackage.TurnOffOutput();
                }
            }
        }

        /// <summary>
        /// Handles the NodeClose event of the completedRecipesTreeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
        private void completedRecipesTreeView_NodeClose(object sender, TreeNodeMouseClickEventArgs e)
        {
            GuidanceNavigatorManager.RecipeExecutionHistory history = e.Node.Tag as GuidanceNavigatorManager.RecipeExecutionHistory;
            if (history != null)
            {
                guidanceNavigatorManager.RemoveLogRecipeExecution(selectedGuidancePackage, history);
                e.Node.Remove();
            }
        }
        #endregion

        /// <summary>
        /// Initializes the data of the control. It's called from the ToolWindow Pane.
        /// </summary>
        internal void InitializeData()
        {
            SubscribeToGaxEvents();
            SubscribeToSolutionExplorerChanges();

            UpdatePackagesInformation();
        }

        private void SubscribeToGaxEvents()
        {
            guidanceNavigatorManager = RecipeManagerPackage.Singleton.GuidanceNavigatorManager;
            guidanceNavigatorManager.BindedPackagesChange += new EventHandler(guidanceNavigatorManager_BindedPackagesChange);
            guidanceNavigatorManager.RecipeExecuted += new RecipeEventHandler(guidanceNavigatorManager_RecipeExecuted);
        }

        private void UnsubscribeToGaxEvents()
        {
            guidanceNavigatorManager = RecipeManagerPackage.Singleton.GuidanceNavigatorManager;
            guidanceNavigatorManager.BindedPackagesChange -= new EventHandler(guidanceNavigatorManager_BindedPackagesChange);
            guidanceNavigatorManager.RecipeExecuted -= new RecipeEventHandler(guidanceNavigatorManager_RecipeExecuted);
        }

        #region Recipe Manager Event Handlers
        void guidanceNavigatorManager_SolutionExplorerSelectionChanged(object sender, EventArgs e)
        {
            UpdateAvailableGuidance();
        }

        void guidanceNavigatorManager_RecipeExecuted(object sender, RecipeEventArgs e)
        {
            GuidancePackage package = (GuidancePackage) sender;

            if (package != selectedGuidancePackage)
            {
                SelectPackage(package);
            }
            GuidanceNavigatorManager.RecipeExecutionHistory[] history = guidanceNavigatorManager.GetExecutedRecipes(package).ToArray();
            GuidanceNavigatorManager.RecipeExecutionHistory historyItem = history.GetValue(history.Length - 1) as GuidanceNavigatorManager.RecipeExecutionHistory;

                AddHistoryRecipeEntry(historyItem);
                ShowHistoryRecipes();
        }

        void guidanceNavigatorManager_BindedPackagesChange(object sender, EventArgs e)
        {
            //UpdatePackagesInformation();
            UpdateAvailablePackages();
        }
        #endregion

        /// <summary>
        /// Selects the package.
        /// </summary>
        /// <param name="guidancePackage">The guidance package.</param>
        private void SelectPackage(GuidancePackage guidancePackage)
        {
            int index = 0;
            foreach (object item in this.cbGuidancePackages.Items)
            {
                GuidancePackageEntry entry = item as GuidancePackageEntry;
                if (entry != null)
                {
                    if (entry.GuidancePackage == guidancePackage)
                    {
                        cbGuidancePackages.SelectedIndex = index;
                        break;
                    }
                }
                index++;
            }
            selectedGuidancePackage = guidancePackage;
        }

        /// <summary>
        /// Updates the package information.
        /// </summary>
        private void UpdatePackagesInformation()
        {
            UpdateAvailablePackages();
            UpdateRecipesHistory();

            if (this.cbGuidancePackages.Items.Count > 1)
            {
                if ((GuidancePackageEntry)cbGuidancePackages.SelectedItem != null)
                {
                    selectedGuidancePackage = ((GuidancePackageEntry)cbGuidancePackages.SelectedItem).GuidancePackage;
                }
                UpdatePackage();
                this.toolStripPackage.Enabled = true;
            }
            else
            {
                selectedGuidancePackage = null;
                this.availableRecipesTreeView.Nodes.Clear();
                this.historyRecipesTreeView.Nodes.Clear();
                SelectToolStripElement(null, this.toolStripPackage);
                this.toolStripPackage.Enabled = false;
                UpdatePackageOverview();
            }
        }

        /// <summary>
        /// refresh the DropDownList showing currently enabled packages
        /// </summary>
        private void UpdateAvailablePackages()
        {
            cbGuidancePackages.Items.Clear();

            foreach (GuidancePackage guidancePackage in guidanceNavigatorManager.EnabledGuidancePackages)
            {
                cbGuidancePackages.Items.Add(new GuidancePackageEntry(guidancePackage));
            }

            if (cbGuidancePackages.Items.Count > 0)
            {
                cbGuidancePackages.SelectedIndex = 0;
                selectedGuidancePackage = ((GuidancePackageEntry)cbGuidancePackages.Items[0]).GuidancePackage;
            }
            else if (cbGuidancePackages.Items.Count == 0)
            {
                selectedGuidancePackage = null;
            }

            // the list should always include an ending "Guidance Package Manager..." item
            // which should fire up the Guidance Package Manager dialog box
            cbGuidancePackages.Items.Add(Properties.Resources.GuidanceNavigatorWindow_GuidancePackageManagerOptionText);
        }

        #region Packages DropDown Event Handlers
        private void cbGuidancePackages_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectedPackageName = cbGuidancePackages.SelectedItem.ToString();
            // TODO: if there is a guidance package named after this option things won't work as expected
            if (selectedPackageName == Properties.Resources.GuidanceNavigatorWindow_GuidancePackageManagerOptionText)
            {
                // TODO: ensure that the command is available.
                object objOut = null;
                object objIn = null;
                DTE dte = (DTE)serviceProvider.GetService(typeof(DTE));
                dte.Commands.Raise("{"+SolutionPackagesContainer.CTC.guidRecipeManagerCmdSet.ToString()+"}", SolutionPackagesContainer.CTC.icmdRecipeManagerCommand, ref objIn, ref objOut);
                
                UpdatePackagesInformation();
            }
            else
            {
                if (this.selectedGuidancePackage == ((GuidancePackageEntry)cbGuidancePackages.SelectedItem).GuidancePackage)
                {
                    return;
                }
                this.selectedGuidancePackage = ((GuidancePackageEntry)cbGuidancePackages.SelectedItem).GuidancePackage;
                // currently selected package has changed, we need to clear the current cache and get a fresh list of available recipes for the new package
                ClearAvailableReferencesCache();
                UpdatePackage();
            }
        }
        #endregion

        Dictionary<string, IAssetReference> currentPackageAvailableReferencesCache = null;

        private void ClearAvailableReferencesCache()
        {
            currentPackageAvailableReferencesCache = null;
        }

        private Dictionary<string, IAssetReference> GetAvailableReferencesCache()
        {
            if (currentPackageAvailableReferencesCache == null)
            {
                currentPackageAvailableReferencesCache = this.guidanceNavigatorManager.GetAvailableRecipes(this.selectedGuidancePackage);
            }
            return currentPackageAvailableReferencesCache;
        }

        /// <summary>
        /// Update all information for the currently selected guidance package
        /// </summary>
        private void UpdatePackage()
        {
            if (selectedGuidancePackage == null)
                return;

            // get the list from the cache (if avail) so we avoid potentially long-running probings executed multiple times in a short amount of time
            Dictionary<string, IAssetReference> references = GetAvailableReferencesCache();

            UpdateAvailableGuidance();
            UpdateRecipesHistory();
            UpdatePackageOverview();

            // select the overview tool strip button and overview panel
            SelectToolStripElement(toolStripPackageOverview, this.toolStripPackage);
            SetVisiblePanel(panelOverview);
        }

        /// <summary>
        /// Updates the package overview.
        /// </summary>
        protected void UpdatePackageOverview()
        {
            string blankOverview = "about:blank";

            if (selectedGuidancePackage == null)
            {
                wbPackageOverview.Navigate(blankOverview);
                return;
            }

            if (selectedGuidancePackage.Configuration != null && selectedGuidancePackage.Configuration.Overview != null)
            {
                if (string.IsNullOrEmpty(selectedGuidancePackage.Configuration.Overview.Url))
                {
                    wbPackageOverview.DocumentText = selectedGuidancePackage.Configuration.Overview.ToString();
                }
                else
                {
                    string overviewLocation = ParseUri(selectedGuidancePackage.Configuration.Overview.Url);
                    wbPackageOverview.Navigate(overviewLocation);
                }
            }
            else
            {
                wbPackageOverview.Navigate(blankOverview);
            }
        }


        private void UpdateAvailableGuidance(bool invalidateCache)
        {
            if (invalidateCache == true)
            {
                ClearAvailableReferencesCache();
            }
            UpdateAvailableGuidance();
        }

        /// <summary>
        /// Updates the available guidance.
        /// </summary>
        private void UpdateAvailableGuidance()
        {
            if (selectedGuidancePackage == null)
                return;

            this.SuspendLayout();
            availableRecipesTreeView.Nodes.Clear();
            Dictionary<String, IAssetReference> references = GetAvailableReferencesCache();

            foreach (IAssetReference assetReference in references.Values)
            {
                ExtendedTreeNode recipeTreeNode = null;

                recipeTreeNode = new ExtendedTreeNode();
                Recipe recipe = guidanceNavigatorManager.GetRecipeConfiguration(selectedGuidancePackage, assetReference.AssetName);

                string caption = assetReference.Caption;
                int imageIndex = ImageRecipeIndex;

                if (assetReference is BoundTemplateReference || assetReference is UnboundTemplateReference)
                {
                    caption = string.Format(Properties.Resources.Navigator_TemplateCaptionMask, assetReference.Caption);
                    imageIndex = ImageTemplateIndex;
                    if (!String.IsNullOrEmpty(assetReference.Description))
                    {
                        recipeTreeNode.ToolTipText = assetReference.Description;
                    }
                }

                recipeTreeNode.Text = caption;
                recipeTreeNode.Tag = recipe;
                recipeTreeNode.ShowCloseButton = false;
                recipeTreeNode.ImageIndex = imageIndex;
                recipeTreeNode.SelectedImageIndex = imageIndex;
                recipeTreeNode.StateImageIndex = imageIndex;

                // Bound template references doesn't have an associated recipe
                if (recipe != null)
                {
                    if (!String.IsNullOrEmpty(recipe.Description))
                    {
                        recipeTreeNode.ToolTipText = recipe.Description;
                    }

                    // Recipe Documentation Nodes
                    if (recipe.DocumentationLinks != null)
                    {
                        foreach (Link documentationLink in recipe.DocumentationLinks)
                        {
                            if (documentationLink.Kind == DocumentationKind.Documentation)
                            {
                                AddDocumentationNode(recipeTreeNode, documentationLink);
                            }
                        }
                    }
                }
                AddExecuteRecipeNode(assetReference, recipeTreeNode);
                availableRecipesTreeView.Nodes.Add(recipeTreeNode);
            }

            this.ResumeLayout();
        }

        #region Update History Methods
        /// <summary>
        /// Updates the execution recipes history view.
        /// </summary>
        protected void UpdateRecipesHistory()
        {
            if (selectedGuidancePackage == null)
                return;

            this.historyRecipesTreeView.Nodes.Clear();

            List<GuidanceNavigatorManager.RecipeExecutionHistory> executedRecipes = guidanceNavigatorManager.GetExecutedRecipes(selectedGuidancePackage);

            foreach (GuidanceNavigatorManager.RecipeExecutionHistory recipeExecutionHistory in executedRecipes)
            {
                AddHistoryRecipeEntry(recipeExecutionHistory);
            }
        }

        /// <summary>
        /// Adds a history recipe execution entry.
        /// </summary>
        private void AddHistoryRecipeEntry(GuidanceNavigatorManager.RecipeExecutionHistory recipeExecutionHistory)
        {
            Recipe recipe = guidanceNavigatorManager.GetRecipeConfiguration(selectedGuidancePackage, recipeExecutionHistory.RecipeName);

            if (recipe == null)
                return;

            ExtendedTreeNode recipeTreeNode = new ExtendedTreeNode();
            recipeTreeNode.Text = recipe.Caption;
            recipeTreeNode.ToolTipText = recipe.Description;
            recipeTreeNode.Tag = recipeExecutionHistory;
            recipeTreeNode.ShowCloseButton = true;

            ExtendedTreeNode suggestedRecipesTreeNode = new ExtendedTreeNode();
            suggestedRecipesTreeNode.StateImageIndex = ImageDocumentationIndex;
            suggestedRecipesTreeNode.ImageIndex = ImageDocumentationIndex;
            suggestedRecipesTreeNode.SelectedImageIndex = ImageDocumentationIndex;
            suggestedRecipesTreeNode.ShowAsLink = false;
            suggestedRecipesTreeNode.Text = Properties.Resources.Navigator_NextStepsTitle;

            // Recipe Documentation Nodes
            if (recipe.DocumentationLinks != null)
            {
                foreach (Link documentationLink in recipe.DocumentationLinks)
                {
                    if (documentationLink.Kind == DocumentationKind.Documentation)
                    {
                        AddDocumentationNode(recipeTreeNode, documentationLink);
                    }
                    else if (documentationLink.Kind == DocumentationKind.NextStep)
                    {
                        AddDocumentationNode(suggestedRecipesTreeNode, documentationLink);
                    }
                }
            }

            AddExecuteRecipeHistoryNode(recipe, recipeTreeNode);

            if (suggestedRecipesTreeNode.Nodes.Count > 0)
            {
                recipeTreeNode.Nodes.Add(suggestedRecipesTreeNode);
            }

            // collapse all history entries and show expand just the latest entry.
            this.historyRecipesTreeView.CollapseAll();
            recipeTreeNode.ExpandAll();
            this.historyRecipesTreeView.Nodes.Add(recipeTreeNode);
            historyRecipesTreeView.SelectedNode = recipeTreeNode;
        }
        #endregion

        #region Add Nodes Private Helpers
        
        private void AddExecuteRecipeHistoryNode(Recipe recipe, ExtendedTreeNode recipeTreeNode)
        {
            IAssetReference assetReference = GetAssetReferenceByName(recipe.Name);

            if (assetReference != null && recipe.Recurrent)
            {
                ExtendedTreeNode executeNode = AddExecuteRecipeNode(assetReference, recipeTreeNode);
                executeNode.Text = Resources.Navigator_RunThisRecipeAgain;
            }
        }


        /// <summary>
        /// Adds the execute recipe node.
        /// </summary>
        private ExtendedTreeNode AddExecuteRecipeNode(IAssetReference assetReference, ExtendedTreeNode recipeTreeNode)
        {
            ExtendedTreeNode executeTreeNode = new ExtendedTreeNode();

            if (assetReference is UnboundTemplateReference || assetReference is BoundTemplateReference)
            {
                executeTreeNode.Text = Properties.Resources.Navigator_UnfoldThisTemplate;
            }
            else
            {
                executeTreeNode.Text = Properties.Resources.Navigator_RunThisRecipe;
            }

            executeTreeNode.ToolTipText = String.Format(Properties.Resources.Reference_AppliesToTooltip, assetReference.AppliesTo);

            executeTreeNode.NodeFont = (executeTreeNode.NodeFont != null) ? new Font(executeTreeNode.NodeFont, FontStyle.Bold) : new Font(SystemFonts.DefaultFont, FontStyle.Bold);
            executeTreeNode.Tag = assetReference;
            executeTreeNode.ShowAsLink = true;
            executeTreeNode.ImageIndex = ImageExecuteIndex;
            executeTreeNode.SelectedImageIndex = ImageExecuteIndex;
            executeTreeNode.StateImageIndex = ImageExecuteIndex;
            recipeTreeNode.Nodes.Add(executeTreeNode);

            return executeTreeNode;
        }

        /// <summary>
        /// Adds the documentation node based on the specified Link configuration.
        /// </summary>
        /// <param name="recipeTreeNode">The recipe tree node.</param>
        /// <param name="documentationLink">The documentation link.</param>
        private void AddDocumentationNode(ExtendedTreeNode recipeTreeNode, Link documentationLink)
        {
            ExtendedTreeNode documentationTreeNode = new ExtendedTreeNode();
            documentationTreeNode.ShowAsLink = true;
            documentationTreeNode.Text = documentationLink.Caption;
            documentationTreeNode.Tag = documentationLink;
            documentationTreeNode.ImageIndex = ImageDocumentationIndex;
            documentationTreeNode.SelectedImageIndex = ImageDocumentationIndex;
            documentationTreeNode.StateImageIndex = ImageDocumentationIndex;
            recipeTreeNode.Nodes.Add(documentationTreeNode);
        }
        #endregion

        #region Private Common Helpers
        /// <summary>
        /// Opens a given uri in the help window.
        /// </summary>
        /// <param name="uri">The URI.</param>
        private void ShowHelp(string uri)
        {
            string helpLocation = ParseUri(uri);

            Microsoft.VisualStudio.VSHelp.Help help = (Microsoft.VisualStudio.VSHelp.Help)base.GetService(typeof(Microsoft.VisualStudio.VSHelp.Help));
            if (help != null)
            {
                try
                {
                    help.DisplayTopicFromURL(helpLocation);
                    Cursor.Current = Cursors.Arrow;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format(Resources.Navigator_CannotGetHelpResource, helpLocation), ex);
                }
            }
        }

        /// <summary>
        /// Parses the specified uri and return the combined base path when possible.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        private string ParseUri(string uri)
        {
            GuidancePackageEntry packageItem = cbGuidancePackages.SelectedItem as GuidancePackageEntry;
            if (packageItem == null)
                return uri;

            string basePath = packageItem.GuidancePackage.BasePath;
            string helpRelativeLocation = uri;

            Uri baselocation = null;
            string helpLocation = string.Empty;

            if (Uri.TryCreate(helpRelativeLocation, UriKind.RelativeOrAbsolute, out baselocation))
            {
                if (baselocation != null)
                {
                    if (baselocation.IsAbsoluteUri)
                    {
                        if (baselocation.IsFile)
                        {
                            helpLocation = baselocation.LocalPath;
                        }
                        else
                        {
                            helpLocation = baselocation.AbsoluteUri;
                        }
                    }
                    else
                    {
                        helpLocation = Path.Combine(basePath, helpRelativeLocation);
                    }
                }
            }
            else
            {
                throw new Exception(string.Format(Resources.Navigator_CannotGetHelpResource, helpLocation));
            }
            return helpLocation;
        }

        /// <summary>
        /// Selects the tool strip specified element.
        /// </summary>
        /// <param name="selectedToolStripItem">The selected tool strip item.</param>
        /// <param name="toolStrip">The tool strip.</param>
        private void SelectToolStripElement(ToolStripButton selectedToolStripItem, ToolStrip toolStrip)
        {
            if (selectedToolStripItem == null)
            {
                selectedToolStripItem = new ToolStripButton();
            }
            else
            {
                // ensure that the toolstrip is enabled.
                toolStrip.Enabled = true;
            }

            foreach (ToolStripItem item in toolStrip.Items)
            {
                if (item != selectedToolStripItem)
                {
                    ToolStripButton toolStripButton = item as ToolStripButton;
                    if (toolStripButton != null)
                    {
                        toolStripButton.Checked = false;
                    }
                }
            }

            selectedToolStripItem.Checked = true;
        }

        bool isShowingAvailableRecipes = false;

        /// <summary>
        /// Sets visible a given panel while making sure only one panel is visible at the same time.
        /// </summary>
        /// <param name="visiblePanel">The panel to make visible</param>
        private void SetVisiblePanel(Panel visiblePanel)
        {
            this.SuspendLayout();

            foreach (Panel panel in this.panelGroup)
            {
                panel.Visible = false;
            }
            if (visiblePanel == panelAvailableRecipes)
            {
//                SubscribeToSolutionExplorerChanges();
                isShowingAvailableRecipes = true;
                this.toolStripRefreshButton.Visible = true;
            }
            else
            {
//                UnsubscribeToSolutionExplorerChanges();
                isShowingAvailableRecipes = false;
                this.toolStripRefreshButton.Visible = false;
            }
            visiblePanel.Visible = true;
            this.ResumeLayout();
        }
        #endregion

        bool listeningToSolutionExplorerChanges = false;
        #region Solution Explorer change tracking
        private void SubscribeToSolutionExplorerChanges()
        {
            if (listeningToSolutionExplorerChanges)
            {
                return;
            }
            guidanceNavigatorManager.SolutionExplorerChanged += new EventHandler(guidanceNavigatorManager_SolutionExplorerChanged);
            guidanceNavigatorManager.SubscribeToSolutionExplorerChanges();
            listeningToSolutionExplorerChanges = true;
        }

        private void UnsubscribeToSolutionExplorerChanges()
        {
            if (!listeningToSolutionExplorerChanges)
            {
                return;
            }
            guidanceNavigatorManager.SolutionExplorerChanged -= new EventHandler(guidanceNavigatorManager_SolutionExplorerChanged);
            guidanceNavigatorManager.UnsubscribeToSolutionExplorerChanges();
            listeningToSolutionExplorerChanges = false;
        }

        void guidanceNavigatorManager_SolutionExplorerChanged(object sender, EventArgs e)
        {
            // the solution has been modified (i.e. items added, removed, etc)

            // if the gNav is showing the Available Guidance tab we need to invalidate the cache and refresh the available guidance list
            if (isShowingAvailableRecipes)
            {
                UpdateAvailableGuidance(true);
            }
            // instead, if gNav is not showing Available Guidance tab, we just invalidate the cache, so next time its shown it will display fresh data 
            else
            {
                ClearAvailableReferencesCache();
            }
        }
        #endregion

        #region Toolstrip buttons -Overview, Available Recipes, History- Event Handlers
        /// <summary>
        /// Handles the Click event of the toolStripPackageOverview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void toolStripPackageOverview_Click(object sender, EventArgs e)
        {
            SelectToolStripElement((ToolStripButton) sender, this.toolStripPackage);
            SetVisiblePanel(panelOverview);
        }

        /// <summary>
        /// Handles the Click event of the toolStripCompletedRecipes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void toolStripCompletedRecipes_Click(object sender, EventArgs e)
        {
            ShowHistoryRecipes();
        }

        private void ShowHistoryRecipes()
        {
            SelectToolStripElement(toolStripCompletedRecipes, this.toolStripPackage);
            SetVisiblePanel(panelCompletedRecipes);
        }

        /// <summary>
        /// Handles the Click event of the toolStripAvailableRecipes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void toolStripAvailableRecipes_Click(object sender, EventArgs e)
        {
            ShowAvailableRecipes();
        }

        private void ShowAvailableRecipes()
        {
            SelectToolStripElement(toolStripAvailableRecipes, this.toolStripPackage);
            SetVisiblePanel(panelAvailableRecipes);

            UpdateAvailableGuidance();
            // While unfolding package the event that refresh the available recipes, is executed before
            // the solution explorer hierachy is constructed.
            //if (this.availableRecipesTreeView.GetNodeCount(true) <= 0)
            //{
            //    UpdateAvailableGuidance();
            //}
        }

        /// <summary>
        /// Handles the Click event of the toolStripRefreshButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void toolStripRefreshButton_Click(object sender, EventArgs e)
        {
            UpdateAvailableGuidance(true);
        }
        #endregion

        private class GuidancePackageEntry
        {
            public GuidancePackage GuidancePackage;

            public override string ToString()
            {
                return GuidancePackage.Configuration.Caption;
            }

            public GuidancePackageEntry(GuidancePackage guidancePackage)
            {
                this.GuidancePackage = guidancePackage;
            }
        }

        private void wbPackageOverview_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.ToString().ToLower().StartsWith("ms-help"))
            {
                Microsoft.VisualStudio.VSHelp.Help help = (Microsoft.VisualStudio.VSHelp.Help)base.GetService(typeof(Microsoft.VisualStudio.VSHelp.Help));
                if (help != null)
                {
                    e.Cancel = true;
                    try
                    {
                        help.DisplayTopicFromURL(e.Url.ToString());
                        Cursor.Current = Cursors.Arrow;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format(Resources.Navigator_CannotGetHelpResource, e.Url.ToString()), ex);
                    }
                }
            }
            return;
        }
    }
}
