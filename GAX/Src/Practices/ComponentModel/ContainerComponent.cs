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

#endregion

namespace Microsoft.Practices.ComponentModel
{
	/// <summary>
	/// Represents a container that can be a component in a parent 
	/// container at the same time.
	/// </summary>
	/// <remarks>
	/// This container automatically checks any <see cref="ServiceDependencyAttribute"/> attributes 
	/// on a component being added to it, so that an exception will be rised if the 
	/// dependent service does not exist in this container or its parent if there is one.
	/// <para>
	/// Components that inherit from <see cref="SitedComponent"/> will get the 
	/// <see cref="SitedComponent.OnMissingServiceDependency"/> method called whenever a missing 
	/// service is detected, giving the component developer a chance to throw a more meaningful 
	/// exception. 
	/// </para>
	/// </remarks>
	public class ContainerComponent : Container, IComponent, IServiceProvider
	{
		#region Container Overrides

		/// <summary>
		/// Disposes the container and all its child components.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			#region See Component.Dispose implementation

			if (!disposing)
			{
				return;
			}

			lock (this)
			{
				if ((this.Site != null) && (this.Site.Container != null))
				{
					this.Site.Container.Remove(this);
				}
				if (Disposed == null)
				{
					return;
				}
				EventHandler ev = Disposed;
				if (ev == null)
				{
					return;
				}
				// Rise the event.
				ev(this, EventArgs.Empty);
			}

			#endregion See Component.Dispose implementation
            base.Dispose(disposing);
        }

		/// <summary>
		/// Creates a <see cref="ISite"/> for the given <see cref="IComponent"/> and assigns the given name to the site.
		/// </summary>
		/// <param name="component">The <see cref="IComponent"/> to create a site for.</param>
		/// <param name="name">The name to assign to component; or <see langword="null"/> to skip the name assignment.</param>
		protected override ISite CreateSite(IComponent component, string name)
		{
			// Ensure service dependencies are satisfied.
			ServiceHelper.CheckDependencies(component, this);

			// Create our own site that will forward calls to GetService to us.
			return new Site(this, component, name);
		}

		/// <summary>
		/// Generic version of <see cref="GetService()"/>.
		/// </summary>
		public T GetService<T>()
		{
			return (T)GetService(typeof(T));
		}

		/// <summary>
		/// Generic version of <see cref="GetService(bool)"/>.
		/// </summary>
		public T GetService<T>(bool ensureExists)
		{
			return (T)GetService(typeof(T), ensureExists);
		}		

        /// <summary>
        /// Tries to retrieve services from this container or the parent ones, and 
        /// optionally ensures that the service exists.
        /// </summary>
        /// <param name="service">The service to retrieve.</param>
        /// <param name="ensureExists">If <see langword="true"/>, will check that the 
        /// service exists and is not <see langword="null"/>, otherwise will 
        /// throw an <see cref="ServiceMissingException"/>.</param>
        /// <returns>The service or <see langword="null"/> if none exists.</returns>
        /// <remarks>
        /// Offers the same behavior as <see cref="IServiceProvider.GetService"/>, but 
        /// if the <paramref name="ensureExists"/> is <see langword="true"/>, failure 
        /// to retrieve an actual instance of the service will result in an 
        /// <see cref="ServiceMissingException"/>.
        /// </remarks>
        /// <exception cref="ServiceMissingException">Exception thrown if 
        /// a valid instance of the requested service is not found and the 
        /// <paramref name="ensureExists"/> is <see langword="true"/>.</exception>
        public object GetService(Type service, bool ensureExists)
        {
            //CheckDisposed(); // See ContainerComponent.cs (180)
            object instance = GetService(service);
            if (instance == null && ensureExists)
            {
                throw new ServiceMissingException(service, this);
            }
            return instance;
        }

		/// <summary>
		/// Tries to retrieve services from this container or the parent ones.
		/// </summary>
		/// <param name="service">The service to retrieve.</param>
		/// <returns>The service or <see langword="null"/> if none exists.</returns>
		protected override object GetService(Type service)
		{
			if (service == typeof(IContainer))
			{
				return this;
			}

			// If the container is sited, pass the call upwards.
			if (this.Site != null)
			{
				return this.Site.GetService(service);
			}

			return null;
		}

		#endregion Container Overrides

		#region IComponent Members

		/// <summary>
		/// Event rised when the component is being disposed.
		/// </summary>
		public event EventHandler Disposed;

		/// <summary>
		/// Provides a way to site the container in a parent container.
		/// </summary>
		public ISite Site
		{
            get
            {
                //CheckDisposed(); // See ContainerComponent.cs (180)
                return site;
            }
			set
			{
                //CheckDisposed(); // See ContainerComponent.cs (180)
                site = value;
				if (value != null)
				{
					OnSited();
				}
			}
		} ISite site;

		/// <summary>
		/// When implemented by a class, allows descendants to 
		/// perform processing whenever the component is being sited.
		/// </summary>
		protected virtual void OnSited()
		{
		}

		#endregion IComponent Members

		#region IServiceProvider Members

		object IServiceProvider.GetService(Type serviceType)
		{
            //CheckDisposed(); // See ContainerComponent.cs (180)
            return GetService(serviceType);
		}

		#endregion IServiceProvider Members

        // UNDONE: the current behavior is consistent with the .NET Framework Container class,
        // which upon disposal just disposes its components, but not the container itself, which continues to be usable.
        //bool disposed = false;

        ///// <summary>
        ///// Checks that the object has not been disposed.
        ///// </summary>
        ///// <exception cref="ObjectDisposedException">The object has been disposed.</exception>
        //protected void CheckDisposed()
        //{
        //    if (disposed)
        //    {
        //        //throw new ObjectDisposedException(this.ToString());
        //    }
        //}
    }
}
