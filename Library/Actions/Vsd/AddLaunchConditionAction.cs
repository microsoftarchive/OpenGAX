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
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions.Vsd
{
    /// <summary>
    /// Tha action adds a new launch condition to a setup
    /// project with the given parameters
    /// The input properties are:
    /// 1. SetupProject is the project where the new launch condition wiil be added
    /// 2. LaunchConditionName is the string value with the name of the new launch condition
    /// 3. Condition is the string condition that represents the launch condition
    /// 4. Message is the error messsage in case the condition fails
    /// 5. InstallURL is the link when the user clicks Yes in the error message specified
    /// </summary>
    public class AddLaunchConditionAction: ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// Setup project where the new launch condition will be added
        /// </summary>
        [Input]
        public EnvDTE.Project SetupProject
        {
            get { return setupProject; }
            set { setupProject = value; }
        } EnvDTE.Project setupProject;

        /// <summary>
        /// The key string condition name
        /// </summary>
        [Input(Required=true)]
        public string LaunchConditionName
        {
            get { return launchConditionName; }
            set { launchConditionName = value; }
        } string launchConditionName;

        /// <summary>
        /// The expression that represents the launch condition
        /// </summary>
        [Input(Required=true)]
        public object Condition
        {
            get { return condition; }
            set { condition = value; }
        } object condition;

        /// <summary>
        /// The error message string that will be displayed to the
        /// user in case the launch condition fails.  It has to be
        /// defined taking in account that uses the buttons yes/no
        /// </summary>
        [Input(Required=true)]
        public string Message
        {
            get { return message; }
            set { message = value; }
        } string message;

        /// <summary>
        /// Web link that the user will be directed in case respond
        /// yes to the error message displayed
        /// </summary>
        [Input(Required=true)]
        public string InstallUrl
        {
            get { return installUrl; }
            set { installUrl = value; }
        } string installUrl;

        #endregion

        #region Output Properties

        /// <summary>
        /// Object that represents the new created launch condition
        /// </summary>
        [Output]
        public object LaunchCondition
        {
            get { return vsdLaunchCondition; }
            set { vsdLaunchCondition = (IVsdLaunchCondition)value; }
        } IVsdLaunchCondition vsdLaunchCondition;

        #endregion

        #region IAction members

        /// <summary>
        /// Creates the new launch condition with the given values
        /// </summary>
        public override void Execute()
        {
            try
            {
                IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
                IVsdCollectionSubset plugins = deployable.GetPlugIns();
                IVsdLaunchConditionPlugIn launchConditionPlugin = plugins.Item("LaunchCondition") as IVsdLaunchConditionPlugIn;
                if (launchConditionPlugin != null)
                {
                    vsdLaunchCondition =
                        (IVsdLaunchCondition)DteHelper.CoCreateInstance(
							this.Site,
                            typeof(VsdLaunchConditionClass),
                            typeof(IVsdLaunchCondition));
                    vsdLaunchCondition.Name = this.LaunchConditionName;
                    vsdLaunchCondition.Condition = this.Condition;
                    vsdLaunchCondition.Message = this.Message;
                    vsdLaunchCondition.InstallUrl = this.InstallUrl;
                    launchConditionPlugin.Items.Add(vsdLaunchCondition);
                }
            }
            catch (Exception)
            {
                vsdLaunchCondition = null;
                throw;
            }

        }

        /// <summary>
        /// Removes the launch condition recently added
        /// </summary>
        public override void Undo()
        {
            if (vsdLaunchCondition != null)
            {
                IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
                IVsdCollectionSubset plugins = deployable.GetPlugIns();
                IVsdLaunchConditionPlugIn launchConditionPlugin = plugins.Item("LaunchCondition") as IVsdLaunchConditionPlugIn;
                if (launchConditionPlugin != null)
                {
                    launchConditionPlugin.Items.RemoveObject(vsdLaunchCondition);
                    vsdLaunchCondition = null;
                }
            }
        }

        #endregion
    }
}
