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
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.Practices.RecipeFramework
{
    internal class Constants
    {
        #region XPath Expressions

        internal const string XPathHosts = "gax:RecipeFramework/gax:Hosts";
        internal const string XPathHostByName = "gax:RecipeFramework/gax:Hosts/gax:Host[@Name=$name]";
        internal const string XPathPackage = "gax:RecipeFramework/gax:GuidancePackages";
        // UNDONE: checks for package name + version.
        //const string XPathPackageByNameAndVersion = "gax:RecipeFramework/gax:GuidancePackages/gax:GuidancePackage[@Name=$name and @Version=$version]";
        internal const string XPathPackageByName = "gax:RecipeFramework/gax:GuidancePackages/gax:GuidancePackage[@Name=$name]";
        internal const string XPathPackageByGuid = "gax:RecipeFramework/gax:GuidancePackages/gax:GuidancePackage[@Guid=$guid]";
        internal const string XPathPackageNamesByHost = "gax:RecipeFramework/gax:GuidancePackages/gax:GuidancePackage[@Host=$host]/@Name";

        #endregion XPath Expressions

        internal const string RegistryGAXRoot = @"SOFTWARE\Microsoft\Guidance Automation Extensions";
        internal const string RegistryVisualStudioRoot = @"Software\Microsoft\VisualStudio\{0}";
        internal const string RegistryVisualStudioGuidancePackages = @"AutoLoadPackages\" + UIContextGuids.SolutionExists + @"\Packages";
    }
}
