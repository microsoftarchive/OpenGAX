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

#endregion

namespace Microsoft.Practices.Common
{
	/// <summary>
	/// The result of an execution.
	/// </summary>
	public enum ExecutionResult
	{
		/// <summary>
		/// The process was cancelled.
		/// </summary>
		Cancel,
		/// <summary>
		/// The process finished successfully.
		/// </summary>
		Finish,
        /// <summary>
        /// The process was suspended.
        /// </summary>
        Suspend,
        /// <summary>
        /// The execution failed with an error.
        /// </summary>
        Failed
	}
}
