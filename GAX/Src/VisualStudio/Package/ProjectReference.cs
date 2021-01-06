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
using System.Runtime.Serialization;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using System.Diagnostics;
using System.Security.Permissions;
using EnvDTE80;
using VsWebSite;
using System.IO;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
    /// <summary>
    /// Represents a recipe reference that points to a project in the solution.
    /// </summary>
    /// <seealso cref="RecipeReference"/>
    [Serializable]
    [System.Drawing.ToolboxBitmap(typeof(ProjectReference))]
    public class ProjectReference : VsBoundReference
    {
        IBoundReferenceLocatorStrategy strategy;

        /// <summary>
        /// Initializes an instance of the <see cref="ProjectReference"/> class.
        /// </summary>
        /// <seealso cref="RecipeReference"/>
        public ProjectReference(string recipe, Project project)
            : base(recipe, project)
        {
            if (project.Kind == PrjKind.prjKindVenusProject)
            {
                WebProjectStrategy webStrategy = new WebProjectStrategy();
                webStrategy.IssueWebSiteLocationWarning(project);
                strategy = webStrategy;
            }
            else
            {
                strategy = new ProjectStrategy();
            }
        }

        /// <summary>
        /// Initializes tracking of the associated item.
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            LocateProject();
        }

        /// <summary>
        /// Gets the string represeting the target object
        /// </summary>
        public override string AppliesTo
        {
            get
            {
                try
                {
                    if (Target != null)
                    {
                        return strategy.GetAppliesTo((Project)Target);
                    }
                    else
                    {
                        return serializedData;
                    }
                }
                catch
                {
                    return serializedData;
                }
            }
        }

        /// <summary>
        /// Returns an <see cref="IBoundReferenceLocatorStrategy"/> object
        /// </summary>
        public override IBoundReferenceLocatorStrategy Strategy
        {
            get { return strategy; }
        }

        /// <summary>
        /// Overriding SetTarget to mark
        /// our internal property if it was a solutionfolder
        /// or a real project
        /// </summary>
        /// <param name="target"></param>
        protected internal override void SetTarget(object target)
        {
            base.SetTarget(target);
            // We have to save in our persistent data becuase it won't be a way
            // of knowing when trying to restore if this was a solutionfolder
            // or a real project			
            Project project = target as Project;
            isSolutionFolder = target is SolutionFolder ||
                (project != null && project.Object is SolutionFolder);
        }

        /// <summary>
        /// Property to specify that if the bind is a Solution Folder
        /// or a real project
        /// </summary>
        internal bool IsSolutionFolder
        {
            get
            {
                return isSolutionFolder;
            }
        }
        private bool isSolutionFolder;

        /// <summary>
        /// Called upon siting if the object was constructed from serialized 
        /// state. In this case the target will be null, and the 
        /// serializedData field will contain the path of the project.
        /// </summary>
        private void LocateProject()
        {
            if (Target != null)
            {
                return;
            }
            IServiceProvider vs = GetService<IServiceProvider>(true);
            object target = strategy.LocateTarget(vs, serializedData);
            if (target != null)
            {
                SetTarget(target);
            }
            else
            {
                throw new ArgumentException(String.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.BoundReference_InvalidItem, serializedData));
            }
        }

        #region ISerializable Members

        /// <summary>
        /// Initializes an instance of the <see cref="ProjectReference"/> class.
        /// </summary>
        /// <seealso cref="IAssetReference"/>
        protected ProjectReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info.GetBoolean("IsWebProject"))
            {
                strategy = new WebProjectStrategy();
            }
            else
            {
                strategy = new ProjectStrategy();
                isSolutionFolder = info.GetBoolean("solutionFolder");
            }
        }

        /// <summary>
        /// <seealso cref="ISerializable.GetObjectData"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (Target != null)
            {
                base.serializedData = strategy.GetSerializationData(Target);
            }

            info.AddValue("solutionFolder", isSolutionFolder);
            if (strategy is WebProjectStrategy)
            {
                info.AddValue("IsWebProject", true);
            }
            else
            {
                info.AddValue("IsWebProject", false);
            }

            // Let the base class save the serializedData value.
            base.GetObjectData(info, context);
        }

        #endregion ISerializable Members

        internal class WebProjectStrategy : IBoundReferenceLocatorStrategy
        {
            public string GetAppliesTo(object target)
            {
                Project targetProject = target as Project;
                if (targetProject == null)
                    throw new ArgumentException("target");

                return targetProject.ParentProjectItem != null ?
                    targetProject.ParentProjectItem.Name :
                    targetProject.Name;
            }

            public string GetSerializationData(object target)
            {
                Project targetProject = target as Project;
                if (targetProject == null)
                    throw new ArgumentException("target");

                VSWebSite targetSite = targetProject.Object as VSWebSite;
                webType kind = (webType)targetProject.Properties.Item("WebSiteType").Value;

                string location = targetProject.UniqueName;

                if (kind == webType.webTypeFileSystem)
                {
                    string slnPath = Path.GetDirectoryName(targetProject.DTE.Solution.FileName);
                    if (targetProject.UniqueName.StartsWith(slnPath))
                    {
                        location = targetProject.UniqueName.Replace(slnPath, "");
                    }
                    // If not under the solution, we will persist the full path. It will 
                    // work as long as the developer does not move the website around. 
                    // A warning is issued at connection or construction time.
                }

                return String.Join("|", new string[] { kind.ToString(), location });
            }

            public object LocateTarget(IServiceProvider serviceProvider, string serializedData)
            {
                DTE vs = (DTE)serviceProvider.GetService(typeof(DTE));
                string[] values = serializedData.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                webType kind = (webType)Enum.Parse(typeof(webType), values[0]);
                string location = values[1];

                string uniqueName = location;
                if (kind == webType.webTypeFileSystem && !HasDriveSpecification(location))
                {
                    uniqueName = Path.GetDirectoryName(vs.Solution.FileName) + location;
                }

                Project project = DteHelper.FindProject(vs, delegate(Project p)
                {
                    return string.Equals(p.UniqueName, uniqueName, StringComparison.OrdinalIgnoreCase);
                });

                return project;
            }

            internal void IssueWebSiteLocationWarning(Project webSite)
            {
                webType kind = (webType)webSite.Properties.Item("WebSiteType").Value;
                if (kind == webType.webTypeFileSystem &&
                    HasDriveSpecification(webSite.UniqueName) &&
                    /* During the unfold process, the solution does not have a filename */
                    webSite.DTE.Solution.FileName.Length > 0 &&
                    !webSite.UniqueName.StartsWith(Path.GetDirectoryName(webSite.DTE.Solution.FileName)))
                {
                     TraceUtil.TraceWarning(TraceUtil.GaxTraceSourceName, 
						 Properties.Resources.WebProjectReference_FileSystemWebNotUnderSolution,
                        webSite.UniqueName);
                }
            }

            private bool HasDriveSpecification(string location)
            {
                return location.Length > 0 && Char.IsLetter(location[0]) && location.IndexOf(':') != -1;
            }
        }

        internal class ProjectStrategy : IBoundReferenceLocatorStrategy
        {
            public string GetAppliesTo(object target)
            {
                return DteHelper.BuildPath(target);
            }

            public string GetSerializationData(object target)
            {
                return DteHelper.BuildPath(target);
            }

            public object LocateTarget(IServiceProvider serviceProvider, string serializedData)
            {
                DTE vs = (DTE)serviceProvider.GetService(typeof(DTE));
                return DteHelper.FindProjectByPath(vs.Solution, serializedData);

            }
        }
    }
}
