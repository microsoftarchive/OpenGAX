using System;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.RecipeFramework.Library.Tests
{
	[TestClass()]
	public class Helper
	{
		private const int targetIsADirectory = 1;

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, int dwFlags);

		public static void CreateDirectoryLink(string linkPath, string targetPath)
		{
			if (!CreateSymbolicLink(linkPath, targetPath, targetIsADirectory) || Marshal.GetLastWin32Error() != 0)
			{
				Int32 hr = Marshal.GetHRForLastWin32Error();
				unchecked
				{
					if (hr == (Int32)0x800700b7) // already exist.
						return;
				}

				try
				{
					Marshal.ThrowExceptionForHR(hr);
				}
				catch (COMException exception)
				{
					throw new IOException(exception.Message, exception);
				}
			}
		}

		public enum VisualStudioVersion { Vs2013 = 120, Vs2015= 140, Vs2017 = 150};

		internal static string GetVisualStudioInstallationDir(VisualStudioVersion version)
		{
			string registryKeyString = String.Format(@"SOFTWARE{0}Microsoft\VisualStudio\{1}", 
				Environment.Is64BitOperatingSystem ? @"\Wow6432Node\" : "", GetVersionNumber(version));

			using (RegistryKey localMachineKey = Registry.LocalMachine.OpenSubKey(registryKeyString))
			{
				if (localMachineKey != null) return localMachineKey.GetValue("InstallDir") as string;
				else return "";
			}
		}

		private static string GetVersionNumber(VisualStudioVersion version)
		{
			return string.Format("{0}.0", Convert.ToInt32(version) / 10);
		}

		/// <summary>
		/// create a link to visual studio ide private assembly path in appbase.
		/// you need to run ide as admin for this to work.
		/// </summary>
		[AssemblyInitialize()]
		public static void PrepareIdeAssemblies(TestContext context)
		{
			// dte, when running outside visual studio, need to link to many private assemblies,
			// can be done either by installing them to global assembly cache, or through assembly probing mechanism,
			// see the app.config file. The second approach is easier to do, just create a link in app base dir to vs private assembly path.

			string ideAssemblyPath = GetVisualStudioInstallationDir(VisualStudioVersion.Vs2015) + "PrivateAssemblies";

			string appBase = AppDomain.CurrentDomain.BaseDirectory;

			CreateDirectoryLink(appBase + "\\private", ideAssemblyPath);
		}
	}
}
