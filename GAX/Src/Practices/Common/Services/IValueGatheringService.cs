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
using System.Xml;
using Microsoft.Practices.Common;

#endregion

namespace Microsoft.Practices.Common.Services
{
	/// <summary>
	/// Service that provides value gathering strategy (such as arguments for a wizard).
	/// </summary>
    public interface IValueGatheringService
	{
		/// <summary>
		/// Starts the gathering process.
		/// </summary>
		/// <param name="serviceData">
		/// Specifies the custom data used by the service
		/// </param>
        /// <param name="allowSuspend">
        /// Specifies whether the gathering can be suspended (if supported).
        /// </param>
        /// <returns>The result of the process.</returns>
		ExecutionResult Execute(XmlElement serviceData, bool allowSuspend);
	}
}
