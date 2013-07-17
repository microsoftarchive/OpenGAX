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

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ErrorDialog", Scope = "member", Target = "Microsoft.Practices.Common.ErrorForm.#InitializeComponent()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Windows.Forms.Control.set_Text(System.String)", Scope = "member", Target = "Microsoft.Practices.Common.ErrorForm.#InitializeComponent()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Scope = "member", Target = "Microsoft.Practices.Common.ErrorHelper.#ShowInternal(System.Windows.Forms.Design.IUIService,System.Exception,System.String,System.String,System.Windows.Forms.MessageBoxButtons)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Microsoft.Practices.Common.Services.TypeResolutionService.#GetType(System.String,System.Boolean,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Scope = "member", Target = "Microsoft.Practices.Common.CompatibleUri.#.ctor(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Scope = "member", Target = "Microsoft.Practices.Common.Services.ValueInfo.#Type")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Scope = "member", Target = "Microsoft.Practices.Common.CompatibleUri.#.ctor(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Scope = "member", Target = "Microsoft.Practices.Common.ErrorHelper.#Ask(System.String,System.String,System.String,System.String,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadWithPartialName", Scope = "member", Target = "Microsoft.Practices.Common.ReflectionHelper.#LoadType(System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom", Scope = "member", Target = "Microsoft.Practices.Common.Services.TypeResolutionService.#GetAssembly(System.Reflection.AssemblyName,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope = "member", Target = "Microsoft.Practices.Common.Services.AliasResolutionService.#.ctor(System.Collections.IDictionary,System.ComponentModel.Design.ITypeResolutionService)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Scope = "member", Target = "Microsoft.Practices.Common.ReflectionHelper.#GetAssemblyPath(System.Reflection.Assembly)")]
