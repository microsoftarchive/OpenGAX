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
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Services;
using System.IO;

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.Actions
{
    /// <summary>
    /// Sets the Post Build Event of a VS Setup project to execute the NoImpersonate.js script
    /// and copy this script to the newly unfolded Guidance Package
    /// </summary>
    public class SetSetupPostBuildEventAction : ConfigurableAction
    {
        EnvDTE.Project setupProject;
        string previousPostBuildEvent = String.Empty;
        const string SETUPPOSTBUILDEVENTTEXT = "\"$(ProjectDir)\\NoImpersonate.js\" \"$(BuiltOuputPath)\"";
        const string SCRIPT_FILENAME = "NoImpersonate.js";

        #region Input Properties

        /// <summary>
        /// The project where the post build event is going to be added to
        /// </summary>
        [Input(Required=true)]
        public EnvDTE.Project SetupProject
        {
            get { return setupProject; }
            set { setupProject = value; }
        } 

        #endregion

        #region IAction Members

        /// <summary>
        /// Sets the Post Build Event to execute the NoImpersonate.js script
        /// </summary>
        public override void Execute()
        {
            if (setupProject.Properties.Count > 0)
            {
                if (setupProject.Properties.Item("PostBuildEvent") != null)
                {
                    // set the text for the post build event
                    previousPostBuildEvent = (String)setupProject.Properties.Item("PostBuildEvent").Value;
                    setupProject.Properties.Item("PostBuildEvent").Value = SETUPPOSTBUILDEVENTTEXT;
                    // copy the file from GAT bin folder to the [PackageName]Setup folder of the just unfolded guidance package
                    File.Copy(GetSourceScriptPath(), GetTargetScriptPath());
                }
            }

        }

        /// <summary>
        /// Gets the path to where to copy NoImpersonate.js script from.
        /// This script lives in the GAT \Templates folder.
        /// </summary>
        /// <returns></returns>
        private String GetSourceScriptPath()
        {
            IConfigurationService configurationService = GetService<IConfigurationService>();
            return Path.Combine(configurationService.BasePath + @"\Templates", SCRIPT_FILENAME);
        }

        /// <summary>
        /// Gets the path to where to copy NoImpersonate.js script to.
        /// This script should be copied inside the [PackageName]Setup folder which is where the post build event
        /// is expecting it to be.
        /// </summary>
        /// <returns></returns>
        private String GetTargetScriptPath()
        {
            IDictionaryService args = GetService<IDictionaryService>();
            DTE vs = GetService<DTE>();

            String packageName = (String)args.GetValue("PackageName");
            String solutionFolder = (string)(vs.Solution.Properties.Item("Path").Value);
            solutionFolder = solutionFolder.Substring(0, solutionFolder.LastIndexOf("\\"));
            String setupProjectFolder = solutionFolder + @"\" + packageName + "Setup";
            return Path.Combine(setupProjectFolder, SCRIPT_FILENAME);
        }

        /// <summary>
        /// Restore the post build action to whatever value it had before the Execute method ran
        /// </summary>
        public override void Undo()
        {
            if (setupProject.Properties.Count > 0)
            {
                if (setupProject.Properties.Item("PostBuildEvent") != null)
                {
                    // restore the post build event to its previous value
                    setupProject.Properties.Item("PostBuildEvent").Value = previousPostBuildEvent;
                    // if the script file was copied, delete it
                    if (File.Exists(GetTargetScriptPath()))
                    {
                        File.Delete(GetTargetScriptPath());
                    }
                }
            }

        }

        #endregion
    }
}
