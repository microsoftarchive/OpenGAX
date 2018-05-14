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
using EnvDTE;
using System.Diagnostics;

#endregion


namespace Microsoft.Practices.ComponentModel
{
	/// <summary>
	/// Service that manages logging to the output window.
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	internal class OutputWindowService: SitedComponent, IOutputWindowService
	{
        SourceSwitch sourceSwitch;
		TraceSource traceSource;
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
						OutputWindow wnd;

						wnd = (OutputWindow)dte.Windows.Item(Constants.vsWindowKindOutput).Object;
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

		public OutputWindowService(string outputWindowName, SourceSwitch sourceSwitch)
		{
			this.name = outputWindowName;
            this.sourceSwitch = sourceSwitch;

			this.traceSource = new TraceSource(outputWindowName);
			this.traceSource.Switch = sourceSwitch;
			TraceUtil.traceSources.Add(this.name, this.traceSource);
		}

		protected override void OnSited()
		{
			base.OnSited();

			//this.Display(string.Empty);

			listener = new OutputWindowTraceListener(this);
			this.traceSource.Listeners.Add(listener);
		}

		public TraceSource TraceSource {
			get
			{
				return traceSource;
			}
		}

		protected override void Dispose(bool disposing)
        {
            if(listener != null)
				traceSource.Listeners.Remove(listener);

			TraceUtil.traceSources.Remove(this.name);

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
