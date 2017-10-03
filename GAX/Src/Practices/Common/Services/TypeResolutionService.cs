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
using System.Collections;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using Microsoft.Practices.Common;

#endregion Using directives

namespace Microsoft.Practices.Common.Services
{
    /// <summary>
    /// Provides type resolution and loading based on a base path.
    /// </summary>
    public class TypeResolutionService : ITypeResolutionService
    {
        #region Fields & Ctor

        ITypeResolutionService parentService;
        Hashtable assemblyCache = new Hashtable();

        /// <summary>
        /// Constructs the resolution service using a path as the 
        /// base location to probe for assemblies.
        /// </summary>
        /// <param name="basePath">Base location to probe for assemblies.</param>
        public TypeResolutionService(string basePath)
        {
            if (basePath == null)
            {
                throw new ArgumentNullException("basePath");
            }
            if (basePath.Length == 0)
            {
                throw new ArgumentException(Properties.Resources.General_ArgumentEmpty, "basePath");
            }
            // Make the path absolute if it's not already.
            this.basePath = new DirectoryInfo(basePath).FullName;
            // Access to the target path upon loading the assembly is already ensured by LoadFrom.
            // See: http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfsystemreflectionassemblyclassloadfromtopic2.asp?frame=true&hidetoc=true
        }

        /// <summary>
        /// Constructs the resolution service using a path as the 
        /// base location to probe for assemblies and a parent service to forward requests to.
        /// </summary>
        /// <param name="basePath">Base location to probe for assemblies.</param>
        /// <param name="parentService">Optional service to forward requests that can't be satisfied by this instance.</param>
        public TypeResolutionService(string basePath, ITypeResolutionService parentService)
            : this(basePath)
        {
            this.parentService = parentService;
        }

        #endregion Fields & Ctors

        private string basePath;

        /// <summary>
        /// Gets the base location of the service used for probing.
        /// </summary>
        public string BasePath
        {
            get { return basePath; }
        }

        #region ITypeResolutionService Members

        #region Dummy overloads

        /// <summary>
        /// See <see cref="ITypeResolutionService.GetType(string)"/>.
        /// </summary>
        public Type GetType(string name)
        {
            return GetType(name, false, false);
        }

        /// <summary>
        /// See <see cref="ITypeResolutionService.GetType(string, bool)"/>.
        /// </summary>
        public Type GetType(string name, bool throwOnError)
        {
            return GetType(name, throwOnError, false);
        }

        /// <summary>
        /// See <see cref="ITypeResolutionService.GetAssembly(AssemblyName)"/>.
        /// </summary>
        public Assembly GetAssembly(AssemblyName name)
        {
            return GetAssembly(name, false);
        }

        #endregion Dummy overloads

        #region Implementations

        /// <summary>
        /// See <see cref="ITypeResolutionService.GetType(string, bool, bool)"/>.
        /// </summary>
        public virtual Type GetType(string name, bool throwOnError, bool ignoreCase)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            Type type = null;
            Exception loadex = null;

            if (name != null && name.Length > 0)
            {
                // Method 1 (full class name in the GAC)
                try
                {
                    type = Type.GetType(name.Trim(), throwOnError, false);
                }
                catch (Exception ex)
                {
                    // Store to rethrow. If we get another exception below (i.e. from 
                    // the GetAssembly method, we'll replace it with that one, which is 
                    // more specific and relevant.
                    loadex = ex;
                }

                if (type == null)
                {
                    // Method 2, ask for the assembly and type from custom logic.
                    string assemblyname = ReflectionHelper.GetAssemblyString(name);
                    Assembly asm = null;
                    try
                    {
                        asm = GetAssembly(ReflectionHelper.ParseAssemblyName(assemblyname), throwOnError);
                    }
                    catch (Exception ex)
                    {
                        loadex = ex;
                    }

                    string[] parts = name.Split(',');

                    if (asm != null)
                    {
                        // If we have the assembly, but we don't succeed to get a valid type, 
                        // there may be a dependency loading problem, and we need to know that.
                        // We cannot fallback to try loading from the parent service in this case 
                        // as we already found the assembly.
                        type = asm.GetType(parts[0].Trim(), throwOnError, false);
                    }
                }
            }

            if (type == null && parentService != null)
            {
                // Method 3, ask parent service. At this point we can let it throw, as there's
                // no need to specialize the returned exception message further.
                type = parentService.GetType(name, throwOnError, ignoreCase);
            }

            if (type == null && throwOnError)
            {
                throw new TypeLoadException(name, loadex);
            }

            return type;
        }

        /// <summary>
        /// See <see cref="ITypeResolutionService.GetAssembly(AssemblyName, bool)"/>.
        /// </summary>
        public virtual Assembly GetAssembly(AssemblyName name, bool throwOnError)
        {
            
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Name.Length == 0)
            {
                throw new ArgumentException(Properties.Resources.General_ArgumentEmpty, "name");
            }

            // Method 1: stored in our cache.
            Assembly asm = (Assembly)assemblyCache[name.FullName];
            if (asm != null)
            {
                return asm;
            }

            try
            {
                // Method 2: Assembly.Load, regular CLR probing.
                asm = Assembly.Load(name);
            }
            catch (FileNotFoundException)
            {
                // Method 3: Assembly.LoadFrom relative to our base path.
                string asmpath = Path.Combine(basePath, name.Name + ".dll");
                if (File.Exists(asmpath))
                {
                    // We know the file exists. Dependencies will automatically be resolved 
                    // relative to the LoadFrom location. 
                    // See http://blogs.msdn.com/suzcook/archive/2003/05/29/57143.aspx
                    asm = Assembly.LoadFrom(asmpath);
                }
                // Method 2b: Assembly.LoadWithPartialName.
                if (asm == null)
                {
                    try
                    {
                        asm = Assembly.LoadWithPartialName(name.Name);
                    }
                    catch (BadImageFormatException)
                    {
                    }
                }
            }

            if (asm == null && parentService != null)
            {
                // Method 5, ask parent service. At this point we can let it throw, as there's
                // no need to specialize the returned exception message further.
                asm = parentService.GetAssembly(name, throwOnError);
            }

            if (asm == null && throwOnError)
            {
                throw new FileNotFoundException(String.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.TypeResolutionService_AssemblyNotFound,
                    name.FullName));
            }

            // Cache it for faster retrieval next time.
            if (asm != null)
            {
                //Cache using the assemblyName we received to the method.
                if (!assemblyCache.ContainsKey(name.FullName))
                {
                    assemblyCache.Add(name.FullName, asm);
                }
            }

            return asm;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public virtual void ReferenceAssembly(AssemblyName name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// See <see cref="ITypeResolutionService.GetPathOfAssembly"/>.
        /// </summary>
        /// <remarks>
        /// Returns either the local file name of the assembly or its 
        /// <see cref="Assembly.CodeBase"/> if it doesn't point to a file name.
        /// </remarks>
        public virtual string GetPathOfAssembly(AssemblyName name)
        {
            Assembly asm = GetAssembly(name);
            if (asm == null)
            {
                return null;
            }

            Uri location = new CompatibleUri(asm.CodeBase);
            if (location.IsFile)
            {
                return location.LocalPath;
            }
            else
            {
                return asm.CodeBase;
            }
        }

        #endregion Implementations

        #endregion ITypeResolutionService Members
    }
}