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

#endregion Using directives

namespace Microsoft.Practices.ComponentModel
{
    /// <summary>
    /// Specifies the dependencies of a command on services that must be available at 
    /// run-time to ensure proper functioning.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public sealed class ServiceDependencyAttribute : Attribute
    {
        /// <summary>
        /// Initializes the dependency.
        /// </summary>
        /// <param name="serviceType">The service type required by the component.</param>
        public ServiceDependencyAttribute(Type serviceType)
        {
            service = serviceType;
        }

        /// <summary>
        /// Gets the service type of the dependency.
        /// </summary>
        public Type ServiceType
        {
            get { return service; }
        } Type service;
    }
}