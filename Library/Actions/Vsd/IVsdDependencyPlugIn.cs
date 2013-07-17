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

#region Using Directives

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions.Vsd
{
    [ComImport, Guid("8CA3273D-A907-4E4A-98BC-CF3B292FA3DE"), TypeLibType((short)4160)]
    internal interface IVsdDependencyPlugIn
    {
        [DispId(0x436)]
        IVsdCollection Items { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x436)] get; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x437)]
        void Clear();
        [DispId(1080)]
        bool HasFrameworkDependency { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1080)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1080)] set; }
    }
}
