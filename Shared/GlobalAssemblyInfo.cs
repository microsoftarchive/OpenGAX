//===================================================================================
// Microsoft patterns & practices
// Guidance Automation Toolkit
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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

[assembly: ComVisible(false)]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyCopyright("Copyright© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyProduct("Microsoft® Guidance Automation Runtime")]


/// <summary>
/// Provides easy access to global assembly-level attributes.
/// </summary>
internal sealed class ThisGlobalAssembly
{
    private ThisGlobalAssembly()
    {
    }

    public const string Company = "Microsoft";
#if DEBUG
    public const AssemblyConfiguration Configuration = AssemblyConfiguration.Debug;
#else
    public const AssemblyConfiguration Configuration = AssemblyConfiguration.Release;
#endif
    internal enum AssemblyConfiguration
    {
        Debug,
        Release        
    }
}