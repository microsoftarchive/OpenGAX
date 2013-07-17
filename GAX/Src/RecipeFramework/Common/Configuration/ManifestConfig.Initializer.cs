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

namespace Microsoft.Practices.RecipeFramework.Configuration.Manifest
{
	/// <summary>
	/// Root of the configuration hierarchy for guidance packages.
	/// </summary>
	public partial class RecipeFramework
	{
		/// <summary>
		/// Name of the embedded resource that contains the schema for configuration validation.
		/// </summary>
		public const string SchemaResourceName = "Microsoft.Practices.RecipeFramework.Configuration.ManifestConfig.xsd";
	}
}
