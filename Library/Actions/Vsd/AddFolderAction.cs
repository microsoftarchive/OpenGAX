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
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions.Vsd
{
    /// <summary>
    /// Action that adds a folder to a setup folder. This 
    /// folder is going to be physically created by the 
    /// time of installation. The 4 input properties used
    /// are: (a) SetupProject - the project where the new
    /// folder will be created (b) FolderName - the name 
    /// of the folder that will be added (c) Property - the
    /// property name that the folder will define 
    /// (d) DefaultLocation - the default location of the 
    /// folder that is the physical name by the time of the
    /// installation
    /// </summary>
    public class AddFolderAction: ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// The project where the new folder will be created
        /// </summary>
        [Input]
        public EnvDTE.Project SetupProject
        {
            get { return setupProject; }
            set { setupProject = value; }
        } EnvDTE.Project setupProject;

        /// <summary>
        /// The name of the folder that will be added
        /// </summary>
        [Input]
        public string FolderName
        {
            get { return folderName; }
            set { folderName = value; }
        } string folderName;

        /// <summary>
        /// The property name that the folder will define
        /// </summary>
        [Input]
        public string Property
        {
            get { return property; }
            set { property = value; }
        } string property;

        /// <summary>
        /// The default location of the folder that is the 
        /// physical name by the time of the installation
        /// </summary>
        [Input]
        public string DefaultLocation
        {
            get { return defaultLocation; }
            set { defaultLocation = value; }
        } string defaultLocation;

        #endregion

        #region Output Properties

        /// <summary>
        /// The object folder that is created by the execution of the action
        /// </summary>
        [Output]
        public object Folder
        {
            get { return vsdFolder; }
            set { vsdFolder = (IVsdCustomFolder)value; }
        } IVsdCustomFolder vsdFolder;

        #endregion

        #region IAction members

        /// <summary>
        /// Adds the new folder to the setup project
        /// with the given parameters
        /// </summary>
        public override void Execute()
        {
            try
            {
                IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
                IVsdCollectionSubset plugins = deployable.GetPlugIns();
                IVsdFolderPlugIn folderPlugin = plugins.Item("Folder") as IVsdFolderPlugIn;
                if (folderPlugin != null)
                {
                    vsdFolder =
                        (IVsdCustomFolder)DteHelper.CoCreateInstance(
							this.Site, 
                            typeof(VsdCustomFolderClass),
                            typeof(IVsdCustomFolder));
                    vsdFolder.Name = FolderName;
                    vsdFolder.Property_2 = this.Property;
                    vsdFolder.DefaultLocation = this.DefaultLocation;
                    folderPlugin.Items.Add(vsdFolder);
                }
            }
            catch (Exception)
            {
                vsdFolder = null;
                throw;
            }
        }

        /// <summary>
        /// Removes the folder that was created by the 
        /// execution of the action
        /// </summary>
        public override void Undo()
        {
            if (vsdFolder != null)
            {
                IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
                IVsdCollectionSubset plugins = deployable.GetPlugIns();
                IVsdFolderPlugIn folderPlugin = plugins.Item("Folder") as IVsdFolderPlugIn;
                if (folderPlugin != null)
                {
                    folderPlugin.Items.RemoveObject(vsdFolder);
                    vsdFolder = null;
                }
            }
        }

        #endregion
    }
}
