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

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.CTC
{
	#region Constants

	// Command types
	internal enum CommandType : uint
	{
		Separator = 0x00000000,
		Button = 0x00000001,
		MenuButton = 0x00000002,
		Swatch = 0x00000003,
		SplitDropDown = 0x00000004,
		DropDownCombo = 0x00000010,
		MRUCombo = 0x00000020,
		DynamicCombo = 0x00000030,
		OwnerDropDownCombo = 0x00000040,
		IndexCombo = 0x00000050,
		Menu = 0x00000100,
		MenuController = 0x00000200,
		MenuToolBar = 0x00000300,
		MenuContext = 0x00000400,
		MenuToolWindowToolBar = 0x00000500,
		MenuControllerLatched = 0x00000600,
		Shared = 0x10000000,
		AppId = 0x20000000,
		CommandMenuDefaultDocked = 0x01000000,
		CommandMenuNoToolBarClose = 0x02000000,
		CommandMenuNotInTbList = 0x04000000,
		CommandMenuAlwaysCreate = 0x08000000
	};

	internal enum CommandTypeMask : uint
	{
		ComboMask = 0x000000F0,   // Mask for combo cmd types; update as new types are added!
		CtrlMask = 0x000000FF,   // Mask for all ctrl cmd types; update as new types are added!
		MenuMask = 0x00000F00,   // Mask for menu cmd types; update as new types are added!
		MenuFlagMask = 0x0F000000,   // Mask for menu flag types; these are merged together with the type in the ctc format
		SharedMask = 0xF0000000
	}

	internal enum CommandTableType : uint
	{
		CommandTableButton = 0,
		CommandTableCombo = 1,
		CommandTableMenu = 2,
		CommandTableGroup = 3
		// next value here would be 4
	}

	// values for IsCommandAutoVisible function
	internal enum CommandVisibility
	{
		cCommandNotVisible = 0,
		cCommandVisible = 1,
		cCommandNotInTable = 2
	}


	internal enum CommandFlag : uint
	{
		None = 0x0,
		NoKeyCustomize = 0x00000001,   // no keyboard customize
		NoButtonCustomize = 0x00000002,   // no button customize
		NoCustomize = (NoKeyCustomize | NoButtonCustomize),   // no customize at all
		TextContextUseBtn = 0x00000004,   // Use button text for this cmd on context menus, default is menu text
		TextChanges = 0x00000008,   // cmd has dynamic text 
		DefaultDisabled = 0x00000010,   // disable by default
		DefaultInvisible = 0x00000020,   // invisible by default
		DynamicVisibility = 0x00000040,   // can be visible or not
		Repeat = 0x00000080,   // repeatable item
		DynamicItemStart = 0x00000100,   // dynamic collection of items
		CommandWellOnly = 0x00000200,   // not shown on main menu
		Pict = 0x00000400,   // show icon on toolbar, text and icon on menu
		Text = 0x00000800,   // show text on toolbar, text and icon on menu
		PictAndText = (Pict | Text),   // show both icon and text on toolbar and menu
		AllowParams = 0x00001000,   // accept parameter passing
		FilterKeys = 0x00002000,   // filter keystrokes
		PostExec = 0x00004000,   // (simulate) posting this cmd (make non-blocking)
		DontCache = 0x00008000,   // don't cache the results of the QueryStatus call to this command
		FixMenuController = 0x00010000,   // if placed on menu controller, this cmd is always default
		NoShowOnMenuController = 0x00020000,   // if placed on menu controller, does not show on dropdown menu portion
		RouteToDocuments = 0x00040000,   // route this cmd to the active document
		NoAutoComplete = 0x00080000,   // For combos: disable autocomplete
		TextMenuUseBtn = 0x00100000,   // Use button text for this cmd on menus, default is menu text
		TextMenuCtrlUseMnu = 0x00200000,   // Use menu text for this cmd on menu controllers, default is button text
		TextCascadeUseBtn = 0x00400000,   // Use button text for this cmd on cascade menus, default is menu text
		CaseSensitive = 0x00800000,   // For combos: this combo's items are case-sensitive
		CommandMenuDefaultDocked = 0x01000000,
		CommandMenuNoToolBarClose = 0x02000000,
		CommandMenuNotInTbList = 0x04000000,
		CommandMenuAlwaysCreate = 0x08000000,
		ProfferedCmd = 0x10000000
		// available                        = 0x20000000,
		// available                        = 0x40000000,
		// available                        = 0x80000000
	}

    internal enum CommandGroupFlag : uint
	{
		cCommandGroupDynamic = 1
	}

    internal enum KeyFlag : uint
	{
		KeyModifierAlt = 0x01,
		KeyModifierShift = 0x02,
		KeyModifierControl = 0x04
	}

	#endregion
}
