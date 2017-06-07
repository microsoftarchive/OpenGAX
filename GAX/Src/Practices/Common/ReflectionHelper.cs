//===================================================================================
// Microsoft patterns & practices
// Guidance Automation Extensions
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// License: MS-LPL
//===================================================================================

#region Using directives

using System;	
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
using System.Text.RegularExpressions;
using System.Web;
using System.Globalization;
using System.Runtime.InteropServices;

#endregion Using directives

namespace Microsoft.Practices.Common
{
	/// <summary>
	/// Provides utility methods for reflection-related operations.
	/// </summary>
	public sealed class ReflectionHelper
	{
		static readonly Regex AssemblyNameRegEx = new Regex(@"^(?<assembly>[\w\. ]+)(,\s?Version=(?<version>\d+\.\d+\.\d+\.\d+))?(,\s?Culture=(?<culture>\w+))?(,\s?PublicKeyToken=(?<token>\w+))?$", RegexOptions.Compiled);

		#region Private ctor

		// Since this class provides only static methods, make the default constructor private to prevent 
		// instances from being created with "new ReflectionHelper()".

		private ReflectionHelper()	{}

		#endregion Private ctor

		#region Miscelaneous

		/// <summary>
		/// Ensures that an instance of the type passed as the 
		/// <paramref name="source"/> can be assigned to a variable of 
		/// type <paramref name="assignableTo"/>.
		/// </summary>
		/// <param name="source">The type to check.</param>
		/// <param name="assignableTo">The type the <paramref name="source"/> must be assignable to.</param>
        /// <remarks>
        /// This check is basically a shortcut for <see cref="Type.IsAssignableFrom"/>, which works only 
        /// for CLR objects. To perform a more COM-friendly check, use <see cref="IsAssignableTo"/>.
        /// </remarks>
		public static void EnsureAssignableTo(Type source, Type assignableTo)
		{
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (assignableTo == null)
            {
                throw new ArgumentNullException("assignableTo");
            }
			if (!assignableTo.IsAssignableFrom(source))
			{
				throw new ArgumentException(String.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.ReflectionHelper_CantAssignTo,
					source, assignableTo));
			}
		}

        /// <summary>
        /// Tests whether an object can be successfully assigned to the 
        /// specified type.
        /// </summary>
        /// <param name="targetType">The type that the <paramref name="value"/> should be compatible with.</param>
        /// <param name="value">The value to be checked.</param>
        /// <remarks>
        /// The rules for compatibility are:
        /// <list type="bullet">
        /// <item>
        /// <term><paramref name="value"/> is a COM object: 
        /// the <paramref name="targetType"/> must be an interface, and 
        /// a query interface on the COM object must return a valid pointer to that interface.
        /// </term>
        /// </item>
        /// <item>
        /// <term><paramref name="value"/> is a CLR object: 
        /// <paramref name="value"/> type (through <see cref="Object.GetType"/> must be equal to 
        /// <paramref name="targetType"/>, or satisfy <see cref="Type.IsAssignableFrom"/> on it.
        /// </term>
        /// </item>
        /// </list>
        /// </remarks>
        public static bool IsAssignableTo(Type targetType, object value)
        {
            bool isinvalid = false;

            if (value == null || targetType == null)
                return false;

            // We can only QI if the service type is an interface.
            if (value.GetType().IsCOMObject)
            {
                if (targetType.IsInterface)
                {
                    // Try a QI.
                    IntPtr iunk = Marshal.GetIUnknownForObject(value);
                    Guid typeguid = targetType.GUID;
                    IntPtr iface = IntPtr.Zero;
                    Marshal.QueryInterface(iunk, ref typeguid, out iface);
                    isinvalid = (iface == IntPtr.Zero);
					if (!isinvalid)
					{
						Marshal.Release(iface);
					}
					if (iunk != IntPtr.Zero)
					{
						Marshal.Release(iunk);
					}
                }
                else
                {
                    // Can't QI a class type. Assume it's not assignable.
                    isinvalid = true;
                }
            }
            else if (value.GetType() != targetType &&
                !targetType.IsAssignableFrom(value.GetType()))
            {
                isinvalid = true;
            }
            return !isinvalid;
        }

		#endregion Miscelaneous

		#region Assembly Methods

		/// <summary>
		/// Returns the assembly part of a fully qualified type name (FQN), 
		/// without requiring <see cref="System.Security.Permissions.FileIOPermission"/> on the calling code.
		/// </summary>
		/// <param name="qualifiedTypeName">Fully qualified type name.</param>
		/// <returns>The assembly name, including version, culture and public key token if present in the FQN.</returns>
		public static string GetAssemblyString(string qualifiedTypeName)
		{
			if (qualifiedTypeName == null)
			{
				throw new ArgumentNullException("qualifiedTypeName");
			}

			if (qualifiedTypeName.IndexOf(',') == -1)
			{
				throw new ArgumentException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.ReflectionHelper_InvalidQName, qualifiedTypeName));
			}

			return qualifiedTypeName.Substring(qualifiedTypeName.IndexOf(',') + 1).Trim();
		}

		/// <summary>
		/// Gets the simple name of the assembly to which the type belongs, 
		/// without requiring <see cref="System.Security.Permissions.FileIOPermission"/> on the calling code.
		/// </summary>
		/// <param name="type">The type which assembly name is to be discovered.</param>
		/// <returns>The same value as "Assembly.GetName().Name".</returns>
		public static string GetAssemblyName(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return GetAssemblyName(type.Assembly);
		}

		/// <summary>
		/// Gets the simple name of the assembly without 
		/// requiring <see cref="System.Security.Permissions.FileIOPermission"/> on the calling code.
		/// </summary>
		/// <param name="assembly">The assembly to evaluate.</param>
		/// <returns>The same value as "Assembly.GetName().Name".</returns>
		public static string GetAssemblyName(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			return assembly.FullName.Substring(0, assembly.FullName.IndexOf(",")).Trim();
		}

		/// <summary>
		/// Returns the simple assembly name part of a fully qualified type name.
		/// </summary>
		/// <param name="qualifiedTypeName">Fully qualified type name.</param>
		/// <returns>The simple assembly name, without the version, culture and public key token if present.</returns>
		public static string GetAssemblyName(string qualifiedTypeName)
		{
			if (qualifiedTypeName == null)
			{
				throw new ArgumentNullException("qualifiedTypeName");
			}

			string[] names = qualifiedTypeName.Split(',');
			if (names.Length < 2)
			{
				throw new ArgumentException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.ReflectionHelper_InvalidQName, qualifiedTypeName));
			}

			return names[1].Trim();
		}

		/// <summary>
		/// Creates an <see cref="AssemblyName"/> from an assembly display name.
		/// </summary>
		/// <param name="assemblyName">Name of the assembly to parse.</param>
		/// <returns>The parsed name.</returns>
		public static AssemblyName ParseAssemblyName(string assemblyName)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (!AssemblyNameRegEx.IsMatch(assemblyName))
			{
				throw new ArgumentException(String.Format(
					System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.ReflectionHelper_InvalidAssemblyName,
					assemblyName));
			}

			Match match = AssemblyNameRegEx.Match(assemblyName);

			AssemblyName name = new AssemblyName();
			name.Name = match.Groups["assembly"].Value;

			if (match.Groups["version"].Success)
			{
				name.Version = new Version(match.Groups["version"].Value);
			}
			if (match.Groups["culture"].Success)
			{
				// Take into account special case with neutral culture.
				if (match.Groups["culture"].Value != "neutral")
				{
					name.CultureInfo = new System.Globalization.CultureInfo(match.Groups["culture"].Value);
				}
				else
				{
					name.CultureInfo = System.Globalization.CultureInfo.InvariantCulture;
				}
			}
			if (match.Groups["token"].Success)
			{
				if (match.Groups["token"].Value != "null")
				{
					name.SetPublicKeyToken(HexToBytes(match.Groups["token"].Value));
				}
				else
				{
					name.SetPublicKeyToken(null);
				}
			}

			return name;
		}

		static byte[] HexToBytes(string hex)
		{
			// Every two string chars (1 hexa char) make up one byte.
			byte[] result = new byte[hex.Length / 2];
			int charidx = 0;
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = Convert.ToByte(hex.Substring(charidx, 2), 16);
				charidx = charidx + 2;
			}
			return result;
		}

        //static string BytesToHex(byte[] bytes)
        //{
        //    System.Text.StringBuilder sb = new System.Text.StringBuilder(bytes.Length * 2);
        //    foreach (byte b in bytes)
        //    {
        //        sb.Append(b.ToString("x2", System.Globalization.CultureInfo.CurrentCulture));
        //    }
        //    return sb.ToString();
        //}

		/// <summary>
		/// Gets the simplified name of the Type, which includes its full namespace qualified 
		/// name and the targetAssembly name, without the other values.
		/// </summary>
		/// <param name="type">The type whose name is to be discovered.</param>
		public static string GetSimpleTypeName(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			return String.Concat(type.FullName, ", ", GetAssemblyName(type));
		}

		/// <summary>
		/// Gets the simplified name of the object's Type, which includes its full namespace qualified 
		/// name and the targetAssembly name, without the other values.
		/// </summary>
		/// <param name="target">The type whose name is to be discovered.</param>
		public static string GetSimpleTypeName(object target)
		{
			if (target == null) throw new ArgumentNullException("target");

			return GetSimpleTypeName(target.GetType());
		}

		/// <summary>
		/// Gets the Assembly Title.
		/// </summary>
		/// <param name="targetAssembly">Assembly to get title.</param>
		/// <returns></returns>
		public static string GetAssemblyTitle(Assembly targetAssembly) 
		{
			if (targetAssembly == null) throw new ArgumentNullException("targetAssembly");

			object[] att = targetAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
			return ((att.Length > 0) ? ((AssemblyTitleAttribute) att [0]).Title : String.Empty);
		}

		/// <summary>
		/// Instructs a compiler to use a specific version number for the Win32 file version resource. 
		/// The Win32 file version is not required to be the same as the targetAssembly's version number.
		/// </summary>
		/// <param name="targetAssembly">Assembly to get version.</param>
		/// <returns></returns>
		public static string GetAssemblyFileVersion(Assembly targetAssembly) 
		{
			if (targetAssembly == null) throw new ArgumentNullException("targetAssembly");
			
			object[] att = targetAssembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
			return ((att.Length > 0) ? ((AssemblyFileVersionAttribute) att [0]).Version : String.Empty);
		}

		/// <summary>
		/// Gets the Assembly Product.
		/// </summary>
		/// <param name="targetAssembly">Assembly to get product.</param>
		/// <returns></returns>
		public static string GetAssemblyProduct(Assembly targetAssembly) 
		{
			if (targetAssembly == null) throw new ArgumentNullException("targetAssembly");
			
			object[] att = targetAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
			return ((att.Length > 0) ? ((AssemblyProductAttribute) att [0]).Product : String.Empty);
		}

		/// <summary>
		/// Gets the Assembly Configuration Attribute.
		/// </summary>
		/// <param name="targetAssembly">Assembly to get configuration.</param>
		/// <returns></returns>
		public static string GetAssemblyConfiguration(Assembly targetAssembly) 
		{
			if (targetAssembly == null) throw new ArgumentNullException("targetAssembly");
			
			object[] att = targetAssembly.GetCustomAttributes(typeof(AssemblyConfigurationAttribute), false);
			return ((att.Length > 0) ? ((AssemblyConfigurationAttribute) att [0]).Configuration : String.Empty);
		}

		/// <summary>
		/// Gets an targetAssembly full path and file name.
		/// </summary>
		/// <param name="targetAssembly">Assembly to get path.</param>
		/// <returns></returns>
		public static string GetAssemblyPath(Assembly targetAssembly)
		{
			if (targetAssembly == null) throw new ArgumentNullException("targetAssembly");
            if (targetAssembly.CodeBase == null || targetAssembly.CodeBase.Length == 0)
            {
                throw new ArgumentException(Properties.Resources.General_ArgumentEmpty, "targetAssembly.CodeBase");
            }
			
			return new CompatibleUri(targetAssembly.CodeBase).LocalPath;
		}

		/// <summary>
		/// Gets an targetAssembly full path and file name.
		/// </summary>
		/// <param name="targetAssembly">Assembly to get path.</param>
		/// <returns></returns>
		public static string GetAssemblyFolder(Assembly targetAssembly)
		{
			if (targetAssembly == null) throw new ArgumentNullException("targetAssembly");
			
			string path = GetAssemblyPath(targetAssembly);
            if (path.IndexOf('\\') != -1)
            {
                path = path.Substring(0, path.LastIndexOf('\\'));
            }
			return path;
		}

		/// <summary>
		/// Creates the name of a type qualified by the display name of its targetAssembly.
		/// The format of the returned string is: FullTypeName, AssemblyName.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string CreateShortQualifiedName(Type value)
		{
			if (value == null) throw new ArgumentNullException("value");

			return Assembly.CreateQualifiedName(GetAssemblyName(value), value.FullName);
		}

		#endregion Assembly Methods

		#region Conventional type loading
		
		/// <summary>
		/// 	Returns a <see cref="Type"/> from the name received, loaded from the GAC, the base application folder or the calling targetAssembly. 
		/// </summary>
		/// <param name="className">Fully qualified name of the class to load.</param>
		/// <returns>The loaded type.</returns>
		/// <exception cref="TypeLoadException">The type couldn't be loaded.</exception>
		/// <remarks>
		/// If the class name is not targetAssembly-qualified, the calling targetAssembly is used to resolve the name 
		/// first.
		/// </remarks>
		public static Type LoadType(string className)
		{
			if (className == null) throw new ArgumentNullException("className");

			return LoadType(className, false);
		}

		/// <summary>
		/// 	Returns a <see cref="Type"/> from the name received, loaded from the GAC or the base application folder. 
		/// </summary>
		/// <param name="className">Fully qualified name of the class to load.</param>
		/// <param name="callingAssembly">True if the type is in the "CallingAssembly".</param>
		/// <returns>The loaded type.</returns>
		/// <exception cref="TypeLoadException">The type couldn't be loaded.</exception>
		public static Type LoadType(string className, bool callingAssembly)
		{
			if (className == null) throw new ArgumentNullException("className");
			
			Type reqtype = null;

			if(className != null && className.Length > 0)
			{
				// Method 1 (full class name in the GAC)
				if(callingAssembly)
					reqtype = Assembly.GetCallingAssembly().GetType(className.Trim(), false, true);
				else 
					reqtype = Type.GetType(className.Trim(), false, true);

				if (reqtype == null)
				{
					// Method 2 (partial class name in the GAC)
					string[] names = className.Split(',');
					Assembly asm = Assembly.LoadWithPartialName(GetAssemblyName(className));

					if (asm == null)
					{
						throw new TypeLoadException(String.Format(
                            System.Globalization.CultureInfo.CurrentCulture,
							Properties.Resources.ReflectionHelper_CantLoadAssembly, 
							names[1], className));
					}

					reqtype = asm.GetType(names[0].Trim(), true, true);
				}
			}
			return reqtype;			
		}

		#endregion Conventional type loading
	}
}