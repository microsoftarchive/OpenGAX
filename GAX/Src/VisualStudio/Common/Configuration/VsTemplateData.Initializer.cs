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
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate
{
	/// <summary>
	/// Root of the template extension data in a Visual Studio template file.
	/// </summary>
	public partial class Template
	{
		/// <summary>
		/// Name of the embedded resource that contains the schema for configuration validation.
		/// </summary>
		public const string SchemaResourceName = "Microsoft.Practices.RecipeFramework.VisualStudio.Common.Configuration.VsTemplateData.xsd";
	}
}
