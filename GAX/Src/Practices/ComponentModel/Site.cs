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
    /// Basic <see cref="ISite"/> implementation that passes 
    /// all service requests to the parent service provider.
    /// </summary>
    public class Site : ISite
	{
		#region Field & Ctor

		IServiceProvider provider;

        /// <summary>
        /// Constructs a site.
        /// </summary>
        /// <param name="provider">The object providing services to this site.</param>
        /// <param name="component">The component this site is being associated with.</param>
        /// <param name="name">A name for the site.</param>
        public Site(IServiceProvider provider, IComponent component, string name)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

			if (component == null)
                throw new ArgumentNullException("component");

            this.component = component;
            // Pull the container from the service provider (if any).
            this.container = (IContainer) provider.GetService(typeof(IContainer));
            this.provider = provider;
            this.name = name;
		}

		#endregion Field & Ctor

		#region Public Properties

		/// <summary>
		/// See <see cref="ISite.Component"/>.
        /// </summary>
        public IComponent Component
        {
            get { return component; }
        } IComponent component;

        /// <summary>
        /// See <see cref="ISite.Container"/>.
        /// </summary>
        public IContainer Container
        {
            get { return container; }
		} IContainer container;

		/// <summary>
        /// Always returns <see langword="false"/>.
        /// </summary>
        public bool DesignMode
        {
            get { return designMode; }
		} bool designMode = false;

		/// <summary>
        /// See <see cref="ISite.Name"/>.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
		} string name;

		#endregion Public Properties

		#region GetService

		/// <summary>
		/// See <see cref="IServiceProvider.GetService"/>.
        /// </summary>
        public virtual object GetService(Type serviceType)
        {
            if (serviceType != typeof(ISite))
                return provider.GetService(serviceType);

			return this;
        }

		#endregion GetService
	}
}