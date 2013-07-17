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
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions.Vsd
{
    /// <summary>
    /// Action that adds a new registry locator in a setup project that
    /// is used by any custom launch condition.
    /// The input properties needed are: (a) SetupProject - the project
    /// where the new registry locator will be added (b) Locator - the
    /// locator name of the new registry locator (c) Property - the property
    /// name that the locator will create and that will be used by a launch 
    /// condition (d) RegistryKey - the registry key that will be searched 
    /// (e) Value - the value in the registry key supplied that is going to 
    /// be searched
    /// </summary>
    public class AddRegistryLocatorAction: ConfigurableAction 
    {
        #region Input Properties

        /// <summary>
        /// The project where the new registry locator will be added
        /// </summary>
        [Input(Required=true)]
        public EnvDTE.Project SetupProject
        {
            get { return setupProject; }
            set { setupProject = value; }
        } EnvDTE.Project setupProject;

        /// <summary>
        /// The locator name of the new registry locator
        /// </summary>
        [Input(Required=true)]
        public string LocatorName
        {
            get { return locatorName; }
            set { locatorName = value; }
        } string locatorName;

        /// <summary>
        /// The property name that the registry locator will create
        /// and will be used by a launch condition
        /// </summary>
        [Input(Required=true)]
        public string Property
        {
            get { return property; }
            set { property = value; }
        } string property;

        /// <summary>
        /// The registry key that will be searched 
        /// </summary>
        [Input(Required=true)]
        public string RegistryKey
        {
            get { return registryKey; }
            set { registryKey = value; }
        } string registryKey;

        /// <summary>
        /// The value in the registry key supplied that is going to 
        /// be searched
        /// </summary>
        [Input(Required=true)]
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        } string value;

        #endregion

        #region Output Properties

        /// <summary>
        /// The new locator object that is created after the 
        /// execution of the action
        /// </summary>
        [Output]
        public object Locator
        {
            get { return vsdLocator; }
            set { vsdLocator = (IVsdRegistryKeyLocator)value; }
        } IVsdRegistryKeyLocator vsdLocator;

        #endregion

        #region IAction members

        /// <summary>
        /// Creates the new registry locator
        /// </summary>
        public override void Execute()
        {
            try
            {
                IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
                IVsdCollectionSubset plugins = deployable.GetPlugIns();
                IVsdLocatorPlugIn locatorPlugin = plugins.Item("Locator") as IVsdLocatorPlugIn;
                if (locatorPlugin != null)
                {
                    vsdLocator =
                        (IVsdRegistryKeyLocator)DteHelper.CoCreateInstance(
							this.Site, 
                            typeof(VsdRegistryKeyLocatorClass),
                            typeof(IVsdRegistryKeyLocator));
                    vsdLocator.Name = this.LocatorName;
                    vsdLocator.Property = this.Property;
                    vsdLocator.RegKey = this.RegistryKey;
                    vsdLocator.Root = vsdRegistryRoot.vsdrrHKLM;
                    vsdLocator.Value = this.Value;
                    locatorPlugin.Items.Add(vsdLocator);
                }
            }
            catch (Exception)
            {
                vsdLocator = null;
                throw;
            }

        }

        /// <summary>
        /// Removes the locator that was recently created
        /// </summary>
        public override void Undo()
        {
            if (vsdLocator != null)
            {
                IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
                IVsdCollectionSubset plugins = deployable.GetPlugIns();
                IVsdLocatorPlugIn locatorPlugin = plugins.Item("Locator") as IVsdLocatorPlugIn;
                if (locatorPlugin != null)
                {
                    locatorPlugin.Items.RemoveObject(vsdLocator);
                    vsdLocator = null;
                }
            }
        }

        #endregion
    }
}
