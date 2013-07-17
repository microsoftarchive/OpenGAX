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
using System.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Represents the abstract base class for asset references.
	/// </summary>
	/// <remarks>
	/// This base class does not inherit from <see cref="Component"/>, in order
	/// to better control serialization of the reference and its members.
	/// </remarks>
    [Serializable]
    public abstract class AssetReference : IAssetReference, ISerializable
    {
        /// <summary>
        /// Initializes the reference to a specified asset.
        /// </summary>
        /// <param name="assetName">The name of the asset represented by this instance.</param>
        public AssetReference(string assetName)
        {
            this.assetName = assetName;
        }

        #region IAssetReference Members

        /// <summary>
        /// See <see cref="IAssetReference.AppliesTo"/>.
        /// </summary>
        public abstract string AppliesTo { get; }

        private string assetName;

        /// <summary>
        /// See <see cref="IAssetReference.AssetName"/>.
        /// </summary>
        public string AssetName
        {
            get { return assetName; }
            protected set { assetName = value; }
        }

        /// <summary>
        /// Gets a key that uniquely identifies this reference in a package. 
        /// </summary>
        public abstract string Key { get; }

        /// <summary>
        /// Gets a caption for the reference.
        /// </summary>
        public abstract string Caption { get; }

        /// <summary>
        /// Gets a description of the purpose of the asset being referenced.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Executes the asset.
        /// </summary>
        /// <returns>An <see cref="ExecutionResult"/> that represents the result of the execution.</returns>
        public ExecutionResult Execute()
        {
            CheckState();
            return OnExecute();
        }

        #endregion

        #region Execution

        /// <summary>
        /// Implements the actual asset execution code.
        /// </summary>
        /// <remarks>
        /// Preconditions for execution such as an existing site and execution service
        /// are pre-checked before calling this method.
        /// </remarks>
        /// <returns>
        /// An <see cref="ExecutionResult"/> that represents the result of the execution as 
        /// implemented on a derived class.
        /// </returns>
        protected abstract ExecutionResult OnExecute();

        #endregion Execution

        #region IComponent Members

        /// <summary>
        /// See <see cref="IComponent.Disposed"/>.
        /// </summary>
        public event EventHandler Disposed;

        [NonSerialized]
        private ISite site;

        /// <summary>
        /// See <see cref="IComponent.Site"/>.
        /// </summary>
        public ISite Site
        {
            get { return site; }
            set
            {
                site = value;
                if (site != null)
                {
                    OnSited();
                }
            }
        }

        /// <summary>
        /// Called when the site is set to a non-null value. 
        /// </summary>
        protected virtual void OnSited()
        {
        }

        /// <summary>
        /// Retrieves a service from the component's site.
        /// </summary>
        /// <param name="service">A <see cref="Type"/> representing the service to retrieve.</param>
        /// <returns>An <see cref="Object"/> that represents the service instance or <see langword="null"/>.</returns>
        protected object GetService(Type service)
        {
            if (site != null)
            {
                return site.GetService(service);
            }
            return null;
        }

		/// <summary>
		/// Generic version of <see cref="GetService()"/>.
		/// </summary>
		protected T GetService<T>()
		{
			return (T)GetService(typeof(T));
		}

		/// <summary>
		/// Tries to retrieve a service, and if it's not found, 
		/// throws a <see cref="ServiceMissingException"/>.
		/// </summary>
		/// <exception cref="ServiceMissingException">The service was not found in the component site.</exception>
		protected T GetService<T>(bool ensureExists)
		{
			T serviceInstance = GetService<T>();
			if (serviceInstance == null && ensureExists)
			{
				throw new ServiceMissingException(typeof(T), this);
			}
			return serviceInstance;
		}

        #endregion

        #region IDisposable Members

        bool disposed;

        /// <summary>
        /// Checks preconditions for usage of the component, such as 
        /// whether the object is sited or if it has been disposed.
        /// </summary>
        protected void CheckState()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(this.GetType().ToString());
            }
            if (this.Site == null)
            {
                throw new InvalidOperationException(String.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.General_ComponentMustBeSited, this));
            }
        }

        /// <summary>
        /// See <see cref="IDisposable.Dispose"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes the component and removes it from its container, if it is sited.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged 
        /// resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (this)
                {
                    if ((site != null) && (site.Container != null))
                    {
                        site.Container.Remove(this);
                    }
                    EventHandler handler = Disposed;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                }
            }
            disposed = true;
        }

        #endregion

        #region ISerializable Members

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetReference"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected AssetReference(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentException("info can not be null", "info");
            }

            this.assetName = info.GetString("asset");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("asset", this.assetName);
            GetObjectData(info, context);
        }

        /// <summary>
        /// See <see cref="ISerializable.GetObjectData"/>.
        /// </summary>
        /// <remarks>
        /// Derived classes must override this method to persist additional 
        /// information that may be needed when the special constructor for deserialization 
        /// is called upon rehydration.
        /// </remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected abstract void GetObjectData(SerializationInfo info, StreamingContext context);

        #endregion ISerializable Members
    }
}