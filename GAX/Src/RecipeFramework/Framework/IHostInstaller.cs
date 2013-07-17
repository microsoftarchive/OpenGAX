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
using System.Configuration.Install;

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework
{
    /// <summary>
    /// Provides installation services for a Recipe Framework host.
    /// </summary>
    public interface IHostInstaller
    {
		/// <summary>
		/// Installs the host.
		/// </summary>
		/// <param name="context">The installation context and parameters.</param>
		void InstallHost(InstallContext context);
		/// <summary>
		/// Uninstalls the host.
		/// </summary>
		/// <param name="context">The installation context and parameters.</param>
		void UninstallHost(InstallContext context);
        /// <summary>
        /// Installs a package on a host.
        /// </summary>
		/// <param name="context">The installation context and parameters.</param>
        /// <param name="packageConfig">Package configuration.</param>
		void InstallPackage(InstallContext context, Configuration.GuidancePackage packageConfig);
        /// <summary>
        /// Uninstalls a package on a host.
        /// </summary>
		/// <param name="context">The installation context and parameters.</param>
		/// <param name="packageConfig">Package configuration.</param>
		void UninstallPackage(InstallContext context, Configuration.GuidancePackage packageConfig);
    }
}
