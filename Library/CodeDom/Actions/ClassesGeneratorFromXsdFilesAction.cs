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
using Microsoft.Practices.RecipeFramework;
using System.Xml.Serialization;
using System.IO;
using System.CodeDom;
using System.Xml.Schema;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Xml;
using System.Collections;
using Microsoft.Practices.RecipeFramework.Library.CodeDom.Helpers;

namespace Microsoft.Practices.RecipeFramework.Library.CodeDom.Actions
{
    /// <summary>
    /// Generates classes from a set of schema files located in a directory
    /// The name of the generated file is based on the corresponding schema id
    /// </summary>
    public class ClassesGeneratorFromXsdFilesAction : ConfigurableAction
    {
        #region Input Properties

        private string schemasPath;
        /// <summary>
        /// Path for the input schemas
        /// </summary>
        [Input(Required=true)]
        public string SchemasPath
        {
            get { return schemasPath; }
            set { schemasPath = value; }
        }

        private string targetNamespace;
        /// <summary>
        /// Namespace of each class
        /// </summary>        
        [Input(Required=true)]
        public string TargetNamespace
        {
            get { return targetNamespace; }
            set { targetNamespace = value; }
        }

        private string outputPath;
        /// <summary>
        /// Output path for the class files
        /// </summary>
        [Input(Required=true)] 
        public string OutputPath
        {
            get { return outputPath; }
            set { outputPath = value; }
        }

        private LanguageType language;
        /// <summary>
        /// Language for the code provider
        /// </summary>
        [Input(Required=true)]
        public LanguageType Language
        {
            get { return language; }
            set { language = value; }
        }

        private bool processComplexTypesOnly;
        /// <summary>
        /// Process ComplexTypes Only
        /// </summary>
        [Input(Required=false)]
        public bool ProcessComplexTypesOnly
        {
            get { return processComplexTypesOnly; }
            set { processComplexTypesOnly = value; }
        }
        #endregion

        #region ConfigurableAction Implementation

        /// <summary>
        /// Generates classes from a set of schema files located in a directory
        /// </summary>
        public override void Execute()
        {
            XmlSchemas schemas = new XmlSchemas();

            if(Path.IsPathRooted(this.SchemasPath) == false)
            {
                this.SchemasPath = new DirectoryInfo(this.SchemasPath).FullName;
            }

            ReadSchemas(schemas);

            GenerateFiles(schemas);
        }

        /// <summary>
        /// Do Nothing
        /// </summary>
        public override void Undo()
        {
            //Do Nothing
        }
        #endregion

        #region Private Implementation
        private void ReadSchemas(XmlSchemas schemas)
        {
            bool skipSchema;

            foreach(string schemaFile in Directory.GetFiles(this.SchemasPath, "*.xsd"))
            {
                skipSchema = false;
                using(XmlReader schemaReader = XmlReader.Create(schemaFile))
                {
                    XmlSchema xsd = XmlSchema.Read(schemaReader, delegate(object sender, ValidationEventArgs e)
                    {
                        if(e.Severity == XmlSeverityType.Error)
                        {
                            // Stop processing the offending schema.
                            skipSchema = true;
                        }
                    });

                    if(!skipSchema)
                    {
                        schemas.Add(xsd, new Uri("file://" + new FileInfo(schemaFile).FullName));
                    }
                }
            }
        }
        private XmlSchema GenerateSchemaForTypes(XmlSchemas schemas)
        {
            Hashtable elements = new Hashtable();
            XmlSchema xsd = new XmlSchema();

            xsd.ElementFormDefault = XmlSchemaForm.Qualified;

            // First list all types that already have elements using them.
            foreach(XmlSchema schema in schemas)
            {
                foreach(XmlSchemaElement element in schema.Elements.Values)
                {
                    if(element.SchemaTypeName != null &&
                        elements.ContainsKey(element.SchemaTypeName) == false)
                    {
                        elements.Add(element.SchemaTypeName, element);
                    }
                }
            }

            // Next, generate an element for each unused type.
            foreach(XmlSchema schema in schemas)
            {
                XmlSchemaInclude include = new XmlSchemaInclude();
                include.Schema = schema;
                xsd.Includes.Add(include);
                foreach(XmlSchemaType type in schema.SchemaTypes.Values)
                {
                    if(elements.ContainsKey(type.QualifiedName) == false)
                    {
                        XmlSchemaElement newElement = new XmlSchemaElement();
                        newElement.SchemaTypeName = type.QualifiedName;
                        newElement.Name = type.Name;
                        xsd.Items.Add(newElement);

                        elements.Add(type.QualifiedName, newElement);
                    }
                }
            }

            return xsd;
        }
        private CodeCompileUnit ExportCodeFromSchemas(XmlSchemas schemas)
        {
            CodeNamespace ns = new CodeNamespace(this.TargetNamespace);
            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(ns);

            XmlSchemaImporter importer = new XmlSchemaImporter(schemas,
                CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync,
                new ImportContext(new CodeIdentifiers(), true));
            XmlCodeExporter exporter = new XmlCodeExporter(ns, unit,
                CodeGenerationOptions.GenerateNewAsync | CodeGenerationOptions.GenerateProperties);

            List<XmlTypeMapping> mappings = new List<XmlTypeMapping>();

            foreach(XmlSchema xsd in schemas)
            {
                foreach(XmlSchemaType type in xsd.SchemaTypes.Values)
                {
                    mappings.Add(importer.ImportSchemaType(type.QualifiedName));
                }

                if(!this.ProcessComplexTypesOnly)
                {
                    foreach(XmlSchemaElement element in xsd.Elements.Values)
                    {
                        mappings.Add(importer.ImportTypeMapping(element.QualifiedName));
                    }
                }
            }

            foreach(XmlTypeMapping mapping in mappings)
            {
                exporter.ExportTypeMapping(mapping);
            }

            CodeGenerator.ValidateIdentifiers(ns);

            return unit;
        }
        private CodeTypeDeclaration FindTypeDeclarationByName(CodeNamespace codeNamespace, string typeName)
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
        private void GenerateFiles(XmlSchemas schemas)
        {
            CodeCompileUnit codeCompileUnit = ExportCodeFromSchemas(schemas);
            CodeNamespace codeNamespace = codeCompileUnit.Namespaces[0];
            CodeDomProvider provider = new CSharpCodeProvider();
            CodeGeneratorOptions options = new CodeGeneratorOptions();

            foreach(XmlSchema schema in schemas)
            {
                CodeCompileUnit codeCompileUnitTemp = new CodeCompileUnit();
                codeCompileUnitTemp.Namespaces.Add(new CodeNamespace(this.TargetNamespace));
                CodeNamespace codeNamespaceTemp = codeCompileUnitTemp.Namespaces[0];
                CodeTypeDeclaration typeToAdd;
                string fileFullName;

                foreach(XmlSchemaType schemaType in schema.SchemaTypes.Values)
                {
                    typeToAdd = CodeCompileUnitHelper.FindTypeDeclarationByName(
                                                        codeNamespace, 
                                                        schemaType.Name);

                    if(typeToAdd != null && !codeNamespaceTemp.Types.Contains(typeToAdd))
                    {
                        codeNamespaceTemp.Types.Add(typeToAdd);
                    }
                }

                if(!this.ProcessComplexTypesOnly)
                {
                    foreach(XmlSchemaElement element in schema.Elements.Values)
                    {
                        typeToAdd = CodeCompileUnitHelper.FindTypeDeclarationByName(
                                                            codeNamespace, 
                                                            element.Name);

                        if(typeToAdd != null && !codeNamespaceTemp.Types.Contains(typeToAdd))
                        {
                            codeNamespaceTemp.Types.Add(typeToAdd);
                        }
                    }
                }

                fileFullName = Path.Combine(this.OutputPath, string.Concat(
                                                                schema.Id, 
                                                                ".", 
                                                                this.Language));

                using(StringWriter sw = new StringWriter())
                {
                    provider.GenerateCodeFromNamespace(codeCompileUnitTemp.Namespaces[0], sw, options);

                    System.IO.File.WriteAllText(fileFullName, sw.ToString());
                }
            }
        }
        #endregion
    }
}
