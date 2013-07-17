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

#endregion Using directives

namespace Microsoft.Practices.ComponentModel
{
    /// <summary>
    /// Performs service-related activities.
    /// </summary>
    public sealed class ServiceHelper
	{
		#region Private & Internal Members

		private ServiceHelper()
        {
        }

		#endregion Private & Internal Members

		#region Public Members

//		/// <summary>
//		/// Retrieves a typed service from a container.
//		/// </summary>
//		/// <typeparam name="T">The type of service to retrieve.</typeparam>
//		/// <param name="provider">The provider to retrieve the service from.</param>
//		/// <returns>The service instance or <see langword="null"/> if it's not found.</returns>
//		public static T GetService<T>(IServiceProvider provider)
//		{
//			return (T) provider.GetService(typeof(T));
//		}

		/// <summary>
		/// Checks that all declared dependencies for the object are present after 
		/// it has been sited.
		/// </summary>
		/// <param name="component">The component to check dependencies on.</param>
		public static void CheckDependencies(IComponent component)
		{
            if (component == null)
                throw new ArgumentException("component can not be null", "component");

			if (component.Site == null)
			{
				throw new InvalidOperationException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.ServiceHelper_ComponentNotSited, component));
			}

			CheckDependencies(component, component.Site);
		}

		/// <summary>
		/// Checks that all declared dependencies for the object are present based on 
        /// the received provider. The object doesn't need to be an <see cref="IComponent"/>.
		/// </summary>
		/// <param name="component">The component to check dependencies on.</param>
        /// <param name="provider">The provider to use to query for services the component depends on.</param>
		public static void CheckDependencies(object component, IServiceProvider provider)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component",
					Properties.Resources.ServiceHelper_ComponentNull);
			}
			if (provider == null)
			{
				throw new ArgumentNullException("provider",
					Properties.Resources.ServiceHelper_ProviderNull);
			}

			ServiceDependencyAttribute[] deps = (ServiceDependencyAttribute[])
				component.GetType().GetCustomAttributes(typeof(ServiceDependencyAttribute), true);
			if (deps == null || deps.Length == 0)
				return;

			IServiceProvider componentAsProvider = component as IServiceProvider;

			foreach (ServiceDependencyAttribute dep in deps)
			{
				if (provider.GetService(dep.ServiceType) == null && (componentAsProvider == null || componentAsProvider.GetService(dep.ServiceType) == null))
				{
					if (component is SitedComponent)
					{
						((SitedComponent)component).OnMissingServiceDependency(dep.ServiceType);
					}
					else
					{
						throw new ServiceMissingException(dep.ServiceType, component);
					}
				}
			}
		}

		/// <summary>
		/// Tries to retrieve a service, and throws an <see cref="ServiceMissingException"/> 
        /// if it doesn't exist on the <paramref name="component"/> container.
		/// </summary>
        /// <param name="component">The component that requests the service.</param>
		/// <param name="serviceType">The service to retrieve.</param>
		/// <returns>The service instance.</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="component"/> is not sited.</exception>
		/// <exception cref="ServiceMissingException">The service was not found on the component <see cref="IComponent.Site"/>.</exception>
		public static object GetService(IComponent component, Type serviceType)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (component.Site == null)
			{
				throw new InvalidOperationException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.ServiceHelper_ComponentNotSited, component));
			}
			object instance = null;
            if (component is IServiceProvider)
            {
                instance = ((IServiceProvider)component).GetService(serviceType);
            }
            if (instance == null)
            {
                instance = component.Site.GetService(serviceType);
            }
			if (instance == null)
			{
				throw new ServiceMissingException(serviceType, component);
			}
			return instance;
		}

        /// <summary>
        /// Tries to retrieve a service, and throws an <see cref="ServiceMissingException"/> 
        /// if it doesn't exist on the <paramref name="component"/> container.
        /// </summary>
        /// <param name="serviceProvider">The service provider that holds services.</param>
        /// <param name="serviceType">The service to retrieve.</param>
        /// <param name="component">The object that needs the service.</param>
        /// <returns>The service instance.</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="component"/> is not sited.</exception>
        /// <exception cref="ServiceMissingException">The service was not found on the component <see cref="IComponent.Site"/>.</exception>
        public static object GetService(IServiceProvider serviceProvider, Type serviceType, object component)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            object instance = serviceProvider.GetService(serviceType);
            if (instance == null)
            {
                throw new ServiceMissingException(serviceType, component);
            }
            return instance;
        }

		#endregion Public Members
	}
}