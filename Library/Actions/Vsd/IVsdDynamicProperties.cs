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
using System.Runtime.InteropServices.CustomMarshalers;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions.Vsd
{
    [ComImport, TypeLibType((short)4160), Guid("066D42A5-A55D-443E-B95A-47C2DF6F5CD6")]
    internal interface IVsdDynamicProperty
    {
        [DispId(0x6a)]
        string Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6a)] get; }
        [DispId(0x6b)]
        string DocString { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6b)] get; }
        [DispId(0x6c)]
        ushort VarType { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6c)] get; }
        [DispId(0x6d)]
        string DisplayString { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6d)] get; }
        [DispId(110)]
        string ComboStrings { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(110)] get; }
        [DispId(0x6f)]
        int State { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6f)] get; }
        [DispId(0x70)]
        Type RefTypeInfo { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(TypeToTypeInfoMarshaler), MarshalCookie = "")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x70)] get; }
        [DispId(0x71)]
        object Value { [return: MarshalAs(UnmanagedType.Struct)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x71)] get; [param: In, MarshalAs(UnmanagedType.Struct)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x71)] set; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x72)]
        void ShowBuilder([MarshalAs(UnmanagedType.Struct)] out object pvResult);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x73)]
        void Reset();
    }
    [ComImport, TypeLibType((short)4160), Guid("B844A007-A301-4BB3-8649-EF9E7804C013")]
    internal interface IVsdDynamicProperties
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7b)]
        void PutName([In, MarshalAs(UnmanagedType.BStr)] string Name);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7c)]
        int Count();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7d)]
        IVsdDynamicProperties Clone();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7e)]
        void AddProperty([In, MarshalAs(UnmanagedType.BStr)] string Key, [In, MarshalAs(UnmanagedType.Interface)] IVsdDynamicProperty Val);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7f)]
        void UpdateProperty([In, MarshalAs(UnmanagedType.BStr)] string Key, [In, MarshalAs(UnmanagedType.Interface)] IVsdDynamicProperty Val);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x80)]
        void RemoveProperty([In, MarshalAs(UnmanagedType.BStr)] string Key);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x81)]
        void ClearProperties();
    }
}
