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
using System.ComponentModel;
using EnvDTE;
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework.Library.CodeModel.Editors;
using System.Globalization;

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel.Converters
{
	/// <summary>
	/// Converter that returns the FullName of a <see cref="CodeElement"/> object
	/// </summary>
	public class CodeModelConverter : TypeConverter, IAttributesConfigurable
	{
		private string filterTypeName = string.Empty;
		private System.Collections.Specialized.StringDictionary attributes;

		/// <summary>
		/// Returns a boolean specifing if the type can be converted from <paramref name="sourceType"/>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="sourceType"></param>
		/// <returns></returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			else if (sourceType == typeof(CodeElement))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// Returns a boolean value specifing if the type can be converted to <paramref name="destinationType"/>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="destinationType"></param>
		/// <returns>Returns true only if <paramref name="destinationType"/> is <see cref="string"/></returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		/// <summary>
		/// Converts <paramref name="value"/> to type <paramref name="destinationType"/>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value">A <see cref="CodeElement"/> object</param>
		/// <param name="destinationType"></param>
		/// <returns>The FullName property of a <see cref="CodeElement"/></returns>
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (value is CodeElement && destinationType == typeof(string))
			{
				return ((CodeElement)value).FullName;
			}
			else if (value is string)
			{
				DTE dte = (DTE)context.GetService(typeof(DTE));
				return CodeModelUtil.ConvertFromString(dte, (string)value);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>
		/// Converts to <see cref="CodeElement"/> type from <paramref name="value"/>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				DTE dte = (DTE)context.GetService(typeof(DTE));
				return CodeModelUtil.ConvertFromString(dte, (string)value);
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>
		/// Validates <paramref name="value"/>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			CodeElement codeElement = null;
			if (value is string)
			{
				codeElement = (CodeElement)ConvertTo(context, CultureInfo.CurrentCulture, value, typeof(CodeElement));
			}
			else if (value is CodeElement)
			{
				codeElement = (CodeElement)value;
			}
			if (codeElement == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(this.filterTypeName))
			{
				return true;
			}
			else
			{
				ICodeModelEditorFilter filter = null;
				using (CodeModelEditor.CreateFilter((IServiceProvider)context,
					this.filterTypeName, this.attributes, out filter))
				{
					if (filter != null)
					{
						try
						{
							return !filter.Filter(codeElement);
						}
						catch
						{
							return false;
						}
					}
					return true;
				}
			}
		}

		#region IAttributesConfigurable Members

		void IAttributesConfigurable.Configure(System.Collections.Specialized.StringDictionary attributes)
		{
			this.filterTypeName = attributes["Filter"];
			this.attributes = attributes;
		}

		#endregion
	}
}
