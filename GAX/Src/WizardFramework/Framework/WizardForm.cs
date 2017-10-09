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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using System.Reflection;
using Microsoft.Practices.Common;
//using EnvDTE;
//using EnvDTE80;
using Microsoft.Practices.Common.Properties;
using Microsoft.Practices.WizardFramework.Properties;
using System.IO;

#endregion

namespace Microsoft.Practices.WizardFramework
{
	/// <summary>
	/// Implements a Wizard for gathering arguments.
	/// </summary>
	[ServiceDependency(typeof(IValueInfoService))]
	[ServiceDependency(typeof(ITypeResolutionService))]
	[ServiceDependency(typeof(IDictionaryService))]
	public class WizardForm : Microsoft.WizardFramework.WizardForm, ISupportInitialize
    {
        #region Designer Stuff

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
        private System.ComponentModel.IContainer components = null;
        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardForm));
            this.SuspendLayout();
            // 
            // WizardForm
            // 
            resources.ApplyResources(this, "$this");
            this.Name = "WizardForm";
            this.HelpButton = !string.IsNullOrEmpty(this.wizardConfig.Help);
			this.ResumeLayout(false);
        }
        #endregion

        #endregion Designer Stuff

        #region Fields
        /// <summary>
		/// The Wizard configuration for this Wizard
		/// </summary>
		private Configuration.Wizard wizardConfig;
		/// <summary>
		/// Indicated that the Wizard is been created/initialized
		/// </summary>
		private bool initializing;
		/// <summary>
		/// Result of the execution of this Wizard
		/// </summary>
		private ExecutionResult gatheringResult;
        /// <summary>
        /// Indicates the base path of the wizard configuration file
        /// </summary>
        private string basePath;

		#endregion

		#region Constructor
        /// <summary>
        /// Builds a gathering arguments Wizard
        /// </summary>
        /// <param name="provider">IServiceProvider</param>
        /// <param name="wizardConfig">Wizard Configuration</param>
        public WizardForm(IServiceProvider provider, Configuration.Wizard wizardConfig) : 
            this(provider, wizardConfig, string.Empty)
        {
        }

		/// <summary>
		/// Builds a gathering arguments Wizard
		/// </summary>
		/// <param name="provider">IServiceProvider</param>
		/// <param name="wizardConfig">Wizard Configuration</param>
        /// <param name="basePath">Configuration File Base Path</param>
		public WizardForm(IServiceProvider provider, Configuration.Wizard wizardConfig, string basePath)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			this.serviceProvider = provider;
			if (wizardConfig == null)
			{
				throw new ArgumentNullException("wizardConfig");
			}
             
            this.basePath = basePath;
			this.wizardConfig = wizardConfig;
			gatheringResult = ExecutionResult.Cancel;
			((ISupportInitialize)this).BeginInit();
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			((ISupportInitialize)this).EndInit();
		}

		#endregion Constructor

		#region Overrides
        /// <summary>
        /// See <see cref="Control.OnGotFocus"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.ActivePage.Focus();
        }

        /// <summary>
        /// See <see cref="Form.OnActivated"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            this.ActivePage.Focus();
        }

        /// <summary>
        /// See <see cref="WizardForm.OnHelpButtonClicked"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHelpButtonClicked(CancelEventArgs e)
        {
            base.OnHelpButtonClicked(e);

            string helpRelativeLocation = this.wizardConfig.Help;

            Uri baseLocation = null;
            string helpLocation = string.Empty;

            if (Uri.TryCreate(helpRelativeLocation, UriKind.RelativeOrAbsolute, out baseLocation))
            {
                if (baseLocation != null)
                {
                    if (baseLocation.IsAbsoluteUri)
                    {
                        if (baseLocation.IsFile)
                        {
                            helpLocation = baseLocation.LocalPath;
                        }
                        else
                        {
                            helpLocation = baseLocation.AbsoluteUri;
                        }
                    }
                    else
                    {
                        helpLocation = Path.Combine(basePath, helpRelativeLocation);
                    }
                }
            }
            else
            {
                throw new Exception(string.Format(Resources.WizardGatheringService_CannotGetHelpResource, helpLocation));
            }

             
            Microsoft.VisualStudio.VSHelp.Help help = (Microsoft.VisualStudio.VSHelp.Help)base.GetService(typeof(Microsoft.VisualStudio.VSHelp.Help));
            if (help != null)
            {
                try
                {
                    help.DisplayTopicFromURL(helpLocation);
                    Cursor.Current = Cursors.Arrow;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format(Resources.WizardGatheringService_CannotGetHelpResource, helpLocation),ex);
                }
            }
        }

		/// <summary>
		/// See <see cref="Microsoft.WizardFramework.WizardForm.OnFinish"/>.
		/// </summary>
		public override void OnFinish()
		{
			// WORKAROUND: v-oscca: Remove override when VSWhidbey bug #356989 is fixed.
			base.OnFinish();
			gatheringResult = ExecutionResult.Finish;
		}

		#endregion

		#region Private Members

		private void CheckValueArguments()
		{
			if (initializing)
			{
				Hashtable arguments = new System.Collections.Hashtable();
				for (int iPage = 0; iPage < this.PageCount;iPage++)
				{
					WizardPage page=(WizardPage)this.GetPage(iPage);
					foreach(ValueInfo argument in page.Arguments.Keys)
					{
						if (argument == null)
						{
							throw new System.Configuration.ConfigurationException(
								Properties.Resources.WizardGatheringService_InvalidConfiguration);
						}
						if (arguments[argument] != null)
						{
							WizardPage boundPage = (WizardPage)arguments[argument];
							throw new WizardFrameworkException(
								String.Format(CultureInfo.CurrentCulture,
								Properties.Resources.WizardGatheringService_InvalidArgForStepField,
								argument.Name,
								page.Headline,
								(boundPage != null) ? boundPage.Headline : Properties.Resources.WizardGatheringService_UnknownStep));
						}
						arguments[argument] = page;
					}
				}
			}
		}

		private void CreateWizardPage(Configuration.Page page)
		{
			WizardPage wizardPage = null;
			try
			{
				if (page.Type == null)
				{
                    if (page.Fields == null)
                    {
						throw new WizardFrameworkException(Properties.Resources.WizardGatheringService_MissingFields);
                    }
					wizardPage = new WizardPageFromConfig(this);
				}
				else
				{
					ITypeResolutionService loaderService = (ITypeResolutionService)
						ServiceProvider.GetService(typeof(ITypeResolutionService));
					System.Type stepType = loaderService.GetType(page.Type, true);
					wizardPage = (WizardPage)Activator.CreateInstance(stepType, new object[] { this });
				}
				((System.ComponentModel.ISupportInitialize)(wizardPage)).BeginInit();
				wizardPage.Configuration = page;
				((System.ComponentModel.ISupportInitialize)(wizardPage)).EndInit();
			}
			catch (Exception exception)
			{
				if (wizardPage != null)
				{
					wizardPage.Dispose();
					wizardPage = null;
				}
				throw new WizardFrameworkException(String.Format(CultureInfo.CurrentCulture,
					Properties.Resources.Wizard_CannotCreateStep, page.Title, exception.Message), exception);
			}
			this.AddPage(wizardPage);
		}

		#endregion Private Members

		#region ISupportInitialize Members

		/// <summary>
		/// <see cref="ISupportInitialize.BeginInit"/>
		/// </summary>
		public void BeginInit()
		{
			this.initializing = true;
			this.Site = new ComponentModel.Site(ServiceProvider, this, this.Name);
			ServiceHelper.CheckDependencies(this);
		}

		/// <summary>
		/// <see cref="ISupportInitialize.BeginInit"/>
		/// </summary>
		public void EndInit()
		{
            IValueInfoService metaDataService = (IValueInfoService)
                GetService(typeof(IValueInfoService));
			this.Title = metaDataService.ComponentName;
			foreach (Configuration.Page step in wizardConfig.Pages) { CreateWizardPage(step); }
			this.CheckValueArguments();
			this.initializing = false;
		}

		#endregion

		#region Properties

		/// <summary>
		/// ServiceProvider for the WizardForm
		/// </summary>
		public IServiceProvider ServiceProvider
		{
			get { return serviceProvider; }
		} IServiceProvider serviceProvider;

		/// <summary>
		/// Result of the execution of this Wizard
		/// </summary>
		public ExecutionResult ExecutionResult
		{
			get { return gatheringResult; }
		}

		#endregion

		#region Starting the Wizard

		private WizardPage GetFirstPage()
		{
			for (int i = 0; i < this.PageCount; i++)
			{
				WizardPage page = this.GetPage(i) as WizardPage;
				if (page != null && page.HasNullValues)
				{
					return page;
				}
			}
			return (WizardPage)GetPage(0);
		}

		#endregion
	}
}