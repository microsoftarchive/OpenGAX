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

using System;
using System.Collections.Generic;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.Configuration;
using EnvDTE;
using System.Diagnostics;
using VSLangProj;
using EnvDTE80;
using Microsoft.Practices.RecipeFramework.VisualStudio.Properties;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.ToolWindow
{
	internal class GuidanceNavigatorManager
    {
        IServiceProvider serviceProvider;
        IRecipeManagerService rms;
        GuidancePackage[] enabledGuidancePackages;

        Dictionary<String, List<RecipeExecutionHistory>> executedRecipesLog;

        /// <summary>
        /// Occurs just after executing a recipe.
        /// </summary>
        internal event RecipeEventHandler RecipeExecuted;
        /// <summary>
        /// Occurs when the list of binded packages change (i.e. package is enabled or disabled)
        /// </summary>
        internal event EventHandler BindedPackagesChange;
        internal event EventHandler SolutionExplorerChanged;

        internal GuidanceNavigatorManager(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            this.serviceProvider = serviceProvider;
            executedRecipesLog = new Dictionary<String, List<RecipeExecutionHistory> >();
            rms = (IRecipeManagerService) serviceProvider.GetService(typeof(IRecipeManagerService));
            rms.EnabledPackage += new PackageEventHandler(OnEnabledDisabledPackage);
            rms.EnablingPackage += new CancelPackageEventHandler(rms_EnablingPackage);
            rms.DisabledPackage += new PackageEventHandler(OnEnabledDisabledPackage);
            rms.AfterRecipeExecution += new RecipeEventHandler(OnAfterRecipeExecution);
            enabledGuidancePackages = new GuidancePackage[] { };
        }

        void rms_EnablingPackage(object sender, CancelPackageEventArgs e)
        {
            enabledGuidancePackages = rms.GetEnabledPackages();
            if (BindedPackagesChange != null)
            {
                BindedPackagesChange(sender, e);
            }
        }

        ~GuidanceNavigatorManager()
        {
            rms.EnabledPackage -= new PackageEventHandler(OnEnabledDisabledPackage);
            rms.DisabledPackage -= new PackageEventHandler(OnEnabledDisabledPackage);
            rms.AfterRecipeExecution -= new RecipeEventHandler(OnAfterRecipeExecution);
        }

        internal GuidancePackage[] EnabledGuidancePackages
        {
            get
            {
                return enabledGuidancePackages;
            }
        }

        void OnAfterRecipeExecution(object sender, RecipeEventArgs e)
        {
            GuidancePackage package = (GuidancePackage)sender;
            // we don't want the toolwindow to get notificatins of recipes
            // that executed because of the unfolding of a template
            if (e.ExecutedFromTemplate)
            {
                return;
            }
            LogRecipeExecution(package.Configuration.Name, e.Recipe.Name);
            if (RecipeExecuted != null)
            {
                RecipeExecuted(sender, e);
            }
        }

        void LogRecipeExecution(String packageName, String recipeName)
        {
            if (!executedRecipesLog.ContainsKey(packageName))
            {
                executedRecipesLog.Add(packageName, new List<RecipeExecutionHistory> ());
            }

            List<RecipeExecutionHistory> log = executedRecipesLog[packageName];
            
            if (log == null)
            {
                log = new List<RecipeExecutionHistory> ();
                executedRecipesLog[packageName] = log;
            }
            log.Add(new RecipeExecutionHistory(recipeName));
        }

        void OnEnabledDisabledPackage(object sender, PackageEventArgs e)
        {
            enabledGuidancePackages = rms.GetEnabledPackages();
            if (BindedPackagesChange != null)
            {
                BindedPackagesChange(sender, e);
            }
        }

        internal void RemoveLogRecipeExecution(GuidancePackage guidancePackage, RecipeExecutionHistory recipeExecutionHistory)
        {
            string packageName = guidancePackage.Configuration.Name;

            if (!executedRecipesLog.ContainsKey(packageName))
                return;

            List<RecipeExecutionHistory> log = executedRecipesLog[packageName];
            if (log.Contains(recipeExecutionHistory))
            {
                log.Remove(recipeExecutionHistory);
            }
        }

        internal List<RecipeExecutionHistory>  GetExecutedRecipes(GuidancePackage guidancePackage)
        {
            string packageName = guidancePackage.Configuration.Name;

            if (executedRecipesLog.ContainsKey(packageName))
            {
                return executedRecipesLog[guidancePackage.Configuration.Name];
            }
            else
                return new List<RecipeExecutionHistory> ();
        }

        private void GetAssetLists(IAssetReference[] allReferences, out List<IUnboundAssetReference> unboundAssetReferenceList, out List<IAssetReference> otherAssetReferenceList)
        {
            unboundAssetReferenceList = new List<IUnboundAssetReference>();
            otherAssetReferenceList = new List<IAssetReference>();

            foreach (IAssetReference reference in allReferences)
            {
                IUnboundAssetReference unboundAssetReference = reference as IUnboundAssetReference;
                if (unboundAssetReference != null)
                {
                    unboundAssetReferenceList.Add(unboundAssetReference);
                }
                else
                {
                    otherAssetReferenceList.Add(reference);
                }
            }
        }

        internal Dictionary<string, IAssetReference> GetAvailableRecipes(GuidancePackage guidancePackage)
        {
#if DEBUG
            DateTime startTime = DateTime.Now;
            Debug.WriteLine(String.Format("GetAvailableRecipes - Start time {0}.", startTime.ToLongTimeString()));
#endif
            Dictionary<string, IAssetReference> availableRecipes = new Dictionary<string, IAssetReference>();
            
            IAssetReferenceService referenceService = (IAssetReferenceService)guidancePackage.GetService(typeof(IAssetReferenceService), true);

            IOutputWindowService outputWindow = guidancePackage.GetService<IOutputWindowService>();


            IPersistenceService persistenceService = guidancePackage.GetService<IPersistenceService>();
            IAssetReference[] allReferences = null;

            allReferences = persistenceService.LoadReferences(guidancePackage.Configuration.Name);
#if DEBUG
            Debug.WriteLine(String.Format("GetAvailableRecipes - Probing using {0} references.", allReferences.Length));
#endif
           
            List<IUnboundAssetReference> unboundAssetReferenceList;
            List<IAssetReference> otherAssetList;
            GetAssetLists(allReferences, out unboundAssetReferenceList, out otherAssetList);
#if DEBUG
            Debug.WriteLine(String.Format("GetAvailableRecipes - Probing using {0} unbound references.", unboundAssetReferenceList.Count));
            Debug.WriteLine(String.Format("GetAvailableRecipes - Probing using {0} other references.", otherAssetList.Count));
#endif
            List<object> allPossibleTargets = GetAllSolutionExplorerItems();
#if DEBUG
            Debug.WriteLine(String.Format("GetAvailableRecipes - Probing against {0} targets.", allPossibleTargets.Count));
#endif


            List<IUnboundAssetReference> unboundAssetReferenceWithNoTargetList = new List<IUnboundAssetReference>();
            foreach (IUnboundAssetReference unboundAssetReference in unboundAssetReferenceList)
            {
                bool referenceHasValidTarget = false;
                
                foreach (object item in allPossibleTargets)
                {
                    try
                    {
                        if (unboundAssetReference.IsEnabledFor(item))
                        {
                            referenceHasValidTarget = true;
                            break;
                        }
                    }
                    catch (Exception resolveReferenceException)
                    {
                        // The reference is not valid for this item. 
                        outputWindow.Display(string.Format(Resources.Navigator_ReferenceThrowException, unboundAssetReference.AssetName, resolveReferenceException.Message));
                    }
                }
                if (!referenceHasValidTarget)
                {
                    unboundAssetReferenceWithNoTargetList.Add(unboundAssetReference);
                }
            }

#if DEBUG
            Debug.WriteLine(String.Format("GetAvailableRecipes - Removing {0} unbound referenes with no valid targets", unboundAssetReferenceWithNoTargetList.Count));
#endif
            // remove those unbound asset references that don't have a valid target in the whole solution
            foreach (IUnboundAssetReference reference in unboundAssetReferenceWithNoTargetList)
            {
                unboundAssetReferenceList.Remove(reference);
            }

            // add the unbound asset references that have a valid target
            foreach (IAssetReference reference in unboundAssetReferenceList)
            {
                otherAssetList.Add(reference);
            }

            foreach (IAssetReference reference in otherAssetList)
            {
                if (!availableRecipes.ContainsKey(reference.AssetName))
                {
                    availableRecipes.Add(reference.AssetName, reference);
                }
            }
#if DEBUG
            DateTime endTime = DateTime.Now;
            Debug.WriteLine(String.Format("GetAvailableRecipes - End time {0}.", endTime.ToLongTimeString()));
            TimeSpan ts = endTime.Subtract(startTime);
            Debug.WriteLine(String.Format("GetAvailableRecipes - Executed in {0} ms.", ts.Milliseconds));
#endif
            return availableRecipes;
        }

        internal Recipe GetRecipeConfiguration(GuidancePackage guidancePackage, string recipeName)
        {
            foreach (Recipe recipe in guidancePackage.Configuration.Recipes)
            {
                if (recipe.Name == recipeName)
                {
                    return recipe;
                }
            }
            return null;
        }

        internal object SaveState ()
        {
            return this.executedRecipesLog;
        }

        internal void LoadState (object o)
        {
            executedRecipesLog = o as Dictionary<String, List<RecipeExecutionHistory>>;
        }

        internal void OnSolutionExplorerChanged ()
        {
            if (SolutionExplorerChanged != null)
            {
                SolutionExplorerChanged(this, EventArgs.Empty);
            }
        }

        internal void Reset()
        {
            executedRecipesLog.Clear();
        }

        internal List<object> GetAllSolutionExplorerItems()
        {
            List<object> solutionItems = new List<object>();

            DTE vs = (DTE)serviceProvider.GetService(typeof(DTE));

            foreach (Project project in vs.Solution.Projects)
            {
                LoadProject(project, solutionItems);
            }

            // add the solution root element too
            solutionItems.Add(vs.Solution);

            return solutionItems;
        }

        private void LoadProject(Project project, List<object> elements)
        {
            if (project.Object is EnvDTE80.SolutionFolder)
            {
                elements.Add(project.Object);
            }
            else
            {
                elements.Add(project);
            }
            VSProject vsProj = project.Object as VSProject;
            if (vsProj != null)
            {
                foreach (Reference reference in vsProj.References)
                {
                    elements.Add(reference);
                }
            }
            LoadItems(project.ProjectItems, elements);
        }

        private void LoadItems(ProjectItems items, List<object> elements)
        {
            if (items == null)
                return;

            foreach (ProjectItem item in items)
            {
                if (item.Object is Project)
                {
                    LoadProject((Project)item.Object, elements);
                }
                else
                {
                    elements.Add(item);
                    LoadItems(item.ProjectItems, elements);
                }
            }
        }

        #region Project/Items changes event handling
        void OnProjectItemRemoved(ProjectItem projectItem)
        {
            OnSolutionExplorerChanged();
        }

        void OnProjectItemRenamed(ProjectItem projectItem, string oldname)
        {
            OnSolutionExplorerChanged();
        }

        void OnProjectAdded(Project Project)
        {
            OnSolutionExplorerChanged();
        }
        #endregion

        #region Solution Explorer change tracking

        // See VSWhidbey bug #445930 on why we keep these references.
        SolutionEvents solutionEvents;
        ProjectItemsEvents solutionItemsEvents;
        bool isListeningToSolutionExplorerChanges = false;

        internal void SubscribeToSolutionExplorerChanges()
        {
            if (isListeningToSolutionExplorerChanges)
            {
                return;
            }

            isListeningToSolutionExplorerChanges = true;
            
            DTE dte = (DTE)serviceProvider.GetService(typeof(DTE));

            if (dte != null)
            {
                DTE2 dte2 = (DTE2)dte;

                this.solutionEvents = dte.Events.SolutionEvents;
                solutionEvents.ProjectAdded += new _dispSolutionEvents_ProjectAddedEventHandler(OnProjectAdded);

                this.solutionItemsEvents = dte.Events.SolutionItemsEvents;
                solutionItemsEvents.ItemRemoved += new _dispProjectItemsEvents_ItemRemovedEventHandler(OnProjectItemRemoved);
                solutionItemsEvents.ItemRenamed += new _dispProjectItemsEvents_ItemRenamedEventHandler(OnProjectItemRenamed);
            }
        }

        internal void UnsubscribeToSolutionExplorerChanges()
        {
            if (!isListeningToSolutionExplorerChanges)
            {
                return;
            }
            DTE dte = (DTE)serviceProvider.GetService(typeof(DTE));
            if (dte != null)
            {
                DTE2 dte2 = (DTE2)dte;

                this.solutionEvents = dte.Events.SolutionEvents;
                solutionEvents.ProjectAdded -= new _dispSolutionEvents_ProjectAddedEventHandler(OnProjectAdded);

                this.solutionItemsEvents = dte.Events.SolutionItemsEvents;
                solutionItemsEvents.ItemRemoved -= new _dispProjectItemsEvents_ItemRemovedEventHandler(OnProjectItemRemoved);
                solutionItemsEvents.ItemRenamed -= new _dispProjectItemsEvents_ItemRenamedEventHandler(OnProjectItemRenamed);
            }
            isListeningToSolutionExplorerChanges = false;
        }
        #endregion

        [Serializable]
        internal class RecipeExecutionHistory
        {
            public DateTime Date;
            public string RecipeName;

            public RecipeExecutionHistory(string recipeName)
            {
                this.Date = DateTime.Now;
                this.RecipeName = recipeName;
            }
        }
    }
}
