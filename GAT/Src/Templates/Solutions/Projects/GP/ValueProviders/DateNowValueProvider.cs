using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;

namespace $PackageNamespace$.ValueProviders
{
    public class DateNowValueProvider : ValueProvider
    {
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if(currentValue!=null)
            {
                 // Do not assign a new value, and return false to flag that 
                 // we don't want the current value to be changed.
                newValue = null;
                return false;
            }
            newValue = DateTime.Now;
            return true;
        }
    }
}
