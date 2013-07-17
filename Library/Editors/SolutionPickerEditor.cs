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
using System.Drawing.Design;
using System.ComponentModel;
using Microsoft.Practices.Common;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using System.Windows.Forms.Design;
using Microsoft.Practices.Common.Services;

namespace Microsoft.Practices.RecipeFramework.Library.Editors
{
    /// <summary>
    /// Allows selection of an element in the current solution.
    /// </summary>
    /// <remarks>
    /// The editor can be configured through the <see cref="IAttributesConfigurable"/> interface, 
    /// used when attributes are added to the Editor element in the configuration file. 
    /// <para>
    /// If an attributes named <c>UnboundReferenceType</c> is not specified, the editor will use 
    /// the ITypeDescriptorContext.PropertyDescriptor.PropertyType received in the 
    /// <see cref="EditValue"/> method to determine a valid selection in the picker.
    /// </para>
    /// </remarks>
    public class SolutionPickerEditor : UITypeEditor, IAttributesConfigurable
    {
        object initialValue;
        string referenceType;
        IUnboundAssetReference reference;
        IWindowsFormsEditorService formsService;
        // Flag required because showing the control causes the selection event to be fired
        // for the first time.
        bool first;


		/// <summary>
		/// Returns the edit style of the Solution that is a dropdown
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

		/// <summary>
		/// Returns if the SolutionPicker is a dropdown resizable which is true
		/// </summary>
        public override bool IsDropDownResizable
        {
            get { return true; }
        }

		/// <summary>
		/// Allows to change the value of the Editor
		/// </summary>
		/// <param name="context"></param>
		/// <param name="provider"></param>
		/// <param name="value"></param>
		/// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (referenceType != null && reference == null)
            {
                ITypeResolutionService resolver = (ITypeResolutionService)ServiceHelper.GetService(provider,
                    typeof(ITypeResolutionService), this);
                Type type = resolver.GetType(referenceType, true);
                ReflectionHelper.EnsureAssignableTo(type, typeof(IUnboundAssetReference));
                reference = (IUnboundAssetReference)Activator.CreateInstance(type, String.Empty);
            }

            DTE vs = (DTE)ServiceHelper.GetService(provider, typeof(DTE), this);
            using (SolutionPickerControl control = new SolutionPickerControl(
                vs, reference, value, context.PropertyDescriptor.PropertyType))
            {
                // Set site so the control can find the IWindowsFormsEditorService
                control.Site = new Site(provider, control, control.GetType().FullName);
                //control.SelectionChanged += new SelectionChangedEventHandler(OnSelectionChanged);
                first = true;
                initialValue = value;

                formsService = (IWindowsFormsEditorService)ServiceHelper.GetService(
                    provider, typeof(IWindowsFormsEditorService), this);
                formsService.DropDownControl(control);
                formsService = null;
                if (reference != null)
                {
                    bool enabled;
                    try
                    {
                        enabled = reference.IsEnabledFor(control.SelectedTarget);
                    }
                    catch (Exception ex)
                    {
                        throw new RecipeExecutionException(reference.AssetName,
                            Properties.Resources.Reference_FailEnabledFor, ex);
                    }
                    if (enabled)
                    {
                        return control.SelectedTarget;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return control.SelectedTarget;
                }
            }
        }

        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Try to minimize the chances that the dialog doesn't respond the first time.
            if (e.IsValid && e.Selection == initialValue && first)
            {
                first = false;
                return;
            }
            if (e.IsValid)
            {
                formsService.CloseDropDown();
            }
        }

        #region IAttributesConfigurable Members

        /// <summary>
        /// Receives the configuration data which must contain an attribute 
        /// <c>UnboundReferenceType</c> with the type to use with 
        /// the <see cref="IUnboundAssetReference"/> to use to determine 
        /// validity of the selection in the dialog.
        /// </summary>
        public void Configure(StringDictionary attributes)
        {
            if (!attributes.ContainsKey("UnboundReferenceType") ||
                attributes["UnboundReferenceType"].Length == 0)
            {
                return;
            }
            referenceType = attributes["UnboundReferenceType"];         
        }

        #endregion
    }
}
