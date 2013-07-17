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

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions
{
    /// <summary>
    /// The action is a "blank" action. The action can be used with recipes that
    /// collect parameters for VS templates and perform no action. 
    /// </summary>
    public class DoNothingAction : Action
    {
        #region Input Properties

        #endregion

        #region Output Propeties
        #endregion

        #region IAction Members

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
		{
		}

        /// <summary>
        /// Undoes the action
        /// </summary>
        public override void Undo()
		{
		}

		#endregion
	}
}
