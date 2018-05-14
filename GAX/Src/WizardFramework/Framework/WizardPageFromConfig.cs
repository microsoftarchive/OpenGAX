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
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using System.Drawing.Design;
using Microsoft.Practices.Common;
using System.Xml;
using System.Windows.Forms.Design;
//using Services = Microsoft.Practices.RecipeFramework.Services;
//using Configuration = Microsoft.Practices.RecipeFramework.Configuration;

#endregion Using directives

namespace Microsoft.Practices.WizardFramework
{
	[ServiceDependency(typeof(IValueInfoService))]
	[ServiceDependency(typeof(ITypeResolutionService))]
	[ServiceDependency(typeof(IServiceProvider))]
	internal class WizardPageFromConfig : WizardPage
    {
        #region Designer Solution

        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// infoPanel
			// 
			this.infoPanel.Location = new System.Drawing.Point(0, 10);
			this.infoPanel.Size = new System.Drawing.Size(627, 459);
			// 
			// WizardPageFromConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.Name = "WizardPageFromConfig";
			this.Size = new System.Drawing.Size(627, 469);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        #endregion Designer Solution

        #region Constructors

        /// <summary>
		/// Creates a WizardPage inside the Windows Form Designer
		/// </summary>
		public WizardPageFromConfig()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Creates a WizardPage inside a WizardFrom
		/// </summary>
		/// <param name="wizard"></param>
		public WizardPageFromConfig(WizardForm wizard)
			: base(wizard)
		{
			InitializeComponent();
		}

        #endregion

        #region UI Building

        private ArgumentPanel CreateEditingPanel(Configuration.Field field)
		{
            IValueInfoService metaDataService =
                (IValueInfoService)GetService(typeof(IValueInfoService));
			ValueInfo argument = metaDataService.GetInfo(field.ValueName);
			if (argument == null)
			{
				throw new ArgumentNullException("Field");
			}
			ArgumentPanel argumentPanel = null;
			ITypeResolutionService loaderService =
				(ITypeResolutionService)GetService(typeof(ITypeResolutionService));
			Type argType = argument.Type;
			if (argType == null)
			{
                throw new TypeLoadException(
					String.Format(
						CultureInfo.CurrentCulture,Properties.Resources.WizardGatheringService_CannotLoadTypeStepField,
						argument.Name,
						this.Configuration.Title));
			}
			if ( argType == typeof(bool) ||  // The type is a simple boolean value, so let's use ArgumentPanelBool
				 argType== typeof(Nullable<bool>) )
			{
				argumentPanel = new ArgumentPanelBool();
			}
			else
			{
				if (!string.IsNullOrEmpty(field.PanelType))
				{
					Type paneltype = loaderService.GetType(field.PanelType);
					argumentPanel = (ArgumentPanelTypeEditor)Activator.CreateInstance(paneltype);
				}
				else
				{
					argumentPanel = new ArgumentPanelTypeEditor();
				}
                //First try to use any UITypeEditor defined for our type
				Type editorType = field.Editor == null ? null : loaderService.GetType(field.Editor.Type);
                if (editorType != null)
                {
                    UITypeEditor editorInstance = (UITypeEditor)Activator.CreateInstance(editorType);
                    if (editorInstance is IAttributesConfigurable)
                    {
                        ImmutableKeyStringDictionary values = new ImmutableKeyStringDictionary();
                        if (field.Editor.AnyAttr != null)
                        {
                            foreach (XmlAttribute xattr in field.Editor.AnyAttr)
                            {
                                values.Add(xattr.Name, xattr.Value);
                            }
                        }
                        ((IAttributesConfigurable)editorInstance).Configure(values);
                    }
                    ((ArgumentPanelTypeEditor)argumentPanel).EditorInstance = editorInstance;
                }
				((ArgumentPanelTypeEditor)argumentPanel).ConverterInstance = argument.Converter;
			}
			argumentPanel.FieldConfig = field;
			this.Arguments[argument] = argumentPanel;
			return argumentPanel;
		}

		#endregion

		#region Overrides

        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            base.OnHelpRequested(hevent);
            Point helpAt=this.PointToClient(hevent.MousePos);
            Control acControl = this.GetChildAtPoint(helpAt);
            if (acControl!=null && acControl is ArgumentPanel)
            {
                ArgumentPanel panel = acControl as ArgumentPanel;
                panel.Focus();
                if (panel.FieldConfig != null && panel.FieldConfig.Tooltip != null && !panel.FieldConfig.Tooltip.Equals(String.Empty))
                {
                    ToolTip toolTip = panel.ToolTip;
                    toolTip.Show(panel.FieldConfig.Tooltip, this, helpAt,toolTip.AutoPopDelay);
                }
            }
        }

        private void SetFirstArgumentPanelFocus()
        {
            this.Wizard.Focus();
            this.Focus();
            if (this.Configuration != null && this.Configuration.Fields.Length >= 1)
            {
                IValueInfoService metaDataService =
                    (IValueInfoService)GetService(typeof(IValueInfoService));
                ValueInfo argument = metaDataService.GetInfo(this.Configuration.Fields[0].ValueName);
                if (argument != null)
                {
                    ArgumentPanel firstArgumentPanel =
                        (ArgumentPanel)Arguments[argument];
                    firstArgumentPanel.Focus();
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            SetFirstArgumentPanelFocus();
        }

        public override void OnActivated()
        {
            base.OnActivated();
            SetFirstArgumentPanelFocus();
        }

		public override bool OnActivate()
		{
			if (base.OnActivate())
			{
				foreach (ArgumentPanel argumentPanel in Arguments.Values)
				{
					argumentPanel.SetValue();
				}
                IComponentChangeService componentChange =
					(IComponentChangeService)GetService(typeof(IComponentChangeService));
				if (componentChange != null)
				{
					componentChange.ComponentChanged += new ComponentChangedEventHandler(RecipeArgumentChanged);
				}
				return true;
			}
			return false;
		}

		public override void OnDeactivated()
		{
			IComponentChangeService componentChange =
				(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if (componentChange != null)
			{
				componentChange.ComponentChanged -= new ComponentChangedEventHandler(RecipeArgumentChanged);
			}
			base.OnDeactivated();
		}

		protected override void BuildRecipeArguments()
		{
            IValueInfoService metaDataService =
                (IValueInfoService)GetService(typeof(IValueInfoService));
			foreach (Configuration.Field field in this.Configuration.Fields)
			{
                ValueInfo argument = metaDataService.GetInfo(field.ValueName);
				if (argument == null)
				{
					throw new WizardFrameworkException(
						String.Format(
							CultureInfo.CurrentCulture,
							Properties.Resources.WizardGatheringService_MissingArgumentMetaData,
                            field.ValueName));
				}
				Arguments.Add(argument, null);
			}
		}

		#endregion

		#region Event Handlers

		private void RecipeArgumentChanged(object sender, ComponentChangedEventArgs e)
		{
			try
			{
				if (e.Component == null)
				{
					throw new ArgumentNullException("Component");
				}
				// Forward call to appropriate providers.
				ArgumentPanel argumentPanel = (ArgumentPanel)Arguments[((ValueInfo)e.Component)];
				if (argumentPanel != null)
				{
					argumentPanel.UpdateValueInternal(e.NewValue);
				}
			}
			catch (Exception ex)
			{
				ErrorHelper.Show(this.Site, ex);
			}
		}

		#endregion

		#region ISupportInitialize Members

		public override void BeginInit()
		{
			base.BeginInit();
		}

		public override void EndInit()
		{
			if (IsInitializing)
			{
				base.EndInit();
				if (!IsInitializing)
				{
					bool usingHelp = !string.IsNullOrEmpty(this.Configuration.Help);
					int heightNeeded = 0;
					IsInitializing = true;
					IServiceProvider serviceProvider = (IServiceProvider)GetService(typeof(IServiceProvider));
					this.SuspendLayout();
					ArgumentPanel[] argumentPanels = new ArgumentPanel[this.Configuration.Fields.Length];
					for (int i = 0; i < this.Configuration.Fields.Length; i++)
					{
						argumentPanels[i] = CreateEditingPanel(this.Configuration.Fields[i]);
						((System.ComponentModel.ISupportInitialize)(argumentPanels[i])).BeginInit();
						argumentPanels[i].SuspendLayout();
						this.Controls.Add(argumentPanels[i]);
						this.Controls.SetChildIndex(argumentPanels[i], 0);
						((System.ComponentModel.ISupportInitialize)(argumentPanels[i])).EndInit();
						argumentPanels[i].ResumeLayout();
						heightNeeded += argumentPanels[i].Height;
						usingHelp |= !string.IsNullOrEmpty(this.Configuration.Fields[i].Help);
					}
					for (int i = this.Configuration.Fields.Length; --i >= 0; )
					{
						argumentPanels[i].TabIndex = i;
					}
					heightNeeded += InfoRTBoxSize.Height;
					if (usingHelp && this.Height < heightNeeded)
					{
						this.MinimumSize = new Size(this.Width, heightNeeded);
						this.Height = heightNeeded;
					}
					this.ResumeLayout();
					IsInitializing = false;
				}
			}
		}

		#endregion
	}
}

