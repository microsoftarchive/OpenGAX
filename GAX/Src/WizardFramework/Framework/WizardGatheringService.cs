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
using System.Windows.Forms;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Collections;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Microsoft.Practices.Common;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Globalization;
using System.Threading;

//using Mvp.Xml.XInclude;

#endregion

namespace Microsoft.Practices.WizardFramework
{
	/// <summary>
	/// Gathers arguments using a wizard interface.
	/// </summary>
	[ServiceDependency(typeof(ITypeResolutionService))]
	[System.ComponentModel.DesignerCategory("Code")]
	public sealed class WizardGatheringService : ComponentModel.ServiceContainer, IValueGatheringService
	{
		private class WizardData
		{
			private class TypeAliasAdapterDictionary : Hashtable
			{
				public TypeAliasAdapterDictionary(Configuration.TypeAlias[] typeAlias)
				{
					if (typeAlias != null)
					{
						foreach (Configuration.TypeAlias alias in typeAlias)
						{
							Add(alias.Name, alias.Type);
						}
					}
				}
			}
			public Configuration.Wizard WizardConfig
			{
				get { return wizardConfig; }
			} Configuration.Wizard wizardConfig;
			public IDictionary TypeAliases
			{
				get { return typeAliases; }
			} TypeAliasAdapterDictionary typeAliases;
			public WizardData(Configuration.Wizard wizardConfig)
			{
				this.wizardConfig = wizardConfig;
				this.typeAliases = new TypeAliasAdapterDictionary(this.wizardConfig.Types);
			}
		}
		private Hashtable wizardConfigs;

		/// <summary>
		/// Default constructor
		/// </summary>
		public WizardGatheringService()
		{
			wizardConfigs = new Hashtable();
		}

		private WizardData GetWizardConfiguration(XmlElement data)
		{
			if (wizardConfigs[data] != null)
			{
				return (WizardData)wizardConfigs[data];
			}
			if (data == null || !(data is XmlNode) )
			{
				return null;
			}

			XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ConformanceLevel = ConformanceLevel.Auto;

            // Pull the XSD from the config assembly (it's an embedded resource).
            using (Stream xsdstream = typeof(Configuration.Wizard).Assembly.GetManifestResourceStream(
                Configuration.Wizard.SchemaResourceName))
            {
                Debug.Assert(xsdstream != null, "XSD not embedded in config assembly");
                // If the schema is not valid (we must check that at design-time), this will throw.
                XmlSchema xsd = XmlSchema.Read(xsdstream, null);
                settings.Schemas.Add(xsd);
            }

            // XInclude at this point has already been resolved by the parent guidance package 
			// configuration reading process.
            XmlReader reader = new XmlNodeReader(data);
            XmlReader xr = XmlReader.Create(reader, settings);
			try
			{
				WizardData wizard = new WizardData(
					(Configuration.Wizard)new Configuration.WizardSerializer().Deserialize(xr));
				wizardConfigs.Add(data, wizard);
				return wizard;
			}
			catch (Exception e)
			{
				wizardConfigs.Remove(data);
				throw new WizardFrameworkException(Properties.Resources.WizardGatheringService_InvalidConfiguration, e);
			}
        }

        #region IValueGatheringService Members

        ExecutionResult IValueGatheringService.Execute(XmlElement data, bool allowSuspend)
		{
			WizardData wizardData = GetWizardConfiguration(data);
			if (wizardData == null)
			{
				throw new System.Configuration.ConfigurationException(
                    Properties.Resources.WizardGatheringService_CannotLoadConfig);
			}
			WizardForm form = null;
			AliasResolutionService aliases = null;
            try
            {
                // Setup alias resolution for the current wizard
                aliases = new AliasResolutionService(wizardData.TypeAliases,
                   GetService<ITypeResolutionService>(true));
                AddService(typeof(ITypeResolutionService), aliases);

                form = new WizardForm(this, wizardData.WizardConfig);
                // Site the form in our own container.
                Add(form);

				IUIService uiService = GetService<IUIService>();
                IWin32Window parentWnd = null;
                if (uiService != null)
                {
                    parentWnd = uiService.GetDialogOwnerWindow();
                }
                form.Start(parentWnd);
                return form.ExecutionResult;
            }
            catch (Exception e)
            {
                string wizardName = "";
                if (wizardData != null && wizardData.WizardConfig != null)
                {
                    wizardName = wizardData.WizardConfig.Name;
                }
				throw new WizardExecutionException(wizardName, e);
            }
			finally
			{
				// Cleanup the service we added temporarily.
				if (aliases != null && aliases == GetService(typeof(ITypeResolutionService)))
				{
					RemoveService(typeof(ITypeResolutionService));
				}

				// Remove from parent container always.
				if (form!=null)
				{
					Remove(form);
					form.Dispose();
					form = null;
				}
			}
        }

		#endregion

        #region Overrides

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion
    }
}
