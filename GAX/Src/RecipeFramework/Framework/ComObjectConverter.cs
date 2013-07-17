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
using System.ComponentModel;
using System.Runtime.InteropServices;
using ComTypes = System.Runtime.InteropServices.ComTypes;
using System.Globalization;
using System.Reflection;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
    internal class ComObjectConverter: TypeConverter
    {
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00020400-0000-0000-C000-000000000046")]
        private interface IDispatch
        {
            int GetTypeInfoCount();
            [return: MarshalAs(UnmanagedType.Interface)]
            ComTypes.ITypeInfo GetTypeInfo([In, MarshalAs(UnmanagedType.U4)] int iTInfo, [In, MarshalAs(UnmanagedType.U4)] int lcid);
            [PreserveSig]
            int GetIDsOfNames([In] ref Guid riid, [In, MarshalAs(UnmanagedType.LPArray)] string[] rgszNames, [In, MarshalAs(UnmanagedType.U4)] int cNames, [In, MarshalAs(UnmanagedType.U4)] int lcid, [Out, MarshalAs(UnmanagedType.LPArray)] int[] rgDispId);
            [PreserveSig]
            int Invoke(int dispIdMember, [In] ref Guid riid, [In, MarshalAs(UnmanagedType.U4)] int lcid, [In, MarshalAs(UnmanagedType.U2)]short dwFlags, [In, Out, MarshalAs(UnmanagedType.LPArray)] ComTypes.DISPPARAMS[] pDispParams, [Out, MarshalAs(UnmanagedType.LPArray)] object[] pVarResult, [In, Out, MarshalAs(UnmanagedType.LPArray)] ComTypes.EXCEPINFO[] pExcepInfo, [Out, MarshalAs(UnmanagedType.LPArray)] IntPtr[] pArgErr);
        }

        Type comType;
        PropertyInfo nameProperty;

        public ComObjectConverter(Type comType)
        {
            this.comType = comType;
            this.nameProperty = comType.GetProperty("Name");
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType.Equals(typeof(string)))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        private string GetName(IDispatch dispatch)
        {
            string theName=null;
            if (nameProperty != null)
            {
                IntPtr ptrDispatch = Marshal.GetIDispatchForObject(dispatch);
                try
                {
                    if ( ptrDispatch != IntPtr.Zero )
                    {
                        object typedObject = Marshal.GetTypedObjectForIUnknown(ptrDispatch, comType);
                        if (typedObject != null)
                        {
                            theName = (string)nameProperty.GetValue(typedObject, null);
                        }
                    }
                }
                catch
                {
                    theName = null;
                }
                finally
                {
                    if (ptrDispatch != IntPtr.Zero)
                    {
                        Marshal.Release(ptrDispatch);
                        ptrDispatch = IntPtr.Zero;
                    }
                }
            }
            if (theName == null )
            {
                Guid guid = Guid.Empty;
                string[] names = new string[] { "Name" };
                int[] dispIds = new int[] { 0 };
                int hr = dispatch.GetIDsOfNames(ref guid, names, names.Length, 0, dispIds);
                if (dispIds[0] != -1)
                {
                    ComTypes.DISPPARAMS[] dispParams = new ComTypes.DISPPARAMS[1];
                    dispParams[0].cNamedArgs = 0;
                    dispParams[0].rgdispidNamedArgs = IntPtr.Zero;
                    dispParams[0].cArgs = 1;
                    dispParams[0].rgvarg = Marshal.AllocCoTaskMem(0x1000);
                    try
                    {
                        Marshal.GetNativeVariantForObject(theName, dispParams[0].rgvarg);
                        hr = dispatch.Invoke(
                            dispIds[0],
                            ref guid,
                            CultureInfo.CurrentCulture.LCID,
                            (short)ComTypes.INVOKEKIND.INVOKE_PROPERTYGET,
                            dispParams,
                            null,
                            null,
                            null);
                        object retValue = Marshal.GetObjectForNativeVariant(dispParams[0].rgvarg);
                        if (retValue is string)
                        {
                            theName = (string)retValue;
                        }
                    }
                    finally
                    {
                        Marshal.FreeCoTaskMem(dispParams[0].rgvarg);
                        dispParams[0].rgvarg = IntPtr.Zero;
                    }
                }
            }
            return theName;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if ( destinationType.Equals(typeof(string)) )
            {
                if (value is IDispatch)
                {
                    string name = GetName((IDispatch)value);
                    if (name != null)
                    {
                        return comType.FullName + ":" + name;
                    }
                }
                return comType.FullName;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
