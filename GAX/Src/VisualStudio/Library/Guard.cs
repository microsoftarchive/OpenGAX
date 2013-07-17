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

using System;
using System.Text;
using System.Globalization;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library
{
	/// <summary>
	/// Common guard clauses
	/// </summary>
	public static class Guard
	{
		/// <summary>
		/// Checks a string argument to ensure it isn't null or empty
		/// </summary>
		/// <param name="argumentValue">The argument value to check.</param>
		/// <param name="argumentName">The name of the argument.</param>
		public static void ArgumentNotNullOrEmptyString(string argumentValue, string argumentName)
		{
			ArgumentNotNull(argumentValue, argumentName);

			if (argumentValue.Length == 0)
				throw new ArgumentException(String.Format(
					CultureInfo.CurrentCulture, 
					Properties.Resources.StringCannotBeEmpty, argumentName));
		}

		/// <summary>
		/// Checks an argument to ensure it isn't null
		/// </summary>
		/// <param name="argumentValue">The argument value to check.</param>
		/// <param name="argumentName">The name of the argument.</param>
		public static void ArgumentNotNull(object argumentValue, string argumentName)
		{
			if (argumentValue == null)
				throw new ArgumentNullException(argumentName);
		}
	}
}
