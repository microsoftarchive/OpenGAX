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
using System.Text;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Common
{
    /// <summary>
    /// Interface to gather data and meta data about a Visual Studio template
    /// </summary>
    public interface IVsTemplate
    {
        /// <summary>
        /// Caption of the template
        /// </summary>
        string              Name           { get; }

        /// <summary>
        /// Description
        /// </summary>
        string              Description    { get; }

        /// <summary>
        /// The complete path to the .vstemplate file
        /// </summary>
        string              FileName       { get; }

        /// <summary>
        /// The Icon file name for this template
        /// </summary>
        string              IconFileName   { get; }

        /// <summary>
        /// The Icon for this template
        /// </summary>
        CommandID           Icon           { get; }

        /// <summary>
        /// The kind of template (Solution|Project|ProjectItem) based on the folder location
        /// </summary>
        TemplateKind       Kind           { get; }

        /// <summary>
        /// The kind of template based on the contents of the .vstemplate file
        /// </summary>
        WizardRunKind      VSKind         { get; } 

        /// <summary>
        /// The name of the package that owns this template
        /// </summary>
        string             PackageName    { get; }

        /// <summary>
        /// The WizardData asociated with this template
        /// </summary>
        VsTemplate.Template  ExtensionData  { get; }

        /// <summary>
        /// An integer representing the sort order and relative priority of the Wizard
        /// </summary>
        int SortPriority { get; }

        /// <summary>
        /// The default name for the Wizard, displayed in the Name field in the dialog box.
        /// </summary>
        string SuggestedBaseName { get; }

        /// <summary>
        /// Is the template visible from the Add New Dialog Box?
        /// </summary>
        bool IsVisibleInAddNewDialogBox { get; }

        /// <summary>
        /// Is the template visible from the Add New Context Menu?
        /// </summary>
        bool IsVisibleFromContextMenu { get; }

        /// <summary>
        /// The command to invoke the unfolding of this template
        /// </summary>
        CommandID Command { get; }

        /// <summary>
        /// Retrieves the language of the template
        /// </summary>
        string Language { get; }

        /// <summary>
        /// Retrieves the project factory for the current template
        /// </summary>
        Guid ProjectFactory { get; }

		/// <summary>
		/// Specifies if a new folder should be created
		/// </summary>
		bool CreateNewFolder { get; }

		/// <summary>
		/// Specifies if a new name exist
		/// </summary>
		bool ProvideDefaultName { get; }

		/// <summary>
		/// Specifies if the user can browse to a different directory to unfold the template in the Add New Dialog Box
		/// </summary>
		bool EnableLocationBrowseButton { get; }

		/// <summary>
		/// Specifies if the user can edit the location field in the Add New Dialog Box
		/// </summary>
		bool EnableEditOfLocationField { get; }

		/// <summary>
		/// Specifies whether the location field in the Add New Dialog Box is enabled, disabled or hidden.
		/// </summary>
		LocationField LocationField { get; }

		/// <summary>
		/// Specifies if the extension is added to the name of the new item
		/// </summary>
		bool AppendDefaultFileExtension { get; }

		/// <summary>
		/// Specifies if the language drop down should be displayed
		/// </summary>
		bool SupportsLanguageDropDown { get; }

		/// <summary>
		/// Supports a master page (WebOption)
		/// </summary>
		bool SupportsMasterPage { get; }
    }
}
