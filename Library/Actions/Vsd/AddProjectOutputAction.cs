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
using EnvDTE;
using System.Diagnostics;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions.Vsd
{
    /// <summary>
    /// Action that adds a project output to the setup project
    /// It has 4 input properties: (a) SetupProject - that is 
    /// the setup project to be changed (b) InputProject - the 
    /// project which its output will be added in the set up
    /// project (c) ProjectOutputKind - the kind of output that
    /// will be used (d) Folder - the folder name that the project
    /// will be located when the installation occurs
    /// </summary>
    public class AddProjectOutputAction : Action
    {
        #region Input Properties

        /// <summary>
        /// Setup project to be modified
        /// </summary>
        [Input(Required=true)]
        public EnvDTE.Project SetupProject
        {
            get { return setupProject; }
            set { setupProject = value; }
        } EnvDTE.Project setupProject;

        /// <summary>
        /// Project which its output will be 
        /// added as part of the setup project
        /// </summary>
        [Input(Required=true)]
        public EnvDTE.Project InputProject
        {
            get { return inputProject; }
            set { inputProject = value; }
        } EnvDTE.Project inputProject;

        enum ProjectOutputKindEnum
        {
            LocalizedResourceDlls,
            XmlSerializer,
            ContentFiles,
            Built,
            SourceFiles,
            Symbols,
            Documentation
        }

        /// <summary>
        /// Project Output Kind that will be used of 
        /// the specified project
        /// </summary>
        [Input(Required=true)]
        public string ProjectOutputKind
        {
            get { return projectOutputKind; }
            set { projectOutputKind = value; }
        } string projectOutputKind;

        /// <summary>
        /// Folder object where the output
        /// will be added
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
        /// Object of the project output that was created
        /// </summary>
        [Output]
        public object ProjectOutput
        {
            get { return projectOutput; }
            set { projectOutput = (IVsdProjectOutputGroup)value; }
        } IVsdProjectOutputGroup projectOutput;

        #endregion

        #region IAction Members

        /// <summary>
        /// Adds the new output from the specified parameters
        /// </summary>
        public override void Execute()
        {
            IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
            IVsdProject project = (IVsdProject)deployable.GetParent();
            if (Folder == null)
            {
                project.AddOutputGroup(ProjectOutputKind, InputProject.UniqueName);
            }
            else // Folder is not null, so let's add it manually
            {
                IVsdCollectionSubset plugins = deployable.GetPlugIns();
                IVsdProjectOutputPlugIn projectsPlugin = plugins.Item("ProjectOutput") as IVsdProjectOutputPlugIn;
                projectOutput =
                    (IVsdProjectOutputGroup)DteHelper.CoCreateInstance(
						this.Site, 
                        typeof(VsdProjectOutputGroupClass),
                        typeof(IVsdProjectOutputGroup));
                //projectOutput.OutputConfig = null;
                projectOutput.OutputGroup = ProjectOutputKind;
                projectOutput.OutputProject = InputProject.UniqueName;
                projectOutput.ShowKeyOutput = true;
                projectOutput.Folder = vsdFolder;
                projectsPlugin.Items.Add(projectOutput);
            }
            // Collapse to project definition.
            UIHierarchyItem uiitem = DteHelper.SelectItem(SetupProject.DTE,
                DteHelper.BuildPath(SetupProject));
            if (uiitem != null)
            {
                uiitem.UIHierarchyItems.Expanded = false;
            }
        }

        /// <summary>
        /// Removes the project output that was recently added
        /// </summary>
        public override void Undo()
        {
            if (projectOutput != null)
            {
                IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
                IVsdCollectionSubset plugins = deployable.GetPlugIns();
                IVsdProjectOutputPlugIn projectsPlugin = plugins.Item("ProjectOutput") as IVsdProjectOutputPlugIn;
                projectsPlugin.Items.RemoveObject(projectOutput);
                projectOutput = null;
            }
        }

        #endregion
    }
}
