#region Original XmlSerializer code
/* -------------------------------------------------------------------------------------
#if _DYNAMIC_XMLSERIALIZER_COMPILATION
[assembly:System.Security.AllowPartiallyTrustedCallers()]
[assembly:System.Security.SecurityTransparent()]
#endif
[assembly:System.Reflection.AssemblyVersionAttribute("1.3.0.0")]
namespace Microsoft.Xml.Serialization.GeneratedAssembly {

    public class XmlSerializationWriterGuidancePackage : System.Xml.Serialization.XmlSerializationWriter {

        public void Write24_GuidancePackage(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"GuidancePackage", @"http://schemas.microsoft.com/pag/gax-core");
                return;
            }
            TopLevelElement();
            Write23_GuidancePackage(@"GuidancePackage", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage)o), false, false);
        }

        void Write23_GuidancePackage(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", FromXmlNCName(((global::System.String)o.@Name)));
            WriteAttribute(@"Caption", @"", ((global::System.String)o.@Caption));
            WriteAttribute(@"Description", @"", ((global::System.String)o.@Description));
            if (((global::System.String)o.@Version) != @"1.0") {
                WriteAttribute(@"Version", @"", ((global::System.String)o.@Version));
            }
            WriteAttribute(@"Guid", @"", ((global::System.String)o.@Guid));
            WriteAttribute(@"SchemaVersion", @"", ((global::System.String)o.@SchemaVersion));
            if (((global::System.String)o.@Host) != @"VisualStudio") {
                WriteAttribute(@"Host", @"", ((global::System.String)o.@Host));
            }
            WriteAttribute(@"BindingRecipe", @"", FromXmlNCName(((global::System.String)o.@BindingRecipe)));
            if (((global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels)o.@SourceLevels) != global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Error) {
                WriteAttribute(@"SourceLevels", @"", Write22_SourceLevels(((global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels)o.@SourceLevels)));
            }
            if (((global::System.Int32)o.@SortPriority) != 100) {
                WriteAttribute(@"SortPriority", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@SortPriority)));
            }
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            Write2_Overview(@"Overview", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Overview)o.@Overview), false, false);
            Write7_GuidancePackageHostData(@"HostData", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData)o.@HostData), false, false);
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[])((global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[])o.@Recipes);
                if (a != null){
                    WriteStartElement(@"Recipes", @"http://schemas.microsoft.com/pag/gax-core", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write21_Recipe(@"Recipe", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Recipe)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            WriteEndElement(o);
        }

        void Write21_Recipe(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Recipe o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Recipe)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", FromXmlNCName(((global::System.String)o.@Name)));
            if (((global::System.Boolean)o.@Recurrent) != true) {
                WriteAttribute(@"Recurrent", @"", System.Xml.XmlConvert.ToString((global::System.Boolean)((global::System.Boolean)o.@Recurrent)));
            }
            if (((global::System.Boolean)o.@Bound) != true) {
                WriteAttribute(@"Bound", @"", System.Xml.XmlConvert.ToString((global::System.Boolean)((global::System.Boolean)o.@Bound)));
            }
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
                global::Microsoft.Practices.RecipeFramework.Configuration.Link[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Link[])((global::Microsoft.Practices.RecipeFramework.Configuration.Link[])o.@DocumentationLinks);
                if (a != null){
                    WriteStartElement(@"DocumentationLinks", @"http://schemas.microsoft.com/pag/gax-core", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write9_Link(@"Link", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Link)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[])((global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[])o.@Types);
                if (a != null){
                    WriteStartElement(@"Types", @"http://schemas.microsoft.com/pag/gax-core", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write10_TypeAlias(@"TypeAlias", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            WriteElementString(@"Caption", @"http://schemas.microsoft.com/pag/gax-core", ((global::System.String)o.@Caption));
            WriteElementString(@"Description", @"http://schemas.microsoft.com/pag/gax-core", ((global::System.String)o.@Description));
            Write11_RecipeHostData(@"HostData", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData)o.@HostData), false, false);
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.Argument[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Argument[])((global::Microsoft.Practices.RecipeFramework.Configuration.Argument[])o.@Arguments);
                if (a != null){
                    WriteStartElement(@"Arguments", @"http://schemas.microsoft.com/pag/gax-core", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write15_Argument(@"Argument", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Argument)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            Write16_RecipeGatheringServiceData(@"GatheringServiceData", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData)o.@GatheringServiceData), false, false);
            Write20_RecipeActions(@"Actions", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions)o.@Actions), false, false);
            WriteEndElement(o);
        }

        void Write20_RecipeActions(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"CoordinatorServiceType", @"", ((global::System.String)o.@CoordinatorServiceType));
            WriteAttribute(@"ExecutionServiceType", @"", ((global::System.String)o.@ExecutionServiceType));
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
                global::Microsoft.Practices.RecipeFramework.Configuration.Action[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Action[])o.@Action;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write19_Action(@"Action", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Action)a[ia]), false, false);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write19_Action(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Action o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Action)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Type", @"", ((global::System.String)o.@Type));
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
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
                global::Microsoft.Practices.RecipeFramework.Configuration.Input[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Input[])o.@Input;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write17_Input(@"Input", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Input)a[ia]), false, false);
                    }
                }
            }
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.Output[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Output[])o.@Output;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write18_Output(@"Output", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Output)a[ia]), false, false);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write18_Output(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Output o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Output)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write17_Input(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Input o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Input)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
            WriteAttribute(@"RecipeArgument", @"", ((global::System.String)o.@RecipeArgument));
            WriteAttribute(@"ActionOutput", @"", ((global::System.String)o.@ActionOutput));
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write16_RecipeGatheringServiceData(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"ServiceType", @"", ((global::System.String)o.@ServiceType));
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write15_Argument(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Argument o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Argument)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
            if (((global::System.String)o.@Type) != @"System.String") {
                WriteAttribute(@"Type", @"", ((global::System.String)o.@Type));
            }
            if (((global::System.Boolean)o.@Required) != true) {
                WriteAttribute(@"Required", @"", System.Xml.XmlConvert.ToString((global::System.Boolean)((global::System.Boolean)o.@Required)));
            }
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            Write12_Converter(@"Converter", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Converter)o.@Converter), false, false);
            Write14_ValueProvider(@"ValueProvider", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider)o.@ValueProvider), false, false);
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write14_ValueProvider(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Type", @"", ((global::System.String)o.@Type));
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
                global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument[])o.@MonitorArgument;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write13_MonitorArgument(@"MonitorArgument", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument)a[ia]), false, false);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write13_MonitorArgument(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
            WriteEndElement(o);
        }

        void Write12_Converter(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Converter o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Converter)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Type", @"", ((global::System.String)o.@Type));
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write11_RecipeHostData(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            if (((global::System.Int32)o.@Priority) != 1536) {
                WriteAttribute(@"Priority", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@Priority)));
            }
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            Write3_Icon(@"Icon", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Icon)o.@Icon), false, false);
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar[])o.@CommandBar;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write5_CommandBar(@"CommandBar", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar)a[ia]), false, false);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write5_CommandBar(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            if (o.@NameSpecified) {
                WriteAttribute(@"Name", @"", Write4_CommandBarName(((global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName)o.@Name)));
            }
            WriteAttribute(@"Menu", @"", ((global::System.String)o.@Menu));
            WriteAttribute(@"Guid", @"", ((global::System.String)o.@Guid));
            if (o.@IDSpecified) {
                WriteAttribute(@"ID", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@ID)));
            }
            if (o.@NameSpecified) {
            }
            if (o.@IDSpecified) {
            }
            WriteEndElement(o);
        }

        string Write4_CommandBarName(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName v) {
            string s = null;
            switch (v) {
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Solution: s = @"Solution"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Project: s = @"Project"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@WebProject: s = @"Web Project"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Item: s = @"Item"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@WebItem: s = @"Web Item"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Folder: s = @"Folder"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@WebFolder: s = @"Web Folder"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@SolutionFolder: s = @"Solution Folder"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@ProjectAdd: s = @"Project Add"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@SolutionAdd: s = @"Solution Add"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@SolutionFolderAdd: s = @"Solution Folder Add"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Microsoft.Practices.RecipeFramework.Configuration.CommandBarName");
            }
            return s;
        }

        void Write3_Icon(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Icon o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Icon)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Guid", @"", ((global::System.String)o.@Guid));
            if (o.@IDSpecified) {
                WriteAttribute(@"ID", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@ID)));
            }
            WriteAttribute(@"File", @"", ((global::System.String)o.@File));
            if (o.@IDSpecified) {
            }
            WriteEndElement(o);
        }

        void Write10_TypeAlias(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
            WriteAttribute(@"Type", @"", ((global::System.String)o.@Type));
            WriteEndElement(o);
        }

        void Write9_Link(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Link o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Link)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Url", @"", ((global::System.String)o.@Url));
            WriteAttribute(@"Caption", @"", ((global::System.String)o.@Caption));
            if (o.@KindSpecified) {
                WriteAttribute(@"Kind", @"", Write8_DocumentationKind(((global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind)o.@Kind)));
            }
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            if (o.@KindSpecified) {
            }
            WriteEndElement(o);
        }

        string Write8_DocumentationKind(global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind v) {
            string s = null;
            switch (v) {
                case global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind.@Documentation: s = @"Documentation"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind.@NextStep: s = @"NextStep"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind");
            }
            return s;
        }

        void Write7_GuidancePackageHostData(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteElementString(@"RegistrationSettings", @"http://schemas.microsoft.com/pag/gax-core", ((global::System.String)o.@RegistrationSettings));
            Write3_Icon(@"Icon", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Icon)o.@Icon), false, false);
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.Menu[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Menu[])o.@Menu;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write6_Menu(@"Menu", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Menu)a[ia]), false, false);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write6_Menu(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Menu o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Menu)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", FromXmlNCName(((global::System.String)o.@Name)));
            WriteAttribute(@"Text", @"", ((global::System.String)o.@Text));
            if (((global::System.Int32)o.@Priority) != 1536) {
                WriteAttribute(@"Priority", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@Priority)));
            }
            Write5_CommandBar(@"CommandBar", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar)o.@CommandBar), false, false);
            WriteEndElement(o);
        }

        void Write2_Overview(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Overview o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Overview)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Url", @"", ((global::System.String)o.@Url));
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        string Write22_SourceLevels(global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels v) {
            string s = null;
            switch (v) {
                case global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Error: s = @"Error"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Information: s = @"Information"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Off: s = @"Off"; break;
				case global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Critical: s = @"Critical"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Warning: s = @"Warning"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Verbose: s = @"Verbose"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Microsoft.Practices.RecipeFramework.Configuration.SourceLevels");
            }
            return s;
        }

        protected override void InitCallbacks() {
        }
    }

    public class XmlSerializationReaderGuidancePackage : System.Xml.Serialization.XmlSerializationReader {

        public object Read24_GuidancePackage() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id1_GuidancePackage && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read23_GuidancePackage(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:GuidancePackage");
            }
            return (object)o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage Read23_GuidancePackage(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage();
            global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[] a_2 = null;
            int ca_2 = 0;
            global::System.Xml.XmlAttribute[] a_13 = null;
            int ca_13 = 0;
            bool[] paramsRead = new bool[14];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[3] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = ToXmlNCName(Reader.Value);
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id5_Caption && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Caption = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id6_Description && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Description = Reader.Value;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id7_Version && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Version = Reader.Value;
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id8_Guid && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Guid = Reader.Value;
                    paramsRead[7] = true;
                }
                else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id9_SchemaVersion && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@SchemaVersion = Reader.Value;
                    paramsRead[8] = true;
                }
                else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id10_Host && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Host = Reader.Value;
                    paramsRead[9] = true;
                }
                else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id11_BindingRecipe && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@BindingRecipe = ToXmlNCName(Reader.Value);
                    paramsRead[10] = true;
                }
                else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id12_SourceLevels && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@SourceLevels = Read22_SourceLevels(Reader.Value);
                    paramsRead[11] = true;
                }
                else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id13_SortPriority && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@SortPriority = System.Xml.XmlConvert.ToInt32(Reader.Value);
                    paramsRead[12] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_13 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_13, ca_13, typeof(global::System.Xml.XmlAttribute));a_13[ca_13++] = attr;
                }
            }
            o.@Recipes = (global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[])ShrinkArray(a_2, ca_2, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Recipe), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_13, ca_13, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_13, ca_13, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id14_Overview && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@Overview = Read2_Overview(false, true);
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id15_HostData && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@HostData = Read7_GuidancePackageHostData(false, true);
                        paramsRead[1] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id16_Recipes && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[] a_2_0 = null;
                            int ca_2_0 = 0;
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
                                        if (((object) Reader.LocalName == (object)id17_Recipe && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_2_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[])EnsureArrayIndex(a_2_0, ca_2_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Recipe));a_2_0[ca_2_0++] = Read21_Recipe(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:Recipe");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:Recipe");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations1, ref readerCount1);
                                }
                            ReadEndElement();
                            }
                            o.@Recipes = (global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[])ShrinkArray(a_2_0, ca_2_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Recipe), false);
                        }
                    }
                    else {
                        UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:Overview, http://schemas.microsoft.com/pag/gax-core:HostData, http://schemas.microsoft.com/pag/gax-core:Recipes");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:Overview, http://schemas.microsoft.com/pag/gax-core:HostData, http://schemas.microsoft.com/pag/gax-core:Recipes");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_13, ca_13, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Recipe Read21_Recipe(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Recipe o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Recipe();
            global::Microsoft.Practices.RecipeFramework.Configuration.Link[] a_0 = null;
            int ca_0 = 0;
            global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[] a_1 = null;
            int ca_1 = 0;
            global::Microsoft.Practices.RecipeFramework.Configuration.Argument[] a_5 = null;
            int ca_5 = 0;
            global::System.Xml.XmlAttribute[] a_11 = null;
            int ca_11 = 0;
            bool[] paramsRead = new bool[12];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[8] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = ToXmlNCName(Reader.Value);
                    paramsRead[8] = true;
                }
                else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id18_Recurrent && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Recurrent = System.Xml.XmlConvert.ToBoolean(Reader.Value);
                    paramsRead[9] = true;
                }
                else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id19_Bound && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Bound = System.Xml.XmlConvert.ToBoolean(Reader.Value);
                    paramsRead[10] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_11 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_11, ca_11, typeof(global::System.Xml.XmlAttribute));a_11[ca_11++] = attr;
                }
            }
            o.@DocumentationLinks = (global::Microsoft.Practices.RecipeFramework.Configuration.Link[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Link), true);
            o.@Types = (global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias), true);
            o.@Arguments = (global::Microsoft.Practices.RecipeFramework.Configuration.Argument[])ShrinkArray(a_5, ca_5, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Argument), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_11, ca_11, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_11, ca_11, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations2 = 0;
            int readerCount2 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id20_DocumentationLinks && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Microsoft.Practices.RecipeFramework.Configuration.Link[] a_0_0 = null;
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
                                        if (((object) Reader.LocalName == (object)id21_Link && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_0_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.Link[])EnsureArrayIndex(a_0_0, ca_0_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Link));a_0_0[ca_0_0++] = Read9_Link(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:Link");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:Link");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations3, ref readerCount3);
                                }
                            ReadEndElement();
                            }
                            o.@DocumentationLinks = (global::Microsoft.Practices.RecipeFramework.Configuration.Link[])ShrinkArray(a_0_0, ca_0_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Link), false);
                        }
                    }
                    else if (((object) Reader.LocalName == (object)id22_Types && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[] a_1_0 = null;
                            int ca_1_0 = 0;
                            if ((Reader.IsEmptyElement)) {
                                Reader.Skip();
                            }
                            else {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations4 = 0;
                                int readerCount4 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                        if (((object) Reader.LocalName == (object)id23_TypeAlias && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_1_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[])EnsureArrayIndex(a_1_0, ca_1_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias));a_1_0[ca_1_0++] = Read10_TypeAlias(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:TypeAlias");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:TypeAlias");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations4, ref readerCount4);
                                }
                            ReadEndElement();
                            }
                            o.@Types = (global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[])ShrinkArray(a_1_0, ca_1_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias), false);
                        }
                    }
                    else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id5_Caption && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        {
                            o.@Caption = Reader.ReadElementString();
                        }
                        paramsRead[2] = true;
                    }
                    else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id6_Description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        {
                            o.@Description = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id15_HostData && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@HostData = Read11_RecipeHostData(false, true);
                        paramsRead[4] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id24_Arguments && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Microsoft.Practices.RecipeFramework.Configuration.Argument[] a_5_0 = null;
                            int ca_5_0 = 0;
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
                                        if (((object) Reader.LocalName == (object)id25_Argument && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_5_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.Argument[])EnsureArrayIndex(a_5_0, ca_5_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Argument));a_5_0[ca_5_0++] = Read15_Argument(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:Argument");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:Argument");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations5, ref readerCount5);
                                }
                            ReadEndElement();
                            }
                            o.@Arguments = (global::Microsoft.Practices.RecipeFramework.Configuration.Argument[])ShrinkArray(a_5_0, ca_5_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Argument), false);
                        }
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id26_GatheringServiceData && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@GatheringServiceData = Read16_RecipeGatheringServiceData(false, true);
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id27_Actions && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@Actions = Read20_RecipeActions(false, true);
                        paramsRead[7] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:DocumentationLinks, http://schemas.microsoft.com/pag/gax-core:Types, http://schemas.microsoft.com/pag/gax-core:Caption, http://schemas.microsoft.com/pag/gax-core:Description, http://schemas.microsoft.com/pag/gax-core:HostData, http://schemas.microsoft.com/pag/gax-core:Arguments, http://schemas.microsoft.com/pag/gax-core:GatheringServiceData, http://schemas.microsoft.com/pag/gax-core:Actions");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:DocumentationLinks, http://schemas.microsoft.com/pag/gax-core:Types, http://schemas.microsoft.com/pag/gax-core:Caption, http://schemas.microsoft.com/pag/gax-core:Description, http://schemas.microsoft.com/pag/gax-core:HostData, http://schemas.microsoft.com/pag/gax-core:Arguments, http://schemas.microsoft.com/pag/gax-core:GatheringServiceData, http://schemas.microsoft.com/pag/gax-core:Actions");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations2, ref readerCount2);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_11, ca_11, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions Read20_RecipeActions(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions();
            global::Microsoft.Practices.RecipeFramework.Configuration.Action[] a_0 = null;
            int ca_0 = 0;
            global::System.Xml.XmlAttribute[] a_4 = null;
            int ca_4 = 0;
            bool[] paramsRead = new bool[5];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id28_CoordinatorServiceType && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@CoordinatorServiceType = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id29_ExecutionServiceType && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@ExecutionServiceType = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_4 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_4, ca_4, typeof(global::System.Xml.XmlAttribute));a_4[ca_4++] = attr;
                }
            }
            o.@Action = (global::Microsoft.Practices.RecipeFramework.Configuration.Action[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Action), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@Action = (global::Microsoft.Practices.RecipeFramework.Configuration.Action[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Action), true);
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations6 = 0;
            int readerCount6 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id30_Action && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.Action[])EnsureArrayIndex(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Action));a_0[ca_0++] = Read19_Action(false, true);
                    }
                    else {
                        o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:Action");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations6, ref readerCount6);
            }
            o.@Action = (global::Microsoft.Practices.RecipeFramework.Configuration.Action[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Action), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Action Read19_Action(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Action o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Action();
            global::Microsoft.Practices.RecipeFramework.Configuration.Input[] a_0 = null;
            int ca_0 = 0;
            global::Microsoft.Practices.RecipeFramework.Configuration.Output[] a_1 = null;
            int ca_1 = 0;
            global::System.Xml.XmlAttribute[] a_5 = null;
            int ca_5 = 0;
            bool[] paramsRead = new bool[6];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[3] && ((object) Reader.LocalName == (object)id31_Type && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Type = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_5 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_5, ca_5, typeof(global::System.Xml.XmlAttribute));a_5[ca_5++] = attr;
                }
            }
            o.@Input = (global::Microsoft.Practices.RecipeFramework.Configuration.Input[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Input), true);
            o.@Output = (global::Microsoft.Practices.RecipeFramework.Configuration.Output[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Output), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_5, ca_5, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@Input = (global::Microsoft.Practices.RecipeFramework.Configuration.Input[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Input), true);
                o.@Output = (global::Microsoft.Practices.RecipeFramework.Configuration.Output[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Output), true);
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_5, ca_5, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations7 = 0;
            int readerCount7 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id32_Input && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.Input[])EnsureArrayIndex(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Input));a_0[ca_0++] = Read17_Input(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id33_Output && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Microsoft.Practices.RecipeFramework.Configuration.Output[])EnsureArrayIndex(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Output));a_1[ca_1++] = Read18_Output(false, true);
                    }
                    else {
                        o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:Input, http://schemas.microsoft.com/pag/gax-core:Output");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations7, ref readerCount7);
            }
            o.@Input = (global::Microsoft.Practices.RecipeFramework.Configuration.Input[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Input), true);
            o.@Output = (global::Microsoft.Practices.RecipeFramework.Configuration.Output[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Output), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_5, ca_5, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Output Read18_Output(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Output o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Output();
            global::System.Xml.XmlAttribute[] a_2 = null;
            int ca_2 = 0;
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_2 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_2, ca_2, typeof(global::System.Xml.XmlAttribute));a_2[ca_2++] = attr;
                }
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations8 = 0;
            int readerCount8 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations8, ref readerCount8);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Input Read17_Input(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Input o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Input();
            global::System.Xml.XmlAttribute[] a_4 = null;
            int ca_4 = 0;
            bool[] paramsRead = new bool[5];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id34_RecipeArgument && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@RecipeArgument = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id35_ActionOutput && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@ActionOutput = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_4 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_4, ca_4, typeof(global::System.Xml.XmlAttribute));a_4[ca_4++] = attr;
                }
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations9 = 0;
            int readerCount9 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations9, ref readerCount9);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData Read16_RecipeGatheringServiceData(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData();
            global::System.Xml.XmlAttribute[] a_2 = null;
            int ca_2 = 0;
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id36_ServiceType && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@ServiceType = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_2 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_2, ca_2, typeof(global::System.Xml.XmlAttribute));a_2[ca_2++] = attr;
                }
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations10 = 0;
            int readerCount10 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations10, ref readerCount10);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Argument Read15_Argument(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Argument o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Argument();
            global::System.Xml.XmlAttribute[] a_6 = null;
            int ca_6 = 0;
            bool[] paramsRead = new bool[7];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[3] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id31_Type && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Type = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id37_Required && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Required = System.Xml.XmlConvert.ToBoolean(Reader.Value);
                    paramsRead[5] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_6 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_6, ca_6, typeof(global::System.Xml.XmlAttribute));a_6[ca_6++] = attr;
                }
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_6, ca_6, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_6, ca_6, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations11 = 0;
            int readerCount11 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id38_Converter && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@Converter = Read12_Converter(false, true);
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id39_ValueProvider && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@ValueProvider = Read14_ValueProvider(false, true);
                        paramsRead[1] = true;
                    }
                    else {
                        o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:Converter, http://schemas.microsoft.com/pag/gax-core:ValueProvider");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations11, ref readerCount11);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_6, ca_6, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider Read14_ValueProvider(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider();
            global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument[] a_0 = null;
            int ca_0 = 0;
            global::System.Xml.XmlAttribute[] a_3 = null;
            int ca_3 = 0;
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id31_Type && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Type = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_3 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_3, ca_3, typeof(global::System.Xml.XmlAttribute));a_3[ca_3++] = attr;
                }
            }
            o.@MonitorArgument = (global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_3, ca_3, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@MonitorArgument = (global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument), true);
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_3, ca_3, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations12 = 0;
            int readerCount12 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id40_MonitorArgument && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument[])EnsureArrayIndex(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument));a_0[ca_0++] = Read13_MonitorArgument(false, true);
                    }
                    else {
                        o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:MonitorArgument");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations12, ref readerCount12);
            }
            o.@MonitorArgument = (global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_3, ca_3, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument Read13_MonitorArgument(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument();
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Name");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations13 = 0;
            int readerCount13 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations13, ref readerCount13);
            }
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Converter Read12_Converter(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Converter o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Converter();
            global::System.Xml.XmlAttribute[] a_2 = null;
            int ca_2 = 0;
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id31_Type && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Type = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_2 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_2, ca_2, typeof(global::System.Xml.XmlAttribute));a_2[ca_2++] = attr;
                }
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations14 = 0;
            int readerCount14 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations14, ref readerCount14);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData Read11_RecipeHostData(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData();
            global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar[] a_1 = null;
            int ca_1 = 0;
            global::System.Xml.XmlAttribute[] a_4 = null;
            int ca_4 = 0;
            bool[] paramsRead = new bool[5];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[3] && ((object) Reader.LocalName == (object)id41_Priority && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Priority = System.Xml.XmlConvert.ToInt32(Reader.Value);
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_4 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_4, ca_4, typeof(global::System.Xml.XmlAttribute));a_4[ca_4++] = attr;
                }
            }
            o.@CommandBar = (global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@CommandBar = (global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar), true);
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations15 = 0;
            int readerCount15 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id42_Icon && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@Icon = Read3_Icon(false, true);
                        paramsRead[0] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id43_CommandBar && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar[])EnsureArrayIndex(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar));a_1[ca_1++] = Read5_CommandBar(false, true);
                    }
                    else {
                        o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:Icon, http://schemas.microsoft.com/pag/gax-core:CommandBar");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations15, ref readerCount15);
            }
            o.@CommandBar = (global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar Read5_CommandBar(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar();
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Read4_CommandBarName(Reader.Value);
                    o.@NameSpecified = true;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id44_Menu && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Menu = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id8_Guid && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Guid = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id45_ID && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@ID = System.Xml.XmlConvert.ToInt32(Reader.Value);
                    o.@IDSpecified = true;
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Name, :Menu, :Guid, :ID");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations16 = 0;
            int readerCount16 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations16, ref readerCount16);
            }
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName Read4_CommandBarName(string s) {
            switch (s) {
                case @"Solution": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Solution;
                case @"Project": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Project;
                case @"Web Project": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@WebProject;
                case @"Item": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Item;
                case @"Web Item": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@WebItem;
                case @"Folder": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Folder;
                case @"Web Folder": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@WebFolder;
                case @"Solution Folder": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@SolutionFolder;
                case @"Project Add": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@ProjectAdd;
                case @"Solution Add": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@SolutionAdd;
                case @"Solution Folder Add": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@SolutionFolderAdd;
                default: throw CreateUnknownConstantException(s, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName));
            }
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Icon Read3_Icon(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Icon o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Icon();
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id8_Guid && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Guid = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id45_ID && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@ID = System.Xml.XmlConvert.ToInt32(Reader.Value);
                    o.@IDSpecified = true;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id46_File && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@File = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Guid, :ID, :File");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations17 = 0;
            int readerCount17 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations17, ref readerCount17);
            }
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias Read10_TypeAlias(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias();
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id31_Type && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Type = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Name, :Type");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations18 = 0;
            int readerCount18 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations18, ref readerCount18);
            }
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Link Read9_Link(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Link o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Link();
            global::System.Xml.XmlAttribute[] a_3 = null;
            int ca_3 = 0;
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id47_Url && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Url = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id5_Caption && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Caption = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id48_Kind && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Kind = Read8_DocumentationKind(Reader.Value);
                    o.@KindSpecified = true;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_3 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_3, ca_3, typeof(global::System.Xml.XmlAttribute));a_3[ca_3++] = attr;
                }
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_3, ca_3, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_3, ca_3, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations19 = 0;
            int readerCount19 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations19, ref readerCount19);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_3, ca_3, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind Read8_DocumentationKind(string s) {
            switch (s) {
                case @"Documentation": return global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind.@Documentation;
                case @"NextStep": return global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind.@NextStep;
                default: throw CreateUnknownConstantException(s, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind));
            }
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData Read7_GuidancePackageHostData(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData();
            global::Microsoft.Practices.RecipeFramework.Configuration.Menu[] a_2 = null;
            int ca_2 = 0;
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@Menu = (global::Microsoft.Practices.RecipeFramework.Configuration.Menu[])ShrinkArray(a_2, ca_2, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Menu), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations20 = 0;
            int readerCount20 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id49_RegistrationSettings && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        {
                            o.@RegistrationSettings = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id42_Icon && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@Icon = Read3_Icon(false, true);
                        paramsRead[1] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id44_Menu && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_2 = (global::Microsoft.Practices.RecipeFramework.Configuration.Menu[])EnsureArrayIndex(a_2, ca_2, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Menu));a_2[ca_2++] = Read6_Menu(false, true);
                    }
                    else {
                        o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:RegistrationSettings, http://schemas.microsoft.com/pag/gax-core:Icon, http://schemas.microsoft.com/pag/gax-core:Menu");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations20, ref readerCount20);
            }
            o.@Menu = (global::Microsoft.Practices.RecipeFramework.Configuration.Menu[])ShrinkArray(a_2, ca_2, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Menu), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Menu Read6_Menu(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Menu o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Menu();
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = ToXmlNCName(Reader.Value);
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id50_Text && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Text = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id41_Priority && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Priority = System.Xml.XmlConvert.ToInt32(Reader.Value);
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Name, :Text, :Priority");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations21 = 0;
            int readerCount21 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id43_CommandBar && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@CommandBar = Read5_CommandBar(false, true);
                        paramsRead[0] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:CommandBar");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:CommandBar");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations21, ref readerCount21);
            }
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.Overview Read2_Overview(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Overview o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Overview();
            global::System.Xml.XmlAttribute[] a_2 = null;
            int ca_2 = 0;
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id47_Url && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Url = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_2 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_2, ca_2, typeof(global::System.Xml.XmlAttribute));a_2[ca_2++] = attr;
                }
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations22 = 0;
            int readerCount22 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations22, ref readerCount22);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels Read22_SourceLevels(string s) {
            switch (s) {
                case @"Error": return global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Error;
                case @"Information": return global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Information;
                case @"Off": return global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Off;
				case @"Critical": return global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Critical;
                case @"Warning": return global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Warning;
                case @"Verbose": return global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Verbose;
                default: throw CreateUnknownConstantException(s, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels));
            }
        }

        protected override void InitCallbacks() {
        }

        string id21_Link;
        string id38_Converter;
        string id16_Recipes;
        string id3_Item;
        string id10_Host;
        string id6_Description;
        string id24_Arguments;
        string id49_RegistrationSettings;
        string id14_Overview;
        string id46_File;
        string id40_MonitorArgument;
        string id44_Menu;
        string id4_Name;
        string id17_Recipe;
        string id15_HostData;
        string id22_Types;
        string id34_RecipeArgument;
        string id19_Bound;
        string id50_Text;
        string id20_DocumentationLinks;
        string id5_Caption;
        string id26_GatheringServiceData;
        string id11_BindingRecipe;
        string id36_ServiceType;
        string id43_CommandBar;
        string id7_Version;
        string id25_Argument;
        string id9_SchemaVersion;
        string id42_Icon;
        string id35_ActionOutput;
        string id32_Input;
        string id41_Priority;
        string id1_GuidancePackage;
        string id29_ExecutionServiceType;
        string id28_CoordinatorServiceType;
        string id2_Item;
        string id47_Url;
        string id45_ID;
        string id31_Type;
        string id39_ValueProvider;
        string id27_Actions;
        string id12_SourceLevels;
        string id33_Output;
        string id48_Kind;
        string id30_Action;
        string id13_SortPriority;
        string id23_TypeAlias;
        string id18_Recurrent;
        string id8_Guid;
        string id37_Required;

        protected override void InitIDs() {
            id21_Link = Reader.NameTable.Add(@"Link");
            id38_Converter = Reader.NameTable.Add(@"Converter");
            id16_Recipes = Reader.NameTable.Add(@"Recipes");
            id3_Item = Reader.NameTable.Add(@"");
            id10_Host = Reader.NameTable.Add(@"Host");
            id6_Description = Reader.NameTable.Add(@"Description");
            id24_Arguments = Reader.NameTable.Add(@"Arguments");
            id49_RegistrationSettings = Reader.NameTable.Add(@"RegistrationSettings");
            id14_Overview = Reader.NameTable.Add(@"Overview");
            id46_File = Reader.NameTable.Add(@"File");
            id40_MonitorArgument = Reader.NameTable.Add(@"MonitorArgument");
            id44_Menu = Reader.NameTable.Add(@"Menu");
            id4_Name = Reader.NameTable.Add(@"Name");
            id17_Recipe = Reader.NameTable.Add(@"Recipe");
            id15_HostData = Reader.NameTable.Add(@"HostData");
            id22_Types = Reader.NameTable.Add(@"Types");
            id34_RecipeArgument = Reader.NameTable.Add(@"RecipeArgument");
            id19_Bound = Reader.NameTable.Add(@"Bound");
            id50_Text = Reader.NameTable.Add(@"Text");
            id20_DocumentationLinks = Reader.NameTable.Add(@"DocumentationLinks");
            id5_Caption = Reader.NameTable.Add(@"Caption");
            id26_GatheringServiceData = Reader.NameTable.Add(@"GatheringServiceData");
            id11_BindingRecipe = Reader.NameTable.Add(@"BindingRecipe");
            id36_ServiceType = Reader.NameTable.Add(@"ServiceType");
            id43_CommandBar = Reader.NameTable.Add(@"CommandBar");
            id7_Version = Reader.NameTable.Add(@"Version");
            id25_Argument = Reader.NameTable.Add(@"Argument");
            id9_SchemaVersion = Reader.NameTable.Add(@"SchemaVersion");
            id42_Icon = Reader.NameTable.Add(@"Icon");
            id35_ActionOutput = Reader.NameTable.Add(@"ActionOutput");
            id32_Input = Reader.NameTable.Add(@"Input");
            id41_Priority = Reader.NameTable.Add(@"Priority");
            id1_GuidancePackage = Reader.NameTable.Add(@"GuidancePackage");
            id29_ExecutionServiceType = Reader.NameTable.Add(@"ExecutionServiceType");
            id28_CoordinatorServiceType = Reader.NameTable.Add(@"CoordinatorServiceType");
            id2_Item = Reader.NameTable.Add(@"http://schemas.microsoft.com/pag/gax-core");
            id47_Url = Reader.NameTable.Add(@"Url");
            id45_ID = Reader.NameTable.Add(@"ID");
            id31_Type = Reader.NameTable.Add(@"Type");
            id39_ValueProvider = Reader.NameTable.Add(@"ValueProvider");
            id27_Actions = Reader.NameTable.Add(@"Actions");
            id12_SourceLevels = Reader.NameTable.Add(@"SourceLevels");
            id33_Output = Reader.NameTable.Add(@"Output");
            id48_Kind = Reader.NameTable.Add(@"Kind");
            id30_Action = Reader.NameTable.Add(@"Action");
            id13_SortPriority = Reader.NameTable.Add(@"SortPriority");
            id23_TypeAlias = Reader.NameTable.Add(@"TypeAlias");
            id18_Recurrent = Reader.NameTable.Add(@"Recurrent");
            id8_Guid = Reader.NameTable.Add(@"Guid");
            id37_Required = Reader.NameTable.Add(@"Required");
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
            return xmlReader.IsStartElement(@"GuidancePackage", @"http://schemas.microsoft.com/pag/gax-core");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriterGuidancePackage)writer).Write24_GuidancePackage(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReaderGuidancePackage)reader).Read24_GuidancePackage();
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
                    _tmp[@"Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage:http://schemas.microsoft.com/pag/gax-core::False:"] = @"Read24_GuidancePackage";
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
                    _tmp[@"Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage:http://schemas.microsoft.com/pag/gax-core::False:"] = @"Write24_GuidancePackage";
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
                    _tmp.Add(@"Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage:http://schemas.microsoft.com/pag/gax-core::False:", new GuidancePackageSerializer());
                    if (typedSerializers == null) typedSerializers = _tmp;
                }
                return typedSerializers;
            }
        }
        public override System.Boolean CanSerialize(System.Type type) {
            if (type == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage)) return true;
            return false;
        }
        public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
            if (type == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage)) return new GuidancePackageSerializer();
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
//     Runtime Version: 2.0.50727.312
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

#pragma warning disable 0642, 0219
namespace Microsoft.Practices.RecipeFramework.Configuration.Serialization
{
    using System.Xml.Serialization;
    using System;
    
    
    /// /// <summary>Custom reader for GuidancePackage instances.</summary>
    internal class GuidancePackageReader : GuidancePackageSerializer.BaseReader
    {
        

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage Read23_GuidancePackage(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage obj = base.Read23_GuidancePackage(isNullable, checkType);
			GuidancePackageDeserializedHandler handler = GuidancePackageDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Recipe Read21_Recipe(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Recipe obj = base.Read21_Recipe(isNullable, checkType);
			RecipeDeserializedHandler handler = RecipeDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions Read20_RecipeActions(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions obj = base.Read20_RecipeActions(isNullable, checkType);
			RecipeActionsDeserializedHandler handler = RecipeActionsDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Action Read19_Action(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Action obj = base.Read19_Action(isNullable, checkType);
			ActionDeserializedHandler handler = ActionDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Output Read18_Output(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Output obj = base.Read18_Output(isNullable, checkType);
			OutputDeserializedHandler handler = OutputDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Input Read17_Input(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Input obj = base.Read17_Input(isNullable, checkType);
			InputDeserializedHandler handler = InputDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData Read16_RecipeGatheringServiceData(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData obj = base.Read16_RecipeGatheringServiceData(isNullable, checkType);
			RecipeGatheringServiceDataDeserializedHandler handler = RecipeGatheringServiceDataDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Argument Read15_Argument(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Argument obj = base.Read15_Argument(isNullable, checkType);
			ArgumentDeserializedHandler handler = ArgumentDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider Read14_ValueProvider(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider obj = base.Read14_ValueProvider(isNullable, checkType);
			ValueProviderDeserializedHandler handler = ValueProviderDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument Read13_MonitorArgument(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument obj = base.Read13_MonitorArgument(isNullable, checkType);
			MonitorArgumentDeserializedHandler handler = MonitorArgumentDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Converter Read12_Converter(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Converter obj = base.Read12_Converter(isNullable, checkType);
			ConverterDeserializedHandler handler = ConverterDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData Read11_RecipeHostData(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData obj = base.Read11_RecipeHostData(isNullable, checkType);
			RecipeHostDataDeserializedHandler handler = RecipeHostDataDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar Read5_CommandBar(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar obj = base.Read5_CommandBar(isNullable, checkType);
			CommandBarDeserializedHandler handler = CommandBarDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName Read4_CommandBarName(string s)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName obj = base.Read4_CommandBarName(s);
			CommandBarNameDeserializedHandler handler = CommandBarNameDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Icon Read3_Icon(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Icon obj = base.Read3_Icon(isNullable, checkType);
			IconDeserializedHandler handler = IconDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias Read10_TypeAlias(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias obj = base.Read10_TypeAlias(isNullable, checkType);
			TypeAliasDeserializedHandler handler = TypeAliasDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Link Read9_Link(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Link obj = base.Read9_Link(isNullable, checkType);
			LinkDeserializedHandler handler = LinkDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind Read8_DocumentationKind(string s)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind obj = base.Read8_DocumentationKind(s);
			DocumentationKindDeserializedHandler handler = DocumentationKindDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData Read7_GuidancePackageHostData(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData obj = base.Read7_GuidancePackageHostData(isNullable, checkType);
			GuidancePackageHostDataDeserializedHandler handler = GuidancePackageHostDataDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Menu Read6_Menu(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Menu obj = base.Read6_Menu(isNullable, checkType);
			MenuDeserializedHandler handler = MenuDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.Overview Read2_Overview(bool isNullable, bool checkType)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.Overview obj = base.Read2_Overview(isNullable, checkType);
			OverviewDeserializedHandler handler = OverviewDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <remarks/>
		protected override global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels Read22_SourceLevels(string s)
		{
			global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels obj = base.Read22_SourceLevels(s);
			SourceLevelsDeserializedHandler handler = SourceLevelsDeserialized;
			if (handler != null)
				handler(obj);

			return obj;
		}

		/// <summary>Reads an object of type Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage.</summary>
		internal Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage Read()
		{
			return (Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage) Read24_GuidancePackage();
		}
        
        /// /// <remarks/>
        public event GuidancePackageDeserializedHandler GuidancePackageDeserialized;
        
        /// /// <remarks/>
        public event RecipeDeserializedHandler RecipeDeserialized;
        
        /// /// <remarks/>
        public event RecipeActionsDeserializedHandler RecipeActionsDeserialized;
        
        /// /// <remarks/>
        public event ActionDeserializedHandler ActionDeserialized;
        
        /// /// <remarks/>
        public event OutputDeserializedHandler OutputDeserialized;
        
        /// /// <remarks/>
        public event InputDeserializedHandler InputDeserialized;
        
        /// /// <remarks/>
        public event RecipeGatheringServiceDataDeserializedHandler RecipeGatheringServiceDataDeserialized;
        
        /// /// <remarks/>
        public event ArgumentDeserializedHandler ArgumentDeserialized;
        
        /// /// <remarks/>
        public event ValueProviderDeserializedHandler ValueProviderDeserialized;
        
        /// /// <remarks/>
        public event MonitorArgumentDeserializedHandler MonitorArgumentDeserialized;
        
        /// /// <remarks/>
        public event ConverterDeserializedHandler ConverterDeserialized;
        
        /// /// <remarks/>
        public event RecipeHostDataDeserializedHandler RecipeHostDataDeserialized;
        
        /// /// <remarks/>
        public event CommandBarDeserializedHandler CommandBarDeserialized;
        
        /// /// <remarks/>
        public event CommandBarNameDeserializedHandler CommandBarNameDeserialized;
        
        /// /// <remarks/>
        public event IconDeserializedHandler IconDeserialized;
        
        /// /// <remarks/>
        public event TypeAliasDeserializedHandler TypeAliasDeserialized;
        
        /// /// <remarks/>
        public event LinkDeserializedHandler LinkDeserialized;
        
        /// /// <remarks/>
        public event DocumentationKindDeserializedHandler DocumentationKindDeserialized;
        
        /// /// <remarks/>
        public event GuidancePackageHostDataDeserializedHandler GuidancePackageHostDataDeserialized;
        
        /// /// <remarks/>
        public event MenuDeserializedHandler MenuDeserialized;
        
        /// /// <remarks/>
        public event OverviewDeserializedHandler OverviewDeserialized;
        
        /// /// <remarks/>
        public event SourceLevelsDeserializedHandler SourceLevelsDeserialized;
    }
    
    /// /// <remarks/>
    public delegate void GuidancePackageDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage guidancePackage);
    
    /// /// <remarks/>
    public delegate void RecipeDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.Recipe recipe);
    
    /// /// <remarks/>
    public delegate void RecipeActionsDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions recipeActions);
    
    /// /// <remarks/>
    public delegate void ActionDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.Action action);
    
    /// /// <remarks/>
    public delegate void OutputDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.Output output);
    
    /// /// <remarks/>
    public delegate void InputDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.Input input);
    
    /// /// <remarks/>
    public delegate void RecipeGatheringServiceDataDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData recipeGatheringServiceData);
    
    /// /// <remarks/>
    public delegate void ArgumentDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.Argument argument);
    
    /// /// <remarks/>
    public delegate void ValueProviderDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider valueProvider);
    
    /// /// <remarks/>
    public delegate void MonitorArgumentDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument monitorArgument);
    
    /// /// <remarks/>
    public delegate void ConverterDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.Converter converter);
    
    /// /// <remarks/>
    public delegate void RecipeHostDataDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData recipeHostData);
    
    /// /// <remarks/>
    public delegate void CommandBarDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar commandBar);
    
    /// /// <remarks/>
    public delegate void CommandBarNameDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName commandBarName);
    
    /// /// <remarks/>
    public delegate void IconDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.Icon icon);
    
    /// /// <remarks/>
    public delegate void TypeAliasDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias typeAlias);
    
    /// /// <remarks/>
    public delegate void LinkDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.Link link);
    
    /// /// <remarks/>
    public delegate void DocumentationKindDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind documentationKind);
    
    /// /// <remarks/>
    public delegate void GuidancePackageHostDataDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData guidancePackageHostData);
    
    /// /// <remarks/>
    public delegate void MenuDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.Menu menu);
    
    /// /// <remarks/>
    public delegate void OverviewDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.Overview overview);
    
    /// /// <remarks/>
    public delegate void SourceLevelsDeserializedHandler(global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels sourceLevels);
    
    /// /// <summary>Custom writer for GuidancePackage instances.</summary>
    internal class GuidancePackageWriter : GuidancePackageSerializer.BaseWriter
    {
        

		/// <summary>Writes an object of type Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage.</summary>
		internal void Write(Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage value)
		{
			Write24_GuidancePackage(value);
		}
    }
}
#if _DYNAMIC_XMLSERIALIZER_COMPILATION
[assembly:System.Security.AllowPartiallyTrustedCallers()]
[assembly:System.Security.SecurityTransparent()]
#endif

namespace Microsoft.Practices.RecipeFramework.Configuration.Serialization {

    internal partial class GuidancePackageSerializer {
	internal class BaseWriter : System.Xml.Serialization.XmlSerializationWriter {

        protected internal void Write24_GuidancePackage(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"GuidancePackage", @"http://schemas.microsoft.com/pag/gax-core");
                return;
            }
            TopLevelElement();
            Write23_GuidancePackage(@"GuidancePackage", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage)o), false, false);
        }

        void Write23_GuidancePackage(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", FromXmlNCName(((global::System.String)o.@Name)));
            WriteAttribute(@"Caption", @"", ((global::System.String)o.@Caption));
            WriteAttribute(@"Description", @"", ((global::System.String)o.@Description));
            if (((global::System.String)o.@Version) != @"1.0") {
                WriteAttribute(@"Version", @"", ((global::System.String)o.@Version));
            }
            WriteAttribute(@"Guid", @"", ((global::System.String)o.@Guid));
            WriteAttribute(@"SchemaVersion", @"", ((global::System.String)o.@SchemaVersion));
            if (((global::System.String)o.@Host) != @"VisualStudio") {
                WriteAttribute(@"Host", @"", ((global::System.String)o.@Host));
            }
            WriteAttribute(@"BindingRecipe", @"", FromXmlNCName(((global::System.String)o.@BindingRecipe)));
            if (((global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels)o.@SourceLevels) != global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Error) {
                WriteAttribute(@"SourceLevels", @"", Write22_SourceLevels(((global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels)o.@SourceLevels)));
            }
            if (((global::System.Int32)o.@SortPriority) != 100) {
                WriteAttribute(@"SortPriority", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@SortPriority)));
            }
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            Write2_Overview(@"Overview", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Overview)o.@Overview), false, false);
            Write7_GuidancePackageHostData(@"HostData", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData)o.@HostData), false, false);
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[])((global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[])o.@Recipes);
                if (a != null){
                    WriteStartElement(@"Recipes", @"http://schemas.microsoft.com/pag/gax-core", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write21_Recipe(@"Recipe", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Recipe)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            WriteEndElement(o);
        }

        void Write21_Recipe(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Recipe o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Recipe)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", FromXmlNCName(((global::System.String)o.@Name)));
            if (((global::System.Boolean)o.@Recurrent) != true) {
                WriteAttribute(@"Recurrent", @"", System.Xml.XmlConvert.ToString((global::System.Boolean)((global::System.Boolean)o.@Recurrent)));
            }
            if (((global::System.Boolean)o.@Bound) != true) {
                WriteAttribute(@"Bound", @"", System.Xml.XmlConvert.ToString((global::System.Boolean)((global::System.Boolean)o.@Bound)));
            }
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
                global::Microsoft.Practices.RecipeFramework.Configuration.Link[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Link[])((global::Microsoft.Practices.RecipeFramework.Configuration.Link[])o.@DocumentationLinks);
                if (a != null){
                    WriteStartElement(@"DocumentationLinks", @"http://schemas.microsoft.com/pag/gax-core", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write9_Link(@"Link", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Link)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[])((global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[])o.@Types);
                if (a != null){
                    WriteStartElement(@"Types", @"http://schemas.microsoft.com/pag/gax-core", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write10_TypeAlias(@"TypeAlias", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            WriteElementString(@"Caption", @"http://schemas.microsoft.com/pag/gax-core", ((global::System.String)o.@Caption));
            WriteElementString(@"Description", @"http://schemas.microsoft.com/pag/gax-core", ((global::System.String)o.@Description));
            Write11_RecipeHostData(@"HostData", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData)o.@HostData), false, false);
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.Argument[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Argument[])((global::Microsoft.Practices.RecipeFramework.Configuration.Argument[])o.@Arguments);
                if (a != null){
                    WriteStartElement(@"Arguments", @"http://schemas.microsoft.com/pag/gax-core", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write15_Argument(@"Argument", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Argument)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            Write16_RecipeGatheringServiceData(@"GatheringServiceData", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData)o.@GatheringServiceData), false, false);
            Write20_RecipeActions(@"Actions", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions)o.@Actions), false, false);
            WriteEndElement(o);
        }

        void Write20_RecipeActions(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"CoordinatorServiceType", @"", ((global::System.String)o.@CoordinatorServiceType));
            WriteAttribute(@"ExecutionServiceType", @"", ((global::System.String)o.@ExecutionServiceType));
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
                global::Microsoft.Practices.RecipeFramework.Configuration.Action[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Action[])o.@Action;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write19_Action(@"Action", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Action)a[ia]), false, false);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write19_Action(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Action o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Action)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Type", @"", ((global::System.String)o.@Type));
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
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
                global::Microsoft.Practices.RecipeFramework.Configuration.Input[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Input[])o.@Input;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write17_Input(@"Input", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Input)a[ia]), false, false);
                    }
                }
            }
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.Output[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Output[])o.@Output;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write18_Output(@"Output", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Output)a[ia]), false, false);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write18_Output(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Output o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Output)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write17_Input(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Input o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Input)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
            WriteAttribute(@"RecipeArgument", @"", ((global::System.String)o.@RecipeArgument));
            WriteAttribute(@"ActionOutput", @"", ((global::System.String)o.@ActionOutput));
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write16_RecipeGatheringServiceData(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"ServiceType", @"", ((global::System.String)o.@ServiceType));
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write15_Argument(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Argument o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Argument)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
            if (((global::System.String)o.@Type) != @"System.String") {
                WriteAttribute(@"Type", @"", ((global::System.String)o.@Type));
            }
            if (((global::System.Boolean)o.@Required) != true) {
                WriteAttribute(@"Required", @"", System.Xml.XmlConvert.ToString((global::System.Boolean)((global::System.Boolean)o.@Required)));
            }
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            Write12_Converter(@"Converter", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Converter)o.@Converter), false, false);
            Write14_ValueProvider(@"ValueProvider", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider)o.@ValueProvider), false, false);
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write14_ValueProvider(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Type", @"", ((global::System.String)o.@Type));
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
                global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument[])o.@MonitorArgument;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write13_MonitorArgument(@"MonitorArgument", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument)a[ia]), false, false);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write13_MonitorArgument(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
            WriteEndElement(o);
        }

        void Write12_Converter(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Converter o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Converter)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Type", @"", ((global::System.String)o.@Type));
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write11_RecipeHostData(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            if (((global::System.Int32)o.@Priority) != 1536) {
                WriteAttribute(@"Priority", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@Priority)));
            }
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            Write3_Icon(@"Icon", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Icon)o.@Icon), false, false);
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar[])o.@CommandBar;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write5_CommandBar(@"CommandBar", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar)a[ia]), false, false);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write5_CommandBar(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            if (o.@NameSpecified) {
                WriteAttribute(@"Name", @"", Write4_CommandBarName(((global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName)o.@Name)));
            }
            WriteAttribute(@"Menu", @"", ((global::System.String)o.@Menu));
            WriteAttribute(@"Guid", @"", ((global::System.String)o.@Guid));
            if (o.@IDSpecified) {
                WriteAttribute(@"ID", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@ID)));
            }
            if (o.@NameSpecified) {
            }
            if (o.@IDSpecified) {
            }
            WriteEndElement(o);
        }

        string Write4_CommandBarName(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName v) {
            string s = null;
            switch (v) {
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Solution: s = @"Solution"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Project: s = @"Project"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@WebProject: s = @"Web Project"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Item: s = @"Item"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@WebItem: s = @"Web Item"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Folder: s = @"Folder"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@WebFolder: s = @"Web Folder"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@SolutionFolder: s = @"Solution Folder"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@ProjectAdd: s = @"Project Add"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@SolutionAdd: s = @"Solution Add"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@SolutionFolderAdd: s = @"Solution Folder Add"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Microsoft.Practices.RecipeFramework.Configuration.CommandBarName");
            }
            return s;
        }

        void Write3_Icon(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Icon o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Icon)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Guid", @"", ((global::System.String)o.@Guid));
            if (o.@IDSpecified) {
                WriteAttribute(@"ID", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@ID)));
            }
            WriteAttribute(@"File", @"", ((global::System.String)o.@File));
            if (o.@IDSpecified) {
            }
            WriteEndElement(o);
        }

        void Write10_TypeAlias(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", ((global::System.String)o.@Name));
            WriteAttribute(@"Type", @"", ((global::System.String)o.@Type));
            WriteEndElement(o);
        }

        void Write9_Link(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Link o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Link)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Url", @"", ((global::System.String)o.@Url));
            WriteAttribute(@"Caption", @"", ((global::System.String)o.@Caption));
            if (o.@KindSpecified) {
                WriteAttribute(@"Kind", @"", Write8_DocumentationKind(((global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind)o.@Kind)));
            }
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            if (o.@KindSpecified) {
            }
            WriteEndElement(o);
        }

        string Write8_DocumentationKind(global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind v) {
            string s = null;
            switch (v) {
                case global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind.@Documentation: s = @"Documentation"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind.@NextStep: s = @"NextStep"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind");
            }
            return s;
        }

        void Write7_GuidancePackageHostData(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteElementString(@"RegistrationSettings", @"http://schemas.microsoft.com/pag/gax-core", ((global::System.String)o.@RegistrationSettings));
            Write3_Icon(@"Icon", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Icon)o.@Icon), false, false);
            {
                global::Microsoft.Practices.RecipeFramework.Configuration.Menu[] a = (global::Microsoft.Practices.RecipeFramework.Configuration.Menu[])o.@Menu;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write6_Menu(@"Menu", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.Menu)a[ia]), false, false);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        void Write6_Menu(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Menu o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Menu)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Name", @"", FromXmlNCName(((global::System.String)o.@Name)));
            WriteAttribute(@"Text", @"", ((global::System.String)o.@Text));
            if (((global::System.Int32)o.@Priority) != 1536) {
                WriteAttribute(@"Priority", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@Priority)));
            }
            Write5_CommandBar(@"CommandBar", @"http://schemas.microsoft.com/pag/gax-core", ((global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar)o.@CommandBar), false, false);
            WriteEndElement(o);
        }

        void Write2_Overview(string n, string ns, global::Microsoft.Practices.RecipeFramework.Configuration.Overview o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Overview)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"http://schemas.microsoft.com/pag/gax-core");
            WriteAttribute(@"Url", @"", ((global::System.String)o.@Url));
            {
                global::System.Xml.XmlAttribute[] a = (global::System.Xml.XmlAttribute[])o.@AnyAttr;
                if (a != null) {
                    for (int i = 0; i < a.Length; i++) {
                        global::System.Xml.XmlAttribute ai = (global::System.Xml.XmlAttribute)a[i];
                        WriteXmlAttribute(ai, o);
                    }
                }
            }
            if ((o.@Any) is System.Xml.XmlNode || o.@Any == null) {
                WriteElementLiteral((System.Xml.XmlNode)o.@Any, @"", null, false, true);
            }
            else {
                throw CreateInvalidAnyTypeException(o.@Any);
            }
            WriteEndElement(o);
        }

        string Write22_SourceLevels(global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels v) {
            string s = null;
            switch (v) {
                case global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Error: s = @"Error"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Information: s = @"Information"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Off: s = @"Off"; break;
				case global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Critical: s = @"Critical"; break;
				case global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Warning: s = @"Warning"; break;
                case global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Verbose: s = @"Verbose"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Microsoft.Practices.RecipeFramework.Configuration.SourceLevels");
            }
            return s;
        }

        protected override void InitCallbacks() {
        }
    }
	}

    internal partial class GuidancePackageSerializer {
	internal class BaseReader : System.Xml.Serialization.XmlSerializationReader {

        protected internal object Read24_GuidancePackage() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id1_GuidancePackage && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read23_GuidancePackage(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:GuidancePackage");
            }
            return (object)o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage Read23_GuidancePackage(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage();
            global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[] a_2 = null;
            int ca_2 = 0;
            global::System.Xml.XmlAttribute[] a_13 = null;
            int ca_13 = 0;
            bool[] paramsRead = new bool[14];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[3] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = ToXmlNCName(Reader.Value);
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id5_Caption && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Caption = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id6_Description && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Description = Reader.Value;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id7_Version && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Version = Reader.Value;
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id8_Guid && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Guid = Reader.Value;
                    paramsRead[7] = true;
                }
                else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id9_SchemaVersion && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@SchemaVersion = Reader.Value;
                    paramsRead[8] = true;
                }
                else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id10_Host && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Host = Reader.Value;
                    paramsRead[9] = true;
                }
                else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id11_BindingRecipe && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@BindingRecipe = ToXmlNCName(Reader.Value);
                    paramsRead[10] = true;
                }
                else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id12_SourceLevels && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@SourceLevels = Read22_SourceLevels(Reader.Value);
                    paramsRead[11] = true;
                }
                else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id13_SortPriority && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@SortPriority = System.Xml.XmlConvert.ToInt32(Reader.Value);
                    paramsRead[12] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_13 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_13, ca_13, typeof(global::System.Xml.XmlAttribute));a_13[ca_13++] = attr;
                }
            }
            o.@Recipes = (global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[])ShrinkArray(a_2, ca_2, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Recipe), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_13, ca_13, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_13, ca_13, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id14_Overview && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@Overview = Read2_Overview(false, true);
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id15_HostData && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@HostData = Read7_GuidancePackageHostData(false, true);
                        paramsRead[1] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id16_Recipes && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[] a_2_0 = null;
                            int ca_2_0 = 0;
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
                                        if (((object) Reader.LocalName == (object)id17_Recipe && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_2_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[])EnsureArrayIndex(a_2_0, ca_2_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Recipe));a_2_0[ca_2_0++] = Read21_Recipe(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:Recipe");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:Recipe");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations1, ref readerCount1);
                                }
                            ReadEndElement();
                            }
                            o.@Recipes = (global::Microsoft.Practices.RecipeFramework.Configuration.Recipe[])ShrinkArray(a_2_0, ca_2_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Recipe), false);
                        }
                    }
                    else {
                        UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:Overview, http://schemas.microsoft.com/pag/gax-core:HostData, http://schemas.microsoft.com/pag/gax-core:Recipes");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:Overview, http://schemas.microsoft.com/pag/gax-core:HostData, http://schemas.microsoft.com/pag/gax-core:Recipes");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_13, ca_13, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Recipe Read21_Recipe(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Recipe o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Recipe();
            global::Microsoft.Practices.RecipeFramework.Configuration.Link[] a_0 = null;
            int ca_0 = 0;
            global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[] a_1 = null;
            int ca_1 = 0;
            global::Microsoft.Practices.RecipeFramework.Configuration.Argument[] a_5 = null;
            int ca_5 = 0;
            global::System.Xml.XmlAttribute[] a_11 = null;
            int ca_11 = 0;
            bool[] paramsRead = new bool[12];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[8] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = ToXmlNCName(Reader.Value);
                    paramsRead[8] = true;
                }
                else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id18_Recurrent && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Recurrent = System.Xml.XmlConvert.ToBoolean(Reader.Value);
                    paramsRead[9] = true;
                }
                else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id19_Bound && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Bound = System.Xml.XmlConvert.ToBoolean(Reader.Value);
                    paramsRead[10] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_11 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_11, ca_11, typeof(global::System.Xml.XmlAttribute));a_11[ca_11++] = attr;
                }
            }
            o.@DocumentationLinks = (global::Microsoft.Practices.RecipeFramework.Configuration.Link[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Link), true);
            o.@Types = (global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias), true);
            o.@Arguments = (global::Microsoft.Practices.RecipeFramework.Configuration.Argument[])ShrinkArray(a_5, ca_5, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Argument), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_11, ca_11, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_11, ca_11, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations2 = 0;
            int readerCount2 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id20_DocumentationLinks && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Microsoft.Practices.RecipeFramework.Configuration.Link[] a_0_0 = null;
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
                                        if (((object) Reader.LocalName == (object)id21_Link && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_0_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.Link[])EnsureArrayIndex(a_0_0, ca_0_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Link));a_0_0[ca_0_0++] = Read9_Link(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:Link");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:Link");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations3, ref readerCount3);
                                }
                            ReadEndElement();
                            }
                            o.@DocumentationLinks = (global::Microsoft.Practices.RecipeFramework.Configuration.Link[])ShrinkArray(a_0_0, ca_0_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Link), false);
                        }
                    }
                    else if (((object) Reader.LocalName == (object)id22_Types && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[] a_1_0 = null;
                            int ca_1_0 = 0;
                            if ((Reader.IsEmptyElement)) {
                                Reader.Skip();
                            }
                            else {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations4 = 0;
                                int readerCount4 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                        if (((object) Reader.LocalName == (object)id23_TypeAlias && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_1_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[])EnsureArrayIndex(a_1_0, ca_1_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias));a_1_0[ca_1_0++] = Read10_TypeAlias(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:TypeAlias");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:TypeAlias");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations4, ref readerCount4);
                                }
                            ReadEndElement();
                            }
                            o.@Types = (global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias[])ShrinkArray(a_1_0, ca_1_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias), false);
                        }
                    }
                    else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id5_Caption && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        {
                            o.@Caption = Reader.ReadElementString();
                        }
                        paramsRead[2] = true;
                    }
                    else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id6_Description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        {
                            o.@Description = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id15_HostData && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@HostData = Read11_RecipeHostData(false, true);
                        paramsRead[4] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id24_Arguments && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Microsoft.Practices.RecipeFramework.Configuration.Argument[] a_5_0 = null;
                            int ca_5_0 = 0;
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
                                        if (((object) Reader.LocalName == (object)id25_Argument && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_5_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.Argument[])EnsureArrayIndex(a_5_0, ca_5_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Argument));a_5_0[ca_5_0++] = Read15_Argument(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:Argument");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @"http://schemas.microsoft.com/pag/gax-core:Argument");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations5, ref readerCount5);
                                }
                            ReadEndElement();
                            }
                            o.@Arguments = (global::Microsoft.Practices.RecipeFramework.Configuration.Argument[])ShrinkArray(a_5_0, ca_5_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Argument), false);
                        }
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id26_GatheringServiceData && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@GatheringServiceData = Read16_RecipeGatheringServiceData(false, true);
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id27_Actions && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@Actions = Read20_RecipeActions(false, true);
                        paramsRead[7] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:DocumentationLinks, http://schemas.microsoft.com/pag/gax-core:Types, http://schemas.microsoft.com/pag/gax-core:Caption, http://schemas.microsoft.com/pag/gax-core:Description, http://schemas.microsoft.com/pag/gax-core:HostData, http://schemas.microsoft.com/pag/gax-core:Arguments, http://schemas.microsoft.com/pag/gax-core:GatheringServiceData, http://schemas.microsoft.com/pag/gax-core:Actions");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:DocumentationLinks, http://schemas.microsoft.com/pag/gax-core:Types, http://schemas.microsoft.com/pag/gax-core:Caption, http://schemas.microsoft.com/pag/gax-core:Description, http://schemas.microsoft.com/pag/gax-core:HostData, http://schemas.microsoft.com/pag/gax-core:Arguments, http://schemas.microsoft.com/pag/gax-core:GatheringServiceData, http://schemas.microsoft.com/pag/gax-core:Actions");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations2, ref readerCount2);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_11, ca_11, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions Read20_RecipeActions(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.RecipeActions();
            global::Microsoft.Practices.RecipeFramework.Configuration.Action[] a_0 = null;
            int ca_0 = 0;
            global::System.Xml.XmlAttribute[] a_4 = null;
            int ca_4 = 0;
            bool[] paramsRead = new bool[5];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id28_CoordinatorServiceType && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@CoordinatorServiceType = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id29_ExecutionServiceType && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@ExecutionServiceType = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_4 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_4, ca_4, typeof(global::System.Xml.XmlAttribute));a_4[ca_4++] = attr;
                }
            }
            o.@Action = (global::Microsoft.Practices.RecipeFramework.Configuration.Action[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Action), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@Action = (global::Microsoft.Practices.RecipeFramework.Configuration.Action[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Action), true);
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations6 = 0;
            int readerCount6 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id30_Action && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.Action[])EnsureArrayIndex(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Action));a_0[ca_0++] = Read19_Action(false, true);
                    }
                    else {
                        o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:Action");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations6, ref readerCount6);
            }
            o.@Action = (global::Microsoft.Practices.RecipeFramework.Configuration.Action[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Action), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Action Read19_Action(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Action o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Action();
            global::Microsoft.Practices.RecipeFramework.Configuration.Input[] a_0 = null;
            int ca_0 = 0;
            global::Microsoft.Practices.RecipeFramework.Configuration.Output[] a_1 = null;
            int ca_1 = 0;
            global::System.Xml.XmlAttribute[] a_5 = null;
            int ca_5 = 0;
            bool[] paramsRead = new bool[6];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[3] && ((object) Reader.LocalName == (object)id31_Type && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Type = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_5 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_5, ca_5, typeof(global::System.Xml.XmlAttribute));a_5[ca_5++] = attr;
                }
            }
            o.@Input = (global::Microsoft.Practices.RecipeFramework.Configuration.Input[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Input), true);
            o.@Output = (global::Microsoft.Practices.RecipeFramework.Configuration.Output[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Output), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_5, ca_5, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@Input = (global::Microsoft.Practices.RecipeFramework.Configuration.Input[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Input), true);
                o.@Output = (global::Microsoft.Practices.RecipeFramework.Configuration.Output[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Output), true);
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_5, ca_5, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations7 = 0;
            int readerCount7 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id32_Input && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.Input[])EnsureArrayIndex(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Input));a_0[ca_0++] = Read17_Input(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id33_Output && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Microsoft.Practices.RecipeFramework.Configuration.Output[])EnsureArrayIndex(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Output));a_1[ca_1++] = Read18_Output(false, true);
                    }
                    else {
                        o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:Input, http://schemas.microsoft.com/pag/gax-core:Output");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations7, ref readerCount7);
            }
            o.@Input = (global::Microsoft.Practices.RecipeFramework.Configuration.Input[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Input), true);
            o.@Output = (global::Microsoft.Practices.RecipeFramework.Configuration.Output[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Output), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_5, ca_5, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Output Read18_Output(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Output o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Output();
            global::System.Xml.XmlAttribute[] a_2 = null;
            int ca_2 = 0;
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_2 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_2, ca_2, typeof(global::System.Xml.XmlAttribute));a_2[ca_2++] = attr;
                }
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations8 = 0;
            int readerCount8 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations8, ref readerCount8);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Input Read17_Input(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Input o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Input();
            global::System.Xml.XmlAttribute[] a_4 = null;
            int ca_4 = 0;
            bool[] paramsRead = new bool[5];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id34_RecipeArgument && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@RecipeArgument = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id35_ActionOutput && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@ActionOutput = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_4 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_4, ca_4, typeof(global::System.Xml.XmlAttribute));a_4[ca_4++] = attr;
                }
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations9 = 0;
            int readerCount9 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations9, ref readerCount9);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData Read16_RecipeGatheringServiceData(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.RecipeGatheringServiceData();
            global::System.Xml.XmlAttribute[] a_2 = null;
            int ca_2 = 0;
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id36_ServiceType && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@ServiceType = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_2 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_2, ca_2, typeof(global::System.Xml.XmlAttribute));a_2[ca_2++] = attr;
                }
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations10 = 0;
            int readerCount10 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations10, ref readerCount10);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Argument Read15_Argument(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Argument o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Argument();
            global::System.Xml.XmlAttribute[] a_6 = null;
            int ca_6 = 0;
            bool[] paramsRead = new bool[7];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[3] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id31_Type && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Type = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id37_Required && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Required = System.Xml.XmlConvert.ToBoolean(Reader.Value);
                    paramsRead[5] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_6 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_6, ca_6, typeof(global::System.Xml.XmlAttribute));a_6[ca_6++] = attr;
                }
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_6, ca_6, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_6, ca_6, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations11 = 0;
            int readerCount11 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id38_Converter && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@Converter = Read12_Converter(false, true);
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id39_ValueProvider && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@ValueProvider = Read14_ValueProvider(false, true);
                        paramsRead[1] = true;
                    }
                    else {
                        o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:Converter, http://schemas.microsoft.com/pag/gax-core:ValueProvider");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations11, ref readerCount11);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_6, ca_6, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider Read14_ValueProvider(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.ValueProvider();
            global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument[] a_0 = null;
            int ca_0 = 0;
            global::System.Xml.XmlAttribute[] a_3 = null;
            int ca_3 = 0;
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id31_Type && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Type = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_3 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_3, ca_3, typeof(global::System.Xml.XmlAttribute));a_3[ca_3++] = attr;
                }
            }
            o.@MonitorArgument = (global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_3, ca_3, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@MonitorArgument = (global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument), true);
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_3, ca_3, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations12 = 0;
            int readerCount12 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id40_MonitorArgument && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_0 = (global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument[])EnsureArrayIndex(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument));a_0[ca_0++] = Read13_MonitorArgument(false, true);
                    }
                    else {
                        o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:MonitorArgument");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations12, ref readerCount12);
            }
            o.@MonitorArgument = (global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument[])ShrinkArray(a_0, ca_0, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_3, ca_3, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument Read13_MonitorArgument(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.MonitorArgument();
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Name");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations13 = 0;
            int readerCount13 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations13, ref readerCount13);
            }
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Converter Read12_Converter(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Converter o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Converter();
            global::System.Xml.XmlAttribute[] a_2 = null;
            int ca_2 = 0;
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id31_Type && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Type = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_2 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_2, ca_2, typeof(global::System.Xml.XmlAttribute));a_2[ca_2++] = attr;
                }
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations14 = 0;
            int readerCount14 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations14, ref readerCount14);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData Read11_RecipeHostData(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.RecipeHostData();
            global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar[] a_1 = null;
            int ca_1 = 0;
            global::System.Xml.XmlAttribute[] a_4 = null;
            int ca_4 = 0;
            bool[] paramsRead = new bool[5];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[3] && ((object) Reader.LocalName == (object)id41_Priority && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Priority = System.Xml.XmlConvert.ToInt32(Reader.Value);
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_4 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_4, ca_4, typeof(global::System.Xml.XmlAttribute));a_4[ca_4++] = attr;
                }
            }
            o.@CommandBar = (global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@CommandBar = (global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar), true);
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations15 = 0;
            int readerCount15 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id42_Icon && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@Icon = Read3_Icon(false, true);
                        paramsRead[0] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id43_CommandBar && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar[])EnsureArrayIndex(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar));a_1[ca_1++] = Read5_CommandBar(false, true);
                    }
                    else {
                        o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:Icon, http://schemas.microsoft.com/pag/gax-core:CommandBar");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations15, ref readerCount15);
            }
            o.@CommandBar = (global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar[])ShrinkArray(a_1, ca_1, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar), true);
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_4, ca_4, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar Read5_CommandBar(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.CommandBar();
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Read4_CommandBarName(Reader.Value);
                    o.@NameSpecified = true;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id44_Menu && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Menu = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id8_Guid && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Guid = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id45_ID && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@ID = System.Xml.XmlConvert.ToInt32(Reader.Value);
                    o.@IDSpecified = true;
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Name, :Menu, :Guid, :ID");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations16 = 0;
            int readerCount16 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations16, ref readerCount16);
            }
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName Read4_CommandBarName(string s) {
            switch (s) {
                case @"Solution": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Solution;
                case @"Project": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Project;
                case @"Web Project": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@WebProject;
                case @"Item": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Item;
                case @"Web Item": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@WebItem;
                case @"Folder": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@Folder;
                case @"Web Folder": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@WebFolder;
                case @"Solution Folder": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@SolutionFolder;
                case @"Project Add": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@ProjectAdd;
                case @"Solution Add": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@SolutionAdd;
                case @"Solution Folder Add": return global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName.@SolutionFolderAdd;
                default: throw CreateUnknownConstantException(s, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.CommandBarName));
            }
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Icon Read3_Icon(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Icon o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Icon();
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id8_Guid && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Guid = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id45_ID && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@ID = System.Xml.XmlConvert.ToInt32(Reader.Value);
                    o.@IDSpecified = true;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id46_File && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@File = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Guid, :ID, :File");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations17 = 0;
            int readerCount17 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations17, ref readerCount17);
            }
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias Read10_TypeAlias(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.TypeAlias();
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id31_Type && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Type = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Name, :Type");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations18 = 0;
            int readerCount18 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations18, ref readerCount18);
            }
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Link Read9_Link(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Link o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Link();
            global::System.Xml.XmlAttribute[] a_3 = null;
            int ca_3 = 0;
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id47_Url && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Url = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id5_Caption && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Caption = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id48_Kind && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Kind = Read8_DocumentationKind(Reader.Value);
                    o.@KindSpecified = true;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_3 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_3, ca_3, typeof(global::System.Xml.XmlAttribute));a_3[ca_3++] = attr;
                }
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_3, ca_3, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_3, ca_3, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations19 = 0;
            int readerCount19 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations19, ref readerCount19);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_3, ca_3, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind Read8_DocumentationKind(string s) {
            switch (s) {
                case @"Documentation": return global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind.@Documentation;
                case @"NextStep": return global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind.@NextStep;
                default: throw CreateUnknownConstantException(s, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.DocumentationKind));
            }
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData Read7_GuidancePackageHostData(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageHostData();
            global::Microsoft.Practices.RecipeFramework.Configuration.Menu[] a_2 = null;
            int ca_2 = 0;
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@Menu = (global::Microsoft.Practices.RecipeFramework.Configuration.Menu[])ShrinkArray(a_2, ca_2, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Menu), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations20 = 0;
            int readerCount20 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id49_RegistrationSettings && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        {
                            o.@RegistrationSettings = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id42_Icon && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@Icon = Read3_Icon(false, true);
                        paramsRead[1] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id44_Menu && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_2 = (global::Microsoft.Practices.RecipeFramework.Configuration.Menu[])EnsureArrayIndex(a_2, ca_2, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Menu));a_2[ca_2++] = Read6_Menu(false, true);
                    }
                    else {
                        o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:RegistrationSettings, http://schemas.microsoft.com/pag/gax-core:Icon, http://schemas.microsoft.com/pag/gax-core:Menu");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations20, ref readerCount20);
            }
            o.@Menu = (global::Microsoft.Practices.RecipeFramework.Configuration.Menu[])ShrinkArray(a_2, ca_2, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.Menu), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Menu Read6_Menu(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Menu o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Menu();
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id4_Name && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Name = ToXmlNCName(Reader.Value);
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id50_Text && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Text = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id41_Priority && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Priority = System.Xml.XmlConvert.ToInt32(Reader.Value);
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":Name, :Text, :Priority");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations21 = 0;
            int readerCount21 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id43_CommandBar && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@CommandBar = Read5_CommandBar(false, true);
                        paramsRead[0] = true;
                    }
                    else {
                        UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:CommandBar");
                    }
                }
                else {
                    UnknownNode((object)o, @"http://schemas.microsoft.com/pag/gax-core:CommandBar");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations21, ref readerCount21);
            }
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.Overview Read2_Overview(bool isNullable, bool checkType) {
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
            global::Microsoft.Practices.RecipeFramework.Configuration.Overview o;
            o = new global::Microsoft.Practices.RecipeFramework.Configuration.Overview();
            global::System.Xml.XmlAttribute[] a_2 = null;
            int ca_2 = 0;
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id47_Url && (object) Reader.NamespaceURI == (object)id3_Item)) {
                    o.@Url = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);
                    ParseWsdlArrayType(attr);
                    a_2 = (global::System.Xml.XmlAttribute[])EnsureArrayIndex(a_2, ca_2, typeof(global::System.Xml.XmlAttribute));a_2[ca_2++] = attr;
                }
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations22 = 0;
            int readerCount22 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    o.@Any = (global::System.Xml.XmlElement)ReadXmlNode(false);
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations22, ref readerCount22);
            }
            o.@AnyAttr = (global::System.Xml.XmlAttribute[])ShrinkArray(a_2, ca_2, typeof(global::System.Xml.XmlAttribute), true);
            ReadEndElement();
            return o;
        }

        /// <remarks/>
        protected virtual global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels Read22_SourceLevels(string s) {
            switch (s) {
                case @"Error": return global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Error;
                case @"Information": return global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Information;
                case @"Off": return global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Off;
				case @"Critical": return global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Critical;
				case @"Warning": return global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Warning;
                case @"Verbose": return global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels.@Verbose;
                default: throw CreateUnknownConstantException(s, typeof(global::Microsoft.Practices.RecipeFramework.Configuration.SourceLevels));
            }
        }

        protected override void InitCallbacks() {
        }

        string id21_Link;
        string id38_Converter;
        string id16_Recipes;
        string id3_Item;
        string id10_Host;
        string id6_Description;
        string id24_Arguments;
        string id49_RegistrationSettings;
        string id14_Overview;
        string id46_File;
        string id40_MonitorArgument;
        string id44_Menu;
        string id4_Name;
        string id17_Recipe;
        string id15_HostData;
        string id22_Types;
        string id34_RecipeArgument;
        string id19_Bound;
        string id50_Text;
        string id20_DocumentationLinks;
        string id5_Caption;
        string id26_GatheringServiceData;
        string id11_BindingRecipe;
        string id36_ServiceType;
        string id43_CommandBar;
        string id7_Version;
        string id25_Argument;
        string id9_SchemaVersion;
        string id42_Icon;
        string id35_ActionOutput;
        string id32_Input;
        string id41_Priority;
        string id1_GuidancePackage;
        string id29_ExecutionServiceType;
        string id28_CoordinatorServiceType;
        string id2_Item;
        string id47_Url;
        string id45_ID;
        string id31_Type;
        string id39_ValueProvider;
        string id27_Actions;
        string id12_SourceLevels;
        string id33_Output;
        string id48_Kind;
        string id30_Action;
        string id13_SortPriority;
        string id23_TypeAlias;
        string id18_Recurrent;
        string id8_Guid;
        string id37_Required;

        protected override void InitIDs() {
            id21_Link = Reader.NameTable.Add(@"Link");
            id38_Converter = Reader.NameTable.Add(@"Converter");
            id16_Recipes = Reader.NameTable.Add(@"Recipes");
            id3_Item = Reader.NameTable.Add(@"");
            id10_Host = Reader.NameTable.Add(@"Host");
            id6_Description = Reader.NameTable.Add(@"Description");
            id24_Arguments = Reader.NameTable.Add(@"Arguments");
            id49_RegistrationSettings = Reader.NameTable.Add(@"RegistrationSettings");
            id14_Overview = Reader.NameTable.Add(@"Overview");
            id46_File = Reader.NameTable.Add(@"File");
            id40_MonitorArgument = Reader.NameTable.Add(@"MonitorArgument");
            id44_Menu = Reader.NameTable.Add(@"Menu");
            id4_Name = Reader.NameTable.Add(@"Name");
            id17_Recipe = Reader.NameTable.Add(@"Recipe");
            id15_HostData = Reader.NameTable.Add(@"HostData");
            id22_Types = Reader.NameTable.Add(@"Types");
            id34_RecipeArgument = Reader.NameTable.Add(@"RecipeArgument");
            id19_Bound = Reader.NameTable.Add(@"Bound");
            id50_Text = Reader.NameTable.Add(@"Text");
            id20_DocumentationLinks = Reader.NameTable.Add(@"DocumentationLinks");
            id5_Caption = Reader.NameTable.Add(@"Caption");
            id26_GatheringServiceData = Reader.NameTable.Add(@"GatheringServiceData");
            id11_BindingRecipe = Reader.NameTable.Add(@"BindingRecipe");
            id36_ServiceType = Reader.NameTable.Add(@"ServiceType");
            id43_CommandBar = Reader.NameTable.Add(@"CommandBar");
            id7_Version = Reader.NameTable.Add(@"Version");
            id25_Argument = Reader.NameTable.Add(@"Argument");
            id9_SchemaVersion = Reader.NameTable.Add(@"SchemaVersion");
            id42_Icon = Reader.NameTable.Add(@"Icon");
            id35_ActionOutput = Reader.NameTable.Add(@"ActionOutput");
            id32_Input = Reader.NameTable.Add(@"Input");
            id41_Priority = Reader.NameTable.Add(@"Priority");
            id1_GuidancePackage = Reader.NameTable.Add(@"GuidancePackage");
            id29_ExecutionServiceType = Reader.NameTable.Add(@"ExecutionServiceType");
            id28_CoordinatorServiceType = Reader.NameTable.Add(@"CoordinatorServiceType");
            id2_Item = Reader.NameTable.Add(@"http://schemas.microsoft.com/pag/gax-core");
            id47_Url = Reader.NameTable.Add(@"Url");
            id45_ID = Reader.NameTable.Add(@"ID");
            id31_Type = Reader.NameTable.Add(@"Type");
            id39_ValueProvider = Reader.NameTable.Add(@"ValueProvider");
            id27_Actions = Reader.NameTable.Add(@"Actions");
				id12_SourceLevels = Reader.NameTable.Add(@"SourceLevels");
            id33_Output = Reader.NameTable.Add(@"Output");
            id48_Kind = Reader.NameTable.Add(@"Kind");
            id30_Action = Reader.NameTable.Add(@"Action");
            id13_SortPriority = Reader.NameTable.Add(@"SortPriority");
            id23_TypeAlias = Reader.NameTable.Add(@"TypeAlias");
            id18_Recurrent = Reader.NameTable.Add(@"Recurrent");
            id8_Guid = Reader.NameTable.Add(@"Guid");
            id37_Required = Reader.NameTable.Add(@"Required");
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
            return xmlReader.IsStartElement(@"GuidancePackage", @"http://schemas.microsoft.com/pag/gax-core");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((BaseWriter)writer).Write24_GuidancePackage(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((BaseReader)reader).Read24_GuidancePackage();
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
                    _tmp[@"Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage:http://schemas.microsoft.com/pag/gax-core::False:"] = @"Read24_GuidancePackage";
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
                    _tmp[@"Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage:http://schemas.microsoft.com/pag/gax-core::False:"] = @"Write24_GuidancePackage";
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
                    _tmp.Add(@"Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage:http://schemas.microsoft.com/pag/gax-core::False:", new GuidancePackageSerializer());
                    if (typedSerializers == null) typedSerializers = _tmp;
                }
                return typedSerializers;
            }
        }
        public override System.Boolean CanSerialize(System.Type type) {
            if (type == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage)) return true;
            return false;
        }
        public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
            if (type == typeof(global::Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage)) return new GuidancePackageSerializer();
            return null;
        }
    }
	}
}


#pragma warning restore 0642, 0219
