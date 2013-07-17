#region Using directives

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

#endregion

namespace Microsoft.Practices.RecipeFramework.MockObjects
{
	[Serializable]
	public class MockReference : RecipeReference, IBoundAssetReference
	{
		object target;

		public MockReference(string recipe, object target) : base(recipe)
		{
			this.target = target;
		}

		#region ISerializable Members

		/// <summary>
		/// Initializes an instance of the <see cref="AbsoluteItemReference"/> class.
		/// </summary>
		/// <seealso cref="IRecipeReference"/>
		private MockReference(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
		}

		#endregion ISerializable Members

		public override string Key
		{
			get { return base.AssetName + "::" + target.ToString(); }
		}

		public override string AppliesTo
		{
			get { return this.target.ToString(); }
		}

		#region IBoundAssetReference Members

		public object Target
		{
			get { return target; }
		}

		public string SubPath
		{
			get { return target.ToString(); }
		}

		public IBoundReferenceLocatorStrategy Strategy
		{
			get { return null; }
		}

		#endregion
	}
}
