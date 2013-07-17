#region Using directives
using System;
using System.Drawing.Design;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework;
using System.Globalization;
using System.IO;
#endregion

namespace $PackageNamespace$.Editors
{

	public class TemplateChooser : FileChooser
	{
		public override string Title
		{
			get { return "Please choose a template file"; }
		}
		public override string FileFilter
		{
			get { return "Template files (*.vstemplate)|*.vstemplate"; }
		}

		public override string InitialDirectory
		{
			get { return initialDirectory; }
		} private string initialDirectory;

		public virtual string AdditionalSubPath
		{
			get { return null; }
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			GuidancePackage package = (GuidancePackage)provider.GetService(typeof(IExecutionService));
			string basepath = @"c:\";
			string temp = package.BasePath;
			if (Directory.Exists(temp))
			{
				basepath = temp;
				temp = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates", basepath);
				if (Directory.Exists(temp))
				{
					basepath = temp;
					if (!string.IsNullOrEmpty(AdditionalSubPath))
					{
						temp = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", basepath, AdditionalSubPath);
						if (Directory.Exists(temp))
						{
							basepath = temp;
						}
					}
				}
			}
			initialDirectory = basepath;
			return base.EditValue(context, provider, value);
		}
	}

	public class ItemTemplateChooser : TemplateChooser
	{
		public override string AdditionalSubPath
		{
			get { return "Items"; }
		}
	}

	public class ProjectTemplateChooser : TemplateChooser
	{
		public override string AdditionalSubPath
		{
			get { return "Projects"; }
		}
	}
}
