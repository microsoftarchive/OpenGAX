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
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Microsoft.Practices.RecipeFramework.Services
{
    /// <summary>
    /// Provides custom coordination of action execution.
    /// </summary>
    public interface IActionCoordinationService
    {
        /// <summary>
        /// Runs the coordination using the configuration data specified in the configuration file.
        /// </summary>
		/// <param name="declaredActions">Actions defined in the package configuration file for the currently executing recipe.</param>
		/// <param name="coordinationData">The configuration data used to setup the coordination.</param>
        void Run(Dictionary<string, Configuration.Action> declaredActions, XmlElement coordinationData);
    }
}
