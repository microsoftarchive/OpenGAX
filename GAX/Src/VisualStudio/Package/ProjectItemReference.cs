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
using VsWebSite;
using System.IO;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
	/// <summary>
	/// Represents a recipe reference that points to a concrete item in the solution.
	/// </summary>
	/// <seealso cref="RecipeReference"/>
	[Serializable]
	[System.Drawing.ToolboxBitmap(typeof(ProjectItemReference))]
	public class ProjectItemReference : VsBoundReference
	{
		IBoundReferenceLocatorStrategy strategy;

		/// <summary>
		/// Initializes an instance of the <see cref="ProjectItemReference"/> class.
		/// </summary>
		/// <seealso cref="RecipeReference"/>
		public ProjectItemReference(string recipe, ProjectItem item)
			: base(recipe, item)
		{
			if (item.ContainingProject.Kind == VsWebSite.PrjKind.prjKindVenusProject)
			{
				strategy = new WebItemStrategy();
			}
			else
			{
				strategy = new ItemStrategy();
			}
		}

		/// <summary>
		/// Initializes tracking of the associated item.
		/// </summary>
		protected override void OnSited()
		{
			base.OnSited();
			LocateItem();
		}

		/// <summary>
		/// Gets the string representing the target object
		/// </summary>
		public override string AppliesTo
		{
			get
			{
				try
				{
					if (Target != null)
					{
						return strategy.GetAppliesTo((ProjectItem)Target);
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
		/// <summary>
		/// Called upon siting if the object was constructed from serialized 
		/// state. In this case the target will be null, and the 
		/// serializedData field will contain the path of the item.
		/// </summary>
		private void LocateItem()
		{
			if (Target != null)
			{
				return;
			}

			IServiceProvider vs = GetService<IServiceProvider>(true);
			object item = strategy.LocateTarget(vs, serializedData);
			if (item != null)
			{
				SetTarget(item);
			}
			else
			{
				throw new ArgumentException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.BoundReference_InvalidItem, serializedData));
			}
		}

		/// <summary>
		/// Initializes an instance of the <see cref="ProjectItemReference"/> class.
		/// </summary>
		/// <seealso cref="IAssetReference"/>
		protected ProjectItemReference(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info.GetBoolean("IsWebItem"))
			{
				strategy = new WebItemStrategy();
			}
			else
			{
				strategy = new ItemStrategy();
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
			if (strategy is WebItemStrategy)
			{
				info.AddValue("IsWebItem", true);
			}
			else
			{
				info.AddValue("IsWebItem", false);
			}

			// Let the base class save the serializedData value.
			base.GetObjectData(info, context);
		}

		private class WebItemStrategy : IBoundReferenceLocatorStrategy
		{
			public string GetAppliesTo(object target)
			{
				ProjectItem targetItem = target as ProjectItem;
				if (targetItem == null)
					throw new ArgumentException("target");

				string itemPath = "";
				bool found = DteHelper.BuildPathFromCollection(targetItem.ContainingProject.ProjectItems, targetItem, ref itemPath);
				// This would indicate a bug in the DTE itself.
				Debug.Assert(found, "Item does not belong to containing project?");

				webType kind = (webType)targetItem.ContainingProject.Properties.Item("WebSiteType").Value;
				if (kind == webType.webTypeFileSystem)
				{
					// Use the simplified representation of the path if available
					if (targetItem.ContainingProject.ParentProjectItem != null)
					{
						itemPath = targetItem.ContainingProject.ParentProjectItem.Name + itemPath;
					}
					else
					{
						itemPath = targetItem.ContainingProject.Name + itemPath;
					}
				}
				else
				{
					itemPath = targetItem.ContainingProject.FullName +
						itemPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
				}

				return itemPath;
			}

			public string GetSerializationData(object target)
			{
				ProjectItem targetItem = target as ProjectItem;
				if (targetItem == null)
					throw new ArgumentException("target");

				string projectData = new ProjectReference.WebProjectStrategy().GetSerializationData(targetItem.ContainingProject);
				string itemPath = "";
				bool found = DteHelper.BuildPathFromCollection(targetItem.ContainingProject.ProjectItems, targetItem, ref itemPath);
				// This would indicate a bug in the DTE itself.
				Debug.Assert(found, "Item does not belong to containing project?");

				return projectData + "||" + itemPath;
			}

			public object LocateTarget(IServiceProvider serviceProvider, string serializedData)
			{
				string[] values = serializedData.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
				Project project = (Project)new ProjectReference.WebProjectStrategy().LocateTarget(serviceProvider, values[0]);
				ProjectItem item = DteHelper.FindInCollection(project.ProjectItems, values[1]);
				return item;
			}
		}

		private class ItemStrategy : IBoundReferenceLocatorStrategy
		{
			public string GetAppliesTo(object target)
			{
				return DteHelper.BuildPath(target);
			}

			public object LocateTarget(IServiceProvider serviceProvider, string serializedData)
			{
				DTE vs = (DTE)serviceProvider.GetService(typeof(DTE));
				return DteHelper.FindItemByPath(vs.Solution, serializedData);
			}

			public string GetSerializationData(object target)
			{
				return DteHelper.BuildPath(target);
			}
		}
	}
}
