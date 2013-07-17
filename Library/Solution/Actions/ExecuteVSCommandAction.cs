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

#region Using Directives

using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using EnvDTE;
using VSLangProj;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.ComponentModel;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Solution.Actions
{
    /// <summary>
    /// Executes a Visual Studio Command
    /// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ServiceDependency(typeof(DTE))]
    public class ExecuteVSCommandAction : Action
	{
		#region Inputs

        /// <summary>
        /// The Visual Studio command to execute
        /// </summary>
		[Input(Required=true)]
		public string Command
		{
            get { return command; }
            set { command = value; }
        } string command;


        /// <summary>
        /// Optional arguments of the command
        /// </summary>
        [Input(Required=false)]
        public string CommandArgs
        {
            get { return commandArgs; }
            set { commandArgs = value; }
        } string commandArgs = string.Empty;

		#endregion

        /// <summary>
        /// Executes the command
        /// </summary>
		public override void Execute()
		{
            DTE dte = (DTE)GetService(typeof(DTE));
            dte.ExecuteCommand(this.Command, this.CommandArgs);
		}

        /// <summary>
        /// Not supported
        /// </summary>
		public override void Undo()
		{
			// No undo supported
		}
    }
}

