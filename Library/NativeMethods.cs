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
using System.Globalization;
using System.Runtime.InteropServices;
using ComTypes = System.Runtime.InteropServices.ComTypes;
using Microsoft.VisualStudio;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library
{
    internal class NativeMethods
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

        private const int SizeOfVariant = 0x10;
        private const int DISPID_UNKNOWN = -1;
        private const int DISPID_VALUE = 0;
        private const int DISPID_PROPERTYPUT = -3;
        private const int DISPID_NEWENUM = -4;
        private const int DISPID_EVALUATE = -5;
        private const int DISPID_CONSTRUCTOR = -6;
        private const int DISPID_DESTRUCTOR = -7;
        private const int DISPID_COLLECT = -8;

        private static bool IsSet(ComTypes.INVOKEKIND invokeKind)
        {
            return (invokeKind == ComTypes.INVOKEKIND.INVOKE_PROPERTYPUT || invokeKind == ComTypes.INVOKEKIND.INVOKE_PROPERTYPUTREF);
        }

        public static bool HasDISPID(object dispatch, string funcName, out int dispID)
        {
            return HasDISPID(dispatch as IDispatch, funcName, out dispID, false);
        }

        private static bool HasDISPID(IDispatch dispatch, string funcName, out int dispID,bool throwOnError)
        {
            Guid guid = Guid.Empty;
            string[] names = new string[] { funcName };
            int[] dispIds = new int[] { -1 };
            int hResult = dispatch.GetIDsOfNames(ref guid, names, names.Length, 0, dispIds);
            if (dispIds[0] != -1 && hResult == VSConstants.S_OK )
            {
                dispID = dispIds[0];
                return true;
            }
            if (throwOnError)
            {
                Marshal.ThrowExceptionForHR(hResult);
            }
            dispID = -1;
            return false;
        }

        public static int GetDISPID(IDispatch dispatch, string funcName)
        {
            int dispId = -1;
            HasDISPID(dispatch, funcName, out dispId, true);
            return dispId;
        }

        private static object DynamicInvoke(IDispatch dispatch, string funcName, object[] parameters, ComTypes.INVOKEKIND invokeKind)
        {
            int dispID = GetDISPID(dispatch, funcName);
            return DynamicInvoke(dispatch, dispID, parameters, invokeKind);
        }

        private static object DynamicInvoke(IDispatch dispatch, int dispID, object[] parameters, ComTypes.INVOKEKIND invokeKind)
        {
            Array.Reverse(parameters);
            IntPtr paramsPtr = Marshal.AllocCoTaskMem(parameters.Length * SizeOfVariant);
            ComTypes.DISPPARAMS[] dispParams = new ComTypes.DISPPARAMS[1];
            if (IsSet(invokeKind))
            {
                dispParams[0].cNamedArgs = 1;
                dispParams[0].rgdispidNamedArgs = Marshal.AllocCoTaskMem(SizeOfVariant);
                Marshal.Copy(new int[] { DISPID_PROPERTYPUT }, 0, dispParams[0].rgdispidNamedArgs, 1);
            }
            else
            {
                dispParams[0].cNamedArgs = 0;
                dispParams[0].rgdispidNamedArgs = IntPtr.Zero;
            }
            dispParams[0].cArgs = parameters.Length;
            dispParams[0].rgvarg = paramsPtr;
            try
            {
                int ptr = paramsPtr.ToInt32();
                foreach (object parameter in parameters)
                {
                    Marshal.GetNativeVariantForObject(parameter, new IntPtr(ptr));
                    ptr += SizeOfVariant;
                }
                Guid guid = Guid.Empty;
                object[] result = new object[] { null };
                Marshal.ThrowExceptionForHR(dispatch.Invoke(
                    dispID,
                    ref guid,
                    CultureInfo.CurrentCulture.LCID,
                    (short)invokeKind,
                    dispParams,
                    result,
                    null,
                    null));
                return result[0];
            }
            finally
            {
                if (dispParams[0].rgdispidNamedArgs != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(dispParams[0].rgdispidNamedArgs);
                    dispParams[0].rgdispidNamedArgs = IntPtr.Zero;
                }
                if (dispParams[0].rgvarg != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(dispParams[0].rgvarg);
                    dispParams[0].rgvarg = IntPtr.Zero;
                }
                paramsPtr = IntPtr.Zero;
            }
        }

        public static object InvokeGetProperty(object dispatch, string funcName, object[] parameters)
        {
            return DynamicInvoke(dispatch as IDispatch, funcName, parameters, ComTypes.INVOKEKIND.INVOKE_PROPERTYGET);
        }

        public static object InvokeSetProperty(object dispatch, string funcName, object[] parameters)
        {
            return DynamicInvoke(dispatch as IDispatch, funcName, parameters, ComTypes.INVOKEKIND.INVOKE_PROPERTYPUT);
        }

        public static object InvokeSetRefProperty(object dispatch, string funcName, object[] parameters)
        {
            return DynamicInvoke(dispatch as IDispatch, funcName, parameters, ComTypes.INVOKEKIND.INVOKE_PROPERTYPUTREF);
        }

        public static object InvokeFunc(object dispatch, string funcName, object[] parameters)
        {
            return DynamicInvoke(dispatch as IDispatch, funcName, parameters, ComTypes.INVOKEKIND.INVOKE_FUNC);
        }

        public static object InvokeGetProperty(object dispatch, int dispID, object[] parameters)
        {
            return DynamicInvoke(dispatch as IDispatch, dispID, parameters, ComTypes.INVOKEKIND.INVOKE_PROPERTYGET);
        }

        public static object InvokeSetProperty(object dispatch, int dispID, object[] parameters)
        {
            return DynamicInvoke(dispatch as IDispatch, dispID, parameters, ComTypes.INVOKEKIND.INVOKE_PROPERTYPUT);
        }

        public static object InvokeSetRefProperty(object dispatch, int dispID, object[] parameters)
        {
            return DynamicInvoke(dispatch as IDispatch, dispID, parameters, ComTypes.INVOKEKIND.INVOKE_PROPERTYPUTREF);
        }

        public static object InvokeFunc(object dispatch, int dispID, object[] parameters)
        {
            return DynamicInvoke(dispatch as IDispatch, dispID, parameters, ComTypes.INVOKEKIND.INVOKE_FUNC);
        }
    }
}
