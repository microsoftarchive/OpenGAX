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
using System.Xml;
using System.Xml.Schema;
using System.Diagnostics;
using System.Collections;
using System.Security.Policy;

namespace Microsoft.Practices.RecipeFramework.Internal
{
	internal class FixedBaseURIWrappingReader : XmlReader, IXmlLineInfo
	{
		string baseUri;
		protected XmlReader reader;
		protected IXmlLineInfo lineInfo;

		internal FixedBaseURIWrappingReader(XmlReader baseReader, string baseUri)
		{
			this.baseUri = baseUri;
			reader = baseReader;
			lineInfo = reader as IXmlLineInfo;
		}

		public override string BaseURI { get { return baseUri; } }

		public override XmlReaderSettings Settings { get { return reader.Settings; } }
		public override XmlNodeType NodeType { get { return reader.NodeType; } }
		public override string Name { get { return reader.Name; } }
		public override string LocalName { get { return reader.LocalName; } }
		public override string NamespaceURI { get { return reader.NamespaceURI; } }
		public override string Prefix { get { return reader.Prefix; } }
		public override bool HasValue { get { return reader.HasValue; } }
		public override string Value { get { return reader.Value; } }
		public override int Depth { get { return reader.Depth; } }
		public override bool IsEmptyElement { get { return reader.IsEmptyElement; } }
		public override bool IsDefault { get { return reader.IsDefault; } }
		public override char QuoteChar { get { return reader.QuoteChar; } }
		public override XmlSpace XmlSpace { get { return reader.XmlSpace; } }
		public override string XmlLang { get { return reader.XmlLang; } }
		public override IXmlSchemaInfo SchemaInfo { get { return reader.SchemaInfo; } }
		public override System.Type ValueType { get { return reader.ValueType; } }
		public override int AttributeCount { get { return reader.AttributeCount; } }
		public override bool CanResolveEntity { get { return reader.CanResolveEntity; } }
		public override bool EOF { get { return reader.EOF; } }
		public override ReadState ReadState { get { return reader.ReadState; } }
		public override bool HasAttributes { get { return reader.HasAttributes; } }
		public override XmlNameTable NameTable { get { return reader.NameTable; } }

		public override string GetAttribute(string name)
		{
			return reader.GetAttribute(name);
		}

		public override string GetAttribute(string name, string namespaceURI)
		{
			return reader.GetAttribute(name, namespaceURI);
		}

		public override string GetAttribute(int i)
		{
			return reader.GetAttribute(i);
		}

		public override bool MoveToAttribute(string name)
		{
			return reader.MoveToAttribute(name);
		}

		public override bool MoveToAttribute(string name, string ns)
		{
			return reader.MoveToAttribute(name, ns);
		}

		public override void MoveToAttribute(int i)
		{
			reader.MoveToAttribute(i);
		}

		public override bool MoveToFirstAttribute()
		{
			return reader.MoveToFirstAttribute();
		}

		public override bool MoveToNextAttribute()
		{
			return reader.MoveToNextAttribute();
		}

		public override bool MoveToElement()
		{
			return reader.MoveToElement();
		}

		public override bool Read()
		{
			return reader.Read();
		}

		public override void Close()
		{
			reader.Close();
		}

		public override void Skip()
		{
			reader.Skip();
		}

		public override string LookupNamespace(string prefix)
		{
			return reader.LookupNamespace(prefix);
		}

		public override void ResolveEntity()
		{
			reader.ResolveEntity();
		}

		public override bool ReadAttributeValue()
		{
			return reader.ReadAttributeValue();
		}

		protected override void Dispose(bool disposing)
		{
			((IDisposable)reader).Dispose();
		}

		#region IXmlLineInfo Members

		bool IXmlLineInfo.HasLineInfo()
		{
			return (lineInfo == null) ? false : lineInfo.HasLineInfo();
		}

		int IXmlLineInfo.LineNumber
		{
			get { return (lineInfo == null) ? 0 : lineInfo.LineNumber; }
		}

		int IXmlLineInfo.LinePosition
		{
			get { return (lineInfo == null) ? 0 : lineInfo.LinePosition; }
		}

		#endregion
	}
}