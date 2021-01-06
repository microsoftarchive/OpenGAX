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
using System.IO;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Microsoft.Practices.Common;
using Microsoft.Build.Framework;
using Microsoft.Practices.RecipeFramework.VisualStudio.CTC;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.VisualStudio.Common;
using System.Collections.Generic;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Build
{
    /// <summary>
    /// Generates a dll with the VS commands
    /// </summary>
    public class GenerateMenuResource : Microsoft.Build.Utilities.Task
	{
		short resourceId = 1000;

		/// <summary>
		/// Gets or sets the template files (*.vstemplate) of the guidance package
		/// </summary>
		[Required]
		public ITaskItem[] Templates { get; set; }

		/// <summary>
		/// Gets or sets the output path of the current project
		/// </summary>
		[Required]
		public string OutputPath { get; set; }

		/// <summary>
		/// Gets or sets the guidance package configuration file
		/// </summary>
		[Required]
		public ITaskItem ConfigurationFile { get; set; }

		/// <summary>
		/// The output command dll file
		/// </summary>
		[Required]
		public string OutputFile { get; set; }

		private Configuration.GuidancePackage configuration;
		private Configuration.GuidancePackage Configuration
		{
			get
			{
				if (this.configuration == null)
					this.configuration = GuidancePackage.ReadConfiguration(this.ConfigurationFile.ItemSpec);

				return this.configuration;
			}
		}

		private string ComponentPath
		{
			get
			{
				string s= Path.GetDirectoryName(this.OutputFile);
                if (s.EndsWith(@"\1033"))
                    return s.Substring(0, s.Length - 5);
                else
                    return s;
			}
		}

		/// <summary>
		/// Executes the task
		/// </summary>
		/// <returns></returns>
		public override bool Execute()
		{
			string tempCTO = null;
			CTCFile ctcFile = null;
			try
			{
				ctcFile = CompileCTO(out tempCTO);
				if (ctcFile != null)
				{
					var satelliteDllFileName = this.OutputFile;
					var gatSatelliteDllFileName = GetGatSatelliteDllFileName();

					if (File.Exists(satelliteDllFileName))
					{
						File.Delete(satelliteDllFileName);
					}
					File.Copy(gatSatelliteDllFileName, satelliteDllFileName);
                    File.SetAttributes(satelliteDllFileName, FileAttributes.Normal);
					IntPtr hUpdate = NativeMethods.BeginUpdateResource(satelliteDllFileName, 1);
					try
					{
						if (hUpdate.Equals(IntPtr.Zero))
						{
							Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
						}
						AddCTMENU(hUpdate, tempCTO);
						AddBITMAP(hUpdate, ctcFile);

						return true;
					}
					finally
					{
						if (!hUpdate.Equals(IntPtr.Zero))
						{
							NativeMethods.EndUpdateResource(hUpdate, 0);
							hUpdate = IntPtr.Zero;
						}
					}
				}
			}
			finally
			{
				ctcFile = null;
				if (tempCTO != null && File.Exists(tempCTO))
				{
#if !DEBUG
                    File.Delete(tempCTO);
#endif
				}
			}
			return false;
		}

		private CTCFile CompileCTO(out string tempCTO)
		{
			string tempCTC = null;
			string tempCPP = null;
			string tempCTCEXE = null;
			try
			{
				var templates = new List<IVsTemplate>();
                var templatelist = this.Templates.ToList();     

                foreach (var vsTemplate in this.Templates)
				{
					var templateMetadata = new TemplateMetaData(
						Path.Combine(this.OutputPath, vsTemplate.ItemSpec),
						new CommandID(new Guid(this.Configuration.Guid), templatelist.IndexOf(vsTemplate) + 1),
						this.Configuration.Name,
						RegistryHelper.GetCurrentVsRegistryKey(false));

					templates.Add(templateMetadata);
				}

				CTCFile ctcFile = new CTCBuilder(this.Configuration, this.ComponentPath, null, templates).Create();
				tempCTC = Path.GetTempFileName();
				using (TextWriter textWriter = new StreamWriter(tempCTC))
				{
					new CTCGenerator(ctcFile, textWriter).Generate();
				}
				tempCPP = Path.GetTempFileName();
				File.Delete(tempCPP);
				tempCPP += ".bat";
				using (TextWriter textWriter = new StreamWriter(tempCPP))
				{
					textWriter.WriteLine("@echo off");
					textWriter.WriteLine("echo #line 1 %4");
					textWriter.WriteLine("type %4");
				}
				tempCTO = Path.GetTempFileName();

				tempCTCEXE = Path.GetTempFileName();
				File.Delete(tempCTCEXE);
				tempCTCEXE += ".exe";

				using (Stream ctcExeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
					ReflectionHelper.GetAssemblyName(Assembly.GetExecutingAssembly()) + ".CTC.CTC.exe"))
				{
					using (FileStream ctcExeFile = new FileStream(tempCTCEXE, FileMode.OpenOrCreate))
					{
						const int bufferSize = 0x1000; // 1Kb Buffer
						byte[] buffer = new byte[bufferSize];
						int readed = bufferSize;
						while (readed == bufferSize)
						{
							readed = ctcExeStream.Read(buffer, 0, bufferSize);
							ctcExeFile.Write(buffer, 0, readed);
						}
					}
				}
				Process ctcProcess = new Process();
				StringBuilder arguments = new StringBuilder();
				arguments.Append(" \"");
				arguments.Append(tempCTC);
				arguments.Append("\" \"");
				arguments.Append(tempCTO);
				arguments.Append("\" -s- \"-C");
				arguments.Append(tempCPP.ToString());
				arguments.Append("\"");
				ctcProcess.StartInfo.UseShellExecute = false;
				ctcProcess.StartInfo.RedirectStandardError = true;
				ctcProcess.StartInfo.RedirectStandardOutput = true;
				ctcProcess.StartInfo.Arguments = arguments.ToString();
				ctcProcess.StartInfo.FileName = tempCTCEXE;
				ctcProcess.StartInfo.WorkingDirectory = this.ComponentPath;
				if (ctcProcess.Start())
				{
					ctcProcess.WaitForExit();
					if (ctcProcess.ExitCode == 0 && File.Exists(tempCTO))
					{
						return ctcFile;
					}
					Debug.Fail("Cannot excute ctc.exe", String.Format(CultureInfo.InvariantCulture, "Invalid exit code {0}, arguments = {1}, stderror= {2}", ctcProcess.ExitCode, arguments.ToString(), ctcProcess.StandardError.ReadToEnd()));
				}
				else
				{
					Debug.Assert(true, "CTC.exe cannot be started");
				}
				return null;
			}
			finally
			{
				try
				{
					if (tempCTC != null && File.Exists(tempCTC))
					{
#if !DEBUG
                        File.Delete(tempCTC);
#endif
					}
					if (tempCPP != null && File.Exists(tempCPP))
					{
						File.Delete(tempCPP);
					}
					if (tempCTCEXE != null && File.Exists(tempCTCEXE))
					{
						File.Delete(tempCTCEXE);
					}
				}
				catch
				{
				}
			}
		}

		private string GetGatSatelliteDllFileName()
		{
			return Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), @"1033\Microsoft.Practices.RecipeFramework.VisualStudioUI.dll");
		}

		internal sealed class NativeMethods
		{
			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern IntPtr BeginUpdateResource(string pFileName, Int32 bDeleteExistingResources);

			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern Int32 EndUpdateResource(IntPtr hUpdate, Int32 fDiscard);

			[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
			public static extern Int32 UpdateResource(IntPtr hUpdate, string lpType, Int32 lpName, Int16 wLanguage, byte[] lpData, Int32 cbData);

			public const int BMPHeaderSize = 14;

			public enum RT
			{
				CURSOR = 1,
				BITMAP = 2,
				ICON = 3,
				MENU = 4,
				DIALOG = 5,
				STRING = 6,
				FONTDIR = 7,
				FONT = 8,
				ACCELERATOR = 9,
				RCDATA = 10,
				MESSAGETABLE = 11
			}

			[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
			public static extern Int32 UpdateResource(IntPtr hUpdate, RT lpType, Int32 lpName, Int16 wLanguage, byte[] lpData, Int32 cbData);

		}
		
		private void AddBITMAP(IntPtr hUpdate, CTCFile ctcFile)
		{
			if (ctcFile == null ||
				ctcFile.CMDSSection == null ||
				ctcFile.CMDSSection.BitmapsSubSection == null ||
				ctcFile.CMDSSection.BitmapsSubSection.Bitmaps == null)
			{
				return;
			}
			foreach (CTCBitmap ctcBitmap in ctcFile.CMDSSection.BitmapsSubSection.Bitmaps)
			{
				using (Bitmap bitmap = ctcBitmap.Bitmap)
				{
					if (bitmap == null)
					{
						continue;
					}
					using (MemoryStream bitmapStream = new MemoryStream())
					{
						bitmap.Save(bitmapStream, ImageFormat.Bmp);
						// For some reason we need to remove the first 14 bytes of the BMP header,
						// It seems that windows adds this header for us :(.
						byte[] bitmapArray = new byte[bitmapStream.Length - NativeMethods.BMPHeaderSize];
						Array.Copy(bitmapStream.GetBuffer(), NativeMethods.BMPHeaderSize, bitmapArray, 0, bitmapArray.Length);
						int res = NativeMethods.UpdateResource(hUpdate,
							NativeMethods.RT.BITMAP,
							ctcBitmap.Command.ID,
							(short)CultureInfo.CurrentUICulture.LCID,
							bitmapArray,
							bitmapArray.Length);
						if (res == 0)
						{
							Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
						}
					}
				}
			}
		}

		private void AddCTMENU(IntPtr hUpdate, string tempCTO)
		{
			using (FileStream ctoFile = new FileStream(tempCTO, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader ctoData = new BinaryReader(ctoFile))
				{
					byte[] ctoArray = ctoData.ReadBytes((int)ctoFile.Length);
					if (NativeMethods.UpdateResource(hUpdate, "CTMENU", resourceId, (short)CultureInfo.CurrentUICulture.LCID, ctoArray, ctoArray.Length) == 0)
					{
						Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
					}
				}
			}
		}
	}
}
