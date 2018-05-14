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


using Microsoft.Practices.ComponentModel;


namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Represents the abstract base class for actions 
	/// that need to access services from the host.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public abstract class Action : SitedComponent, IAction
	{
		#region IAction Members

		/// <summary>
		/// See <see cref="IAction.Execute"/>.
		/// </summary>
		public abstract void Execute();

		/// <summary>
		/// See <see cref="IAction.Undo"/>.
		/// </summary>
		public abstract void Undo();

		#endregion
    }
}
