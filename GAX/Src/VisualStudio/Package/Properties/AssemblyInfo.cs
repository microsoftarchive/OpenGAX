#region Using directives

using System.Reflection;
using System.Runtime.CompilerServices;
using System;

#endregion

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Microsoft.Practices.RecipeFramework.VisualStudio")]
[assembly: AssemblyDescription("Microsoft.Practices.RecipeFramework.VisualStudio")]
[assembly: InternalsVisibleTo("Microsoft.Practices.RecipeFramework.VisualStudio.Test")]
[assembly: InternalsVisibleTo("Microsoft.Practices.RecipeFramework.Library")]
[assembly: CLSCompliant(false)]

/// <summary>
/// Provides easy access to assembly-level attributes.
/// </summary>
internal sealed class ThisAssembly
{
    public const string Title = "RecipeFramework.VisualStudio";
	public const string Product = "Microsoft® Guidance Automation Runtime";
	public const string Description = "Guidance Automation Visual Studio Integration";
}