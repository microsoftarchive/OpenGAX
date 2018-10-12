using System;
using System.Runtime.Serialization;
using EnvDTE;
using Microsoft.Practices.Common;

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.References
{
    [Serializable]
    public class ProjectFolderReference : UnboundRecipeReference, IAttributesConfigurable
    {
        private const string FolderNameAttribute = "FolderName";
		public string folderName;

         #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected ProjectFolderReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
			try
			{
				this.folderName = info.GetString(FolderNameAttribute);
			}
			catch 
			{
				this.folderName = null;
			}
        }

		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			if(!string.IsNullOrEmpty(this.folderName))
				info.AddValue(FolderNameAttribute, folderName);
		}

        #endregion ISerializable Members

        public ProjectFolderReference(string recipe)
            : base(recipe)
        { }

        public override bool IsEnabledFor(object target)
        {
            if (!string.IsNullOrEmpty(folderName) && target != null)
            {
                ProjectItem projectItem = target as ProjectItem;
                return (projectItem != null && projectItem.Name == folderName);
            }

            return false;
        }

        public override string AppliesTo
        {
            get { return string.Format("Any project folder that matchs with the specified {0} name.", folderName); }
        }

        #region IAttributesConfigurable Members

        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes != null && attributes.ContainsKey(FolderNameAttribute))
            {
                folderName = attributes[FolderNameAttribute];
            }
        }

        #endregion
    }
}