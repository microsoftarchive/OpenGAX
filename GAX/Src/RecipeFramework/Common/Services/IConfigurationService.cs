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
using System.Collections.Generic;

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework.Services
{
	/// <summary>
	/// Provides access to the GuidancePackage configuration data.
	/// </summary>
	public interface IConfigurationService
	{
		/// <summary>
		/// Gets the current configuration of the current recipe package.
		/// </summary>
		Configuration.GuidancePackage CurrentPackage { get; }
		/// <summary>
		/// Gets the current configuration for the currently executing recipe.
		/// </summary>
		/// <remarks>
		/// If no recipe is being executed, it will return <see langword="null"/>.
		/// </remarks>
		Configuration.Recipe CurrentRecipe { get; }
		/// <summary>
		/// Gets the gathering service data configuration associated with the currently executing recipe, 
		/// or <see langword="null"/> if there is no current recipe or if the current recipe does not contain 
		/// gathering data. 
		/// </summary>
		object CurrentGatheringServiceData { get; }
		/// <summary>
		/// The path where the package is installed, and where the configuration was loaded from.
		/// </summary>
		string BasePath { get; }
	}
}
