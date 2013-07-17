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
using System.Xml;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.RecipeFramework.Library.Actions;
using EnvDTE;
using System.IO;

namespace Microsoft.Practices.RecipeFramework.Library.XML.Actions
{
    /// <summary>
    /// Serializes an <see cref="XmlDocument"/> into the content of a <see cref="ProjectItem"/>
    /// </summary>
	public sealed class SaveXmlAction: Action
    {
        #region Input Properties

        /// <summary>
        /// The <see cref="ProjectItem"/> whose content is XML
        /// </summary>
        [Input(Required=true)]
        public ProjectItem ProjectItem
        {
            get { return projectItem; }
            set { projectItem = value;  }
        } ProjectItem projectItem;

        /// <summary>
        /// The <see cref="XmlDocument"/> to save
        /// </summary>
        [Input(Required=true)]
        public XmlDocument XmlDoc
        {
            get { return xmlDoc; }
            set { xmlDoc = value; }
        } XmlDocument xmlDoc;

        #endregion

        #region Output properties

        #endregion

        #region Overrides

        /// <summary>
        /// Saves the xml file into the content of the project item
        /// </summary>
        public override void Execute()
        {
            using (SetProjectItemContentAction setContent = new SetProjectItemContentAction())
            {
                this.Container.Add(setContent);
                setContent.ProjectItem = this.ProjectItem;
                StringBuilder sb=new StringBuilder();
				XmlWriterSettings writerSettings = new XmlWriterSettings();
				writerSettings.Encoding = System.Text.Encoding.UTF8;
				writerSettings.Indent = true;
				writerSettings.CloseOutput = true;
				writerSettings.NewLineHandling = NewLineHandling.Entitize;
				writerSettings.OmitXmlDeclaration = true;
				using (XmlWriter xmlWriter = XmlWriter.Create(sb, writerSettings))
                {
					this.XmlDoc.Save(xmlWriter);
                }
                setContent.Content = sb.ToString();
                setContent.Execute();
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
