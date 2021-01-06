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
using System.Diagnostics;
using System.Xml;
using Microsoft.Practices.RecipeFramework.Configuration;
using System.IO;

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.Actions
{
	/// <summary>
	/// Adds a VSIX manifest project item based on the Guidance Package configuration file
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	public class AddVsixManifestAction : Action
	{
		/// <summary>
		/// The guidance package project.
		/// </summary>
		[Input(Required = true)]
		public Project PackageProject { get; set; }

		/// <summary>
		/// Does nothing, as un-registration must be done explicitly.
		/// </summary>
		public override void Undo()
		{
			// Must un-register to undo.
		}

		/// <summary>
		/// Adds the vsix manifest to the guidance package project
		/// </summary>
		public override void Execute()
		{
			TraceUtil.TraceInformation(this, "Adding VSIX manifest...");

			// Search the guidance package configuration file
			var configurationFile = GetConfigurationFileName(this.PackageProject);
			
			// Generate the vsix manifest content
			var template = new VsixManifestTemplate();
			template.GuidancePackage = Microsoft.Practices.RecipeFramework.GuidancePackage.ReadConfiguration(configurationFile);
			template.ConfigurationFile = Path.GetFileName(configurationFile);

			var vsixManifestContent = template.TransformText();

			// Write the content
			var targetManifestFile = Path.Combine(Path.GetDirectoryName(this.PackageProject.FullName), "source.extension.vsixmanifest");
			File.WriteAllText(targetManifestFile, vsixManifestContent);

			// Add the manifest to the guidance package project
			var manifest = this.PackageProject.ProjectItems.AddFromFile(targetManifestFile);
			
			// Set vsix manifest project item properties
			manifest.Properties.Item("BuildAction").Value = 0;
			manifest.Properties.Item("CopyToOutputDirectory").Value = 0;
			manifest.Properties.Item("ItemType").Value = "None";
		}

		private string GetConfigurationFileName(Project project)
		{
			foreach (ProjectItem item in project.ProjectItems)
			{
				if (item.Name.EndsWith(".xml"))
				{
					using (XmlReader reader = XmlReader.Create(item.get_FileNames(1)))
					{
						reader.MoveToContent();
						if (reader.LocalName == ElementNames.GuidancePackage &&
							reader.NamespaceURI == SchemaInfo.PackageNamespace)
						{
							string file = Path.GetFileName(item.get_FileNames(1));

							if (!Path.IsPathRooted(file))
							{
								file = Path.Combine(Path.GetDirectoryName(project.FileName), file);
							}
							return file;
						}
					}
				}
			}
			throw new InvalidOperationException(Properties.Resources.Registration_NoPackageConfig);
		}
	}
}