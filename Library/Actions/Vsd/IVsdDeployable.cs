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
    [InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("7F69C899-BF6F-464B-BBD6-98AE761C8B90")]
    internal interface IVsdDeployable
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        object GetType();
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetRootFolder();
        void SetRootFolder([In, MarshalAs(UnmanagedType.BStr)] string _p0);
        [return: MarshalAs(UnmanagedType.Interface)]
        IVsdCollectionSubset GetPlugIns();
        [return: MarshalAs(UnmanagedType.Interface)]
        object GetEditors();
        [return: MarshalAs(UnmanagedType.Interface)]
        object GetEvents();
        [return: MarshalAs(UnmanagedType.Interface)]
        object GetParent();
        void SetParent([In, MarshalAs(UnmanagedType.Interface)] object _p0);
        void OpenFromFile([In, MarshalAs(UnmanagedType.BStr)] string FileName_p0, [In, MarshalAs(UnmanagedType.Bool)] bool ReadOnly_p1);
        void SaveToFile([In, MarshalAs(UnmanagedType.BStr)] string FileName_p0);
        void OpenFromStream([In, MarshalAs(UnmanagedType.Interface)] object StreamIn_p0, [In, MarshalAs(UnmanagedType.Bool)] bool ReadOnly_p1);
        void SaveToStream([In, MarshalAs(UnmanagedType.Interface)] object StreamOut_p0);
        void Close();
        void Build([In, MarshalAs(UnmanagedType.Interface)] object BuildManager_p0, [In, MarshalAs(UnmanagedType.Bool)] bool Background_p1);
        void Validate([In, MarshalAs(UnmanagedType.Interface)] object ValidationManager_p0, [In, MarshalAs(UnmanagedType.Bool)] bool Background_p1);
        void Deploy([In, MarshalAs(UnmanagedType.Interface)] object DeploymentManager_p0, [In, MarshalAs(UnmanagedType.Bool)] bool Background_p1);
        void ApplyTemplate([In, MarshalAs(UnmanagedType.Interface)] object Template_p0);
        void GetSite([Out, MarshalAs(UnmanagedType.LPArray)] object[] _p0);
        void GetTaskList([Out, MarshalAs(UnmanagedType.LPArray)] object[] ppIVsdTaskList_p0);
        void GetMsmCache([Out, MarshalAs(UnmanagedType.Interface)] object _p0);
        void GetAssemblyBinder([Out, MarshalAs(UnmanagedType.Interface)] object _p0);
        [return: MarshalAs(UnmanagedType.Bool)]
        string GetMigrateProject();
        void SetMigrateProject([In, MarshalAs(UnmanagedType.Bool)] string _p0);
    }

}
