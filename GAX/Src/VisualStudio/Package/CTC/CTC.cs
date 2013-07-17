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
using System.Windows.Forms;
using System.Drawing;
#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.CTC
{
	#region CTC File Definition Classes

	internal class CTCSubSection
	{
	}

	internal class CTCSection
	{
	}

	internal class MenuGroup
	{
		public MenuGroup(CommandID group, CommandID parent, int priority, CommandType type, string commandText, string menuText)
		{
			this.group = group;
			this.parentGroup = parent;
			this.priority = priority;
			this.commandType = type;
			this.commandText = commandText;
			this.menuText = menuText;
		}

		public CommandID Group
		{
			get { return group; }
		} CommandID group;

		public CommandID ParentGroup
		{
			get { return parentGroup; }
		} CommandID parentGroup;

		public int Priority
		{
			get { return priority; }
		} int priority;

		public CommandType CommandType
		{
			get { return commandType; }
		} CommandType commandType;

		public string CommandText
		{
			get { return commandText; }
		} string commandText;

		public string MenuText
		{
			get { return menuText; }
		} string menuText;
	}

	internal class MenusSubSection : CTCSubSection
	{
		public MenusSubSection(MenuGroup[] group)
		{
			this.menus = group;
		}

		public MenuGroup[] Menus
		{
			get { return menus; }
		} MenuGroup[] menus;
	}

	internal class NewGroup : CTCSubSection
	{
		public NewGroup(CommandID group, CommandID parent, int priority)
		{
			this.group = group;
			this.parent = parent;
			this.priority = priority;
		}

		public CommandID Group
		{
			get { return group; }
		} CommandID group;

		public CommandID Parent
		{
			get { return parent; }
		} CommandID parent;

		public int Priority
		{
			get { return priority; }
		} int priority;

	}

	internal class NewGroupsSubSection : CTCSubSection
	{
		public NewGroupsSubSection(NewGroup[] groups)
		{
			this.newGroups = groups;
		}

		public NewGroup[] NewGroups
		{
			get { return newGroups; }
		} NewGroup[] newGroups;

	}

	internal class Button
	{
		public Button(CommandID command, CommandID group, int priority, CommandID icon, CommandType type, CommandFlag flags, string text)
		{
			this.command = command;
			this.group = group;
			this.priority = priority;
			if (icon == null)
			{
				this.icon = Util.NoIcon;
			}
			else
			{
				this.icon = icon;
			}
			this.commandType = type;
			this.flags = flags;
			if (string.IsNullOrEmpty(text))
			{
				this.text = Properties.Resources.CTC_LabelUnknown;
			}
			else
			{
				this.text = text;
			}
		}

		public CommandID Command
		{
			get { return command; }
		} CommandID command;

		public CommandID Group
		{
			get { return group; }
            set { group = value; }
		} CommandID group;

		public int Priority
		{
			get { return priority; }
		} int priority;

		public CommandID Icon
		{
			get { return icon; }
		} CommandID icon;

		public CommandType CommandType
		{
			get { return commandType; }
		} CommandType commandType;

		public CommandFlag Flags
		{
			get { return flags; }
		} CommandFlag flags;

		public string Text
		{
			get { return text; }
		} string text;
	}

	internal class ButtonsSubSection : CTCSubSection
	{
		public ButtonsSubSection(Button[] buttons)
		{
			this.buttons = buttons;
		}

		public Button[] Buttons
		{
			get { return buttons; }
		} Button[] buttons;
	}

    internal class CTCBitmap
    {
        private CommandID command;
	    public CommandID Command
	    {
		    get { return command;}
	    }

        private ImageList imageList;
        public CTCBitmap(CommandID command)
        {
            this.command = command;
            this.imageList = new ImageList();
			this.imageList.ImageSize = new Size(16, 16);
			this.imageList.ColorDepth = ColorDepth.Depth24Bit;
			this.imageList.TransparentColor = Color.Transparent;
        }

		public int Add(Icon icon)
		{
			int id = this.Images.Count + 1;
			this.imageList.Images.Add(id.ToString(), icon);
			return id;
		}

		public ImageList.ImageCollection Images
		{
			get
			{
				return this.imageList.Images;
			}
		}

		public const int ImageSize = 16;

		public Bitmap Bitmap
		{
			get
			{
				if (this.imageList.Images.Count==0)
				{
					return null;
				}
				Bitmap bitmap = new Bitmap(imageList.Images.Count * ImageSize, ImageSize);
				using (Graphics g = Graphics.FromImage(bitmap))
				{
					int x = 0;
					foreach (Image image in imageList.Images)
					{
						g.DrawImage(image, x, 0);
						x += ImageSize;
					}
				}
				bitmap.MakeTransparent();
				return bitmap;
			}
		}
	}

	internal class BitmapsSubSection : CTCSubSection
	{
        private CTCBitmap[] bitmaps;

        public CTCBitmap[] Bitmaps
        {
            get { return bitmaps; }
        }

        public BitmapsSubSection(CTCBitmap[] bitmaps)
        {
            this.bitmaps = bitmaps;
        }
	}

	internal class CMDSSection : CTCSection
	{
		public CMDSSection(Guid package, MenusSubSection menus, NewGroupsSubSection groups, ButtonsSubSection buttons, BitmapsSubSection bitmaps)
		{
			this.packageGuid = package;
			this.menusSubSection = menus;
			this.newGroupsSubSection = groups;
			this.buttonsSubSection = buttons;
			this.bitmapsSubSection = bitmaps;
		}

		public Guid PackageGuid
		{
			get { return packageGuid; }
		} Guid packageGuid;

		public MenusSubSection MenusSubSection
		{
			get { return menusSubSection; }
		} MenusSubSection menusSubSection;

		public NewGroupsSubSection NewGroupsSubSection
		{
			get { return newGroupsSubSection; }
		} NewGroupsSubSection newGroupsSubSection;

		public ButtonsSubSection ButtonsSubSection
		{
			get { return buttonsSubSection; }
		} ButtonsSubSection buttonsSubSection;

		public BitmapsSubSection BitmapsSubSection
		{
			get { return bitmapsSubSection; }
		} BitmapsSubSection bitmapsSubSection;
	}

	internal class CMDUsedSection : CTCSection
	{
	}

    internal class CMDPlacement
    {
        public CMDPlacement(CommandID command, CommandID bar, int priority)
        {
            this.command = command;
            this.parent = bar;
            this.priority = priority;
        }

        public CommandID Command
        {
            get { return command;  }
        } CommandID command;

        public CommandID Parent
        {
            get { return parent; }
        } CommandID parent;

        public int Priority
        {
            get { return priority; }
        } int priority;

    }

	internal class CMDPlacementSection : CTCSection
	{
        public CMDPlacementSection(CMDPlacement[] placements)
        {
            this.placements = placements;
        }

        public CMDPlacement[] Placements
        {
            get { return placements; }
        } CMDPlacement[] placements;
	}

    internal class Visibility
    {
        public Visibility(CommandID command,Guid context)
        {
            this.command = command;
            this.context = context;
        }

        public CommandID Command
        {
            get { return command; }
        } CommandID command;

        public Guid Context
        {
            get { return context; }
        } Guid context;
    }

	internal class VisibilitySection : CTCSection
	{
        public VisibilitySection(Visibility[] visibility)
        {
            this.visibilities = visibility;
        }

        public Visibility[] Visibilities
        {
            get { return visibilities; }
        } Visibility[] visibilities;
	}

	internal class KeybindingsSection : CTCSection
	{
	}

	#endregion
}
