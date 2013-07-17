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

using System;
using System.Text;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
	internal enum VSTASKVALUEFLAGS
	{
		TVF_EDITABLE = 0x00000001,   // This value can be changed by the user.
		TVF_ENUM = 0x00000002,   // The value is not arbitrary, but may only be one of a limited set of strings provided by the task item.
		TVF_BINARY_STATE = 0x00000004,   // This is an enum with only two possible states.  The user can switch the state by a similar UI action to clicking a checkbox.
		TVF_HORZ_RIGHT = 0x00000008,   // The value is aligned against the right edge of the column.
		TVF_HORZ_CENTER = 0x00000010,   // The value is horizontally centered in the column.
		TVF_STRIKETHROUGH = 0x00000020,   // The value is drawn with a strikethrough font style.
		TVF_FILENAME = 0x00000040,   // This textual value will be treated as a file name.
	}
	internal class VSConstantsEx
	{
		public const int FALSE = 0;
		public const int TRUE = 1;
	}
	internal class NativeMethods
	{
		public static bool Succeeded(int hr)
		{
			return (hr >= 0);
		}
	}
}
