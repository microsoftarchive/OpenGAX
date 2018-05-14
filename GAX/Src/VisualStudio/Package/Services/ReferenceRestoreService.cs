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

#region using

using System;
using System.Collections;
using System.Globalization;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.WizardFramework.Configuration;
using Config = Microsoft.Practices.RecipeFramework.Configuration;
using System.Xml;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using System.IO;
using EnvDTE;
using EnvDTE80;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Converters;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Services
{
    /// <summary>
    /// Class in charge of collecting the broken references
    /// and to show a wizard with the dangling references
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
	[ServiceDependency(typeof(IRecipeManagerService))]
	[ServiceDependency(typeof(DTE))]
	[ServiceDependency(typeof(IHostService))]
	[ServiceDependency(typeof(IPersistenceService))]
	internal class ReferenceRestoreService : Component, IReferenceRestoreService
    {
		#region Fields
		private IRecipeManagerService manager;
		private EnvDTE.DTE vs;
		private ArrayList referencesToFix;
		#endregion

		/// <summary>
		/// Performs the validation of all references
		/// and for the ones that are dangling
		/// It will create a dialog in order to get the new valid
		/// targets
		/// </summary>
		public void PerformValidation()
		{
			manager = (IRecipeManagerService)
				ServiceHelper.GetService(this, typeof(IRecipeManagerService));
			vs = (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE));
			referencesToFix = new ArrayList();
			try
			{
				CheckReferences();
				PerformRestore();
			}
			catch (Exception ex)
			{
				ErrorHelper.Show(this.Site, ex, Properties.Resources.ReferenceRestoreService_Error);
			}
		}

		#region Performs Restore
		private void CheckReferences()
		{
			SolutionPackagesContainer container = (SolutionPackagesContainer)GetService(typeof(IHostService));
			IPersistenceService persist = (IPersistenceService)container.GetService(typeof(IPersistenceService), true);
			
			foreach (object obj in container.Components)
			{
				if (obj is VsGuidancePackage)
				{
					GuidancePackage package = ((VsGuidancePackage)obj).GuidancePackage;

					if ((package == null) || (package.Configuration == null))
					{
						continue;
					}

					foreach (IAssetReference reference in persist.LoadReferences(package.Configuration.Name))
					{
						if (reference is IBoundAssetReference)
						{
							IBoundAssetReference boundReference = (IBoundAssetReference)reference;
							if (boundReference.Target == null)
							{
								referencesToFix.Add(
									new FixupReference(package, reference.AssetName, boundReference));
							}
						}
					}
				}
			}
		}				
		
		/// <summary>
		/// It creates a dynamic recipe in order to show a dialog that asks the user
		/// to introduce new targets for all dangling references
		/// </summary>
        private void PerformRestore()
        {
            if (referencesToFix.Count == 0)
            {
                return;
            }

			#region Asking to the user

			string msg;
			string details;
			BuildMessage(out msg, out details);

			if (ErrorHelper.Ask(Properties.Resources.ReferenceRestoreService_StartTitle, msg, details, 
				Properties.Resources.ReferenceRestoreService_ResolveButton, 
				Properties.Resources.ReferenceRestoreService_RemoveButton)
				== System.Windows.Forms.DialogResult.No)
            {
				IAssetReferenceService referenceService = null;
				GuidancePackage lastPackage = null;
				foreach (FixupReference lostReference in referencesToFix)
				{
					if (lastPackage != lostReference.OwningPackage)
					{
						referenceService =
							(IAssetReferenceService)ServiceHelper.GetService(lostReference.OwningPackage,
							typeof(IAssetReferenceService), this);
					}
					referenceService.Remove(lostReference.OldReference);
				}
				return;
			}

			#endregion

			// The pending FixupReferences we added to the list will be ordered 
            // by the template they were in, so we can just build a wizard page
            // for each group of X of fields for fixup.
            int maxfields = 5;

            // Used to create configuration attributes
            XmlDocument xmlfactory = new XmlDocument();

            // Create a package dynamically.
			Config.GuidancePackage package = CreateDynamicPackage();

			Config.Recipe recipe;
			ArrayList arguments;
			ArrayList actions;
			CreateDynamicRecipe(package, out recipe, out arguments, out actions);

            Page lastpage = null;
            ArrayList pages = new ArrayList();
            ArrayList fields = new ArrayList(maxfields);
            Hashtable argumentNamesUsed = new Hashtable(7);

            int nPages = referencesToFix.Count / maxfields;
			if ((referencesToFix.Count % maxfields) != 0)
			{
				nPages++;
			}

			Hashtable referenceDictionary = CreateFixupPages(maxfields, xmlfactory, 
				arguments, actions, ref lastpage, pages, ref fields, nPages);

            lastpage.Fields = new Field[fields.Count];
            fields.CopyTo(lastpage.Fields, 0);

            recipe.Arguments = new Config.Argument[arguments.Count];
            arguments.CopyTo(recipe.Arguments, 0);
			if (recipe.Actions == null) recipe.Actions = new Config.RecipeActions();
            recipe.Actions.Action = new Config.Action[actions.Count];
            actions.CopyTo(recipe.Actions.Action, 0);

            Wizard wizard = new Wizard();
            wizard.SchemaVersion = "1.0";
            wizard.Pages = new Page[pages.Count];
            pages.CopyTo(wizard.Pages, 0);

            // Get the wizard in XML form so that it's passed to the recipe execution.
			SerializeToAny(recipe, wizard);

			EnableAndExecute(package, referenceDictionary);
        }

		private void BuildMessage(out string msg, out string details)
		{
			string listrefs = Environment.NewLine;
			foreach (FixupReference fixup in referencesToFix)
			{
				if (fixup.OldReference is BoundTemplateReference)
				{
					listrefs += string.Format(Properties.Resources.ReferenceRestoreService_TemplateOldPath, fixup.ReferencedAsset, fixup.SavedTarget);
				}
				else
				{
					listrefs += string.Format(Properties.Resources.ReferenceRestoreService_RecipeOldPath, fixup.ReferencedAsset, fixup.SavedTarget);
				}
			}
			msg = string.Format(CultureInfo.CurrentCulture,
				referencesToFix.Count > 1 ?
				Properties.Resources.ReferenceRestoreService_StartDanglingReferences
				: Properties.Resources.ReferenceRestoreService_StartDanglingReference, string.Empty);
			details = string.Format(CultureInfo.CurrentCulture,
			  referencesToFix.Count > 1 ?
			  Properties.Resources.ReferenceRestoreService_StartDanglingReferences
			  : Properties.Resources.ReferenceRestoreService_StartDanglingReference,
			  listrefs);
		}

		private static void SerializeToAny(Config.Recipe recipe, Wizard wizard)
		{
			StringWriter sw = new StringWriter();
			new System.Xml.Serialization.XmlSerializer(typeof(Wizard)).Serialize(sw, wizard);
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(sw.ToString());
			recipe.GatheringServiceData = new Configuration.RecipeGatheringServiceData();
			recipe.GatheringServiceData.Any = doc.DocumentElement;
		}

		private void EnableAndExecute(Config.GuidancePackage package, Hashtable referenceDictionary)
		{
			GuidancePackage loadedpackage = null;

			try
			{
				loadedpackage = manager.EnablePackage(package);
				bool stop = true;
				do
				{
					stop = (loadedpackage.Execute("ReferenceRestoreRecipe", referenceDictionary) == ExecutionResult.Finish);
					if (!stop)
					{
						stop = (MessageBox.Show(Properties.Resources.ReferenceRestoreService_Cancelled,
							Properties.Resources.ReferenceRestoreService_DialogTitle,
							MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No);
					}
				} while (!stop);
			}
			finally
			{
				try
				{
					if (loadedpackage != null)
					{
						manager.DisablePackage(loadedpackage);
						loadedpackage.Dispose();
					}
				}
				catch (Exception ex)
				{
					ErrorHelper.Show(this.Site, ex);
				}
			}
		}

		private void CreateDynamicRecipe(Config.GuidancePackage package, out Config.Recipe recipe, out ArrayList arguments, out ArrayList actions)
		{
			// Recipe to execute and add references.
			recipe = new Config.Recipe();
			package.Recipes = new Config.Recipe[] { recipe };
			recipe.Name = "ReferenceRestoreRecipe";
			recipe.Caption = Properties.Resources.ReferenceRestoreService_DialogTitle;
			arguments = new ArrayList(referencesToFix.Count);
			actions = new ArrayList(referencesToFix.Count);
		}

		private static Config.GuidancePackage CreateDynamicPackage()
		{
			Config.GuidancePackage package = new Config.GuidancePackage();
			package.SchemaVersion = "1.0";
			package.SourceLevels = Config.SourceLevels.Off;
			package.Host = "VisualStudio";
			package.Name = "DynamicPackage";
			package.Caption = "DynamicPackage";
			package.Guid = Guid.NewGuid().ToString();
			return package;
		}

		private Hashtable CreateFixupPages(int maxfields, XmlDocument xmlfactory, ArrayList arguments, ArrayList actions, ref Page lastpage, ArrayList pages, ref ArrayList fields, int nPages)
		{
			int iPage = 1;
			int iArgument = 1;
			bool firstPage = true;

			Hashtable referenceDictionary = new Hashtable();

			foreach (FixupReference fixup in referencesToFix)
			{
				if (firstPage ||
					(lastpage != null) && (fields.Count == maxfields))
				{
					#region Create new page
					firstPage = false;

					// If we have fields and a previous page, set all fields in one shot.
					if (lastpage != null && fields != null)
					{
						lastpage.Fields = new Field[fields.Count];
						fields.CopyTo(lastpage.Fields, 0);
					}

					// Start a new page for the fields.
					lastpage = new Page();
					pages.Add(lastpage);
					fields = new ArrayList(maxfields);
					lastpage.Title = String.Format(
						CultureInfo.CurrentCulture,
						Properties.Resources.ReferenceRestoreService_PageTitle,
						iPage, nPages);
					lastpage.Help = String.Format(
						CultureInfo.CurrentCulture,
						Properties.Resources.ReferenceRestoreService_PageHelp);
					lastpage.LinkTitle = String.Format(
						CultureInfo.CurrentCulture,
						Properties.Resources.ReferenceRestoreService_PageLinkTitle,
						iPage, nPages);
					iPage++;

					#endregion Create new page
				}

				#region Setup recipe arguments

				Config.Argument argument = new Config.Argument();

				argument.Name = string.Format(CultureInfo.InvariantCulture, "Argument{0}", iArgument);
				argument.Type = GetTargetType(fixup.ExpectedTargetKind).AssemblyQualifiedName;
				argument.Converter = new Config.Converter();
				argument.Converter.Type = GetConverterType(fixup.ExpectedTargetKind);
				arguments.Add(argument);

				Config.Argument argumentKeyRef = new Config.Argument();
				argumentKeyRef.Name = string.Format(CultureInfo.InvariantCulture, "Reference{0}", iArgument);
				argumentKeyRef.Type = "Microsoft.Practices.RecipeFramework.IAssetReference, Microsoft.Practices.RecipeFramework.Common";
				referenceDictionary.Add(argumentKeyRef.Name, fixup.OldReference);

				arguments.Add(argumentKeyRef);
				iArgument++;

				#endregion Setup recipe arguments

				#region Create new field



				Field field = new Field();
				if (fixup.OldReference is BoundTemplateReference)
				{
					int templateLength = fixup.OwningPackage.BasePath.Length + 11; //11 corresponds to Lenght of string "\Template\"
					if (templateLength > fixup.ReferencedAsset.Length)
					{
						templateLength = 0;
					}
					field.Label = string.Format("{0} ({1})", fixup.ReferencedAsset.Substring(templateLength), fixup.SavedTarget);
					field.Help = String.Format(
						CultureInfo.CurrentCulture,
						Properties.Resources.ReferenceRestoreService_FieldTemplateHelp,
						fixup.ExpectedTargetKind, fixup.ReferencedAsset.Substring(templateLength));
				}
				else // case of recipe reference
				{
					IConfigurationService configService =
						(IConfigurationService)fixup.OwningPackage.GetService(typeof(IConfigurationService), true);
					Config.Recipe recipeReference = configService.CurrentPackage[fixup.OldReference.AssetName];
					field.Label = string.Format("{0} ({1})", recipeReference.Caption, fixup.SavedTarget);
					field.Help = String.Format(
						CultureInfo.CurrentCulture,
						Properties.Resources.ReferenceRestoreService_FieldRecipeHelp,
						fixup.ExpectedTargetKind, recipeReference.Caption);
				}
				field.Tooltip = String.Format(
					CultureInfo.CurrentCulture,
					Properties.Resources.ReferenceRestoreService_FieldTooltip,
					fixup.SavedTarget, fixup.ExpectedTargetKind);
				field.InvalidValueMessage = String.Format(
					CultureInfo.CurrentCulture,
					Properties.Resources.ReferenceRestoreService_FieldInvalid,
					fixup.ExpectedTargetKind);
				field.ValueName = argument.Name;
				field.Editor = new Editor();
				// Again, we can't use the type directly to avoid circular references.
				field.Editor.Type = "Microsoft.Practices.RecipeFramework.Library.Editors.SolutionPickerEditor, Microsoft.Practices.RecipeFramework.Library";
				field.PanelType = "Microsoft.Practices.RecipeFramework.VisualStudio.Services.CustomArgumentPanel, Microsoft.Practices.RecipeFramework.VisualStudio";
				fields.Add(field);

				#endregion Create new field

				#region Setup an action for it

				Config.Action action = new Config.Action();
				// Name will be ugly, but we need to ensure uniqueness too.
				action.Name = argument.Name;
				action.Type = typeof(AddFixedReferenceAction).AssemblyQualifiedName;
				XmlAttribute assetattr;
				assetattr = xmlfactory.CreateAttribute("Recipe");
				assetattr.Value = fixup.ReferencedAsset;

				XmlAttribute packageattr = xmlfactory.CreateAttribute("TargetPackage");
				packageattr.Value = fixup.OwningPackage.Configuration.Name;
				action.AnyAttr = new XmlAttribute[] { assetattr, packageattr };
				// Sync action input with collected argument for the target.
				Config.Input input = new Config.Input();
				input.Name = "Target";
				input.RecipeArgument = argument.Name;
				Config.Input inputRef = new Config.Input();
				inputRef.Name = "OldReference";
				inputRef.RecipeArgument = argumentKeyRef.Name;
				action.Input = new Config.Input[] { input, inputRef };
				actions.Add(action);

				#endregion Setup an action for it
			}
			return referenceDictionary;
		}

        #endregion

        #region FixupReference

        private enum TargetKind
        {
            Solution,
            SolutionFolder,
            Project,
            ProjectItem
        }

        /// <summary>
        /// Determines the type of converter to use for the target based on its kind.
        /// </summary>
        private static string GetConverterType(TargetKind kind)
        {
            switch (kind)
            {
                case TargetKind.Project:
                    return typeof(ProjectOrEmptyConverter).AssemblyQualifiedName;
                case TargetKind.ProjectItem:
					return typeof(ProjectItemOrEmptyConverter).AssemblyQualifiedName;
                case TargetKind.Solution:
					return typeof(SolutionOrEmptyConverter).AssemblyQualifiedName;
                case TargetKind.SolutionFolder:
					return typeof(SolutionFolderOrEmptyConverter).AssemblyQualifiedName;
				default:
					throw new NotSupportedException(kind.ToString());
            }
        }

        /// <summary>
        /// Determines the type of the target based on its kind.
        /// </summary>
        private static Type GetTargetType(TargetKind kind)
        {
            switch (kind)
            {
				case TargetKind.SolutionFolder:
					return typeof(SolutionFolder);
				case TargetKind.Project:
                    return typeof(Project);
                case TargetKind.ProjectItem:
                    return typeof(ProjectItem);
                case TargetKind.Solution:
                    return typeof(Solution);
                default:
                    throw new NotSupportedException(kind.ToString());
            }
        }

        /// <summary>
        /// Reference information of a broken reference
        /// </summary>
        private class FixupReference
        {
            public FixupReference(GuidancePackage owningPackage,
                string asset,  
                IBoundAssetReference oldReference)
            {
                this.OwningPackage = owningPackage;
				this.ReferencedAsset = asset;
				VsBoundReference realReference = null;
				if (oldReference is VsBoundReference)
				{
					realReference = (VsBoundReference)oldReference;
				}
				else if (oldReference is BoundTemplateReference)
				{
					realReference = (VsBoundReference)((BoundTemplateReference)oldReference).BoundReference;
				}
				this.SavedTarget = realReference.SubPath;
				this.ExpectedTargetKind = GetTargetKind(realReference);
				this.OldReference = oldReference;
            }

            public GuidancePackage OwningPackage;
            public string ReferencedAsset;
            public string SavedTarget;
            public TargetKind ExpectedTargetKind;
			public IBoundAssetReference OldReference;

            /// <summary>
            /// Determines the kind of target based on the type of the reference.
            /// </summary>
			private TargetKind GetTargetKind(IBoundAssetReference reference)
            {
                if (reference is SolutionReference)
                {
                    return TargetKind.Solution;
                }
                else if (reference is ProjectReference)
                {
					if (((ProjectReference)reference).IsSolutionFolder)
					{
						return TargetKind.SolutionFolder;
					}
                    return TargetKind.Project;
                }
                else if (reference is ProjectItemReference)
                {
                    return TargetKind.ProjectItem;
                }
				else if (reference is BoundTemplateReference)
				{
					return TargetKind.SolutionFolder;
				}
                throw new NotSupportedException(reference.ToString());
            }
        }

        #endregion FixupReference

        #region AddFixedReference
        [ServiceDependency(typeof(IRecipeManagerService))]
        private class AddFixedReferenceAction : ConfigurableAction
        {
            // Properties other than the Target are set through "configuration"
            private string recipe;

            public string Recipe
            {
                get { return recipe; }
                set { recipe = value; }
            }

            private string template;

            public string Template
            {
                get { return template; }
                set { template = value; }
            }

            private string targetPackage;

            public string TargetPackage
            {
                get { return targetPackage; }
                set { targetPackage = value; }
            }

            private object target;

			[Input]
			public IBoundAssetReference OldReference
			{
				get { return boundReference; }
				set { boundReference = value; }
			}
			private IBoundAssetReference boundReference;

            [Input]
            public object Target
            {
                get { return target; }
                set { target = value; }
            }

            public override void Execute()
            {
				if (OldReference == null)
				{
					return;
				}
				IRecipeManagerService manager = (IRecipeManagerService)
					ServiceHelper.GetService(this, typeof(IRecipeManagerService));
				// Add the references to the target package.
				GuidancePackage package = manager.GetPackage(targetPackage);
				IAssetReferenceService referenceService = (IAssetReferenceService)
					ServiceHelper.GetService(package, typeof(IAssetReferenceService), this);
				IPersistenceService persist = (IPersistenceService)
					ServiceHelper.GetService(this, typeof(IPersistenceService));

				try
				{

					IDictionary state = persist.LoadState(package.Configuration.Name, OldReference);
					if ((target != null) && !(target is DummyDTE.EmptyDteElement))
					{
						//We have to get the real object Project for case of Solution Folder
						object realTarget = target;
						if (target is SolutionFolder)
						{
							realTarget = ((SolutionFolder)target).Parent;
						}

						if (OldReference is VsBoundReference)
						{
							((VsBoundReference)OldReference).SetTarget(realTarget);
						}
						else if (OldReference is BoundTemplateReference)
						{
							((BoundTemplateReference)OldReference).BoundReference.SetTarget(realTarget);
						}
						referenceService.Add(OldReference, state);
					}
				}
				catch (Exception ex)
				{
					if (OldReference != null)
					{
						referenceService.Remove(OldReference);
					}
					ErrorHelper.Show(this.Site, ex);
				}
            }

			/// <summary>
			/// We are going to handle the errors inside the same execute
			/// In order to continue binding as much references as possible
			/// </summary>
            public override void Undo()
            {
			}
        }
        #endregion AddFixedReference

	}
	#region Class that implements dte project, solution, solutionfolder
}

namespace DummyDTE
{
    internal class EmptyDteElement : EnvDTE.Solution, EnvDTE.ProjectItem, EnvDTE.Project, EnvDTE80.SolutionFolder
	{
		#region _Solution Members

		Project _Solution.AddFromFile(string FileName, bool Exclusive)
		{
			return null;
		}

		Project _Solution.AddFromTemplate(string FileName, string Destination, string ProjectName, bool Exclusive)
		{
			return null;
		}

		AddIns _Solution.AddIns
		{
			get { return null; }
		}

		void _Solution.Close(bool SaveFirst)
		{

		}

		int _Solution.Count
		{
			get { return 0; }
		}

		void _Solution.Create(string Destination, string Name)
		{

		}

		DTE _Solution.DTE
		{
			get { return null; }
		}

		string _Solution.ExtenderCATID
		{
			get { return null; }
		}

		object _Solution.ExtenderNames
		{
			get { return null; }
		}

		string _Solution.FileName
		{
			get { return null; }
		}

		ProjectItem _Solution.FindProjectItem(string FileName)
		{
			return null;
		}

		string _Solution.FullName
		{
			get { return null; }
		}

		IEnumerator _Solution.GetEnumerator()
		{
			return null;
		}

		Globals _Solution.Globals
		{
			get { return null; }
		}

		bool _Solution.IsDirty
		{
			get
			{
				return false;
			}
			set
			{

			}
		}

		bool _Solution.IsOpen
		{
			get { return false; }
		}

		Project _Solution.Item(object index)
		{
			return null;
		}

		void _Solution.Open(string FileName)
		{

		}

		DTE _Solution.Parent
		{
			get { return null; }
		}

		string _Solution.ProjectItemsTemplatePath(string ProjectKind)
		{
			return null;
		}

		Projects _Solution.Projects
		{
			get { return null; }
		}

		EnvDTE.Properties _Solution.Properties
		{
			get { return null; }
		}

		void _Solution.Remove(Project proj)
		{

		}

		void _Solution.SaveAs(string FileName)
		{

		}

		bool _Solution.Saved
		{
			get
			{
				return false;
			}
			set
			{

			}
		}

		SolutionBuild _Solution.SolutionBuild
		{
			get { return null; }
		}

		object _Solution.get_Extender(string ExtenderName)
		{
			return null;
		}

		string _Solution.get_TemplatePath(string ProjectType)
		{
			return null;
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		#endregion

		#region ProjectItem Members

		ProjectItems ProjectItem.Collection
		{
			get { return null; }
		}

		ConfigurationManager ProjectItem.ConfigurationManager
		{
			get { return null; }
		}

		Project ProjectItem.ContainingProject
		{
			get { return null; }
		}

		DTE ProjectItem.DTE
		{
			get { return null; }
		}

		void ProjectItem.Delete()
		{

		}

		Document ProjectItem.Document
		{
			get { return null; }
		}

		void ProjectItem.ExpandView()
		{

		}

		string ProjectItem.ExtenderCATID
		{
			get { return null; }
		}

		object ProjectItem.ExtenderNames
		{
			get { return null; }
		}

		FileCodeModel ProjectItem.FileCodeModel
		{
			get { return null; }
		}

		short ProjectItem.FileCount
		{
			get { return 0; }
		}

		bool ProjectItem.IsDirty
		{
			get
			{
				return false;
			}
			set
			{

			}
		}

		string ProjectItem.Kind
		{
			get { return null; }
		}

		string ProjectItem.Name
		{
			get
			{
				return "<<Deleted Reference>>";
			}
			set
			{

			}
		}

		object ProjectItem.Object
		{
			get { return null; }
		}

		Window ProjectItem.Open(string ViewKind)
		{
			return null;
		}

		ProjectItems ProjectItem.ProjectItems
		{
			get { return null; }
		}

		EnvDTE.Properties ProjectItem.Properties
		{
			get { return null; }
		}

		void ProjectItem.Remove()
		{

		}

		void ProjectItem.Save(string FileName)
		{

		}

		bool ProjectItem.SaveAs(string NewFileName)
		{
			return false;
		}

		bool ProjectItem.Saved
		{
			get
			{
				return false;
			}
			set
			{

			}
		}

		Project ProjectItem.SubProject
		{
			get { return null; }
		}

		object ProjectItem.get_Extender(string ExtenderName)
		{
			return null;
		}

		string ProjectItem.get_FileNames(short index)
		{
			return null;
		}

		bool ProjectItem.get_IsOpen(string ViewKind)
		{
			return false;
		}

		#endregion

		#region Project Members

		CodeModel Project.CodeModel
		{
			get { return null; }
		}

		Projects Project.Collection
		{
			get { return null; }
		}

		ConfigurationManager Project.ConfigurationManager
		{
			get { return null; }
		}

		DTE Project.DTE
		{
			get { return null; }
		}

		void Project.Delete()
		{

		}

		string Project.ExtenderCATID
		{
			get { return null; }
		}

		object Project.ExtenderNames
		{
			get { return null; }
		}

		string Project.FileName
		{
			get { return null; }
		}

		string Project.FullName
		{
			get { return null; }
		}

		Globals Project.Globals
		{
			get { return null; }
		}

		bool Project.IsDirty
		{
			get
			{
				return false;
			}
			set
			{

			}
		}

		string Project.Kind
		{
			get { return null; }
		}

		string Project.Name
		{
			get
			{
				return "<<Deleted Reference>>";
			}
			set
			{

			}
		}

		object Project.Object
		{
			get { return null; }
		}

		ProjectItem Project.ParentProjectItem
		{
			get { return null; }
		}

		ProjectItems Project.ProjectItems
		{
			get { return null; }
		}

		EnvDTE.Properties Project.Properties
		{
			get { return null; }
		}

		void Project.Save(string FileName)
		{

		}

		void Project.SaveAs(string NewFileName)
		{

		}

		bool Project.Saved
		{
			get
			{
				return false;
			}
			set
			{

			}
		}

		string Project.UniqueName
		{
			get { return null; }
		}

		object Project.get_Extender(string ExtenderName)
		{
			return null;
		}

		#endregion

		#region SolutionFolder Members

		Project SolutionFolder.AddFromFile(string FileName)
		{
			return null;
		}

		Project SolutionFolder.AddFromTemplate(string FileName, string Destination, string ProjectName)
		{
			return null;
		}

		Project SolutionFolder.AddSolutionFolder(string Name)
		{
			return null;
		}

		DTE SolutionFolder.DTE
		{
			get { return null; }
		}

		bool SolutionFolder.Hidden
		{
			get
			{
				return false;
			}
			set
			{

			}
		}

		Project SolutionFolder.Parent
		{
			get { return null; }
		}

		#endregion

		#region Empty Value

		static EmptyDteElement instance;

		static EmptyDteElement()
		{
			instance = new EmptyDteElement();
		}

		static internal EmptyDteElement EmptyValue()
		{
			return instance;
		}

		#endregion
	}

	#endregion
}
