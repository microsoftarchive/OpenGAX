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
using System.Collections;
using System.ComponentModel;

using Microsoft.Practices.ComponentModel;
using System.Diagnostics;
using System.Globalization;

#endregion

namespace Microsoft.Practices.RecipeFramework.Services
{
    /// <summary>
    /// Implements the <see cref="IAssetReferenceService"/>.
    /// </summary>
    /// <remarks>
    /// Comes built-in with the <see cref="IndexerBoundAsset"/>,
    /// <see cref="IndexerBoundTarget"/>, <see cref="IndexerBoundAssetTarget"/> and
    /// <see cref="IndexerUnboundAsset"/> indexers.
    /// </remarks>
    [ServiceDependency(typeof(IPersistenceService))]
    [ServiceDependency(typeof(IConfigurationService))]
    internal class AssetReferenceService : ContainerComponent, IAssetReferenceService
    {
        /// <summary>
        /// See <see cref="IAssetReferenceService.Changed"/>.
        /// </summary>
        public event EventHandler Changed;

        public AssetReferenceService()
        {
            AddIndexer(typeof(IndexerBoundAsset), new IndexerBoundAsset());
            AddIndexer(typeof(IndexerBoundTarget), new IndexerBoundTarget());
            AddIndexer(typeof(IndexerBoundAssetTarget), new IndexerBoundAssetTarget());
            AddIndexer(typeof(IndexerUnboundAsset), new IndexerUnboundAsset());
        }

        #region Fields

        /// <summary>
        /// Flags whether the service has been disposed.
        /// </summary>
        bool isDisposed = false;
        /// <summary>
        /// Holds the service that persists the references.
        /// </summary>
        IPersistenceService persistence;
        /// <summary>
        /// Flags whether we're performing a batch load of recipes, therefore 
        /// we should not save them at each iteration.
        /// </summary>
        bool isLoading = false;
        /// <summary>
        /// Holds indexers of references.
        /// </summary>
        IDictionary indexers = new System.Collections.Specialized.HybridDictionary();

        #endregion Fields

        #region Non-public Members

        private void OnChanged()
        {
            if (Changed != null)
            {
                Changed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Saves all the current recipe references.
        /// </summary>
        private void Save()
        {
            if (persistence != null)
            {
                IAssetReference[] refs = new IAssetReference[this.Components.Count];
                this.Components.CopyTo(refs, 0);
                persistence.SaveReferences(Package.Configuration.Name, refs);
            }
        }

        private void CheckState()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().ToString());
            }
            if (this.Site == null)
            {
                throw new InvalidOperationException(String.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.General_ComponentMustBeSited, this));
            }
            if (persistence == null)
            {
                persistence = GetService<IPersistenceService>(true);
            }
        }

        #endregion Non-public Members

        #region Container Overrides

        /// <summary>
        /// Disposes the service.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            indexers = null;
            isDisposed = true;
        }

        /// <summary>
        /// Override to provide a more meaningful message.
        /// </summary>
        protected override void ValidateName(IComponent component, string name)
        {
            try
            {
                // Will cause an ArgumentException if the reference key already exists.
                base.ValidateName(component, name);
            }
            catch (ArgumentException)
            {
                string appliesTo;
                try
                {
                    appliesTo = ((IAssetReference)component).AppliesTo;
                }
                catch (Exception e)
                {
                    this.TraceInformation(e.Message);
                    appliesTo = Properties.Resources.Reference_AppliesToThrew;
                }
                throw new ArgumentException(String.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.AssetReferenceService_DuplicateReference,
                    appliesTo,
                    ((IAssetReference)component).Key));
            }
        }

        protected override void OnSited()
        {
            base.OnSited();
            if (!(Site.Container is GuidancePackage))
            {
                throw new InvalidOperationException(Properties.Resources.AssetReferenceService_NotSitedOnPackage);
            }
            // Load all persisted references.
            IPersistenceService persistence = GetService<IPersistenceService>(true);
            AddRange(persistence.LoadReferences(Package.Configuration.Name), false, false);
        }

        #endregion Container Overrides

        #region Add/Remove references

        /// <summary>
        /// Adds a reference to the service. 
        /// </summary>
        /// <param name="component">The reference to add.</param>
        /// <param name="name">The name is ignored, as the <see cref="IAssetReference.Key"/> is used instead.</param>
        public override void Add(IComponent component, string name)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            if (!(component is IAssetReference))
            {
                throw new InvalidOperationException("component");
            }

            IAssetReference reference = (IAssetReference)component;
            base.Add(component, reference.Key);
            // Indexes and saves the reference.
            AddReference((IAssetReference)component);
        }

        /// <summary>
        /// See <see cref="IAssetReferenceService.Add(IAssetReference)"/>.
        /// </summary>
        public void Add(IAssetReference reference)
        {
            Add(reference, null);
        }

        /// <summary>
        /// See <see cref="IAssetReferenceService.Add(IAssetReference, bool)"/>.
        /// </summary>
        public void Add(IAssetReference reference, bool throwIfDuplicate)
        {
            Add(reference, null, throwIfDuplicate);
        }

        /// <summary>
        /// See <see cref="IAssetReferenceService.Add(IAssetReference, IDictionary)"/>.
        /// </summary>
        public void Add(IAssetReference reference, IDictionary initialState)
        {
            if (reference is RecipeReference)
            {
                IConfigurationService configuration = GetService<IConfigurationService>(true);
                // The configuration service will throw if the recipe doesn't exist.
                Configuration.Recipe recipeReferenced = configuration.CurrentPackage[reference.AssetName];
            }
            Add(reference, initialState, false);
        }

        /// <summary>
        /// It is actually the implementation of base.ValidateName
        /// but in this moment it has a failure in the code
        /// It is assuming that this.sites is not null, and in the first call
        /// before any Add is called the this.sites is null.
        /// Then creating our own implementation to check for duplicates
        /// </summary>
        /// <param name="component"></param>
        /// <param name="name"></param>
        void BaseValidateName(IComponent component, string name)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            if (name != null)
            {
                for (int num = 0; num < Components.Count; num++)
                {
                    ISite site = (ISite)Components[num].Site;
                    if (((site != null) && (site.Name != null)) &&
                        (string.Equals(site.Name, name, StringComparison.OrdinalIgnoreCase) &&
                        (site.Component != component)))
                    {
                        string appliesTo;
                        try
                        {
                            appliesTo = ((IAssetReference)component).AppliesTo;
                        }
                        catch (Exception e)
                        {
							this.TraceInformation(e.Message);
                            appliesTo = Properties.Resources.Reference_AppliesToThrew;
                        }
                        throw new ArgumentException(String.Format(
                            System.Globalization.CultureInfo.CurrentCulture,
                            Properties.Resources.AssetReferenceService_DuplicateReference,
                            appliesTo,
                            ((IAssetReference)component).Key));

                    }
                }
            }
        }

        /// <summary>
        /// See <see cref="IAssetReferenceService.Add(IAssetReference, IDictionary)"/>.
        /// </summary>
        public void Add(IAssetReference reference, IDictionary initialState, bool throwIfDuplicate)
        {
            try
            {
                BaseValidateName(reference, reference.Key);
            }
            catch (ArgumentException) // Exception thrown by the ValidateName method when duplicate components are added.
            {
                if (throwIfDuplicate)
                {
                    throw;
                }
                return;
            }
            try
            {
                Add((IComponent)reference);
            }
            catch (ArgumentException) // Exception thrown by the OnSited of the reference
            {
                Remove(reference);
            }

            if (initialState != null)
            {
                persistence.SaveState(Package.Configuration.Name, reference, initialState);
            }
        }

        private void AddReference(IAssetReference reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }
            if ((!(reference is IBoundAssetReference) && !(reference is IUnboundAssetReference)) ||
                (reference is IBoundAssetReference && reference is IUnboundAssetReference))
            {
                // Must implement one or the other.
                throw new InvalidOperationException(Properties.Resources.AssetReferenceService_MustBeBoundOrUnbound);
            }
            CheckState();
            if (reference is RecipeReference)
            {
                // Check compatibility with recipe definition.
                IConfigurationService configuration = GetService<IConfigurationService>(true);
                // The indexer will throw if the recipe doesn't exist.
                bool isbound = configuration.CurrentPackage[((RecipeReference)reference).AssetName].Bound;
                if (isbound && !(reference is IBoundAssetReference))
                {
                    throw new InvalidOperationException(String.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
                        Properties.Resources.AssetReferenceService_WrongForBound,
                        ((RecipeReference)reference).AssetName, Package.Configuration.Caption));
                }
                else if (!isbound && !(reference is IUnboundAssetReference))
                {
                    throw new InvalidOperationException(String.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
                        Properties.Resources.AssetReferenceService_WrongForUnbound,
                        ((RecipeReference)reference).AssetName, Package.Configuration.Caption));
                }
            }
            // Index the reference for retrieval.
            foreach (IAssetReferenceIndexer indexer in indexers.Values)
            {
                indexer.Add(reference);
            }
            if (!isLoading)
            {
                Save();
                OnChanged();
            }
        }

        /// <summary>
        /// See <see cref="IAssetReferenceService.AddRange(IAssetReference[])"/>.
        /// </summary>
        public void AddRange(IAssetReference[] references)
        {
            AddRange(references, false);
        }

        /// <summary>
        /// See <see cref="IAssetReferenceService.AddRange(IAssetReference[], bool)"/>.
        /// </summary>
        public void AddRange(IAssetReference[] references, bool throwIfDuplicate)
        {
            AddRange(references, true, throwIfDuplicate);
        }

        private void AddRange(IAssetReference[] references, bool save, bool throwIfDuplicate)
        {
            if (references == null)
            {
                throw new ArgumentNullException("references");
            }

            if (references.Length == 0) return;

            isLoading = true;
            // Keep references we add to undo on error.
            ArrayList added = new ArrayList(references.Length);

            try
            {
                // Add all references, but save at the end.
                foreach (IAssetReference reference in references)
                {
                    bool failure = false;
                    try
                    {
                        BaseValidateName(reference, reference.Key);
                    }
                    catch (ArgumentException) // Exception thrown by the ValidateName method when duplicate components are added.
                    {
                        failure = true;
                        if (throwIfDuplicate)
                        {
                            throw;
                        }
                    }
                    try
                    {
                        if (!failure)
                        {
                            Add((IComponent)reference);
                            added.Add(reference);
                        }
                    }
                    catch (ArgumentException) // Exception thrown by the OnSited of the reference
                    {
                        Remove(reference);
                    }
                }
            }
            catch
            {
                // Remove the references we added.
                foreach (IAssetReference reference in added)
                {
                    Remove(reference);
                }
                throw;
            }
            finally
            {
                isLoading = false;
            }

            if (save)
            {
                Save();
            }
            OnChanged();
        }

        /// <summary>
        /// Adds a reference from the service. 
        /// </summary>
        /// <param name="component">The reference to remove.</param>
        public override void Remove(IComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            if (!(component is IAssetReference))
            {
                throw new InvalidOperationException("component");
            }

            CheckState();
            base.Remove(component);
            IAssetReference reference = (IAssetReference)component;
            // Remove state.
            persistence.RemoveState(Package.Configuration.Name, reference);
            // Remove from indexers.
            foreach (IAssetReferenceIndexer indexer in indexers.Values)
            {
                indexer.Remove(reference);
            }

            if (!isLoading)
            {
                Save();
                OnChanged();
            }
        }

        /// <summary>
        /// See <see cref="IAssetReferenceService.Remove(IAssetReference)"/>.
        /// </summary>
        public IDictionary Remove(IAssetReference reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }
            CheckState();
            IDictionary state = persistence.RemoveState(Package.Configuration.Name, reference);
            // Normal removal procedure.
            Remove((IComponent)reference);
            return state;
        }

        #endregion Add/Remove references

        /// <summary>
        /// See <see cref="IAssetReferenceService.IsAssetEnabledFor"/>.
        /// </summary>
        public bool IsAssetEnabledFor(string asset, object target)
        {
            IAssetReference[] references = GetReferencesFor(asset, target);

            if (references != null)
            {
                foreach (IAssetReference reference in references)
                {
                    bool isEnabled = false;

                    if (reference is IBoundAssetReference)
                    {
                        return true;
                    }
                    else if (reference is IUnboundAssetReference)
                    {
                        try
                        {
                            isEnabled = ((IUnboundAssetReference)reference).IsEnabledFor(target);

                            if (isEnabled)
                                return true;
                        }
                        catch (Exception e)
                        {
                            throw new RecipeExecutionException(asset,
                                Properties.Resources.AssetReferenceService_FailEnabledFor, e);
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// See <see cref="IAssetReferenceService.GetReferenceFor"/>.
        /// </summary>
        public IAssetReference GetReferenceFor(string asset, object target)
        {
            IAssetReference[] references = Find(typeof(IndexerBoundAssetTarget), asset, target);
            if (references.Length == 0)
            {
                // If there are no bound references, look for unbound ones (will be only one for a given asset)
                references = Find(typeof(IndexerUnboundAsset), asset);
            }
            if (references.Length == 0)
            {
                return null;
            }
            else
            {
                return references[0];
            }
        }


        /// <summary>
        /// See <see cref="IAssetReferenceService.GetReferencesFor"/>.
        /// </summary>
        public IAssetReference[] GetReferencesFor(string asset, object target)
        {
            IAssetReference[] references = Find(typeof(IndexerBoundAssetTarget), asset, target);

            if (references.Length == 0)
            {
                // If there are no bound references, look for unbound ones (will be only one for a given asset)
                references = Find(typeof(IndexerUnboundAsset), asset);
            }

            return references;
        }

        /// <summary>
        /// See <see cref="IAssetReferenceService.GetAll"/>.
        /// </summary>
        public IAssetReference[] GetAll()
        {
            IAssetReference[] refs = new IAssetReference[this.Components.Count];
            this.Components.CopyTo(refs, 0);
            return refs;
        }

        /// <summary>
        /// See <see cref="IAssetReferenceService.AddIndexer"/>.
        /// </summary>
        public void AddIndexer(Type indexerType, IAssetReferenceIndexer indexerInstance)
        {
            if (indexerType == null)
            {
                throw new ArgumentNullException("indexerType");
            }
            if (indexerInstance == null)
            {
                throw new ArgumentNullException("indexerInstance");
            }
            indexers.Add(indexerType, indexerInstance);
            // Now, let's index the existing references in the new index
            foreach (IAssetReference existingReference in GetAll())
            {
                indexerInstance.Add(existingReference);
            }
        }

        /// <summary>
        /// See <see cref="IAssetReferenceService.AddIndexer"/>.
        /// </summary>
        public IAssetReference[] Find(Type indexerType, params object[] conditions)
        {
            if (indexerType == null)
            {
                throw new ArgumentNullException("indexerType");
            }
            IAssetReferenceIndexer indexer = (IAssetReferenceIndexer)indexers[indexerType];
            if (indexer == null)
            {
                throw new InvalidOperationException(String.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.AssetReferenceService_NoIndexer, indexerType));
            }
            return indexer.Find(conditions);
        }

        /// <summary>
        /// See <see cref="IAssetReferenceService.AddIndexer"/>.
        /// </summary>
        public IAssetReference FindOne(Type indexerType, params object[] conditions)
        {
            IAssetReference[] references = Find(indexerType, conditions);
            if (references.Length == 0)
            {
                return null;
            }
            else
            {
                return references[0];
            }
        }

        /// <summary>
        /// Gets the package the service is sited on.
        /// </summary>
        /// <remarks>
        /// Retrieval of this property must be done after the service has been 
        /// sited in a container.
        /// </remarks>
        public GuidancePackage Package
        {
            get
            {
                // Ensures site.
                CheckState();
                // Ensured at OnSited.
                return (GuidancePackage)this.Site.Container;
            }
        }
    }
}