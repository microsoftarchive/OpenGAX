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

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions.Vsd
{
    [ComImport, Guid("C648C68F-A88B-4B83-ABC2-700D8395254C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IVsdProject
    {
        void IsDocumentInProject([In, MarshalAs(UnmanagedType.BStr)] string pszMkDocument, [Out, MarshalAs(UnmanagedType.Bool)] bool pfFound, [Out, MarshalAs(UnmanagedType.U4)] int pdwPriority, [Out, MarshalAs(UnmanagedType.U4)] int pitemid);
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetMkDocument([In, MarshalAs(UnmanagedType.U4)] int itemid);
        void OpenItem([In, MarshalAs(UnmanagedType.U4)] int itemid, [In] ref Guid rguidLogicalView, [In, MarshalAs(UnmanagedType.IUnknown)] object punkDocDataExisting, [Out, MarshalAs(UnmanagedType.IUnknown)] object ppWindowFrame);
        void GetItemContext([In, MarshalAs(UnmanagedType.U4)] int itemid, [Out, MarshalAs(UnmanagedType.LPArray)] object[] ppSP);
        void GenerateUniqueItemName([In, MarshalAs(UnmanagedType.U4)] int itemidLoc, [In, MarshalAs(UnmanagedType.BStr)] string pszExt, [In, MarshalAs(UnmanagedType.BStr)] string pszSuggestedRoot, [Out, MarshalAs(UnmanagedType.LPArray)] string[] pbstrItemName);
        [return: MarshalAs(UnmanagedType.I4)]
        int AddItem([In, MarshalAs(UnmanagedType.U4)] int itemidLoc, [In, MarshalAs(UnmanagedType.I4)] int dwAddItemOperation, [In, MarshalAs(UnmanagedType.BStr)] string pszItemName, [In, MarshalAs(UnmanagedType.U4)] int cFilesToOpen, [In, MarshalAs(UnmanagedType.LPArray)] string[] rgpszFilesToOpen, [In] IntPtr hwndDlg);
        [return: MarshalAs(UnmanagedType.I4)]
        int RemoveItem([In, MarshalAs(UnmanagedType.U4)] int dwReserved, [In, MarshalAs(UnmanagedType.U4)] int itemidLoc, [Out, MarshalAs(UnmanagedType.Bool)] bool pfResult);
        [return: MarshalAs(UnmanagedType.I4)]
        int ReopenItem([In, MarshalAs(UnmanagedType.U4)] int itemidLoc, [In] ref Guid rguidEditorType, [In, MarshalAs(UnmanagedType.BStr)] string pszPhysicalView, [In] ref Guid rguidLogicalView, [In, MarshalAs(UnmanagedType.IUnknown)] object punkDocDataExisting, [Out, MarshalAs(UnmanagedType.IUnknown)] object ppWindowFrame);
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetProjectFilename();
        void SetDirty();
        void UpdateAllDependencies();
        void AddFile([In, MarshalAs(UnmanagedType.BStr)] string _p0);
        void AddOutputGroup([In, MarshalAs(UnmanagedType.BStr)] string _p0, [In, MarshalAs(UnmanagedType.BStr)] string _p1);
    }
}
