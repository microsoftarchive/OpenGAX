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
using System.Text;
using System.Collections.Specialized;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.Design;
using Microsoft.Practices.ComponentModel;
using System.Globalization;

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Represents the abstract base class for actions that can receive configuration data 
	/// either as input arguments or attributes.
	/// </summary>
	[ServiceDependency(typeof(ITypeResolutionService))]
	public abstract class ConfigurableAction : Action, Microsoft.Practices.Common.IAttributesConfigurable
	{
		#region IAttributesConfigurable Members

		/// <summary>
		/// Sets the properties of the current action using the values provided, if possible.
		/// </summary>
		/// <param name="attributes">A <see cref="StringDictionary"/> containing the set of attributes 
		/// to use to set the properties of the action.</param>
		public virtual void Configure(StringDictionary attributes)
		{
			PropertyDescriptorCollection props;
			ISite mysite = this.Site;
			try
			{
				// Must remove the site temporarily because 
				// otherwise the TypeDescriptor doesn't work properly.
				this.Site = null;
				props = TypeDescriptor.GetProperties(this);
			}
			finally
			{
				this.Site = mysite;
			}

			foreach (string key in attributes.Keys)
			{
				PropertyDescriptor prop = props[key];
				if (prop == null || prop.IsReadOnly)
				{
					continue;
				}
				TypeConverter converter = null;
				TypeConverterAttribute convattr = (TypeConverterAttribute)prop.Attributes[typeof(TypeConverterAttribute)];
				if (convattr != null && !string.IsNullOrEmpty(convattr.ConverterTypeName))
				{
					ITypeResolutionService resolutionService = (ITypeResolutionService)
						ServiceHelper.GetService(this, typeof(ITypeResolutionService));
					Type type = resolutionService.GetType(convattr.ConverterTypeName, true);
					converter = (TypeConverter)Activator.CreateInstance(type);
				}
				else
				{
					converter = TypeDescriptor.GetConverter(prop.PropertyType);
				}
				object value;
				try
				{
					// Perform minimum conversion if necessary.
					if (!prop.PropertyType.IsAssignableFrom(typeof(string)))
					{
						value = converter.ConvertFromString(new CustomDescriptor(this, prop), attributes[key]);
					}
					else
					{
						value = attributes[key];
					}
					prop.SetValue(this, value);
				}
				catch (TargetInvocationException e)
				{
					throw e.InnerException;
				}
			}
		}

		#endregion

		#region Context

		/// <summary>
		/// Provides a way to get the services to a custom converter
		/// when it is not the string converter
		/// </summary>
		private class CustomDescriptor : ITypeDescriptorContext
		{
			PropertyDescriptor descriptor;
			ConfigurableAction action;

			public CustomDescriptor(ConfigurableAction action, PropertyDescriptor descriptor)
			{
				this.action = action;
				this.descriptor = descriptor;
			}

			#region ITypeDescriptorContext Members

			public IContainer Container
			{
				get { return action.Site.Container; }
			}

			public object Instance
			{
				get { return action; }
			}

			public void OnComponentChanged()
			{
			}

			public bool OnComponentChanging()
			{
				return true;
			}

			public PropertyDescriptor PropertyDescriptor
			{
				get { return descriptor; }
			}

			#endregion

			#region IServiceProvider Members

			public object GetService(Type serviceType)
			{
				return action.GetService(serviceType);
			}

			#endregion
		}

		#endregion
	}
}
