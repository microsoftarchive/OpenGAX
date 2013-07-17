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
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.CustomMarshalers;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions.Vsd
{
    [ComImport, Guid("DE76ACE9-DF93-4029-B054-E8E376E6D72F"), TypeLibType((short)4160)]
    internal interface IVsdGetNewEnum /*: IEnumerable*/
    {
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4), TypeLibFunc((short)0x40)]
        IEnumerator GetEnumerator();
    }
}
