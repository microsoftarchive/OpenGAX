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

#region Using directives

using System.Runtime.InteropServices;

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
	internal static class UnsafeNativeMethods
	{
        /// <summary>
        /// Visual Studio IProfferService interface
        /// </summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		[ComImport]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("CB728B20-F786-11CE-92AD-00AA00A74CD0")]
		public interface IProfferService
		{
			/// <remarks/>
			int ProfferService(
				[In] ref System.Guid rguidService,
				[In]
            [MarshalAs(UnmanagedType.Interface)] Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp,
			[Out]
            [MarshalAs(UnmanagedType.U4)] out uint pdwCookie);

			/// <remarks/>
			void RevokeService(
						[In]
            [MarshalAs(UnmanagedType.U4)] uint dwCookie);
		}
	}
}
