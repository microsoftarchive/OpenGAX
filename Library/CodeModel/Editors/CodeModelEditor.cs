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
using System.Collections;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel.Editors
{
	/// <summary>
	/// Allows browsing and selecting a class defined in the currently 
	/// selected projects.
	/// </summary>
	/// <remarks>
	/// This editor relies on the <see cref="DTE"/> service being available.
	/// </remarks>
	public class CodeModelEditor : UITypeEditor, IAttributesConfigurable
	{
		#region Fields

		/// <summary>
		/// Optional attribute that can be specified at the Editor XML element with the 
		/// name "BrowseRoot", used to specify the kind of browsing to use. 
		/// See <see cref="CodeModelEditor.BrowseRoot"/> for default and valid values.
		/// </summary>
		public const string BrowseRootAttribute = "BrowseRoot";
		BrowseRoot root = BrowseRoot.Default;

		/// <summary>
		/// Optional attribute that can be specified at the Editor XML element with the 
		/// name "BrowseKind", used to specify the kind of level of browsing to use. 
		/// See <see cref="CodeModelEditor.BrowseKind"/> for default and valid values.
		/// </summary>
		public const string BrowseKindAttribute = "BrowseKind";
		BrowseKind kind = BrowseKind.Default;


		/// <summary>
		/// Optional attribute that can be specified at the Editor XML element with the 
		/// name "Filter", used to specify the type of a custom filter object.
		/// The filter object must implement the interface <see cref="ICodeModelEditorFilter"/> for default and valid values.
		/// </summary>
		public const string FilterAttribute = "Filter";
		string filterTypeName = string.Empty;


		/// <summary>
		/// Optional attribute that can be specified at the Editor XML element with the 
		/// name "RootEntryName", used to specify the root used to browse. 
		/// The name must be the name of an entry in the current <see cref="IDictionaryService"/>
		/// </summary>
		public const string RootAttribute = "RootEntryName";
		string customRootEntryName = string.Empty;

		/// <summary>
		/// Optional attribute, if true the editor will only display code elements from the user source code
		/// </summary>
		public const string UserCodeAttribute = "UserCode";
		private bool onlyUserCode = false;

		private StringDictionary attributes;

		#endregion

		#region IDictionaryService wrapper

		private class DictionaryWrapper : SitedComponent, IDictionaryService
		{
			StringDictionary attributes;
			IDictionaryService dictionaryService;

			public DictionaryWrapper(StringDictionary attributes)
			{
				this.attributes = attributes;
			}

			#region Overides

			protected override void OnSited()
			{
				base.OnSited();
				dictionaryService = (IDictionaryService)GetService(typeof(IDictionaryService));
			}

			#endregion

			#region IDictionaryService Members

			object IDictionaryService.GetKey(object value)
			{
				return dictionaryService.GetKey(value);
			}

			object IDictionaryService.GetValue(object key)
			{
				if (attributes.ContainsKey(key.ToString()))
				{
					return attributes[key.ToString()];
				}
				return dictionaryService.GetValue(key);
			}

			void IDictionaryService.SetValue(object key, object value)
			{
				dictionaryService.SetValue(key, value);
			}

			#endregion
		}

		#endregion IDictionaryService wrapper

		#region Public Static methods

		internal static Microsoft.Practices.ComponentModel.ServiceContainer CreateFilter(IServiceProvider provider, string filterTypeName, StringDictionary attributes, out ICodeModelEditorFilter filter)
		{
			Microsoft.Practices.ComponentModel.ServiceContainer editorServiceProvider = new Microsoft.Practices.ComponentModel.ServiceContainer(true);
			editorServiceProvider.Site = new Site(provider, editorServiceProvider, editorServiceProvider.GetType().FullName);
			editorServiceProvider.AddService(typeof(IDictionaryService), new DictionaryWrapper(attributes));
			if (string.IsNullOrEmpty(filterTypeName))
			{
				filter = null;
			}
			else
			{
				ITypeResolutionService typeResService =
					(ITypeResolutionService)provider.GetService(typeof(ITypeResolutionService));
				Type filterType = typeResService.GetType(filterTypeName);
				if (filterType == null)
				{
					filter = null;
				}
				else
				{
					filter = (ICodeModelEditorFilter)Activator.CreateInstance(filterType);
					if (filter is IComponent)
					{
						editorServiceProvider.Add((IComponent)filter);
					}
				}
			}
			return editorServiceProvider;
		}

		#endregion

		#region Method overrides

		/// <summary>
		/// See <see cref="UITypeEditor.EditValue(System.ComponentModel.ITypeDescriptorContext,IServiceProvider,object)"/>.
		/// </summary>
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			// We need the editor service.
			if (provider.GetService(typeof(IWindowsFormsEditorService)) == null ||
				provider.GetService(typeof(DTE)) == null)
			{
				return base.EditValue(context, provider, value);
			}

			ICodeModelEditorFilter filter = null;
			using (CreateFilter(provider, this.filterTypeName, this.attributes, out filter))
			{
				CodeElement customRoot = null;
				if (!string.IsNullOrEmpty(customRootEntryName))
				{
					IDictionaryService dictService =
						(IDictionaryService)provider.GetService(typeof(IDictionaryService));
					try
					{
						customRoot = (CodeElement)dictService.GetValue(customRootEntryName);
					}
					catch
					{
						// Error getting the root object
						return null;
					}
				}
				using (CodeModelEditorForm form = new CodeModelEditorForm(context, root, kind, filter, customRoot, onlyUserCode))
				{
					IWindowsFormsEditorService svc = (IWindowsFormsEditorService)
						provider.GetService(typeof(IWindowsFormsEditorService));
					form.Site = new Site(provider, form, form.GetType().FullName);
					if (svc.ShowDialog(form) == DialogResult.OK)
					{
						return form.SelectedObject;
					}
					else
					{
						return null;
					}
				}
			}
		}

		/// <summary>
		/// See <see cref="UITypeEditor.GetEditStyle(System.ComponentModel.ITypeDescriptorContext)"/>.
		/// </summary>
		public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		#endregion Method overrides

		#region IAttributesConfigurable Members

		void IAttributesConfigurable.Configure(System.Collections.Specialized.StringDictionary attributes)
		{
			if (attributes.ContainsKey(BrowseRootAttribute))
			{
				root = (BrowseRoot)Enum.Parse(typeof(BrowseRoot), attributes[BrowseRootAttribute]);
			}
			if (attributes.ContainsKey(BrowseKindAttribute))
			{
				kind = (BrowseKind)Enum.Parse(typeof(BrowseKind), attributes[BrowseKindAttribute]);
			}
			if (attributes.ContainsKey(FilterAttribute))
			{
				filterTypeName = attributes[FilterAttribute];
			}
			if (attributes.ContainsKey(RootAttribute))
			{
				customRootEntryName = attributes[RootAttribute];
			}
			if (attributes.ContainsKey(UserCodeAttribute))
			{
				onlyUserCode = Boolean.Parse(attributes[UserCodeAttribute]);
			}
			this.attributes = attributes;
		}

		#endregion

		#region Public Enums

		/// <summary>
		/// Kinds of browsing supported by the editor.
		/// </summary>
		public enum BrowseRoot
		{
			/// <summary>
			/// Only classes from the currently selected projects will be shown.
			/// </summary>
			SelectedProjects,
			/// <summary>
			/// Classes from the entire solution will be shown.
			/// </summary>
			Solution,
			/// <summary>
			/// Default value equals <see cref="Solution"/>.
			/// </summary>
			Default = Solution,
			/// <summary>
			/// Custom codeElement value as Root
			/// </summary>
			CustomRoot,
		}

		/// <summary>
		/// Kinds of browsing supported by the editor
		/// </summary>
		public enum BrowseKind
		{
			/// <summary>
			/// No browsing
			/// </summary>
			None = 0,
			/// <summary>
			/// Nasmespace browsing
			/// </summary>
			Namespace = 0x01000000,
			/// <summary>
			/// Only browse for classes
			/// </summary>
			Class = 0x00000001,
			/// <summary>
			/// Only browse for enums
			/// </summary>
			Enum = 0x00000002,
			/// <summary>
			/// Only browse for delegates
			/// </summary>
			Delegate = 0x00000004,
			/// <summary>
			/// Only browse for interfaces
			/// </summary>
			Interface = 0x00000008,
			/// <summary>
			/// Only browse for Structs
			/// </summary>
			Struct = 0x00000010,
			/// <summary>
			/// Only browse for functions
			/// </summary>
			Function = 0x00000100,
			/// <summary>
			/// Only browse for variables
			/// </summary>
			Variable = 0x00000200,
			/// <summary>
			/// Only browse for properties
			/// </summary>
			Prop = 0x00000400,
			/// <summary>
			/// Only browse for events
			/// </summary>
			Event = 0x00000800,
			/// <summary>
			/// Only browse for methods inside a class
			/// </summary>
			ClassMethod = Function | Class,
			/// <summary>
			/// Only browse for methods inside a class
			/// </summary>
			ClassMember = Variable | Class,
			/// <summary>
			/// Only browse for properties inside a class
			/// </summary>
			ClassProperty = Prop | Class,
			/// <summary>
			/// Only browse for events inside a class
			/// </summary>
			ClassEvent = Event | Class,
			/// <summary>
			/// Browse for all elements inside a class
			/// </summary>
			ClassAll = Prop | Variable | Function | Event | Class,
			/// <summary>
			/// Only browse for methods inside an interface
			/// </summary>
			InterfaceMethod = Function | Interface,
			/// <summary>
			/// Only browse for properties inside an interface
			/// </summary>
			InterfaceProperty = Prop | Interface,
			/// <summary>
			/// Browse for all elements inside an interface
			/// </summary>
			InterfaceAll = Prop | Function | Interface,
			/// <summary>
			/// Only browse for methods inside a Struct
			/// </summary>
			StructMethod = Function | Struct,
			/// <summary>
			/// Only browse for members inside a Struct
			/// </summary>
			StructMember = Variable | Struct,
			/// <summary>
			/// Only browse for properties inside a Struct
			/// </summary>
			StructProperty = Prop | Struct,
			/// <summary>
			/// Browse for all elements inside a Struct
			/// </summary>
			StructAll = Prop | Function | Struct,
			/// <summary>
			/// Browse for members inside an Enum
			/// </summary>
			EnumMember = Variable | Enum,
			/// <summary>
			/// Default browse level is <see cref="BrowseKind.Class"/>
			/// </summary>
			Default = Class,
		}

		#endregion
	}
}
