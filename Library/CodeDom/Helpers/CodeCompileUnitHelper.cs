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
using System.CodeDom;

namespace Microsoft.Practices.RecipeFramework.Library.CodeDom.Helpers
{
    internal static class CodeCompileUnitHelper
    {
        public static CodeTypeDeclaration FindTypeDeclarationByName(CodeNamespace codeNamespace, string typeName)
        {
            CodeTypeDeclaration response = null;

            foreach(CodeTypeDeclaration type in codeNamespace.Types)
            {
                if(type.Name.Equals(typeName))
                {
                    response = type;
                    break;
                }
            }

            return response;
        }
    }
}
