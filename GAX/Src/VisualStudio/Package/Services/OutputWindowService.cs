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
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using System.ComponentModel;
using Microsoft.Practices.ComponentModel;
using System.Diagnostics;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Services
{
    /// <summary>
    /// Service that manages logging to the output window.
    /// </summary>
	[ServiceDependency(typeof(DTE))]
	internal class OutputWindowService: SitedComponent, Microsoft.Practices.RecipeFramework.Services.IOutputWindowService
	{
        OutputWindow wnd;
        TraceSwitch traceSwitch;
        TraceListener listener;

        OutputWindowPane Pane
        {
            get
            {               
				if (pane == null)
				{
					DTE dte = (DTE)GetService(typeof(DTE));
					if (dte != null)
					{
						wnd = (OutputWindow)dte.Windows.Item(EnvDTE.Constants.vsWindowKindOutput).Object;
                        foreach (OutputWindowPane windowPane in wnd.OutputWindowPanes)
                        {
                            if (windowPane.Name.Equals(name))
                            {
                                pane = windowPane;
                                break;
                            }
                        }
                        if (pane == null)
                        {
                            pane = wnd.OutputWindowPanes.Add(name);
                            pane.OutputString(Properties.Resources.OutputWindowService_InitializedMsg);
                            pane.OutputString(Environment.NewLine);
                        }
					}
				}
                return pane;
            }
        } OutputWindowPane pane;

        public OutputWindowService()
            : this(Properties.Resources.OutputWindowService_WindowName, RecipeManager.TraceSwitch)
        { }

        protected override void OnSited()
        {
            base.OnSited();

            //this.Display(string.Empty);

            listener = new OutputWindowTraceListener(this, traceSwitch);
            Trace.Listeners.Add(listener);
        }

		public OutputWindowService(string outputWindowName, TraceSwitch traceSwitch)
		{
			this.name = outputWindowName;
            this.traceSwitch = traceSwitch;
        }

        protected override void Dispose(bool disposing)
        {
            if(listener != null)
                Trace.Listeners.Remove(listener);
            base.Dispose(disposing);
        }

        string name;

        /// <summary>
        /// Gets the output window name.
        /// </summary>
        public string WindowName
        {
            get { return name; }
        }

		#region IOutputWindowService Members

		public void Display(string message)
		{
            if (Pane != null)
            {
                Pane.OutputString(message);
            }
		}
		
		#endregion
	}
}
