#region Using directives

using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using System.Diagnostics;
using Microsoft.Practices.Common;
using System.Windows.Forms.Design;

#endregion

namespace Microsoft.Practices.WizardFramework
{
    /// <summary>
    /// An ArgumentPanel is a UI artifact that collects the value of a RecipeArgument.
    /// </summary>
    [ServiceDependency(typeof(IServiceProvider))]
	[ServiceDependency(typeof(IDictionaryService))]
	[ServiceDependency(typeof(ITypeResolutionService))]
    [ServiceDependency(typeof(IValueInfoService))]
	public class ArgumentPanel : System.Windows.Forms.Panel, ISupportInitialize
    {
        #region Designer Stuff

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArgumentPanel));
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.invalidValuePictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.invalidValuePictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// invalidValuePictureBox
			// 
			this.invalidValuePictureBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.invalidValuePictureBox.Image = ((System.Drawing.Image)(resources.GetObject("invalidValuePictureBox.Image")));
			this.invalidValuePictureBox.Location = new System.Drawing.Point(202, 19);
			this.invalidValuePictureBox.Margin = new System.Windows.Forms.Padding(0);
			this.invalidValuePictureBox.MaximumSize = new System.Drawing.Size(16, 16);
			this.invalidValuePictureBox.MinimumSize = new System.Drawing.Size(16, 16);
			this.invalidValuePictureBox.Name = "invalidValuePictureBox";
			this.invalidValuePictureBox.Size = new System.Drawing.Size(16, 16);
			this.invalidValuePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.invalidValuePictureBox.TabIndex = 0;
			this.invalidValuePictureBox.TabStop = false;
			this.invalidValuePictureBox.Visible = false;
			// 
			// ArgumentPanel
			// 
			this.Controls.Add(this.invalidValuePictureBox);
			this.Dock = System.Windows.Forms.DockStyle.Top;
			((System.ComponentModel.ISupportInitialize)(this.invalidValuePictureBox)).EndInit();
			this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Picture box to display invalid value icon
        /// </summary>
        protected System.Windows.Forms.PictureBox invalidValuePictureBox;
		/// <summary>
		/// tooltip control to display the tooltips of the Argument Panel
		/// </summary>
        protected System.Windows.Forms.ToolTip toolTip;

        #endregion Designer Stuff

        #region Fields

        /// <summary>
		/// Indicated when the component is been initialized, this value is true between the calls to BeginInit and EndInit
		/// </summary>
		private bool initializing;

		#endregion

		#region Constructor

		/// <summary>
		/// Builds a ArgumentPanel for the current argument of the current field
		/// </summary>
		public ArgumentPanel()
		{
			initializing = true;
			InitializeComponent();
		}

		#endregion 
		
		#region Properties

		/// <summary>
		/// Indicated when the component is been initialized, this value is true between the calls to BeginInit and EndInit
		/// </summary>
		protected bool IsInitializing
		{
			get { return initializing; }
		}

		/// <summary>
		/// Tooltip for elements inside an ArgumentPanel
		/// </summary>
		[Browsable(false)]
		internal ToolTip ToolTip
		{
			get { return toolTip; }
		}

		/// <summary>
		/// Field configuration for the field that will component will draw
		/// </summary>
		[Browsable(true)]
		public Configuration.Field FieldConfig
		{
			get { return fieldConfig; }
			set { fieldConfig = value; }
		} Configuration.Field fieldConfig; 

		/// <summary>
		/// Type of the value been collected
		/// </summary>
		[Browsable(false)]
		protected Type ValueType
		{
			get { return valueType; }
		} Type valueType;

		/// <summary>
		/// Message to display to the user when an invalid value is entered
		/// </summary>
		[Browsable(true)]
		public string InvalidValueMessage
		{
			get { return invalidValueMessage; }
			set { invalidValueMessage = value; }
		} string invalidValueMessage;

		/// <summary>
		/// The <seealso cref="WizardPage"/> where this ArgumentPanel is sited
		/// </summary>
		[Browsable(false)]
		public WizardPage WizardPage
		{
			get
			{
				if (wizardPage == null)
				{
					wizardPage = (WizardPage)this.Parent;
				}
				return wizardPage;
			}
		} WizardPage wizardPage;

		private readonly static object NotNullValue = new object();
		/// <summary>
		/// True when the Value is been updated
		/// </summary>
		private object ValueBeenSet
		{
			get { return valueBeenSet; }
			set { valueBeenSet = value; }
		} object valueBeenSet = NotNullValue;

		#endregion

        #region Value Management

        /// <summary>
		/// Binds <paramref name=" newValue"/> to the recipe argument
		/// </summary>
		/// <param name="newValue"></param>
		/// <returns>Returns the updated value, if the update is successfull returns newValue</returns>
		protected object SetValue(object newValue)
		{
			try
			{
				if (this.invalidValuePictureBox.Visible)
				{
					this.toolTip.SetToolTip(this.invalidValuePictureBox, "");
					this.invalidValuePictureBox.Visible = false;
				}
				if (initializing || valueBeenSet==null || (valueBeenSet!=null && valueBeenSet.Equals(newValue)))
				{
					return newValue;
				}
				bool error = false;
				IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
				try
				{
					dictionaryService.SetValue(fieldConfig.ValueName, newValue);
				}
				catch (Exception e)
				{
					this.toolTip.SetToolTip(this.invalidValuePictureBox, e.Message);
					this.invalidValuePictureBox.Visible = true;
					error = true;
				}
				object value = dictionaryService.GetValue(fieldConfig.ValueName);
				if (!error)
				{
					this.WizardPage.Wizard.OnValidationStateChanged(this.WizardPage);
				}
				return value;
			}
			catch (Exception e)
			{
				ErrorHelper.Show(this.Site, e);
				return null;
			}
		}

		/// <summary>
		/// Updates the UI with <paramref name="newValue"/>
		/// </summary>
		/// <param name="newValue"></param>
		protected virtual void UpdateValue(object newValue)
		{
		}

		/// <summary>
		/// Displays the error icon on the ArgumentPanel when an invalid value is entered
		/// </summary>
		protected void OnInvalidValue()
		{
            // Set the dictionary to null, we are entering an invalid value.
            // The UI should not change the current invalid value
            try
            {
                IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
                if (dictionaryService != null)
                {
                    dictionaryService.SetValue(this.fieldConfig.ValueName, null);
                }
            }
            finally
            {
    			this.WizardPage.Wizard.OnValidationStateChanged(this.WizardPage);
                this.toolTip.SetToolTip(this.invalidValuePictureBox, this.InvalidValueMessage);
                this.invalidValuePictureBox.Visible = true;
            }
		}

		internal void UpdateValueInternal(object newValue)
		{
			try
			{
				valueBeenSet = newValue;
				UpdateValue(newValue);
			}
			catch (Exception e)
			{
                this.toolTip.SetToolTip(this.invalidValuePictureBox, e.Message );
                this.invalidValuePictureBox.Visible = true;
				this.TraceError(e.ToString());
			}
			finally
			{
				valueBeenSet = NotNullValue;
			}
		}

		internal void SetValue()
		{
			IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
			if (dictionaryService == null)
			{
				throw new ComponentModel.ServiceMissingException(typeof(IDictionaryService),this);
			}
			// A null value in currentValue, means that the value is not bound to any value
            object initialValue = dictionaryService.GetValue(this.fieldConfig.ValueName);
			UpdateValueInternal(initialValue);
        }

        #endregion

        #region ISupportInitialize Members

        /// <summary>
        /// Begin the intialization process
        /// </summary>
        public virtual void BeginInit()
        {
            initializing = true;
            invalidValueMessage = Properties.Resources.ValueEditor_InvalidValue;
        }

        /// <summary>
        /// Ends the construction of the component
        /// </summary>
        public virtual void EndInit()
        {
            if (Parent == null)
            {
                return;
            }
            if (Parent.Site != null)
            {
                this.Name = this.fieldConfig.ValueName;
                if (this.fieldConfig.InvalidValueMessage != null)
                {
                    this.invalidValueMessage = this.fieldConfig.InvalidValueMessage;
                }
                IServiceProvider serviceProvider = (IServiceProvider)Parent.Site.GetService(typeof(IServiceProvider));
                this.Site = new ComponentModel.Site(serviceProvider, this, this.Name);
                ServiceHelper.CheckDependencies(this);
                IValueInfoService metaDataService =
                    (IValueInfoService)GetService(typeof(IValueInfoService));
                valueType = metaDataService.GetInfo(this.FieldConfig.ValueName).Type;
                initializing = false;
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Everytime the panel is displayed then the Helpbox in the WizardPage is updated
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (this.Parent != null && this.Parent is Microsoft.WizardFramework.WizardPage)
            {
                Microsoft.WizardFramework.WizardPage page = this.Parent as Microsoft.WizardFramework.WizardPage;
                if (page == null)
                {
                    return;
                }
                if (FieldConfig != null && !string.IsNullOrEmpty(FieldConfig.Help))
                {
                    page.ShowInfoPanel = true;
                    page.InfoRTBoxText = this.FieldConfig.Help;
                }
                else if ( page is WizardPage )
                {
                    (page as WizardPage).SetInfoTextHelp();
                }
            }
        }

        /// <summary>
        /// If we are leaving the panel, then make sure to turn off the error icon
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLeave(EventArgs e)
        {
            if (this.invalidValuePictureBox.Visible)
            {
                this.invalidValuePictureBox.Visible = false;
            }
            base.OnLeave(e);
        }

        #endregion
    }
}
