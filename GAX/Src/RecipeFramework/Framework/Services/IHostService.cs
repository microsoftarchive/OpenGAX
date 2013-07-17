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
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Windows.Forms;

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework.Services
{
    /// <summary>
    /// Required service for hosts using the recipe framework.
    /// </summary>
    /// <remarks>
    /// This service must be added to the <see cref="RecipeManager"/> 
    /// before using it.
    /// <para>
    /// A host notifies the framework about its name (used to filter available 
	/// packages in the package management UI) and provides additional 
	/// information about assets available from the host.
    /// </para>
    /// </remarks>
    public interface IHostService
    {
		/// <summary>
		/// Gets the name of the host.
		/// </summary>
		string HostName { get; }

		/// <summary>
		/// Retrieves host-specific assets that are available for a Guidance Package.
		/// </summary>
		/// <remarks>
		/// Used by the package manager UI to explore features on a package.
		/// </remarks>
        /// <param name="packagePath">The location of the Guidance Package to retrieve the assets for.</param>
        /// <param name="packageConfiguration">The full configuration of the Guidance Package.</param>
        /// <returns>
        /// The list of assets that the specific host supports in addition to the built-in recipe assets. 
        /// This list is used in the Guidance Package Manager to display the assets that are host-specific (such 
        /// as Visual Studio templates).
        /// </returns>
		IAssetDescription[] GetHostAssets(string packagePath, Configuration.GuidancePackage packageConfiguration);

		/// <summary>
		/// Sets the specified target as the selected item in the host.
		/// </summary>
		/// <param name="target">The target item to select.</param>
        /// <returns>Indicates whether the selection was successful.</returns>
		bool SelectTarget(object target);

        /// <summary>
        /// Gives the host the opportunity of opening a selection dialog box to select a target 
        /// suitable for the asset reference.
        /// </summary>
        /// <param name="ownerWindow">Window to use as the parent of the dialog.</param>
        /// <param name="forReference">The refence that will be used to execute against the selected target.</param>
        /// <returns>Whether the selection was successful.</returns>
        bool SelectTarget(IWin32Window ownerWindow, IUnboundAssetReference forReference);
    }
}
