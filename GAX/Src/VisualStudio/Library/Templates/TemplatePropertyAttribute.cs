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
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates
{
	/// <summary>
	/// Marks a property as generated from a template "property" directive.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=false)]
	public class TemplatePropertyAttribute : Attribute
	{
	}
}
