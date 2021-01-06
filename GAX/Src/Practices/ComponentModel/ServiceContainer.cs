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
using System.Runtime.InteropServices;
using System.Diagnostics;

#endregion Using directives

namespace Microsoft.Practices.ComponentModel
{
	/// <summary>
	/// Represents a container for components and services.
	/// </summary>
	/// <remarks>
	/// This container automatically checks any <see cref="ServiceDependencyAttribute"/> attributes 
	/// on a component being added to it, so that an exception will be rised if the 
	/// dependent service does not exist in this container.
	/// <para>
	/// Components that inherit from <see cref="SitedComponent"/> will get the 
	/// <see cref="SitedComponent.OnMissingServiceDependency"/> method called whenever a missing 
	/// service is detected, giving the component developer a chance to throw a more meaningful 
	/// exception. 
	/// </para>
	/// Services added to the container that implement <see cref="IComponent"/> are automatically 
	/// sited on the container too.
	/// </remarks>
	public class ServiceContainer : ContainerComponent, IServiceContainer
	{
		#region Fields, Ctors & Lifecycle

		/// <summary>
		/// Reference to optional parent container service, so that services can 
		/// be promoted.
		/// </summary>
		IServiceContainer parentContainer;
		/// <summary>
		/// List of promoted services, to perform demotion afterwards if necessary.
		/// </summary>
		IDictionary promoted;
		/// <summary>
		/// Services owned by this container.
		/// </summary>
		IDictionary services;
		/// <summary>
		/// Flag specifying whether components get access to the IServiceContainer service.
		/// </summary>
		bool isServiceContainer = true;

		/// <summary>
		/// Initialize a new instance of the <see cref="ServiceContainer"/> class.
		/// </summary>
		/// <remarks>In order to let contained components to publish additional services, use 
		/// the overload receiving the <c>exposeServiceContainerService</c> flag.</remarks>
		public ServiceContainer()
		{
			promoted = new System.Collections.Specialized.HybridDictionary();
			services = new System.Collections.Specialized.HybridDictionary();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceContainer"/> class using the specified parent service provider.
		/// </summary>
		/// <param name="exposeServiceContainerService">Whether the <see cref="IServiceContainer"/> service 
		/// for this container should be exposed, to allow contained component to publish new services.</param>
		public ServiceContainer(bool exposeServiceContainerService) : this()
		{
			isServiceContainer = exposeServiceContainerService;
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the <see cref="ServiceContainer"/> is reclaimed by garbage collection. 
		/// </summary>
		~ServiceContainer()
		{
			Dispose(false);
		}

		/// <summary>
		/// Releases all resources and performs other cleanup operations before the <see cref="ServiceContainer"/> 
		/// is reclaimed by garbage collection.
		/// </summary>
		/// <param name="disposing"><see langword="true"/> if we are disposing; 
		/// otherwise, <see langword="false"/> which means the call is coming from our finalizer.</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			// Remove all services promoted to VS.
			foreach (Type svc in promoted.Keys)
			{
				RemovePromotedService(svc);
			}
			// Dispose services that are disposable.
			foreach (object svc in services.Values)
			{
				// Don't dispose ourselves again.
                IDisposable disposable = svc as IDisposable;
				if (disposable != null && disposable != this)
				{
					try
					{
						disposable.Dispose();
					}
					catch (Exception ex)
					{
						this.TraceWarning("Some objects does not support Displose: " + ex.GetType().ToString());
					}
				}
			}

			GC.SuppressFinalize(this);
		}

        #endregion Fields, Ctors & Lifecycle

        #region AddService Overloads

		/// <summary>
		/// Adds the specified service to the service container, 
		/// and optionally promotes the service to parent service containers.
		/// </summary>
		/// <param name="serviceType">The type of service to add.</param>
		/// <param name="callback">A callback object that is used to create the service. This allows a service to be declared as available, but delays the creation of the object until the service is requested.</param>
		/// <param name="promote"><see langword="true"/> to promote this request to any parent service containers; otherwise, <see langword="false"/>.</param>
		/// <exception cref="ArgumentNullException">
		/// Either <paramref name="serviceType"/> or <paramref name="callback"/> 
		/// is a <see langword="null"/> reference.
		/// </exception>
		public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
		{
			AddService(serviceType, callback);
            if (promote)
            {
                AddPromotedService(serviceType, callback);
            }
		}

		/// <summary>
		/// Adds the specified service to the service container.
		/// </summary>  
		/// <param name="serviceType">The type of service to add.</param>
		/// <param name="callback">A callback object that is used to create the service. This allows a service to be declared as available, but delays the creation of the object until the service is requested.</param>
		/// <exception cref="ArgumentNullException">
		/// Either <paramref name="serviceType"/> or <paramref name="callback"/> 
		/// is a <see langword="null"/> reference.
		/// </exception>
		public void AddService(Type serviceType, ServiceCreatorCallback callback)
		{
            //CheckDisposed(); // See ContainerComponent.cs (180)
            if (null == serviceType)
            {
                throw new ArgumentNullException("serviceType");
            }
            if (null == callback)
            {
                throw new ArgumentNullException("callback");
            }
            if (services.Contains(serviceType))
            {
                throw new ArgumentException(Properties.Resources.ServiceContainer_ServiceExists, "serviceType");
            }
			services[serviceType] = callback;
		}

		/// <summary>
		/// Adds the specified service to the service container, 
		/// and optionally promotes the service to any parent service containers.
		/// </summary>
		/// <param name="serviceType">The type of service to add.</param>
		/// <param name="serviceInstance">An instance of the service type to add. 
		/// This object must implement or inherit from the type indicated by the serviceType parameter.</param>
		/// <param name="promote"><see langword="true"/> to promote this request to any parent service containers; otherwise, <see langword="false"/>.</param>
		/// <exception cref="ArgumentNullException">
		/// </exception>
		public void AddService(Type serviceType, object serviceInstance, bool promote)
		{
			AddService(serviceType, serviceInstance);
			if (promote)
			{
				AddPromotedService(serviceType, serviceInstance);
			}
		}

		/// <summary>
		/// Adds the specified service to the service container.
		/// </summary>
		/// <param name="serviceType">The type of service to add.</param>
		/// <param name="serviceInstance">An instance of the service type to add. 
		/// This object must implement or inherit from the type indicated by the serviceType parameter.</param>
		/// <exception cref="ArgumentNullException">
		/// </exception>
		public void AddService(Type serviceType, object serviceInstance)
		{
            //CheckDisposed(); // See ContainerComponent.cs (180)
            if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceInstance == null)
			{
				throw new ArgumentNullException("serviceInstance");
			}


			if (IsInvalidInstance(serviceType, serviceInstance))
			{
				throw new ArgumentException(String.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.ServiceContainer_InvalidServiceInstance,
					serviceInstance.GetType(), serviceType), "serviceInstance");
			}

			if (services.Contains(serviceType))
			{
				throw new ArgumentException(String.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.ServiceContainer_ServiceExists, serviceType),
					"serviceType");
			}

			// If a service is a component, it's automatically sited.
			if (serviceInstance is IComponent && serviceInstance != this)
			{
				Add((IComponent)serviceInstance);
			}

			services[serviceType] = serviceInstance;
		}

        #endregion AddService Overloads

        #region GetService

		/// <summary>
		/// Gets the requested service.
		/// </summary>
		/// <param name="service">The type of service to retrieve.</param>
		/// <returns>An instance of the service if it could be found, 
		/// or a <see langword="null"/> reference if it could not be found.</returns>
		/// <seealso cref="Container"/>
		protected override object GetService(Type service)
		{
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
			// Our services first.
			if (service == typeof(IServiceContainer))
			{
				// Only if we're told to expose it.
				if (isServiceContainer)
				{
					return this;
				}
				else
				{
					// Return a parent container if available.
					return this.Site != null ?
						this.Site.GetService(service) : null;
				}
			}
			if (service == typeof(IServiceProvider))
			{
				return this;
			}

			object serviceInstance = services[service];

			if (serviceInstance != null && serviceInstance is ServiceCreatorCallback)
			{
				ServiceCreatorCallback cbk = (ServiceCreatorCallback)serviceInstance;
				// Create the instance through the callback.
				serviceInstance = cbk(this, service);

				if (IsInvalidInstance(service, serviceInstance))
				{
					services.Remove(service);
					throw new ArgumentException(String.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
						Properties.Resources.ServiceContainer_InvalidServiceInstance,
						serviceInstance.GetType(), service), "serviceInstance");
				}
				else
				{
					services[service] = serviceInstance;

					// If the created service is a component, it's automatically sited.
					if (serviceInstance is IComponent && serviceInstance != this)
					{
						Add((IComponent)serviceInstance);
					}
				}
			}

			// Propagate request to parents.
			if (serviceInstance == null)
			{
				// Base ContainerComponent class automatically bases the call 
				// upwards to the Site if there's one.
				return base.GetService(service);
			}

			return serviceInstance;
		}

		object IServiceProvider.GetService(Type serviceType)
		{
			return GetService(serviceType);
		}

		#endregion GetService

		/// <summary>
		/// return the TraceSource specific to the current gax package.
		/// </summary>
		public TraceSource SiteTraceSource
		{
			get
			{
				return ((IOutputWindowService)GetService(typeof(IOutputWindowService))).TraceSource;
			}
		}

		#region RemoveService Overloads

		/// <summary>
		/// Removes the specified service type from the service container.
		/// </summary>
		/// <param name="serviceType">The type of service to remove.</param>
		/// <param name="promote"><see langword="true"/> to promote this request to any parent 
		/// service containers; otherwise, <see langword="false"/>.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="serviceType"/> is a <see langword="null"/> reference.
		/// </exception>
		public void RemoveService(Type serviceType, bool promote)
		{
			RemoveService(serviceType);
			if (promote)
			{
				RemovePromotedService(serviceType);
			}
		}

		/// <summary>
		/// Removes the specified service type from the service container.
		/// </summary>
		/// <param name="serviceType">The type of service to remove.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="serviceType"/> is a <see langword="null"/> reference.
		/// </exception>
		public void RemoveService(Type serviceType)
		{
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
			// Check for IComponent, as we need to remove it from the container.
			object serviceInstance = services[serviceType];
			if (serviceInstance != null && serviceInstance is IComponent)
				Remove((IComponent)serviceInstance);

			// Finally remove the service type.
			services.Remove(serviceType);
		}

		#endregion RemoveService Overloads

		#region ContainerComponent Overrides

		/// <summary>
		/// When implemented by a class, allows descendants to 
		/// perform processing whenever the component is being sited.
		/// </summary>
		protected override void OnSited()
		{
			base.OnSited();
			// Try to retrieve a parent service container.
			parentContainer = this.Site.GetService(typeof(IServiceContainer)) as IServiceContainer;
		}

		#endregion ContainerComponent Overrides

		#region Promoted Services Implementation

		private void AddPromotedService(Type serviceType, object serviceInstanceOrCallback)
		{
			if (parentContainer == null)
				return;

			parentContainer.AddService(serviceType, serviceInstanceOrCallback, true);
            promoted.Add(serviceType, null);
		}

		private void RemovePromotedService(Type serviceType)
		{
			if (parentContainer == null)
				return;

			parentContainer.RemoveService(serviceType, true);
		}

		#endregion Promoted Services Implementation

		#region Temporary services

		/// <summary>
		/// Temporarily adds a a service in the container.
		/// </summary>
		/// <param name="serviceType">Type of the service to replace.</param>
		/// <param name="serviceInstance">The instance to use for the service.</param>
		/// <remarks>
		/// When the returned object is disposed, the container is brought back to its 
		/// original state. This means that if the service was a new one, it will be removed, 
		/// and if there was an existing one, the original is put back in the container and 
		/// the new one removed.
		/// </remarks>
		public IDisposable SetupTemporaryService(Type serviceType, object serviceInstance)
		{
            //CheckDisposed(); // See ContainerComponent.cs (180)
            // Arguments are checked when the service is added.
			return new ServiceReplacer(this, serviceType, serviceInstance);
		}

        private class ServiceReplacer : IDisposable
        {
            object originalService;
            object serviceInstance;
            ServiceContainer container;
            Type serviceType;

            public ServiceReplacer(ServiceContainer container, Type serviceType, object serviceInstance)
            {
                this.originalService = container.GetService(serviceType);
                this.serviceType = serviceType;
                this.container = container;
                this.serviceInstance = serviceInstance;
                if (originalService != null)
                {
                    // Let's site the new service, note that the old service is still in it, 
                    // so serviceInstance can get a hold of the old service during the OnSited call.
                    // This will cause a double siting when the service instance is actually added
                    // to the parent container. To be removed when we introduce a base class for this.
                    if (serviceInstance is IComponent)
                    {
                        //Site the service into the container
                        ((IComponent)serviceInstance).Site = new Site(container,
                            (IComponent)serviceInstance, originalService.GetType().ToString());
                    }
                    // We are about to change the container for the old service, 
                    // so let's add it to the new container if there's one.
                    if (serviceInstance is IServiceContainer)
                    {
                        //Site the originalService into the new Service
                        ((IServiceContainer)serviceInstance).AddService(serviceType, originalService);
                    }
                    // Now let's remove the service from the container
                    this.container.RemoveService(serviceType);
                    //Make sure that the originalService is still sited
                    if (originalService is IComponent)
                    {
                        ((IComponent)originalService).Site = new Site(container,
                            (IComponent)originalService, originalService.GetType().ToString());
                    }
                }
                this.container.AddService(serviceType, serviceInstance);
            }

            #region IDisposable Members

            public void Dispose()
            {
                container.RemoveService(serviceType);
                if (originalService != null)
                {
                    if (serviceInstance is IServiceContainer)
                    {
                        //Make sure to remove the originalService from the temporary service
                        ((IServiceContainer)serviceInstance).RemoveService(serviceType);
                    }
                    container.AddService(serviceType, originalService);
                }
            }

            #endregion
        }

		#endregion Temporary services

        /// <summary>
        /// Implements the same rules as ReflectionHelper.IsAssignableTo in Practices.Common. 
        /// Not used here to avoid an unnecessary dependency.
        /// </summary>
        static private bool IsInvalidInstance(Type serviceType, object serviceInstance)
        {
            bool isinvalid = false;
            // We can only QI if the service type is an interface.
            if (serviceInstance.GetType().IsCOMObject)
            {
                if (serviceType.IsInterface)
                {
                    // Try a QI.
                    IntPtr iunk = Marshal.GetIUnknownForObject(serviceInstance);
                    Guid typeguid = serviceType.GUID;
                    IntPtr iface = IntPtr.Zero;
                    Marshal.QueryInterface(iunk, ref typeguid, out iface);
                    isinvalid = (iface == IntPtr.Zero);
					if (!isinvalid)
					{
						Marshal.Release(iface);
					}
					if (iunk != IntPtr.Zero)
					{
						Marshal.Release(iunk);
					}
				}
                else
                {
                    // Can't QI a class type. Assume it's not assignable.
                    isinvalid = true;
                }
            }
            else if (serviceInstance.GetType() != serviceType &&
                !serviceType.IsAssignableFrom(serviceInstance.GetType()))
            {
                isinvalid = true;
            }
            return isinvalid;
        }
	}
}