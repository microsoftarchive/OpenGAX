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
using ComTypes = System.Runtime.InteropServices.ComTypes;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions.Vsd
{
    internal sealed class NativeMethods
    {
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00020400-0000-0000-C000-000000000046")]
        public interface IDispatch
        {
            int GetTypeInfoCount();
            [return: MarshalAs(UnmanagedType.Interface)]
            ComTypes.ITypeInfo GetTypeInfo([In, MarshalAs(UnmanagedType.U4)] int iTInfo, [In, MarshalAs(UnmanagedType.U4)] int lcid);
            [PreserveSig]
            int GetIDsOfNames([In] ref Guid riid, [In, MarshalAs(UnmanagedType.LPArray)] string[] rgszNames, [In, MarshalAs(UnmanagedType.U4)] int cNames, [In, MarshalAs(UnmanagedType.U4)] int lcid, [Out, MarshalAs(UnmanagedType.LPArray)] int[] rgDispId);
            [PreserveSig]
            int Invoke(int dispIdMember, [In] ref Guid riid, [In, MarshalAs(UnmanagedType.U4)] int lcid, [In, MarshalAs(UnmanagedType.U2)]short dwFlags, [In, Out, MarshalAs(UnmanagedType.LPArray)] ComTypes.DISPPARAMS[] pDispParams, [Out, MarshalAs(UnmanagedType.LPArray)] object[] pVarResult, [In, Out, MarshalAs(UnmanagedType.LPArray)] ComTypes.EXCEPINFO[] pExcepInfo, [Out, MarshalAs(UnmanagedType.LPArray)] IntPtr[] pArgErr);
        }
    }
}
