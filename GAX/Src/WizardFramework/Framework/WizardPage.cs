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
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using System.ComponentModel.Design;

#endregion

namespace Microsoft.Practices.WizardFramework
{
    /// <summary>
    /// A WizardPage defined a step inside a WizardForm
    /// </summary>
    [ServiceDependency(typeof(IServiceProvider))]
	[ServiceDependency(typeof(IValueInfoService))]
#if DEBUG
    public class WizardPage : Microsoft.WizardFramework.WizardPage, ISupportInitialize
#else
    public abstract class WizardPage : Microsoft.WizardFramework.WizardPage, ISupportInitialize
#endif
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
			this.SuspendLayout();
			// 
			// infoPanel
			// 
			this.infoPanel.Size = new System.Drawing.Size(505, 459);
			// 
			// WizardPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.InfoRTBoxSize = new System.Drawing.Size(496, 60);			
			this.Name = "WizardPage";
			this.Size = new System.Drawing.Size(505, 459);
			this.Skippable = true;
			this.SizeChanged += new System.EventHandler(this.WizardPage_SizeChanged);

			Control _infoRTBox = null;
			foreach (Control c in this.infoPanel.Controls)
			{
				if (c.Name == "_infoRTBox")
					_infoRTBox = c;
			}
			_infoRTBox.BackColor = Color.FromArgb( _infoRTBox.ForeColor.ToArgb() + 0x181818) ;  //CalcContrastColor(_infoRTBox.ForeColor);

			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		#endregion Designer Stuff

		//static Color CalcContrastColor(Color color)
		//{
		//	if (Math.Abs(color.R - 0x80) <= 0x20 &&
		//		Math.Abs(color.G - 0x80) <= 0x20 &&
		//		Math.Abs(color.B - 0x80) <= 0x20)
		//		return Color.FromArgb((0x7F7F7F + color.ToArgb()) & 0xFFFFFF);
		//	else
		//		return Color.FromArgb(color.ToArgb() ^ 0xFFFFFF);
		//}

		#region Constructor

			/// <summary>
			/// Creates a WizardPage inside the Windows Form Designer
			/// </summary>
		public WizardPage()
		{
			arguments = new Hashtable();
			InitializeComponent();
		}

		/// <summary>
		/// Creates a WizardPage inside a WizardFrom
		/// </summary>
		/// <param name="wizard"></param>
		public WizardPage(WizardForm wizard):base((Microsoft.WizardFramework.WizardForm)wizard)
		{
			arguments = new Hashtable();
			InitializeComponent();
		}

        #endregion Constructor

        #region Overrides

        /// <summary>
        /// Is called to ask the page whether or not it is OK to deactivate the page.  True means OK to
        /// deactivate the page.  False means not OK.
        /// </summary>
        public override bool CanDeactivate
        {
            get
            {
                return IsDataValid;
            }
        }

        internal void SetInfoTextHelp()
        {
            if (!String.IsNullOrEmpty(config.Help))
            {
                this.ShowInfoPanel = true;
                this.InfoRTBoxText = config.Help;
            }
        }

        /// <summary>
        /// Every time the page is entered the help box is updated
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            SetInfoTextHelp();
        }

		/// <summary>
		/// <see cref="Microsoft.WizardFramework.WizardPage.IsDataValid"/>
		/// Search for all the arguments that this Wizard gather
		/// If all of them are not null, then the Wizard can finish
		/// </summary>
		public override bool IsDataValid
		{
			get
			{
				if (base.IsDataValid)
				{
					return !HasNullValues;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Returns the prefered size of the page base on the configuration setings
		/// </summary>
		/// <param name="proposedSize"></param>
		/// <returns></returns>
		public override Size GetPreferredSize(Size proposedSize)
		{
			Size newSize = base.GetPreferredSize(proposedSize);
            if (config != null)
			{
				return new Size(
                    config.Width == 0 ? newSize.Width : config.Width,
                    config.Height == 0 ? newSize.Height : config.Height);
			}
			return newSize;
		}

#endregion

#region Properties

		/// <summary>
		/// Returns whether this Page have some argument with a null value
		/// </summary>
		public bool HasNullValues
		{
			get
			{
				IDictionaryService dictionaryService =
					GetService(typeof(IDictionaryService)) as IDictionaryService;
				if (dictionaryService == null || arguments == null)
				{
					return true; //If we are not sited or we have not build the argumetns then we cannot move on
				}
                foreach (ValueInfo argument in arguments.Keys)
				{
					if (dictionaryService.GetValue(argument.Name) == null && argument.IsRequired)
					{
						return true;
					}
				}
				return false;
			}
		}

		/// <summary>
		/// True when the <see cref="WizardPage"/> is been built, true between the calls <see cref="ISupportInitialize.BeginInit"/> and <see cref="ISupportInitialize.EndInit"/>
		/// </summary>
		protected bool IsInitializing
		{
			get { return initializing; }
			set { initializing = value; }
		} bool initializing = true;
 
		/// <summary>
		/// Step configuration used to build this <see cref="WizardPage"/>
		/// </summary>
		public Configuration.Page Configuration
		{
			get { return config; }
			set { config = value; }
		} Configuration.Page config;

		/// <summary>
		/// Exposes the parent wizard.
		/// </summary>
		public new WizardForm Wizard
		{
			get { return (WizardForm) base.Wizard; }
		} 

		/// <summary>
		/// The list of arguments that this WizardPage is going to gather
		/// </summary>
		public Hashtable Arguments
		{
			get { return arguments; }
		} Hashtable arguments;

#endregion Properties

#region Abstract Members

        /// <summary>
        /// Fills the arguments hashtable with ArgumentMetaData objects corresponsing to the arguments that this Page collects.
        /// </summary>
#if DEBUG
        protected virtual void BuildRecipeArguments() { throw new NotImplementedException("abstract method."); }
#else
        protected abstract void BuildRecipeArguments();
#endif

#endregion

#region ISupportInitialize Members

        /// <summary>
        /// Begins component initialization
        /// </summary>
        public virtual void BeginInit()
		{
			initializing = true;
		}

		/// <summary>
		/// Ends initialization of the component
		/// </summary>
		public virtual void EndInit()
		{
            if (this.Wizard != null && config != null)
			{
				IServiceProvider serviceProvider = (IServiceProvider)this.Wizard.ServiceProvider;
				this.Site = new ComponentModel.Site(serviceProvider, this, this.Name);
				ServiceHelper.CheckDependencies(this);
                if (((this.Headline != null && this.Headline.Equals(String.Empty)) || (this.Headline == null)) &&
                    (config.Title != null && !config.Title.Equals(String.Empty)))
                {
                    this.Headline = config.Title;
                }
                if ((this.StepTitle != null && this.StepTitle.Equals(String.Empty)) || (this.StepTitle == null))
                {
                    if (config.LinkTitle != null && !config.LinkTitle.Equals(String.Empty))
				    {
                        this.StepTitle = config.LinkTitle;
				    }
                    else if (config.Title != null && !config.Title.Equals(String.Empty))
				    {
                        this.StepTitle = config.Title;
				    }
                }
                SetInfoTextHelp();
				this.BuildRecipeArguments();
				initializing = false;
			}
		}

		#endregion ISupportInitialize Members

		public bool SuncInfoRTWidth { get; set; } = true;

		private void WizardPage_SizeChanged(object sender, EventArgs e)
		{ 
			if(SuncInfoRTWidth && this.InfoRTBoxSize. Width != this.infoPanel.Width)
				this.InfoRTBoxSize = new Size(this.infoPanel.Width, this.InfoRTBoxSize.Height);
		}
	}
}

