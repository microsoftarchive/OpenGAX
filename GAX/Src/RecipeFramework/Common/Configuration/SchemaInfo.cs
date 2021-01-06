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
using System.Text;

#endregion

namespace Microsoft.Practices.RecipeFramework.Configuration
{
	/// <summary>
	/// Exposes schema information for the framework configuration.
	/// </summary>
	public sealed class SchemaInfo
	{
		private SchemaInfo() {}

		/// <summary>
		/// Namespace of package configuration files.
		/// </summary>
		public const string PackageNamespace = "http://schemas.microsoft.com/pag/gax-core";

		/// <summary>
		/// Default namespace used in elements and queries, which equals "gax".
		/// </summary>
		public const string Prefix = "gax";

        /// <summary>
        /// Namespace of manifest files.
        /// </summary>
        public const string ManifestNamespace = "http://schemas.microsoft.com/pag/gax-manifest";
    }

    /// <summary>
    /// Element names used in the configuration schema.
    /// </summary>
    public sealed class ElementNames
    {
        private ElementNames() { }

        /// <summary>
        /// Root element of the configuration for a package, and the 
		/// node in the manifest specifying configuration for a package.
        /// </summary>
        public const string GuidancePackage = "GuidancePackage";
		/// <summary>
		/// Defines a host binding node.
		/// </summary>
		public const string Host = "Host";
    }

    /// <summary>
    /// Attribute names used in the configuration schema.
    /// </summary>
    public sealed class AttributeNames
    {
        private AttributeNames() { }

        /// <summary>
        /// The attribute that identifies a named element, such as the GuidancePackage, a Recipe or a Wizard.
        /// </summary>
        public const string Name = "Name";
		/// <summary>
		/// Version of an element or document.
		/// </summary>
		public const string Version = "Version";
		/// <summary>
		/// Display name of a package.
		/// </summary>
		public const string Caption = "Caption";
		/// <summary>
		/// Full description of a package.
		/// </summary>
		public const string Description = "Description";
        /// <summary>
        /// Timestamp of the package configuration file at installation time.
        /// </summary>
        public const string Timestamp = "Timestamp";
        /// <summary>
		/// Level of information tracing for a package or the framework.
		/// </summary>
		public const string SourceLevels = "SourceLevels";
        /// <summary>
        /// Optional unique identifier for a package on a host.
        /// </summary>
        public const string Guid = "Guid";
        /// <summary>
		/// Host where a package will run on.
		/// </summary>
		public const string Host = "Host";
		/// <summary>
		/// Location of the configuration file for a package.
		/// </summary>
		public const string ConfigurationFile = "ConfigurationFile";
		/// <summary>
		/// Attribute of the <see cref="ElementNames.Host"/> node that specifies the 
		/// type that performs installation of packages in the host.
		/// </summary>
		public const string InstallerType = "InstallerType";
    }
}
