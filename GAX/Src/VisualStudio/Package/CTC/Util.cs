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
using System.Text;
using System.ComponentModel.Design;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.CTC
{
	internal class Util
	{
		public static readonly CommandID NoIcon = new CommandID(OfficeCmdDef.guidOfficeIcon, OfficeCmdDef.msotcidNoIcon);
		public const CommandFlag VisibilityDefault = CommandFlag.CommandWellOnly;
        public const CommandFlag DisabledDefault = CommandFlag.CommandWellOnly | CommandFlag.DynamicVisibility | CommandFlag.DefaultDisabled | CommandFlag.DefaultInvisible | CommandFlag.TextChanges | CommandFlag.TextContextUseBtn;
	}
}
