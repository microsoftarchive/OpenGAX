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
using System.Windows.Forms.Design;

namespace Microsoft.Practices.ComponentModel
{
	/// <summary>
	/// Exposes the error information on a form being shown as part of a call 
	/// to <see cref="IUIService.ShowDialog"/> by one of the <see cref="ErrorHelper"/> 
	/// <c>Show</c> method overloads.
	/// </summary>
	/// <remarks>
	/// An <see cref="IUIService"/> can cast the form received in the <see cref="IUIService.ShowDialog"/> 
	/// to this interface to retrieve error information.
	/// </remarks>
	public interface IErrorInfo
    {
        /// <summary>
        /// Caption to use for the error in a dialog.
        /// </summary>
        string Caption { get; }
        /// <summary>
        /// Exception to show.
        /// </summary>
        Exception Exception { get; }
        /// <summary>
        /// Summary message that describes the error.
        /// </summary>
        string Message { get; }
    }
}
