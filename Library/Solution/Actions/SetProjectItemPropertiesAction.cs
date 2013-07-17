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
using System.Collections.Generic;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using VSLangProj;
using System.Collections.Specialized;
using System.Collections;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Solution.Actions
{
    /// <summary>
    /// Sets properties in a project item object
    /// </summary>
    public sealed class SetProjectItemPropertiesAction : ConfigurableAction
    {
        StringDictionary properties;

        #region Input Properties

        /// <summary>
        /// The project item object whose properties are been set to
        /// </summary>
        [Input(Required=true)]
        public ProjectItem ProjectItem
        {
            get { return projectItem; }
            set { projectItem = value; }
        } ProjectItem projectItem;

        #endregion

        #region Output Properties

        #endregion

        #region IAttributtesConfigurable members

        /// <summary>
        /// Stores the set of user defined attributes
        /// </summary>
        /// <param name="attributes"></param>
        public override void Configure(StringDictionary attributes)
        {
            properties = attributes;
        }

        #endregion

        #region Action members

        /// <summary>
        /// Sets the properties of the project item
        /// </summary>
        public override void Execute()
        {
            //FileProperties fileProperties = (FileProperties)this.ProjectItem.Object;
            //fileProperties.BuildAction = this.BuildAction;
            foreach (DictionaryEntry prop in properties)
            {
                this.ProjectItem.Properties.Item((string)prop.Key).Value = prop.Value;
            }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override void Undo()
        {
        }

        #endregion
    }
}
