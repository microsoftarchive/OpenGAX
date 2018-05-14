//===================================================================================
// Microsoft patterns & practices
// Guidance Automation Toolkit
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
using Microsoft.Practices.ComponentModel;
using EnvDTE;

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.Actions
{
	/// <summary>
	/// Includes all content files in the vsix
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	public class IncludeContentInVsixAction : Action, IServiceProvider
	{
		/// <summary>
		/// The target project
		/// </summary>
		[Input(Required = true)]
		public Project Project { get; set; }

		/// <summary>
		/// Does nothing, as un-registration must be done explicitly.
		/// </summary>
		public override void Undo()
		{
			// Must un-register to undo.
		}

		/// <summary>
		/// Includes all content files in the vsix
		/// </summary>
		public override void Execute()
		{
			TraceUtil.TraceInformation(this, "Incluiding content in vsix...");

			foreach (ProjectItem item in Utils.FindProjectItems(this.Project.ProjectItems,ContentItemsFilter))				
			{
				Utils.SetIncludeInVsix(this, item, true);
			}
		}

		private Predicate<ProjectItem> ContentItemsFilter
		{
			get
			{
				return item =>
					{
						try
						{
							if (item.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
							{
								if ("Content".Equals(item.Properties.Item("ItemType").Value) &&
									(int)item.Properties.Item("BuildAction").Value == 2 &&
									(uint)item.Properties.Item("CopyToOutputDirectory").Value > 0)
								{
									return true;
								}
							}
						}
						catch (Exception ex)
						{
							TraceUtil.TraceWarning(this, ex.Message);
						}

						return false;
					};
			}
		}

		object IServiceProvider.GetService(Type serviceType)
		{
			return this.GetService(serviceType);
		}
	}
}