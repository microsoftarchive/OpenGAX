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
using System.Xml;

namespace Microsoft.Practices.RecipeFramework.Services
{
    /// <summary>
    /// Service that provides execution of Actions of a Recipe
    /// </summary>
	public interface IActionExecutionService
    {
        /// <summary>
        /// Executes the action with the given name.
        /// </summary>
		/// <param name="actionName">The name of the action to execute.</param>
        void Execute(string actionName);

		/// <summary>
		/// Executes the action with the given name.
		/// </summary>
		/// <param name="actionName">The name of the action to execute.</param>
		/// <param name="inputValues">Additional values to pass for action execution.</param>
		void Execute(string actionName, Dictionary<string, object> inputValues);
	}
}
