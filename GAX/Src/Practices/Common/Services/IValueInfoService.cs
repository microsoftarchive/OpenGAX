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

namespace Microsoft.Practices.Common.Services
{
    /// <summary>
    /// Service to provide metadata about a value.
    /// </summary>
	public interface IValueInfoService
	{
        /// <summary>
        /// Retrieve metadata about a value.
        /// </summary>
        /// <param name="valueName">Name of the value to query for metadata.</param>
        /// <returns>The meta information about the value, or <see langword="null"/> if it doesn't exist.</returns>
        ValueInfo GetInfo(string valueName);
        /// <summary>
        /// Name or description of the container that is providing the service.
        /// </summary>
        /// <remarks>
        /// Can be used when the service is hosted by a component that provides a caption 
        /// or description, to be shown in a UI as part of the exploration or gathering 
        /// of the values the component uses.
        /// </remarks>
        string ComponentName { get; }
	}
}
