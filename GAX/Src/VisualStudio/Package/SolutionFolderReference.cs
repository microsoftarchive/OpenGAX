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
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
	/// <summary>
	/// Represents a recipe reference that points to a solution folder in the solution.
	/// </summary>
	/// <seealso cref="RecipeReference"/>
	[Serializable]
	[System.Drawing.ToolboxBitmap(typeof(SolutionFolderReference))]
	public class SolutionFolderReference : VsBoundReference
	{
		private class SolutionFolderReferenceStrategy: IBoundReferenceLocatorStrategy
		{
			#region IReferenceStrategy Members

			public string GetAppliesTo(object target)
			{
				return DteHelper.BuildPath(((EnvDTE80.SolutionFolder)target).Parent);
			}

			public string GetSerializationData(object target)
			{
				return DteHelper.BuildPath(((EnvDTE80.SolutionFolder)target).Parent);
			}

			public object LocateTarget(IServiceProvider serviceProvider, string serializedData)
			{
				DTE vs = (DTE)serviceProvider.GetService(typeof(DTE));
				Project project = DteHelper.FindProjectByPath(vs.Solution, serializedData);
				if (project != null)
				{
					return (project.Object as EnvDTE80.SolutionFolder);
				}
				return null;
			}

			#endregion
		}
		IBoundReferenceLocatorStrategy strategy;

		#region Constructor

		/// <summary>
		/// Initializes an instance of the <see cref="SolutionFolderReference"/> class.
		/// </summary>
		/// <seealso cref="RecipeReference"/>
		public SolutionFolderReference(string recipe, EnvDTE80.SolutionFolder folder)
			: base(recipe, folder)
		{
			this.strategy = new SolutionFolderReferenceStrategy();
		}

		#endregion Constructor

		#region Overrides

		/// <summary>
		/// Initializes tracking of the associated item.
		/// </summary>
		protected override void OnSited()
		{
			base.OnSited();
			LocateFolder();
		}

		/// <summary>
		/// Gets the string represeting the target object
		/// </summary>
		public override string AppliesTo
		{
			get
			{
				if (Target != null)
				{
					// Target will be the project.
					return strategy.GetAppliesTo(Target);
				}
				else
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

		#endregion Overrides

		#region Private Members

		/// <summary>
		/// Called upon siting if the object was constructed from serialized 
		/// state. In this case the target will be null, and the 
		/// serializedData field will contain the path of the folder.
		/// </summary>
		private void LocateFolder()
		{
			if (Target != null)
			{
				return;
			}
			IServiceProvider vs = GetService<IServiceProvider>();
			// Try the project.
			SetTarget(this.strategy.LocateTarget(vs,serializedData));
			if (Target == null)
			{
				// We couldn't find either a solution, item or project.
				throw new ArgumentException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.BoundReference_InvalidItem, serializedData));
			}
		}

		#endregion Private Members

		#region ISerializable Members

		/// <summary>
		/// Initializes an instance of the <see cref="ProjectReference"/> class.
		/// </summary>
		/// <seealso cref="IAssetReference"/>
		protected SolutionFolderReference(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.strategy = new SolutionFolderReferenceStrategy();
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
				base.serializedData = this.strategy.GetSerializationData(Target);
			}
			info.AddValue("folder", serializedData);
		}

		#endregion ISerializable Members
	}
}
