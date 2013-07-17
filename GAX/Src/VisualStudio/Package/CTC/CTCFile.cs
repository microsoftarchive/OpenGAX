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
using System.Collections;
using System.Globalization;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.CTC
{
	internal class CTCFile
	{
		public CTCFile(CMDSSection cmds, CMDUsedSection used, CMDPlacementSection placement, VisibilitySection visibility, KeybindingsSection keys)
		{
			this.cmdsSection = cmds;
			this.cmdUsedSection = used;
			this.cmdPlacementSection = placement;
			this.visibilitySection = visibility;
			this.keybindingsSection = keys;
		}

		public CMDSSection CMDSSection
		{
			get { return cmdsSection; }
		} CMDSSection cmdsSection;

		public CMDUsedSection CMDUsedSection
		{
			get { return cmdUsedSection; }
		} CMDUsedSection cmdUsedSection;

		public CMDPlacementSection CMDPlacementSection
		{
			get { return cmdPlacementSection; }
		} CMDPlacementSection cmdPlacementSection;

		public VisibilitySection VisibilitySection
		{
			get { return visibilitySection; }
		} VisibilitySection visibilitySection;

		public KeybindingsSection KeybindingsSection
		{
			get { return keybindingsSection; }
		} KeybindingsSection keybindingsSection;

	}
}
