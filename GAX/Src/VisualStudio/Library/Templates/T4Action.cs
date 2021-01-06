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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.ComponentModel;
using System.ComponentModel.Design;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.VisualStudio.TextTemplating.VSHost;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates
{
	/// <summary>
	/// Abstract action that implementes T4 rendering.
	/// </summary>
	[ServiceDependency(typeof(ITypeResolutionService))]
    public abstract class T4Action : DynamicInputAction
    {
        #region Private Implementation


        /// <summary>
        /// Uses ITypeResolutionService to obtain the package root folder path 
        /// </summary>
        /// <returns></returns>
        protected string GetBasePath()
        {
            // Path is always relative to package root folder.
            // WARNING: if we were to copy our files and zip them, this may not work.
			IConfigurationService config = GetService<IConfigurationService>(true);
			Debug.Assert(!String.IsNullOrEmpty(config.BasePath));
            return config.BasePath;
        }

        /// <summary>
        /// Returns the base path for all templates in the current package
        /// </summary>
        /// <returns></returns>
        protected string GetTemplateBasePath()
        {
            return new DirectoryInfo(GetBasePath() + "\\Templates").FullName;
        }

        #endregion

        #region Protected Implementation

        /// <summary>
        /// Invokes the T4 rendering engine
        /// </summary>
        /// <param name="templateCode">The T4 template to render</param>
        /// <param name="templateFile">The file containing the template code</param>
        /// <returns>The rendered result</returns>
        protected string Render(string templateCode,string templateFile)
        {
            // Get the package root folder.
            string basePath = GetBasePath();

			// Create the Engine
			ITextTemplatingEngine engine = new Engine();

            // Build arguments.
            IValueInfoService mdService = (IValueInfoService)GetService(typeof(IValueInfoService));
            Dictionary<string, PropertyData> args = new Dictionary<string, PropertyData>();
            foreach (string key in additionalArguments.Keys)
            {
                Type type = null;
                try
                {
                    type = mdService.GetInfo(key).Type;
                }
                catch (ArgumentException)
                {
                    // Type is not defined in recipe, so take it from the argument value itself.
                    // This is useful for values passed-in as configuration attributes.
                    if (additionalArguments[key] != null)
                    {
                        type = additionalArguments[key].GetType();
                    }
                    else
                    {
                        // Can't determine type, so we can't emit a property.
                        continue;
                    }
                }

                PropertyData propertyData = new PropertyData(additionalArguments[key], type);
                args.Add(key, propertyData);
            }

			// Create the Host. References will be resolved relative to the guidance 
			// package installation folder.

			Type hostServiceType = typeof(STextTemplating);
			// hostServiceType = hostServiceType.Assembly.GetType("TextTemplatingService");
			ITextTemplatingEngineHost syshost = this.GetService(hostServiceType) as ITextTemplatingEngineHost;

			TemplateHost host = new TemplateHost(basePath, args);
            host.TemplateFile = templateFile;
			host.SysHost = syshost;

			// Set the output
			string content = engine.ProcessTemplate(templateCode, host);
            // Looking for errors
            if (host.Errors.HasErrors)
            {
				this.TraceError(host.Errors[0].ToString());
                throw new TemplateException(host.Errors);
            }
            else if (host.Errors.HasWarnings)
            {
                StringBuilder warnings = new StringBuilder();
                foreach (CompilerError warning in host.Errors)
                {
                    warnings.AppendLine(warning.ToString());
                }

                this.TraceInformation(String.Format( 
                    CultureInfo.CurrentCulture,
                    Properties.Resources.T4Action_CompilationWarnings, 
                    templateFile,
                    warnings.ToString()));
            }
            return content;
        }

        #endregion
    }
}
