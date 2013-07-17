#region Usage
/* Usage:
 * Add an .xsd file to the project and set:
 *	Build Action: Content
 *	Custom Tool: XsdCodeGen
 * 
 * Author: Daniel Cazzulino - kzu.net@gmail.com
 */
#endregion Usage

using System;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Mvp.Xml.Design.CustomTools
{
	/// <summary>
	/// Uses the XsdGeneratorLibrary to process XSD files and generate the corresponding 
	/// classes.
	/// </summary>
	[Guid("39215CF7-DA75-49f1-809E-7D027E1AB24D")]
	[ComVisible(true)]
	[CustomTool("Mvp.Xml.XsdGen", "Mvp.Xml XSD to Classes Generator", true)]
	[VersionSupport("8.0")]
	[CategorySupport(CategorySupportAttribute.CSharpCategory)]
	[CategorySupport(CategorySupportAttribute.VBCategory)]
	public class XsdCodeGenerator : CustomTool
	{
		/// <summary>
		/// Generates the output.
		/// </summary>
		protected override string OnGenerateCode(string inputFileName, string inputFileContent)
		{
			CodeNamespace ns = ExportCode();
			string output = GenerateSource(ns);

			// Workaround for known bug with fixed attributes: http://lab.msdn.microsoft.com/productfeedback/viewfeedback.aspx?feedbackid=d457a36e-0ce8-4368-9a27-92762860d8e1
			output = @"// Workaround for bug http://lab.msdn.microsoft.com/productfeedback/viewfeedback.aspx?feedbackid=d457a36e-0ce8-4368-9a27-92762860d8e1
#pragma warning disable 1591
" + output + @"
#pragma warning restore 1591";
			output = CustomTool.GetToolGeneratedCodeWarning(typeof(XsdCodeGenerator)) + output;

			return output;
		}

		private string GenerateSource(CodeNamespace ns)
		{
			CodeGeneratorOptions opt = new CodeGeneratorOptions();
			opt.BracingStyle = "C";
			StringWriter sw = new StringWriter();
			GetCodeWriter().GenerateCodeFromNamespace(ns, sw, opt);
			sw.Flush();

			return sw.ToString();
		}

		private CodeNamespace ExportCode()
		{
			XmlSchema xsd;
			XmlSchemas schemas;
			LoadSchemas(out xsd, out schemas);

			CodeNamespace ns = new CodeNamespace(base.FileNameSpace);

			XmlSchemaImporter importer = new XmlSchemaImporter(schemas);
			XmlCodeExporter exporter = new XmlCodeExporter(ns);

			GenerateForElements(xsd, importer, exporter);
			GenerateForComplexTypes(xsd, importer, exporter);

			return ns;
		}

		private static void GenerateForComplexTypes(XmlSchema xsd, XmlSchemaImporter importer, XmlCodeExporter exporter)
		{
			foreach (XmlSchemaObject type in xsd.SchemaTypes.Values)
			{
				XmlSchemaComplexType ct = type as XmlSchemaComplexType;
				if (ct != null)
				{
					XmlTypeMapping mapping = importer.ImportSchemaType(ct.QualifiedName);
					exporter.ExportTypeMapping(mapping);
				}
			}
		}

		private static void GenerateForElements(XmlSchema xsd, XmlSchemaImporter importer, XmlCodeExporter exporter)
		{
			foreach (XmlSchemaElement element in xsd.Elements.Values)
			{
				XmlTypeMapping mapping = importer.ImportTypeMapping(element.QualifiedName);
				exporter.ExportTypeMapping(mapping);
			}
		}

		private void LoadSchemas(out XmlSchema xsd, out XmlSchemas schemas)
		{
			using (FileStream fs = File.OpenRead(base.InputFilePath))
			{
				xsd = XmlSchema.Read(fs, null);
				xsd.Compile(null);
			}

			schemas = new XmlSchemas();
			schemas.Add(xsd);
		}

		#region Registration and Installation

		/// <summary>
		/// Registers the generator.
		/// </summary>
		[ComRegisterFunction]
		public static void RegisterClass(Type type)
		{
			CustomTool.Register(typeof(XsdCodeGenerator));
		}

		/// <summary>
		/// Unregisters the generator.
		/// </summary>
		[ComUnregisterFunction]
		public static void UnregisterClass(Type t)
		{
			CustomTool.UnRegister(typeof(XsdCodeGenerator));
		}

		#endregion Registration and Installation
	}
}