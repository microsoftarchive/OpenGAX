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
using Config = System.Configuration.Configuration;
using System.Collections.Generic;
using Microsoft.Practices.RecipeFramework;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Configuration.Actions
{
    /// <summary>
    /// Adds a new configuration section to a <see cref="System.Configuration.Configuration"/> object
    /// </summary>
    public class AddConfigurationSectionAction : Action
    {
        #region Input Properties

        /// <summary>
        /// The configuration object where the section will be added
        /// </summary>
        [Input(Required=true)]
        public Config Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        } Config configuration;

        /// <summary>
        /// The name of the configuration section
        /// </summary>
        [Input(Required=true)]
        public string SectionName
        {
            get { return sectionName; }
            set { sectionName = value; }
        } string sectionName;

        /// <summary>
        /// Type of the new configuration section
        /// </summary>
        [Input(Required=false)]
        public Type SectionType
        {
            get { return sectionType; }
            set { sectionType = value;  }
        } Type sectionType = typeof(DefaultSection);

        #endregion

        #region Output Properties

        /// <summary>
        /// The output <see cref="System.Configuration.ConfigurationSection"/> object
        /// </summary>
        [Output]
        public ConfigurationSection ConfigurationSection
        {
            get { return configSection; }
            set { configSection = value; }
        } ConfigurationSection configSection;

        #endregion

        #region Action members

        /// <summary>
        /// <see cref="IAction.Execute"/>
        /// </summary>
        public override void Execute()
        {
            this.ConfigurationSection = this.Configuration.GetSection(this.SectionName);
            if (this.ConfigurationSection == null)
            {
                this.ConfigurationSection = (ConfigurationSection)
                    Activator.CreateInstance(this.SectionType);
                this.Configuration.Sections.Add(this.SectionName, this.ConfigurationSection);
            }
        }

        /// <summary>
        /// <see cref="IAction.Undo"/>
        /// </summary>
        public override void Undo()
        {
            this.Configuration.Sections.Remove(this.SectionName);
            this.ConfigurationSection = null;
        }

        #endregion
    }
}
