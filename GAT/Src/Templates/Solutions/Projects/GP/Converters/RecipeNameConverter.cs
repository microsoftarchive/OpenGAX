using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.Configuration;

namespace $PackageNamespace$.Converters
{
	public class RecipeNameConverter : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			IConfigurationService configuration = (IConfigurationService)context.GetService(typeof(IConfigurationService));
			List<string> recipeNames = new List<string>(configuration.CurrentPackage.Recipes.Length);

			foreach (Recipe recipe in configuration.CurrentPackage.Recipes)
			{
				if (recipe.Bound)
                {
                    recipeNames.Add(recipe.Name);
                }
			}

			return new StandardValuesCollection(recipeNames);
		}
	}
}
