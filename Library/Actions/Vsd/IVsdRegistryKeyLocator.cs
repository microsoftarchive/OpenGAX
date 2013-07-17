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
    [Guid("B54FD1E3-2685-4172-8ECC-723DAF25B690")]
    internal enum vsdRegistryRoot
    {
        // Fields
        vsdrrHKCR = 3,
        vsdrrHKCU = 2,
        vsdrrHKLM = 1,
        vsdrrHKU = 4
    }
    [ComImport, Guid("CD6827E8-B3AF-4E76-93B3-973728AB29CE"), TypeLibType((short)4160)]
    internal interface IVsdRegistryKeyLocator
    {
        [DispId(0x3cb)]
        IVsdManagedCollection Collection { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3cb)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3cb)] set; }
        [DispId(0x3cc)]
        string Key { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3cc)] get; }
        [DispId(0x3cd)]
        string Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3cd)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3cd)] set; }
        [DispId(0x3ce)]
        vsdRegistryRoot Root { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ce)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ce)] set; }
        [DispId(0x3cf)]
        string RegKey { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3cf)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3cf)] set; }
        [DispId(0x3d0)]
        string Value { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3d0)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3d0)] set; }
        [DispId(0x3d1)]
        string Property { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3d1)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3d1)] set; }
    }
    [ComImport, Guid("CD6827E8-B3AF-4E76-93B3-973728AB29CE"), CoClass(typeof(VsdRegistryKeyLocatorClass))]
    internal interface VsdRegistryKeyLocator : IVsdRegistryKeyLocator
    {
    }
    [ComImport, Guid("CF66B7F7-B7DA-4B08-A67A-233430638C9C"), TypeLibType((short)2), ClassInterface((short)0)]
    internal class VsdRegistryKeyLocatorClass : IVsdRegistryKeyLocator, VsdRegistryKeyLocator
    {
        // Properties
        [DispId(0x3cb)]
        public extern IVsdManagedCollection Collection { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3cb)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3cb)] set; }
        [DispId(0x3cc)]
        public extern string Key { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3cc)] get; }
        [DispId(0x3cd)]
        public extern string Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3cd)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3cd)] set; }
        [DispId(0x3d1)]
        public extern string Property { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3d1)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3d1)] set; }
        [DispId(0x3cf)]
        public extern string RegKey { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3cf)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3cf)] set; }
        [DispId(0x3ce)]
        public extern vsdRegistryRoot Root { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ce)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ce)] set; }
        [DispId(0x3d0)]
        public extern string Value { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3d0)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3d0)] set; }
    }
}
