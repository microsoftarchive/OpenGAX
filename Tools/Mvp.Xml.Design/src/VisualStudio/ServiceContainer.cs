using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Mvp.Xml.Design.VisualStudio 
{
	/// <summary>
	/// Implements a generic container for services.
	/// </summary>
	public abstract class ServiceContainer : IServiceContainer, IServiceProvider, IOleServiceProvider, IDisposable
	{
		#region Fields & Ctors

		IServiceProvider _provider;
		IServiceContainer _container;
		IProfferService _proffer;

		/// <summary>
		/// Default parameterless constructor for standalone containers.
		/// </summary>
		public ServiceContainer() {}

		/// <summary>
		/// Initializes the container with a parent service provider.
		/// </summary>
		public ServiceContainer(IServiceProvider provider)
		{
			_provider = provider;
			// Try to retrieve a parent container.
			_container = (IServiceContainer) provider.GetService(typeof (IServiceContainer));
			// Try to retrieve the proffer service.
			_proffer = (IProfferService) provider.GetService(typeof (IProfferService));
		}

		#endregion Fields & Ctors

		#region IServiceContainer Members

		/// <summary>
		/// Removes the specified service type from the service container, and optionally promotes the service to parent service containers.
		/// </summary>
		/// <param name="serviceType">The type of service to remove.</param>
		/// <param name="promote"><c>true</c> to promote this request to any parent service containers; otherwise, <c>false</c>.</param>
		public void RemoveService(Type serviceType, bool promote)
		{
			RemoveService(serviceType);
			if (promote) RemovePromotedService(serviceType);
		}

		/// <summary>
		/// Removes the specified service type from the service container.
		/// </summary>
		/// <param name="serviceType">The type of service to remove.</param>
		public void RemoveService(Type serviceType)
		{
			_services.Remove(serviceType);
		}

		/// <summary>
		/// Adds the specified service to the service container, and optionally promotes the service to parent service containers.
		/// </summary>
		/// <param name="serviceType">The type of service to add.</param>
		/// <param name="callback">A callback object that is used to create the service. This allows a service to be declared as available, but delays the creation of the object until the service is requested.</param>
		/// <param name="promote"><c>true</c> to promote this request to any parent service containers; otherwise, <c>false</c>.</param>
		public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
		{
			AddService(serviceType, callback);
			if (promote) AddPromotedService(serviceType, callback);
		}

		/// <summary>
		/// Adds the specified service to the service container.
		/// </summary>
		/// <param name="serviceType">The type of service to add.</param>
		/// <param name="callback">A callback object that is used to create the service. This allows a service to be declared as available, but delays the creation of the object until the service is requested.</param>
		public void AddService(Type serviceType, ServiceCreatorCallback callback)
		{
			if (serviceType == null)
				throw new ArgumentNullException("serviceType");
 
			if (callback == null)
				throw new ArgumentNullException("callback");
 
			if (_services.Contains(serviceType))
				throw new ArgumentException("Service already exists.", "serviceType");

			_services[serviceType] = callback;
		}

		/// <summary>
		/// Adds the specified service to the service container, and optionally promotes the service to any parent service containers.
		/// </summary>
		/// <param name="serviceType">The type of service to add.</param>
		/// <param name="serviceInstance">An instance of the service type to add. This object must implement or inherit from the type indicated by the serviceType parameter. </param>
		/// <param name="promote"><c>true</c> to promote this request to any parent service containers; otherwise, <c>false</c>.</param>
		public void AddService(Type serviceType, object serviceInstance, bool promote)
		{
			AddService(serviceType, serviceInstance);
			if (promote) AddPromotedService(serviceType, serviceInstance);
		}

		/// <summary>
		/// Adds the specified service to the service container.
		/// </summary>
		/// <param name="serviceType">The type of service to add.</param>
		/// <param name="serviceInstance">An instance of the service type to add. This object must implement or inherit from the type indicated by the serviceType parameter. </param>
		public void AddService(Type serviceType, object serviceInstance)
		{
			if (serviceType == null)
				throw new ArgumentNullException("serviceType");
 
			if (serviceInstance == null)
				throw new ArgumentNullException("serviceInstance");
 
			// __ComObjects are assignable anyway. We can't check that. 
			// This is what System.ComponentModel.Design.ServiceContainer does.
			if (((serviceInstance as ServiceCreatorCallback) == null) && 
				((!serviceInstance.GetType().IsCOMObject && !serviceType.IsAssignableFrom(serviceInstance.GetType()))))
				throw new ArgumentException("Invalid service instance.", "serviceInstance");

			if (_services.Contains(serviceType))
				throw new ArgumentException("Service already exists.", "serviceType");

			_services[serviceType] = serviceInstance;
		}

		#endregion

		#region IServiceProvider Members

		/// <summary>
		/// Provides access to services in this container.
		/// </summary>
		/// <param name="serviceType">The service type to retrieve.</param>
		/// <returns>The service instance or null if it's not available.</returns>
		public virtual object GetService(Type serviceType)
		{
			object serviceInstance = _services[serviceType];

			if ((serviceInstance as ServiceCreatorCallback) != null)
			{
				ServiceCreatorCallback cbk = (ServiceCreatorCallback) serviceInstance;
				// Create the instance through the callback.
				serviceInstance = cbk(this, serviceType);

				// __ComObjects are assignable anyway. We can't check that. 
				// This is what System.ComponentModel.Design.ServiceContainer does.
				if ((serviceInstance != null) && (!serviceInstance.GetType().IsCOMObject && 
					!serviceType.IsAssignableFrom(serviceInstance.GetType()))) 
				{
					_services.Remove(serviceType);
					serviceInstance = null;
				}
				else
				{
					_services[serviceType] = serviceInstance; 
				}
			}

			// Propagate request to parents.
			if (serviceInstance == null && _provider != null)
				return _provider.GetService(serviceType);

			return serviceInstance;

		} IDictionary _services = new System.Collections.Specialized.HybridDictionary();

		#endregion

		#region IOleServiceProvider Members

		/// <summary>
		/// Provides the ole implementation of the service retrieval routine.
		/// </summary>
		int IOleServiceProvider.QueryService(ref Guid guidService, ref Guid riid, out System.IntPtr ppvObject)
		{
			foreach (DictionaryEntry entry in _services)
			{
				if (((Type)entry.Key).GUID == guidService)
				{
					object service = GetService((Type) entry.Key);
					IntPtr pUnk = System.Runtime.InteropServices.Marshal.GetIUnknownForObject(service);
					int hr = System.Runtime.InteropServices.Marshal.QueryInterface(pUnk, ref riid, out ppvObject);
					System.Runtime.InteropServices.Marshal.Release(pUnk);

					return hr;
				}
			}

			ppvObject = (IntPtr) 0;
			return 0;
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Clears the container state.
		/// </summary>
		public virtual void Dispose()
		{
			// Remove all services promoted to VS.
			foreach (Type svc in _profferedservices.Keys)
			{
				RemovePromotedService(svc);
			}
		}

		#endregion

		#region Service promotion

		/// <summary>
		/// Keeps services that were promoted directly to VS.
		/// </summary>
		IDictionary _profferedservices = new System.Collections.Specialized.HybridDictionary();

		private void AddPromotedService(Type serviceType, object serviceInstanceOrCallback)
		{
			if (_container == null && _proffer == null) return;

			// Regular service promotion.
			if (_container != null)
			{
				_container.AddService(serviceType, serviceInstanceOrCallback, true);
				return;
			}

			// Proffered services promotion.
			if (_proffer != null)
			{
				ProfferedService svc = new ProfferedService();
				svc.Instance = serviceInstanceOrCallback;                
				uint cookie;
				Guid sg = serviceType.GUID;
				int hr = _proffer.ProfferService(ref sg, this, out cookie);
				svc.Cookie= cookie;
				// If there're failures, throw?
				if (hr < 0)
				{
					throw new System.Runtime.InteropServices.COMException(
						String.Format("Failed to proffer service {0}", serviceType.FullName), hr);
				}

				_profferedservices[serviceType] = svc;
			}
		}

		private void RemovePromotedService(Type serviceType)
		{
			if (_container == null && _proffer == null) return;

			// Regular service demotion.
			if (_container != null)
			{
				_container.RemoveService(serviceType, true);
				return;
			}

			// We have a proffered service at hand.
			ProfferedService svc = (ProfferedService) _profferedservices[serviceType];
			if (svc != null)
			{
				if (svc.Cookie != 0)
				{
					_proffer.RevokeService(svc.Cookie);
					// Dispose if appropriate, but don't dispose ourselves again.
					if (svc.Instance is IDisposable && svc.Instance != this)
					{
						((IDisposable)svc.Instance).Dispose();
					}
				}
			}
		}

		#region ProfferedService class

		/// <summary>
		/// This class contains a service that is being promoted to VS.  
		/// </summary>
		private sealed class ProfferedService 
		{
			public object Instance;
			public uint   Cookie;
		}

		#endregion ProfferedService class

		#endregion Service proffering
	}
}
