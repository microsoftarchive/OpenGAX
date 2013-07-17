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
    [ComImport, TypeLibType((short)4160), Guid("29B1A1BA-3B1D-4F94-9640-54C3AD9EB7BA")]
    internal interface IVsdLaunchConditionPlugIn
    {
        [DispId(0x392)]
        IVsdManagedCollection Items { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x392)] get; }
    }
}
