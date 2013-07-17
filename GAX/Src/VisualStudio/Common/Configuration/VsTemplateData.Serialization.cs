#region Original XmlSerializer code
/* -------------------------------------------------------------------------------------
#if _DYNAMIC_XMLSERIALIZER_COMPILATION
[assembly:System.Security.AllowPartiallyTrustedCallers()]
[assembly:System.Security.SecurityTransparent()]
#endif
[assembly:System.Reflection.AssemblyVersionAttribute("1.0.60429.0")]
namespace Microsoft.Xml.Serialization.GeneratedAssembly {

    public class XmlSerializationWriterTemplate : System.Xml.Serialization.XmlSerializationWriter {

        public void Write6_Template(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"Template", @"http://schemas.microsoft.com/pag/gax-template");
                return;
            }
            TopLevelElement();
            Write5_Template(@"Template", @"http://schemas.microsoft.com/pag/gax-template", ((global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template)o), false, false);
        }

        void Write5_Template(string n, string ns, global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-template");
            WriteAttribute(@"Recipe", @"", FromXmlNCName(((global::System.String)o.@Recipe)));
            WriteAttribute(@"SchemaVersion", @"", ((global::System.String)o.@SchemaVersion));
            Write4_References(@"References", @"http://schemas.microsoft.com/pag/gax-template", ((global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References)o.@References), false, false);
            {
                global::System.Xml.XmlElement[] a = (global::System.Xml.XmlElement[])o.@Any;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        if ((a[ia]) is System.Xml.XmlNode || a[ia] == null) {
                            WriteElementLiteral((System.Xml.XmlNode)a[ia], @"", null, false, true);
                        }
                        else {
                            throw CreateInvalidAnyTypeException(a[ia]);
                        }
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write4_References(string n, string ns, global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-template");
            {
                global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[] a = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])o.@RecipeReference;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write3_AssetReference(@"RecipeReference", @"http://schemas.microsoft.com/pag/gax-template", ((global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference)a[ia]), false, false);
                    }
                }
            }
            {
                global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[] a = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])o.@TemplateReference;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write3_AssetReference(@"TemplateReference", @"http://schemas.microsoft.com/pag/gax-template", ((global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write3_AssetReference(string n, string ns, global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AssetReference", @"http://schemas.microsoft.com/pag/gax-template");
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
            WriteAttribute(@"Target", @"", ((global::System.String)o.@Target));
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            {
                global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[] a = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])((global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])o.@InitialState);
                if (a != null){
                    WriteStartElement(@"InitialState", @"http://schemas.microsoft.com/pag/gax-template", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write2_StateEntry(@"Entry", @"http://schemas.microsoft.com/pag/gax-template", ((global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::System.Xml.XmlElement[] a = (global::System.Xml.XmlElement[])o.@Any;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        if ((a[ia]) is System.Xml.XmlNode || a[ia] == null) {
                            WriteElementLiteral((System.Xml.XmlNode)a[ia], @"", null, false, true);
                        }
                        else {
                            throw CreateInvalidAnyTypeException(a[ia]);
                        }
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write2_StateEntry(string n, string ns, global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"StateEntry", @"http://schemas.microsoft.com/pag/gax-template");
            Write1_Object(@"Key", @"http://schemas.microsoft.com/pag/gax-template", ((global::System.Object)o.@Key), false, false);
            Write1_Object(@"Value", @"http://schemas.microsoft.com/pag/gax-template", ((global::System.Object)o.@Value), false, false);
            WriteEndElement(o);
        }

        void Write1_Object(string n, string ns, global::System.Object o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::System.Object)) {
                }
                else if (t == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference)) {
                    Write3_AssetReference(n, ns,(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference)o, isNullable, true);
                    return;
                }
                else if (t == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry)) {
                    Write2_StateEntry(n, ns,(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry)o, isNullable, true);
                    return;
                }
                else if (t == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])) {
                    Writer.WriteStartElement(n, ns);
                    WriteXsiType(@"ArrayOfStateEntry", @"http://schemas.microsoft.com/pag/gax-template");
                    {
                        global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[] a = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])o;
                        if (a != null) {
                            for (int ia = 0; ia < a.Length; ia++) {
                                Write2_StateEntry(@"Entry", @"http://schemas.microsoft.com/pag/gax-template", ((global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry)a[ia]), false, false);
                            }
                        }
                    }
                    Writer.WriteEndElement();
                    return;
                }
                else {
                    WriteTypedPrimitive(n, ns, o, true);
                    return;
                }
            }
            WriteStartElement(n, ns, o, false, null);
            WriteEndElement(o);
        }

        protected override void InitCallbacks() {
        }
    }

    public class XmlSerializationReaderTemplate : System.Xml.Serialization.XmlSerializationReader {

        public object Read6_Template() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id1_Template && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read5_Template(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-template:Template");
            }
            return (object)o;
        }

        global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template Read5_Template(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template o;
            o = new global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template();
            global::System.Xml.XmlElement[] a_1 = null;
            int ca_1 = 0;
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id4_Recipe && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Recipe = ToXmlNCName(Reader.Value);
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id5_SchemaVersion && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@SchemaVersion = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Recipe, :SchemaVersion");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@Any = (global::System.Xml.XmlElement[])ShrinkArray(a_1, ca_1, typeof(global::System.Xml.XmlElement), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id6_References && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@References = Read4_References(false, true);
                        paramsRead[0] = true;
                    }
                    else {
                        a_1 = (global::System.Xml.XmlElement[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Xml.XmlElement));a_1[ca_1++] = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-template:References");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            o.@Any = (global::System.Xml.XmlElement[])ShrinkArray(a_1, ca_1, typeof(global::System.Xml.XmlElement), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References Read4_References(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References o;
            o = new global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References();
            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[] a_0 = null;
            int ca_0 = 0;
            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[] a_1 = null;
            int ca_1 = 0;
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@RecipeReference = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference), true);
                o.@TemplateReference = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations1 = 0;
            int readerCount1 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id7_RecipeReference && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_0 = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])EnsureArrayIndex(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference));a_0[ca_0++] = Read3_AssetReference(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id8_TemplateReference && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])EnsureArrayIndex(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference));a_1[ca_1++] = Read3_AssetReference(false, true);
                    }
                    else {
                        UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-template:RecipeReference, http://schemas.microsoft.com/pag/gax-template:TemplateReference");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-template:RecipeReference, http://schemas.microsoft.com/pag/gax-template:TemplateReference");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations1, ref readerCount1);
            }
            o.@RecipeReference = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference), true);
            o.@TemplateReference = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference Read3_AssetReference(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id9_AssetReference && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference o;
            o = new global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference();
            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[] a_0 = null;
            int ca_0 = 0;
            global::System.Xml.XmlElement[] a_1 = null;
            int ca_1 = 0;
            global::System.Xml.XmlAttribute[] a_4 = null;
            int ca_4 = 0;
            bool[] paramsRead = new bool[5];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id10_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id11_Target && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Target = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_4 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_4, ca_4, typeof(global::System.Xml.XmlAttribute));a_4[ca_4++] = attr;
                }
            }
            o.@InitialState = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry), true);
            o.@Any = (global::System.Xml.XmlElement[])ShrinkArray(a_1, ca_1, typeof(global::System.Xml.XmlElement), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
                o.@Any = (global::System.Xml.XmlElement[])ShrinkArray(a_1, ca_1, typeof(global::System.Xml.XmlElement), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations2 = 0;
            int readerCount2 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id12_InitialState && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[] a_0_0 = null;
                            int ca_0_0 = 0;
                            if ((Reader.IsEmptyElement)) {
                                Reader.Skip();
                            }
                            else {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations3 = 0;
                                int readerCount3 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                        if (((object) Reader.LocalName == (object)id13_Entry && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_0_0 = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])EnsureArrayIndex(a_0_0, ca_0_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry));a_0_0[ca_0_0++] = Read2_StateEntry(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-template:Entry");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-template:Entry");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations3, ref readerCount3);
                                }
                            ReadEndElement();
                            }
                            o.@InitialState = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])ShrinkArray(a_0_0, ca_0_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry), false);
                        }
                    }
                    else {
                        a_1 = (global::System.Xml.XmlElement[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Xml.XmlElement));a_1[ca_1++] = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-template:InitialState");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations2, ref readerCount2);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            o.@Any = (global::System.Xml.XmlElement[])ShrinkArray(a_1, ca_1, typeof(global::System.Xml.XmlElement), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry Read2_StateEntry(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id14_StateEntry && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry o;
            o = new global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry();
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
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
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id15_Key && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@Key = Read1_Object(false, true);
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id16_Value && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@Value = Read1_Object(false, true);
                        paramsRead[1] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-template:Key, http://schemas.microsoft.com/pag/gax-template:Value");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-template:Key, http://schemas.microsoft.com/pag/gax-template:Value");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations4, ref readerCount4);
            }
            ReadEndElement();
            return o;
        }

        global::System.Object Read1_Object(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
                if (isNull) {
                    if (xsiType != null) return (global::System.Object)ReadTypedNull(xsiType);
                    else return null;
                }
                if (xsiType == null) {
                    return ReadTypedPrimitive(new System.Xml.XmlQualifiedName("anyType", "http://www.w3.org/2001/XMLSchema"));
                }
                else if (((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id9_AssetReference && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                    return Read3_AssetReference(isNullable, false);
                else if (((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id14_StateEntry && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                    return Read2_StateEntry(isNullable, false);
                else if (((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id17_ArrayOfStateEntry && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
                    global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[] a = null;
                    if (!ReadNull()) {
                        global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[] z_0_0 = null;
                        int cz_0_0 = 0;
                        if ((Reader.IsEmptyElement)) {
                            Reader.Skip();
                        }
                        else {
                            Reader.ReadStartElement();
                            Reader.MoveToContent();
                            int whileIterations5 = 0;
                            int readerCount5 = ReaderCount;
                            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                    if (((object) Reader.LocalName == (object)id13_Entry && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                        z_0_0 = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])EnsureArrayIndex(z_0_0, cz_0_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry));z_0_0[cz_0_0++] = Read2_StateEntry(false, true);
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-template:Entry");
                                    }
                                }
                                else {
                                    UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-template:Entry");
                                }
                                Reader.MoveToContent();
                                CheckReaderCount(ref whileIterations5, ref readerCount5);
                            }
                        ReadEndElement();
                        }
                        a = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])ShrinkArray(z_0_0, cz_0_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry), false);
                    }
                    return a;
                }
                else
                    return ReadTypedPrimitive((System.Xml.XmlQualifiedName)xsiType);
                }
                if (isNull) return null;
                global::System.Object o;
                o = new global::System.Object();
                bool[] paramsRead = new bool[0];
                while (Reader.MoveToNextAttribute()) {
                    if (!IsXmlnsAttribute(Reader.Name)) {
                        UnknownNode((object)o);
                    }
                }
                Reader.MoveToElement();
                if (Reader.IsEmptyElement) {
                    Reader.Skip();
                    return o;
                }
                Reader.ReadStartElement();
                Reader.MoveToContent();
                int whileIterations6 = 0;
                int readerCount6 = ReaderCount;
                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                    if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                        UnknownNode((object)o, @"");
                    }
                    else {
                        UnknownNode((object)o, @"");
                    }
                    Reader.MoveToContent();
                    CheckReaderCount(ref whileIterations6, ref readerCount6);
                }
                ReadEndElement();
                return o;
            }

            protected override void InitCallbacks() {
            }

            string id9_AssetReference;
            string id7_RecipeReference;
            string id15_Key;
            string id13_Entry;
            string id1_Template;
            string id16_Value;
            string id3_Item;
            string id14_StateEntry;
            string id2_Item;
            string id4_Recipe;
            string id10_Name;
            string id8_TemplateReference;
            string id11_Target;
            string id17_ArrayOfStateEntry;
            string id5_SchemaVersion;
            string id6_References;
            string id12_InitialState;

            protected override void InitIDs() {
                id9_AssetReference = Reader.NameTable.Add(@"AssetReference");
                id7_RecipeReference = Reader.NameTable.Add(@"RecipeReference");
                id15_Key = Reader.NameTable.Add(@"Key");
                id13_Entry = Reader.NameTable.Add(@"Entry");
                id1_Template = Reader.NameTable.Add(@"Template");
                id16_Value = Reader.NameTable.Add(@"Value");
                id3_Item = Reader.NameTable.Add(@"");
                id14_StateEntry = Reader.NameTable.Add(@"StateEntry");
                id2_Item = Reader.NameTable.Add(@"http://schemas.microsoft.com/pag/gax-template");
                id4_Recipe = Reader.NameTable.Add(@"Recipe");
                id10_Name = Reader.NameTable.Add(@"Name");
                id8_TemplateReference = Reader.NameTable.Add(@"TemplateReference");
                id11_Target = Reader.NameTable.Add(@"Target");
                id17_ArrayOfStateEntry = Reader.NameTable.Add(@"ArrayOfStateEntry");
                id5_SchemaVersion = Reader.NameTable.Add(@"SchemaVersion");
                id6_References = Reader.NameTable.Add(@"References");
                id12_InitialState = Reader.NameTable.Add(@"InitialState");
            }
        }

        public abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer {
            protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
                return new XmlSerializationReaderTemplate();
            }
            protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
                return new XmlSerializationWriterTemplate();
            }
        }

        public sealed class TemplateSerializer : XmlSerializer1 {

            public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
                return xmlReader.IsStartElement(@"Template", @"http://schemas.microsoft.com/pag/gax-template");
            }

            protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
                ((XmlSerializationWriterTemplate)writer).Write6_Template(objectToSerialize);
            }

            protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
                return ((XmlSerializationReaderTemplate)reader).Read6_Template();
            }
        }

        public class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
            public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new XmlSerializationReaderTemplate(); } }
            public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new XmlSerializationWriterTemplate(); } }
            System.Collections.Hashtable readMethods = null;
            public override System.Collections.Hashtable ReadMethods {
                get {
                    if (readMethods == null) {
                        System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                        _tmp[@"Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template:http://schemas.microsoft.com/pag/gax-template::False:"] = @"Read6_Template";
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
                        _tmp[@"Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template:http://schemas.microsoft.com/pag/gax-template::False:"] = @"Write6_Template";
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
                        _tmp.Add(@"Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template:http://schemas.microsoft.com/pag/gax-template::False:", new TemplateSerializer());
                        if (typedSerializers == null) typedSerializers = _tmp;
                    }
                    return typedSerializers;
                }
            }
            public override System.Boolean CanSerialize(System.Type type) {
                if (type == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template)) return true;
                return false;
            }
            public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
                if (type == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template)) return new TemplateSerializer();
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
namespace Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate
{
    using System.Xml.Serialization;
    using System;
    
    
    /// /// <summary>Custom reader for Template instances.</summary>
    internal class TemplateReader : TemplateSerializer.BaseReader
    {
        

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template Read5_Template(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template obj = base.Read5_Template(isNullable, checkType);
			TemplateDeserializedHandler handler = TemplateDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References Read4_References(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References obj = base.Read4_References(isNullable, checkType);
			ReferencesDeserializedHandler handler = ReferencesDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference Read3_AssetReference(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference obj = base.Read3_AssetReference(isNullable, checkType);
			AssetReferenceDeserializedHandler handler = AssetReferenceDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry Read2_StateEntry(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry obj = base.Read2_StateEntry(isNullable, checkType);
			StateEntryDeserializedHandler handler = StateEntryDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <summary>Reads an object of type Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template.</summary>
		internal Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template Read()
		{
			return (Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template) Read6_Template();
		}
        
        /// /// <remarks/>
        public event TemplateDeserializedHandler TemplateDeserialized;
        
        /// /// <remarks/>
        public event ReferencesDeserializedHandler ReferencesDeserialized;
        
        /// /// <remarks/>
        public event AssetReferenceDeserializedHandler AssetReferenceDeserialized;
        
        /// /// <remarks/>
        public event StateEntryDeserializedHandler StateEntryDeserialized;
    }
    
    /// /// <remarks/>
    public delegate void TemplateDeserializedHandler(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template template);
    
    /// /// <remarks/>
    public delegate void ReferencesDeserializedHandler(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References references);
    
    /// /// <remarks/>
    public delegate void AssetReferenceDeserializedHandler(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference assetReference);
    
    /// /// <remarks/>
    public delegate void StateEntryDeserializedHandler(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry stateEntry);
    
    /// /// <summary>Custom writer for Template instances.</summary>
    internal class TemplateWriter : TemplateSerializer.BaseWriter
    {
        

		/// <summary>Writes an object of type Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template.</summary>
		internal void Write(Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template value)
		{
			Write6_Template(value);
		}
    }
}
#if _DYNAMIC_XMLSERIALIZER_COMPILATION
[assembly:System.Security.AllowPartiallyTrustedCallers()]
[assembly:System.Security.SecurityTransparent()]
#endif

namespace Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate {

    internal partial class TemplateSerializer {
	internal class BaseWriter : System.Xml.Serialization.XmlSerializationWriter {

        protected internal void Write6_Template(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"Template", @"http://schemas.microsoft.com/pag/gax-template");
                return;
            }
            TopLevelElement();
            Write5_Template(@"Template", @"http://schemas.microsoft.com/pag/gax-template", ((global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template)o), false, false);
        }

        void Write5_Template(string n, string ns, global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-template");
            WriteAttribute(@"Recipe", @"", FromXmlNCName(((global::System.String)o.@Recipe)));
            WriteAttribute(@"SchemaVersion", @"", ((global::System.String)o.@SchemaVersion));
            Write4_References(@"References", @"http://schemas.microsoft.com/pag/gax-template", ((global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References)o.@References), false, false);
            {
                global::System.Xml.XmlElement[] a = (global::System.Xml.XmlElement[])o.@Any;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        if ((a[ia]) is System.Xml.XmlNode || a[ia] == null) {
                            WriteElementLiteral((System.Xml.XmlNode)a[ia], @"", null, false, true);
                        }
                        else {
                            throw CreateInvalidAnyTypeException(a[ia]);
                        }
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write4_References(string n, string ns, global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-template");
            {
                global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[] a = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])o.@RecipeReference;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write3_AssetReference(@"RecipeReference", @"http://schemas.microsoft.com/pag/gax-template", ((global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference)a[ia]), false, false);
                    }
                }
            }
            {
                global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[] a = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])o.@TemplateReference;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write3_AssetReference(@"TemplateReference", @"http://schemas.microsoft.com/pag/gax-template", ((global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write3_AssetReference(string n, string ns, global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"AssetReference", @"http://schemas.microsoft.com/pag/gax-template");
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
            WriteAttribute(@"Target", @"", ((global::System.String)o.@Target));
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            {
                global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[] a = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])((global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])o.@InitialState);
                if (a != null){
                    WriteStartElement(@"InitialState", @"http://schemas.microsoft.com/pag/gax-template", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write2_StateEntry(@"Entry", @"http://schemas.microsoft.com/pag/gax-template", ((global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::System.Xml.XmlElement[] a = (global::System.Xml.XmlElement[])o.@Any;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        if ((a[ia]) is System.Xml.XmlNode || a[ia] == null) {
                            WriteElementLiteral((System.Xml.XmlNode)a[ia], @"", null, false, true);
                        }
                        else {
                            throw CreateInvalidAnyTypeException(a[ia]);
                        }
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write2_StateEntry(string n, string ns, global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"StateEntry", @"http://schemas.microsoft.com/pag/gax-template");
            Write1_Object(@"Key", @"http://schemas.microsoft.com/pag/gax-template", ((global::System.Object)o.@Key), false, false);
            Write1_Object(@"Value", @"http://schemas.microsoft.com/pag/gax-template", ((global::System.Object)o.@Value), false, false);
            WriteEndElement(o);
        }

        void Write1_Object(string n, string ns, global::System.Object o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::System.Object)) {
                }
                else if (t == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference)) {
                    Write3_AssetReference(n, ns,(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference)o, isNullable, true);
                    return;
                }
                else if (t == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry)) {
                    Write2_StateEntry(n, ns,(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry)o, isNullable, true);
                    return;
                }
                else if (t == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])) {
                    Writer.WriteStartElement(n, ns);
                    WriteXsiType(@"ArrayOfStateEntry", @"http://schemas.microsoft.com/pag/gax-template");
                    {
                        global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[] a = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])o;
                        if (a != null) {
                            for (int ia = 0; ia < a.Length; ia++) {
                                Write2_StateEntry(@"Entry", @"http://schemas.microsoft.com/pag/gax-template", ((global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry)a[ia]), false, false);
                            }
                        }
                    }
                    Writer.WriteEndElement();
                    return;
                }
                else {
                    WriteTypedPrimitive(n, ns, o, true);
                    return;
                }
            }
            WriteStartElement(n, ns, o, false, null);
            WriteEndElement(o);
        }

        protected override void InitCallbacks() {
        }
    }
	}

    internal partial class TemplateSerializer {
	internal class BaseReader : System.Xml.Serialization.XmlSerializationReader {

        protected internal object Read6_Template() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id1_Template && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read5_Template(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-template:Template");
            }
            return (object)o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template Read5_Template(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template o;
            o = new global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template();
            global::System.Xml.XmlElement[] a_1 = null;
            int ca_1 = 0;
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id4_Recipe && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Recipe = ToXmlNCName(Reader.Value);
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id5_SchemaVersion && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@SchemaVersion = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Recipe, :SchemaVersion");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@Any = (global::System.Xml.XmlElement[])ShrinkArray(a_1, ca_1, typeof(global::System.Xml.XmlElement), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id6_References && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@References = Read4_References(false, true);
                        paramsRead[0] = true;
                    }
                    else {
                        a_1 = (global::System.Xml.XmlElement[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Xml.XmlElement));a_1[ca_1++] = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-template:References");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            o.@Any = (global::System.Xml.XmlElement[])ShrinkArray(a_1, ca_1, typeof(global::System.Xml.XmlElement), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References Read4_References(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References o;
            o = new global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.References();
            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[] a_0 = null;
            int ca_0 = 0;
            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[] a_1 = null;
            int ca_1 = 0;
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@RecipeReference = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference), true);
                o.@TemplateReference = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations1 = 0;
            int readerCount1 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id7_RecipeReference && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_0 = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])EnsureArrayIndex(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference));a_0[ca_0++] = Read3_AssetReference(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id8_TemplateReference && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])EnsureArrayIndex(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference));a_1[ca_1++] = Read3_AssetReference(false, true);
                    }
                    else {
                        UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-template:RecipeReference, http://schemas.microsoft.com/pag/gax-template:TemplateReference");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-template:RecipeReference, http://schemas.microsoft.com/pag/gax-template:TemplateReference");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations1, ref readerCount1);
            }
            o.@RecipeReference = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference), true);
            o.@TemplateReference = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference Read3_AssetReference(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id9_AssetReference && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference o;
            o = new global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.AssetReference();
            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[] a_0 = null;
            int ca_0 = 0;
            global::System.Xml.XmlElement[] a_1 = null;
            int ca_1 = 0;
            global::System.Xml.XmlAttribute[] a_4 = null;
            int ca_4 = 0;
            bool[] paramsRead = new bool[5];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id10_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id11_Target && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Target = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_4 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_4, ca_4, typeof(global::System.Xml.XmlAttribute));a_4[ca_4++] = attr;
                }
            }
            o.@InitialState = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry), true);
            o.@Any = (global::System.Xml.XmlElement[])ShrinkArray(a_1, ca_1, typeof(global::System.Xml.XmlElement), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
                o.@Any = (global::System.Xml.XmlElement[])ShrinkArray(a_1, ca_1, typeof(global::System.Xml.XmlElement), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations2 = 0;
            int readerCount2 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id12_InitialState && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[] a_0_0 = null;
                            int ca_0_0 = 0;
                            if ((Reader.IsEmptyElement)) {
                                Reader.Skip();
                            }
                            else {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations3 = 0;
                                int readerCount3 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                        if (((object) Reader.LocalName == (object)id13_Entry && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_0_0 = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])EnsureArrayIndex(a_0_0, ca_0_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry));a_0_0[ca_0_0++] = Read2_StateEntry(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-template:Entry");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-template:Entry");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations3, ref readerCount3);
                                }
                            ReadEndElement();
                            }
                            o.@InitialState = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])ShrinkArray(a_0_0, ca_0_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry), false);
                        }
                    }
                    else {
                        a_1 = (global::System.Xml.XmlElement[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Xml.XmlElement));a_1[ca_1++] = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-template:InitialState");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations2, ref readerCount2);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            o.@Any = (global::System.Xml.XmlElement[])ShrinkArray(a_1, ca_1, typeof(global::System.Xml.XmlElement), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry Read2_StateEntry(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id14_StateEntry && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry o;
            o = new global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry();
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
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
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id15_Key && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@Key = Read1_Object(false, true);
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id16_Value && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@Value = Read1_Object(false, true);
                        paramsRead[1] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-template:Key, http://schemas.microsoft.com/pag/gax-template:Value");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-template:Key, http://schemas.microsoft.com/pag/gax-template:Value");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations4, ref readerCount4);
            }
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::System.Object Read1_Object(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
                if (isNull) {
                    if (xsiType != null) return (global::System.Object)ReadTypedNull(xsiType);
                    else return null;
                }
                if (xsiType == null) {
                    return ReadTypedPrimitive(new System.Xml.XmlQualifiedName("anyType", "http://www.w3.org/2001/XMLSchema"));
                }
                else if (((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id9_AssetReference && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                    return Read3_AssetReference(isNullable, false);
                else if (((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id14_StateEntry && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                    return Read2_StateEntry(isNullable, false);
                else if (((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id17_ArrayOfStateEntry && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
                    global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[] a = null;
                    if (!ReadNull()) {
                        global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[] z_0_0 = null;
                        int cz_0_0 = 0;
                        if ((Reader.IsEmptyElement)) {
                            Reader.Skip();
                        }
                        else {
                            Reader.ReadStartElement();
                            Reader.MoveToContent();
                            int whileIterations5 = 0;
                            int readerCount5 = ReaderCount;
                            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                    if (((object) Reader.LocalName == (object)id13_Entry && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                        z_0_0 = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])EnsureArrayIndex(z_0_0, cz_0_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry));z_0_0[cz_0_0++] = Read2_StateEntry(false, true);
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-template:Entry");
                                    }
                                }
                                else {
                                    UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-template:Entry");
                                }
                                Reader.MoveToContent();
                                CheckReaderCount(ref whileIterations5, ref readerCount5);
                            }
                        ReadEndElement();
                        }
                        a = (global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry[])ShrinkArray(z_0_0, cz_0_0, typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.StateEntry), false);
                    }
                    return a;
                }
                else
                    return ReadTypedPrimitive((System.Xml.XmlQualifiedName)xsiType);
                }
                if (isNull) return null;
                global::System.Object o;
                o = new global::System.Object();
                bool[] paramsRead = new bool[0];
                while (Reader.MoveToNextAttribute()) {
                    if (!IsXmlnsAttribute(Reader.Name)) {
                        UnknownNode((object)o);
                    }
                }
                Reader.MoveToElement();
                if (Reader.IsEmptyElement) {
                    Reader.Skip();
                    return o;
                }
                Reader.ReadStartElement();
                Reader.MoveToContent();
                int whileIterations6 = 0;
                int readerCount6 = ReaderCount;
                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                    if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                        UnknownNode((object)o, @"");
                    }
                    else {
                        UnknownNode((object)o, @"");
                    }
                    Reader.MoveToContent();
                    CheckReaderCount(ref whileIterations6, ref readerCount6);
                }
                ReadEndElement();
                return o;
            }

            protected override void InitCallbacks() {
            }

            string id9_AssetReference;
            string id7_RecipeReference;
            string id15_Key;
            string id13_Entry;
            string id1_Template;
            string id16_Value;
            string id3_Item;
            string id14_StateEntry;
            string id2_Item;
            string id4_Recipe;
            string id10_Name;
            string id8_TemplateReference;
            string id11_Target;
            string id17_ArrayOfStateEntry;
            string id5_SchemaVersion;
            string id6_References;
            string id12_InitialState;

            protected override void InitIDs() {
                id9_AssetReference = Reader.NameTable.Add(@"AssetReference");
                id7_RecipeReference = Reader.NameTable.Add(@"RecipeReference");
                id15_Key = Reader.NameTable.Add(@"Key");
                id13_Entry = Reader.NameTable.Add(@"Entry");
                id1_Template = Reader.NameTable.Add(@"Template");
                id16_Value = Reader.NameTable.Add(@"Value");
                id3_Item = Reader.NameTable.Add(@"");
                id14_StateEntry = Reader.NameTable.Add(@"StateEntry");
                id2_Item = Reader.NameTable.Add(@"http://schemas.microsoft.com/pag/gax-template");
                id4_Recipe = Reader.NameTable.Add(@"Recipe");
                id10_Name = Reader.NameTable.Add(@"Name");
                id8_TemplateReference = Reader.NameTable.Add(@"TemplateReference");
                id11_Target = Reader.NameTable.Add(@"Target");
                id17_ArrayOfStateEntry = Reader.NameTable.Add(@"ArrayOfStateEntry");
                id5_SchemaVersion = Reader.NameTable.Add(@"SchemaVersion");
                id6_References = Reader.NameTable.Add(@"References");
                id12_InitialState = Reader.NameTable.Add(@"InitialState");
            }
        }
	}

        internal abstract class TemplateSerializerBase : System.Xml.Serialization.XmlSerializer {
            protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
                return new TemplateSerializer.BaseReader();
            }
            protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
                return new TemplateSerializer.BaseWriter();
            }
        }

        internal sealed partial class TemplateSerializer : TemplateSerializerBase {

		TemplateReader _reader;
		TemplateWriter _writer;

		/// <summary>Constructs the serializer.</summary>
		public TemplateSerializer()
		{
		}

		/// <summary>Constructs the serializer with a pre-built reader.</summary>
		public TemplateSerializer(TemplateReader reader)
		{
			_reader = reader;
		}

		/// <summary>Constructs the serializer with a pre-built writer.</summary>
		public TemplateSerializer(TemplateWriter writer)
		{
			_writer = writer;
		}

		/// <summary>Constructs the serializer with pre-built reader and writer.</summary>
		public TemplateSerializer(TemplateReader reader, TemplateWriter writer)
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
                return xmlReader.IsStartElement(@"Template", @"http://schemas.microsoft.com/pag/gax-template");
            }

            protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
                ((BaseWriter)writer).Write6_Template(objectToSerialize);
            }

            protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
                return ((BaseReader)reader).Read6_Template();
            }
        }

        internal partial class TemplateSerializer {
	internal class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
            public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new TemplateSerializer.BaseReader(); } }
            public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new TemplateSerializer.BaseWriter(); } }
            System.Collections.Hashtable readMethods = null;
            public override System.Collections.Hashtable ReadMethods {
                get {
                    if (readMethods == null) {
                        System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                        _tmp[@"Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template:http://schemas.microsoft.com/pag/gax-template::False:"] = @"Read6_Template";
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
                        _tmp[@"Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template:http://schemas.microsoft.com/pag/gax-template::False:"] = @"Write6_Template";
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
                        _tmp.Add(@"Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template:http://schemas.microsoft.com/pag/gax-template::False:", new TemplateSerializer());
                        if (typedSerializers == null) typedSerializers = _tmp;
                    }
                    return typedSerializers;
                }
            }
            public override System.Boolean CanSerialize(System.Type type) {
                if (type == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template)) return true;
                return false;
            }
            public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
                if (type == typeof(global::Microsoft.Practices.RecipeFramework.VisualStudio.VsTemplate.Template)) return new TemplateSerializer();
                return null;
            }
        }
	}
    }


#pragma warning restore 0642, 0219
