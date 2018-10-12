using System;
using EnvDTE;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.References
{
    /// <summary>
    /// Reference that only applies to non-VSIX guidance package projects
    /// </summary>
    [Serializable]
	public class UnboundNonVsixPackageReference : UnboundPackageReference
	{
		const string ExtensibilityProjectTypeGuid = "82b43b9b-a64c-4715-b499-d71e9ca2bd60";

		/// <summary>
		/// Default constructor receiving the recipe name.
		/// </summary>
		public UnboundNonVsixPackageReference(string recipe) : base(recipe) { }

		/// <summary>
		/// Required constructor for deserialization.
		/// </summary>
		protected UnboundNonVsixPackageReference(SerializationInfo info, StreamingContext context) : base(info, context) { }

		/// <summary>
		/// Determines whether the target is a non v2.0 guidance package project 
		/// by looking at if the project is of an VSIX Extensibility type
		/// </summary>
		public override bool IsEnabledFor(object target)
		{
			if (base.IsEnabledFor(target))
			{
				Project targetProject = target as Project;
				if (targetProject != null && targetProject.ProjectItems != null)
				{
					try
					{
						return !GetProjectTypeGuids(targetProject).ToLower().Contains(ExtensibilityProjectTypeGuid);
					}
					catch
					{
						return false;
					}
				}
			}

			return false;
		}

		public string GetProjectTypeGuids(Project project)
		{
			IVsHierarchy hierarchy;
			IVsSolution vsSolution = (IVsSolution)this.GetService(typeof(SVsSolution));
			vsSolution.GetProjectOfUniqueName(project.UniqueName, out hierarchy);

			var aggregatableProject = (IVsAggregatableProject)hierarchy;

			string projectTypeGuids;
			aggregatableProject.GetAggregateProjectTypeGuids(out projectTypeGuids);

			return projectTypeGuids;
		}

		/// <summary>
		/// Friendly representation of the condition this reference expresses.
		/// </summary>
		public override string AppliesTo
		{
			get { return Properties.Resources.UnboundNonVsixPackageReference_AppliesTo; }
		}
	}
}
