#region Using directives

using System;
using System.Collections;
using System.Xml;
using System.ComponentModel;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Mvp.Xml.XInclude;
using System.IO;

#endregion

namespace Microsoft.Practices.RecipeFramework.MockServices
{
	public class MockConfigurationService : SitedComponent, Services.IConfigurationService, IValueInfoService 
	{
		#region Delegate and Events

		public delegate Configuration.GuidancePackage GetCurrentPackageHandler();
		public delegate Configuration.Recipe GetCurrentRecipeHandler();

		public event GetCurrentPackageHandler OnGetCurrentPackage;
		public event GetCurrentRecipeHandler OnGetCurrentRecipe;

		#endregion Delegate and Events

		Configuration.GuidancePackage configuration;
		Hashtable metaData;

		public int CurrentRecipeIndex
		{
			get { return currentRecipeIndex; }
			set { currentRecipeIndex = value; }
		} int currentRecipeIndex;

		public MockConfigurationService(Configuration.GuidancePackage configuration)
		{
			this.configuration = configuration;
			this.metaData = new Hashtable();
		}

		public MockConfigurationService(string file)
		{
			MemoryStream mem = new MemoryStream();
			XIncludingReader xir = new XIncludingReader(file);
			XmlWriter writer = XmlWriter.Create(mem);
			writer.WriteNode(xir, false);
			writer.Flush();

			mem.Position = 0;
			XmlReader reader = XmlReader.Create(mem);
			
			configuration = (Configuration.GuidancePackage)
				new Configuration.Serialization.GuidancePackageSerializer().Deserialize(reader);

			this.metaData = new Hashtable();
		}

		#region IConfigurationService Members

		public Configuration.GuidancePackage CurrentPackage
		{
			get
			{
				if (OnGetCurrentPackage == null)
					return configuration;
				else
					return OnGetCurrentPackage();
			}
		}

		public Configuration.Recipe CurrentRecipe
		{
			get 
			{
				if (OnGetCurrentRecipe != null)
					return OnGetCurrentRecipe();
				else if (configuration != null)
					return configuration.Recipes[CurrentRecipeIndex];
				else
				return null;
			}
		}

		public object CurrentGatheringServiceData
		{
			get
			{
				if (configuration != null && CurrentRecipe != null && CurrentRecipe.GatheringServiceData != null)
				{
					return CurrentRecipe.GatheringServiceData;
				}
				return null;
			}
		}

		public string BasePath
		{
			get { return AppDomain.CurrentDomain.BaseDirectory; }
		}

		#endregion

        #region IValueInfoService members

        public ValueInfo GetInfo(string name)
		{
			Configuration.Argument arg = this.CurrentRecipe.ArgumentsByName[name];
			if (arg == null)
			{
				return null;
			}
			// We need to cache the ArgumentMetaData, otherwise the Wizard validation for repeated arguments will fail
			if (!metaData.ContainsKey(arg))
			{
                ITypeResolutionService typeResolutionService = 
                    (ITypeResolutionService)GetService(typeof(ITypeResolutionService));
                Type argType =null;
                if (typeResolutionService != null)
                {
                    argType = typeResolutionService.GetType(arg.Type);
                }
                else
                {
                    argType = Type.GetType(arg.Type);
                }
                TypeConverter converter = null;
                if (arg.Converter != null)
                {
                    Type converterType = null;
                    if (typeResolutionService != null)
                    {
                        converterType = typeResolutionService.GetType(arg.Converter.Type);
                    }
                    else
                    {
                        converterType = Type.GetType(arg.Converter.Type);
                    }
                    if ( converterType!=null)
                    {
                        converter = (TypeConverter)Activator.CreateInstance(converterType);
                    }
                }
                if (converter == null && argType!=null )
                {
                    TypeConverter cinstance = TypeDescriptor.GetConverter(argType);
                    if (argType.IsEnum)
                    {
                        converter = new EnumerationConverter(argType);
                    }
                    else if (argType.IsCOMObject ||
                            Attribute.GetCustomAttribute(argType, typeof(ComImportAttribute), true) != null)
                    {
                        converter = new ComObjectConverter(argType);
                    }
                    else if (cinstance != null && cinstance.GetType() != typeof(ComponentConverter))
                    {
                        // Only use converters that are not the generic ComponentConverter.
                        converter = cinstance;
                    }
                }
                ValueInfo argumentMetaData = new ValueInfo(
                    arg.Name,
                    arg.Required,
                    argType,
                    converter);
				metaData[arg] = argumentMetaData;
			}
			return (ValueInfo)metaData[arg];
		}

        public string ComponentName
        {
            get { return string.Empty; }
        }

		#endregion
	}
}