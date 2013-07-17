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

using System;
using System.Drawing;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Services;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Templates
{
    /// <summary>
    /// Base class for a bound reference to a template.
    /// </summary>
	[Serializable]
	[ToolboxBitmap(typeof(BoundTemplateReference))]
	public sealed class BoundTemplateReference: TemplateReference, IBoundAssetReference 
	{
		VsBoundReference boundReference;

        /// <summary>
        /// Constructor from the template file name and the bound reference.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="reference"></param>
		public BoundTemplateReference(string template, VsBoundReference reference):base(template)
		{
			this.boundReference = reference;	
		}

		#region IBoundAssetReference Members

		object IBoundAssetReference.Target
		{
			get { return boundReference.Target; }
		}

		string IBoundAssetReference.SubPath
		{
			get { return boundReference.SubPath; }
		}

		IBoundReferenceLocatorStrategy IBoundAssetReference.Strategy
		{
			get { return boundReference.Strategy; }
		}

		internal VsBoundReference BoundReference
		{
			get
			{
				return boundReference;
			}
		}

		#endregion

		#region ISerializable members

		private BoundTemplateReference(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			boundReference = (VsBoundReference)info.GetValue("reference", typeof(VsBoundReference));
		}
		
        /// <summary>
        /// <seealso cref="ISerializable.GetObjectData"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("reference", boundReference);
		}

		#endregion

		#region Overrides

        /// <summary>
        /// Sites the boundReference and checks that the reference is a valid.
        /// </summary>
		protected override void OnSited()
		{
			base.OnSited();
			boundReference.Site = new ComponentModel.Site((IServiceProvider)this.Site.Container, 
                this, "boundReference");

			// If trying to bound to a project item reference
			if (boundReference is ProjectItemReference)
			{
				if (boundReference.Target != null)
				{
					// If the item is not a folder
					if (String.Compare(((EnvDTE.ProjectItem)(boundReference.Target)).Kind, EnvDTE.Constants.vsProjectItemKindPhysicalFolder, true, CultureInfo.InvariantCulture) != 0)
					{
						throw new RecipeFrameworkException(String.Format(
							CultureInfo.CurrentCulture,
							Properties.Resources.Templates_ItemCantHaveTemplates,
							Template.Name));
					}
					else if (Template.Kind != TemplateKind.ProjectItem)
					{
						throw new RecipeFrameworkException(String.Format(
							CultureInfo.CurrentCulture,
							Properties.Resources.Templates_ItemCantHaveTemplates,
							Template.Name));
					}
				}
			}
			// If trying to bound to a solution reference
			else if (boundReference is SolutionReference)
			{
				if (Template.Kind == TemplateKind.ProjectItem)
				{
					throw new RecipeFrameworkException(String.Format(
							CultureInfo.CurrentCulture,
							Properties.Resources.Templates_ItemCannotBeRoot,
							Template.Name));
				}
			}
			// If trying to bound to a project reference
			else if (boundReference is ProjectReference)
			{
				if (boundReference.Target is EnvDTE.Project)
				{
					//Only project items are allowed
					EnvDTE.Project project = (EnvDTE.Project)boundReference.Target;
					if (Template.Kind == TemplateKind.ProjectItem)
					{
						Guid projectFactory = new Guid(project.Kind);
						if (!Template.ProjectFactory.Equals(projectFactory) && !(project.Object is EnvDTE80.SolutionFolder))
						{
							throw new RecipeFrameworkException(String.Format(
								CultureInfo.CurrentCulture,
								Properties.Resources.Templates_InvalidProjectForItemTemplate,
								Template.Name, project.Name));
						}
						else if (project.Object is EnvDTE80.SolutionFolder)
						{
							if (string.Compare(Template.ProjectFactory.ToString("B"), "{2150E333-8FDC-42a3-9474-1A3956D46DE8}", true, CultureInfo.InvariantCulture) != 0)
							{
								throw new RecipeFrameworkException(String.Format(
									CultureInfo.CurrentCulture,
									Properties.Resources.Templates_InvalidTypeItemForSolutionFolder,
									Template.Name, Template.Language, project.Name));
							}
						}
					}
					else if (Template.Kind == TemplateKind.Project)
					{
						if (!(project.Object is EnvDTE80.SolutionFolder))
						{
							throw new RecipeFrameworkException(String.Format(
									CultureInfo.CurrentCulture,
									Properties.Resources.Templates_ProjectInvalidReference,
									Template.Name, project.Name));
						}
					}
				}
			}
			if (Template.Kind == TemplateKind.Solution)
			{
				throw new RecipeFrameworkException(String.Format(
					CultureInfo.CurrentCulture,
					Properties.Resources.Templates_SolutionCannotBeBound,
					Template.Name));
			}
		}

        /// <summary>
        /// <seealso cref="IAssetReference.AppliesTo"/>
        /// </summary>
		public override string AppliesTo
		{
			get { return boundReference.AppliesTo; }
		}

        /// <summary>
        /// <seealso cref="IAssetReference.Key"/>
        /// </summary>
        public override string Key
        {
            // Key is the target key (its path usually) plus the template name.
            // Ensures uniqueness of template+target.
            get { return boundReference.Key + "::" + base.AssetName; }
        }

		#endregion
	}
}
