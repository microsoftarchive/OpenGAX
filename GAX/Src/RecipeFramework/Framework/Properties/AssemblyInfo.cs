#region Using directives

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Microsoft.Practices.RecipeFramework")]
[assembly: AssemblyDescription("Microsoft.Practices.RecipeFramework")]
[assembly: InternalsVisibleTo("Microsoft.Practices.RecipeFramework.Test, PublicKey = " +
"002400000480000094000000060200000024000052534131000400000100010063956249a476a4" +
"c68606114d55c6ec09263c7b74474cb79cabfe85024fc38ca88dbcd6e9bf2755998aa7913f75f2" +
"16399cb8389c95bb9ad38c406d2bee5b9f21441be00b4bc015907ee9503175c4cc6b25f1c809ca" +
"58d5330dbcd4365bfe218199896d75241846fc8ecd623361158c5e7cb2d8cf6692246994b60055" +
"23dd3dad")]
[assembly: InternalsVisibleTo("Microsoft.Practices.WizardFramework.Test, PublicKey=" +
"002400000480000094000000060200000024000052534131000400000100010063956249a476a4" +
"c68606114d55c6ec09263c7b74474cb79cabfe85024fc38ca88dbcd6e9bf2755998aa7913f75f2" +
"16399cb8389c95bb9ad38c406d2bee5b9f21441be00b4bc015907ee9503175c4cc6b25f1c809ca" +
"58d5330dbcd4365bfe218199896d75241846fc8ecd623361158c5e7cb2d8cf6692246994b60055" +
"23dd3dad")]
[assembly: InternalsVisibleTo("Microsoft.Practices.RecipeFramework.Configuration.Test, PublicKey=" +
"002400000480000094000000060200000024000052534131000400000100010063956249a476a4" +
"c68606114d55c6ec09263c7b74474cb79cabfe85024fc38ca88dbcd6e9bf2755998aa7913f75f2" +
"16399cb8389c95bb9ad38c406d2bee5b9f21441be00b4bc015907ee9503175c4cc6b25f1c809ca" +
"58d5330dbcd4365bfe218199896d75241846fc8ecd623361158c5e7cb2d8cf6692246994b60055" +
"23dd3dad")]
[assembly: CLSCompliant(true)]

/// <summary>
/// Provides easy access to assembly-level attributes.
/// </summary>
internal sealed class ThisAssembly
{
    public const string Title = "Recipe Framework";
    public const string Product = "Microsoft® Recipe Framework";
    public const string Description = "Microsoft® Patterns & Practices Recipe Framework";
}