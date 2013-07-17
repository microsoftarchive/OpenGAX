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
using Microsoft.VisualStudio.ExtensionManager;
using System.IO;
using System.Xml;

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Extension methods for <see cref="IVsExtensionManager"/>
	/// </summary>
	internal static class ExtensionManagerExtensions
	{
		internal static IEnumerable<IInstalledExtension> GetGuidancePackages(this IVsExtensionManager extensionManager)
		{
			return extensionManager
				.GetEnabledExtensions()
				.Where(extension => extension.AsGuidancePackage() != null);
		}
	}

	internal static class InstalledExtensionExtensions
	{
		private static string GetGuidancePackageManifest(this IInstalledExtension extension)
		{
			var customExtension = extension.Content.FirstOrDefault(content => content.ContentTypeName == "GuidancePackage");

			return customExtension != null ? customExtension.RelativePath : null;
		}

		internal static Configuration.Manifest.GuidancePackage AsGuidancePackage(this IInstalledExtension extension)
		{
			var manifest = extension.GetGuidancePackageManifest();

			if (manifest != null)
			{
				var package = new Configuration.Manifest.GuidancePackage()
				{
					Caption = extension.Header.Name,
					ConfigurationFile = System.IO.Path.Combine(extension.InstallPath, manifest),
					Description = extension.Header.Description,
					Host = "VisualStudio",
					Version = extension.Header.Version.ToString()
				};

				if(File.Exists(package.ConfigurationFile))
				{
					using (var reader = XmlReader.Create(package.ConfigurationFile))
					{
						if (reader.ReadToDescendant("GuidancePackage", @"http://schemas.microsoft.com/pag/gax-core"))
						{
							package.Guid = reader.GetAttribute("Guid");
							package.Name = reader.GetAttribute("Name");

							return package;
						}						
					}
				}
			}

			return null; 
		}
	}
}
