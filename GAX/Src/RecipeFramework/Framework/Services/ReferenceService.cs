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
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework;
using Configuration = Microsoft.Practices.RecipeFramework.Configuration;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.ComponentModel;
using System.ComponentModel;
using System.Collections;

namespace Microsoft.Practices.RecipeFramework.Services
{
	[ServiceDependency(typeof(IDictionaryService))]
	[ServiceDependency(typeof(IConfigurationService))]
	internal class ReferenceService: SitedComponent, IReferenceService
	{
		public ReferenceService()
		{
		}

		#region IReferenceService members

		IComponent IReferenceService.GetComponent(object reference)
		{
			object guidancePackage = GetService(typeof(IConfigurationService));
			if (guidancePackage != null && guidancePackage is IComponent)
			{
				return (IComponent)guidancePackage;
			}
			return null;
		}

		string IReferenceService.GetName(object reference)
		{
			IDictionaryService dictionaryService = 
				(IDictionaryService)GetService(typeof(IDictionaryService));
			object value = dictionaryService.GetKey(reference);
			if (value != null && value is string)
			{
				return (string)value;
			}
			return "";
		}

		object IReferenceService.GetReference(string name)
		{
			IDictionaryService dictionaryService =
				(IDictionaryService)GetService(typeof(IDictionaryService));
			if (dictionaryService != null)
			{
				object value = dictionaryService.GetValue(name);
				return value;
			}
			return null;
		}

		object[] IReferenceService.GetReferences()
		{
			return ((IReferenceService)this).GetReferences(null);
		}

		object[] IReferenceService.GetReferences(Type baseType)
		{
			IConfigurationService configService =
				(IConfigurationService)GetService(typeof(IConfigurationService));
			if (configService == null || configService.CurrentRecipe == null)
			{
				return new object[] { };
			}
			Configuration.Argument[] arguments = configService.CurrentRecipe.Arguments;
			IDictionaryService dictionaryService =
				(IDictionaryService)GetService(typeof(IDictionaryService));
			if (dictionaryService == null || arguments == null)
			{
				return new object[] { };
			}
			ArrayList references = new ArrayList();
			foreach (Configuration.Argument arg in arguments)
			{
				object value = dictionaryService.GetValue(arg.Name);
				if (value != null )
				{
					if (baseType == null || (baseType.IsAssignableFrom(value.GetType())))
					{
						references.Add(value);
					}
				}
			}
			return references.ToArray();
		}

		#endregion
	}
}
