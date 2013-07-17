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
using System.Globalization;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Registration
{
	internal sealed class ProvideVsTemplateInfoAttribute : Registration.GuidancePackageRegistrationAttribute
	{
		public ProvideVsTemplateInfoAttribute()
			: base(true)
		{
		}

		private string TemplatesRoot
		{
			get
			{
				return String.Format(CultureInfo.CurrentCulture, @"Packages\{0}\Templates", typeof(RecipeManagerPackage).GUID.ToString("B"));
			}
		}

		protected override void Register()
		{
			using (Context.CreateKey(this.TemplatesRoot)) ;
		}

		protected override void Unregister()
		{
			Context.RemoveKey(this.TemplatesRoot);
		}

	}
}
