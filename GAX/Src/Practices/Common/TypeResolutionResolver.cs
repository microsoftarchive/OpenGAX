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
using System.ComponentModel.Design;
using System.Reflection;

namespace Microsoft.Practices.Common
{
    /// <summary>
    /// A disposable object that can be used to setup a resolution 
    /// context for the AppDomain that will use the received 
    /// <see cref="ITypeResolutionService"/> to probe for assemblies.
    /// </summary>
    /// <remarks>
    /// Typical use for this resolver is:
    /// <example>
    /// <code>
    /// using (TypeResolutionResolver resolver = new TypeResolutionResolver(resolutionService))
    /// {
    ///     // Do something that may cause probing for assemblies.
    /// }
    /// </code>
    /// </example>
    /// </remarks>
    public sealed class TypeResolutionResolver : IDisposable
    {
        ITypeResolutionService resolutionService;

        /// <summary>
        /// Constructs the resolver using the specified 
        /// <see cref="ITypeResolutionService"/>.
        /// </summary>
        public TypeResolutionResolver(ITypeResolutionService resolutionService)
        {
            this.resolutionService = resolutionService;
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(OnAssemblyResolve);
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(OnAssemblyResolve);
            try
            {
                return resolutionService.GetAssembly(ReflectionHelper.ParseAssemblyName(args.Name), false);
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(OnAssemblyResolve);
            }
        }

        /// <summary>
        /// Disposes the resolver, therefore unregistering for the
        /// <see cref="AppDomain.AssemblyResolve"/>.
        /// </summary>
        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(OnAssemblyResolve);
        }
    }
}
