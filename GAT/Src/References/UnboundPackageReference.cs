using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using System.Xml;
using Microsoft.Practices.RecipeFramework.Configuration;
using System.Runtime.Serialization;

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.References
{
	/// <summary>
	/// Reference that only applies to guidance package projects.
	/// </summary>
	[Serializable]
	public class UnboundPackageReference : UnboundRecipeReference
	{
		/// <summary>
		/// Default constructor receiving the recipe name.
		/// </summary>
		public UnboundPackageReference(string recipe) : base(recipe) {}

		/// <summary>
		/// Required constructor for deserialization.
		/// </summary>
		protected UnboundPackageReference(SerializationInfo info, StreamingContext context) : base(info, context) {}

		/// <summary>
		/// Determines whether the target is a guidance package project by 
		/// looking at its top-level items for an XML file containing the root 
		/// node of a guidance package configuration file.
		/// </summary>
		public override bool IsEnabledFor(object target)
		{
			Project targetProject = target as Project;
			if (targetProject != null && targetProject.ProjectItems != null)
			{
				foreach (ProjectItem item in targetProject.ProjectItems)
				{
                    if (item.get_FileNames(1) != null)
                    {
                        if (item.get_FileNames(1).EndsWith(".xml"))
                        {
                            try
                            {
                                using (XmlReader reader = XmlReader.Create(item.get_FileNames(1)))
                                {
                                    bool isPackageNode =
                                        reader.MoveToContent() == XmlNodeType.Element &&
                                        reader.NamespaceURI == SchemaInfo.PackageNamespace &&
                                        reader.LocalName == ElementNames.GuidancePackage;

                                    if (isPackageNode)
                                    {
                                        return true;
                                    }
                                }
                            }
                            catch { } // It wasn't an XML file.			
                        }
                    }
				}
			}

			return false;
		}

		/// <summary>
		/// Friendly representation of the condition this reference expresses.
		/// </summary>
		public override string AppliesTo
		{
			get { return Properties.Resources.UnboundPackageReference_AppliesTo; }
		}
	}
}
