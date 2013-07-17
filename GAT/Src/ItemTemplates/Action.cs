//===================================================================================
// Microsoft patterns & practices
// Guidance Automation Toolkit
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// License: MS-LPL
//===================================================================================

#region Using Directives

using System;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;

#endregion

namespace $rootnamespace$
{
	class $ActionName$Action: Action
    {
        #region Input Properties

        [Input]
        public string RecipeArgument1
        {
            get { return recipeArgument1; }
            set { recipeArgument1 = value; }
        } string recipeArgument1;

        #endregion

        #region Output Properties

        [Output]
        public string ActionOutput1
        {
            get { return actionOutput1; }
            set { actionOutput1 = value; }
        } string actionOutput1;

        #endregion

        #region IAction Members

        public override void Execute()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Undo()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
