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
using System.Text;
using Microsoft.Practices.ComponentModel;
using Microsoft.Win32;
using EnvDTE;
using System.IO;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.Actions
{
	/// <summary>
	/// Sets the window Introduction Html to be at the top of the windows
	/// </summary>
    [ServiceDependency(typeof(DTE))]
    public class CloseDocumentWindows : Action
    {
		/// <summary>
		/// Selects the introduction page html to be at the top of the windows
		/// </summary>
        public override void Execute()
        {
            DTE vs = GetService<DTE>(true);
			foreach (Window window in vs.Windows)
            {
				if (window.Kind == "Document")
				{
					window.Close(vsSaveChanges.vsSaveChangesNo);
				}
            }
        }

		/// <summary>
		/// Undo the set of the window not supported
		/// </summary>
        public override void Undo()
        {
        }
    }
}
