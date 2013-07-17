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
using System.ComponentModel.Design;
using System.Globalization;
using System.Collections.Generic;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.CTC
{
	internal partial class ShellCmdDef
	{
		public static readonly Guid guidSHLMainMenu = new Guid("{d309f791-903f-11d0-9efc-00a0c911004f}");
		public static readonly Guid guidOfficeIcon  = new Guid("{d309f794-903f-11d0-9efc-00a0c911004f}");
        public static readonly Guid Group_Undefined = new Guid(0x83285929, 0x227c, 0x11d3, 0xb8, 0x70, 0x0, 0xc0, 0x4f, 0x79, 0xf8, 0x2);
		public const int bmpidGaxManager = 1;
		public const int bmpidGaxNoIcon = 2;
		public const int IDM_VS_MENU_FILE				= 0x0080;
		public const int IDM_VS_MENU_EDIT				= 0x0081;
		public const int IDM_VS_MENU_VIEW				= 0x0082;
		public const int IDM_VS_MENU_PROJECT			= 0x0083;
		public const int IDM_VS_MENU_TOOLS				= 0x0085;
		public const int IDM_VS_MENU_WINDOW				= 0x0086;
		public const int IDM_VS_MENU_ADDINS				= 0x0087;
		public const int IDM_VS_MENU_HELP				= 0x0088;
		public const int IDM_VS_MENU_DEBUG				= 0x0089;
		public const int IDM_VS_MENU_FORMAT				= 0x008A;
		public const int IDM_VS_MENU_ALLMACROS			= 0x008B;
		public const int IDM_VS_MENU_BUILD				= 0x008C;
		public const int IDM_VS_MENU_CONTEXTMENUS		= 0x008D;
		public const int IDG_VS_MENU_CONTEXTMENUS		= 0x008E;
		public const int IDM_VS_MENU_REFACTORING		= 0x008f;
		public const int IDM_VS_MENU_COMMUNITY			= 0x0090;
		public const int IDM_VS_CTXT_PROJNODE			= 0x0402;
		public const int IDM_VS_CTXT_PROJWIN			= 0x0403;
		public const int IDM_VS_CTXT_PROJWINBREAK		= 0x0404;
		public const int IDM_VS_CTXT_ERRORLIST			= 0x0405;
		public const int IDM_VS_CTXT_DOCKEDWINDOW		= 0x0406;
		public const int IDM_VS_CTXT_MENUDES			= 0x0407;
		public const int IDM_VS_CTXT_PROPBRS			= 0x0408;
		public const int IDM_VS_CTXT_TOOLBOX			= 0x0409;
		// UNUSED: 0x040A - 0x040C
		public const int IDM_VS_CTXT_CODEWIN			= 0x040D;
		public const int IDM_VS_CTXT_TASKLIST			= 0x040E;
		public const int IDM_VS_CTXT_RESULTSLIST		= 0x0411;
		public const int IDM_VS_CTXT_STUBPROJECT		= 0x0412;
		public const int IDM_VS_CTXT_SOLNNODE			= 0x0413;
		public const int IDM_VS_CTXT_SOLNFOLDER			= 0x0414;
		// Common Item Node context menu
		public const int IDM_VS_CTXT_ITEMNODE			= 0x0430;
		// Folder Node context menu
		public const int IDM_VS_CTXT_FOLDERNODE			= 0x0431;
		public const int IDM_VS_CTXT_WEBREFFOLDER		= 0x0452;
		public const int IDM_VS_CTXT_REFERENCE			= 0x0451;
		// "Add" context menus
		public const int IDG_VS_CTXT_SOLUTION_ADD_PROJ = 0x021D;
		public const int IDG_VS_CTXT_SOLUTION_ADD_ITEM = 0x021E;
		public const int IDG_VS_CTXT_SOLUTION_ADD      = 0x0233;
		public const int IDG_VS_CTXT_PROJECT_ADD       = 0x0202;
		public const int IDG_VS_CTXT_PROJECT_ADD_ITEMS = 0x0203;

		//Solution Folder menues
		public const int IDG_VS_CTXT_SLNFLDR_ADD_PROJ  = 0x0261;
		public const int IDG_VS_CTXT_SLNFLDR_ADD_ITEM  = 0x0262;
		public const int IDG_VS_CTXT_SLNFLDR_BUILD     = 0x0263;
		public const int IDG_VS_CTXT_SLNFLDR_ADD       = 0x0264;

		// Project Menu Groups
		public const int IDG_VS_PROJ_ADD               = 0x0140;

		// Cascading menus
		public const int IDM_VS_CSCD_SOLUTION_ADD      = 0x0350;
		public const int IDM_VS_CSCD_SOLUTION_DEBUG    = 0x0351;
		public const int IDM_VS_CSCD_PROJECT_ADD       = 0x0352;
		public const int IDM_VS_CSCD_PROJECT_DEBUG     = 0x0353;
		public const int IDM_VS_CSCD_SLNFLDR_ADD       = 0x0357;
		// ClassView cascades
		public const int IDM_VS_CSCD_CV_PROJADD        = 0x0354;
		public const int IDM_VS_CSCD_CV_ITEMADD        = 0x0355;

        // File Menu Groups
        public const int IDG_VS_FILE_NEW_PROJ_CSCD     = 0x010E;
        public const int IDG_VS_FILE_ITEM              = 0x010F;
        public const int IDG_VS_FILE_FILE              = 0x0110;
        public const int IDG_VS_FILE_ADD               = 0x0111;
        public const int IDG_VS_FILE_SAVE              = 0x0112;
        public const int IDG_VS_FILE_RENAME            = 0x0113;
        public const int IDG_VS_FILE_PRINT             = 0x0114;
        public const int IDG_VS_FILE_MRU               = 0x0115;
        public const int IDG_VS_FILE_EXIT              = 0x0116;
        public const int IDG_VS_FILE_DELETE            = 0x0117;
        public const int IDG_VS_FILE_SOLUTION          = 0x0118;
        public const int IDG_VS_FILE_NEW_CASCADE       = 0x0119;
        public const int IDG_VS_FILE_OPENP_CASCADE     = 0x011A;
        public const int IDG_VS_FILE_OPENF_CASCADE     = 0x011B;
        public const int IDG_VS_FILE_ADD_PROJECT_NEW   = 0x011C;
        public const int IDG_VS_FILE_ADD_PROJECT_EXI   = 0x011D;
        public const int IDG_VS_FILE_FMRU_CASCADE      = 0x011E;
        public const int IDG_VS_FILE_PMRU_CASCADE      = 0x011F;
        public const int IDG_VS_FILE_BROWSER           = 0x0120;
        public const int IDG_VS_FILE_MOVE              = 0x0121;
        public const int IDG_VS_FILE_MOVE_CASCADE      = 0x0122;
        public const int IDG_VS_FILE_MOVE_PICKER       = 0x0123;
        public const int IDG_VS_FILE_MISC              = 0x0124;
        public const int IDG_VS_FILE_MISC_CASCADE      = 0x0125;
        public const int IDG_VS_FILE_MAKE_EXE          = 0x0126;

		// Venus context menus 
		public const int IDM_VS_CTXT_WEBPROJECT        = 0x0470;
		public const int IDM_VS_CTXT_WEBFOLDER         = 0x0471;
		public const int IDM_VS_CTXT_WEBITEMNODE       = 0x0472;
		public const int IDM_VS_CTXT_WEBSUBWEBNODE     = 0x0474;


		public static Hashtable VSCommandBars
		{
			get
			{
				if (vsCommandBars == null)
				{
					vsCommandBars = new Hashtable();

					#region Visual Studio Commands Bars

					// Solution Explorer menus
					vsCommandBars[Configuration.CommandBarName.Solution] = IDM_VS_CTXT_SOLNNODE;
					vsCommandBars[Configuration.CommandBarName.Project] = IDM_VS_CTXT_PROJNODE;
					vsCommandBars[Configuration.CommandBarName.Item] = IDM_VS_CTXT_ITEMNODE;
					vsCommandBars[Configuration.CommandBarName.Folder] = IDM_VS_CTXT_FOLDERNODE;
					vsCommandBars[Configuration.CommandBarName.SolutionFolder] = IDM_VS_CTXT_SOLNFOLDER;

					// Context menues for the "Add" menu item in solution explorer
					vsCommandBars[Configuration.CommandBarName.ProjectAdd] = IDM_VS_CSCD_PROJECT_ADD;
                    vsCommandBars[Configuration.CommandBarName.SolutionAdd] = IDM_VS_CSCD_SOLUTION_ADD;
                    vsCommandBars[Configuration.CommandBarName.SolutionFolderAdd] = IDM_VS_CSCD_SLNFLDR_ADD;

					// Context menues for Web Project
					vsCommandBars[Configuration.CommandBarName.WebProject] = IDM_VS_CTXT_WEBPROJECT;
					vsCommandBars[Configuration.CommandBarName.WebItem] = IDM_VS_CTXT_WEBITEMNODE;
					vsCommandBars[Configuration.CommandBarName.WebFolder] = IDM_VS_CTXT_WEBFOLDER;

					#endregion

				}
				return vsCommandBars;
			}
		} private static Hashtable vsCommandBars = null;

        public static CommandID GetPredefinedCommandID(Configuration.CommandBarName commandBarName)
        {
            int commandId = 0;
            Guid guid = ShellCmdDef.guidSHLMainMenu;
            if (!ShellCmdDef.VSCommandBars.ContainsKey(commandBarName))
            {
				throw new RecipeFrameworkException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resources.CommandBar_NotFound,
                        commandBarName)
                    );
            }
            commandId = (int)ShellCmdDef.VSCommandBars[commandBarName];
            return new CommandID(guid, commandId);
        }

        internal sealed class UndefinedCommandID: CommandID
        {
            internal UndefinedCommandID()
                : base(Group_Undefined, 0)
            {
            }
        }

        public static CommandID UndefinedGroup
        {
            get
            {
                if (undefinedGroup == null)
                {
                    undefinedGroup = new UndefinedCommandID();
                }
                return undefinedGroup;
            }
        } static CommandID undefinedGroup = null;

		private static Dictionary<Guid, Dictionary<int, int>> projectPackages = ProjectPackages;

		private static Dictionary<Guid, Dictionary<int, int>> ProjectPackages
		{
			get
			{
				if (projectPackages == null)
				{
					projectPackages = new Dictionary<Guid, Dictionary<int, int>>();
				}
				return projectPackages;
			}
		}

		public static CommandID MapIconToBitmap(CommandID commadId)
		{
			if (ProjectPackages.ContainsKey(commadId.Guid))
			{
				Dictionary<int, int> map = ProjectPackages[commadId.Guid];
				if ( map.ContainsKey(commadId.ID) )
				{
					int newId = map[commadId.ID];
					return new CommandID(commadId.Guid,newId);
				}
			}
			return commadId;
		}
	}
}
