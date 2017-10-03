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
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.ComponentModel;

#endregion

namespace Microsoft.Practices.WizardFramework
{
    [ServiceDependency(typeof(IWindowsFormsEditorService))]
	internal class DefaultValueEditor : UITypeEditor
	{
		PropertyGrid propertyGrid;
		public DefaultValueEditor()
		{
			propertyGrid = null;
		}
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return false;
		}
		public override bool IsDropDownResizable
		{
			get 
			{
				return true;
			}
		}
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsEditorService = ((IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService)));
				if (windowsEditorService != null)
				{
					if (this.propertyGrid == null)
					{
						this.propertyGrid = new PropertyGrid();
					}
					this.propertyGrid.Site = new ComponentModel.Site(provider, this.propertyGrid, this.propertyGrid.Name);
					propertyGrid.SelectedObject = value;
					windowsEditorService.DropDownControl(this.propertyGrid);
				}
			}
			return value;
		}
	}
}
