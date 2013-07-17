//===================================================================================
// Microsoft patterns & practices
// Guidance Automation Toolkit
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
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.ValueProviders
{
    public class CurrentVsHiveProvider : ValueProvider
    {
        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
            return GetValue(out newValue);
        }

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            return GetValue(out newValue);
        }

        private bool GetValue(out object newValue)
        {
            string regRoot = "8.0";
            ILocalRegistry3 registryService = GetService<SLocalRegistry>() as ILocalRegistry3;
            if (registryService != null)
            {
                registryService.GetLocalRegistryRoot(out regRoot);
            }

            newValue = regRoot;
            return true;
        }
    }
}
