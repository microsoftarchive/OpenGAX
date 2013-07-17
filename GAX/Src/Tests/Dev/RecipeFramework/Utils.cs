#region Using directives

using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
	public class Utils
	{
		public static string MakeTestRelativePath(string file)
		{
			return Path.Combine(
				Path.GetDirectoryName(new Uri(typeof(Utils).Assembly.CodeBase).LocalPath),file);
		}

		public static string GetClrInstallationDirectory()
		{
			int MAX_PATH = 260;
			StringBuilder sb = new StringBuilder(MAX_PATH);
			GetCORSystemDirectory(sb, MAX_PATH, ref MAX_PATH);
			return sb.ToString();
		}

		[DllImport("mscoree.dll")]
		private static extern int GetCORSystemDirectory(
			[MarshalAs(UnmanagedType.LPWStr)]StringBuilder pbuffer,
			int cchBuffer, ref int dwlength);
	}
}
