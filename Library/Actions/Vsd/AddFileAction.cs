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

#region Using directives

using System;
using System.IO;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions.Vsd
{
    /// <summary>
    /// Action that adds a file to a setup project. The 2 input 
    /// properties used are: (a) SetupProject - the setup project 
    /// where the file is going to be added (b) InputFileName - the
    /// name of the file to add (c) Folder - the folder where the
    /// file is going to be installed
    /// </summary>
    public class AddFileAction : Action
    {
        #region Input Properties

        /// <summary>
        /// The setup project where the file is going to be added
        /// </summary>
        [Input(Required=true)]
        public EnvDTE.Project SetupProject
        {
            get { return setupProject; }
            set { setupProject = value; }
        } EnvDTE.Project setupProject;

        /// <summary>
        /// The name of the file to add
        /// </summary>
        [Input(Required=true)]
        public string InputFileName
        {
            get { return inputFileName; }
            set { inputFileName = value; }
        } string inputFileName;

        /// <summary>
        /// The folder where the file is going to be installed
        /// </summary>
        [Input]
        public object Folder
        {
            get { return vsdFolder; }
            set { vsdFolder = (IVsdFolder)value; }
        } IVsdFolder vsdFolder;

        #endregion

        #region Output Propeties

        /// <summary>
        /// Object to the added file by the execution of the action
        /// </summary>
        [Output]
        public object File
        {
            get { return file; }
            set { file = value; }
        } object file;

        #endregion

        #region Private Implementation

        private void AddSimpleFile()
        {
            IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
            IVsdProject project = (IVsdProject)deployable.GetParent();
            IVsdCollectionSubset plugins = deployable.GetPlugIns();
            IVsdFilePlugIn filePlugin = plugins.Item("File") as IVsdFilePlugIn;
            IVsdFile vsdFile =
                (IVsdFile)DteHelper.CoCreateInstance(
					this.Site,
                    typeof(VsdFileClass),
                    typeof(IVsdFile));
            vsdFile.TargetName = Path.GetFileName(this.InputFileName);
            vsdFile.SourcePath = this.InputFileName;
            vsdFile.Folder = this.vsdFolder;
            filePlugin.Items.Add(vsdFile);
            file = vsdFile;
        }

        private void DeleteSimpleFile()
        {
            IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
            IVsdProject project = (IVsdProject)deployable.GetParent();
            IVsdCollectionSubset plugins = deployable.GetPlugIns();
            IVsdFilePlugIn filePlugin = plugins.Item("File") as IVsdFilePlugIn;
            filePlugin.Items.RemoveObject(File);
            File = null;
        }

        private void AddMergeModule()
        {
            IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
            IVsdProject project = (IVsdProject)deployable.GetParent();
            IVsdCollectionSubset plugins = deployable.GetPlugIns();
            IVsdMergeModulePlugIn modulePlugin = plugins.Item("MergeModule") as IVsdMergeModulePlugIn;
            IVsdModule vsdModule =
                (IVsdModule)DteHelper.CoCreateInstance(
					this.Site,
                    typeof(VsdModuleClass),
                    typeof(IVsdModule));
            vsdModule.UseDynamicProperties = true;
            vsdModule.SourcePath = this.InputFileName;
            vsdModule.Folder = this.vsdFolder;
            modulePlugin.Items.Add(vsdModule);
            file = vsdModule;
        }

        private void DeleteMergeModule()
        {
            IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
            IVsdProject project = (IVsdProject)deployable.GetParent();
            IVsdCollectionSubset plugins = deployable.GetPlugIns();
            IVsdMergeModulePlugIn modulePlugin = plugins.Item("MergeModule") as IVsdMergeModulePlugIn;
            modulePlugin.Items.RemoveObject(File);
            File = null;
        }

        #endregion

        #region IAction Members

        /// <summary>
        /// Adds the new file to the setup project
        /// </summary>
        public override void Execute()
        {
            IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
            IVsdProject project = (IVsdProject)deployable.GetParent();
            if (Folder == null)
            {
                project.AddFile(InputFileName);
            }
            else
            {
                if (Path.GetExtension(this.InputFileName).Equals(".msm"))
                {
                    AddMergeModule();
                }
                else
                {
                    AddSimpleFile();
                }
            }
        }

        /// <summary>
        /// Deletes the file added by the execution of the action
        /// </summary>
        public override void Undo()
        {
            if (File != null)
            {
                if (file is IVsdModule)
                {
                    DeleteMergeModule();
                }
                else
                {
                    DeleteSimpleFile();
                }
            }
        }

        #endregion
    }
}
