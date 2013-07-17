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
using System.Runtime.Serialization;
using EnvDTE;
using System.IO;
using System.Diagnostics;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using System.ComponentModel.Design;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio.Services;
using System.Security.Permissions;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Templates
{
    /// <summary>
    /// Creates a new refrence to a Visual Studio template
    /// </summary>
	[Serializable]
	[ServiceDependency(typeof(IVsTemplatesService))]
    [ServiceDependency(typeof(ITypeResolutionService))]
    [ServiceDependency(typeof(EnvDTE.DTE))]
	[CategoryResource(typeof(Properties.Resources), "TemplateReference_Category")]
    [RecipeFramework.ManagerExecutable(false)]
	public abstract class TemplateReference: AssetReference
	{
        /// <summary>
        /// Constructor from the template file name
        /// </summary>
        /// <param name="templateFileName"></param>
        protected TemplateReference(string templateFileName)
            : base(templateFileName)
		{
		}

		#region Properties

        /// <summary>
        /// The referenced Visual Studio template
        /// </summary>
		protected internal IVsTemplate Template
		{
			get
			{
				return template;
			}
        } IVsTemplate template;

		#endregion

		#region Overrides

        /// <summary>
        /// Gets the <see cref="IVsTemplate"/> interface once it is sited.
        /// <seealso cref="AssetReference.OnSited"/>
        /// </summary>
		protected override void OnSited()
		{
			base.OnSited();
			IConfigurationService config = GetService<IConfigurationService>(true);
            // Only look for base path if it's relative.
            if (!Path.IsPathRooted(base.AssetName))
            {
                base.AssetName = new FileInfo(Path.Combine(
                    config.BasePath + @"\Templates\", base.AssetName)).FullName;
                base.AssetName = new CompatibleUri(base.AssetName).LocalPath;
            }
            if (!File.Exists(this.AssetName))
			{ // Template does not exist, check with the Guidance package and update the template path 
				int relPathStartBefore = 0;
				string probeTemplateName = this.AssetName;
				while (!File.Exists(this.AssetName))
				{
					int relPathStart = probeTemplateName.IndexOf(@"\Templates\", relPathStartBefore, StringComparison.InvariantCultureIgnoreCase);
					if (relPathStart == -1 || relPathStart <= relPathStartBefore)
					{
						throw new FileNotFoundException(Properties.Resources.Templates_TemplateNotFound, this.AssetName);
					}
					relPathStartBefore = relPathStart;
					string relPath = probeTemplateName.Substring(relPathStart+1);
					this.AssetName = Path.Combine(config.BasePath, relPath);
				}
			}
            IVsTemplatesService infosvc = GetService<IVsTemplatesService>(true);
            this.template = infosvc.GetTemplate(base.AssetName);
		}

        /// <summary>
        /// <seealso cref="IAssetReference.Caption"/>
        /// </summary>
		public override string Caption
		{
			get
			{
                Debug.Assert(this.Template.Name != null);
                return this.Template.Name;
			}
		}

        /// <summary>
        ///  <seealso cref="IAssetReference.Description"/>
        /// </summary>
		public override string Description
		{
			get
			{
				return this.Template.Description;
			}
		}

        /// <summary>
        /// Displsys the Add New dialog box
        /// </summary>
        /// <returns></returns>
		protected override ExecutionResult OnExecute()
		{
            IMenuCommandService menuCommandService = GetService<IMenuCommandService>();
            
            if (menuCommandService != null)
            {
                MenuCommand command = menuCommandService.FindCommand(this.template.Command);
                command.Invoke();
            }
            return ExecutionResult.Finish;
		}

		#endregion

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected TemplateReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

		/// <summary>
		/// Required function for deserialization.
		/// </summary>
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
		}

        #endregion ISerializable Members
	}
}
