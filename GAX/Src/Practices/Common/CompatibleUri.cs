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

namespace Microsoft.Practices.Common
{
    /// <summary>
    /// This class provides unified behavior for Uri.LocalPath under WinXP and Vista
    /// </summary>
    public sealed class CompatibleUri : Uri
    {
        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="uriString"></param>
        public CompatibleUri(String uriString) : base(uriString) { }

        /// <summary>
        /// Under Vista
        /// </summary>
        new public String LocalPath
        {
            get
            {
                String localPath = base.LocalPath;
                return localPath.Replace(@"\\", @"\");
            }
        }
    }
}
