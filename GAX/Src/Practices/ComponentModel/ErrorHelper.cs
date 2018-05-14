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

using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Diagnostics;

namespace Microsoft.Practices.ComponentModel
{
	/// <summary>
	/// Generic helper class used to display an error to the user
	/// </summary>
	public static class ErrorHelper
	{
		internal static DialogResult ShowInternal(IUIService uiService, Exception exception, string textMessage, 
			string caption, MessageBoxButtons buttons, TraceSource traceSource = null)
		{
			if (traceSource == null)
				traceSource = TraceUtil.GaxTraceSource;
			try
			{
				if (exception == null)
				{
					if (uiService != null)
					{
						return uiService.ShowMessage(textMessage,caption,buttons);
					}
					else
					{
						return MessageBox.Show(textMessage,caption,buttons);
					}
				}
				else
				{
					if (string.IsNullOrEmpty(textMessage))
					{
						textMessage = exception.Message;
					}
					traceSource.TraceEvent(TraceEventType.Error, 0, exception.ToString());
                    using (ErrorForm errorForm = new ErrorForm(caption, textMessage, exception, buttons))
                    {
                        if (uiService != null)
                        {
                            return uiService.ShowDialog(errorForm);
                        }
                        else
                        {
                            return errorForm.ShowDialog();
                        }
                    }
				}
			}
			catch (Exception e)
			{
				traceSource.TraceEvent(TraceEventType.Error, 0, e.ToString());
				// Let the implementer of the IUIService decide if they REALLY want the exception to propagate!
				throw;
			}
		}
		
		#region Show methods

		/// <summary>
		/// It asks the user for a description specified in the textMessage and detailed in questionDetails
		/// </summary>
		/// <param name="caption"></param>
		/// <param name="textMessage"></param>
		/// <param name="questionDetails"></param>
		/// <returns></returns>
		public static DialogResult Ask(string caption, string textMessage, string questionDetails)
		{
            using (ErrorForm errorForm = new ErrorForm(caption, textMessage, questionDetails,
                Properties.Resources.ErrorHelper_Yes, Properties.Resources.ErrorHelper_No))
            {
                return errorForm.ShowDialog();
            }
		}

		/// <summary>
		/// It asks the user for a description specified in the textMessage and detailed in questionDetails
		/// </summary>
		public static DialogResult Ask(string caption, string textMessage, string questionDetails, string yesString, string noString)
		{
            using (ErrorForm errorForm = new ErrorForm(caption, textMessage, questionDetails,
                yesString, noString))
            {
                return errorForm.ShowDialog();
            }
		}

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="uiService"></param>
		/// <param name="exception"></param>
		/// <param name="textMessage"></param>
		/// <param name="caption"></param>
		/// <param name="buttons"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public static DialogResult Show(IUIService uiService, Exception exception, string textMessage, string caption, 
			MessageBoxButtons buttons, TraceSource t = null)
		{
			return ShowInternal(uiService, exception, textMessage, caption, buttons, t);
		}

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="exception"></param>
		/// <param name="textMessage"></param>
		/// <param name="caption"></param>
		/// <param name="buttons"></param>
		/// <returns></returns>
		public static DialogResult Show(IServiceProvider provider, Exception exception, string textMessage, string caption, MessageBoxButtons buttons)
		{
			IUIService uiService = null;
			TraceSource t = TraceUtil.GaxTraceSource;

			if (provider != null)
			{
				uiService = (IUIService)provider.GetService(typeof(IUIService));
				IOutputWindowService io = (IOutputWindowService)provider.GetService(typeof(IOutputWindowService));
				if (io != null)
					t = io.TraceSource;
			}

			return Show(uiService, exception, textMessage, caption, buttons, t);
		}

		#region Using IServiceProvider 

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="exception"></param>
		/// <param name="textMessage"></param>
		/// <param name="buttons"></param>
		/// <returns></returns>
		public static DialogResult Show(IServiceProvider provider, Exception exception, string textMessage, MessageBoxButtons buttons)
		{
			return Show(provider, exception, textMessage, Properties.Resources.ErrorHelper_Caption, buttons);
		}

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="exception"></param>
		/// <param name="textMessage"></param>
		/// <param name="caption"></param>
		/// <returns></returns>
		public static DialogResult Show(IServiceProvider provider, Exception exception, string textMessage, string caption)
		{
			return Show(provider, exception, textMessage, caption, MessageBoxButtons.OK);
		}

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="exception"></param>
		/// <param name="textMessage"></param>
		/// <returns></returns>
		public static DialogResult Show(IServiceProvider provider, Exception exception, string textMessage)
		{
			return Show(provider, exception, textMessage, Properties.Resources.ErrorHelper_Caption, MessageBoxButtons.OK);
		}

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="exception"></param>
		/// <param name="buttons"></param>
		/// <returns></returns>
		public static DialogResult Show(IServiceProvider provider, Exception exception, MessageBoxButtons buttons)
		{
			return Show(provider, exception, "", Properties.Resources.ErrorHelper_Caption, buttons);
		}

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="exception"></param>
		/// <returns></returns>
		public static DialogResult Show(IServiceProvider provider, Exception exception)
		{
			return Show(provider, exception, "", Properties.Resources.ErrorHelper_Caption, MessageBoxButtons.OK);
		}

		#endregion

		#region Using IUIService

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="uiService"></param>
		/// <param name="exception"></param>
		/// <param name="textMessage"></param>
		/// <param name="buttons"></param>
		/// <returns></returns>
		public static DialogResult Show(IUIService uiService, Exception exception, string textMessage, MessageBoxButtons buttons)
		{
			return Show(uiService, exception, textMessage, Properties.Resources.ErrorHelper_Caption, buttons);
		}

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="uiService"></param>
		/// <param name="exception"></param>
		/// <param name="textMessage"></param>
		/// <param name="caption"></param>
		/// <returns></returns>
		public static DialogResult Show(IUIService uiService, Exception exception, string textMessage, string caption)
		{
			return Show(uiService, exception, textMessage, caption, MessageBoxButtons.OK);
		}

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="uiService"></param>
		/// <param name="exception"></param>
		/// <param name="textMessage"></param>
		/// <returns></returns>
		public static DialogResult Show(IUIService uiService, Exception exception, string textMessage)
		{
			return Show(uiService, exception, textMessage, Properties.Resources.ErrorHelper_Caption, MessageBoxButtons.OK);
		}

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="uiService"></param>
		/// <param name="exception"></param>
		/// <param name="buttons"></param>
		/// <returns></returns>
		public static DialogResult Show(IUIService uiService, Exception exception, MessageBoxButtons buttons)
		{
			return Show(uiService, exception, "", Properties.Resources.ErrorHelper_Caption, buttons);
		}

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="uiService"></param>
		/// <param name="exception"></param>
		/// <returns></returns>
		public static DialogResult Show(IUIService uiService, Exception exception)
		{
			return Show(uiService, exception, "", Properties.Resources.ErrorHelper_Caption, MessageBoxButtons.OK);
		}

		#endregion

		#region Not using ServiceProvider not IUIService

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="textMessage"></param>
		/// <param name="buttons"></param>
		/// <returns></returns>
		public static DialogResult Show(Exception exception, string textMessage, MessageBoxButtons buttons)
		{
			return ShowInternal(null, exception, textMessage, Properties.Resources.ErrorHelper_Caption, buttons);
		}

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="textMessage"></param>
		/// <param name="caption"></param>
		/// <returns></returns>
		public static DialogResult Show(Exception exception, string textMessage, string caption)
		{
			return ShowInternal(null, exception, textMessage, caption, MessageBoxButtons.OK);
		}

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="textMessage"></param>
		/// <returns></returns>
		public static DialogResult Show(Exception exception, string textMessage)
		{
			return ShowInternal(null, exception, textMessage, Properties.Resources.ErrorHelper_Caption, MessageBoxButtons.OK);
		}

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="buttons"></param>
		/// <returns></returns>
		public static DialogResult Show(Exception exception, MessageBoxButtons buttons)
		{
			return ShowInternal(null, exception, "", Properties.Resources.ErrorHelper_Caption, buttons);
		}

		/// <summary>
		/// Shows an error to the user
		/// </summary>
		/// <param name="exception"></param>
		/// <returns></returns>
		public static DialogResult Show(Exception exception)
		{
			return ShowInternal(null, exception, "", Properties.Resources.ErrorHelper_Caption, MessageBoxButtons.OK);
		}

		#endregion

		#region Not using Exception

		/// <summary>
		/// Shows a message to the user
		/// </summary>
		/// <param name="textMessage"></param>
		/// <returns></returns>
		public static DialogResult Show(string textMessage)
		{
			return ShowInternal(null, null, textMessage, Properties.Resources.ErrorHelper_Caption, MessageBoxButtons.OK);
		}

		/// <summary>
		///  Shows a message to the user
		/// </summary>
		/// <param name="service"></param>
		/// <param name="textMessage"></param>
		/// <returns></returns>
		public static DialogResult Show(IUIService service, string textMessage)
		{
			return ShowInternal(service, null, textMessage, Properties.Resources.ErrorHelper_Caption, MessageBoxButtons.OK);
		}

		/// <summary>
		/// Shows a message to the user
		/// </summary>
		/// <param name="textMessage"></param>
		/// <param name="buttons"></param>
		/// <returns></returns>
		public static DialogResult Show(string textMessage, MessageBoxButtons buttons)
		{
			return ShowInternal(null, null, textMessage, Properties.Resources.ErrorHelper_Caption, buttons);
		}

		/// <summary>
		/// Shows a message to the user
		/// </summary>
		/// <param name="textMessage"></param>
		/// <param name="caption"></param>
		/// <returns></returns>
		public static DialogResult Show(string textMessage, string caption)
		{
			return ShowInternal(null, null, textMessage, caption, MessageBoxButtons.OK);
		}

		/// <summary>
		/// Shows a message to the user
		/// </summary>
		/// <param name="textMessage"></param>
		/// <param name="caption"></param>
		/// <param name="buttons"></param>
		/// <returns></returns>
		public static DialogResult Show(string textMessage, string caption, MessageBoxButtons buttons)
		{
			return ShowInternal(null, null, textMessage, caption, buttons);
		}

		/// <summary>
		/// Shows a message to the user
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="textMessage"></param>
		/// <returns></returns>
		public static DialogResult Show(IServiceProvider provider, string textMessage)
		{
			return Show(provider, null, textMessage, Properties.Resources.ErrorHelper_Caption, MessageBoxButtons.OK);
		}

		/// <summary>
		/// Shows a message to the user
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="textMessage"></param>
		/// <param name="buttons"></param>
		/// <returns></returns>
		public static DialogResult Show(IServiceProvider provider, string textMessage, MessageBoxButtons buttons)
		{
			return Show(provider, null, textMessage, Properties.Resources.ErrorHelper_Caption, buttons);
		}

		/// <summary>
		/// Shows a message to the user
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="textMessage"></param>
		/// <param name="caption"></param>
		/// <returns></returns>
		public static DialogResult Show(IServiceProvider provider, string textMessage, string caption)
		{
			return Show(provider, null, textMessage, caption, MessageBoxButtons.OK);
		}

		/// <summary>
		/// Shows a message to the user
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="textMessage"></param>
		/// <param name="caption"></param>
		/// <param name="buttons"></param>
		/// <returns></returns>
		public static DialogResult Show(IServiceProvider provider, string textMessage, string caption, MessageBoxButtons buttons)
		{
			return Show(provider, null, textMessage, caption, buttons);
		}

		#endregion

		#endregion
	}
}
