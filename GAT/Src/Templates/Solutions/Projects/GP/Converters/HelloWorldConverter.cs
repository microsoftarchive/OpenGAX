using System;
using System.ComponentModel;

namespace $PackageNamespace$.Converters
{
    /// <summary>
    /// Sample converter that returns a list of optional values to pick from, but 
    /// validates that the value entered starts with "Hello ".
    /// </summary>
	public class HelloWorldConverter : TypeConverter
    {
        static string[] validValues = new string[] {
            "Hello Guidance Automation Toolkit!", "Hello Microsoft!", "Hello World!" };

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return (value is string && ((string)value).StartsWith("Hello "));
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(validValues);
        }
	}
}
