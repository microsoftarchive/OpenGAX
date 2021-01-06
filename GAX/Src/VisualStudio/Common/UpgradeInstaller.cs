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

using System.Configuration.Install;
using Microsoft.Win32;
using Microsoft.Practices.ComponentModel;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Common
{
	/// <summary>
	/// 
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
    [System.ComponentModel.RunInstaller(true)]
    public class UpgradeInstaller : Installer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);

			TraceUtil.GaxTraceSource.TraceInformation("Starting set up upgrade setup...");
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{EDB3B1C1-ADC6-4263-AE1D-8D8401C88236}", true))
            {
                if (registryKey != null)
                {
					TraceUtil.GaxTraceSource.TraceInformation("Removing NoModify value.");
                    registryKey.DeleteValue("NoModify", false);
                }
            }
			TraceUtil.GaxTraceSource.TraceInformation("Ending set up upgrade setup...");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedState"></param>
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedState"></param>
        public override void Commit(System.Collections.IDictionary savedState)
        {
            base.Commit(savedState);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedState"></param>
        public override void Rollback(System.Collections.IDictionary savedState)
        {
            base.Rollback(savedState);
        }
        
    }
}
