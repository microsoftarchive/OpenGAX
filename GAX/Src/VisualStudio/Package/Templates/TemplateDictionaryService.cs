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
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.Collections;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using System.ComponentModel;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;

#endregion

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Templates
{
    [ServiceDependency(typeof(IDictionaryService))]
	internal sealed class TemplateDictionaryService: Microsoft.Practices.ComponentModel.ServiceContainer, IDictionaryService
    {
        #region Properties

        internal IDictionary State
        {
            get 
            { 
                return arguments; 
            }
        } System.Collections.Specialized.HybridDictionary arguments;

        #endregion

        #region Fields and constructor

        IDictionaryService realDictioraryService;
        IComponentChangeService componentChangeService;
        Dictionary<string, string> replacementDictionary;

		public TemplateDictionaryService(Dictionary<string, string> replacementDictionary)
		{
			this.replacementDictionary = replacementDictionary;
            this.arguments = new System.Collections.Specialized.HybridDictionary();
            // Context parameters are processed first, so that replacement dictionary values
            // may replace them.
            if (VszWizard.ContextParams != null)
            {
                foreach (DictionaryEntry entry in VszWizard.ContextParams)
                {
                    arguments[entry.Key] = entry.Value;
                }
            }
            foreach (KeyValuePair<String, String> keyValuePair in replacementDictionary)
            {
                string key = keyValuePair.Key.Substring(1, keyValuePair.Key.Length - 2);
                arguments[key] = keyValuePair.Value;
            }
        }

        #endregion

        #region Overrides

        protected override void OnSited()
        {
            base.OnSited();
            this.realDictioraryService = GetService<IDictionaryService>(true);
            this.componentChangeService = this.realDictioraryService as IComponentChangeService;
            if (this.componentChangeService != null)
            {
                this.componentChangeService.ComponentChanged += new ComponentChangedEventHandler(OnComponentChanged);
            }
        }

        void OnComponentChanged(object sender, ComponentChangedEventArgs e)
        {
            Guard.ArgumentNotNull(e.Component, "Component");
            ValueInfo valueInfo = (ValueInfo)e.Component;
            arguments[valueInfo.Name] = e.NewValue;
        }

        #endregion
        
        #region IDictionaryService members

        public object GetKey(object value)
		{
			return realDictioraryService.GetKey(value);
		}

		public object GetValue(object key)
		{
            object value = null;
            if (realDictioraryService != null)
            {
                value = realDictioraryService.GetValue(key);
                if (value != null)
                {
                    string replacementKey = "$" + key.ToString() + "$";
                    if (!replacementDictionary.ContainsKey(replacementKey))
                    {
                        replacementDictionary[replacementKey] = value.ToString();
                        arguments[key] = value;
                    }
                }
            }
            if (value == null && key is string )
            {
                string dollarKey = "$"+(string)key+"$";
                if (replacementDictionary.ContainsKey(dollarKey))
                {
                    value = replacementDictionary[dollarKey];
                    if (value != null)
                    {
                        if (realDictioraryService != null)
                        {
							try
							{
								realDictioraryService.SetValue(key, value);
							}
							catch (Exception)
							{
								replacementDictionary.Remove(dollarKey);
							}
                        }
                        arguments[key] = value;
                    }
                }
                else
                {
                    // Try to get from the arguments we built so far.
                    value = arguments[key];
                }
            }
			return value;
		}

		public void SetValue(object key, object value)
		{
            if (realDictioraryService != null )
            {
                // The TypeDescriptor in .NET also uses the IDictionaryService
                // If the cache is activated then we are called using the typeof(object) as key
                if (key.GetType()!=typeof(object))
                {
                    realDictioraryService.SetValue(key, value);
                }
            }
            arguments[key] = value;
            string dollarKey = "$" + key.ToString() + "$";
            if (value != null)
            {
                replacementDictionary[dollarKey] = value.ToString();
            }
            else
            {
                replacementDictionary.Remove(dollarKey);
                arguments.Remove(key);
            }
		}

		#endregion
	}
}
