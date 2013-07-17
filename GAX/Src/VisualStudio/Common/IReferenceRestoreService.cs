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
using System.ComponentModel;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Common
{
	/// <summary>
	/// Service that validates references restored when a Visual Studio solution is reopened.
	/// </summary>
	public interface IReferenceRestoreService : IComponent
	{
		/// <summary>
		/// Validates that the references in the currently opened solution are valid. 
		/// This method is called after a solution is opened, but not if during a 
		/// solution template unfolding operation.
		/// </summary>
		void PerformValidation();
	}
}
