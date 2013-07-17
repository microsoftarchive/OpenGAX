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
using System.IO;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.Practices.RecipeFramework;
using Config = System.Configuration.Configuration;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Configuration.Actions
{
    /// <summary>
    /// Saves any changes to a Configuration object
    /// </summary>
    public sealed class SaveConfigurationAction : Action
    {
        #region Input Properties

        /// <summary>
        /// The configuration object to Save
        /// </summary>
        [Input(Required=true)]
        public Config Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        } Config configuration;

        #endregion

        #region Output Properties

        #endregion

        #region Action members

        /// <summary>
        /// <see cref="IAction.Execute"/>
        /// </summary>
        public override void Execute()
        {
            this.Configuration.Save(ConfigurationSaveMode.Full);
        }

        /// <summary>
        /// <see cref="IAction.Undo"/>
        /// </summary>
        public override void Undo()
        {
        }

        #endregion
    }
}

