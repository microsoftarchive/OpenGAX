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

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using System.Diagnostics;

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.Actions
{
	/// <summary>
	/// Activates the VS output window
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	public class ActivateOutputWindowAction : Action
	{
		/// <summary>
		/// Does nothing, as un-registration must be done explicitly.
		/// </summary>
		public override void Undo()
		{
			// Must un-register to undo.
		}

		/// <summary>
		/// Executes the InstallUtil.exe utility to register the package with the framework.
		/// </summary>
		public override void Execute()
		{
			var dte = this.GetService<EnvDTE.DTE>();

			if (dte != null && dte.Windows != null)
			{
				var outputWindow = dte.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);

				if (outputWindow != null)
					outputWindow.Activate();
			}
		}
	}
}