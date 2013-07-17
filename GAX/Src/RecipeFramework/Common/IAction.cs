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

#endregion

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Provides methods for actions that can be executed 
	/// by a recipe.
	/// </summary>
	/// <remarks>
	/// If the action needs access to services exposed by the recipe or any 
	/// of the parent containers, it needs to implement <see cref="System.ComponentModel.IComponent"/>     
    /// (or inherit from <see cref="System.ComponentModel.Component"/> or <see cref="Action"/>) to be 
    /// sited properly before execution.
    /// </remarks>
	public interface IAction
	{
		/// <summary>
		/// Executes the action with the given parameters.
		/// </summary>
		void Execute();

		/// <summary>
		/// Undoes a previous execution operation, using the same set of 
		/// parameters.
		/// </summary>
		void Undo();
	}
}
