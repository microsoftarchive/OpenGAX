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
using EnvDTE;
using Microsoft.Practices.Common;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions.Vsd
{
    /// <summary>
    /// Action that creates a new custom action in a setup project
    /// The 5 input properties used are: (a) SetupProject - the project
    /// where the new custom action is going to be added (b) ActionName
    /// - the name of the new custom action (c) InstallAction - the action
    /// that is going to be added, it could be Install, Uninstall, Commit,
    /// Rollback (d) Assembly - the assembly name where the action will
    /// be executed at the time of installation (e) CustomActionData - the 
    /// string parameter that is going to be passed to the installutil
    /// </summary>
    public class AddCustomAction : ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// The project where the new custom action is going to be added
        /// </summary>
        [Input(Required=true)]
        public EnvDTE.Project SetupProject
        {
            get { return setupProject; }
            set { setupProject = value; }
        } EnvDTE.Project setupProject;

        /// <summary>
        /// The name of the new custom action
        /// </summary>
        [Input(Required=true)]
        public string ActionName
        {
            get { return actionName; }
            set { actionName = value; }
        } string actionName;

        /// <summary>
        /// The action that is going to be added, it could be Install, 
        /// Uninstall, Commit or Rollback
        /// </summary>
        [Input(Required=true)]
        public string InstallAction
        {
            get { return installAction; }
            set { installAction = value; }
        } string installAction;

        /// <summary>
        /// The assembly name where the action will
        /// be executed at the time of installation
        /// </summary>
        [Input(Required=true)]
        public object Assembly
        {
            get { return assembly; }
            set { assembly = value; }
        } object assembly;

        /// <summary>
        /// The string parameter that is going to be passed to the installutil
        /// </summary>
        [Input(Required=true)]
        public string CustomActionData
        {
            get { return customActionData; }
            set { customActionData = value; }
        } string customActionData;

        #endregion

        #region Output Propeties

        /// <summary>
        /// The object to the new custom action that is created
        /// by the execution of the action
        /// </summary>
        public object CustomAction
        {
            get { return customAction; }
            set { customAction = (IVsdCustomAction)value; }
        } IVsdCustomAction customAction;

        #endregion

        #region IAction Members

        /// <summary>
        /// Creates the new custom action with the given parameters
        /// </summary>
        public override void Execute()
        {
            try
            {
                IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
                IVsdCollectionSubset plugins= deployable.GetPlugIns();
                IVsdCustomActionPlugIn customActionPlugin = plugins.Item("CustomAction") as IVsdCustomActionPlugIn;
                if (customActionPlugin != null)
                {
                    customAction =
                        (IVsdCustomAction)DteHelper.CoCreateInstance(
							this.Site, 
                            typeof(VsdCustomActionClass),
                            typeof(IVsdCustomAction));
                    customAction.Name = this.ActionName;
                    customAction.InstallerClass = true;
                    customAction.Object = this.Assembly;
                    customAction.FileType = vsdCustomActionFileTypes.vsdcaDll;
                    if (InstallAction.Equals("Install"))
                    {
                        customAction.InstallAction = vsdCustomActionInstallActions.vsdcaInstall;
                    }
                    else if (InstallAction.Equals("Uninstall"))
                    {
                        customAction.InstallAction = vsdCustomActionInstallActions.vsdcaUninstall;
                    }
                    else if (InstallAction.Equals("Commit"))
                    {
                        customAction.InstallAction = vsdCustomActionInstallActions.vsdcaCommit;
                    }
                    else if (InstallAction.Equals("Rollback"))
                    {
                        customAction.InstallAction = vsdCustomActionInstallActions.vsdcaRollback;
                    }
                    else
                    { 
                        throw new InvalidOperationException(
                            String.Format(CultureInfo.CurrentCulture, Properties.Resources.AddCustomAction_InvalidInstallAction,
                            InstallAction));
                    }
                    customAction.CustomActionData = this.CustomActionData;
                    customActionPlugin.Items.Add(customAction);
                }
            }
            catch(Exception)
            {
                if ( CustomAction!=null )
                {
                    CustomAction = null;
                }
                throw;
            }
        }

        /// <summary>
        /// Removes the custom action created by the execution 
        /// of the action
        /// </summary>
        public override void Undo()
        {
            if (customAction != null)
            {
                IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
                IVsdCollectionSubset plugins = deployable.GetPlugIns();
                IVsdCustomActionPlugIn customActionPlugin = plugins.Item("CustomAction") as IVsdCustomActionPlugIn;
                customActionPlugin.Items.RemoveObject(customAction);
                customAction = null;
            }
        }

        #endregion
    }
}
