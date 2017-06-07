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
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Security;
using Microsoft.Win32;
using System.Globalization;
using System.ComponentModel.Design;
using System.Runtime.Versioning;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates
{
	/// <summary>
	/// Provides Arguments-PropertyData pairs for the <see cref="PropertiesDirectiveProcessor"/>. 
	/// Resolves the physical path for a logical filename passed in as argument. 
	/// Creates a new AppDomain to run the generated transformation class. 
	/// </summary>
	public class TemplateHost : MarshalByRefObject, ITextTemplatingEngineHost
	{
		private const string propertyDirectiveProcessorName = "PropertyProcessor";
		private string binPath;
		private IDictionary<string, PropertyData> arguments;
		private System.CodeDom.Compiler.CompilerErrorCollection errors;

		#region Ctors

		/// <summary>
		/// Constructs the host.
		/// </summary>
		/// <param name="binPath">The path to resolve assemblies to.</param>
		/// <param name="arguments">The arguments to resolve properties to.</param>
        public TemplateHost(string binPath, IDictionary<string, PropertyData> arguments)
		{
			if (binPath == null)
			{
				throw new ArgumentNullException("binPath");
			}
			if (arguments == null)
			{
				throw new ArgumentNullException("arguments");
			}
            
			this.binPath = new DirectoryInfo(binPath).FullName;
			this.arguments = arguments;
			currentHost = this;
		}

		#endregion

		#region Properties

		static TemplateHost currentHost;

		/// <summary>
		/// Static property used by TextTemplate Properties to access this Host instance across 
		/// a domain boundary.
		/// </summary>
		public static TemplateHost CurrentHost
		{
			get { return currentHost; }
			set { currentHost = value; }
		} 
		
		/// <summary>
		/// Arguments-PropertyData pairs for the directive
		/// </summary>
		public IDictionary<string, PropertyData> Arguments
		{
			get { return this.arguments; }
		}

		/// <summary>
		/// Errors that happened during compilation of the template.
		/// </summary>
		public System.CodeDom.Compiler.CompilerErrorCollection Errors
		{
			get { return this.errors; }
		}

		#endregion

		#region ITextTemplatingEngineHost Members
		/// <summary>
		/// Resolves the full path of the assembly and combines it with the assembly reference.
		/// </summary>
		public string ResolveAssemblyReference(string assemblyReference)
		{
			string path = Path.Combine(this.binPath, assemblyReference);

			// Check if the DLL exists. Otherwise, override the binPath and let it use the 
			// current AppDomain information.
			if (File.Exists(path))
			{
				return path;
			}
			// No resolution at all.
			if (File.Exists(assemblyReference))
			{
				return assemblyReference;
			}

			// Probe public and private asms folders.
			path = Path.Combine(
				Path.Combine(
					AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "PublicAssemblies"),
				assemblyReference);
			if (File.Exists(path))
			{
				return path;
			}

			path = Path.Combine(
				Path.Combine(
					AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "PrivateAssemblies"),
				assemblyReference);
			if (File.Exists(path))
			{
				return path;
			}

			if (String.IsNullOrEmpty(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath) == false)
			{
				// Look in private probe paths.
				string[] privateBins = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string privateBin in privateBins)
				{
					path = Path.Combine(
						Path.Combine(
							AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
							privateBin),
						assemblyReference);

					if (File.Exists(path))
					{
						return path;
					}
				}
			}
            //TODO:Fabio;2017.06.03 Remove check id it's works
            // Search for reference assemblies of the currently targeted .NET Framework verion.
            // These are stored in "C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v<version>".
            //var targetFramework = new FrameworkName(AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName);
            //var referenceAssembliesBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Reference Assemblies\Microsoft\Framework\.NETFramework");
            //var referenceAssembliesPath = Path.Combine(referenceAssembliesBasePath, "v" + targetFramework.Version.ToString());
            //var referenceAssemblyPath = Path.Combine(referenceAssembliesPath, assemblyReference);
            //if (File.Exists(referenceAssemblyPath))
            //{
            //    return referenceAssemblyPath;
            //}
            //TODO:Fabio;2017.06.03 Remove check id it's works

            // Will fail at template compilation time.
            return assemblyReference;
		}

		/// <summary>
		/// Retrieves the list of references added by default when compiling the template.
		/// </summary>
		public IList<string> StandardAssemblyReferences
		{
			get { return null; }
		}

		/// <summary>
		/// Standard imports (using in C#) for the template to use.
		/// </summary>
		public IList<string> StandardImports
		{
			get { return null; }
		}

        private string templateFileName;

        /// <summary>
        /// Get or sets the template filename.
        /// </summary>
        public string TemplateFile
        {
            get { return templateFileName; }
            set { templateFileName = value; }
        }

		/// <summary>
		/// Resolves the name of the supported processor, which is named <c>PropertyProcessor</c>.
		/// </summary>
		/// <param name="processorName"></param>
		/// <returns></returns>
		public Type ResolveDirectiveProcessor(string processorName)
		{
			if (processorName == null)
			{
				throw new ArgumentNullException("processorName");
			}
			if (string.Compare(processorName, propertyDirectiveProcessorName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return typeof(PropertiesDirectiveProcessor);
			}
			return null;
		}

		/// <summary>
		/// Resolves the path of the file if it's relative, based on the templates path received.
		/// </summary>
		public string ResolveFileName(string fileName)
		{
			if (Path.IsPathRooted(fileName) == false)
			{
				return Path.Combine(this.binPath, fileName);
			}
			else
			{
				return fileName;
			}
		}

		/// <summary>
		/// Throws <see cref="NotImplementedException"/> as this feature is not implemented by the host.
		/// </summary>
		/// <exception cref="NotImplementedException">The feature is not implemented.</exception>
		public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
		{
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		/// <summary>
		/// Creates and return a new AppDomain to run the generated transformation class.
		/// </summary>
		public AppDomain ProvideTemplatingAppDomain(string content)
		{
			return AppDomain.CurrentDomain;
		}

		/// <summary>
		/// Logs the errors passed from the engine.
		/// </summary>
		public void LogErrors(System.CodeDom.Compiler.CompilerErrorCollection errors)
		{
			this.errors = errors;
		}

		/// <summary>
		/// Not used by this host.
		/// </summary>
		public void SetFileExtension(string extension)
		{
		}

        /// <summary>
        /// Opens the specified file and returns his content.
        /// </summary>
        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
              if (requestFileName == null)
              {
                  throw new ArgumentNullException("filePath");
              }

              location = ResolveFileName(requestFileName);
              if (location.StartsWith(binPath) == false)
              {
                  throw new ArgumentException(String.Format(
                      CultureInfo.CurrentCulture,
                      Properties.Resources.TemplateHost_IncludeTemplateNotInPackage,
                      location, this.binPath));
              }
              content = File.ReadAllText(location);

              return true;
        }

        /// <summary>
        /// Not used by this host.
        /// </summary>
        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not used by this host.
        /// </summary>
        public object GetHostOption(string optionName)
        {
            return null;
        }

        /// <summary>
        /// Not used by this host.
        /// </summary>
        public string ResolvePath(string path)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
