using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly: ComVisible(true)]
[assembly: CLSCompliant(false)]

[assembly: AssemblyTitle("Mvp.Xml.Design")]
[assembly: AssemblyDescription("MVP XML Design Tools")]
[assembly: AssemblyVersion(ThisAssembly.Version)]

[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif

#region Security Permissions

//[assembly: SecurityPermission(SecurityAction.RequestRefuse, UnmanagedCode=true)]

#endregion Security Permissions

internal class ThisAssembly
{
	public const string Title = "Mvp.Xml.Design";
	public const string Description = "MVP XML Design Tools";
	public const string Version = "1.1.1.0";
}