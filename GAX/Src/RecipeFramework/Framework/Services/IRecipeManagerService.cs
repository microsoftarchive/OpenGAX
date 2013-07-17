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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Xml;
using System.Configuration;

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework.Services
{
	#region Delegates & Args

    /// <summary>
    /// Provides data for recipe notifications.
    /// </summary>
    public class RecipeEventArgs : EventArgs
    {
        private Configuration.Recipe recipe;
        private bool executedFromTemplate;

        /// <summary>
        /// Initializes the data class.
        /// </summary>
        /// <param name="recipe">The recipe instance for the event.</param>
        /// <param name="executedFromTemplate">Tells if the recipe was executed because the unfolding of a template</param>
        public RecipeEventArgs(Configuration.Recipe recipe, bool executedFromTemplate)
        {
            this.recipe = recipe;
            this.executedFromTemplate = executedFromTemplate;
        }

        /// <summary>
        /// Gets the recipe configuration instance.
        /// </summary>
        public Configuration.Recipe Recipe
        {
            get { return recipe; }
        }

        /// <summary>
        /// Determines if the recipe has been executed because the unfolding of a template.
        /// </summary>
        public bool ExecutedFromTemplate
        {
            get { return executedFromTemplate; }
        }
    }

	/// <summary>
	/// Provides data for package notifications.
	/// </summary>
	public class PackageEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes the data class.
		/// </summary>
		/// <param name="package">The package instance for the event.</param>
		public PackageEventArgs(GuidancePackage package)
		{
			this.package = package;
		}

		private GuidancePackage package;

		/// <summary>
		/// Gets the package instance.
		/// </summary>
		public GuidancePackage Package
		{
			get { return package; }
		}
	}

	/// <summary>
	/// Provides data for package notifications that can be canceled.
	/// </summary>
	public class CancelPackageEventArgs : PackageEventArgs
	{
		/// <summary>
		/// Initializes the data class.
		/// </summary>
		/// <param name="package">The package instance for the event.</param>
		/// <param name="cancel">Indicates whether the operation is canceled by default.</param>
		public CancelPackageEventArgs(GuidancePackage package, 
			bool cancel) : base(package)
		{
			this.cancel = cancel;
		}

		private bool cancel;

		/// <summary>
		/// Gets or sets a value indicating whether the event should be canceled.
		/// </summary>
		public bool Cancel
		{
			get { return cancel; }
			set { cancel = value; }
		}

		private bool executeBinding = true;

		/// <summary>
		/// Gets/sets whether the binding recipe (if present) should be executed. 
		/// Defaults to <see langword="true"/>.
		/// </summary>
		public bool ExecuteBindingRecipe
		{
			get { return executeBinding; }
			set { executeBinding = value; }
		}
	}

	/// <summary>
	/// Represents the method that handles a recipe event.
	/// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Argument data for the event.</param>
    public delegate void RecipeEventHandler(object sender, RecipeEventArgs e);

	/// <summary>
	/// Represents the method that handles a package event.
	/// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Argument data for the event.</param>
	public delegate void PackageEventHandler(object sender, PackageEventArgs e);

	/// <summary>
	/// Represents the method that handles a cancelable package event.
	/// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Argument data for the event.</param>
    public delegate void CancelPackageEventHandler(object sender, CancelPackageEventArgs e);

	#endregion

	/// <summary>
	/// Exposes global features of the Recipe Framework.
	/// </summary>
	public interface IRecipeManagerService : IServiceContainer, IContainer
	{
		#region Events

		/// <summary>
		/// Occurs when a package is about to be enabled. At this point the 
		/// package is not sited yet in the container.
		/// </summary>
		event CancelPackageEventHandler EnablingPackage;
		/// <summary>
		/// Occurs after a package has been enabled and sited in the container.
		/// </summary>
		event PackageEventHandler EnabledPackage;
		/// <summary>
		/// Occurs when a package is about to be disabled. At this point the 
		/// package is still sited in the container.
		/// </summary>
		event CancelPackageEventHandler DisablingPackage;
		/// <summary>
		/// Occurs after a package has been disabled. At this point the 
		/// package is no longer sited in the container.
		/// </summary>
		event PackageEventHandler DisabledPackage;
        /// <summary>
        /// Occurs just after a recipe is executed
        /// </summary>
        event RecipeEventHandler AfterRecipeExecution;

		#endregion Events

		#region Enable/Disable

		/// <summary>
		/// Enables a package that has already been installed with the framework 
		/// manifest.
		/// </summary>
		/// <param name="packageName">Name of the package to enable.</param>
		/// <remarks>
		/// Enabling a package by name implies it exists in the Recipe Framework manifest file, 
		/// by means of a previous installation.
		/// </remarks>
        /// <returns>
        /// Returns the loaded and enabled package.
        /// </returns>
		GuidancePackage EnablePackage(string packageName);

		/// <summary>
		/// Enables a package.
		/// </summary>
		/// <param name="configuration">
		/// Configuration to use for the new package.
		/// </param>
        /// <returns>
        /// Returns the loaded and enabled package.
        /// </returns>
        GuidancePackage EnablePackage(XmlReader configuration);

		/// <summary>
		/// Enables a package.
		/// </summary>
		/// <param name="configuration">
		/// Configuration to use for the new package.
		/// </param>
        /// <returns>
        /// Returns the loaded and enabled package.
        /// </returns>
        GuidancePackage EnablePackage(Configuration.GuidancePackage configuration);

		/// <summary>
		/// Enables a preloaded package.
		/// </summary>
		/// <param name="package">
		/// The package instance to enable on the manager.
		/// </param>
		void EnablePackage(GuidancePackage package);

		/// <summary>
		/// Disables a package.
		/// </summary>
		/// <param name="packageName">Name of the package to disable.</param>
		/// <remarks>
		/// Disabling a package also causes it to be disposed.
		/// </remarks>
		void DisablePackage(string packageName);

		/// <summary>
		/// Disables a package.
		/// </summary>
		/// <param name="package">
		/// The package instance to disable on the manager.
		/// </param>
		/// <remarks>
		/// Disabling a package also causes it to be disposed.
		/// </remarks>
		void DisablePackage(GuidancePackage package);

		#endregion Enable/Disable

		#region Getter methods

		/// <summary>
		/// Gets an already loaded package.
		/// </summary>
		/// <param name="name">The name of the package to receive.</param>
		/// <returns>
        /// A loaded package with the given name, or <see langword="null"/>.
        /// </returns>
        GuidancePackage GetPackage(string name);

		/// <summary>
		/// Gets all enabled packages.
		/// </summary>
        /// <returns>
        /// All the packages that have been enabled.
        /// </returns>
        GuidancePackage[] GetEnabledPackages();

		/// <summary>
		/// Gets all installed packages.
		/// </summary>
        /// <returns>
        /// The configuration information for all installed packages.
        /// </returns>
        Configuration.Manifest.GuidancePackage[] GetInstalledPackages();

		/// <summary>
		/// Gets all installed packages for a certain host.
		/// </summary>
        /// <param name="forHost">The host to look installed packages for.</param>
        /// <returns>
        /// The configuration information for all installed packages for a given host.
        /// </returns>
        Configuration.Manifest.GuidancePackage[] GetInstalledPackages(string forHost);

		/// <summary>
		/// Retrieves the full in-memory model of the package configuration.
		/// </summary>
        /// <param name="packageName">The package to get the configuration information for.</param>
        /// <returns>
        /// The configuration information for the given package.
        /// </returns>
        /// <exception cref="ConfigurationException">A package with the given name is not installed.</exception>
        Configuration.GuidancePackage GetConfiguration(string packageName);

        /// <summary>
        /// Returns the Main Manifest path for the current hive.
        /// </summary>
        /// <returns></returns>
        string GetMainManifestFullPath(string hive);


		#endregion Getter methods
	}
}
