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
    /// Represents a component that wish to implement behavior when sited.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class SitedComponent : Component
    {
        /// <summary>
        /// Gets or sets the <see cref="ISite"/> of the <see cref="Component"/>.
        /// </summary>
        /// <value>
        /// The <see cref="ISite"/> of the <see cref="Component"/>.
        /// </value>
        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;
				if (value != null)
				{
					OnSited();
				}
			}
        }

        /// <summary>
        /// When implemented by a class, allows descendants to 
        /// perform processing whenever the component is being sited.
        /// </summary>
        protected virtual void OnSited()
        {
        }

		/// <summary>
		/// When implemented by a class, allows descendants to 
		/// throw more meaningful exceptions when dependent services are not found.
		/// </summary>
		/// <param name="missingService">The type of the missing service.</param>
		/// <exception cref="InvalidOperationException">
		/// A dependent service was not found in the current component container.
		/// </exception>
		/// <remarks>
		/// Descendents should throw an exception upon calls to this method if the 
		/// component can not work without the missing dependency. If an exception is 
		/// not thrown, the component will be sited and execution will continue.
		/// </remarks>
		protected internal virtual void OnMissingServiceDependency(Type missingService)
		{
            throw new ServiceMissingException(missingService, this);
		}

		/// <summary>
		/// Generic version of <see cref="GetService()"/>.
		/// </summary>
		public T GetService<T>()
		{
			return (T)GetService(typeof(T));
		}

		/// <summary>
		/// Tries to retrieve a service, and if it's not found, 
		/// throws a <see cref="ServiceMissingException"/>.
		/// </summary>
		/// <exception cref="ServiceMissingException">The service was not found in the component site.</exception>
		public T GetService<T>(bool ensureExists)
		{
			T serviceInstance = GetService<T>();
			if (serviceInstance == null && ensureExists)
			{
				throw new ServiceMissingException(typeof(T), this);
			}
			return serviceInstance;
		}
	}
}