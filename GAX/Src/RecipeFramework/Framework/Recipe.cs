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
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Text;

using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using Config = Microsoft.Practices.RecipeFramework.Configuration;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.Common;
using System.Reflection;
using System.Xml;
using Microsoft.Practices.RecipeFramework.Configuration;
using System.Collections.Generic;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Performs recipe execution based on configuration received at construction time.
	/// </summary>
	/// <remarks>
	/// The recipe instance is not thread-safe.
	/// </remarks>
	[ServiceDependency(typeof(ITypeResolutionService))]
	[ServiceDependency(typeof(IConfigurationService))]
	[ServiceDependency(typeof(IDictionaryService))]
	[ServiceDependency(typeof(IComponentChangeService))]
	[ServiceDependency(typeof(IValueInfoService))]
	internal sealed class Recipe : Microsoft.Practices.ComponentModel.ServiceContainer,
		IActionCoordinationService, IActionExecutionService
	{
		#region Fields & Ctor

		bool forbidChangeService = false;
		IDictionary monitoredArguments;
		Dictionary<string, IAction> actionInstances;
		// Stack for undo.
		string currentAction;
		List<IAction> executedActions = new List<IAction>();
		List<string> executedNameActions = new List<string>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Recipe"/> class, 
		/// with the associated configuration.
		/// </summary>
		/// <param name="configuration">The configuration of the recipe.</param>
		public Recipe(Configuration.Recipe configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}
			this.recipeConfig = configuration;
		}

		#endregion Fields & Ctor

		/// <summary>
		/// Disables <see cref="IComponentChangeService"/> depending on the processing stage.
		/// </summary>
		protected override object GetService(Type serviceType)
		{
			if (forbidChangeService && serviceType == typeof(IComponentChangeService))
			{
				return null;
			}

			object serviceInstance = base.GetService(serviceType);

			if (serviceInstance == null)
			{
				if (serviceType == typeof(IActionExecutionService))
				{
					// Provide default execution service.
					return (IActionExecutionService)this;
				}
				else if (serviceType == typeof(IActionCoordinationService))
				{
					// Provide default coordination service.
					return (IActionCoordinationService)this;
				}
			}

			return serviceInstance;
		}

		/// <summary>
		/// Exposes the recipe configuration.
		/// </summary>
		public Configuration.Recipe Configuration
		{
			get { return recipeConfig; }
		} Configuration.Recipe recipeConfig;

		/// <summary>
		/// Executes the recipe, processing the configuration received at construction time.
		/// </summary>
		/// <param name="allowSuspend">Specifies whether the recipe can be suspended.</param>
		/// <remarks>
		/// Only when the recipe is executed completely (that is, it's not suspended or cancelled), the 
		/// return value will be <see langword="true"/>.
		/// </remarks>
		/// <returns>Whether the recipe finished executing succesfully.</returns>
		public ExecutionResult Execute(bool allowSuspend)
		{
			ITypeResolutionService resolution = GetService<ITypeResolutionService>(true);
			IDictionaryService arguments = GetService<IDictionaryService>(true);
			IDictionaryService readonlyargs = new ReadOnlyDictionaryService(arguments);
			IDictionary providers = LoadProviders(resolution);

			// Setup custom action execution service if specified.
			if (Configuration.Actions != null)
			{
				if (!String.IsNullOrEmpty(Configuration.Actions.ExecutionServiceType))
				{
					AddService(typeof(IActionExecutionService),
						GetInstance<IActionExecutionService>(resolution, Configuration.Actions.ExecutionServiceType));
				}
				// Setup custom action coordinator service if specified.
				if (!String.IsNullOrEmpty(Configuration.Actions.CoordinatorServiceType))
				{
					AddService(typeof(IActionCoordinationService),
						GetInstance<IActionCoordinationService>(resolution, Configuration.Actions.CoordinatorServiceType));
				}
			}

			try
			{
				CallProviders(providers, readonlyargs, arguments, true);
                ThrowIfValueTypeArgumentIsOptionalNotNullable();
                IConfigurationService configservice = GetService<IConfigurationService>(true);
				object wizardconfig = configservice.CurrentGatheringServiceData;

				// Check if we have null values.
				ThrowIfValueTypeArgumentIsOptionalNotNullable();
				RecipeGatheringServiceData gatheringConfig = (RecipeGatheringServiceData)wizardconfig;

				// Collect if we have both a wizard and a gathering strategy.
				if (wizardconfig != null)
				{
					IValueGatheringService gathering = GetValueGatheringService(resolution, gatheringConfig);
					if (gathering == null)
					{
						throw new RecipeExecutionException(this.Configuration.Name,
							Properties.Resources.Recipe_ArgumentGatheringRequired);
					}

					ExecutionResult result = gathering.Execute(gatheringConfig.Any, allowSuspend);
					if (result == ExecutionResult.Suspend && !allowSuspend)
					{
						throw new InvalidOperationException(Properties.Resources.Recipe_CantSuspendRecipe);
					}
					if (result == ExecutionResult.Finish)
					{
						CallProviders(providers, readonlyargs, arguments, false);
						ThrowIfRequiredArgumentsAreNull(arguments);
						ExecuteActions(readonlyargs, arguments, resolution);
					}
					return result;
				}
				else
				{
					CallProviders(providers, readonlyargs, arguments, false);
					ThrowIfRequiredArgumentsAreNull(arguments);
					ExecuteActions(readonlyargs, arguments, resolution);
					return ExecutionResult.Finish;
				}
			}
			finally
			{
				UnloadProviders(providers);
			}
		}

		private IValueGatheringService GetValueGatheringService(ITypeResolutionService resolution,
			RecipeGatheringServiceData gatheringConfig)
		{
			IValueGatheringService gathering;

			// A custom value gathering type has been specified, we will use that one instead of using the built-in one
			if (!String.IsNullOrEmpty(gatheringConfig.ServiceType))
			{
				gathering = GetInstance<IValueGatheringService>(resolution, gatheringConfig.ServiceType);

				// Add the service to the current container (just for playing nice)
				this.AddService(typeof(IValueGatheringService), gathering);
			}
			else
			{

				gathering = GetService<IValueGatheringService>();
			}
			return gathering;
		}

		#region Providers loading, unloading and event handling

		IDictionary LoadProviders(ITypeResolutionService resolution)
		{
			if (Configuration.Arguments == null)
			{
				return new System.Collections.Specialized.HybridDictionary(0);
			}

			try
			{
				// At most we will have a provider for each argument.
				IDictionary providers = new System.Collections.Specialized.HybridDictionary(Configuration.Arguments.Length);
				monitoredArguments = new System.Collections.Specialized.HybridDictionary(Configuration.Arguments.Length);
				bool hasmonitors = false;
				IValueInfoService mdservice = GetService<IValueInfoService>(true);

				// Setup monitoring for dependent argument changes.
				foreach (Config.Argument argument in Configuration.Arguments)
				{
					if (argument.ValueProvider != null)
					{
						IValueProvider provider = GetInstance<IValueProvider>(resolution, argument.ValueProvider.Type);

						// Initialize the provider by passing the configuration for the argument it provides values to.
						provider.Initialize(mdservice.GetInfo(argument.Name));
						if (provider is IAttributesConfigurable)
						{
							Configure((IAttributesConfigurable)provider, argument.ValueProvider.AnyAttr);
						}
						providers.Add(argument.Name, provider);
						// Site the provider so it can access all services.
						if (provider is IComponent)
						{
							Add((IComponent)provider);
						}

						// Add to the argument-indexed list of providers monitoring arguments.
						if (argument.ValueProvider.MonitorArgument != null)
						{
							hasmonitors = true;
							foreach (Config.MonitorArgument monitored in argument.ValueProvider.MonitorArgument)
							{
								// Throw if the value provider is monitoring the same argument it's attached to.
								if (monitored.Name == argument.Name)
								{
									throw new System.Configuration.ConfigurationException(String.Format(
										System.Globalization.CultureInfo.CurrentCulture,
										Properties.Resources.Recipe_ArgumentCantMonitorItself,
										argument.ValueProvider.Type,
										argument.Name));
								}

								ArrayList monitoringproviders = (ArrayList)monitoredArguments[monitored.Name];
								if (monitoringproviders == null)
								{
									monitoringproviders = new ArrayList();
									monitoredArguments[monitored.Name] = monitoringproviders;
								}
								monitoringproviders.Add(provider);
							}
						}
					}
				}

				if (providers.Count != 0 && hasmonitors)
				{
					// Attach to change event if monitoring arguments.
					IComponentChangeService changes = GetService<IComponentChangeService>(true);
					changes.ComponentChanged += new ComponentChangedEventHandler(OnArgumentChanged);
				}

				return providers;

			}
			catch (Exception ex)
			{
				throw new ValueProviderException(this.Configuration.Name,
					Properties.Resources.Recipe_ValueProviderLoadFailed, ex);
			}
		}

		void UnloadProviders(IDictionary providers)
		{
			foreach (IValueProvider provider in providers.Values)
			{
				if (provider is IComponent)
				{
					Remove((IComponent)provider);
				}
			}
		}

		void OnArgumentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (e.Component == null)
			{
				throw new ArgumentNullException("Component");
			}
			if (!(sender is IDictionaryService))
			{
				// Ignore change events that don't come from the dictionary service.
				return;
			}

			// Forward call to appropriate providers.
			ArrayList providers = (ArrayList)monitoredArguments[((ValueInfo)e.Component).Name];
			if (providers != null)
			{
				IDictionaryService dictionary = (IDictionaryService)sender;
				using (SetupTemporaryService(typeof(IDictionaryService), new ReadOnlyDictionaryService(dictionary)))
				{
					// Don't let value providers get a hook to the change service (would allow them to monitor manually/change values).
					forbidChangeService = true;
					try
					{
						foreach (IValueProvider provider in providers)
						{
							object newvalue;
							bool mustchange = provider.OnArgumentChanged(
								((ValueInfo)e.Component).Name, e.NewValue,
								dictionary.GetValue(provider.Argument.Name), out newvalue);
							if (mustchange)
							{
								dictionary.SetValue(provider.Argument.Name, newvalue);
							}
						}
					}
					finally
					{
						// Restore change events.
						forbidChangeService = false;
					}
				}
			}
		}

		void CallProviders(IDictionary providers, IDictionaryService readonlyArguments,
			IDictionaryService arguments, bool isBefore)
		{
			if (Configuration.Arguments != null)
			{
				// Expose dictionary service as readonly.
				using (SetupTemporaryService(typeof(IDictionaryService), readonlyArguments))
				{
					// Don't let value providers get a hook to the change service (would allow them to monitor manually/change values).
					forbidChangeService = true;
					try
					{
						foreach (Config.Argument argument in Configuration.Arguments)
						{
							try
							{
								// Provider is called always.
								if (providers[argument.Name] != null)
								{
									object newvalue;
									bool mustchange = false;
									if (isBefore)
									{
										mustchange = ((IValueProvider)providers[argument.Name]).OnBeginRecipe(
											arguments.GetValue(argument.Name), out newvalue);
									}
									else
									{
										mustchange = ((IValueProvider)providers[argument.Name]).OnBeforeActions(
											arguments.GetValue(argument.Name), out newvalue);
									}

									if (mustchange)
									{
										arguments.SetValue(argument.Name, newvalue);
									}
								}
							}
							catch (Exception ex)
							{
								throw new ValueProviderException(this.Configuration.Name, String.Format(
									System.Globalization.CultureInfo.CurrentCulture,
									Properties.Resources.Recipe_ValueProviderException, argument.Name), ex);
							}
						}
					}
					finally
					{
						forbidChangeService = false;
					}
				}
			}
		}

		#endregion Providers loading, unloading and event handling

		#region ExecuteActions

		void ExecuteActions(IDictionaryService readOnlyArguments,
			IDictionaryService arguments, ITypeResolutionService resolution)
		{
			if (!HasActions())
			{
				return;
			}

			IActionCoordinationService coordinator = GetService<IActionCoordinationService>(true);
			LoadActionsFromConfiguration(resolution);

			try
			{
				// Don't let value providers/actions get a hook to the change service
				forbidChangeService = true;
				// Expose dictionary service as readonly to the action.
				using (SetupTemporaryService(typeof(IDictionaryService), readOnlyArguments))
				{
					coordinator.Run(Configuration.Actions.GetIndexedActions(), Configuration.Actions.Any);
				}
			}
			catch (Exception ex)
			{
				UndoExecutedActionsAndRethrow(ex);
			}
			finally
			{
				forbidChangeService = false;
			}

			return;
		}

		private T GetInstance<T>(ITypeResolutionService resolution, string concreteType)
		{
			ReflectionHelper.EnsureAssignableTo(resolution.GetType(concreteType, true), typeof(T));
			return (T)Activator.CreateInstance(resolution.GetType(concreteType, true));
		}

		private bool HasActions()
		{
			Debug.Assert(Configuration != null, "Recipe configuration is null");
			return Configuration.Actions != null &&
				Configuration.Actions.Action != null &&
				Configuration.Actions.Action.Length > 0;
		}

		private void UndoExecutedActionsAndRethrow(Exception ex)
		{
			List<Exception> undoexceptions = new List<Exception>();
			// Undo in reverse order.
			// Expose dictionary service as readonly to the action.
			for (int i = executedActions.Count - 1; i >= 0; i--)
			{
				IAction executedAction = executedActions[i];
				try
				{
					executedAction.Undo();
				}
				catch (Exception undoex)
				{
					// We hide the exception as undo must never cause 
					// additional failures. The accumulation of undo exceptions is 
					// kept in the action exception, however.
					undoexceptions.Add(new UndoActionException(executedNameActions[i], undoex));
				}
			}

			throw new ActionExecutionException(this.Configuration.Name, currentAction, ex, undoexceptions.ToArray());
		}

		private void LoadActionsFromConfiguration(ITypeResolutionService resolution)
		{
			actionInstances = new Dictionary<string, IAction>(Configuration.Actions.Action.Length);
			foreach (Config.Action actionconfig in Configuration.Actions.Action)
			{
				actionInstances.Add(actionconfig.Name,
					GetInstance<IAction>(resolution, actionconfig.Type));
			}
		}

		private void SetupInputs(IAction action, Configuration.Action config,
			IDictionary actions, IDictionaryService arguments)
		{
			if (config.Input != null)
			{
				// Set input properties.
				foreach (Configuration.Input parameter in config.Input)
				{
					object value = null;
					if (parameter.RecipeArgument != null)
					{
						value = arguments.GetValue(parameter.RecipeArgument);
					}
					else if (parameter.ActionOutput != null)
					{
						value = GetActionOutputValue(config, parameter, value);
					}

					// Retrieve the property using the type description also for extensibility.
					PropertyDescriptor inprop = GetComponentProperty(action, parameter.Name);
					if (inprop != null)
					{
						InputAttribute inputattr = (InputAttribute)inprop.Attributes[typeof(InputAttribute)];
						if (inputattr == null)
						{
							throw new NotSupportedException(String.Format(
								System.Globalization.CultureInfo.CurrentCulture,
								Properties.Resources.Recipe_NotMarkedInput,
								parameter.Name, config.Type));
						}
						if (inputattr.Required && (value == null ||
							(value is String && ((String)value).Length == 0)))
						{
							throw new ArgumentNullException(parameter.Name,
								Properties.Resources.General_ArgumentEmpty);
						}
						try
						{
							inprop.SetValue(action, value);
						}
						catch (TargetInvocationException tex)
						{
							// Throw with inner exception which contains the real error.
							throw new NotSupportedException(String.Format(
								System.Globalization.CultureInfo.CurrentCulture,
								Properties.Resources.Recipe_CantSetActionProperty,
								inprop.Name, config.Name), tex.InnerException);
						}
					}
					else
					{
						throw new ActionExecutionException(
							this.Configuration.Name, config.Name,
							String.Format(System.Globalization.CultureInfo.CurrentCulture,
							Properties.Resources.Recipe_ActionPropertyMissing,
							parameter.Name, action.GetType()));
					}
				}
			}
		}

		private static PropertyDescriptor GetComponentProperty(object component, string propertyName)
		{
			PropertyDescriptor inprop;
			using (new UnsiteComponent(component))
			{
				inprop = TypeDescriptor.GetProperties(component).Find(propertyName, false);
			}
			return inprop;
		}

		private object GetActionOutputValue(Configuration.Action config, Configuration.Input parameter, object value)
		{
			string[] actionoutvalues = parameter.ActionOutput.Split(new char[] { '.' });
			object outaction = actionInstances[actionoutvalues[0]];
			if (outaction == null)
			{
				throw new ArgumentException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.Recipe_ActionOutputNull,
					config.Name, parameter.Name, actionoutvalues[0]));
			}
			// Use the type descriptor for the object, so it can be extensible.
			PropertyDescriptor outprop = GetComponentProperty(outaction, actionoutvalues[1]);
			bool HasOutputAttribute = false;
			if (outprop != null)
			{
				HasOutputAttribute = (outprop.Attributes[typeof(OutputAttribute)] != null);
			}
			if (!HasOutputAttribute)
			{
				throw new NotSupportedException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.Recipe_ActionOutputMissing,
					actionoutvalues[1], actionoutvalues[0], config.Name, outaction.GetType()));
			}

			return outprop.GetValue(outaction);
		}

		private void ValidateOutput(IAction action, Configuration.Action config)
		{
			if (config.Output != null)
			{
				foreach (Config.Output parameter in config.Output)
				{
					// Retrieve the property using the type description also for extensibility.
					PropertyDescriptor outprop;
					using (new UnsiteComponent(action))
					{
						outprop = TypeDescriptor.GetProperties(action).Find(parameter.Name, false);
						if (outprop != null && (outprop.Attributes[typeof(OutputAttribute)] == null))
						{
							throw new NotSupportedException(String.Format(
								System.Globalization.CultureInfo.CurrentCulture,
								Properties.Resources.Recipe_ActionOutputInvalid,
								parameter.Name, config.Name, action.GetType()));
						}
					}
					if (outprop == null)
					{
						throw new ActionExecutionException(
							this.Configuration.Name, config.Name,
							String.Format(System.Globalization.CultureInfo.CurrentCulture,
							Properties.Resources.Recipe_ActionPropertyMissing,
							parameter.Name, action.GetType()));
					}
				}
			}
		}

		private void FinalizeSetup(IAction action, Configuration.Action config)
		{
			if (action is IAttributesConfigurable)
			{
				Configure((IAttributesConfigurable)action, config.AnyAttr);
			}

			using (new UnsiteComponent(action))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(action);
				// Check all required input arguments prior to execution.
				// Some may not be included in the configuration file and 
				// may thus not be checked above.
				foreach (PropertyDescriptor property in properties)
				{
					InputAttribute actioninput = (InputAttribute)property.Attributes[typeof(InputAttribute)];
					if (actioninput != null && actioninput.Required)
					{
						object propertyvalue = property.GetValue(action);
						if (propertyvalue == null || (propertyvalue is String &&
							((String)propertyvalue).Length == 0))
						{
							throw new ArgumentNullException(property.Name,
								Properties.Resources.General_ArgumentEmpty);
						}
					}
				}
			}
		}

		#endregion ExecuteActions

		#region Null arguments checks

		void ThrowIfValueTypeArgumentIsOptionalNotNullable()
		{
			if (Configuration.Arguments == null || Configuration.Arguments.Length == 0)
			{
				return;
			}
			IValueInfoService infoSvc = GetService<IValueInfoService>(true);
			ArrayList valueTypeNullables = new ArrayList(Configuration.Arguments.Length);
			foreach (Config.Argument arg in Configuration.Arguments)
			{
				// ValueTypes must be either required or Nullable<T>.
				ValueInfo info = infoSvc.GetInfo(arg.Name);
				if (info.Type.IsValueType && !arg.Required &&
					(!info.Type.IsGenericType ||
					!(info.Type.GetGenericTypeDefinition() == typeof(Nullable<>))))
				{
					valueTypeNullables.Add(arg.Name);
				}
			}

			if (valueTypeNullables.Count != 0)
			{
				throw new RecipeExecutionException(this.Configuration.Name, String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.Recipe_InvalidOptionalValueType,
					String.Join(", ", (string[])valueTypeNullables.ToArray(typeof(string)))));
			}
		}

		void ThrowIfRequiredArgumentsAreNull(IDictionaryService arguments)
		{
			bool requiredAreNull = false;

			if (Configuration.Arguments == null)
			{
				return;
			}

			GetService<IValueInfoService>(true);
			ArrayList missingReqArgs = new ArrayList(Configuration.Arguments.Length);
			foreach (Config.Argument arg in Configuration.Arguments)
			{
				bool isNull = arguments.GetValue(arg.Name) == null;
				if (arg.Required && isNull)
				{
					missingReqArgs.Add(arg.Name);
				}
				requiredAreNull = requiredAreNull || (arg.Required && isNull);
			}

			if (requiredAreNull)
			{
				throw new RecipeExecutionException(this.Configuration.Name, String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.Recipe_MissingRequiredArguments,
					String.Join(", ", (string[])missingReqArgs.ToArray(typeof(string)))));
			}
		}

		static ExecutionResult QueryUser(string message, IUIService uiservice)
		{
			DialogResult result = uiservice.ShowMessage(message, "", MessageBoxButtons.YesNo);
			if (result == DialogResult.Yes)
			{
				return ExecutionResult.Suspend;
			}
			else
			{
				return ExecutionResult.Cancel;
			}
		}

		#endregion Null arguments checks

		/// <summary>
		/// Configures an <see cref="IAttributesConfigurable"/> component with the set of attributes specified.
		/// </summary>
		internal static void Configure(IAttributesConfigurable component, XmlAttribute[] attributes)
		{
			ImmutableKeyStringDictionary values = new ImmutableKeyStringDictionary();
			if (attributes != null)
			{
				foreach (XmlAttribute xattr in attributes)
				{
					values.Add(xattr.Name, xattr.Value);
				}
			}
			component.Configure(values);
		}


		/// <summary>
		/// Removes the site of a component temporarily.
		/// </summary>
		private class UnsiteComponent : IDisposable
		{
			IComponent component;
			ISite site;
			public UnsiteComponent(object component)
			{
				if (component is IComponent)
				{
					this.component = (IComponent)component;
					this.site = this.component.Site;
					this.component.Site = null;
				}
			}

			public void Dispose()
			{
				if (this.component != null)
				{
					this.component.Site = this.site;
				}
			}
		}

		#region Default IActionCoordinator Members

		void IActionCoordinationService.Run(Dictionary<string, Configuration.Action> declaredActions, XmlElement coordinationData)
		{
			IActionExecutionService execution = GetService<IActionExecutionService>(true);

			foreach (Config.Action config in declaredActions.Values)
			{
				execution.Execute(config.Name);
			}
		}

		#endregion

		#region IActionExecutionService Members

		void IActionExecutionService.Execute(string actionName)
		{
			((IActionExecutionService)this).Execute(actionName, new Dictionary<string, object>());
		}

		void IActionExecutionService.Execute(string actionName, Dictionary<string, object> inputValues)
		{
			IDictionaryService combinedValues = new SuplementedDictionaryService(inputValues,
				GetService<IDictionaryService>(true));

			currentAction = actionName;
			IAction action = actionInstances[actionName];
			Config.Action actionConfig = Configuration.Actions[actionName];

			SetupInputs(action, actionConfig, actionInstances, combinedValues);
			ValidateOutput(action, actionConfig);
			// Site the action if it's a component.
			if (action is IComponent)
			{
				Add((IComponent)action, actionName);
			}
			// Pass values if IAttributesConfigurable is implemented, 
			// and do final pass on required input properties.
			FinalizeSetup(action, actionConfig);

			action.Execute();
			executedActions.Add(action);
			executedNameActions.Add(actionName);
		}

		private class SuplementedDictionaryService : IDictionaryService
		{
			IDictionary<string, object> values;
			IDictionaryService service;

			public SuplementedDictionaryService(IDictionary<string, object> values, IDictionaryService service)
			{
				this.values = values;
				this.service = service;
			}

			#region IDictionaryService Members

			public object GetKey(object value)
			{
				foreach (KeyValuePair<string, object> pair in values)
				{
					if (pair.Value == value)
					{
						return pair.Key;
					}
				}

				return service.GetKey(value);
			}

			public object GetValue(object key)
			{
				if (values.ContainsKey((string)key))
				{
					return values[(string)key];
				}
				else
				{
					return service.GetValue(key);
				}
			}

			public void SetValue(object key, object value)
			{
				values[(string)key] = value;
			}

			#endregion
		}

		#endregion
	}
}
