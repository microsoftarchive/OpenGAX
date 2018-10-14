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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using System.ComponentModel.Design;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Build
{
	/// <summary>
	/// Generates the .regpkg information for the Guidance Package
	/// </summary>
	public class GenerateGuidancePackageRegistry : Task
	{
		/// <summary>
		/// Gets or sets the guidance package configuration file
		/// </summary>
		[Required]
		public ITaskItem ConfigurationFile { get; set; }

		/// <summary>
		/// Gets or sets the filename of the generated UI dll
		/// </summary>
		[Required]
		public string SatelliteDllFile { get; set; }

		/// <summary>
		/// Gets or sets the template files (*.vstemplate) of the guidance package
		/// </summary>
		[Required]
		public ITaskItem[] Templates { get; set; }

		/// <summary>
		/// Gets or sets the output path of the current project
		/// </summary>
		[Required]
		public string OutputPath { get; set; }

		/// <summary>
		/// Gets or sets the output file for the guidance package registry entries
		/// </summary>
		[Required]
		public string OutputFile { get; set; }      

        /// <summary>
        /// Gets or sets the output template cache files
        /// </summary>
        [Microsoft.Build.Framework.Output]
		public ITaskItem[] CacheFiles { get; set; }

		/// <summary>
		/// Executes the task
		/// </summary>
		/// <returns></returns>
		public override bool Execute()
		{
			var configuration = Microsoft.Practices.RecipeFramework.GuidancePackage.ReadConfiguration(Path.GetFileName(this.ConfigurationFile.ItemSpec));
			
			var cacheFiles = new List<string>();
			var addItemsProjectFactories = new List<Guid>();
            List<ITaskItem> templateList = this.Templates.ToList();

            foreach (var vsTemplate in this.Templates)
			{
				var templateMetadata = new TemplateMetaData(
					Path.Combine(this.OutputPath, vsTemplate.ItemSpec),
					new CommandID(new Guid(configuration.Guid), templateList.IndexOf(vsTemplate) + 1),
					configuration.Name,
					RegistryHelper.GetCurrentVsRegistryKey(false));

				templateMetadata.Register(true);

				if (templateMetadata.Kind == Common.TemplateKind.ProjectItem && !addItemsProjectFactories.Contains(templateMetadata.ProjectFactory))
					addItemsProjectFactories.Add(templateMetadata.ProjectFactory);

				cacheFiles.AddRange(Directory.EnumerateFiles(templateMetadata.CacheBaseBath));
			}

			var registryTemplate = new GuidancePackageRegistryTemplate();
			registryTemplate.GuidancePackage = configuration;
			registryTemplate.SatelliteDllFile = Path.GetFileName(this.SatelliteDllFile);
			registryTemplate.VsTemplates = this.Templates;
			registryTemplate.AddItemsProjectFactories = addItemsProjectFactories;
			registryTemplate.OutputPath = Path.GetFullPath(this.OutputPath);
            
			var content = registryTemplate.TransformText();

			this.CacheFiles = cacheFiles.Distinct().Select(file =>
				{
					var item = new TaskItem(file);
					item.SetMetadata("VSIXSubPath", Path.GetDirectoryName(file).Remove(0, this.OutputPath.Length));

					// Fix the output path in the .vsz file
					if (".vsz".Equals(Path.GetExtension(item.ItemSpec), StringComparison.InvariantCultureIgnoreCase))
					{
						var vsz = File.ReadAllText(file).Replace("Param=\"" + this.OutputPath, "Param=\"");

						File.Delete(file);
						File.WriteAllText(file, vsz);
					}

					return item;
				}
				).ToArray();

			if (File.Exists(this.OutputFile))
				File.Delete(this.OutputFile);

			File.WriteAllText(this.OutputFile, content);

			return true;
		}
	}
}