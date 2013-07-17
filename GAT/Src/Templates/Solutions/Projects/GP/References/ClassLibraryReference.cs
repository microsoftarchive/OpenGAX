using System;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE;
using VSLangProj;

namespace $PackageNamespace$.References
{
    [Serializable]
    public class ClassLibraryReference : UnboundTemplateReference
    {
        public ClassLibraryReference(string template)
            : base(template)
        { 
        }

        public override bool IsEnabledFor(object target)
        {
            return target is Project && ((Project)target).Kind == PrjKind.prjKindCSharpProject &&
                (int)((Project)target).Properties.Item("OutputType").Value == (int)prjOutputType.prjOutputTypeLibrary;
        }

        public override string AppliesTo
        {
            get { return "All C# projects that are class libraries"; }
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected ClassLibraryReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion ISerializable Members
    }
}
