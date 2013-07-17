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
    internal sealed class ProvidePackageSetting : Registration.GuidancePackageRegistrationAttribute 
    {
        string name;
        object value;

        public ProvidePackageSetting(string name, object value)
            : base(true)
        {
            this.name = name;
            this.value = value;
        }

        private string PackageKey
        {
            get
            {
                return GetRegistryKey(this.Context.ComponentType);
            }
        }

        internal static string GetRegistryKey(Type componentType)
        {
            return string.Format(CultureInfo.InvariantCulture, @"Packages\{0}", componentType.GUID.ToString("B")); 
        }

        protected override void Register()
        {
            using (RegistrationAttribute.Key key = Context.CreateKey(this.PackageKey))
            {
                key.SetValue(name, value);
            }
        }

        protected override void Unregister()
        {
            Context.RemoveValue(this.PackageKey, name);
        }
    }
}
