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
using System.Xml.Serialization;
using System.IO;
using System.Xml.Schema;
using System.Xml;
using System.Globalization;
using System.Collections;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Library.CodeDom.Helpers;

namespace Microsoft.Practices.RecipeFramework.Library.CodeDom.Actions
{
    /// <summary>
    /// Generates classes for schemas.
    /// </summary>
    public class CompileUnitGeneratorFromXsdAction : ConfigurableAction
    {
        #region input properties

        private string xsdFile;
        /// <summary>
        /// Schemas to process
        /// </summary>
        [Input(Required=true)]
        public string XsdFile
        {
            get { return xsdFile; }
            set { xsdFile = value; }
        }

        private string targetNamespace;
        /// <summary>
        /// Namespace used in the CodeNamespace
        /// </summary>
        [Input(Required=true)]
        public string TargetNamespace
        {
            get { return targetNamespace; }
            set { targetNamespace = value; }
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

        #region output properties

        private CodeCompileUnit compileUnit;
        /// <summary>
        /// Classes generated from the input xsd
        /// </summary>
        [Output]
        public CodeCompileUnit CompileUnit
        {
            get { return compileUnit; }
            set { compileUnit = value; }
        }

        #endregion

        #region Action Member

        /// <summary>
        /// Generates classes for the schemaFileName specified.
        /// </summary>
        public override void Execute()
        {
            // Load the XmlSchema and its collection.
            XmlSchema xsd;
            string xsdImportPath;

            using(FileStream fs = new FileStream(this.xsdFile, FileMode.Open))
            {
                xsd = XmlSchema.Read(fs, null);
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add(xsd);

                foreach(XmlSchemaImport import in xsd.Includes)
                {
                    xsdImportPath = ResolveImportPath(import);

                    using(FileStream fsSchemaImport = new FileStream(xsdImportPath, FileMode.Open))
                    {
                        XmlSchema xsdTemp = XmlSchema.Read(fsSchemaImport, null);
                        schemaSet.Add(xsdTemp);
                    }
                }

                schemaSet.Compile();
            }

            XmlSchemas schemas = new XmlSchemas();

            schemas.Add(xsd);

            foreach(XmlSchemaImport import in xsd.Includes)
            {
                xsdImportPath = ResolveImportPath(import);

                using(FileStream fs = new FileStream(xsdImportPath, FileMode.Open))
                {
                    XmlSchema xsdTemp = XmlSchema.Read(fs, null);
                    schemas.Add(xsdTemp);
                }
            }

            // Create the importer for these schemas.
            XmlSchemaImporter importer = new XmlSchemaImporter(schemas);

            // System.CodeDom namespace for the XmlCodeExporter to put classes in.
            CodeNamespace ns = new CodeNamespace(this.targetNamespace);
            XmlCodeExporter exporter = new XmlCodeExporter(ns);

            List<XmlTypeMapping> mappings = new List<XmlTypeMapping>();

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

            foreach(XmlTypeMapping mapping in mappings)
            {
                exporter.ExportTypeMapping(mapping);
            }

            CodeGenerator.ValidateIdentifiers(ns);

            compileUnit = new CodeCompileUnit();

            schemas.Remove(xsd);

            //Remove Types from CodeCompileUnit that belong to the imported schemas
            RemoveTypes(schemas, ns);

            compileUnit.Namespaces.Add(ns);
        }

        /// <summary>
        /// This method do nothing
        /// </summary>
        public override void Undo()
        {
            // This method do nothing
        }

        #endregion

        #region Private methods

        private void RemoveTypes(XmlSchemas schemas, CodeNamespace ns)
        {
            CodeTypeDeclaration typeToRemove;

            foreach(XmlSchema schema in schemas)
            {
                foreach(XmlSchemaElement element in schema.Elements.Values)
                {
                    typeToRemove = CodeCompileUnitHelper.FindTypeDeclarationByName(ns, element.Name);

                    if(typeToRemove != null)
                    {
                        ns.Types.Remove(typeToRemove);
                    }
                }

                foreach(XmlSchemaType schemaType in schema.SchemaTypes.Values)
                {
                    typeToRemove = CodeCompileUnitHelper.FindTypeDeclarationByName(ns, schemaType.Name);

                    if(typeToRemove != null)
                    {
                        ns.Types.Remove(typeToRemove);
                    }
                }

            }
        }
        private string ResolveImportPath(XmlSchemaImport import)
        {
            string xsdImportPath;

            if(!Path.IsPathRooted(import.SchemaLocation))
            {
                xsdImportPath = Path.Combine(Path.GetDirectoryName(this.xsdFile), import.SchemaLocation);
            }
            else
            {
                xsdImportPath = import.SchemaLocation;
            }

            return xsdImportPath;
        }
        #endregion

    }
}
