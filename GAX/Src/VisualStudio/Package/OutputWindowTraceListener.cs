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
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.ComponentModel;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
	/// <summary>
	/// Logs events to the output window.
	/// </summary>
	internal class OutputWindowTraceListener : TraceListener
	{
		IOutputWindowService outputWindowService;
		TraceSwitch traceSwitch;

		public OutputWindowTraceListener(IOutputWindowService outputWindow, TraceSwitch traceSwitch)
		{
			if (outputWindow == null)
			{
				throw new ArgumentNullException("outputWindow");
			}
			if (traceSwitch == null)
			{
				throw new ArgumentNullException("traceSwitch");
			}
			this.traceSwitch = traceSwitch;
			this.outputWindowService = outputWindow;
		}

		public override void Write(string message, string category)
		{
			base.Write(message, category);
		}

		public override void Write(object o, string category)
		{
			base.Write(o, category);
		}

		public override void WriteLine(object o, string category)
		{
			base.WriteLine(o, category);
		}

		public override void WriteLine(string message, string category)
		{
			base.WriteLine(message, category);
		}

		public override void Write(string message)
		{
			if (traceSwitch.Level == TraceLevel.Off)
			{
				return;
			}
			outputWindowService.Display(message);
		}

		public override void WriteLine(string message)
		{
			if (traceSwitch.Level == TraceLevel.Off)
			{
				return;
			}
            outputWindowService.Display(message);
            outputWindowService.Display(Environment.NewLine);
		}

		public override void TraceEvent(TraceEventCache eventCache, string source,
			TraceEventType severity, int id, string message)
		{
			if (DoNotLog(severity, traceSwitch.Level))
			{
				return;
			}
			WritePrefix(severity);
			base.TraceEvent(eventCache, String.Empty, severity, id, message);
		}

		public override void TraceEvent(TraceEventCache eventCache, string source,
			TraceEventType severity, int id, string format, params object[] args)
		{
			if (DoNotLog(severity, traceSwitch.Level))
			{
				return;
			}
			WritePrefix(severity);
			base.TraceEvent(eventCache, String.Empty, severity, id, format, args);
		}

		public override void TraceData(TraceEventCache eventCache, string source,
			TraceEventType severity, int id, params object[] data)
		{
			if (DoNotLog(severity, traceSwitch.Level))
			{
				return;
			}
			WritePrefix(severity);
			base.TraceData(eventCache, String.Empty, severity, id, data);
		}

		public override void TraceData(TraceEventCache eventCache, string source,
			TraceEventType severity, int id, object data)
		{
			if (DoNotLog(severity, traceSwitch.Level))
			{
				return;
			}
			WritePrefix(severity);
			base.TraceData(eventCache, String.Empty, severity, id, data);
		}

		private void WritePrefix(TraceEventType severity)
		{
			this.Write(new string(' ', this.IndentLevel * this.IndentSize));
			if (severity == TraceEventType.Warning)
			{
				this.Write("(!)");
			}
			else if (severity == TraceEventType.Error)
			{
				this.Write("(*)");
			}
		}

		private bool DoNotLog(TraceEventType severity, TraceLevel level)
		{
			TraceLevel converted;
			switch (severity)
			{
				case TraceEventType.Critical:
				case TraceEventType.Error:
					converted = TraceLevel.Error;
					break;
				case TraceEventType.Warning:
					converted = TraceLevel.Warning;
					break;
				case TraceEventType.Verbose:
					converted = TraceLevel.Verbose;
					break;
				default:
					// Everything else is considered informational.
					converted = TraceLevel.Info;
					break;
			}
			return (int)converted > (int)level;
		}

	}
}
