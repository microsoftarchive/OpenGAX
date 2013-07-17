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
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Practices.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.Services
{
	/// <summary>
	/// Implements the dictionary service.
	/// </summary>
	[ComponentModel.ServiceDependency(typeof(IValueInfoService))]
	[System.ComponentModel.DesignerCategory("Code")]
	internal class DictionaryService : SitedComponent, IDictionaryService, IComponentChangeService
	{
        DescriptorContextFlyweight context;

		public DictionaryService(IDictionary initialState)
		{
			if (initialState == null)
			{
				arguments = new System.Collections.Specialized.HybridDictionary();
			}
			else
			{
				arguments = initialState;
			}
		}

		internal IDictionary State
		{
			get { return arguments; }
		} IDictionary arguments;

        protected override void OnSited()
        {
            context = new DescriptorContextFlyweight(this.Site);
        }

		#region IDictionaryService Implementation

		/// <summary>
		/// See <see cref="IDictionaryService.GetKey"/>.
		/// </summary>
		public object GetKey(object value)
		{
			// Value CAN be null.
			if (!State.Contains(value))
			{
				return null;
			}
			else
			{
				foreach (DictionaryEntry entry in State)
				{
					if (entry.Value == value)
					{
						return entry.Key;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// See <see cref="IDictionaryService.GetValue"/>.
		/// </summary>
		public object GetValue(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return State[key];
		}

		/// <summary>
		/// See <see cref="IDictionaryService.SetValue"/>.
		/// </summary>
        /// <exception cref="InvalidOperationException">The service is not sited.</exception>
        /// <exception cref="ArgumentException">Value is invalid.</exception>
		public void SetValue(object key, object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (this.Site == null)
			{
				throw new InvalidOperationException(String.Format(
					CultureInfo.CurrentCulture,
					Properties.Resources.General_ComponentMustBeSited, this));
			}
            IValueInfoService mdservice = (IValueInfoService)
                ComponentModel.ServiceHelper.GetService(this, typeof(IValueInfoService));
			// Will throw if not present.
			ValueInfo mdata = mdservice.GetInfo(key.ToString());
            context.SetData(mdata.Name, mdata.Type);
            if (mdata.Converter != null && value != null)
            {
                bool needsconversion = !ReflectionHelper.IsAssignableTo(mdata.Type, value);
                // We're restrictive and don't allow assignment of COM objects to 
                // arguments that are not interfaces.
                if (needsconversion && value.GetType().IsCOMObject && !mdata.Type.IsInterface)
                {
                    throw new ArgumentException(String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resources.IDictionaryService_InvalidType,
                        value, key, mdata.Type));
                }
                if (needsconversion)
                {
                    // Perform conversion.
                    if (!mdata.Converter.CanConvertFrom(context, value.GetType()))
                    {
                        throw new ArgumentException(String.Format(
                            CultureInfo.CurrentCulture,
                            Properties.Resources.IDictionaryService_InvalidType,
                            value, key, mdata.Type));
                    }
                    // Verify if value is valid and can be converted according to converter.
                    if (!mdata.Converter.IsValid(context, value))
                    {
                        throw new ArgumentException(String.Format(
                            CultureInfo.CurrentCulture,
                            Properties.Resources.IDictionaryService_InvalidValue, key));
                    }
					// Even if the value says that "IsValid" it could be failing performing the conversion
					try
					{
						value = mdata.Converter.ConvertFrom(context, CultureInfo.CurrentCulture, value);
					}
					catch (FormatException)
					{
						throw new ArgumentException(String.Format(
							CultureInfo.CurrentCulture,
							Properties.Resources.IDictionaryService_InvalidValue, key));
					}
					catch (Exception ex)
					{
						if (ex.InnerException != null && 
							(ex.InnerException is FormatException || ex.InnerException is ArgumentException))
						{
							throw new ArgumentException(String.Format(
								CultureInfo.CurrentCulture,
								Properties.Resources.IDictionaryService_InvalidValue, key),
								ex.InnerException);
						}
						else
						{
							throw;
						}
					}
                    // Check again for validity.
                    bool invalidconversion = !ReflectionHelper.IsAssignableTo(mdata.Type, value);
                    // We're restrictive and don't allow assignment of COM objects to 
                    // arguments that are not interfaces.
                    if (invalidconversion && value.GetType().IsCOMObject && !mdata.Type.IsInterface)
                    {
                        throw new ArgumentException(String.Format(
                            CultureInfo.CurrentCulture,
                            Properties.Resources.IDictionaryService_InvalidType,
                            value, key, mdata.Type));
                    }
                    if (invalidconversion)
                    {
                        throw new ArgumentException(String.Format(
                            CultureInfo.CurrentCulture,
                            Properties.Resources.IDictionaryService_InvalidConvertedValue, key));
                    }
                }
            }

			if (ComponentChanging != null)
			{
				ComponentChanging(this, new ComponentChangingEventArgs(
					mdata, null));
			}
			State[key] = value;
			if (ComponentChanged != null)
			{
				ComponentChanged(this, new ComponentChangedEventArgs(
					mdata, null, State[key], value));
			}
		}

		#endregion IDictionaryService Implementation

		#region IComponentChangeService Members

		//Using pragmas because these interface members are required even if they are 
		//not used by our implementation
		/// <summary>
		/// See <see cref="IComponentChangeService.ComponentAdded"/>.
		/// </summary>
		public event ComponentEventHandler ComponentAdded;
		/// <summary>
		/// See <see cref="IComponentChangeService.ComponentAdding"/>.
		/// </summary>
		public event ComponentEventHandler ComponentAdding;
		/// <summary>
		/// See <see cref="IComponentChangeService.ComponentChanged"/>.
		/// </summary>
		public event ComponentChangedEventHandler ComponentChanged;
		/// <summary>
		/// See <see cref="IComponentChangeService.ComponentChanging"/>.
		/// </summary>
		public event ComponentChangingEventHandler ComponentChanging;
		/// <summary>
		/// See <see cref="IComponentChangeService.ComponentRemoved"/>.
		/// </summary>
		public event ComponentEventHandler ComponentRemoved;
		/// <summary>
		/// See <see cref="IComponentChangeService.ComponentRemoving"/>.
		/// </summary>
		public event ComponentEventHandler ComponentRemoving;
		/// <summary>
		/// See <see cref="IComponentChangeService.ComponentRename"/>.
		/// </summary>
		public event ComponentRenameEventHandler ComponentRename;

		public void OnComponentChanged(object component, MemberDescriptor member, object oldValue, object newValue)
		{
			if (ComponentChanged != null && component is ValueInfo)
				ComponentChanged(this, new ComponentChangedEventArgs(component, member, oldValue, newValue));
		}

		public void OnComponentChanging(object component, MemberDescriptor member)
		{
			if (ComponentChanging != null && component is ValueInfo)
				ComponentChanging(this, new ComponentChangingEventArgs(component, member));
		}

		#endregion IComponentChangeService Members

        private class DescriptorContextFlyweight : ITypeDescriptorContext
        {
            string valueName;
            Type valueType;

            public DescriptorContextFlyweight(IServiceProvider provider)
            {
                this.provider = provider;
                this.container = (IContainer)provider.GetService(typeof(IContainer));
            }

            public void SetData(string valueName, Type valueType)
            {
                this.valueName = valueName;
                this.valueType = valueType;
            }

            #region ITypeDescriptorContext Members

            IContainer container;

            public IContainer Container
            {
                get { return container; }
            }

            public object Instance
            {
                get { return container; }
            }

            public void OnComponentChanged()
            {
                throw new NotImplementedException();
            }

            public bool OnComponentChanging()
            {
				throw new NotImplementedException("The method or operation is not implemented.");
            }

            PropertyDescriptor descriptor;

            PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
            {
                get
                {
                    if (descriptor == null && valueType != null && valueName != null)
                    {
                        // Will return a property descriptor where the component type 
                        // will be the recipe itself.
                        descriptor = TypeDescriptor.CreateProperty(
                            container.GetType(), valueName, valueType);
                    }
                    return descriptor;
                }
            }

            #endregion

            #region IServiceProvider Members

            IServiceProvider provider;

            public object GetService(Type serviceType)
            {
                return provider.GetService(serviceType);
            }

            #endregion
        }
    }
}
