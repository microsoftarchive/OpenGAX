<#@ template language="C#" #>
<#@ property processor="PropertyProcessor" name="TargetNamespace" #>
<#@ property processor="PropertyProcessor" name="Iterations" #>
<#@ assembly name="System.dll" #>
#region info
//===============================================================================
// Microsoft VS Integrated Guidance Package
//
// This file contains the implementation of the $safeitemrootname$ class
// This file was generated automatically by a tool.
//
//===============================================================================
// Copyright (C) 2003-2004 Microsoft Corporation
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//==============================================================================
#endregion info

using System;
using System.Text;

namespace <#= this.TargetNamespace #>
{
	/// <summary>
	/// The code in this class uses a for loop to generate multiple lines of code.
	/// </summary>
	public class $safeitemrootname$
	{
		public void Repeat()
		{
<# int count = this.Iterations; //note that the property is emited strong-typed
for (int i = 0; i < count; i++) { #>
			Console.WriteLine("Hello iteration: " + <#= i #>);
<# } #>
		}
	}
}
