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
using System.Xml;
using Microsoft.Practices.RecipeFramework.Configuration;
using System.IO;
using System.Xml.XPath;
using Microsoft.Practices.RecipeFramework.VisualStudio.Properties;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Build
{
	/// <summary>
	/// Find the Guidance Package configuration file by reading the vsix manifest and searching for the Guidance Package custom extension
	/// </summary>
	public class FindGuidancePackageConfiguration : Task
	{
		/// <summary>
		/// Gets or sets the vsix manifest
		/// </summary>
		[Required]
		public ITaskItem VsixManifest { get; set; }

		/// <summary>
		/// Gets ro sets the guidance package configuration file
		/// </summary>
		[Microsoft.Build.Framework.Output]
		public ITaskItem Configuration { get; set; }

		private bool Fail(string message, params string[] args)
		{
			if (args != null)
				message = string.Format(message, args);

			this.Log.LogError(message);
			return false;
		}

		/// <summary>
		/// Executes the task
		/// </summary>
		/// <returns></returns>
		public override bool Execute()
		{
			// Read the vsix manifest and find the guidance package custom extension
			using (var reader = XmlReader.Create(this.VsixManifest.ItemSpec))
			{
				reader.MoveToContent();

				var guidancePackageCustomExtensions = 0;
				var guidancePackageConfigurationFile = string.Empty;

				if (reader.Name == "Vsix")
				{
					// Find a single Guidance Package custom extension
					while (reader.ReadToFollowing("CustomExtension", "http://schemas.microsoft.com/developer/vsx-schema/2010"))
					{
						if (reader.GetAttribute("Type") == "GuidancePackage")
						{
							guidancePackageCustomExtensions++;
							guidancePackageConfigurationFile = reader.ReadElementContentAsString();
						}
					}
				}
				else if (reader.Name == "PackageManifest") // new format, vsx-schema/2011
				{
					reader.ReadToDescendant("Assets");
					bool ok = reader.ReadToDescendant("Asset");
					while (ok)
					{
						if (reader.GetAttribute("Type") == "GuidancePackage")
						{
							guidancePackageCustomExtensions++;
							guidancePackageConfigurationFile = reader.GetAttribute("Path");
							break;
						}
						ok = reader.ReadToNextSibling("Asset");
					}
				}

				// Check if there is only one Custom Extension of type "GuidancePackage"
				if (guidancePackageCustomExtensions != 1)
					return Fail(Resources.Build_GuidancePackageCustomExtensionNotFound);

				// Check if the configuration file exists
				if (!File.Exists(guidancePackageConfigurationFile))
					return Fail(Resources.Build_GuidancePackageConfigurationFileNotFound, guidancePackageConfigurationFile);

				// Create the output task item		
				this.Configuration = new TaskItem(guidancePackageConfigurationFile);
				
				// Add the guidance package attributes (name, caption, description, etc) to the output Configuration task item as Metadata
				using (var configurationReader = XmlReader.Create(guidancePackageConfigurationFile))
				{
					configurationReader.MoveToContent();

					while (configurationReader.MoveToNextAttribute())
					{
						this.Configuration.SetMetadata(configurationReader.Name, configurationReader.Value);
					}
				}

				return true;
			}
		}
	}
}