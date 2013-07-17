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
using Microsoft.VisualStudio.Shell;
using System.Globalization;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Registration
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	internal sealed class ProvidePseudoFolderAttribute: Registration.GuidancePackageRegistrationAttribute 
	{
		Guid folderGuid;
		string name;
		int sortPriority;

		public ProvidePseudoFolderAttribute(Type folderType, string name, int sortPriority)
			:base(true)
		{
			this.folderGuid = folderType.GUID;
			this.name = name;
			this.sortPriority = sortPriority;
		}

		private string PseudoFolderRegKey
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture, @"NewProjectTemplates\PseudoFolders\{0}", folderGuid.ToString("B"));
			}
		}
		protected override void Register()
		{
			using (RegistrationAttribute.Key key = Context.CreateKey(this.PseudoFolderRegKey))
			{
				key.SetValue(string.Empty, name);
				key.SetValue("DisplayName", name);
				key.SetValue("Package", typeof(RecipeManagerPackage).GUID.ToString("B"));
				key.SetValue("SortPriority", sortPriority);
			}
		}

		protected override void Unregister()
		{
			Context.RemoveKey(this.PseudoFolderRegKey);
		}
	}
}
