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

namespace Microsoft.Practices.RecipeFramework.VisualStudio.CTC
{
	internal partial class ShellCmdDef
	{
		public static System.Guid guidJSharpPackage
		{
			get
			{
				return new System.Guid("{e6fdf8b0-f3d1-11d4-8576-0002a516ece8}");
			}
		}
		private static System.Collections.Generic.Dictionary<int, int> mapJSharpPackage = MapJSharpPackage;
		private static System.Collections.Generic.Dictionary<int, int> MapJSharpPackage
		{
			get
			{
				if (mapJSharpPackage == null)
				{
					mapJSharpPackage = new System.Collections.Generic.Dictionary<int, int>();
					ProjectPackages.Add(guidJSharpPackage,mapJSharpPackage);
				    mapJSharpPackage.Add(0x00000064, 0x001);
				    mapJSharpPackage.Add(0x00000065, 0x002);
				    mapJSharpPackage.Add(0x00001194, 0x003);
				    mapJSharpPackage.Add(0x00001195, 0x004);
				    mapJSharpPackage.Add(0x00001196, 0x005);
				    mapJSharpPackage.Add(0x00001197, 0x006);
				    mapJSharpPackage.Add(0x00001198, 0x007);
				    mapJSharpPackage.Add(0x00001199, 0x008);
				    mapJSharpPackage.Add(0x0000119A, 0x009);
				    mapJSharpPackage.Add(0x0000119B, 0x00A);
				    mapJSharpPackage.Add(0x0000119C, 0x00B);
				    mapJSharpPackage.Add(0x0000119D, 0x00C);
				    mapJSharpPackage.Add(0x0000119E, 0x00D);
				    mapJSharpPackage.Add(0x0000119F, 0x00E);
				    mapJSharpPackage.Add(0x000011A0, 0x00F);
				    mapJSharpPackage.Add(0x000011A1, 0x010);
				    mapJSharpPackage.Add(0x000011A2, 0x011);
				    mapJSharpPackage.Add(0x000011A3, 0x012);
				    mapJSharpPackage.Add(0x000011A4, 0x013);
				    mapJSharpPackage.Add(0x000011A5, 0x014);
				    mapJSharpPackage.Add(0x000011A6, 0x015);
				    mapJSharpPackage.Add(0x000011A8, 0x016);
				    mapJSharpPackage.Add(0x000011A9, 0x017);
				    mapJSharpPackage.Add(0x000011AA, 0x018);
				    mapJSharpPackage.Add(0x000011AB, 0x019);
				    mapJSharpPackage.Add(0x000011AC, 0x01A);
				    mapJSharpPackage.Add(0x000011AD, 0x01B);
				    mapJSharpPackage.Add(0x000011AE, 0x01C);
				    mapJSharpPackage.Add(0x000011AF, 0x01D);
				    mapJSharpPackage.Add(0x000011B0, 0x01E);
				    mapJSharpPackage.Add(0x000011B1, 0x01F);
				    mapJSharpPackage.Add(0x000011B2, 0x020);
				    mapJSharpPackage.Add(0x000011B3, 0x021);
				    mapJSharpPackage.Add(0x000011B4, 0x022);
				    mapJSharpPackage.Add(0x000011B5, 0x023);
				    mapJSharpPackage.Add(0x000011B6, 0x024);
				    mapJSharpPackage.Add(0x000011B7, 0x025);
				    mapJSharpPackage.Add(0x000011B8, 0x026);
				    mapJSharpPackage.Add(0x000011B9, 0x027);
				    mapJSharpPackage.Add(0x000011BA, 0x028);
				    mapJSharpPackage.Add(0x000011BC, 0x029);
				    mapJSharpPackage.Add(0x000011BD, 0x02A);
				    mapJSharpPackage.Add(0x000011BE, 0x02B);
				    mapJSharpPackage.Add(0x000011BF, 0x02C);
				    mapJSharpPackage.Add(0x000011C0, 0x02D);
				    mapJSharpPackage.Add(0x000011C1, 0x02E);
				    mapJSharpPackage.Add(0x000011C2, 0x02F);
				    mapJSharpPackage.Add(0x000011C3, 0x030);
				    mapJSharpPackage.Add(0x000011C4, 0x031);
				    mapJSharpPackage.Add(0x000011C5, 0x032);
				    mapJSharpPackage.Add(0x000011C6, 0x033);
				    mapJSharpPackage.Add(0x000011C7, 0x034);
				    mapJSharpPackage.Add(0x000011C8, 0x035);
				    mapJSharpPackage.Add(0x000011C9, 0x036);
				    mapJSharpPackage.Add(0x000011CA, 0x037);
				    mapJSharpPackage.Add(0x000011CB, 0x038);
				    mapJSharpPackage.Add(0x000011CC, 0x039);
				    mapJSharpPackage.Add(0x000011CD, 0x03A);
				    mapJSharpPackage.Add(0x000011CE, 0x03B);
				    mapJSharpPackage.Add(0x000011D1, 0x03C);
				    mapJSharpPackage.Add(0x000011D2, 0x03D);
				    mapJSharpPackage.Add(0x000011D3, 0x03E);
				    mapJSharpPackage.Add(0x000011D4, 0x03F);
				    mapJSharpPackage.Add(0x000011D5, 0x040);
				    mapJSharpPackage.Add(0x000011D6, 0x041);
				    mapJSharpPackage.Add(0x000011DA, 0x042);
				    mapJSharpPackage.Add(0x000011DB, 0x043);
				    mapJSharpPackage.Add(0x000011DC, 0x044);
				    mapJSharpPackage.Add(0x000011EA, 0x045);
				    mapJSharpPackage.Add(0x000011EB, 0x046);
				    mapJSharpPackage.Add(0x000011F8, 0x047);
				    mapJSharpPackage.Add(0x000011F9, 0x048);
				    mapJSharpPackage.Add(0x000011FA, 0x049);
				    mapJSharpPackage.Add(0x000011FB, 0x04A);
				    mapJSharpPackage.Add(0x00001201, 0x04B);
				}
				return mapJSharpPackage;
			}
		}
	}
}
