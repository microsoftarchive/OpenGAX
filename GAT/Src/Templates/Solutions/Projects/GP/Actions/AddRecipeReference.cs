#region Using Directives

using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE;

#endregion

namespace $PackageNamespace$.Actions
{
	/// <summary>
	/// Adds a new bound recipe reference to the IAssetReferenceService 
	/// pointing to the currently selected item.
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	public class AddRecipeReference : Microsoft.Practices.RecipeFramework.Action
	{
		IBoundAssetReference addedReference;
		string recipeName;

		/// <summary>
		/// Specifies the filename name of the template 
		/// we are going to reference
		/// </summary>
		[Input(Required=true)]
		public string RecipeName
		{
			get { return recipeName; }
			set { recipeName = value; }
		} 

		/// <summary>
		/// Instance of the new added reference
		/// </summary>
		[Output]
		public IBoundAssetReference Reference
		{
			get { return addedReference; }
		} 

		/// <summary>
		/// Adds the template reference to the IAssetReferenceService
		/// </summary>
		public override void Execute()
		{
			DTE vs = GetService<DTE>(true);
			IAssetReferenceService referenceService = GetService<IAssetReferenceService>(true);
			object item = DteHelper.GetTarget(vs);
			
			if (item == null)
				throw new InvalidOperationException("There is no valid target to reference the template.");

			if (item is Project)
			{
				addedReference = new ProjectReference(recipeName, (Project)item);
			}
			else if (item is Solution)
			{
				addedReference = new SolutionReference(recipeName, (Solution)item);
			}
			else if (item is ProjectItem)
			{
				addedReference = new ProjectItemReference(recipeName, (ProjectItem)item);
			}
			else
			{
				throw new NotSupportedException("Current selection is unsupported.");
			}

			referenceService.Add(addedReference);

			MessageBox.Show("The new reference was successfully added.", "New Reference",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/// <summary>
		/// Removes the previously added reference, if it was created
		/// </summary>
		public override void Undo()
		{
			if (addedReference != null)
			{
				IAssetReferenceService referenceService = GetService<IAssetReferenceService>(true);
				referenceService.Remove(addedReference);
			}
		}
	}
}