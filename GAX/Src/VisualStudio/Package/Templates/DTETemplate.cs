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

#region Using Directives

using System;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Templates
{
    /// <summary>
    /// This class is a proxy class for the EnvDTE.DTE class.
    /// An instance of this class is passed to the CommonIDE.Templates.Wizard class to be
    /// able to do project name replacement in a multi project template
    /// </summary>
    internal sealed class DTETemplate : EnvDTE.DTE, Microsoft.VisualStudio.OLE.Interop.IServiceProvider
    {
        EnvDTE.DTE dte;
        IDictionaryService replacementDictionary;

        public DTETemplate(EnvDTE.DTE dte)
        {
            if (dte == null)
            {
                throw new ArgumentNullException("dte");
            }
            this.dte = dte;
        }

        #region _DTE Members

        EnvDTE.Document EnvDTE._DTE.ActiveDocument
        {
            get { return dte.ActiveDocument; }
        }

        object EnvDTE._DTE.ActiveSolutionProjects
        {
            get { return dte.ActiveSolutionProjects; }
        }

        EnvDTE.Window EnvDTE._DTE.ActiveWindow
        {
            get { return dte.ActiveWindow; }
        }

        EnvDTE.AddIns EnvDTE._DTE.AddIns
        {
            get { return dte.AddIns; }
        }

        EnvDTE.DTE EnvDTE._DTE.Application
        {
            get { return dte.Application; }
        }

        object EnvDTE._DTE.CommandBars
        {
            get { return dte.CommandBars; }
        }

        string EnvDTE._DTE.CommandLineArguments
        {
            get { return dte.CommandLineArguments; }
        }

        EnvDTE.Commands EnvDTE._DTE.Commands
        {
            get { return dte.Commands; }
        }

        EnvDTE.ContextAttributes EnvDTE._DTE.ContextAttributes
        {
            get { return dte.ContextAttributes; }
        }

        EnvDTE.DTE EnvDTE._DTE.DTE
        {
            get { return this; }
        }

        EnvDTE.Debugger EnvDTE._DTE.Debugger
        {
            get { return dte.Debugger; }
        }

        EnvDTE.vsDisplay EnvDTE._DTE.DisplayMode
        {
            get
            {
                return dte.DisplayMode;
            }
            set
            {
                dte.DisplayMode = value;
            }
        }

        EnvDTE.Documents EnvDTE._DTE.Documents
        {
            get { return dte.Documents; }
        }

        string EnvDTE._DTE.Edition
        {
            get { return dte.Edition; }
        }

        EnvDTE.Events EnvDTE._DTE.Events
        {
            get { return dte.Events; }
        }

        void EnvDTE._DTE.ExecuteCommand(string CommandName, string CommandArgs)
        {
            dte.ExecuteCommand(CommandName, CommandArgs);
        }

        string EnvDTE._DTE.FileName
        {
            get { return dte.FileName; }
        }

        EnvDTE.Find EnvDTE._DTE.Find
        {
            get { return dte.Find; }
        }

        string EnvDTE._DTE.FullName
        {
            get { return dte.FullName; }
        }

        object EnvDTE._DTE.GetObject(string Name)
        {
            return dte.GetObject(Name);
        }

        EnvDTE.Globals EnvDTE._DTE.Globals
        {
            get { return dte.Globals; }
        }

        EnvDTE.ItemOperations EnvDTE._DTE.ItemOperations
        {
            get { return dte.ItemOperations; }
        }

        EnvDTE.wizardResult EnvDTE._DTE.LaunchWizard(string VSZFile, ref object[] ContextParams)
        {
            if (this.replacementDictionary == null && UnfoldTemplate.UnfoldingTemplates.Count > 0 )
            {
                UnfoldTemplate unfoldTemplate = (UnfoldTemplate)UnfoldTemplate.UnfoldingTemplates.Peek();
                this.replacementDictionary = unfoldTemplate.ReplacementDictionary;
            }
            for (int i = 0; i < ContextParams.Length; i++)
            {
                if (ContextParams[i] is string)
                {
                    ContextParams[i] = DteHelper.ReplaceParameters(ContextParams[i] as string, this.replacementDictionary);
                }
            }
            return dte.LaunchWizard(VSZFile, ref ContextParams);
        }

        int EnvDTE._DTE.LocaleID
        {
            get { return dte.LocaleID; }
        }

        EnvDTE.Macros EnvDTE._DTE.Macros
        {
            get { return dte.Macros; }
        }

        EnvDTE.DTE EnvDTE._DTE.MacrosIDE
        {
            get { return dte.MacrosIDE; }
        }

        EnvDTE.Window EnvDTE._DTE.MainWindow
        {
            get { return dte.MainWindow; }
        }

        EnvDTE.vsIDEMode EnvDTE._DTE.Mode
        {
            get { return dte.Mode; }
        }

        string EnvDTE._DTE.Name
        {
            get { return dte.Name; }
        }

        EnvDTE.ObjectExtenders EnvDTE._DTE.ObjectExtenders
        {
            get { return dte.ObjectExtenders; }
        }

        EnvDTE.Window EnvDTE._DTE.OpenFile(string ViewKind, string FileName)
        {
            return dte.OpenFile(ViewKind, FileName);
        }

        void EnvDTE._DTE.Quit()
        {
            dte.Quit();
        }

        string EnvDTE._DTE.RegistryRoot
        {
            get { return dte.RegistryRoot; }
        }

        string EnvDTE._DTE.SatelliteDllPath(string Path, string Name)
        {
            return dte.SatelliteDllPath(Path, Name);
        }

        EnvDTE.SelectedItems EnvDTE._DTE.SelectedItems
        {
            get { return dte.SelectedItems; }
        }

        EnvDTE.Solution EnvDTE._DTE.Solution
        {
            get { return dte.Solution; }
        }

        EnvDTE.SourceControl EnvDTE._DTE.SourceControl
        {
            get { return dte.SourceControl; }
        }

        EnvDTE.StatusBar EnvDTE._DTE.StatusBar
        {
            get { return dte.StatusBar; }
        }

        bool EnvDTE._DTE.SuppressUI
        {
            get
            {
                return dte.SuppressUI;
            }
            set
            {
                dte.SuppressUI = value;
            }
        }

        EnvDTE.UndoContext EnvDTE._DTE.UndoContext
        {
            get { return dte.UndoContext; }
        }

        bool EnvDTE._DTE.UserControl
        {
            get
            {
                return dte.UserControl;
            }
            set
            {
                dte.UserControl = value;
            }
        }

        string EnvDTE._DTE.Version
        {
            get { return dte.Version; }
        }

        EnvDTE.WindowConfigurations EnvDTE._DTE.WindowConfigurations
        {
            get { return dte.WindowConfigurations; }
        }

        EnvDTE.Windows EnvDTE._DTE.Windows
        {
            get { return dte.Windows; }
        }

        bool EnvDTE._DTE.get_IsOpenFile(string ViewKind, string FileName)
        {
            return dte.get_IsOpenFile(ViewKind, FileName);
        }

        EnvDTE.Properties EnvDTE._DTE.get_Properties(string Category, string Page)
        {
            return dte.get_Properties(Category, Page);
        }

        #endregion

        #region IServiceProvider Members

        int Microsoft.VisualStudio.OLE.Interop.IServiceProvider.QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
        {
            return ((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)dte).QueryService(ref guidService,ref riid,out ppvObject);
        }

        #endregion
    }
}
