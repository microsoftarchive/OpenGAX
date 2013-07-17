using System;
using EnvDTE;
using EnvDTE80;
using VSLangProj;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework;

namespace $PackageNamespace$.References
{
	/// <summary>
	/// UnBoundRecipe that allows to be executed only on Solution Folders
	/// </summary>
	[Serializable]
	public class SolutionFolderRecipeReference : UnboundRecipeReference
	{

		/// <summary>
		/// Constructor of the SolutionFolderRecipeReference that must specify the 
		/// recipe name that will be used by the reference
		/// </summary>
		/// <param name="recipe"></param>
		public SolutionFolderRecipeReference(string recipe)
			: base(recipe)
		{
		}

		/// <summary>
		/// Returns a friendly name as Any Solution folder
		/// </summary>
		public override string AppliesTo
		{
			get { return "Any Solution Folder or the Solution Root"; }
		}

		/// <summary>
		/// Performs the validation of the item passed as target
		/// Returns true if the reference is allowed to be executed in the target
		/// that is if the target is a solution folder
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public override bool IsEnabledFor(object target)
		{
			if (target is SolutionFolder || target is Solution)
			{
				return true;
			}
			if (!(target is Project))
			{
				return false;
			}
			return (((Project)target).Object is SolutionFolder);
		}

		#region ISerializable Members

		/// <summary>
		/// Required constructor for deserialization.
		/// </summary>
		protected SolutionFolderRecipeReference(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion ISerializable Members
	}
}
