using System;

namespace Mvp.Xml.Design.CustomTools
{
	/// <summary>
	/// Determines which versions of VS.NET are supported by the custom tool.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class VersionSupportAttribute : Attribute
	{
		Version _version;

		/// <summary>
		/// Initializes the attribute.
		/// </summary>
		/// <param name="version">Version supported by the tool.</param>
		public VersionSupportAttribute(string version)
		{
			_version = new Version(version);
		}

		/// <summary>
		/// Version supported by the tool.
		/// </summary>
		public Version Version
		{
			get { return _version; }
		} 
	}
}
