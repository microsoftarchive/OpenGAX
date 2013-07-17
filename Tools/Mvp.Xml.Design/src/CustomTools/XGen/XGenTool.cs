#region Usage
/* Usage:
 * On any C# code file, set:
 *	Custom Tool: XsdCodeGen
 *  Custom Tool Namespace: [Optional override for default namespace that matches the C# file namespace]
 * 
 * Author: Daniel Cazzulino - kzu.net@gmail.com
 */
#endregion Usage

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Schema;

using EnvDTE;
using VSLangProj;
using System.Globalization;

namespace Mvp.Xml.Design.CustomTools.XGen
{
	/// <summary>
	/// Generates custom typed XmlSerializers.
	/// </summary>
	/// <remarks>
	/// On any class set the Custom Tool property to "Mvp.Xml.XGen". 
	/// This tool supports C# projects only, as that's the code generated 
	/// by the XmlSerializer class.
	/// </remarks>
	[Guid("B393315E-C463-41cd-A274-CEB18DB7073C")]
	[CustomTool("Mvp.Xml.XGen", "Mvp.Xml Project XmlSerializer Generation Tool", true)]
	[ComVisible(true)]
	[VersionSupport("8.0")]
	[CategorySupport(CategorySupportAttribute.CSharpCategory)]
	public class XGenTool : CustomTool
	{
		static string ConfigFile;
		const string XGenPrefix = "Mvp_Xml_XGen__";

		#region Static config initialization

		static XGenTool()
		{
			AssemblyName name = Assembly.GetExecutingAssembly().GetName();

			ConfigFile = Path.GetTempFileName() + ".config";
			using (StreamWriter sw = new StreamWriter(ConfigFile))
			{
				// Keep serialization files. Required for SGen to work.
				sw.Write(@"<?xml version='1.0' encoding='utf-8' ?>
<configuration>
	<system.diagnostics>
		<switches>
			<add name='XmlSerialization.Compilation' value='4'/>
		</switches>
	</system.diagnostics>
</configuration>");
			}
		}

		#endregion Static config initialization

		#region GenerateCode

		/// <summary>
		/// Generates the output.
		/// </summary>
		protected override string OnGenerateCode(string inputFileName, string inputFileContent)
		{
			ThrowIfNoClassFound();

			string outputPath = GetProjectOutputFullPath();

			// Force compilation of the current project. We need the type in the output.
			CurrentItem.DTE.Solution.SolutionBuild.BuildProject(
				CurrentItem.DTE.Solution.SolutionBuild.ActiveConfiguration.Name,
				CurrentItem.ContainingProject.UniqueName, true);

			if (CurrentItem.DTE.Solution.SolutionBuild.LastBuildInfo == 1)
			{
				return Properties.Resources.XGenTool_ProjectDoesNotCompile +
					File.ReadAllText(Path.ChangeExtension(InputFilePath, GetDefaultExtension()));
			}

			CopyDesignToOutput(outputPath);

			SelectionCollection selections = GetSerializedSelection();
			selections = ShowSelectionUI(selections);

			if (selections == null || selections.Count == 0)
			{
				return String.Empty;
			}
			
			StringBuilder output = new StringBuilder();

			AppDomainSetup appSetup = new AppDomainSetup();
			appSetup.ApplicationName = typeof(XGenTool).Namespace;
			appSetup.ApplicationBase = outputPath;
			appSetup.ConfigurationFile = ConfigFile;

			string[] targetTypes = GetTypesFromSelection(selections);

			GetSerializationCode(appSetup, output, targetTypes);

			DeleteDesignFromOutput(outputPath);

			SaveSelections(selections);

			return output.ToString();
		}

		private string[] GetTypesFromSelection(SelectionCollection selections)
		{
			string assemblyName = CurrentItem.ContainingProject.Properties.Item("AssemblyName").Value.ToString();
			string[] types = new string[selections.Count];
			for (int i = 0; i < selections.Count; i++)
			{
				types[i] = selections[i].ClassName + ", " + assemblyName;
			}

			return types;
		}

		private void SaveSelections(SelectionCollection selections)
		{
			string key = BuildItemKey(CurrentItem);
			CurrentItem.ContainingProject.Globals[key] = selections.ToString();
			CurrentItem.ContainingProject.Globals.set_VariablePersists(key, true);
		}

		private void GetSerializationCode(AppDomainSetup appSetup, StringBuilder output, string[] targetTypes)
		{
			string codefile = Path.GetTempFileName();
			AppDomain domain = AppDomain.CreateDomain(appSetup.ApplicationName, null, appSetup);
			try
			{
				// Runner ctor will dump the output to the file we pass.
				domain.CreateInstance(
					Assembly.GetExecutingAssembly().FullName,
					typeof(XGenRunner).FullName, false, 0, null,
					new object[] { 
						codefile,
						targetTypes,
						base.FileNameSpace },
					null, null, null);

				using (StreamReader reader = new StreamReader(codefile))
				{
					output.Append(reader.ReadToEnd()).Append(Environment.NewLine);
				}
			}
			finally
			{
				AppDomain.Unload(domain);
			}
		}

		private SelectionCollection ShowSelectionUI(SelectionCollection selections)
		{
			ClassPicker picker = new ClassPicker(CurrentItem.FileCodeModel.CodeElements, base.FileNameSpace, selections);
			if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				selections = picker.Selections;
			}
			return selections;
		}

		private SelectionCollection GetSerializedSelection()
		{
			string key = BuildItemKey(CurrentItem);
			string serialized = String.Empty;
			if (CurrentItem.ContainingProject.Globals.get_VariableExists(key))
			{
				serialized = (string)CurrentItem.ContainingProject.Globals[key];
			}

			SelectionCollection selections = null;

			if (serialized.Length > 0)
			{
				selections = SelectionCollection.FromString(serialized);
			}
			return selections;
		}

		private void CopyDesignToOutput(string outputPath)
		{
			// Copy Design assembly to output for the isolated AppDomain.
			string asmfile = GetAssemblyPath(Assembly.GetExecutingAssembly());
			try
			{
				File.Copy(asmfile, Path.Combine(outputPath, Path.GetFileName(asmfile)), true);
			}
			catch (Exception ex)
			{
				// May already exist, be locked, etc.
				System.Diagnostics.Debug.WriteLine(ex.ToString());
			}
		}

		private void DeleteDesignFromOutput(string outputPath)
		{
			string asmfile = GetAssemblyPath(Assembly.GetExecutingAssembly());
			try
			{
				File.Delete(Path.Combine(outputPath, Path.GetFileName(asmfile)));
			}
			catch (Exception ex)
			{
				// May already exist, be locked, etc.
				System.Diagnostics.Debug.WriteLine(ex.ToString());
			}
		}

		private void ThrowIfNoClassFound()
		{
			if (CurrentItem.FileCodeModel == null || CurrentItem.FileCodeModel.CodeElements == null)
			{
				throw new InvalidOperationException(Properties.Resources.XGenTool_NoClassFound);
			}
		}

		private string GetProjectOutputFullPath()
		{
			string outputPath = base.CurrentItem.ContainingProject.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
			outputPath = Path.Combine(base.CurrentItem.ContainingProject.Properties.Item("FullPath").Value.ToString(), outputPath);
			return outputPath;
		}

		private string BuildItemKey(ProjectItem item)
		{
			string itemVirtualPath = DteHelper.BuildPath(item);
			itemVirtualPath = itemVirtualPath.Replace(".", "__").Replace("\\", "___");

			return itemVirtualPath;
		}

		private string GetAssemblyPath(Assembly assembly)
		{
			Uri uri = new Uri(assembly.CodeBase);
			return uri.LocalPath;
		}

		#endregion GenerateCode

		#region GetDefaultExtension

		/// <summary>
		/// This tool generates code, and the default extension equals that of the current code provider.
		/// </summary>
		public override string GetDefaultExtension()
		{
			return ".Serialization." + base.CodeProvider.FileExtension;
		}

		#endregion GetDefaultExtension

		#region Registration and Installation

		/// <summary>
		/// Registers the generator.
		/// </summary>
		[ComRegisterFunction]
		public static void RegisterClass(Type type)
		{
			CustomTool.Register(typeof(XGenTool));
		}

		/// <summary>
		/// Unregisters the generator.
		/// </summary>
		[ComUnregisterFunction]
		public static void UnregisterClass(Type t)
		{
			CustomTool.UnRegister(typeof(XGenTool));
		}

		#endregion Registration and Installation
	}
}