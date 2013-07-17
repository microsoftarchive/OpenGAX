using System;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE80;
using EnvDTE;

namespace $PackageNamespace$.References
{
    [Serializable]
    public class SolutionFolderAReference : UnboundTemplateReference
    {
        public SolutionFolderAReference(string template)
            : base(template)
        { 
        }

        public override bool IsEnabledFor(object target)
        {
            return target is Project && ((Project)target).Object is SolutionFolder &&
                ((Project)target).Name.StartsWith("A");
        }

        public override string AppliesTo
        {
            get { return "All solution folders starting with an 'A'"; }
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected SolutionFolderAReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion ISerializable Members
    }
}
