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
using Microsoft.Practices.RecipeFramework.Library.Actions.Vsd;

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.Actions
{
    public class AddVBTemplatesSupport : ConfigurableAction
    {
        private bool addVBSupport;

        /// <summary>
        /// Gets or sets a value indicating whether add VB support.
        /// </summary>
        [Input()]
        public bool AddVBSupport
        {
            get { return addVBSupport; }
            set { addVBSupport = value; }
        }

        private EnvDTE.Project setupProject;

        [Input(Required=true)]
        public EnvDTE.Project SetupProject
        {
            get { return setupProject; }
            set { setupProject = value; }
        }

        private string message;

        [Input(Required = true)]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        private string installUrl;

        [Input(Required = true)]
        public string InstallUrl
        {
            get { return installUrl; }
            set { installUrl = value; }
        }

        private string registryKey;

        [Input(Required = true)]
        public string RegistryKey
        {
            get { return registryKey; }
            set { registryKey = value; }
        }
	

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
        /// </summary>
        public override void Execute()
        {
            if (addVBSupport)
            {
                AddRegistryLocatorAction registryLocatorAction = new AddRegistryLocatorAction();
                this.Container.Add(registryLocatorAction);
                registryLocatorAction.SetupProject = this.setupProject;
                registryLocatorAction.LocatorName = "VB";
                registryLocatorAction.Property = "VB";
                registryLocatorAction.Value = "Package";
                registryLocatorAction.RegistryKey = registryKey;
                registryLocatorAction.Execute();

                AddLaunchConditionAction launchConditionAction = new AddLaunchConditionAction();
                this.Container.Add(launchConditionAction);

                launchConditionAction.Message = this.message;
                launchConditionAction.InstallUrl = this.installUrl;
                launchConditionAction.Condition = "VB";
                launchConditionAction.LaunchConditionName = "VB Language";
                launchConditionAction.SetupProject = setupProject;
                launchConditionAction.Execute();
            }
        }

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
        /// </summary>
        public override void Undo()
        {
            // nothing to do...
        }
    }
}
