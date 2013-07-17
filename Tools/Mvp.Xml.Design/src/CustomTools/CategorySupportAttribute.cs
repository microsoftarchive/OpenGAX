using System;

namespace Mvp.Xml.Design.CustomTools
{
	/// <summary>
	/// Determines which VS.NET generator categories are supported by the custom tool.
	/// This class also contains constants for C# and VB.NET category guids.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class CategorySupportAttribute : Attribute
	{
		Guid _category;

		/// <summary>
		/// VS Generator Category for C# Language.
		/// </summary>
		public const string CSharpCategory = "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}";

		/// <summary>
		/// VS Generator Category for VB Language.
		/// </summary>
		public const string VBCategory = "{164B10B9-B200-11D0-8C61-00A0C91E29D5}";

		/// <summary>
		/// Initializes the attribute.
		/// </summary>
		/// <param name="categoryGuid">
		/// Either <see cref="CSharpCategory"/> or <see cref="VBCategory"/>.
		/// </param>
		public CategorySupportAttribute(string categoryGuid)
		{
			_category = new Guid(categoryGuid);
		}

		/// <summary>
		/// The identifier of the supported category.
		/// </summary>
		public Guid Guid
		{
			get { return _category; }
		} 
	}
}
