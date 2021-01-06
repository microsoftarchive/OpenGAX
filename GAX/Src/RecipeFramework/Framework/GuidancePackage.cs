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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Schema;

using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework.Configuration.Serialization;
using Microsoft.Practices.RecipeFramework.Services;

// Create an alias to avoid conflicts with the Configuration property.
using Config = Microsoft.Practices.RecipeFramework.Configuration;
using Microsoft.Practices.Common.Services;
using Mvp.Xml.XInclude;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using Microsoft.Practices.RecipeFramework.Internal;
using System.Collections.Generic;

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Represents a logical grouping of recipes.
	/// </summary>
	/// <remarks> 
	/// A guidance package depends on an <see cref="IPersistenceService"/> being 
	/// exposed to suspend execution of recipes. The implementation of 
	/// this service must be hooked either into the parent <see cref="RecipeManager"/> 
	/// (for global storage common to all packages) or into the package itself for 
	/// package-specific storage.
	/// </remarks>
	[ServiceDependency(typeof(ITypeResolutionService))]
	public sealed class GuidancePackage : ComponentModel.ServiceContainer,
		IConfigurationService, IValueInfoService, IExecutionService
	{
		#region Fields

		/// <summary>
		/// Holds the <see cref="XmlReader.BaseURI"/> of the reader the configuration 
		/// was loaded from, or the path specified by the user.
		/// </summary>
		string basePath = String.Empty;

		/// <summary>
		/// Metadata for arguments by recipe.
		/// </summary>
		Dictionary<string, Dictionary<string, ValueInfo>> argumentsMetaDataByRecipe = new Dictionary<string, Dictionary<string, ValueInfo>>();

		/// <summary>
		/// Holds the current recipe being executed.
		/// </summary>
		Recipe currentRecipe;
		/// <summary>
		/// Holds the current reference being executed.
		/// </summary>
		IAssetReference currentReference;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="GuidancePackage"/> class 
		/// with a configuration file.
		/// </summary>
		/// <param name="configuration">The file containing the package configuration.</param>
		/// <remarks>
		/// External included files will be resolved as relative paths based on the 
		/// <paramref name="configuration"/> location.
		/// </remarks>
		public GuidancePackage(string configuration)
		{
			InitializeServices();
			XmlTextReader reader = new XmlTextReader(configuration);
			try
			{
				Initialize(reader);
			}
			finally
			{
				reader.Close();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GuidancePackage"/> class with a 
		/// reader pointing to the configuration.
		/// </summary>
		/// <param name="configuration">The reader to load configuration from.</param>
		/// <remarks>
		/// External included files will be resolved as relative paths based on the 
		/// <paramref name="configuration"/> reader <see cref="XmlReader.BaseURI"/> property.
		/// </remarks>
		public GuidancePackage(XmlReader configuration)
		{
			InitializeServices();
			Initialize(configuration);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GuidancePackage"/> class with a 
		/// configuration model, ready for use.
		/// </summary>
		/// <param name="configuration">The configuration for the package.</param>
		/// <remarks>To let the contained components publish additional services, use 
		/// the overload that is receiving the <c>exposeServiceContainerService</c> flag.</remarks>
		public GuidancePackage(Configuration.GuidancePackage configuration)
			: this(configuration, String.Empty)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GuidancePackage"/> class with a 
		/// configuration model, ready for use.
		/// </summary>
		/// <param name="configuration">The configuration for the package.</param>
		/// <param name="basePath">Location to use as the base folder to resolve types used by the package. 
		/// Can be <see langword="null"/> or an empty string to use the <see cref="RecipeManager"/> installation folder.</param>
		/// <remarks>
		/// Types used by the package configuration are resolved relative to the framework installation only.
		/// Use the overload receiving the base path to change this behavior.
		/// </remarks>
		public GuidancePackage(Configuration.GuidancePackage configuration, string basePath)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}
			InitializeServices();

			// We need to serialize it and reprocess, to ensure validity and indexing.
			using (Stream mem = new MemoryStream())
			{
				// We avoid the perf. cost of generating a runtime serialization assembly
				// by using a design-time generated one that knows how to deal with out config.
				GuidancePackageSerializer ser = new GuidancePackageSerializer();
				ser.Serialize(mem, configuration);

				// Reinitialize with the other overload.
				mem.Seek(0, SeekOrigin.Begin);
				Initialize(new XmlTextReader(mem));
			}

			// Force Path validation if non-empty.
			if (basePath != null && basePath.Length > 0)
			{
				this.basePath = Path.GetDirectoryName(basePath);
			}
		}

		#endregion Constructors

        #region Events

        /// <summary>
        /// Occurs just after executing a recipe.
        /// </summary>
        internal event RecipeEventHandler AfterRecipeExecution;

        #endregion

        #region Properties

        SourceSwitch tracing;

		/// <summary>
		/// SourceSwitch for this Guidance Package
		/// </summary>
		public SourceSwitch SourceSwitch
		{
			get { return tracing; }
		}

		SourceLevels tracingLevel = SourceLevels.Off;

		/// <summary>
		/// Current level of tracing for this GuidancePackage
		/// </summary>
		private SourceLevels SourceLevels
		{
			get { return tracingLevel; }
			set { tracingLevel = value; }
		}

		int sortPriority = 0;
		/// <summary>
		/// Sort priority used in the Add New dialog box for GuidancePackages
		/// </summary>
		public int SortPriority
		{
			get { return sortPriority; }
			set { sortPriority = value; }
		}

		Configuration.GuidancePackage config;

		/// <summary>
		/// Gets the current package configuration.
		/// </summary>
		public Configuration.GuidancePackage Configuration
		{
			get { return config; }
		}

		/// <summary>
		/// Returns the installation directory of this Guidance Package
		/// </summary>
		public string BasePath
		{
			get { return this.basePath; }
		}

		#endregion Properties

		#region Initialization

		private void InitializeServices()
		{
			AddService(typeof(IConfigurationService), this);
			AddService(typeof(IValueInfoService), this);
			AddService(typeof(IExecutionService), this);
		}

		/// <summary>
		/// Initializes the package with a new configuration.
		/// </summary>
		/// <param name="configuration">The reader to load configuration from.</param>
		/// <remarks>
		/// <para>External included files will be resolved as relative paths based on the 
		/// <paramref name="configuration"/> reader <see cref="XmlReader.BaseURI"/> property.
		/// </para>
		/// Another overload is provided for using a custom provided resolver.
		/// </remarks>
		private void Initialize(XmlReader configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}

			if (basePath.Length == 0)
			{
				basePath = GetBasePath(configuration);
			}

			this.config = ReadConfiguration(configuration);
			tracing = new SourceSwitch(Configuration.Caption);
			tracing.Level = SourceLevels.Off;
			tracingLevel = (SourceLevels)Enum.Parse(typeof(SourceLevels), Configuration.SourceLevels.ToString());
			this.sortPriority = Configuration.SortPriority;
		}

		/// <summary>
		/// Adds the recipe reference service.
		/// </summary>
		protected override void OnSited()
		{
			base.OnSited();
			// Hook the service that will resolve relative to the BaseURI location determined 
			// at initialization time from the XmlReader or passed by the user. 
			// Chain it with the parent persistence provider.
			AddService(typeof(ITypeResolutionService), new TypeResolutionService(
				basePath, GetService<ITypeResolutionService>()));

			// Add the recipe reference service, which depends on the IPersistenceService being present.
			// IMPORTANT: The IAssetReferenceService must be sited AFTER ITypeResolutionService becuase
			// some References requeire the ITypeResolutionService of the Guidance package
			AddService(typeof(IAssetReferenceService), new AssetReferenceService());
		}

		#endregion Initialize

		#region Execution

		private ExecutionResult Execute(string recipe, IAssetReference reference, IDictionary arguments)
		{
			if (recipe == null)
			{
				throw new ArgumentNullException("recipe");
			}
			ThrowIfAlreadyExecutingRecipe();
			Config.Recipe config = ThrowIfRecipeNotConfigured(recipe);

			ReferenceService referenceService = null;
			try
			{
				string appliesTo = GetReferenceAppliesToOrErrorMessage(reference);
				this.TraceInformation(Properties.Resources.Recipe_StartingExecution, recipe,
					reference != null ? String.Format(Properties.Resources.Recipe_ReferenceApplied, appliesTo) : "");

				// This "parent" loader service is the one we created in OnSited that 
				// wraps the one provided by the recipe manager itself, and adds resolution 
				// relative to the package assembly location.
				ITypeResolutionService loader = GetService<ITypeResolutionService>(true);

				// Create the recipe from the configuration.
				currentRecipe = new Recipe(config);
				currentReference = reference;

				// Setup the alias resolution "gateway" loader service. 
				// This resolves aliases defined for the recipe only.
				// This service will go away together with the recipe.
				currentRecipe.AddService(typeof(ITypeResolutionService), new AliasResolutionService(
					config.TypeAliasesByName, loader));

				bool isPersisted;
				IPersistenceService persistenceService;
				arguments = LoadPersitedState(reference, arguments, out isPersisted, out persistenceService);

				// Construct the dictionary service. We don't pass the arguments at this 
				// point as we want to go over the process of setting each in turn, so they are validated.
				DictionaryService dictionarysvc = new DictionaryService(null);
				AddService(typeof(IDictionaryService), dictionarysvc);
				// Expose the IComponentChangeService implementation too.
				AddService(typeof(IComponentChangeService), dictionarysvc);

				if (arguments != null && arguments.Count > 0)
				{
					bool shouldUpdateState = InitializeDictionaryService(arguments, dictionarysvc);

					// Persist changes if appropriate. 
					if (isPersisted && shouldUpdateState)
					{
						persistenceService.SaveState(Configuration.Name, reference, arguments);
					}
				}

				// Construct the dictionary service, passing the persisted state (or null) as well 
				// as the arguments configuration.

				// Site and execute the recipe.
				Add(currentRecipe);
				referenceService = new ReferenceService();
				currentRecipe.AddService(typeof(IReferenceService), referenceService);

				bool allowsuspend = (reference != null && reference is IBoundAssetReference) ||
					(arguments != null);
				EnsureInitializeMetadataForCurrentRecipe();
				ExecutionResult result = currentRecipe.Execute(allowsuspend);
				this.TraceInformation(Properties.Resources.Recipe_ExecutionResult, result);

				if (result == ExecutionResult.Finish)
				{
					// If recipe is not recurrent and it's a bound reference, remove it.
					if (!currentRecipe.Configuration.Recurrent && reference is IBoundAssetReference)
					{
						IAssetReferenceService refsvc = GetService<IAssetReferenceService>(true);
						refsvc.Remove(reference);
						// Remove the persisted state.
						persistenceService.RemoveState(Configuration.Name, reference);
					}
                    // Fire the AfterRecipeExecution event
                    // note this will happen only when the recipe is finished (not suspend nor cancelled)
                    if (AfterRecipeExecution != null)
                    {
                        this.AfterRecipeExecution(this, new RecipeEventArgs(config, isExecutingRecipeFromTemplate));
                    }
                }

				return result;
			}
			finally
			{
				#region Cleanup

				if (referenceService != null)
				{
					currentRecipe.RemoveService(typeof(IReferenceService));
				}

				// Remove recipe from container.
				Remove(currentRecipe);
				// Remove services we added.
				RemoveService(typeof(IDictionaryService));
				RemoveService(typeof(IComponentChangeService));
				// Dispose and clean current variables.
				if (currentRecipe != null)
				{
					currentRecipe.Dispose();
					currentRecipe = null;
				}
				currentReference = null;

				#endregion Cleanup
				// Write a separator on the trace.
				this.TraceInformation(new string('-', 150));
			}
		}

		private bool InitializeDictionaryService(IDictionary arguments, DictionaryService dictionarysvc)
		{
			// Build the new dictionary by trying to set each value in the initial state.
			// Must copy keys because we'll be modifying the dictionary (can't iterate with foreach).
			object[] keys = new object[arguments.Count];
			arguments.Keys.CopyTo(keys, 0);
			bool updatestate = false;
			foreach (object key in keys)
			{
				try
				{
					dictionarysvc.SetValue(key, arguments[key]);
				}
				catch (ArgumentDoesntExistException)
				{
					// Don't remove arguments that are not defined. They may be 
					// used for purposes other than recipe execution, i.e. template 
					// expansion and parameter replacements. Set them via internal 
					// mechanism so they make it into the dictionary.
					dictionarysvc.State[key] = arguments[key];
				}
				catch (ArgumentNullException)
				{
					this.TraceWarning(Properties.Resources.GuidancePackage_InvalidStateKey);
					arguments.Remove(key);
					updatestate = true;
				}
				catch (ArgumentException)
				{
					this.TraceWarning(String.Format(
						CultureInfo.CurrentCulture,
						Properties.Resources.GuidancePackage_InvalidStateValue,
						key));
					arguments.Remove(key);
					updatestate = true;
				}
			}
			return updatestate;
		}

		private IDictionary LoadPersitedState(IAssetReference reference, IDictionary arguments, out bool isPersisted, out IPersistenceService persistenceService)
		{
			isPersisted = false;
			persistenceService = GetService<IPersistenceService>();
			if (persistenceService == null)
			{
				this.TraceWarning(Properties.Resources.Recipe_NoPersistence);
			}
			// Only retrieve the persisted values if there's a persistence service, 
			// an asset reference and we have not received a dictionary that overrides it.
			if (persistenceService != null && reference != null && arguments == null)
			{
				// Reload state. May return null. 
				arguments = persistenceService.LoadState(Configuration.Name, reference);
				if (arguments == null)
				{
					this.TraceInformation(Properties.Resources.Recipe_NoSavedState);
				}
				else
				{
					isPersisted = true;
					this.TraceInformation(Properties.Resources.Recipe_ArgumentsSaved, arguments.Count);
				}
			}
			return arguments;
		}

		private static string GetReferenceAppliesToOrErrorMessage(IAssetReference reference)
		{
			string appliesTo = null;
			try
			{
				if (reference != null)
				{
					appliesTo = reference.AppliesTo;
				}
			}
			catch (Exception e)
			{
				TraceUtil.GaxTraceSource.TraceInformation(e.Message);
				appliesTo = Properties.Resources.Reference_AppliesToThrew;
			}
			return appliesTo;
		}

		private Config.Recipe ThrowIfRecipeNotConfigured(string recipe)
		{
			if (this.Configuration.Contains(recipe) == false)
			{
				throw new ArgumentException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.GuidancePackage_RecipeNotExists,
					recipe, Configuration.Caption));
			}
			return this.Configuration[recipe];
		}

		private void ThrowIfAlreadyExecutingRecipe()
		{
			if (currentRecipe != null)
			{
				throw new InvalidOperationException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.GuidancePackage_AlreadyExecutingRecipe,
					currentRecipe.Configuration == null ? null :
					currentRecipe.Configuration.Name));
			}
		}

		/// <summary>
		/// Sets the TraceSwitch level of the GuidancePackage to the corresponding level specified in the settings
		/// </summary>
		public void TurnOnOutput()
		{
			SourceSwitch.Level = this.SourceLevels;
		}

		/// <summary>
		/// Sets the TraceSwitch level of the Guidance Package to Off
		/// </summary>
		public void TurnOffOutput()
		{
			SourceSwitch.Level = SourceLevels.Off;
		}

		/// <summary>
		/// Executes a recipe with no persistence associated.
		/// </summary>
		/// <param name="recipe">Name of the recipe to execute.</param>
		/// <exception cref="InvalidOperationException">A recipe is already being executed.</exception>
		/// <exception cref="ArgumentException">The recipe does not exist in the package configuration.</exception>
		/// <returns>The result of the execution.</returns>
		public ExecutionResult Execute(string recipe)
		{
			return Execute(recipe, null, null);
		}

		/// <summary>
		/// Executes a recipe using the 
		/// received dictionary as the initial state for the execution.
		/// </summary>
		/// <param name="recipe">Name of the recipe to execute.</param>
		/// <param name="arguments">The initial state to use for the execution.</param>
		/// <exception cref="InvalidOperationException">A recipe is already being executed.</exception>
		/// <exception cref="ArgumentException">The recipe does not exist in the package configuration.</exception>
		/// <returns>The result of the execution.</returns>
		public ExecutionResult Execute(string recipe, IDictionary arguments)
		{
			return Execute(recipe, null, arguments);
		}

        bool isExecutingRecipeFromTemplate = false;

        /// <summary>
        /// Executes a recipe using the 
        /// received dictionary as the initial state for the execution.
        /// </summary>
        /// <param name="recipe">Name of the recipe to execute.</param>
        /// <param name="arguments">The initial state to use for the execution.</param>
        /// <exception cref="InvalidOperationException">A recipe is already being executed.</exception>
        /// <exception cref="ArgumentException">The recipe does not exist in the package configuration.</exception>
        /// <returns>The result of the execution.</returns>
        /// <remarks>This overload exists in order to determine when a recipe in executed from a template and expose this fact
        /// through the IsExecutingFromTemplate property which can be queried by consumers of the AfterRecipeExecution event</remarks>
        public ExecutionResult ExecuteFromTemplate(string recipe, IDictionary arguments)
		{
            isExecutingRecipeFromTemplate = true;
			ExecutionResult result = Execute(recipe, null, arguments);
            isExecutingRecipeFromTemplate = false;
            return result;
		}


		/// <summary>
		/// Executes the recipe pointed by the <see cref="IAssetReference.AssetName"/>, using the 
		/// reference to restore any state that may have been saved.
		/// </summary>
		/// <param name="reference">The reference that is used to identify the recipe and any
		/// state associated with a previous execution.
		/// </param>
		/// <exception cref="InvalidOperationException">A recipe is already being executed.</exception>
		/// <exception cref="ArgumentException">The recipe does not exist in the package configuration.</exception>
		/// <returns>The result of the execution.</returns>
		public ExecutionResult Execute(IAssetReference reference)
		{
			return Execute(reference.AssetName, reference, null);
		}

		/// <summary>
		/// Executes the recipe pointed by the <see cref="IAssetReference.AssetName"/>, using the 
		/// received dictionary as the initial state for the execution.
		/// </summary>
		/// <param name="reference">The reference that is used to identify the recipe and any
		/// state associated with a previous execution.
		/// </param>
		/// <param name="arguments">The initial state to use for the execution.</param>
		/// <exception cref="InvalidOperationException">A recipe is already being executed.</exception>
		/// <exception cref="ArgumentException">The recipe does not exist in the package configuration.</exception>
		/// <returns>The result of the execution.</returns>
		public ExecutionResult Execute(IAssetReference reference, IDictionary arguments)
		{
			return Execute(reference.AssetName, reference, arguments);
		}

		#endregion IGuidancePackageService Members

		#region IConfigurationService Members

		Config.GuidancePackage IConfigurationService.CurrentPackage
		{
			get { return Configuration; }
		}

		Config.Recipe IConfigurationService.CurrentRecipe
		{
			get { return currentRecipe == null ? null : currentRecipe.Configuration; }
		}

		object IConfigurationService.CurrentGatheringServiceData
		{
			get
			{
				return currentRecipe == null ? null :
					currentRecipe.Configuration == null ? null :
					currentRecipe.Configuration.GatheringServiceData;
			}
		}

		string IConfigurationService.BasePath
		{
			get { return basePath; }
		}

		#endregion IConfigurationService Members

		#region IValueInfoService members

		ValueInfo IValueInfoService.GetInfo(string argumentName)
		{
			ThrowIfNoRecipeExecuting();
			ThrowIfArgumentNotDefined(argumentName);
			EnsureInitializeMetadataForCurrentRecipe();
			return argumentsMetaDataByRecipe[currentRecipe.Configuration.Name][argumentName];
		}

		private void EnsureInitializeMetadataForCurrentRecipe()
		{
			if (!argumentsMetaDataByRecipe.ContainsKey(currentRecipe.Configuration.Name))
			{
				Dictionary<string, ValueInfo> infos;
				if (currentRecipe.Configuration.Arguments == null)
				{
					infos = new Dictionary<string, ValueInfo>(0);
				}
				else
				{
					infos = new Dictionary<string, ValueInfo>(currentRecipe.Configuration.Arguments.Length);
					foreach (Config.Argument argument in currentRecipe.Configuration.Arguments)
					{
						ITypeResolutionService resolution = currentRecipe.GetService<ITypeResolutionService>(true);
						Type argumentType = resolution.GetType(argument.Type, true);
						TypeConverter converter = null;

						if (argument.Converter != null)
						{
							converter = CreateAndConfigureConverter(argument, resolution, converter);
						}
						else
						{
							converter = GetBuiltInConverter(argumentType, converter);
						}

						infos.Add(argument.Name, new ValueInfo(argument.Name, argument.Required, argumentType, converter));
					}
				}

				argumentsMetaDataByRecipe.Add(currentRecipe.Configuration.Name, infos);
			}
		}

		private static TypeConverter GetBuiltInConverter(Type argtype, TypeConverter converter)
		{
			TypeConverter cinstance = TypeDescriptor.GetConverter(argtype);
			if (argtype.IsEnum)
			{
				converter = new EnumerationConverter(argtype);
			}
			else if (argtype.IsCOMObject ||
					Attribute.GetCustomAttribute(argtype, typeof(ComImportAttribute), true) != null)
			{
				converter = new ComObjectConverter(argtype);
			}
			else if (cinstance != null && cinstance.GetType() != typeof(ComponentConverter))
			{
				// Only use converters that are not the generic ComponentConverter.
				converter = cinstance;
			}
			return converter;
		}

		private static TypeConverter CreateAndConfigureConverter(Config.Argument argument, ITypeResolutionService resolution, TypeConverter converter)
		{
			converter = Activator.CreateInstance(resolution.GetType(argument.Converter.Type, true)) as TypeConverter;
			if (converter == null)
			{
				// Couldn't do implicit cast to TypeConverter.
				throw new ArgumentException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.General_TypeIsNotAssignable,
					converter, typeof(TypeConverter)));
			}
			// Check if converter is configurable.
			if (converter is IAttributesConfigurable)
			{
				Recipe.Configure((IAttributesConfigurable)converter, argument.Converter.AnyAttr);
			}
			return converter;
		}

		private void ThrowIfNoRecipeExecuting()
		{
			if (currentRecipe == null)
			{
				throw new InvalidOperationException(Properties.Resources.Recipe_NoRecipeExecuting);
			}
		}

		private void ThrowIfArgumentNotDefined(string argumentName)
		{
			if (!this.currentRecipe.Configuration.ArgumentsByName.ContainsKey(argumentName))
			{
				throw new ArgumentDoesntExistException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.Recipe_ArgumentUndefined,
					argumentName));
			}
		}

		string IValueInfoService.ComponentName
		{
			get { return ((IConfigurationService)this).CurrentRecipe.Caption; }
		}

		#endregion

		#region Miscelaneous Members

		/// <summary>
		/// Reads and validates the configuration for a package.
		/// </summary>
		/// <param name="configFile">The file to read the configuration from.</param>
		/// <returns>The loaded in-memory representation of the configuration.</returns>
		public static Configuration.GuidancePackage ReadConfiguration(string configFile)
		{
			if (!File.Exists(configFile))
			{
				throw new FileNotFoundException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.GuidancePackage_ConfigurationNotFound,
					configFile));
			}

			XmlTextReader reader = new XmlTextReader(configFile);
			try
			{
				return ReadConfiguration(reader);
			}
			finally
			{
				reader.Close();
			}
		}

		/// <summary>
		/// Reads and validates the configuration for a package.
		/// </summary>
		/// <param name="configReader">The reader containing the configuration.</param>
		/// <returns>The loaded in-memory representation of the configuration.</returns>
		public static Configuration.GuidancePackage ReadConfiguration(XmlReader configReader)
		{
			configReader = ResolveXInclude(configReader);

			#region Initialize the reader and schema

			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ValidationType = ValidationType.Schema;
			settings.ConformanceLevel = ConformanceLevel.Auto;

			// Pull the XSD from the config assembly (it's an embedded resource).
			using (Stream xsdstream = typeof(Config.GuidancePackage).Assembly.GetManifestResourceStream(
				Config.GuidancePackage.SchemaResourceName))
			{
				Debug.Assert(xsdstream != null, "XSD not embedded in config assembly");

				// If the schema is not valid (we must check that at design-time), this will throw.
				// We never call close on the stream as it's an unmanaged resource stream. Closing 
				// it will render it unusable for the rest of the life of the AppDomain.
				XmlSchema xsd = XmlSchema.Read(xsdstream, null);
				settings.Schemas.Add(xsd);
			}

			XmlReader xr = XmlReader.Create(configReader, settings);

			#endregion Initialize the reader and schema

			GuidancePackageReader reader = new GuidancePackageReader();
			reader.RecipeDeserialized += delegate(Config.Recipe recipe) { Config.RecipeValidator.Validate(recipe); };

			try
			{
				return (Configuration.GuidancePackage)new GuidancePackageSerializer(reader).Deserialize(xr);
			}
			catch (Exception ex)
			{
				// Real serialization exceptions are in the inner exception.
				throw new System.Configuration.ConfigurationException(
					Properties.Resources.GuidancePackage_CantLoadConfig,
					ex.InnerException != null ? ex.InnerException : ex);
			}
		}

		private static XmlReader ResolveXInclude(XmlReader configReader)
		{
			// Wrap reader in including reader to enable XInclude.
			if (!(configReader is XIncludingReader))
			{
				// Temporarily render included file because of 
				// an error in deserialization otherwise.
				MemoryStream mem = new MemoryStream();
				XIncludingReader xir = new XIncludingReader(configReader);
				XmlWriter writer = XmlWriter.Create(mem);
				writer.WriteNode(xir, false);
				writer.Flush();

				mem.Position = 0;
				return new FixedBaseURIWrappingReader(XmlReader.Create(mem), configReader.BaseURI);
			}

			return configReader;
		}

		/// <summary>
		/// Retrieves the <see cref="Config.AttributeNames.Name"/> of 
		/// the <see cref="Config.ElementNames.GuidancePackage"/> element.
		/// </summary>
		/// <param name="reader">The reader to read the name from.</param>
		/// <returns>The name of the package, or <see cref="String.Empty"/> 
		/// if the reader is not positioned in a <see cref="Config.ElementNames.GuidancePackage"/> 
		/// element.</returns>
		public static string GetPackageName(XmlReader reader)
		{
			if (reader.ReadState == System.Xml.ReadState.Initial) reader.Read();

			// Position on the element.
			if (reader.NodeType != XmlNodeType.Element) reader.MoveToContent();

			if (reader.LocalName != Config.ElementNames.GuidancePackage ||
				reader.NamespaceURI != Config.SchemaInfo.PackageNamespace)
			{
				return String.Empty;
			}

			// Return the Name attribute.
			return reader.GetAttribute(Config.AttributeNames.Name);
		}

		/// <summary>
		/// Gets a string representation of the current <see cref="Configuration"/>
		/// </summary>
		/// <returns>A string representation of the package.</returns>
		public override string ToString()
		{
			if (Configuration == null)
			{
				return base.ToString();
			}

			return "GuidancePackage(" + Configuration.Caption + ")";
		}

		#endregion Miscelaneous Members

		private string GetBasePath(XmlReader reader)
		{
			string baseuri;

			// If we have a BaseURI, we'll use that one. Otherwise, default to framework 
			// location, after a warning.
			if (reader.BaseURI.Length == 0)
			{
				this.TraceWarning(Properties.Resources.GuidancePackage_NoBaseURI);
				// UNDONE: change when we add GAC support.
				baseuri = typeof(RecipeManager).Assembly.CodeBase;
			}
			else
			{
				baseuri = reader.BaseURI;
			}

			Uri baselocation = new CompatibleUri(baseuri);
			if (!baselocation.IsFile)
			{
				throw new System.Configuration.ConfigurationException(
					Properties.Resources.GuidancePackage_UriNoFile);
			}

			return Path.GetDirectoryName(baselocation.LocalPath);
		}

		[Serializable]
		private class ArgumentDoesntExistException : ArgumentException
		{
			public ArgumentDoesntExistException(string message) : base(message) { }

			protected ArgumentDoesntExistException(SerializationInfo info, StreamingContext context)
				: base(info, context) { }
		}
	}
}