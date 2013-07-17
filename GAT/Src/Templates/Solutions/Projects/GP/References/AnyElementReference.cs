using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using System.Runtime.Serialization;

namespace $PackageNamespace$.References
{
	[Serializable]
	public class AnyElementReference : UnboundRecipeReference
	{
		public AnyElementReference(string recipe) : base(recipe) {}

		protected AnyElementReference(SerializationInfo info, StreamingContext context) : base(info, context) { }

		public override bool IsEnabledFor(object target)
		{
			return true;
		}

		public override string AppliesTo
		{
			get { return "Any solution element"; }
		}
	}
}
