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
using System.Reflection;
using System.ComponentModel.Design;

#endregion

namespace Microsoft.Practices.Common.Services
{
	/// <summary>
	/// Specialized <see cref="ITypeResolutionService"/> that lazily loads 
	/// types based on their aliases, and resolves this mapping.
	/// </summary>
	public class AliasResolutionService : TypeResolutionService
	{
		#region Fields and Constructor

		IDictionary typeAliases;
		ITypeResolutionService parentLoader;

		/// <summary>
		/// Creates an instance of the specialized resolution service.
		/// </summary>
		/// <param name="aliases">The list of aliases this loader resolves, or <see langword="null"/>.</param>
		/// <param name="parentLoader">The parent loader that will resolve the aliased types.</param>
		public AliasResolutionService(IDictionary aliases, ITypeResolutionService parentLoader) :
            base(parentLoader is TypeResolutionService ? ((TypeResolutionService)parentLoader).BasePath : 
            new CompatibleUri(typeof(AliasResolutionService).Assembly.CodeBase).LocalPath)
		{
			if (aliases == null)
			{
				typeAliases = new System.Collections.Specialized.HybridDictionary();
			}
			if (parentLoader == null)
			{
				throw new ArgumentNullException("parentLoader");
			}
			this.typeAliases = aliases;
			this.parentLoader = parentLoader;
		}

		#endregion Fields and Constructor

		#region TypeResolutionService Overrides

		/// <summary>
		/// See <see cref="ITypeResolutionService.GetType(string, bool, bool)"/>.
		/// </summary>
		public override Type GetType(string name, bool throwOnError, bool ignoreCase)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			object value = typeAliases[name];
			if (value != null)
			{
                string stringValue = value as string;
				if (stringValue != null)
				{
					// In this case, we haven't loaded the type yet. Ask the parent loader.
					Type type = parentLoader.GetType(stringValue, throwOnError, ignoreCase);
					if (type != null)
					{
						typeAliases[name] = type;
					}
					return type;
				}
				else
				{
					// We already have the type.
					return (Type)value;
				}
			}
			else
			{
				return parentLoader.GetType(name, throwOnError, ignoreCase);
			}
		}

		/// <summary>
		/// See <see cref="ITypeResolutionService.GetAssembly(AssemblyName, bool)"/>.
		/// </summary>
		public override Assembly GetAssembly(AssemblyName name, bool throwOnError)
		{
			return parentLoader.GetAssembly(name, throwOnError);
		}

		/// <summary>
		/// See <see cref="ITypeResolutionService.ReferenceAssembly"/>.
		/// </summary>
		public override void ReferenceAssembly(AssemblyName name)
		{
			parentLoader.ReferenceAssembly(name);
		}

		/// <summary>
		/// See <see cref="ITypeResolutionService.GetPathOfAssembly"/>.
		/// </summary>
		public override string GetPathOfAssembly(AssemblyName name)
		{
			return parentLoader.GetPathOfAssembly(name);
		}

		#endregion ITypeResolutionService Members
	}
}
