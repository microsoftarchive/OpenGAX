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
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices.CustomMarshalers;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions.Vsd
{
    [ComImport, Guid("4A6CD477-BE65-488E-9528-D71BAD931380"), DefaultMember("Item"), TypeLibType((short)4160)]
    internal interface IVsdCollection : IEnumerable
    {
        [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
        object Add([In, MarshalAs(UnmanagedType.BStr)] string Key, [In, MarshalAs(UnmanagedType.IDispatch)] object Object);
        [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        object Item([In, MarshalAs(UnmanagedType.BStr)] string Key);
        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
        string KeyOfObject([In, MarshalAs(UnmanagedType.IDispatch)] object Object);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
        void Remove([In, MarshalAs(UnmanagedType.BStr)] string Key);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)]
        void RemoveObject([In, MarshalAs(UnmanagedType.IDispatch)] object Object);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
        IVsdCollectionSubset Subset([In, Optional, MarshalAs(UnmanagedType.Interface)] object Filter);
        [DispId(8)]
        int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)] get; }
        [DispId(10)]
        IVsdGetNewEnum Keys { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short)0x40), DispId(10)] get; }
        //[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        //[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4), TypeLibFunc((short)0x40)]
        //IEnumerator GetEnumerator();
    }
}
