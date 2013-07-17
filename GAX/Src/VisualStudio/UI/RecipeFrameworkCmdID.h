/***************************************************************************
         Copyright (c) Microsoft Corporation, All rights reserved.             
    This code sample is provided "AS IS" without warranty of any kind, 
    it is not recommended for use in a production environment.
***************************************************************************/

// RecipeFrameworkCmdID.h
// Command IDs used in defining command bars
//
// do not use #pragma once - used by ctc compiler
#ifndef __RFCMDID_H_
#define __RFCMDID_H_

///////////////////////////////////////////////////////////////////////////////
// Menu Group IDs
#define igrpRecipeFrameworkMenuGroup		0x100
// Command IDs
#define icmdRecipeManagerCommand			0x100
#define icmdNavigatorWindowCommand			0x101

// shorthand macros for a more compact and manageable table
#define OI_NOID         guidOfficeIcon:msotcidNoIcon
#define OI_ICON(icmd)   guidRecipeFrameworkPkg:icmd
#define VIS_DEF         COMMANDWELLONLY
#define DIS_DEFEX		DYNAMICVISIBILITY|DEFAULTINVISIBLE|DEFAULTDISABLED|TEXTCHANGES|TEXTCHANGESBUTTON
#define DIS_DEF         DYNAMICVISIBILITY|DEFAULTINVISIBLE|DEFAULTDISABLED
#define NO_TEXT			"Unknown"

#define bmpidGaxManager 1
#define bmpidGaxNoIcon  2

#endif // __RFCMDID_H_
