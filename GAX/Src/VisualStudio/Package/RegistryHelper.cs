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
using Microsoft.Win32;
using System.IO;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Microsoft.Practices.ComponentModel;

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
	/// <summary>
	/// Wrapper to the new ILocalRegistry4 which comes in Shell 9.0
	/// </summary>
	[ComImport, InterfaceType((short)1), Guid("5C45B909-E820-4ACC-B894-0A013C6DA212")]
	public interface IGaxLocalRegistry
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="rclsid"></param>
		/// <param name="pdwCookie"></param>
		/// <returns></returns>
		[PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int RegisterClassObject([In, ComAliasName("Microsoft.VisualStudio.OLE.Interop.REFCLSID")] ref Guid rclsid, [ComAliasName("Microsoft.VisualStudio.OLE.Interop.DWORD")] out uint pdwCookie);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dwCookie"></param>
		/// <returns></returns>
		[PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int RevokeClassObject([ComAliasName("Microsoft.VisualStudio.OLE.Interop.DWORD")] uint dwCookie);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="riid"></param>
		/// <returns></returns>
		[PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int RegisterInterface([In, ComAliasName("Microsoft.VisualStudio.OLE.Interop.REFIID")] ref Guid riid);
		/// <summary>
		/// 
		/// </summary>
		[PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int GetLocalRegistryRootEx([In, ComAliasName("Microsoft.VisualStudio.Shell.Interop.VSLOCALREGISTRYTYPE")] uint dwRegType, [ComAliasName("Microsoft.VisualStudio.Shell.Interop.VSLOCALREGISTRYROOTHANDLE")] out uint pdwRegRootHandle, [MarshalAs(UnmanagedType.BStr)] out string pbstrRoot);
	}

	internal enum VsLocalRegistryRootHandle
	{
		RegHandle_CurrentUser = -2147483647,
		RegHandle_Invalid = 0,
		RegHandle_LocalMachine = -2147483646
	}

	internal enum VsLocalRegistryType
	{
		RegType_Configuration = 2,
		RegType_NewUserSettings = 5,
		RegType_PrivateConfig = 3,
		RegType_SessionSettings = 4,
		RegType_UserSettings = 1
	}

    internal static class RegistryHelper
    {
        internal const String VS_EXPHIVE_GAXPACKAGE_KEY = @"{77d93a80-73fc-40f8-87db-acd3482964b2}";
        internal const String VISUALSTUDIO_HIVE = @"Software\Microsoft\VisualStudio\";
        internal const string AUTOLOAD_SOLUTION_KEY = @"AutoLoadPackages\{ADFC4E64-0397-11D1-9F4E-00A0C911004F}";
        internal const string AUTOLOAD_PROJECT_KEY = @"AutoLoadPackages\{F1536EF8-92EC-443C-9ED7-FDADF150DA82}";


		internal const string VisualStudioRoot = @"Software\Microsoft\VisualStudio\";

		internal static RegistryKey GetCurrentVsRegistryKey(bool writeable)
		{
			IGaxLocalRegistry localRegistry = Package.GetGlobalService(typeof(SLocalRegistry)) as IGaxLocalRegistry;

			if (localRegistry != null)
			{
				uint rootHandle;
				string rootPath;
				if (ErrorHandler.Succeeded(localRegistry.GetLocalRegistryRootEx((uint)VsLocalRegistryType.RegType_Configuration, out rootHandle, out rootPath)))
				{
					// Check if we have valid data.
					VsLocalRegistryRootHandle handle = (VsLocalRegistryRootHandle)rootHandle;
					if (!string.IsNullOrEmpty(rootPath) && (VsLocalRegistryRootHandle.RegHandle_Invalid != handle))
					{
						// Check if the root is inside HKLM or HKCU. Note that this does not depends only from
						// the registry type, but also from instance-specific data like the RANU flag.
						RegistryKey root = (VsLocalRegistryRootHandle.RegHandle_LocalMachine == handle) ? Registry.LocalMachine : Registry.CurrentUser;
						return root.OpenSubKey(rootPath, writeable);
					}
				}
			}
			else
			{
                return Registry.CurrentUser.OpenSubKey(GetCurrentVsRootHive());
			}

			return null;
		}

		internal static string GetCurrentVsRootHive()
		{
			string hive = RecipeManager.GetCurrentVsHive();

			if (string.IsNullOrEmpty(hive))
			{
				DefaultRegistryRootAttribute rootAttrib = (DefaultRegistryRootAttribute)
								Attribute.GetCustomAttribute(
									typeof(RecipeManagerPackage),
									typeof(DefaultRegistryRootAttribute));
				if (rootAttrib == null)
				{
					throw new InvalidOperationException(Properties.Resources.Missing_RegRoot);
				}

				return rootAttrib.Root;
			}

			return RegistryHelper.VisualStudioRoot + hive;
		}


        internal static void DeleteAllRegistryValues(string valueName, string root, RegistryKey parent)
        {
            using (parent)
            {
                RegistryKey rootKey = parent.OpenSubKey(root, true);
                if (rootKey != null)
                {
					TraceUtil.GaxTraceSource.TraceInformation("Removing Registry value Root: {0} ValueName: {1}.", root, valueName);
                    rootKey.DeleteValue(valueName, false);
                }
            }
        }

        internal static void DeleteAllRegistryKeys(string keyName, string root, bool recursive, RegistryKey parent)
        {
            using (parent)
            {
                using (RegistryKey rootKey = parent.OpenSubKey(root, true))
                {
                    if (rootKey != null)
                    {
                        RegistryKey subKeyToBeDeleted = rootKey.OpenSubKey(keyName);

                        if (subKeyToBeDeleted != null)
                        {
							TraceUtil.GaxTraceSource.TraceInformation("Removing SubKey Registry Tree Root:  {0} - Key: {1}", root, keyName);
                            rootKey.DeleteSubKeyTree(keyName);
                        }

                        if (recursive)
                        {
                            foreach (string subKeyName in rootKey.GetSubKeyNames())
                            {
                                DeleteAllRegistryKeys(keyName, Path.Combine(root, subKeyName), recursive, parent);
                            }
                        }
                    }
                }
            }
        }
    }
}
