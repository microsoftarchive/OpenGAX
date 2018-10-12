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
using System.Linq;
using System.Collections;
using System.ComponentModel.Design;
using System.IO;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using Microsoft.VisualStudio.Shell.Interop;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;

#endregion

////
////  This is a template of a CTC generated from a guidance package.
////  The constants have been replace by #define for readability.
////  hostdata.menu define the top level menus placed on ide context menu (specified by commandbar.name).
////  recipe defines the command, recipe.hostdata.commandbar specify where the cmd is placed, 
////  if use Name, then it is diectly placed on  context menu (specified by commandbar.name), 
////  if use Menu, then it is placed on a submenu, which in turn placed on  context menu (specified by commandbar.name).
////
//#define PRI_0  0x001
//#define PRI_1  0x010
//#define PRI_2  0x400
//#define DEF_BT
////#define DEF_BT DYNAMICVISIBILITY | DEFAULTDISABLED | DEFAULTINVISIBLE | COMMANDWELLONLY
//#define myGuid guidBscPkgMenuGrp
//#define Group_Undefined   { 0x83285929, 0x227c, 0x11d3, { 0xb8, 0x70, 0x00, 0xc0, 0x4f, 0x79, 0xf8, 0x02 } }
//CMDS_SECTION guidBscPkgPkg
//MENUS_BEGIN
//    myGuid:0x20000,myGuid:0x10003,PRI_2,0x00000400,"SlnMenu","My Solution Cascade Menu";
//    myGuid:0x20001,myGuid:0x10004,PRI_0,0x00000400,"NestedSlnMenu","My nested Solution Cascade Menu";
//MENUS_END
//NEWGROUPS_BEGIN
//    // GAT Package group
//    myGuid:0x10000,guidSHLMainMenu:IDM_VS_MENU_TOOLS,0x0;
//    // Template Package group
//    myGuid:0x10100,guidSHLMainMenu:IDM_VS_MENU_TOOLS,0x0;
//    //Command bar groups
//    myGuid:0x10003,guidSHLMainMenu:0x0413,PRI_0;		//Group for Solution root
//    myGuid:0x10004,myGuid:0x20000,PRI_1;				//Group for SlnMenu
//    myGuid:0x10005,myGuid:0x20001,PRI_2;				//Group for NestedSlnMenu
//    myGuid:0x10006,guidSHLMainMenu:0x414,PRI_0;			//Group for Solution folder
//NEWGROUPS_END
//BUTTONS_BEGIN
//    myGuid:0x0100,myGuid:0x10000,PRI_0,guidOfficeIcon:0x015A,BUTTON,DEF_BT,"CascadingMenuTestCluster.RecipeInSolutionContextMenuOne";
//    myGuid:0x0200,myGuid:0x10100,PRI_1,guidOfficeIcon:0x015A,BUTTON,DEF_BT,"CascadingMenuTestCluster.RecipeInSolutionContextMenuTwo";
//    myGuid:0x0300,myGuid:0x10000,PRI_2,guidOfficeIcon:0x015A,BUTTON,DEF_BT,"CascadingMenuTestCluster.RecipeInSolutionContextMenuThree";
//BUTTONS_END
//BITMAPS_BEGIN
//BITMAPS_END
//CMDS_END
//CMDUSED_SECTION
//CMDUSED_END
//CMDPLACEMENT_SECTION
//// Command placements
//myGuid:0x0100,myGuid:0x10003,PRI_0; // SlnRoot
//myGuid:0x0200,myGuid:0x10004,PRI_1; // SlnRoot|Menu
//myGuid:0x0300,myGuid:0x10005,PRI_2; // SlnRoot|Menu|Menu
//myGuid:0x0100,myGuid:0x10006,PRI_2; // SlnFolder
//myGuid:0x0200,myGuid:0x10006,PRI_1; // SlnFolder
//myGuid:0x0300,myGuid:0x10006,PRI_0; // SlnFolder
//CMDPLACEMENT_END
//VISIBILITY_SECTION
//VISIBILITY_END
//KEYBINDINGS_SECTION
//KEYBINDINGS_END

namespace Microsoft.Practices.RecipeFramework.VisualStudio.CTC
{
    /// <summary>
    /// This class builds a CTC tree give the configuration of the guidance package
    /// </summary>
    internal class CTCBuilder
    {
        const int defaultPriority = 0x600;
        Configuration.GuidancePackage guidancePackage;
        string basePath;
        Guid guidRecipeFrameworkPkg;
        Guid guidRecipeFrameworkPkgCmdSet;
        CommandID defaultParentCommand;
        CommandID defaultIcon;
        CTCBitmap ctcBitmap;
        Dictionary<string, NewGroup> newGroups;
        Dictionary<string, MenuGroup> menus;
        Hashtable placements;
        ArrayList buttons;
        ArrayList visibilities;
        int groupCounter;
        int menuCounter;
        NewGroup packageGroup;
        RegistryKey regRoot = null;
        IEnumerable<IVsTemplate> templates;

        public CTCBuilder(Configuration.GuidancePackage package, string basePath, RegistryKey regRoot)
            :this(package, basePath, regRoot, null)
        { }

        public CTCBuilder(Configuration.GuidancePackage package, string basePath, RegistryKey regRoot, IEnumerable<IVsTemplate> templates)
        {
            this.guidancePackage = package;
            this.templates = templates;
            this.basePath = basePath;
            this.guidRecipeFrameworkPkg = typeof(RecipeManagerPackage).GUID;
            this.guidRecipeFrameworkPkgCmdSet = new Guid(guidancePackage.Guid);
            this.groupCounter = 0x10000;
            this.menuCounter = 0x20000;
            this.defaultParentCommand = new CommandID(ShellCmdDef.guidSHLMainMenu, ShellCmdDef.IDM_VS_MENU_TOOLS);
            this.ctcBitmap = new CTCBitmap(new CommandID(this.guidRecipeFrameworkPkgCmdSet, 100));
            this.newGroups = new Dictionary<string, NewGroup>(7);
            this.menus = new Dictionary<string, MenuGroup>(7);
            this.placements = new Hashtable(7);
            this.buttons = new ArrayList();
            this.visibilities = new ArrayList();
            this.defaultIcon = new CommandID(this.guidRecipeFrameworkPkg, ShellCmdDef.bmpidGaxNoIcon);
            this.regRoot = regRoot;
            if (package.HostData != null && package.HostData.Icon != null)
            {
                this.defaultIcon = CreateBitmap(package.HostData.Icon);
            }
        }

        private CommandID GetIcon(Configuration.Icon icon)
        {
            if (icon != null)
            {
                Guid iconGuid = this.defaultIcon.Guid;
                int iconId = this.defaultIcon.ID;
                if (icon.IDSpecified)
                {
                    iconId = icon.ID;
                    if (icon.Guid == null)
                    {
                        iconGuid = ShellCmdDef.guidOfficeIcon;
                    }
                }
                if (icon.Guid != null)
                {
                    iconGuid = new Guid(icon.Guid);
                }
                return ShellCmdDef.MapIconToBitmap(new CommandID(iconGuid,iconId));
            }
            else
            {
                return this.defaultIcon;
            }
        }


        private void AddCommand(Button button, Configuration.RecipeHostData hostData)
        {
            buttons.Add(button);
            //Uncomment if you need to support visibility for a command
            //visibilities.Add(new Visibility(button.Command, new Guid(UIContextGuids.SolutionExists)));
            if (hostData != null && hostData.CommandBar != null)
            {
                if (hostData.CommandBar.Length == 0)
                {
                    // Check that we always get at least one command bar
                    return;
                }
                Dictionary<CommandID, string> installedBars = new Dictionary<CommandID, string>();
                for (int iCommand = 0; iCommand < hostData.CommandBar.Length; iCommand++)
                {
                    Configuration.CommandBar configCommandBar = hostData.CommandBar[iCommand];
                    CommandID commandBar = GetCommandID(configCommandBar);
                    CommandID parentCommandBar = GetCommandParentID(configCommandBar);
                    if (installedBars.ContainsKey(parentCommandBar))
                    {
                        throw new InvalidOperationException(
                            String.Format(Properties.Resources.CTC_RepeatedCommandBar, parentCommandBar.ToString()));
                    }
                    else
                    {
                        AddGroupPlacement(button, commandBar);
                        installedBars.Add(parentCommandBar, commandBar.ToString());
                    }
                }
            }
        }

        private void BuiltRecipeCommands()
        {
            if (guidancePackage.Recipes == null)
            {
                return;
            }
            for (int iRecipe = 0; iRecipe < guidancePackage.Recipes.Length; iRecipe++)
            {
                Configuration.Recipe recipe = guidancePackage.Recipes[iRecipe];
                if (recipe.HostData != null && recipe.HostData.CommandBar != null)
                {
                    CommandID myCommand = new CommandID(guidRecipeFrameworkPkgCmdSet, (iRecipe + 1) * 0x100);
                    CommandID icon = CreateBitmap(recipe.HostData.Icon);
                    Button myButton = new Button(myCommand,
                        this.PackageGroup.Group,
                        recipe.HostData.Priority,
                        icon,
                        CommandType.Button,
                        Util.DisabledDefault,
                        guidancePackage.Caption + "." + recipe.Name);
                    AddCommand(myButton, recipe.HostData);
                }
            }
        }

        private NewGroup GetNewGroup(string commandBar)
        {
            if (newGroups.ContainsKey(commandBar))
            {
                return newGroups[commandBar];
            }
            return null;
        }

        private NewGroup CreateNewGroup(string groupName, int priority, CommandID parentCommand)
        {
            if (!newGroups.ContainsKey(groupName))
            {
                CommandID newGroupCommand = new CommandID(guidRecipeFrameworkPkgCmdSet, this.groupCounter++);
                NewGroup myNewGroup = new NewGroup(
                    newGroupCommand,
                    parentCommand,
                    priority);
                newGroups.Add(groupName, myNewGroup);
            }
            return newGroups[groupName];
        }

        private NewGroup CreateNewGroup(string groupName, int priority)
        {
            return CreateNewGroup(groupName, priority, this.defaultParentCommand);
        }

        private void AddGroupPlacement(Button button, CommandID commandBar)
        {
            string key = button.Command.ToString() + commandBar.ToString();
            if (!placements.ContainsKey(key))
            {
                // Create or get the gruop associted with commandBar
                NewGroup newGroup = CreateNewGroup(commandBar.ToString(), button.Priority, commandBar);
                CMDPlacement placement = new CMDPlacement(button.Command, newGroup.Group, button.Priority);
                placements.Add(key, placement);
            }
        }

        public CTCFile Create()
        {
            #region Standard group Init
            // Make sure to initialize the default groups
            this.packageGroup = this.PackageGroup;
            #endregion

            #region New Menu creation
            // Create the new menus first before creating any command
            BuildMenuGroups();
            #endregion

            #region Command creation
            // Templates commands must be created first than recipe commands
            // becuase a template can steal the command bars from the recipe it is related to
            BuildTemplateCommands();
            // Build recipes commands
            BuiltRecipeCommands();
            #endregion

            #region Build CTC Tree
            NewGroup[] newGroupsArray = new NewGroup[newGroups.Values.Count];
            newGroups.Values.CopyTo(newGroupsArray, 0);
            MenuGroup[] menuGroupsArray = new MenuGroup[menus.Values.Count];
            menus.Values.CopyTo(menuGroupsArray, 0);
            CMDPlacement[] placementsArray = new CMDPlacement[placements.Count];
            placements.Values.CopyTo(placementsArray, 0);
            Button[] buttonsArray = new Button[buttons.Count];
            buttons.CopyTo(buttonsArray);
            Visibility[] visibiltiesArray = new Visibility[visibilities.Count];
            visibilities.CopyTo(visibiltiesArray);
            CTCFile ctcFile = new CTCFile(
                new CMDSSection(guidRecipeFrameworkPkg,
                    new MenusSubSection(menuGroupsArray),
                    new NewGroupsSubSection(newGroupsArray),
                    new ButtonsSubSection(buttonsArray),
                    new BitmapsSubSection(new CTCBitmap[] { ctcBitmap })),
                new CMDUsedSection(),
                new CMDPlacementSection(placementsArray),
                new VisibilitySection(visibiltiesArray),
                new KeybindingsSection());
            return ctcFile;
            #endregion
        }

        private void BuildMenuGroups()
        {
            if (guidancePackage.HostData != null && guidancePackage.HostData.Menu != null)
            {
                foreach (Configuration.Menu configMenu in guidancePackage.HostData.Menu)
                {
                    AddMenuGroup(configMenu);
                }
            }
        }

        private void AddMenuGroup(Configuration.Menu configMenu)
        {
            AddMenuGroup(configMenu.Name, configMenu.Text, configMenu.Priority, configMenu.CommandBar);
        }

        private CommandID GetCommandParentID(Configuration.CommandBar commandBar)
        {
            while (!string.IsNullOrEmpty(commandBar.Menu))
            {
                bool found = false;
                foreach (Configuration.Menu menu in guidancePackage.HostData.Menu)
                {
                    if (menu.Name.Equals(commandBar.Menu, StringComparison.InvariantCultureIgnoreCase))
                    {
                        commandBar = menu.CommandBar;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    throw new InvalidOperationException(
                        String.Format(Properties.Resources.CTC_MenuDoesNotExist, commandBar.Menu));
                }
            }
            return GetCommandID(commandBar);
        }

        private CommandID GetCommandID(Configuration.CommandBar commandBar)
        {
            if (commandBar.NameSpecified)
            {
                return ShellCmdDef.GetPredefinedCommandID(commandBar.Name);
            }
            else if (!string.IsNullOrEmpty(commandBar.Menu))
            {
                if (menus.ContainsKey(commandBar.Menu))
                {
                    MenuGroup menuGroup = (MenuGroup)menus[commandBar.Menu];
                    return menuGroup.Group;
                }
                else
                {
                    throw new InvalidOperationException(
                        String.Format(Properties.Resources.CTC_MenuDoesNotExist, commandBar.Menu));
                }
            }
            else
            {
                int commandId = 0;
                Guid guid = ShellCmdDef.guidSHLMainMenu;
                if (commandBar.IDSpecified)
                {
                    commandId = commandBar.ID;
                }
                if (commandBar.Guid != null)
                {
                    guid = new Guid(commandBar.Guid);
                }
                return new CommandID(guid, commandId);
            }
        }

        private NewGroup PackageGroup
        {
            get
            {
                if (packageGroup == null)
                {
                    packageGroup = CreateNewGroup("__PackageGroup", 0);
                }
                return packageGroup;
            }
        }

        private MenuGroup AddMenuGroup(string menuName, string menuText, int priority, Configuration.CommandBar commandBar)
        {
            if (!menus.ContainsKey(menuName))
            {
                CommandID commandID = GetCommandID(commandBar);
                NewGroup newGroup = CreateNewGroup(commandID.ToString(), priority, commandID);
                CommandID newMenuCommand = new CommandID(guidRecipeFrameworkPkgCmdSet, this.menuCounter++);
                MenuGroup myNewMenu = new MenuGroup(
                    newMenuCommand,
                    newGroup.Group,
                    priority,
                    CommandType.Menu,
                    menuName,
                    menuText);
                menus.Add(menuName, myNewMenu);
            }
            return menus[menuName];
        }

        private void AddTemplateCommand(IVsTemplate template)
        {
            if (template.Kind == TemplateKind.Solution)
            {
                return;
            }
            Configuration.RecipeHostData hostData = null;
            if (template.ExtensionData != null && !string.IsNullOrEmpty(template.ExtensionData.Recipe))
            {
                Configuration.Recipe recipeConfig = GetRecipeConfig(template.ExtensionData.Recipe);
                if (recipeConfig != null && recipeConfig.HostData != null && recipeConfig.HostData.CommandBar != null)
                {
                    hostData = recipeConfig.HostData;
                    // Set the host data to null to avoid registering two commands
                    recipeConfig.HostData = null;
                }
            }
            int priority = CTCBuilder.defaultPriority;
            CommandID icon = CreateBitmap(template);
            if (hostData != null)
            {
                hostData.Icon = new Configuration.Icon();
                hostData.Icon.Guid = icon.Guid.ToString();
                hostData.Icon.ID = icon.ID;
                priority = hostData.Priority;
            };
            Button myButton = new Button(template.Command,
                this.PackageGroup.Group,
                priority,
                icon,
                CommandType.Button,
                Util.DisabledDefault,
                guidancePackage.Caption + "." + template.Name);
            AddCommand(myButton, hostData);
            if (hostData == null)
            {
                // Add the template command in the standard places
                CommandID parentCommand = null;
                if (template.Kind == TemplateKind.ProjectItem)
                {
                    parentCommand = ShellCmdDef.GetPredefinedCommandID(Configuration.CommandBarName.ProjectAdd);
                    AddGroupPlacement(myButton, parentCommand);
                }
                else if (template.Kind == TemplateKind.Project)
                {
                    parentCommand = ShellCmdDef.GetPredefinedCommandID(Configuration.CommandBarName.SolutionAdd);
                    AddGroupPlacement(myButton, parentCommand);
                }
                parentCommand = ShellCmdDef.GetPredefinedCommandID(Configuration.CommandBarName.SolutionFolderAdd);
                AddGroupPlacement(myButton, parentCommand);
            }
        }

        private CommandID LoadBitmap(String fileIcon)
        {
            Debug.Print("CTC: Loading ico {0}", fileIcon);
            Icon icon = new Icon(fileIcon, CTCBitmap.ImageSize, CTCBitmap.ImageSize);
            int id = this.ctcBitmap.Add(icon);
            return new CommandID(this.guidRecipeFrameworkPkgCmdSet,id);
        }

        private CommandID CreateBitmap(Configuration.Icon icon)
        {
            if (icon != null && !string.IsNullOrEmpty(icon.File))
            {
                string fileIcon = Path.Combine(this.basePath,icon.File);
                return LoadBitmap(fileIcon);
            }
            return GetIcon(icon);
        }

        private CommandID CreateBitmap(IVsTemplate template)
        {
            if (!string.IsNullOrEmpty(template.IconFileName))
            {
                string fileIcon = new FileInfo(template.FileName).Directory + "\\" + template.IconFileName;
                return LoadBitmap(fileIcon);
            }
            return ShellCmdDef.MapIconToBitmap(template.Icon);
        }

        private Microsoft.Practices.RecipeFramework.Configuration.Recipe GetRecipeConfig(string recipeName)
        {
            foreach (Configuration.Recipe recipe in this.guidancePackage.Recipes)
            {
                if (recipe.Name.Equals(recipeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return recipe;
                }
            }
            return null;
        }

        private void BuildTemplateCommands()
        {
            if (this.templates != null)
            {
                foreach (IVsTemplate template in templates)
                {
                    AddTemplateCommand(template);
                }
            }
        }
    }
}