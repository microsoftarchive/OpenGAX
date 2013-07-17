using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Globalization;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using System.Drawing.Design;

namespace Microsoft.Practices.WizardFramework
{
	/// <summary>
	/// Summary description for ArgumentPanelTypeEditor.
	/// </summary>
	[DesignerCategory("Form")]
	[ServiceDependency(typeof(IValueInfoService))]
	public class ArgumentPanelTypeEditor : ArgumentPanel, ISupportInitialize
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
            this.valueEditor = new Microsoft.Practices.WizardFramework.ValueEditor();
            this.argumentLabel = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.valueEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // valueEditor
            // 
            this.valueEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.valueEditor.BackColor = System.Drawing.SystemColors.Window;
            this.valueEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.valueEditor.ConverterInstance = null;
            this.valueEditor.EditorType = null;
            this.valueEditor.ForeColor = System.Drawing.SystemColors.WindowText;
            this.valueEditor.Location = new System.Drawing.Point(3, 18);
            this.valueEditor.MinimumSize = new System.Drawing.Size(100, 18);
            this.valueEditor.Name = "valueEditor";
            this.valueEditor.Size = new System.Drawing.Size(200, 18);
            this.valueEditor.TabIndex = 0;
            this.valueEditor.ToolTip = "";
            this.valueEditor.ValueRequired = false;
            this.valueEditor.ValueType = null;
            this.valueEditor.ValueChanged += new System.ComponentModel.Design.ComponentChangedEventHandler(this.ValueChanged);
            this.valueEditor.InvalidValue += new System.ComponentModel.Design.ComponentChangedEventHandler(this.InvalidValue);
            // 
            // argumentLabel
            // 
            this.argumentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.argumentLabel.AutoSize = false;
            this.argumentLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            //this.argumentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.argumentLabel.Location = new System.Drawing.Point(0, 0);
            this.argumentLabel.Multiline = true;
            this.argumentLabel.Name = "argumentLabel";
            this.argumentLabel.ReadOnly = true;
            this.argumentLabel.Size = new System.Drawing.Size(200, 18);
            this.argumentLabel.TabIndex = 0;
            this.argumentLabel.TabStop = false;
            this.argumentLabel.Text = "argumentLabel";
            // 
            // ArgumentPanelTypeEditor
            // 
            this.Controls.Add(this.valueEditor);
            this.Controls.Add(this.argumentLabel);
            this.InvalidValueMessage = "Invalid Value";
            this.Size = new System.Drawing.Size(218, 42);
            ((System.ComponentModel.ISupportInitialize)(this.valueEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

		/// <summary>
		/// It is the control that can edit the value of the argument
		/// </summary>
        protected ValueEditor valueEditor;
		/// <summary>
		/// It is the label that displays information about the argument
		/// </summary>
        protected System.Windows.Forms.TextBox argumentLabel;

        #endregion Designer Stuff

        #region Default Constructor

        /// <summary>
		/// Default constructor
		/// </summary>
		public ArgumentPanelTypeEditor()
		{
			InitializeComponent();
		}

		#endregion

		#region Properties

		/// <summary>
		/// The ValueEditor ojbect editing the <see cref="ValueType"/>
		/// </summary>
		[Browsable(false)]
		public ValueEditor ValueEditor
		{
			get { return valueEditor; }
		}

        /// <summary>
        /// A <see cref="UITypeEditor"/> instance that knows how to edit the <see cref="ValueType"/>.
        /// </summary>
        /// <remarks>
        /// If an instance is specified, the <see cref="EditorType"/> will be ignored. Otherwise, an 
        /// instance of that type will be created and assigned to this property.
        /// </remarks>
        [Browsable(false)]
        public UITypeEditor EditorInstance
        {
            get { return valueEditor.EditorInstance; }
            set 
            { 
                valueEditor.EditorInstance = value; 
                // For consistency.
                valueEditor.EditorType = value.GetType();
            }
        } 

		/// <summary>
		/// The EditorType for the <see cref="ValueEditor"/>
		/// </summary>
		[Browsable(true)]
		public Type EditorType
		{
			get { return valueEditor.EditorType; }
			set { valueEditor.EditorType = value; }
		}

		/// <summary>
		/// An instance of a <see cref="TypeConverter"/> for the <see cref="ValueEditor"/>.
		/// </summary>
		[Browsable(true)]
		public TypeConverter ConverterInstance
		{
			get { return valueEditor.ConverterInstance; }
			set { valueEditor.ConverterInstance = value; }
		}

		#endregion

		#region ISupportInitialize Members

		/// <summary>
		/// <see cref="ISupportInitialize.BeginInit"/>
		/// </summary>
		public override void BeginInit()
		{
			base.BeginInit();
			((ISupportInitialize)(this.valueEditor)).BeginInit();
		}

		/// <summary>
		/// <see cref="ISupportInitialize.BeginInit"/>
		/// </summary>
		public override void EndInit()
		{
			base.EndInit();
			//If we keep initializing then do nothing
			if (IsInitializing)
			{
				return;
			}
			//OK: The base class is now initialized
			argumentLabel.Text = this.FieldConfig.Label;
			argumentLabel.Cursor = this.Cursor;
            IValueInfoService metaDataService =
                (IValueInfoService)GetService(typeof(IValueInfoService));
            valueEditor.ValueRequired = metaDataService.GetInfo(this.FieldConfig.ValueName).IsRequired;
			valueEditor.ValueType = this.ValueType;
			valueEditor.ToolTip = this.FieldConfig.Tooltip;
            valueEditor.ReadOnly = this.FieldConfig.ReadOnly;
			((ISupportInitialize)(this.valueEditor)).EndInit();
		}

		#endregion

		#region ValueEditor Event Handlers

		private void ValueChanged(object sender, ComponentChangedEventArgs e)
		{
			SetValue(valueEditor.Value);
		}

		private void InvalidValue(object sender, ComponentChangedEventArgs e)
		{
			OnInvalidValue();
		}

		#endregion

		#region Overrides

		/// <summary>
		/// <see cref="UpdateValue"/>
		/// </summary>
		/// <param name="newValue"></param>
		protected override void UpdateValue(object newValue)
		{
			//For boolean values, assume that a null value if false
			valueEditor.Value = newValue;
		}

		/// <summary>
		/// Adjust the ScrollBar property of the label depending on the size of the component
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			if ( this.argumentLabel!=null )
			{
				using (Bitmap bitmap = new Bitmap(1, 1))
				{
					SizeF textSize = Graphics.FromImage(bitmap).MeasureString(this.argumentLabel.Text, this.argumentLabel.Font);
					if (textSize.Width >= this.argumentLabel.Size.Width)
					{
						this.argumentLabel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
					}
				}
			}
		}

        /// <summary>
        /// See <see cref="Control.OnGotFocus"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.ValueEditor.Focus();
        }

		#endregion
	}
}
