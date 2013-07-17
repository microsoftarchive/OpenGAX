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
using System.IO;
using System.Collections;
using Microsoft.Practices.Common;
using System.Globalization;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Drawing;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.CTC
{
	internal sealed class CTCGenerator
	{
		#region Fields

		TextWriter textWriter;
		CTCFile    ctcFile;

		#endregion

		public CTCGenerator(CTCFile ctcFile,TextWriter textWriter)
		{
			this.ctcFile = ctcFile;
			this.textWriter = textWriter;
		}

		private void Generate(object o)
		{
			throw new InvalidCastException();
		}

		private void Generate(MenusSubSection subsection)
		{
			textWriter.WriteLine("  MENUS_BEGIN");
            foreach (MenuGroup menu in subsection.Menus)
            {
                Generate(menu);
            }
			textWriter.WriteLine("  MENUS_END");
		}

        private void Generate(MenuGroup menu)
        {
            textWriter.Write("    ");
            Generate(menu.Group);
            textWriter.Write(",");
            Generate(menu.ParentGroup);
            textWriter.Write(",");
			textWriter.Write(String.Format(CultureInfo.InvariantCulture,"0x{0:X3}",menu.Priority));
            textWriter.Write(",");
			textWriter.Write(String.Format(CultureInfo.InvariantCulture, "0x{0:X8}", (int)menu.CommandType));
            textWriter.Write(",");
            textWriter.Write(String.Format(CultureInfo.InvariantCulture, "\"{0}\"", menu.CommandText));
            textWriter.Write(",");
            textWriter.Write(String.Format(CultureInfo.InvariantCulture, "\"{0}\"", menu.MenuText));
			textWriter.WriteLine(";");
        }


		private void Generate(NewGroup group)
		{
			textWriter.Write("    ");
			Generate(group.Group);
			textWriter.Write(",");
			Generate(group.Parent);
			textWriter.Write(",");
			textWriter.Write(String.Format(CultureInfo.InvariantCulture,"0x{0:X3}",group.Priority));
			textWriter.WriteLine(";");
		}

		private void Generate(NewGroupsSubSection subsection)
		{
			textWriter.WriteLine("  NEWGROUPS_BEGIN");
			foreach (NewGroup group in subsection.NewGroups)
			{
				Generate(group);
			}
			textWriter.WriteLine("  NEWGROUPS_END");
		}

		private void Generate(Button button)
		{
			textWriter.Write("    ");
			Generate(button.Command);
			textWriter.Write(",");
			Generate(button.Group);
			textWriter.Write(",");
			textWriter.Write(String.Format(CultureInfo.InvariantCulture, "0x{0:X4}", button.Priority));
			textWriter.Write(",");
			Generate(button.Icon);
			textWriter.Write(",");
			textWriter.Write(String.Format(CultureInfo.InvariantCulture, "0x{0:X8}", (int)button.CommandType));
			textWriter.Write(",");
			textWriter.Write(String.Format(CultureInfo.InvariantCulture, "0x{0:X8}", (int)button.Flags));
			textWriter.Write(",");
			textWriter.Write(String.Format(CultureInfo.InvariantCulture, "\"{0}\"",button.Text));
			textWriter.WriteLine(";");
		}

		private void Generate(ButtonsSubSection subsection)
		{
			textWriter.WriteLine("  BUTTONS_BEGIN");
			foreach (Button button in subsection.Buttons)
			{
				Generate(button);
			}
			textWriter.WriteLine("  BUTTONS_END");
		}

        private void Generate(CTCBitmap bitmap)
        {
            textWriter.Write("    ");
            Generate(bitmap.Command);
            textWriter.Write(" ");
            if (bitmap.Images.Count > 0)
            {
                for (int i = 0; i < bitmap.Images.Keys.Count; i++)
                {
                    textWriter.Write("0x");
					string key = bitmap.Images.Keys[i];
					int keyAsNumber = int.Parse(key);
					textWriter.Write(keyAsNumber.ToString("X4"));
                    if (i == bitmap.Images.Count - 1)
                    {
                        textWriter.WriteLine(";");
                    }
                    else
                    {
                        textWriter.Write(",");
                    }
                }
            }
        }

        private void Generate(BitmapsSubSection subsection)
		{
			textWriter.WriteLine("  BITMAPS_BEGIN");
            if (subsection.Bitmaps != null)
            {
                foreach (CTCBitmap bitmap in subsection.Bitmaps)
                {
                    if (bitmap.Images.Count > 0)
                    {
                        Generate(bitmap);
                    }
                }
            }
			textWriter.WriteLine("  BITMAPS_END");
		}

		private void Generate(CommandID command)
		{
            Generate(command.Guid);
            textWriter.Write(":0x");
            textWriter.Write(command.ID.ToString("X4"));
		}

		private void Generate(Guid guid)
		{
			byte[] bytes = guid.ToByteArray();
			object[] oBytes = new object[bytes.Length];
			int i=0;
			foreach(byte b in bytes)
			{
				oBytes[i++]=b;
			}
			textWriter.Write(
				String.Format(
					CultureInfo.InvariantCulture,
					"{{ 0x{3:X2}{2:X2}{1:X2}{0:X2}, 0x{5:X2}{4:X2}, 0x{7:X2}{6:X2}, {{ 0x{8:X2}, 0x{9:X2}, 0x{10:X2}, 0x{11:X2}, 0x{12:X2}, 0x{13:X2}, 0x{14:X2}, 0x{15:X2} }} }}",
					oBytes
				)
			);
		}

		private void Generate(CMDSSection section)
		{
			textWriter.Write("CMDS_SECTION ");
			Generate(section.PackageGuid);
			textWriter.WriteLine();
			Generate(section.MenusSubSection);
			Generate(section.NewGroupsSubSection);
			Generate(section.ButtonsSubSection);
			Generate(section.BitmapsSubSection);
			textWriter.WriteLine("CMDS_END");
		}

		private void Generate(CMDUsedSection section)
		{
			textWriter.WriteLine("CMDUSED_SECTION");
			textWriter.WriteLine("CMDUSED_END");
		}

        private void Generate(CMDPlacement placement)
        {
            textWriter.Write("  ");
            Generate(placement.Command);
            textWriter.Write(",");
            Generate(placement.Parent);
            textWriter.Write(",0x");
            textWriter.Write(placement.Priority.ToString("X4"));
            textWriter.WriteLine(";");
        }

		private void Generate(CMDPlacementSection section)
		{
			textWriter.WriteLine("CMDPLACEMENT_SECTION");
            foreach (CMDPlacement placement in section.Placements)
            {
                Generate(placement);
            }
			textWriter.WriteLine("CMDPLACEMENT_END");
		}

        private void Generate(Visibility visibility)
        {
            textWriter.Write("  ");
            Generate(visibility.Command);
            textWriter.Write(",");
            Generate(visibility.Context);
            textWriter.WriteLine(";");
        }

		private void Generate(VisibilitySection section)
		{
			textWriter.WriteLine("VISIBILITY_SECTION");
            foreach (Visibility visibility in section.Visibilities)
            {
                Generate(visibility);
            }
			textWriter.WriteLine("VISIBILITY_END");
		}

		private void Generate(KeybindingsSection section)
		{
			textWriter.WriteLine("KEYBINDINGS_SECTION");
			textWriter.WriteLine("KEYBINDINGS_END");
		}
		
		public void Generate()
		{
			Generate(ctcFile.CMDSSection);
			Generate(ctcFile.CMDUsedSection);
			Generate(ctcFile.CMDPlacementSection);
			Generate(ctcFile.VisibilitySection);
			Generate(ctcFile.KeybindingsSection);
		}
	}
}
