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
using Microsoft.Practices.RecipeFramework.Services;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Common
{
    /// <summary>
    /// Service exposed by the RecipeManagerPackage
    /// </summary>
    public interface IVsTemplatesService
    {
        /// <summary>
        /// Gets the IVsTemplate interface from the templateFileName
        /// </summary>
        /// <param name="templateFileName"></param>
        /// <returns></returns>
        IVsTemplate GetTemplate(string templateFileName);

        /// <summary>
        /// Gets the current executing IVsTemplate
        /// </summary>
        /// <returns></returns>
        IVsTemplate GetCurrentTemplate();

        /// <summary>
        /// Gets the IVsTemplate interface from the Guid of the package and the id of the template
        /// </summary>
        /// <param name="package"></param>
        /// <param name="iTemplate"></param>
        /// <returns></returns>
        IVsTemplate GetTemplate(Guid package, int iTemplate);

        /// <summary>
        /// Registers the templates with a package and assign it a command id
        /// </summary>
        /// <param name="basePath">The base path.</param>
        /// <param name="guidancePackage">The guidance package.</param>
        /// <returns></returns>
        IAssetDescription[] RegisterTemplates(string basePath, Configuration.GuidancePackage guidancePackage);

        /// <summary>
        /// Unregister every template from the package
        /// </summary>
        /// <param name="basePath">The base path.</param>
        /// <param name="guidancePackage">The guidance package.</param>
        void UnregisterTemplates(string basePath, Configuration.GuidancePackage guidancePackage);

        /// <summary>
        /// Retrieves the templates exposed by a VS package.
        /// </summary>
        IAssetDescription[] GetHostAssets(string basePath);

    }
}
