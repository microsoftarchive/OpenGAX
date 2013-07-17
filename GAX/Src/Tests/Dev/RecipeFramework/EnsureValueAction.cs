using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Common;

namespace Microsoft.Practices.RecipeFramework
{
	public class EnsureValueAction : Action, IAttributesConfigurable
	{
        [Input]
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        } string value;

        string valueFromConfig;

        public override void Execute()
		{
			Assert.IsNotNull(Value);
            Assert.AreEqual(Value, valueFromConfig);
		}

		public override void Undo()
		{
		}

		#region IAttributesConfigurable Members

		public void Configure(System.Collections.Specialized.StringDictionary attributes)
		{
			valueFromConfig = attributes["Value"];
		}

		#endregion
	}
}
