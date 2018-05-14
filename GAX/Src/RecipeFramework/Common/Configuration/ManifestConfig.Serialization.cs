#region Original XmlSerializer code
/* -------------------------------------------------------------------------------------
#if _DYNAMIC_XMLSERIALIZER_COMPILATION
[assembly:System.Security.AllowPartiallyTrustedCallers()]
[assembly:System.Security.SecurityTransparent()]
#endif
[assembly:System.Reflection.AssemblyVersionAttribute("1.0.60429.0")]
namespace Microsoft.Xml.Serialization.GeneratedAssembly {

    public class XmlSerializationWriterGuidancePackage : System.Xml.Serialization.XmlSerializationWriter {

        public void Write3_GuidancePackage(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"GuidancePackage", @"http://schemas.microsoft.com/pag/gax-manifest");
                return;
            }
            TopLevelElement();
            Write2_GuidancePackage(@"GuidancePackage", @"http://schemas.microsoft.com/pag/gax-manifest", ((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage)o), false, false);
        }

        void Write2_GuidancePackage(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-manifest");
            WriteAttribute(@"Name", @"", FromXmlNCName(((global::System.String)o.@Name)));
            WriteAttribute(@"Caption", @"", ((global::System.String)o.@Caption));
            WriteAttribute(@"Description", @"", ((global::System.String)o.@Description));
            WriteAttribute(@"Version", @"", ((global::System.String)o.@Version));
            if (((global::System.String)o.@Host) != @"VisualStudio") {
                WriteAttribute(@"Host", @"", ((global::System.String)o.@Host));
            }
            WriteAttribute(@"Guid", @"", ((global::System.String)o.@Guid));
            WriteAttribute(@"ConfigurationFile", @"", ((global::System.String)o.@ConfigurationFile));
            WriteAttribute(@"Timestamp", @"", ((global::System.String)o.@Timestamp));
            WriteEndElement(o);
        }

        protected override void InitCallbacks() {
        }
    }

    public class XmlSerializationReaderGuidancePackage : System.Xml.Serialization.XmlSerializationReader {

        public object Read3_GuidancePackage() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id1_GuidancePackage && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read2_GuidancePackage(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-manifest:GuidancePackage");
            }
            return (object)o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage Read2_GuidancePackage(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage();
            bool[] paramsRead = new bool[8];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = ToXmlNCName(Reader.Value);
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id5_Caption && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Caption = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id6_Description && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Description = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id7_Version && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Version = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id8_Host && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Host = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id9_Guid && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Guid = Reader.Value;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id10_ConfigurationFile && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@ConfigurationFile = Reader.Value;
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id11_Timestamp && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Timestamp = Reader.Value;
                    paramsRead[7] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Name, :Caption, :Description, :Version, :Host, :Guid, :ConfigurationFile, :Timestamp");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            ReadEndElement();
            return o;
        }

        protected override void InitCallbacks() {
        }

        string id4_Name;
        string id10_ConfigurationFile;
        string id3_Item;
        string id2_Item;
        string id8_Host;
        string id7_Version;
        string id9_Guid;
        string id11_Timestamp;
        string id6_Description;
        string id5_Caption;
        string id1_GuidancePackage;

        protected override void InitIDs() {
            id4_Name = Reader.NameTable.Add(@"Name");
            id10_ConfigurationFile = Reader.NameTable.Add(@"ConfigurationFile");
            id3_Item = Reader.NameTable.Add(@"");
            id2_Item = Reader.NameTable.Add(@"http://schemas.microsoft.com/pag/gax-manifest");
            id8_Host = Reader.NameTable.Add(@"Host");
            id7_Version = Reader.NameTable.Add(@"Version");
            id9_Guid = Reader.NameTable.Add(@"Guid");
            id11_Timestamp = Reader.NameTable.Add(@"Timestamp");
            id6_Description = Reader.NameTable.Add(@"Description");
            id5_Caption = Reader.NameTable.Add(@"Caption");
            id1_GuidancePackage = Reader.NameTable.Add(@"GuidancePackage");
        }
    }

    public abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer {
        protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
            return new XmlSerializationReaderGuidancePackage();
        }
        protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
            return new XmlSerializationWriterGuidancePackage();
        }
    }

    public sealed class GuidancePackageSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"GuidancePackage", @"http://schemas.microsoft.com/pag/gax-manifest");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriterGuidancePackage)writer).Write3_GuidancePackage(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReaderGuidancePackage)reader).Read3_GuidancePackage();
        }
    }

    public class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
        public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new XmlSerializationReaderGuidancePackage(); } }
        public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new XmlSerializationWriterGuidancePackage(); } }
        System.Collections.Hashtable readMethods = null;
        public override System.Collections.Hashtable ReadMethods {
            get {
                if (readMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage:http://schemas.microsoft.com/pag/gax-manifest::False:"] = @"Read3_GuidancePackage";
                    if (readMethods == null) readMethods = _tmp;
                }
                return readMethods;
            }
        }
        System.Collections.Hashtable writeMethods = null;
        public override System.Collections.Hashtable WriteMethods {
            get {
                if (writeMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage:http://schemas.microsoft.com/pag/gax-manifest::False:"] = @"Write3_GuidancePackage";
                    if (writeMethods == null) writeMethods = _tmp;
                }
                return writeMethods;
            }
        }
        System.Collections.Hashtable typedSerializers = null;
        public override System.Collections.Hashtable TypedSerializers {
            get {
                if (typedSerializers == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp.Add(@"Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage:http://schemas.microsoft.com/pag/gax-manifest::False:", new GuidancePackageSerializer());
                    if (typedSerializers == null) typedSerializers = _tmp;
                }
                return typedSerializers;
            }
        }
        public override System.Boolean CanSerialize(System.Type type) {
            if (type == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage)) return true;
            return false;
        }
        public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
            if (type == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage)) return new GuidancePackageSerializer();
            return null;
        }
    }
}

-------------------------------------------------------------------------------------*/
#endregion
//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by the Mvp.Xml.XGen tool.
//     Tool Version:    1.1.1.0
//     Runtime Version: 2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

#pragma warning disable 0642, 0219
namespace Microsoft.Practices.RecipeFramework.Configuration.Manifest
{
    using System.Xml.Serialization;
    using System;
    
    
    /// /// <summary>Custom reader for GuidancePackage instances.</summary>
    internal class GuidancePackageReader : GuidancePackageSerializer.BaseReader
    {
        

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage Read2_GuidancePackage(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage obj = base.Read2_GuidancePackage(isNullable, checkType);
			GuidancePackageDeserializedHandler handler = GuidancePackageDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <summary>Reads an object of type Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage.</summary>
		internal Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage Read()
		{
			return (Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage) Read3_GuidancePackage();
		}
        
        /// /// <remarks/>
        public event GuidancePackageDeserializedHandler GuidancePackageDeserialized;
    }
    
    /// /// <remarks/>
    public delegate void GuidancePackageDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage guidancePackage);
    
    /// /// <summary>Custom writer for GuidancePackage instances.</summary>
    internal class GuidancePackageWriter : GuidancePackageSerializer.BaseWriter
    {
        

		/// <summary>Writes an object of type Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage.</summary>
		internal void Write(Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage value)
		{
			Write3_GuidancePackage(value);
		}
    }
}
#if _DYNAMIC_XMLSERIALIZER_COMPILATION
[assembly:System.Security.AllowPartiallyTrustedCallers()]
[assembly:System.Security.SecurityTransparent()]
#endif

namespace Microsoft.Practices.RecipeFramework.Configuration.Manifest {

    internal partial class GuidancePackageSerializer {
	internal class BaseWriter : System.Xml.Serialization.XmlSerializationWriter {

        protected internal void Write3_GuidancePackage(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"GuidancePackage", @"http://schemas.microsoft.com/pag/gax-manifest");
                return;
            }
            TopLevelElement();
            Write2_GuidancePackage(@"GuidancePackage", @"http://schemas.microsoft.com/pag/gax-manifest", ((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage)o), false, false);
        }

        void Write2_GuidancePackage(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-manifest");
            WriteAttribute(@"Name", @"", FromXmlNCName(((global::System.String)o.@Name)));
            WriteAttribute(@"Caption", @"", ((global::System.String)o.@Caption));
            WriteAttribute(@"Description", @"", ((global::System.String)o.@Description));
            WriteAttribute(@"Version", @"", ((global::System.String)o.@Version));
            if (((global::System.String)o.@Host) != @"VisualStudio") {
                WriteAttribute(@"Host", @"", ((global::System.String)o.@Host));
            }
            WriteAttribute(@"Guid", @"", ((global::System.String)o.@Guid));
            WriteAttribute(@"ConfigurationFile", @"", ((global::System.String)o.@ConfigurationFile));
            WriteAttribute(@"Timestamp", @"", ((global::System.String)o.@Timestamp));
            WriteEndElement(o);
        }

        protected override void InitCallbacks() {
        }
    }
	}

    internal partial class GuidancePackageSerializer {
	internal class BaseReader : System.Xml.Serialization.XmlSerializationReader {

        protected internal object Read3_GuidancePackage() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id1_GuidancePackage && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read2_GuidancePackage(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-manifest:GuidancePackage");
            }
            return (object)o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage Read2_GuidancePackage(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage();
            bool[] paramsRead = new bool[8];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = ToXmlNCName(Reader.Value);
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id5_Caption && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Caption = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id6_Description && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Description = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id7_Version && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Version = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id8_Host && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Host = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id9_Guid && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Guid = Reader.Value;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id10_ConfigurationFile && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@ConfigurationFile = Reader.Value;
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id11_Timestamp && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Timestamp = Reader.Value;
                    paramsRead[7] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Name, :Caption, :Description, :Version, :Host, :Guid, :ConfigurationFile, :Timestamp");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            ReadEndElement();
            return o;
        }

        protected override void InitCallbacks() {
        }

        string id4_Name;
        string id10_ConfigurationFile;
        string id3_Item;
        string id2_Item;
        string id8_Host;
        string id7_Version;
        string id9_Guid;
        string id11_Timestamp;
        string id6_Description;
        string id5_Caption;
        string id1_GuidancePackage;

        protected override void InitIDs() {
            id4_Name = Reader.NameTable.Add(@"Name");
            id10_ConfigurationFile = Reader.NameTable.Add(@"ConfigurationFile");
            id3_Item = Reader.NameTable.Add(@"");
            id2_Item = Reader.NameTable.Add(@"http://schemas.microsoft.com/pag/gax-manifest");
            id8_Host = Reader.NameTable.Add(@"Host");
            id7_Version = Reader.NameTable.Add(@"Version");
            id9_Guid = Reader.NameTable.Add(@"Guid");
            id11_Timestamp = Reader.NameTable.Add(@"Timestamp");
            id6_Description = Reader.NameTable.Add(@"Description");
            id5_Caption = Reader.NameTable.Add(@"Caption");
            id1_GuidancePackage = Reader.NameTable.Add(@"GuidancePackage");
        }
    }
	}

    internal abstract class GuidancePackageSerializerBase : System.Xml.Serialization.XmlSerializer {
        protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
            return new GuidancePackageSerializer.BaseReader();
        }
        protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
            return new GuidancePackageSerializer.BaseWriter();
        }
    }

    internal sealed partial class GuidancePackageSerializer : GuidancePackageSerializerBase {

		GuidancePackageReader _reader;
		GuidancePackageWriter _writer;

		/// <summary>Constructs the serializer.</summary>
		public GuidancePackageSerializer()
		{
		}

		/// <summary>Constructs the serializer with a pre-built reader.</summary>
		public GuidancePackageSerializer(GuidancePackageReader reader)
		{
			_reader = reader;
		}

		/// <summary>Constructs the serializer with a pre-built writer.</summary>
		public GuidancePackageSerializer(GuidancePackageWriter writer)
		{
			_writer = writer;
		}

		/// <summary>Constructs the serializer with pre-built reader and writer.</summary>
		public GuidancePackageSerializer(GuidancePackageReader reader, GuidancePackageWriter writer)
		{
			_reader = reader;
			_writer = writer;
		}

		/// <remarks/>
		protected override System.Xml.Serialization.XmlSerializationReader CreateReader()
		{
			if (_reader != null) return _reader;
			
			return base.CreateReader();
		}

		/// <remarks/>
		protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter()
		{
			if (_writer != null) return _writer;
			
			return base.CreateWriter();
		}

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"GuidancePackage", @"http://schemas.microsoft.com/pag/gax-manifest");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((BaseWriter)writer).Write3_GuidancePackage(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((BaseReader)reader).Read3_GuidancePackage();
        }
    }

    internal partial class GuidancePackageSerializer {
	internal class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
        public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new GuidancePackageSerializer.BaseReader(); } }
        public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new GuidancePackageSerializer.BaseWriter(); } }
        System.Collections.Hashtable readMethods = null;
        public override System.Collections.Hashtable ReadMethods {
            get {
                if (readMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage:http://schemas.microsoft.com/pag/gax-manifest::False:"] = @"Read3_GuidancePackage";
                    if (readMethods == null) readMethods = _tmp;
                }
                return readMethods;
            }
        }
        System.Collections.Hashtable writeMethods = null;
        public override System.Collections.Hashtable WriteMethods {
            get {
                if (writeMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage:http://schemas.microsoft.com/pag/gax-manifest::False:"] = @"Write3_GuidancePackage";
                    if (writeMethods == null) writeMethods = _tmp;
                }
                return writeMethods;
            }
        }
        System.Collections.Hashtable typedSerializers = null;
        public override System.Collections.Hashtable TypedSerializers {
            get {
                if (typedSerializers == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp.Add(@"Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage:http://schemas.microsoft.com/pag/gax-manifest::False:", new GuidancePackageSerializer());
                    if (typedSerializers == null) typedSerializers = _tmp;
                }
                return typedSerializers;
            }
        }
        public override System.Boolean CanSerialize(System.Type type) {
            if (type == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage)) return true;
            return false;
        }
        public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
            if (type == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage)) return new GuidancePackageSerializer();
            return null;
        }
    }
	}
}


#pragma warning restore 0642, 0219
#region Original XmlSerializer code
/* -------------------------------------------------------------------------------------
#if _DYNAMIC_XMLSERIALIZER_COMPILATION
[assembly:System.Security.AllowPartiallyTrustedCallers()]
[assembly:System.Security.SecurityTransparent()]
#endif
[assembly:System.Reflection.AssemblyVersionAttribute("1.0.60429.0")]
namespace Microsoft.Xml.Serialization.GeneratedAssembly {

    public class XmlSerializationWriterRecipeFramework : System.Xml.Serialization.XmlSerializationWriter {

        public void Write6_RecipeFramework(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"RecipeFramework", @"http://schemas.microsoft.com/pag/gax-manifest");
                return;
            }
            TopLevelElement();
            Write5_RecipeFramework(@"RecipeFramework", @"http://schemas.microsoft.com/pag/gax-manifest", ((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework)o), false, false);
        }

        void Write5_RecipeFramework(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-manifest");
            WriteAttribute(@"SchemaVersion", @"", ((global::System.String)o.@SchemaVersion));
            if (((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels)o.@SourceLevels) != global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Error) {
                WriteAttribute(@"SourceLevels", @"", Write4_SourceLevels(((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels)o.@SourceLevels)));
            }
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host[])((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host[])o.@Hosts);
                if (a != null){
                    WriteStartElement(@"Hosts", @"http://schemas.microsoft.com/pag/gax-manifest", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write2_Host(@"Host", @"http://schemas.microsoft.com/pag/gax-manifest", ((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage[])((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage[])o.@GuidancePackages);
                if (a != null){
                    WriteStartElement(@"GuidancePackages", @"http://schemas.microsoft.com/pag/gax-manifest", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write3_GuidancePackage(@"GuidancePackage", @"http://schemas.microsoft.com/pag/gax-manifest", ((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            WriteEndElement(o);
        }

        void Write3_GuidancePackage(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-manifest");
            WriteAttribute(@"Name", @"", FromXmlNCName(((global::System.String)o.@Name)));
            WriteAttribute(@"Caption", @"", ((global::System.String)o.@Caption));
            WriteAttribute(@"Description", @"", ((global::System.String)o.@Description));
            WriteAttribute(@"Version", @"", ((global::System.String)o.@Version));
            if (((global::System.String)o.@Host) != @"VisualStudio") {
                WriteAttribute(@"Host", @"", ((global::System.String)o.@Host));
            }
            WriteAttribute(@"Guid", @"", ((global::System.String)o.@Guid));
            WriteAttribute(@"ConfigurationFile", @"", ((global::System.String)o.@ConfigurationFile));
            WriteAttribute(@"Timestamp", @"", ((global::System.String)o.@Timestamp));
            WriteEndElement(o);
        }

        void Write2_Host(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-manifest");
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
            WriteAttribute(@"InstallerType", @"", ((global::System.String)o.@InstallerType));
            WriteEndElement(o);
        }

        string Write4_SourceLevels(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels v) {
            string s = null;
            switch (v) {
                case global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Error: s = @"Error"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Information: s = @"Information"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Off: s = @"Off"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Critical: s = @"Critical"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Warning: s = @"Warning"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Verbose: s = @"Verbose"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels");
            }
            return s;
        }

        protected override void InitCallbacks() {
        }
    }

    public class XmlSerializationReaderRecipeFramework : System.Xml.Serialization.XmlSerializationReader {

        public object Read6_RecipeFramework() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id1_RecipeFramework && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read5_RecipeFramework(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-manifest:RecipeFramework");
            }
            return (object)o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework Read5_RecipeFramework(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework();
            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host[] a_0 = null;
            int ca_0 = 0;
            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage[] a_1 = null;
            int ca_1 = 0;
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id4_SchemaVersion && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@SchemaVersion = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id5_SourceLevels && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@SourceLevels = Read4_SourceLevels(Reader.Value);
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":SchemaVersion, :SourceLevels");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id6_Hosts && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host[] a_0_0 = null;
                            int ca_0_0 = 0;
                            if ((Reader.IsEmptyElement)) {
                                Reader.Skip();
                            }
                            else {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations1 = 0;
                                int readerCount1 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                        if (((object) Reader.LocalName == (object)id7_Host && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_0_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host[])EnsureArrayIndex(a_0_0, ca_0_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host));a_0_0[ca_0_0++] = Read2_Host(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-manifest:Host");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-manifest:Host");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations1, ref readerCount1);
                                }
                            ReadEndElement();
                            }
                            o.@Hosts = (global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host[])ShrinkArray(a_0_0, ca_0_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host), false);
                        }
                    }
                    else if (((object) Reader.LocalName == (object)id8_GuidancePackages && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage[] a_1_0 = null;
                            int ca_1_0 = 0;
                            if ((Reader.IsEmptyElement)) {
                                Reader.Skip();
                            }
                            else {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations2 = 0;
                                int readerCount2 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                        if (((object) Reader.LocalName == (object)id9_GuidancePackage && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_1_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage[])EnsureArrayIndex(a_1_0, ca_1_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage));a_1_0[ca_1_0++] = Read3_GuidancePackage(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-manifest:GuidancePackage");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-manifest:GuidancePackage");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations2, ref readerCount2);
                                }
                            ReadEndElement();
                            }
                            o.@GuidancePackages = (global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage[])ShrinkArray(a_1_0, ca_1_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage), false);
                        }
                    }
                    else {
                        UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-manifest:Hosts, http://schemas.microsoft.com/pag/gax-manifest:GuidancePackages");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-manifest:Hosts, http://schemas.microsoft.com/pag/gax-manifest:GuidancePackages");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage Read3_GuidancePackage(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage();
            bool[] paramsRead = new bool[8];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id10_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = ToXmlNCName(Reader.Value);
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id11_Caption && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Caption = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id12_Description && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Description = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id13_Version && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Version = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id7_Host && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Host = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id14_Guid && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Guid = Reader.Value;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id15_ConfigurationFile && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@ConfigurationFile = Reader.Value;
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id16_Timestamp && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Timestamp = Reader.Value;
                    paramsRead[7] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Name, :Caption, :Description, :Version, :Host, :Guid, :ConfigurationFile, :Timestamp");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations3 = 0;
            int readerCount3 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations3, ref readerCount3);
            }
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host Read2_Host(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host();
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id10_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id17_InstallerType && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@InstallerType = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Name, :InstallerType");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations4 = 0;
            int readerCount4 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations4, ref readerCount4);
            }
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels Read4_SourceLevels(string s) {
            switch (s) {
                case @"Error": return global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Error;
                case @"Information": return global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Information;
                case @"Off": return global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Off;
                case @"Critical": return global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Critical;
                case @"Warning": return global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Warning;
                case @"Verbose": return global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Verbose;
                default: throw CreateUnknownConstantException(s, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels));
            }
        }

        protected override void InitCallbacks() {
        }

        string id10_Name;
        string id13_Version;
        string id5_SourceLevels;
        string id15_ConfigurationFile;
        string id1_RecipeFramework;
        string id16_Timestamp;
        string id7_Host;
        string id3_Item;
        string id12_Description;
        string id6_Hosts;
        string id11_Caption;
        string id8_GuidancePackages;
        string id4_SchemaVersion;
        string id14_Guid;
        string id2_Item;
        string id9_GuidancePackage;
        string id17_InstallerType;

        protected override void InitIDs() {
            id10_Name = Reader.NameTable.Add(@"Name");
            id13_Version = Reader.NameTable.Add(@"Version");
            id5_SourceLevels = Reader.NameTable.Add(@"SourceLevels");
            id15_ConfigurationFile = Reader.NameTable.Add(@"ConfigurationFile");
            id1_RecipeFramework = Reader.NameTable.Add(@"RecipeFramework");
            id16_Timestamp = Reader.NameTable.Add(@"Timestamp");
            id7_Host = Reader.NameTable.Add(@"Host");
            id3_Item = Reader.NameTable.Add(@"");
            id12_Description = Reader.NameTable.Add(@"Description");
            id6_Hosts = Reader.NameTable.Add(@"Hosts");
            id11_Caption = Reader.NameTable.Add(@"Caption");
            id8_GuidancePackages = Reader.NameTable.Add(@"GuidancePackages");
            id4_SchemaVersion = Reader.NameTable.Add(@"SchemaVersion");
            id14_Guid = Reader.NameTable.Add(@"Guid");
            id2_Item = Reader.NameTable.Add(@"http://schemas.microsoft.com/pag/gax-manifest");
            id9_GuidancePackage = Reader.NameTable.Add(@"GuidancePackage");
            id17_InstallerType = Reader.NameTable.Add(@"InstallerType");
        }
    }

    public abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer {
        protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
            return new XmlSerializationReaderRecipeFramework();
        }
        protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
            return new XmlSerializationWriterRecipeFramework();
        }
    }

    public sealed class RecipeFrameworkSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"RecipeFramework", @"http://schemas.microsoft.com/pag/gax-manifest");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriterRecipeFramework)writer).Write6_RecipeFramework(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReaderRecipeFramework)reader).Read6_RecipeFramework();
        }
    }

    public class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
        public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new XmlSerializationReaderRecipeFramework(); } }
        public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new XmlSerializationWriterRecipeFramework(); } }
        System.Collections.Hashtable readMethods = null;
        public override System.Collections.Hashtable ReadMethods {
            get {
                if (readMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework:http://schemas.microsoft.com/pag/gax-manifest::False:"] = @"Read6_RecipeFramework";
                    if (readMethods == null) readMethods = _tmp;
                }
                return readMethods;
            }
        }
        System.Collections.Hashtable writeMethods = null;
        public override System.Collections.Hashtable WriteMethods {
            get {
                if (writeMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework:http://schemas.microsoft.com/pag/gax-manifest::False:"] = @"Write6_RecipeFramework";
                    if (writeMethods == null) writeMethods = _tmp;
                }
                return writeMethods;
            }
        }
        System.Collections.Hashtable typedSerializers = null;
        public override System.Collections.Hashtable TypedSerializers {
            get {
                if (typedSerializers == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp.Add(@"Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework:http://schemas.microsoft.com/pag/gax-manifest::False:", new RecipeFrameworkSerializer());
                    if (typedSerializers == null) typedSerializers = _tmp;
                }
                return typedSerializers;
            }
        }
        public override System.Boolean CanSerialize(System.Type type) {
            if (type == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework)) return true;
            return false;
        }
        public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
            if (type == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework)) return new RecipeFrameworkSerializer();
            return null;
        }
    }
}

-------------------------------------------------------------------------------------*/
#endregion
//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by the Mvp.Xml.XGen tool.
//     Tool Version:    1.1.1.0
//     Runtime Version: 2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

#pragma warning disable 0642, 0219
namespace Microsoft.Practices.RecipeFramework.Configuration.Manifest
{
    using System.Xml.Serialization;
    using System;
    
    
    /// /// <summary>Custom reader for RecipeFramework instances.</summary>
    internal class RecipeFrameworkReader : RecipeFrameworkSerializer.BaseReader
    {
        

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework Read5_RecipeFramework(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework obj = base.Read5_RecipeFramework(isNullable, checkType);
			RecipeFrameworkDeserializedHandler handler = RecipeFrameworkDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage Read3_GuidancePackage(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage obj = base.Read3_GuidancePackage(isNullable, checkType);
			GuidancePackageDeserializedHandler handler = GuidancePackageDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host Read2_Host(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host obj = base.Read2_Host(isNullable, checkType);
			HostDeserializedHandler handler = HostDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels Read4_SourceLevels(string s)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels obj = base.Read4_SourceLevels(s);
			SourceLevelsDeserializedHandler handler = SourceLevelsDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <summary>Reads an object of type Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework.</summary>
		internal Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework Read()
		{
			return (Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework) Read6_RecipeFramework();
		}
        
        /// /// <remarks/>
        public event RecipeFrameworkDeserializedHandler RecipeFrameworkDeserialized;
        
        /// /// <remarks/>
        public event GuidancePackageDeserializedHandler GuidancePackageDeserialized;
        
        /// /// <remarks/>
        public event HostDeserializedHandler HostDeserialized;
        
        /// /// <remarks/>
        public event SourceLevelsDeserializedHandler SourceLevelsDeserialized;
    }
    
    /// /// <remarks/>
    public delegate void RecipeFrameworkDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework recipeFramework);
    
    /// /// <remarks/>
    public delegate void HostDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host host);
    
    /// /// <remarks/>
    public delegate void SourceLevelsDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels sourceLevels);
    
    /// /// <summary>Custom writer for RecipeFramework instances.</summary>
    internal class RecipeFrameworkWriter : RecipeFrameworkSerializer.BaseWriter
    {
        

		/// <summary>Writes an object of type Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework.</summary>
		internal void Write(Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework value)
		{
			Write6_RecipeFramework(value);
		}
    }
}
#if _DYNAMIC_XMLSERIALIZER_COMPILATION
[assembly:System.Security.AllowPartiallyTrustedCallers()]
[assembly:System.Security.SecurityTransparent()]
#endif

namespace Microsoft.Practices.RecipeFramework.Configuration.Manifest {

    internal partial class RecipeFrameworkSerializer {
	internal class BaseWriter : System.Xml.Serialization.XmlSerializationWriter {

        protected internal void Write6_RecipeFramework(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"RecipeFramework", @"http://schemas.microsoft.com/pag/gax-manifest");
                return;
            }
            TopLevelElement();
            Write5_RecipeFramework(@"RecipeFramework", @"http://schemas.microsoft.com/pag/gax-manifest", ((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework)o), false, false);
        }

        void Write5_RecipeFramework(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-manifest");
            WriteAttribute(@"SchemaVersion", @"", ((global::System.String)o.@SchemaVersion));
            if (((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels)o.@SourceLevels) != global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Error) {
                WriteAttribute(@"SourceLevels", @"", Write4_SourceLevels(((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels)o.@SourceLevels)));
            }
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host[])((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host[])o.@Hosts);
                if (a != null){
                    WriteStartElement(@"Hosts", @"http://schemas.microsoft.com/pag/gax-manifest", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write2_Host(@"Host", @"http://schemas.microsoft.com/pag/gax-manifest", ((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage[])((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage[])o.@GuidancePackages);
                if (a != null){
                    WriteStartElement(@"GuidancePackages", @"http://schemas.microsoft.com/pag/gax-manifest", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write3_GuidancePackage(@"GuidancePackage", @"http://schemas.microsoft.com/pag/gax-manifest", ((global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            WriteEndElement(o);
        }

        void Write3_GuidancePackage(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-manifest");
            WriteAttribute(@"Name", @"", FromXmlNCName(((global::System.String)o.@Name)));
            WriteAttribute(@"Caption", @"", ((global::System.String)o.@Caption));
            WriteAttribute(@"Description", @"", ((global::System.String)o.@Description));
            WriteAttribute(@"Version", @"", ((global::System.String)o.@Version));
            if (((global::System.String)o.@Host) != @"VisualStudio") {
                WriteAttribute(@"Host", @"", ((global::System.String)o.@Host));
            }
            WriteAttribute(@"Guid", @"", ((global::System.String)o.@Guid));
            WriteAttribute(@"ConfigurationFile", @"", ((global::System.String)o.@ConfigurationFile));
            WriteAttribute(@"Timestamp", @"", ((global::System.String)o.@Timestamp));
            WriteEndElement(o);
        }

        void Write2_Host(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-manifest");
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
            WriteAttribute(@"InstallerType", @"", ((global::System.String)o.@InstallerType));
            WriteEndElement(o);
        }

        string Write4_SourceLevels(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels v) {
            string s = null;
            switch (v) {
                case global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Error: s = @"Error"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Information: s = @"Information"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Off: s = @"Off"; break;
				case global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Critical: s = @"Critical"; break;
				case global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Warning: s = @"Warning"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Verbose: s = @"Verbose"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels");
            }
            return s;
        }

        protected override void InitCallbacks() {
        }
    }
	}

    internal partial class RecipeFrameworkSerializer {
	internal class BaseReader : System.Xml.Serialization.XmlSerializationReader {

        protected internal object Read6_RecipeFramework() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id1_RecipeFramework && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read5_RecipeFramework(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-manifest:RecipeFramework");
            }
            return (object)o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework Read5_RecipeFramework(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework();
            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host[] a_0 = null;
            int ca_0 = 0;
            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage[] a_1 = null;
            int ca_1 = 0;
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id4_SchemaVersion && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@SchemaVersion = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id5_SourceLevels && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@SourceLevels = Read4_SourceLevels(Reader.Value);
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":SchemaVersion, :SourceLevels");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id6_Hosts && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host[] a_0_0 = null;
                            int ca_0_0 = 0;
                            if ((Reader.IsEmptyElement)) {
                                Reader.Skip();
                            }
                            else {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations1 = 0;
                                int readerCount1 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                        if (((object) Reader.LocalName == (object)id7_Host && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_0_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host[])EnsureArrayIndex(a_0_0, ca_0_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host));a_0_0[ca_0_0++] = Read2_Host(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-manifest:Host");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-manifest:Host");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations1, ref readerCount1);
                                }
                            ReadEndElement();
                            }
                            o.@Hosts = (global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host[])ShrinkArray(a_0_0, ca_0_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host), false);
                        }
                    }
                    else if (((object) Reader.LocalName == (object)id8_GuidancePackages && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage[] a_1_0 = null;
                            int ca_1_0 = 0;
                            if ((Reader.IsEmptyElement)) {
                                Reader.Skip();
                            }
                            else {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations2 = 0;
                                int readerCount2 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                        if (((object) Reader.LocalName == (object)id9_GuidancePackage && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_1_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage[])EnsureArrayIndex(a_1_0, ca_1_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage));a_1_0[ca_1_0++] = Read3_GuidancePackage(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-manifest:GuidancePackage");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-manifest:GuidancePackage");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations2, ref readerCount2);
                                }
                            ReadEndElement();
                            }
                            o.@GuidancePackages = (global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage[])ShrinkArray(a_1_0, ca_1_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage), false);
                        }
                    }
                    else {
                        UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-manifest:Hosts, http://schemas.microsoft.com/pag/gax-manifest:GuidancePackages");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-manifest:Hosts, http://schemas.microsoft.com/pag/gax-manifest:GuidancePackages");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage Read3_GuidancePackage(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.GuidancePackage();
            bool[] paramsRead = new bool[8];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id10_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = ToXmlNCName(Reader.Value);
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id11_Caption && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Caption = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id12_Description && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Description = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id13_Version && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Version = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id7_Host && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Host = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id14_Guid && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Guid = Reader.Value;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id15_ConfigurationFile && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@ConfigurationFile = Reader.Value;
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id16_Timestamp && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Timestamp = Reader.Value;
                    paramsRead[7] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Name, :Caption, :Description, :Version, :Host, :Guid, :ConfigurationFile, :Timestamp");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations3 = 0;
            int readerCount3 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations3, ref readerCount3);
            }
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host Read2_Host(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.Host();
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id10_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id17_InstallerType && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@InstallerType = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Name, :InstallerType");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations4 = 0;
            int readerCount4 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations4, ref readerCount4);
            }
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels Read4_SourceLevels(string s) {
            switch (s) {
                case @"Error": return global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Error;
                case @"Information": return global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Information;
                case @"Off": return global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Off;
				case @"Critical": return global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Critical;
				case @"Warning": return global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Warning;
                case @"Verbose": return global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels.@Verbose;
                default: throw CreateUnknownConstantException(s, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.SourceLevels));
            }
        }

        protected override void InitCallbacks() {
        }

        string id10_Name;
        string id13_Version;
        string id5_SourceLevels;
        string id15_ConfigurationFile;
        string id1_RecipeFramework;
        string id16_Timestamp;
        string id7_Host;
        string id3_Item;
        string id12_Description;
        string id6_Hosts;
        string id11_Caption;
        string id8_GuidancePackages;
        string id4_SchemaVersion;
        string id14_Guid;
        string id2_Item;
        string id9_GuidancePackage;
        string id17_InstallerType;

        protected override void InitIDs() {
            id10_Name = Reader.NameTable.Add(@"Name");
            id13_Version = Reader.NameTable.Add(@"Version");
            id5_SourceLevels = Reader.NameTable.Add(@"SourceLevels");
            id15_ConfigurationFile = Reader.NameTable.Add(@"ConfigurationFile");
            id1_RecipeFramework = Reader.NameTable.Add(@"RecipeFramework");
            id16_Timestamp = Reader.NameTable.Add(@"Timestamp");
            id7_Host = Reader.NameTable.Add(@"Host");
            id3_Item = Reader.NameTable.Add(@"");
            id12_Description = Reader.NameTable.Add(@"Description");
            id6_Hosts = Reader.NameTable.Add(@"Hosts");
            id11_Caption = Reader.NameTable.Add(@"Caption");
            id8_GuidancePackages = Reader.NameTable.Add(@"GuidancePackages");
            id4_SchemaVersion = Reader.NameTable.Add(@"SchemaVersion");
            id14_Guid = Reader.NameTable.Add(@"Guid");
            id2_Item = Reader.NameTable.Add(@"http://schemas.microsoft.com/pag/gax-manifest");
            id9_GuidancePackage = Reader.NameTable.Add(@"GuidancePackage");
            id17_InstallerType = Reader.NameTable.Add(@"InstallerType");
        }
    }
	}

    internal abstract class RecipeFrameworkSerializerBase : System.Xml.Serialization.XmlSerializer {
        protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
            return new RecipeFrameworkSerializer.BaseReader();
        }
        protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
            return new RecipeFrameworkSerializer.BaseWriter();
        }
    }

    internal sealed partial class RecipeFrameworkSerializer : RecipeFrameworkSerializerBase {

		RecipeFrameworkReader _reader;
		RecipeFrameworkWriter _writer;

		/// <summary>Constructs the serializer.</summary>
		public RecipeFrameworkSerializer()
		{
		}

		/// <summary>Constructs the serializer with a pre-built reader.</summary>
		public RecipeFrameworkSerializer(RecipeFrameworkReader reader)
		{
			_reader = reader;
		}

		/// <summary>Constructs the serializer with a pre-built writer.</summary>
		public RecipeFrameworkSerializer(RecipeFrameworkWriter writer)
		{
			_writer = writer;
		}

		/// <summary>Constructs the serializer with pre-built reader and writer.</summary>
		public RecipeFrameworkSerializer(RecipeFrameworkReader reader, RecipeFrameworkWriter writer)
		{
			_reader = reader;
			_writer = writer;
		}

		/// <remarks/>
		protected override System.Xml.Serialization.XmlSerializationReader CreateReader()
		{
			if (_reader != null) return _reader;
			
			return base.CreateReader();
		}

		/// <remarks/>
		protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter()
		{
			if (_writer != null) return _writer;
			
			return base.CreateWriter();
		}

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"RecipeFramework", @"http://schemas.microsoft.com/pag/gax-manifest");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((BaseWriter)writer).Write6_RecipeFramework(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((BaseReader)reader).Read6_RecipeFramework();
        }
    }

    internal partial class RecipeFrameworkSerializer {
	internal class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
        public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new RecipeFrameworkSerializer.BaseReader(); } }
        public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new RecipeFrameworkSerializer.BaseWriter(); } }
        System.Collections.Hashtable readMethods = null;
        public override System.Collections.Hashtable ReadMethods {
            get {
                if (readMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework:http://schemas.microsoft.com/pag/gax-manifest::False:"] = @"Read6_RecipeFramework";
                    if (readMethods == null) readMethods = _tmp;
                }
                return readMethods;
            }
        }
        System.Collections.Hashtable writeMethods = null;
        public override System.Collections.Hashtable WriteMethods {
            get {
                if (writeMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework:http://schemas.microsoft.com/pag/gax-manifest::False:"] = @"Write6_RecipeFramework";
                    if (writeMethods == null) writeMethods = _tmp;
                }
                return writeMethods;
            }
        }
        System.Collections.Hashtable typedSerializers = null;
        public override System.Collections.Hashtable TypedSerializers {
            get {
                if (typedSerializers == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp.Add(@"Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework:http://schemas.microsoft.com/pag/gax-manifest::False:", new RecipeFrameworkSerializer());
                    if (typedSerializers == null) typedSerializers = _tmp;
                }
                return typedSerializers;
            }
        }
        public override System.Boolean CanSerialize(System.Type type) {
            if (type == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework)) return true;
            return false;
        }
        public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
            if (type == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Manifest.RecipeFramework)) return new RecipeFrameworkSerializer();
            return null;
        }
    }
	}
}


#pragma warning restore 0642, 0219
