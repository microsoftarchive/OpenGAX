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
using System.ComponentModel.Design;
using System.Globalization;

using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using System.Reflection;
using Microsoft.Practices.Common;
using System.Windows.Forms.Design;

#endregion Using directives

namespace Microsoft.Practices.WizardFramework
{
    /// <summary>
    /// The parent class for any custom wizard page.
    /// Inherit from this class when you want to create a custom user interface for a page in a wizard.
    /// </summary>
    /// <remarks>
    /// For each recipe argument that you want to collect, you must declare a setter property matching the name of the 
    /// recipe argument and decorate it with the attribute RecipeArgument.
    /// </remarks>
    /// <example>
    /// <code>
    /// [RecipeArgument]
    /// public TheTypeOfTheRecipeArgument RecipeArgumentName 
    /// { 
    ///     set 
    ///     { 
    ///         if ( value == null )
    ///         {
    ///            // Update your UI to draw NULL value
    ///            ...
    ///         }
    ///         else
    ///         {
    ///            // Update your UI
    ///            ...
    ///         }
    ///     }
    /// }
    /// </code>
    /// <note>
    /// Do not forget to check for <see langword="null"/> in your setter property.
    /// The WizardFramework will automatically invoke the setter of your property every time the recipe argument value changes.
    /// The WizardFramework will also invoke your setter with a <see langword="null"/> value if the recipe argument value is <see langword="null"/>.
    /// It is your responsibility to update your UI accordingly.
    /// From your UI code you must manually query for the service IDictionaryService and invoke SetValue to update the value of the RecipeArgument
    /// </note>
    /// </example>
    /// <seealso cref="IDictionaryService"/>
    /// <seealso cref="CustomWizardPage.RecipeArgumentAttribute"/>

    [ServiceDependency(typeof(IValueInfoService))]
	[ServiceDependency(typeof(ITypeResolutionService))]
	[ServiceDependency(typeof(IServiceProvider))]
	public class CustomWizardPage : WizardPage
    {
        #region Designer Stuff

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
			this.infoPanel.Size = new System.Drawing.Size(620, 459);
			// 
			// CustomWizardPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.Name = "CustomWizardPage";
			this.Size = new System.Drawing.Size(620, 469);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }
        #endregion

        #endregion Designer Stuff

        #region Constructors & Fields

        Hashtable changingProperties;

        /// <summary>
		/// Creates a WizardPage inside the Windows Form Designer
		/// </summary>
		public CustomWizardPage()
		{
            changingProperties = new Hashtable();
			InitializeComponent();
		}

		/// <summary>
		/// Creates a WizardPage inside a WizardForm.
		/// </summary>
		/// <param name="wizard">The <see cref="WizardForm"/>that will host the new page.</param>
		public CustomWizardPage(WizardForm wizard)
			: base(wizard)
		{
            changingProperties = new Hashtable();
            InitializeComponent();
		}

		#endregion

		#region Recipe Arguments management

		/// <summary>
		/// Attribute to decorate each setter property in a custom wizard page.
		/// </summary>
		[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
		/*protected*/public sealed class RecipeArgumentAttribute : Attribute
		{
			/// <summary>
			/// The default constructor.
			/// </summary>
			public RecipeArgumentAttribute()
			{
			}
		}

		/// <summary>
		/// Gets the value in the recipe dictionary
		/// </summary>
		/// <param name="recipeArgument"></param>
		/// <returns></returns>
		protected object this[string recipeArgument]
		{
			get
			{
                IDictionaryService dictionaryService=
                    GetService(typeof(IDictionaryService)) as IDictionaryService;
                if (dictionaryService != null)
                {
                    return dictionaryService.GetValue(recipeArgument);
                }
				return null;
			}
		}

		#endregion

		#region Overrides 

		/// <summary>
        /// Builds the list of recipe arguments collected by this wizard, based on the defined setter properties.
		/// <seealso cref="WizardPage.BuildRecipeArguments"/>
		/// </summary>
		protected override void BuildRecipeArguments()
		{
            IValueInfoService metaDataService =
                (IValueInfoService)GetService(typeof(IValueInfoService));
			foreach (PropertyInfo property in this.GetType().GetProperties())
			{
				if ( Attribute.GetCustomAttribute(property,typeof(RecipeArgumentAttribute),true)!= null)
				{
					ValueInfo argument = metaDataService.GetInfo(property.Name);
					if (argument == null)
					{
						throw new WizardFrameworkException(
							String.Format(
								CultureInfo.CurrentCulture,
								Properties.Resources.WizardGatheringService_MissingArgumentMetaData,
								property.Name));
					}
					Arguments.Add(argument, property);
				}
			}
		}

		/// <summary>
        /// On activation, invokes all setter properties with the current recipe argument value.
		/// <seealso cref="Microsoft.WizardFramework.WizardPage.OnActivate"/>
		/// </summary>
		/// <returns></returns>
		public override bool OnActivate()
		{
			if (base.OnActivate())
			{
				IDictionaryService dictionaryService =
					(IDictionaryService)GetService(typeof(IDictionaryService));
				foreach (DictionaryEntry entry in Arguments)
				{
					ValueInfo argument = (ValueInfo)entry.Key;
					PropertyInfo property = (PropertyInfo)entry.Value;
					try
					{
						property.SetValue(this, dictionaryService.GetValue(argument.Name), null);
						this.Wizard.OnValidationStateChanged(this);
					}
					catch (Exception ex)
					{
						throw new InvalidOperationException(
							String.Format(CultureInfo.CurrentCulture,
								Properties.Resources.WizardGatheringService_CannotSetProperty,
								property.Name,
								GetType().Name), ex);
					}

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

		/// <summary>
		/// <see cref="Microsoft.WizardFramework.WizardPage.OnDeactivated"/>
		/// </summary>
		/// <returns></returns>
		public override void OnDeactivated()
		{
            try
            {
                IComponentChangeService componentChange =
                    (IComponentChangeService)GetService(typeof(IComponentChangeService));
                if (componentChange != null)
                {
                    componentChange.ComponentChanged -= new ComponentChangedEventHandler(RecipeArgumentChanged);
                }
                base.OnDeactivated();
            }
            catch (Exception e)
            {
                ErrorHelper.Show(this.Site, e);
            }
        }

		#endregion

		#region Event Handlers

		private void RecipeArgumentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (e.Component == null)
			{
				throw new ArgumentNullException("Component");
			}
			// Forward call to appropriate providers.
			ValueInfo argument = (ValueInfo)e.Component;
			PropertyInfo property = (PropertyInfo)Arguments[argument];
			if (property != null && !changingProperties.ContainsKey(property))
            {
                changingProperties.Add(property, e.NewValue);
                try
                {
                    property.SetValue(this, e.NewValue, null);
					this.Wizard.OnValidationStateChanged(this);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        String.Format(CultureInfo.CurrentCulture,
                            Properties.Resources.WizardGatheringService_CannotSetProperty,
                            property.Name,
                            GetType().Name), ex);
                }
                finally
                {
                    changingProperties.Remove(property);
                }
			}
		}

		#endregion
	}
}

