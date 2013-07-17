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

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Common
{
	/// <summary>
	/// Specifies whether the location field in the Add New Dialog Box is enabled, disabled or hidden.
	/// </summary>
	public enum LocationField
	{
		/// <summary>
		/// Location field is enabled
		/// </summary>
		Enabled,
		/// <summary>
		/// Location field is disabled
		/// </summary>
		Disabled,
		/// <summary>
		/// Location field is hidden
		/// </summary>
		Hidden
	}
}
