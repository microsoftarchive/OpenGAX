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
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Microsoft.Practices.RecipeFramework.Services;

using Microsoft.Practices.RecipeFramework.Internal;
using Mvp.Xml.XInclude;
using Microsoft.Practices.Common.Services;
using System.Diagnostics;
using System.Security.Policy;
using System.Security;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ExtensionManager;

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Main container that ultimately hosts all framework components and child 
	/// containers. Provides package management features.
	/// </summary>
	public class RecipeManager : ComponentModel.ServiceContainer, IRecipeManagerService
    {
        /// <summary>
        /// Embedded resource with empty manifest if no manifest is present.
        /// </summary>
        static readonly string EmptyManifest = Common.ReflectionHelper.GetAssemblyName(
            typeof(RecipeManager)) + ".EmptyManifest.xml";
        const string ManifestFile = "RecipeFramework.xml";
        const string DefaultManifestHive = "8.0";
        const string VisualStudioRoot = @"Software\Microsoft\VisualStudio\";
        const string GaxRoot = @"SOFTWARE\Microsoft\Guidance Automation Extensions";

        #region XPath expressions

        /// <summary>
        /// Retrieves the host the package is registered for in the manifest file.
        /// </summary>
        const string XPathPackageHost = "string(gax:RecipeFramework/gax:GuidancePackages/gax:GuidancePackage[@Name=$name]/@Host)";

        /// <summary>
        /// Retrieves the package data with a specified name from the framework manifest file.
        /// </summary>
        const string XPathPackage = "gax:RecipeFramework/gax:GuidancePackages/gax:GuidancePackage[@Name=$name]";

        /// <summary>
        /// Retrieves all the packages that are installed for a host.
        /// </summary>
        const string XPathPackageByHost = "gax:RecipeFramework/gax:GuidancePackages/gax:GuidancePackage[@Host=$host]";

        #endregion XPath expressions

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeManager"/> class.
        /// </summary>
        /// <remarks>To let contained components publish additional services, use 
        /// the overload receiving the <c>exposeServiceContainerService</c> flag.</remarks>
        public RecipeManager()
        {
            InitializeServices();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeManager"/> class using the specified parent service provider.
        /// </summary>
        /// <param name="exposeServiceContainerService">Indicates whether the <see cref="ComponentModel.ServiceContainer"/> service 
        /// for this container should be exposed to allow contained component to publish new services.</param>
        public RecipeManager(bool exposeServiceContainerService)
            : base(exposeServiceContainerService)
        {
            InitializeServices();
        }

        #endregion Constructors

        #region Initialization

        private void InitializeServices()
        {
            AddService(typeof(IRecipeManagerService), this);
            // UNDONE: implement GAC support by taking this path from the framework manifest.
            AddService(typeof(ITypeResolutionService), new TypeResolutionService(
                Path.GetDirectoryName(Common.ReflectionHelper.GetAssemblyPath(this.GetType().Assembly))));
        }

        #endregion Initialization

        #region Container Overrides (actual implementations)

        /// <summary>
        /// Adds a package to the manager, enabling it for use.
        /// </summary>
        /// <param name="component">The package component.</param>
        /// <param name="name">Optional name (can be <see langword="null"/>) 
        /// that must match the package name.</param>
        /// <remarks>
        /// This method is also called to add to the container services that 
        /// inherit from <see cref="IComponent"/>, so the type of the component 
        /// is not enforced. Just special logic is applied to package component types.
        /// </remarks>
        public override void Add(IComponent component, string name)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            if (component is GuidancePackage)
            {
                GuidancePackage package = (GuidancePackage)component;
                if (name != null && name != package.Configuration.Name)
                {
                    throw new ArgumentException(String.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
                        Properties.Resources.RecipeManager_NameDoesntMatch,
                        name, package.Configuration.Caption));
                }
                // If there's a host listening, query for name to see if they match.
                IHostService host = GetService<IHostService>();
                if (host != null && host.HostName != package.Configuration.Host)
                {
                    throw new ArgumentException(String.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
                        Properties.Resources.RecipeManager_PackageHostMismatch,
                        package.Configuration.Caption,
                        package.Configuration.Host,
                        host.HostName));
                }

                try
                {
                    // Force the package to be sited at Enabling event time also.
                    base.Add(component, package.Configuration.Name);
                    bool executebinding = true;
                    if (EnablingPackage != null)
                    {
                        CancelPackageEventArgs args = new CancelPackageEventArgs(
                            package, false);
                        EnablingPackage(this, args);
                        if (args.Cancel)
                        {
                            base.Remove(component);
                            return;
                        }
                        executebinding = args.ExecuteBindingRecipe;
                    }
                    if (executebinding && package.Configuration.BindingRecipe != null &&
                        package.Configuration.BindingRecipe.Length > 0)
                    {
                        package.TurnOnOutput();
                        package.Execute(package.Configuration.BindingRecipe);
                        package.TurnOffOutput();
                    }
                    if (EnabledPackage != null)
                    {
                        EnabledPackage(this, new PackageEventArgs(package));
                    }
                }
                catch
                {
                    IPersistenceService persistanceService = package.GetService<IPersistenceService>(true);
                    // Notify host that we're disabling.
                    if (DisablingPackage != null)
                    {
                        DisablingPackage(this, new CancelPackageEventArgs(package, false));
                    }
                    // Don't let the package be enabled if an exception happened.
                    base.Remove(component);
                    if (DisabledPackage != null)
                    {
                        DisabledPackage(this, new PackageEventArgs(package));
                    }
                    persistanceService.ClearState(package.Configuration.Name);
                    throw;
                }
            }
            else
            {
                // Just add the component so it gets sited. 
                // Services that implement IComponent will arrive here.
                base.Add(component, name);
            }
        }

        /// <summary>
        /// Removes a component from the manager. If the component is a 
        /// package, it is disabled for execution. All components are disposed upon removal.
        /// </summary>
        /// <param name="component">The component to remove.</param>
        public override void Remove(IComponent component)
        {
            if (component is GuidancePackage)
            {
                GuidancePackage package = (GuidancePackage)component;

                if (DisablingPackage != null)
                {
                    CancelPackageEventArgs args = new CancelPackageEventArgs(
                        package, false);
                    DisablingPackage(this, args);
                    if (args.Cancel)
                    {
                        return;
                    }
                }

                base.Remove(package);

                if (DisabledPackage != null)
                {
                    DisabledPackage(this, new PackageEventArgs(package));
                }

                package.Dispose();
            }
            else
            {
                base.Remove(component);
                component.Dispose();
            }
        }

        #endregion Container Overrides (actual implementations)

        #region Events

        /// <summary>
        /// Occurs when a package is about to be enabled.
        /// </summary>
        public event CancelPackageEventHandler EnablingPackage;
        /// <summary>
        /// Occurs after a package has been enabled.
        /// </summary>
        public event PackageEventHandler EnabledPackage;
        /// <summary>
        /// Occurs when a package is about to be disabled.
        /// </summary>
        public event CancelPackageEventHandler DisablingPackage;
        /// <summary>
        /// Occurs after a package has been disabled.
        /// </summary>
        public event PackageEventHandler DisabledPackage;
        /// <summary>
        /// Occurs just after a recipe is executed
        /// </summary>
        public event RecipeEventHandler AfterRecipeExecution;

        #endregion Events

        #region Getter methods

        /// <summary>
        /// Gets an already loaded package.
        /// </summary>
        /// <param name="name">The name of the package to receive.</param>
        /// <returns>
        /// A loaded package with the given name, or <see langword="null"/>.
        /// </returns>
        public GuidancePackage GetPackage(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("package");
            }
            return Components[name] as GuidancePackage;
        }

        /// <summary>
        /// Gets all enabled packages.
        /// </summary>
        /// <returns>
        /// All the packages that have been enabled.
        /// </returns>
        public GuidancePackage[] GetEnabledPackages()
        {
            ArrayList packages = new ArrayList(Components.Count);
            foreach (IComponent component in Components)
            {
                if (component is GuidancePackage)
                {
                    packages.Add(component);
                }
            }

            return (GuidancePackage[])packages.ToArray(typeof(GuidancePackage));
        }

        /// <summary>
        /// Gets all installed packages.
        /// </summary>
        /// <returns>
        /// The configuration information for all installed packages.
        /// </returns>
        public Configuration.Manifest.GuidancePackage[] GetInstalledPackages()
        {
            return GetInstalledPackagesForHive(string.Empty);
        }

        /// <summary>
        /// Gets all installed packages for the specified hive.
        /// </summary>
        /// <returns>
        /// The configuration information for all installed packages.
        /// </returns>
        public Configuration.Manifest.GuidancePackage[] GetInstalledPackagesForHive(string hive)
        {
			return this.GetExtensionManager()
				.GetGuidancePackages()
				.Select(extension => extension.AsGuidancePackage())
				.ToArray();
		}

        /// <summary>
        /// Removes all installed pagkages information from the Main manifest.
        /// </summary>
        public void RemoveInstalledPackages()
        {
			throw new NotSupportedException();
        }

		private IVsExtensionManager GetExtensionManager()
		{
			return (IVsExtensionManager)this.GetService(typeof(SVsExtensionManager));
		}

        /// <summary>
        /// Gets all installed packages for a certain host.
        /// </summary>
        /// <param name="forHost">The host to look installed packages for.</param>
        /// <returns>
        /// The configuration information for all installed packages for a given host.
        /// </returns>
        public Configuration.Manifest.GuidancePackage[] GetInstalledPackages(string forHost)
        {
			return this.GetExtensionManager()
				.GetGuidancePackages()
				.Select(extension => extension.AsGuidancePackage())
				.ToArray();
        }

        #endregion Getter methods

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
        /// See <see cref="IRecipeManagerService.EnablePackage(string)"/>.
        /// </returns>
        public GuidancePackage EnablePackage(string packageName)
        {
            if (packageName == null)
            {
                throw new ArgumentNullException("packageName");
            }
            XmlReader reader = GetConfigurationReader(this, packageName);
            try
            {
                return EnablePackage(reader);
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Enables a package.
        /// </summary>
        /// <param name="configuration">
        /// Configuration to use for the new package.
        /// </param>
        /// <returns>
        /// Returns the loaded and enabled package.
        /// </returns>
        public GuidancePackage EnablePackage(XmlReader configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

			GuidancePackage package = new GuidancePackage(configuration);
			try
            {
				package.AfterRecipeExecution += new RecipeEventHandler(package_AfterRecipeExecution);
                Add(package);
			}
            catch
            {
                package.Dispose();
                throw;
            }

			return package;
		}

        /// <summary>
        /// Enables a package.
        /// </summary>
        /// <param name="configuration">
        /// Configuration to use for the new package.
        /// </param>
        /// <returns>
        /// Returns the loaded and enabled package.
        /// </returns>
        public GuidancePackage EnablePackage(Configuration.GuidancePackage configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            GuidancePackage package = new GuidancePackage(configuration);
            try
            {
                package.AfterRecipeExecution += new RecipeEventHandler(package_AfterRecipeExecution);
                Add(package);
            }
            catch
            {
                package.Dispose();
                throw;
            }
            return package;
        }

        /// <summary>
        /// Enables a preloaded package.
        /// </summary>
        /// <param name="package">
        /// The package instance to enable on the manager.
        /// </param>
        public void EnablePackage(GuidancePackage package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

			package.AfterRecipeExecution += new RecipeEventHandler(package_AfterRecipeExecution);
            Add(package);
        }

        /// <summary>
        /// Disables a package.
        /// </summary>
        /// <param name="packageName">Name of the package to disable.</param>
        /// <remarks>
        /// Disabling a package causes it to be disposed also.
        /// </remarks>
        public void DisablePackage(string packageName)
        {
            if (packageName == null)
            {
                throw new ArgumentNullException("packageName");
            }
            GuidancePackage package = GetPackage(packageName);
            if (package != null)
            {
                package.AfterRecipeExecution -= new RecipeEventHandler(package_AfterRecipeExecution);
                Remove(package);
            }
        }

        /// <summary>
        /// Disables a package.
        /// </summary>
        /// <param name="package">
        /// The package instance to disable.
        /// </param>
        /// <remarks>
        /// Disabling a package causes it to be disposed also.
        /// </remarks>
        public void DisablePackage(GuidancePackage package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            package.AfterRecipeExecution -= new RecipeEventHandler(package_AfterRecipeExecution);
            Remove(package);
        }

        // bubble up the event so consumers of the IRecipeManagerService can get a hold of it
        void package_AfterRecipeExecution(object sender, RecipeEventArgs e)
        {
            if (AfterRecipeExecution != null)
            {
                AfterRecipeExecution(sender, e);
            }
        }

        #endregion Enable/Disable

        #region Manifest Management

        Hashtable configurationCache = new Hashtable();

        /// <summary>
        /// Retrieves the full in-memory model of the package configuration.
        /// </summary>
        /// <param name="packageName">The package to get the configuration information for.</param>
        /// <returns>
        /// The configuration information for the given package.
        /// </returns>
        /// <exception cref="ConfigurationException">A package with the given name is not installed.</exception>
        public Configuration.GuidancePackage GetConfiguration(string packageName)
        {
            if (packageName == null)
            {
                throw new ArgumentNullException("packageName");
            }
            GuidancePackage package = GetPackage(packageName);
            if (package != null)
            {
                return package.Configuration;
            }
            else
            {
                if (configurationCache.ContainsKey(packageName))
                {
                    return (Configuration.GuidancePackage)configurationCache[packageName];
                }

                // Otherwise, we have to read the entire file.
                XmlReader reader = GetConfigurationReader(this, packageName);
                // We don't perform extensive validation at this point, as this is just for displaying 
                // purposes. Just XSD validation will happen.
                Configuration.GuidancePackage packageconfig = (Configuration.GuidancePackage)
                    new Configuration.Serialization.GuidancePackageSerializer().Deserialize(reader);
                // Cache the configuration for later retrievals.
                configurationCache.Add(packageName, packageconfig);
                return packageconfig;
            }
        }

		public static string GetConfigurationFile(IServiceProvider serviceProvider, string packageName)
		{
			if (packageName == null)
			{
				throw new ArgumentNullException("packageName");
			}

			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			var extensionManager = serviceProvider.GetService(typeof(SVsExtensionManager)) as IVsExtensionManager;

			var package = extensionManager
				.GetGuidancePackages()
				.Select(extension => extension.AsGuidancePackage())
				.FirstOrDefault(p => p.Name == packageName);

			if (package == null)
				return null;

			return package.ConfigurationFile; 
		}
        /// <summary>
        /// Retrieves an <see cref="XmlReader"/> over the configuration for the specified package.
        /// </summary>
		/// <param name="packageName">The package to get the configuration reader for.</param>
		/// <param name="serviceProvider">The service provider.</param>
		/// <returns>
        /// The reader over the configuration.
        /// </returns>
        /// <exception cref="ArgumentException">The given package is not installed.</exception>
        /// <exception cref="ConfigurationException">The information about the package configuration location 
        /// is not valid or the package configuration was modified after installadion.</exception>
        public static XmlReader GetConfigurationReader(IServiceProvider serviceProvider, string packageName)
        {
            if (packageName == null)
            {
                throw new ArgumentNullException("packageName");
            }

			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			string configfile = GetConfigurationFile(serviceProvider, packageName);

			if (!File.Exists(configfile))
			{
				throw new System.Configuration.ConfigurationException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.RecipeManager_InvalidPackageLocation,
					configfile, packageName));
			}

			CheckPackageSecurity(packageName, configfile);

            XmlReader configreader = ResolveXInclude(configfile);

            #region Initialize the reader and schema

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ConformanceLevel = ConformanceLevel.Auto;

            // Pull the XSD from the config assembly (it's an embedded resource).
            using (Stream xsdstream = typeof(Configuration.GuidancePackage).Assembly.GetManifestResourceStream(
                Configuration.GuidancePackage.SchemaResourceName))
            {
                System.Diagnostics.Debug.Assert(xsdstream != null, "XSD not embedded in config assembly");

                // If the schema is not valid (we must check that at design-time), this will throw.
                // We never call close on the stream as it's an unmanaged resource stream. Closing 
                // it will render it unusable for the rest of the life of the AppDomain.
                XmlSchema xsd = XmlSchema.Read(xsdstream, null);
                settings.Schemas.Add(xsd);
            }

            XmlReader validatingreader = XmlReader.Create(
                configreader, settings);

            #endregion Initialize the reader and schema

            return validatingreader;
        }

        private static XmlReader ResolveXInclude(string configfile)
        {
            MemoryStream mem = new MemoryStream();
            XIncludingReader xir = new XIncludingReader(configfile);
            XmlWriter writer = XmlWriter.Create(mem);
            writer.WriteNode(xir, false);
            writer.Flush();

            mem.Position = 0;
            XmlReaderSettings settings = new XmlReaderSettings();
            return new FixedBaseURIWrappingReader(XmlReader.Create(mem), configfile);
        }

        #endregion Manifest Management

        private static void CheckPackageSecurity(string package, string location)
        {
            // Ensures that the package location is included in the MyComputer trusted zone.
            ZoneMembershipCondition condition = new ZoneMembershipCondition(SecurityZone.MyComputer);
            Evidence ev = new Evidence();
            Zone zone = Zone.CreateFromUrl(location);
            ev.AddHost(zone);

            if (!condition.Check(ev))
            {
                throw new SecurityException(String.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.RecipeManager_PackageNotTrusted,
                    package, location));
            }
        }


        /// <summary>
        /// Returns the main manifest full path for the current running hive.
        /// </summary>
        /// <returns></returns>
        public string GetMainManifestFullPath(string hive)
        {
			throw new NotSupportedException();
        }

        /// <summary>
        /// Return the current vs hive in where gax is executed.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentVsHive()
        {
            string regRoot = null;
            ILocalRegistry3 registry = Package.GetGlobalService(typeof(SLocalRegistry)) as ILocalRegistry3;

            // if we're at design-time try the VS service
            if (registry != null)
            {
                ErrorHandler.ThrowOnFailure(registry.GetLocalRegistryRoot(out regRoot));

                regRoot = regRoot.Replace(VisualStudioRoot, string.Empty);
                return regRoot.Split('\\')[0];
            }

            return "14.0_Config"; // todo: what if 2017?
        }
    }
}