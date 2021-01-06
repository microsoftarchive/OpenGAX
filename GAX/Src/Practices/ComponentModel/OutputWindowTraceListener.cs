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
using System.Diagnostics;

#endregion

namespace Microsoft.Practices.ComponentModel
{
	/// <summary>
	/// Logs events to the output window.
	/// </summary>
	internal class OutputWindowTraceListener : TraceListener
	{
		IOutputWindowService outputWindowService;		

		public OutputWindowTraceListener(IOutputWindowService outputWindow)
		{
			if (outputWindow == null)
			{
				throw new ArgumentNullException("outputWindow");
			}
			
			this.outputWindowService = outputWindow;
		}

		public override void Write(string message)
		{			
			outputWindowService.Display(message);
		}

		public override void WriteLine(string message)
		{			
            outputWindowService.Display(message);
            outputWindowService.Display(Environment.NewLine);
		}
	}
}
