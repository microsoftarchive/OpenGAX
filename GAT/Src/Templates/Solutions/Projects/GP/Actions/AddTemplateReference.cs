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
	/// Adds a new template reference to the IAssetReferenceService
	/// It needs the templatefilename
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	public class AddTemplateReference : Microsoft.Practices.RecipeFramework.Action
	{
		#region Input Properties

		/// <summary>
		/// Specifies the filename name of the template 
		/// we are going to reference
		/// </summary>
		[Input(Required=true)]
		public string TemplateFilename
		{
			get { return templateFilename; }
			set { templateFilename = value; }
		} string templateFilename;

		#endregion

		#region Output Properties


		/// <summary>
		/// Instance to the new added reference
		/// </summary>
		[Output]
		public IBoundAssetReference NewReference
		{
			get { return newReference; }
			set { newReference = value; }
		} IBoundAssetReference newReference;

		#endregion

		/// <summary>
		/// Adds the template reference to the IAssetReferenceService
		/// </summary>
		public override void Execute()
		{
			EnvDTE.DTE dte = (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE));
			IAssetReferenceService referenceService = (IAssetReferenceService)GetService(typeof(IAssetReferenceService));
			object item = DteHelper.GetTarget(dte);
			if (item == null)
			{
				MessageBox.Show("There is no valid target to reference the template");
				return;
			}
			templateFilename = new Uri(templateFilename).LocalPath;

			VsBoundReference vsTarget = null;
			if (item is Project)
			{
				vsTarget = new ProjectReference(templateFilename, (Project)item);
			}
			else if (item is Solution)
			{
				vsTarget = new SolutionReference(templateFilename, (Solution)item);
			}
			else if (item is ProjectItem)
			{
				vsTarget = new ProjectItemReference(templateFilename, (ProjectItem)item);
			}
			else if (item is EnvDTE80.SolutionFolder)
			{
				vsTarget = new ProjectReference(templateFilename, ((EnvDTE80.SolutionFolder)item).Parent);
			}
			if (item == null || vsTarget == null)
			{
				MessageBox.Show(string.Format(
					CultureInfo.CurrentCulture,
					"Target {0} specified for reference to asset {1} doesn't exist.",
					"target", templateFilename));
				return;
			}
			if (!File.Exists(templateFilename) || !templateFilename.EndsWith(".vstemplate", StringComparison.InvariantCultureIgnoreCase))
			{
				MessageBox.Show(string.Format(
					CultureInfo.CurrentCulture,
					"The filename specified for the template \"{0}\" does not exist.",
					templateFilename));
				return;
			}
			newReference = new BoundTemplateReference(templateFilename, vsTarget);
			referenceService.Add(newReference);
			MessageBox.Show("The new reference was successfully added","New Reference", 
				MessageBoxButtons.OK, MessageBoxIcon.Information);

		}

		/// <summary>
		/// Removes the previously added reference, if it was created
		/// </summary>
		public override void Undo()
		{
			if (newReference != null)
			{
				IAssetReferenceService referenceService = (IAssetReferenceService)GetService(typeof(IAssetReferenceService));
				referenceService.Remove(newReference);
			}
		}
	}
}