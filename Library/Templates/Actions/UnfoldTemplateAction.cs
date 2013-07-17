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
using System.Collections.Generic;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using EnvDTE80;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using Microsoft.Practices.Common.Services;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Templates.Actions
{
    /// <summary>
    /// Unfolds a Visual Studio Template
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    [ServiceDependency(typeof(ITypeResolutionService))]
    public class UnfoldTemplateAction: ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// The root object where the template will be unfolded.
        /// This object could be a <see cref="ProjectItem"/>, a <see cref="Project"/> or null
        /// </summary>
        [Input(Required=false)]
        public object Root
        {
            get { return root;  }
            set { root = value; }
        } object root;

        /// <summary>
        /// The .vstemplate file to unfold
        /// </summary>
        [Input(Required=true)]
        public string Template
        {
            get 
            {
                if (!File.Exists(template))
                {
                    TypeResolutionService typeResService =
                        (TypeResolutionService)GetService(typeof(ITypeResolutionService));
                    if (typeResService != null)
                    {
                        template = new FileInfo(System.IO.Path.Combine(
                            typeResService.BasePath + @"\Templates\", template)).FullName;
                    }
                }
                return template;  
            }
            set { template = value; }
        } string template;

        /// <summary>
        /// The name of the new item after unfold
        /// </summary>
        [Input(Required=true)]
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        } string itemName;

        /// <summary>
        /// The physical locatiuon of the a project template been unfolded
        /// </summary>
        [Input(Required=false)]
        public string DestinationFolder
        {
            get { return destFolder; }
            set { destFolder = value; }
        } string destFolder;

        /// <summary>
        /// The path whitin a <see cref="Project"/> where the item template will be unfolded 
        /// </summary>
        [Input(Required=false)]
        public string Path
        {
            get { return path; }
            set { path = value; }
        } string path = string.Empty;

        #endregion

        #region Output Properties

        /// <summary>
        /// The new item just created, this object can be a <see cref="Project"/> or a <see cref="ProjectItem"/>
        /// </summary>
        [Output]
        public object NewItem
        {
            get { return newItem; }
            set { newItem = value; }
        } object newItem;

        #endregion

        #region IAction members

        /// <summary>
        /// Unfolds the template
        /// </summary>
        public override void Execute()
        {
            if (Root == null || Root is EnvDTE.Solution)
            {
                AddProjectTemplate(null);
            }
            else if (Root is Project && ((Project)Root).Object is SolutionFolder)
            {
                AddProjectTemplate((Project)Root);
            }
            else if(Root is SolutionFolder)
            {
                AddProjectTemplate(((SolutionFolder)Root).Parent);
            }
            else if (Root is Project)
            {
                AddItemTemplate((Project)Root);
            }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override void Undo()
        {
        }

        #endregion

        #region Private Implementation

        private void AddItemTemplate(Project rootproject)
        {
            DTE dte=(DTE)GetService(typeof(DTE));
            string projectPath = DteHelper.BuildPath(rootproject);
            string folderPath = projectPath;
            if (!string.IsNullOrEmpty(this.Path))
            {
                folderPath += "\\" + this.Path;
            }
            ProjectItem prItem = DteHelper.FindItemByPath(dte.Solution, folderPath);
            if (prItem != null)
            {
                this.NewItem =
                    prItem.ProjectItems.AddFromTemplate(this.Template, this.ItemName);
            }
            else
            {
                Project project = DteHelper.FindProjectByPath(dte.Solution, folderPath);
                if (project != null)
                {
	          this.NewItem =
                    project.ProjectItems.AddFromTemplate(this.Template, this.ItemName);
                }
                else
                {
                    throw new InvalidOperationException(
                        String.Format("Cannot find insertion point {0}",folderPath));
                }
            }
        }

        private void AddProjectTemplate(Project project)
        {
            DTE dte=(DTE)GetService(typeof(DTE));
            SolutionFolder slnFolder = null;

            if (project == null)
            {
                if (string.IsNullOrEmpty(this.Path))
                {
                    this.NewItem = dte.Solution.AddFromTemplate(this.Template, this.DestinationFolder, this.ItemName, false);
                }
                else
                {
                    Project subProject = DteHelper.FindProjectByPath(dte.Solution, this.Path);
                    slnFolder = (SolutionFolder)subProject.Object;
                    this.NewItem = slnFolder.AddFromTemplate(this.Template, this.DestinationFolder, this.ItemName);
                }
            }
            else
            {
                slnFolder = (SolutionFolder)project.Object;
                this.NewItem = slnFolder.AddFromTemplate(this.Template, this.DestinationFolder, this.ItemName);
            }

            if(this.newItem == null)
            {
                //Return the project already added if the AddFromTemplate method returns null
                ProjectItems childItems;

                if (slnFolder != null)
                {
                    childItems = slnFolder.Parent.ProjectItems;
                }
                else
                {
                    childItems = (ProjectItems)dte.Solution.Projects;
                }

                foreach(ProjectItem item in childItems)
                {
                    if(item.Name.Contains(this.ItemName))
                    {
                        this.NewItem = item.Object as Project;
                        break;
                    }                        
                }
            }
        }

        #endregion
    }
}
