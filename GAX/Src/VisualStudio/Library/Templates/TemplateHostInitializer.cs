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
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates
{
    /// <summary>
    /// Provides access to the Host instance from the TextTemplate application domain.
    /// </summary>
	public class TemplateHostInitializer
    {
		/// <summary>
		/// Causes the current host on the current AppDomain to be set to 
		/// the received one for static access from the template.
		/// </summary>
        public TemplateHostInitializer(TemplateHost host)
        {
            this.SetCurrentHost(host);
        }

        /// <summary>
        /// In the constructor sets the host instance into a static object.
        /// </summary>
        /// <param name="host">The host to set as current.</param>
        public void SetCurrentHost(TemplateHost host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            TemplateHost.CurrentHost = host;
        }
    }
}
