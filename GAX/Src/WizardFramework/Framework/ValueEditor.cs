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
using System.Globalization;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common;

#endregion

namespace Microsoft.Practices.WizardFramework
{
	/// <summary>
	/// A ValueEditor control let's you edit any Type as long as the Type have a UITypeEditor or TypeConverter
	/// </summary>
	[ServiceDependency(typeof(IServiceProvider))]
	[ServiceDependency(typeof(IContainer))]
	public class ValueEditor : System.Windows.Forms.UserControl, ISupportInitialize
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
            if (disposing && (components != null))
            {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValueEditor));
			this.buttonDropDown = new System.Windows.Forms.Button();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.edit = new ValueEditorTextBox();
			((System.ComponentModel.ISupportInitialize)(this.edit)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonDropDown
			// 
			resources.ApplyResources(this.buttonDropDown, "buttonDropDown");
			this.buttonDropDown.BackColor = System.Drawing.SystemColors.Control;
			this.buttonDropDown.ForeColor = System.Drawing.SystemColors.Window;
			this.buttonDropDown.Name = "buttonDropDown";
			this.buttonDropDown.TabStop = false;
			this.buttonDropDown.UseVisualStyleBackColor = false;
			this.buttonDropDown.Click += new System.EventHandler(this.EditDropDownBtn);
			// 
			// edit
			// 
			resources.ApplyResources(this.edit, "edit");
			this.edit.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.edit.Name = "edit";
			// 
			// ValueEditor
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.edit);
			this.Controls.Add(this.buttonDropDown);
			this.Name = "ValueEditor";
			resources.ApplyResources(this, "$this");
			((System.ComponentModel.ISupportInitialize)(this.edit)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.Button buttonDropDown;
        private System.Windows.Forms.ToolTip toolTip;
        private ValueEditor.ValueEditorTextBox edit;

        #endregion Designer Stuff

        #region Layout constants
        private static class Constants
		{ 
			internal const int SpaceToBorder = 1;
			internal const int ImageSize = 16;
			internal const int EditPaintBoxSize = 20;
			internal const int SpaceBetweenTextAndButton = 4;
			internal const int SpaceBetweenPaintedValueAndText = 4;
			internal const int MaximunWidth = 150;
			internal const int MinimumWidth = 100;
		}
		#endregion

		#region Fields & ctor

		private bool initializing;
		private Services services;
		private Rectangle rectangleForValue;
		private DropDownHolder dropDownHolder = null;
		private ValueEditorListBox listBox = null;

		/// <summary>
		/// Returns the instance of the current text box
		/// </summary>
		public TextBox TextBox
		{
			get
			{
				return edit;
			}
		}

		/// <summary>
		/// Default construtor
		/// </summary>
		public ValueEditor()
		{
			services = new Services(this);
			initializing = true;
			InitializeComponent();
		}

		#endregion

		private bool CanUseDefaultValueEditor()
		{
			Type tmpValueType = null;
			if (valueType.IsGenericType && (valueType.GetGenericTypeDefinition() == typeof(Nullable<>)))
			{
				tmpValueType = valueType.GetGenericArguments()[0];
			}
			else
			{
				tmpValueType = valueType;
			}
			if (tmpValueType.IsEnum)
			{
				return false;
			}
			else if (tmpValueType == typeof(string))
			{
				return false;
			}
			else if (tmpValueType.GetProperties().Length == 0)
			{
				return false;
			}
			else if (typeConverter != null && typeConverter.GetStandardValuesSupported(services))
			{
				return false;
			}
			return true;
		}

		#region Designer Properties

		/// <summary>
		/// Type of the Value that this ValueEditor will edit
		/// </summary>
		[Browsable(true)]
		public Type ValueType
		{
			get { return valueType; }
			set
			{
				if (!initializing) throw new InvalidOperationException(Properties.Resources.Component_Already_Initialized);
				valueType = value;
			}
		} Type valueType;

		/// <summary>
		/// Type of a the <see cref="UITypeEditor"/> that know how to edit a <see cref="ValueType"/>
		/// </summary>
		[Browsable(true)]
		public Type EditorType
		{
			get { return editorType; }
			set
			{
				if (!initializing) throw new InvalidOperationException(Properties.Resources.Component_Already_Initialized);
				editorType = value;
			}
		} Type editorType;

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
            get { return typeEditor; }
            set
            {
                if (!initializing) throw new InvalidOperationException(Properties.Resources.Component_Already_Initialized);
                typeEditor = value;
            }
        } UITypeEditor typeEditor;

        /// <summary>
        /// If true the text box will be read only and not editor will be used
        /// </summary>
        [Browsable(true)]
        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        } bool readOnly = false;

		/// <summary>
		/// A <see cref="TypeConverter"/> that knows how to convert a <see cref="ValueType"/>.
		/// </summary>
		[Browsable(true)]
		public TypeConverter ConverterInstance
		{
			get { return typeConverter; }
			set
			{
				if (!initializing) throw new InvalidOperationException(Properties.Resources.Component_Already_Initialized);
                typeConverter = value;
			}
        } TypeConverter typeConverter;

		/// <summary>
		/// Specifies if this value must not be null
		/// </summary>
		[Browsable(true)]
		public bool ValueRequired
		{
			get { return valueRequired; }
			set { valueRequired = value; }
		} bool valueRequired;

		#endregion
		
		#region Events

		/// <summary>
		/// Notifies that the user has changed a parameter value.
		/// </summary>
		public event ComponentChangedEventHandler ValueChanged;
		
		private void RaiseValueChanged(object oldValue)
		{
			if (!initializing)
			{
				ComponentChangedEventHandler handler = ValueChanged;
				if (handler != null)
					handler(this, new ComponentChangedEventArgs(this, null, oldValue, Value));
			}
			// Set friendly display value.
			edit.Text = ValueString;
			// Cause repainting.
			this.Invalidate(true);
		}

		/// <summary>
		/// Notifies that the user has tryied to set an invalid value.
		/// </summary>
		public event ComponentChangedEventHandler InvalidValue;

		private void RaiseInvalidValue(object probedValue)
		{
			if (!initializing)
			{
				ComponentChangedEventHandler handler = InvalidValue;
				if (handler != null)
					handler(this, new ComponentChangedEventArgs(this,null,Value,probedValue));
			}
			// Cause repainting.
			this.Invalidate(true);
		}
		
		#endregion
		
		#region Properties

		private bool TypeHaveSetOfValues
		{
			get
			{
                UITypeEditorEditStyle editStyle = UITypeEditorEditStyle.None;
                if (typeEditor != null)
                {
                    editStyle = typeEditor.GetEditStyle(this.services);
                }
				return ( (editStyle == UITypeEditorEditStyle.DropDown) ||
                         (typeConverter != null && 
						  ((typeConverter.GetStandardValuesSupported(this.services) && !(typeConverter is NullableConverter)) ||
						  ((typeConverter is NullableConverter) && 
						  ((NullableConverter)typeConverter).UnderlyingTypeConverter.GetStandardValuesSupported(this.services)))) || 
					     (valueType!=null && valueType.IsEnum));
			}
		}

		private DropDownHolder Holder
		{
			get
			{
				if (dropDownHolder == null)
				{
					dropDownHolder = new DropDownHolder(this);
				}
				return dropDownHolder;
			}
		}

		private ValueEditorListBox ListBox
		{
			get
			{
				if (listBox == null)
				{
					listBox = new ValueEditorListBox(this);
				}
				return listBox;
			}
		}

		private UITypeEditor UITypeEditor
		{
			get { return typeEditor; }
		}
		
		private ValueEditorTextBox Edit
		{
			get
			{
				return edit;
			}
		}

		/// <summary>
		/// Tooltip displayed when the user is edting this control
		/// </summary>
		[Browsable(true)]
		public string ToolTip
		{
			get
			{
				return toolTip.GetToolTip(this.edit);
			}
			set
			{
				toolTip.SetToolTip(this, value);
				toolTip.SetToolTip(this.buttonDropDown, value);
				toolTip.SetToolTip(this.edit, value);
			}
		}

		internal bool IsAssignableValue(object value)
		{
            return (value.GetType().IsCOMObject ||
                valueType == value.GetType() || 
                valueType != null && valueType.IsAssignableFrom(value.GetType()));
		}

        internal bool IsValueConvertible(object value)
        {
            return (typeConverter != null && typeConverter.CanConvertFrom(this.services, value.GetType()));
        }

		internal bool IsValidValue(object value)
		{
            if (typeConverter == null)
            {
                // There is not typeConverter, so we are optimistic and we hope the value is valid
                return true;
            }
            else if (value != null && ReflectionHelper.IsAssignableTo(this.ValueType, value))
            {
                return true;
            }

			return (typeConverter.IsValid(this.services, value));
		}

		internal bool CanAssignValue(object value)
		{
			try
			{
				object reComputedValue = typeConverter.ConvertFrom(this.services, CultureInfo.CurrentCulture, value);
				return reComputedValue != null;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Gets the current value of the parameter.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Value
		{
			get 
			{
				return value;
			}
			set
			{
                if (String.Empty.Equals(value))
                {
                    // Set the value to null if we are entering an empty string
                    value = null;
                }
				object oldValue = this.value;
				if (value == null)
				{
					this.value = null;
					RaiseValueChanged(oldValue);
				}
				else if (valueType == null && initializing)
				{
					this.value = value;
				}
				else if (IsValidValue(value))
				{
                    if ( IsValueConvertible(value) )
                    {
                        // Assign properly typed value.
                        try
                        {
                            object reComputedValue = typeConverter.ConvertFrom(this.services, CultureInfo.CurrentCulture, value);
                            this.value = reComputedValue;
                            RaiseValueChanged(oldValue);
                        }
                        catch (Exception ex)
                        {
                            this.value = oldValue;
                            this.TraceError(ex.ToString());
                            RaiseInvalidValue(value);
                        }
                    }
                    else if (IsAssignableValue(value))
                    {
                        this.value = value;
                        RaiseValueChanged(oldValue);
                    }
                    else
                    {
                        RaiseInvalidValue(value);
                        return;
                    }
				}
				else
				{
					RaiseInvalidValue(value);
					return;
				}
			}
		} object value;

		/// <summary>
		/// Gets the current value of the parameter converted as a string.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ValueString
		{
			get  
			{
				if ( Value == null )
				{
					return Properties.Resources.ValueEditor_ValueNull;
				}
				else if (typeConverter != null)
				{
					return typeConverter.ConvertToString(this.services, Value);
				}
				else
				{
					return Value.ToString();
				}
			}
		}
		
		#endregion

		#region Overrides

        /// <summary>
		/// Resize the value editor
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			int EditPaintBoxSize = Constants.EditPaintBoxSize;
			int SpaceBetweenPaintedValueAndText = Constants.SpaceBetweenPaintedValueAndText;
			if (typeEditor == null || !typeEditor.GetPaintValueSupported())
			{
				EditPaintBoxSize = 0;
				SpaceBetweenPaintedValueAndText = 0;
			}

            int editBoxHeight = (SystemFonts.IconTitleFont.Height > Constants.ImageSize) ? SystemFonts.IconTitleFont.Height : Constants.ImageSize;

			this.edit.Location = new Point(EditPaintBoxSize + SpaceBetweenPaintedValueAndText, 0);
			if (this.buttonDropDown.Enabled)
			{
                this.buttonDropDown.Size = new Size(Constants.ImageSize, editBoxHeight);
                this.buttonDropDown.Location = new Point(this.Size.Width - editBoxHeight - 2, 0);
                this.edit.Size = new Size(this.Size.Width - Constants.ImageSize - Constants.SpaceBetweenTextAndButton - EditPaintBoxSize, editBoxHeight);
			}
			else
			{
                this.edit.Size = new Size(this.Size.Width - EditPaintBoxSize, editBoxHeight);
			}

            this.rectangleForValue = new Rectangle(0, 0, EditPaintBoxSize, editBoxHeight);
		}

		/// <summary>
		/// If the TypeEditor knows how to paint the current value, then it will painted in a little rectangle on the left size of the control
		/// </summary>
		/// <param name="pe"></param>
		protected override void OnPaint(PaintEventArgs pe)
		{
			if (typeEditor!=null && typeEditor.GetPaintValueSupported())
			{
				Rectangle rect = new Rectangle(this.rectangleForValue.Location, this.rectangleForValue.Size);
				rect.Offset(-1, -1);
				rect.Inflate(-2, -2);
				typeEditor.PaintValue(Value, pe.Graphics, rect);
				pe.Graphics.DrawRectangle(new Pen(Color.Black), rect);
			}
			base.OnPaint(pe);
		}

        /// <summary>
        /// See <see cref="Control.OnGotFocus"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.Edit.Focus();
        }

		#endregion
		
		#region Event Handlers
		
		private void EditDropDownBtn(object sender, EventArgs e)
		{
			try
			{
				if (typeEditor != null)
				{
					Value = typeEditor.EditValue(this.services, this.services, Value);
				} 
				else if (TypeHaveSetOfValues)
				{
					IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)this.services;
					editorService.DropDownControl(ListBox);
					editorService.CloseDropDown();
					Value = ListBox.Value;
				}
				else
				{
					throw new InvalidOperationException("EditDropDownBtn");
				}
			}
			catch (Exception ex)
			{
				this.TraceError(ex.ToString());
				RaiseInvalidValue(value);
			}
		}

		#endregion
		
		#region Helper UI clases

		internal class ValueEditorListBox : ListBox, ISupportInitialize 
		{
			private ValueEditor valueEditor;
			private bool initializing;
            private static readonly string NullValue = Properties.Resources.ValueEditor_ValueNullCaption;

			public ValueEditorListBox(ValueEditor valueEditor)
			{
				this.valueEditor = valueEditor;
				this.FormattingEnabled = true;
				this.Format += new ListControlConvertEventHandler(OnFormatItem);
				InitializeComponent();
			}

			void OnFormatItem(object sender, ListControlConvertEventArgs e)
			{
                if (e.ListItem.ToString() == NullValue)
                {
                    e.Value = NullValue;
                }
                else if (valueEditor.typeConverter != null && valueEditor.typeConverter.CanConvertTo(e.DesiredType))
                { // If there's a valid converter that we can use, format the item with that.
					e.Value = valueEditor.typeConverter.ConvertTo(valueEditor.services, 
						CultureInfo.CurrentCulture, e.ListItem, e.DesiredType);					
				}
			}

			protected override void OnEnter(EventArgs e)
			{
				base.OnEnter(e);
				initializing = true;
				try
				{
					RefreshListBoxItems();
					int i = 0;
					foreach (object value in this.Items)
					{
						if (value!=null && value.Equals(this.valueEditor.Value))
						{
							this.SelectedIndex = i;
							break;
						}
						i++;
					}
				}
				finally
				{
					initializing = false;
				}
			}

			protected override void OnClick(EventArgs e)
			{
				base.OnClick(e);
				if (!initializing)
				{
					this.valueEditor.CloseDropDown();
				}
			}

			protected override void OnKeyDown(KeyEventArgs e)
			{
				if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
				{
					this.valueEditor.CloseDropDown();
				}
				base.OnKeyDown(e);
			}

			protected override bool IsInputKey(Keys keyData)
			{
				Keys keys = keyData;
				if (keys == Keys.Return || keys == Keys.Escape )
				{
					return true;
				}
				return base.IsInputKey(keyData);
			}
 

			#region Component Designer generated code

			/// <summary>
			/// Required method for Designer support - do not modify 
			/// the contents of this method with the code editor.
			/// </summary>
			private void InitializeComponent()
			{
				System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValueEditorListBox));
				((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
				this.SuspendLayout();
// 
// ValueEditorListBox
// 
				this.BorderStyle = BorderStyle.None;
				((System.ComponentModel.ISupportInitialize)(this)).EndInit();
				this.ResumeLayout(false);
			}

			#endregion

			#region ISupportInitialize Members

			public void BeginInit()
			{
				initializing = true;
			}

			public object Value
			{
				get 
				{
					if (this.SelectedIndex == -1)
					{
						return null;
					}
					object value = this.Items[this.SelectedIndex];
                    if (value!=null && value.ToString() == NullValue)
                    {
                        return null;
                    }
                    return value;
				}
			}

			public void EndInit()
			{
				RefreshListBoxItems();
				initializing = false;
			}

			private void RefreshListBoxItems()
			{
				if (this.valueEditor.typeConverter == null)
				{
					throw new InvalidOperationException(Properties.Resources.ValueEditor_ListBoxWithOutTypeConverter);
				}
				this.Items.Clear();
				TypeConverter.StandardValuesCollection values = this.valueEditor.typeConverter.GetStandardValues(this.valueEditor.services);
				if ((values == null) && (this.valueEditor.typeConverter is NullableConverter))
				{
					values = ((NullableConverter)this.valueEditor.typeConverter).UnderlyingTypeConverter.GetStandardValues(this.valueEditor.services);
				}
				if (values != null)
				{
					foreach (object value in values)
					{
                        if (value != null)
                        {
                            this.Items.Add(value);
                        }
                        else
                        {  // Add special entry to represent null value
                            this.Items.Add(NullValue);
                        }
					}
				}
			}

			#endregion
		}

		internal sealed class ValueEditorTextBox : TextBox, ISupportInitialize 
		{
			private ValueEditor valueEditor = null;
			private bool changingText = false;

			internal ValueEditorTextBox()
			{
				this.valueEditor = null;
				this.changingText = false;
			}

			public override string Text
			{
				get
				{
					return base.Text;
				}
				set
				{
					if (!this.changingText)
					{
						base.Text = value;
					}
				}
			}

			protected override void OnLeave(EventArgs eventArgs)
			{
				if (this.Modified)
				{
					this.changingText = false;
					valueEditor.Value = this.Text;
				}
				base.OnLeave(eventArgs);
			}

			protected override void OnTextChanged(EventArgs e)
			{
				base.OnTextChanged(e);
				if (changingText )
				{
					return;
				}
                try
                {
                    this.changingText = true;
                    if (this.Modified)
                    {
                        valueEditor.Value = this.Text;
                    }
                }
                finally
                {
                    changingText = false;
                }
			}

			protected override void WndProc(ref Message m)
			{
				if ( m.Msg == NativeMethods.WM_KEYDOWN )
				{
					int keyCode = NativeMethods.Util.LOWORD(m.WParam);
					if (keyCode == NativeMethods.VK_DOWN && valueEditor.TypeHaveSetOfValues)
					{
						this.valueEditor.buttonDropDown.PerformClick();
					}
				}
				base.WndProc(ref m);
			}

			#region ISupportInitialize Members

			public void BeginInit()
			{
			}

            private bool IsTypeEditableInTextBox
            {
                get
                {
                    return (//Type is asignable from string a it is not a COM imported type
                            (this.valueEditor.ValueType.IsAssignableFrom(typeof(String)) &&
                             Attribute.GetCustomAttribute(this.valueEditor.ValueType, typeof(ComImportAttribute), true) == null) ||
                            // or Type Converter can convert it from string
                            (this.valueEditor.ConverterInstance != null && this.valueEditor.ConverterInstance.CanConvertFrom(typeof(string)))
                           );
                }
            }

			public void EndInit()
			{
				this.valueEditor = Parent as ValueEditor;
                if (valueEditor != null)
                {
                    this.Text = valueEditor.ValueString;
					this.BackColor = this.valueEditor.BackColor;
					this.ForeColor = this.valueEditor.ForeColor;
					if (this.valueEditor.ValueType != null)
                    {
                        if (!IsTypeEditableInTextBox || this.valueEditor.ReadOnly ||
							HasExclusiveValues())
                        {
                            //Disable the edit box
                            if (HasExclusiveValues())
                            {
                                this.ReadOnly = true;
                                this.Enabled = true;
                                this.BackColor = SystemColors.Window;
                                this.ForeColor = SystemColors.WindowText;
                                this.valueEditor.BackColor = SystemColors.Window;
                                this.valueEditor.ForeColor = SystemColors.WindowText;
                            }
                            else
                            {
                                this.ReadOnly = true;
							    this.Enabled = false;
							    this.BackColor = SystemColors.Control;
							    this.ForeColor = SystemColors.WindowText;
							    this.valueEditor.BackColor = SystemColors.Control;
                                this.valueEditor.ForeColor = SystemColors.WindowText;
                            }
							this.Cursor = this.valueEditor.Cursor;
							if (this.valueEditor.buttonDropDown.Enabled)
							{
								// Set the TabStop on the button so the editor is accesible trhought the keyboard
								this.valueEditor.buttonDropDown.TabStop = true;
							}
                        }
                    }
                }
			}

			private bool HasExclusiveValues()
			{
				return (this.valueEditor.TypeHaveSetOfValues &&
					(this.valueEditor.ConverterInstance.GetStandardValuesExclusive() ||
						((this.valueEditor.ConverterInstance is NullableConverter) &&
						(((NullableConverter)this.valueEditor.ConverterInstance).UnderlyingTypeConverter.GetStandardValuesExclusive()))
					));
			}

            [DllImport("user32")]
            private static extern int HideCaret(IntPtr hWnd);

            protected override void OnHandleCreated(EventArgs e)
            {
                base.OnHandleCreated(e);
                if (this.ReadOnly)
                {
                    HideCaret(this.Handle);
                }
            }

			#endregion
		}

		private void CloseDropDown()
		{
			if ((dropDownHolder == null) || !dropDownHolder.Visible)
			{
				return;
			}
			//this.Edit.Filter = false;
			dropDownHolder.SetComponent(null, false);
			dropDownHolder.Visible = false;
		}
		
		internal class DropDownHolder : Form
		{
			private Control currentControl = null; // the control that is hosted in the holder
			private ValueEditor valueEditor;              // the owner gridview

			// all the resizing goo...
			//
			private bool resizable = true;  // true if we're showing the resize widget.
			private bool resizing = false; // true if we're in the middle of a resize operation.
			private bool resizeUp = false; // true if the dropdown is above the grid row, which means the resize widget is at the top.
			private Point dragStart = Point.Empty;     // the point at which the drag started to compute the delta
			private Rectangle dragBaseRect = Rectangle.Empty; // the original bounds of our control.
			private int currentMoveType = MoveTypeNone;    // what type of move are we processing? left, bottom, or both?
			private readonly static int ResizeBarSize;    // the thickness of the resize bar
			private readonly static int ResizeBorderSize; // the thickness of the 2-way resize area along the outer edge of the resize bar
			private readonly static int ResizeGripSize;   // the size of the 4-way resize grip at outermost corner of the resize bar
			private readonly static Size MinDropDownSize;  // the minimum size for the control.
			private Bitmap sizeGripGlyph;    // our cached size grip glyph.  Control paint only does right bottom glyphs, so we cache a mirrored one.  See GetSizeGripGlyph
			private const int DropDownHolderBorder = 1;
			private const int MoveTypeNone = 0x0;
			private const int MoveTypeBottom = 0x1;
			private const int MoveTypeLeft = 0x2;
			private const int MoveTypeTop = 0x4;

			static DropDownHolder()
			{
				MinDropDownSize = new Size(SystemInformation.VerticalScrollBarWidth * 4, SystemInformation.HorizontalScrollBarHeight * 4);
				ResizeGripSize = SystemInformation.HorizontalScrollBarHeight;
				ResizeBarSize = ResizeGripSize + 1;
				ResizeBorderSize = ResizeBarSize / 2;
			}

			internal DropDownHolder(ValueEditor valueEditor)
				: base()
			{
                this.valueEditor = valueEditor;
                this.ShowInTaskbar = false;
				this.ControlBox = false;
				this.MinimizeBox = false;
				this.MaximizeBox = false;
				this.Text = "";
				this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
				Visible = false;
                Font = this.valueEditor.Font;
                this.CreateControl();
			}

			public virtual void FocusComponent()
			{
				Debug.WriteLine("DropDownHolder:FocusComponent()");
				if (currentControl != null && Visible)
				{
					currentControl.Focus();
				}
			}

			protected override void OnGotFocus(EventArgs e)
			{
                base.OnGotFocus(e);
				currentControl.Focus();
			}

			protected override void OnKeyDown(KeyEventArgs e)
			{
				base.OnKeyDown(e);
				if (e.KeyCode == Keys.Escape)
					this.valueEditor.CloseDropDown();
			}

			protected override CreateParams CreateParams
			{
				get
				{
					CreateParams createParams = base.CreateParams;
					createParams.ExStyle |= NativeMethods.WS_EX_TOOLWINDOW;
					createParams.Style |= NativeMethods.WS_POPUP | NativeMethods.WS_BORDER;
					if (OSFeature.IsPresent(SystemParameter.DropShadow))
					{
						createParams.ClassStyle |= NativeMethods.CS_DROPSHADOW;
					}
					if (valueEditor != null)
					{
						createParams.Parent = valueEditor.Parent.Handle;
					}
					return createParams;
				}
			}

			/// <devdoc>
			/// This gets set to true if there isn't enough space below the currently selected
			/// row for the drop down, so it appears above the row.  In this case, we make the resize
			/// grip appear at the top left.
			/// </devdoc>
			public bool ResizeUp
			{
				get
				{
					return resizeUp;
				}
				set
				{
					if (resizeUp != value)
					{

						// clear the glyph so we regenerate it.
						//
						sizeGripGlyph = null;
						resizeUp = value;

						if (resizable)
						{
							this.DockPadding.Bottom = 0;
							this.DockPadding.Top = 0;
							if (value)
							{
								this.DockPadding.Top = ResizeBarSize;
							}
							else
							{
								this.DockPadding.Bottom = ResizeBarSize;
							}
						}
					}
				}
			}

			public void DoModalLoop()
			{
				// Push a modal loop.  This kind of stinks, but I think it is a
				// better user model than returning from DropDownControl immediately.
				//  
				while (this.Visible)
				{
					Application.DoEvents();
					NativeMethods.MsgWaitForMultipleObjects(0, IntPtr.Zero, true, 250, NativeMethods.QS_ALLINPUT);
				}
			}

			public virtual Control Component
			{
				get
				{
					return currentControl;
				}
			}

			/// <devdoc>
			/// Get a glyph for sizing the lower left hand grip.  The code in ControlPaint only does lower-right glyphs
			/// so we do some GDI+ magic to take that glyph and mirror it.  That way we can still share the code (in case it changes for theming, etc),
			/// not have any special cases, and possibly solve world hunger.  
			/// </devdoc>
			private Bitmap GetSizeGripGlyph(Graphics graphics)
			{
				if (this.sizeGripGlyph != null)
				{
					return sizeGripGlyph;
				}

				// create our drawing surface based on the current graphics context.
				//
				sizeGripGlyph = new Bitmap(ResizeGripSize, ResizeGripSize, graphics);

				using (Graphics glyphGraphics = Graphics.FromImage(sizeGripGlyph))
				{
					// mirror the image around the x-axis to get a gripper handle that works
					// for the lower left.
					System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();

					// basically, mirroring is just scaling by -1 on the X-axis.  So any point that's like (10, 10) goes to (-10, 10). 
					// that mirrors it, but also moves everything to the negative axis, so we just bump the whole thing over by it's width.
					// 
					// the +1 is because things at (0,0) stay at (0,0) since [0 * -1 = 0] so we want to get them over to the other side too.
					//
					// resizeUp causes the image to also be mirrored vertically so the grip can be used as a top-left grip instead of bottom-left.
					//
					m.Translate(ResizeGripSize + 1, (resizeUp ? ResizeGripSize + 1 : 0));
					m.Scale(-1, (resizeUp ? -1 : 1));
					glyphGraphics.Transform = m;
					ControlPaint.DrawSizeGrip(glyphGraphics, BackColor, 0, 0, ResizeGripSize, ResizeGripSize);
					glyphGraphics.ResetTransform();

				}
				sizeGripGlyph.MakeTransparent(BackColor);
				return sizeGripGlyph;
			}

			public virtual bool GetUsed()
			{
				return (currentControl != null);
			}

			/// <devdoc>
			///    General purpose method, based on Control.Contains()...
			///
			///    Determines whether a given window (specified using native window handle)
			///    is a descendant of this control. This catches both contained descendants
			///    and 'owned' windows such as modal dialogs. Using window handles rather
			///    than Control objects allows it to catch un-managed windows as well.
			/// </devdoc>
			private bool OwnsWindow(IntPtr hWnd)
			{
				while (hWnd != IntPtr.Zero)
				{
					hWnd = NativeMethods.GetWindowLong(new HandleRef(null, hWnd), NativeMethods.GWL_HWNDPARENT);
					if (hWnd == IntPtr.Zero)
					{
						return false;
					}
					if (hWnd == this.Handle)
					{
						return true;
					}
				}
				return false;
			}

			private void OnCurrentControlResize(object sender, EventArgs e)
			{
				if (currentControl != null && !resizing)
				{
					int oldWidth = this.Width;
					Size newSize = new Size(2 * DropDownHolderBorder + currentControl.Width, 2 * DropDownHolderBorder + currentControl.Height);
					if (resizable)
					{
						newSize.Height += ResizeBarSize;
					}
					try
					{
						resizing = true;
						this.SuspendLayout();
						this.Size = newSize;
					}
					finally
					{
						resizing = false;
						this.ResumeLayout(false);
					}
					this.Left -= (this.Width - oldWidth);
				}
			}

			protected override void OnLayout(LayoutEventArgs levent)
			{
				try
				{
					resizing = true;
					base.OnLayout(levent);
				}
				finally
				{
					resizing = false;
				}
			}

			/// <devdoc>
			/// Just figure out what kind of sizing we would do at a given drag location.
			/// </devdoc>
			private int MoveTypeFromPoint(int pointX, int pointY)
			{
				Rectangle bGripRect = new Rectangle(0, Height - ResizeGripSize, ResizeGripSize, ResizeGripSize);
				Rectangle tGripRect = new Rectangle(0, 0, ResizeGripSize, ResizeGripSize);

				if (!resizeUp && bGripRect.Contains(pointX, pointY))
				{
					return MoveTypeLeft | MoveTypeBottom;
				}
				else if (resizeUp && tGripRect.Contains(pointX, pointY))
				{
					return MoveTypeLeft | MoveTypeTop;
				}
				else if (!resizeUp && Math.Abs(Height - pointY) < ResizeBorderSize)
				{
					return MoveTypeBottom;
				}
				else if (resizeUp && Math.Abs(pointY) < ResizeBorderSize)
				{
					return MoveTypeTop;
				}
				return MoveTypeNone;
			}

			/// <devdoc>
			/// Decide if we're going to be sizing at the given point, and if so, Capture and safe our current state.
			/// </devdoc>Mo
			protected override void OnMouseDown(MouseEventArgs mouseEventArgs)
			{
				if (mouseEventArgs.Button == MouseButtons.Left)
				{
					this.currentMoveType = MoveTypeFromPoint(mouseEventArgs.X, mouseEventArgs.Y);
					if (this.currentMoveType != MoveTypeNone)
					{
						dragStart = PointToScreen(new Point(mouseEventArgs.X, mouseEventArgs.Y));
						dragBaseRect = Bounds;
						Capture = true;
					}
					else
					{
						valueEditor.CloseDropDown();
					}
				}
				base.OnMouseDown(mouseEventArgs);
			}

			/// <devdoc>
			/// Either set the cursor or do a move, depending on what our currentMoveType is/
			/// </devdoc>
			protected override void OnMouseMove(MouseEventArgs e)
			{
				// not moving so just set the cursor.
				//
				if (this.currentMoveType == MoveTypeNone)
				{
					int cursorMoveType = MoveTypeFromPoint(e.X, e.Y);
					switch (cursorMoveType)
					{
						case (MoveTypeLeft | MoveTypeBottom):
							Cursor = Cursors.SizeNESW;
							break;
						case MoveTypeBottom:
						case MoveTypeTop:
							Cursor = Cursors.SizeNS;
							break;
						case MoveTypeTop | MoveTypeLeft:
							Cursor = Cursors.SizeNWSE;
							break;
						default:
							Cursor = null;
							break;
					}
				}
				else
				{
					Point dragPoint = PointToScreen(new Point(e.X, e.Y));
					Rectangle newBounds = Bounds;

					// we're in a move operation, so do the resize.
					//
					if ((currentMoveType & MoveTypeBottom) == MoveTypeBottom)
					{
						newBounds.Height = Math.Max(MinDropDownSize.Height, dragBaseRect.Height + (dragPoint.Y - dragStart.Y));
					}

					// for left and top moves, we actually have to resize and move the form simultaneously.
					// do to that, we compute the xdelta, and apply that to the base rectangle if it's not going to
					// make the form smaller than the minimum.
					//
					if ((currentMoveType & MoveTypeTop) == MoveTypeTop)
					{
						int delta = dragPoint.Y - dragStart.Y;

						if ((dragBaseRect.Height - delta) > MinDropDownSize.Height)
						{
							newBounds.Y = dragBaseRect.Top + delta;
							newBounds.Height = dragBaseRect.Height - delta;
						}
					}

					if ((currentMoveType & MoveTypeLeft) == MoveTypeLeft)
					{
						int delta = dragPoint.X - dragStart.X;

						if ((dragBaseRect.Width - delta) > MinDropDownSize.Width)
						{
							newBounds.X = dragBaseRect.Left + delta;
							newBounds.Width = dragBaseRect.Width - delta;
						}
					}

					if (newBounds != Bounds)
					{
						try
						{
							resizing = true;
							this.Bounds = newBounds;
						}
						finally
						{
							resizing = false;
						}
					}

					// Redraw!
					//
					Invalidate();
				}
				base.OnMouseMove(e);
			}

			protected override void OnMouseLeave(EventArgs e)
			{
				// just clear the cursor back to the default.
				//
				Cursor = null;
				base.OnMouseLeave(e);
			}

			protected override void OnMouseUp(MouseEventArgs e)
			{
				base.OnMouseUp(e);
				if (e.Button == MouseButtons.Left)
				{
					// reset the world.
					//
					this.currentMoveType = MoveTypeNone;
					this.dragStart = Point.Empty;
					this.dragBaseRect = Rectangle.Empty;
					this.Capture = false;
				}
			}

			/// <devdoc>
			/// Just paint and draw our glyph.
			/// </devdoc>
			protected override void OnPaint(PaintEventArgs paintEventArgs)
			{
				base.OnPaint(paintEventArgs);
				if (resizable)
				{
					// Draw the grip
					Rectangle lRect = new Rectangle(0, resizeUp ? 0 : Height - ResizeGripSize, ResizeGripSize, ResizeGripSize);
					paintEventArgs.Graphics.DrawImage(GetSizeGripGlyph(paintEventArgs.Graphics), lRect);
					// Draw the divider
					int y = resizeUp ? (ResizeBarSize - 1) : (Height - ResizeBarSize);
					Pen pen = new Pen(SystemColors.ControlDark, 1);
					pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
					paintEventArgs.Graphics.DrawLine(pen, 0, y, Width, y);
					pen.Dispose();
				}
			}

			protected override bool ProcessDialogKey(Keys keyData)
			{
				if ((keyData & (Keys.Shift | Keys.Control | Keys.Alt)) == 0)
				{
					switch (keyData & Keys.KeyCode)
					{
						case Keys.Escape:
							//gridView.OnEscape(this);
							return true;
						case Keys.F4:
							//gridView.F4Selection(true);
							return true;
						case Keys.Return:
							// make sure the return gets forwarded to the control that
							// is being displayed
							//if (gridView.UnfocusSelection())
							//{
							//	gridView.SelectedGridEntry.OnValueReturnKey();
							//}
							return true;
					}
				}
				return base.ProcessDialogKey(keyData);
			}

			public void SetComponent(Control ctl, bool resizable)
			{
				this.BackColor = System.Drawing.SystemColors.Window;
				this.resizable = resizable;
				// clear any existing control we have
				//
				if (currentControl != null)
				{
					currentControl.Resize -= new EventHandler(this.OnCurrentControlResize);
					Controls.Remove(currentControl);
					currentControl = null;
				}
				// now set up the new control, top to bottom
				//
				if (ctl != null)
				{
					currentControl = ctl;
					Debug.WriteLine("DropDownHolder:SetComponent(" + (ctl.GetType().Name) + ")");
					this.DockPadding.All = 0;
					// first handle the control.  If it's a listbox, make sure it's got some height
					// to it.
					//
					if (currentControl is ValueEditorListBox)
					{
						ListBox lb = (ListBox)currentControl;
						if (lb.Items.Count == 0)
						{
							lb.Height = Math.Max(lb.Height, lb.ItemHeight);
						}
					}
					Size sz = new Size(2 * DropDownHolderBorder + ctl.Width, 2 * DropDownHolderBorder + ctl.Height);
					// finally, if we're resizable, add the space for the widget.
					//
					if (resizable)
					{
						sz.Height += ResizeBarSize;
						// we use dockpadding to save space to draw the widget.
						//
						if (resizeUp)
						{
							this.DockPadding.Top = ResizeBarSize;
						}
						else
						{
							this.DockPadding.Bottom = ResizeBarSize;
						}
					}
					// set the size stuff.
					//
					try
					{
						SuspendLayout();
						Size = sz;
						ctl.Dock = DockStyle.Fill;
						ctl.Visible = true;
						Controls.Add(ctl);
					}
					finally
					{
						ResumeLayout(true);
					}
					// hook the resize event.
					//
					currentControl.Resize += new EventHandler(this.OnCurrentControlResize);
				}
				Enabled = currentControl != null;
			}

			protected override void WndProc(ref Message m)
			{
				if (m.Msg == NativeMethods.WM_ACTIVATE)
				{
					//SetState(STATE_MODAL, true);
					Debug.WriteLine("DropDownHolder:WM_ACTIVATE():WParam=0x" + m.WParam.ToInt32().ToString("X") + ",LParam=0x" + m.LParam.ToInt32().ToString("X"));
					IntPtr activatedWindow = (IntPtr)m.LParam;
					if (Visible && NativeMethods.Util.LOWORD(m.WParam) == NativeMethods.WA_INACTIVE && !this.OwnsWindow(activatedWindow))
					{
						valueEditor.CloseDropDown();
						return;
					}
					// prevent the IMsoComponentManager active code from getting fired.
					//Active = ((int)m.WParam & 0x0000FFFF) != NativeMethods.WA_INACTIVE;
					//return;
				}
				else if (m.Msg == NativeMethods.WM_CLOSE)
				{
					// don't let an ALT-F4 get you down
					//
					if (Visible)
					{
						valueEditor.CloseDropDown();
					}
					return;
				}
				else if (m.Msg == NativeMethods.WM_KEYDOWN)
				{
					int keyCode = NativeMethods.Util.LOWORD(m.WParam);
					if (keyCode == NativeMethods.VK_ESCAPE && Visible )
					{
						this.valueEditor.CloseDropDown();
						return;
					}
				}

				base.WndProc(ref m);
			}
		}
		
		#endregion

		#region Native Methods

		internal class NativeMethods
		{
			#region Windows Managment

			public const int QS_ALLINPUT = 255;
			[DllImport("User32.dll")]
			public extern static int MsgWaitForMultipleObjects(int nCount, IntPtr pHandles, bool fWaitAll, int dwMilliseconds, int dwWakeMask);
			public const int GWL_HWNDPARENT = (-8);
			public const int GWL_EXSTYLE = (-20);
			public const int DLGC_WANTARROWS = 0x0001;
			public const int DLGC_WANTCHARS = 0x0080;
			public const int DLGC_WANTTAB = 0x0002; // Control wants tab keys
			public const int DLGC_WANTALLKEYS = 0x0004;  // Control wants all keys
			[DllImport("User32.dll")]
			public extern static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, HandleRef dwNewLong);
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr GetWindowLong(HandleRef hWnd, int nIndex);
			public const int SW_SHOWNA = 8;
			[DllImport("User32.dll")]
			public extern static bool ShowWindow(HandleRef hWnd, int nCmdShow);
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			public static extern bool PostMessage(HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);
			[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			public static extern IntPtr GetFocus();
			[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			public static extern IntPtr SetFocus(HandleRef hWnd);

			#endregion

			#region Windows Styles

			public const int WS_EX_TOOLWINDOW = 128;
			public const int WS_POPUP = -2147483648;
			public const int WS_BORDER = 0x00800000;
			public const int CS_DROPSHADOW = 0x00020000;

			#endregion

			#region Windows Messages

			public const int WM_ACTIVATE = 0x0006;
			public const int WA_INACTIVE = 0x0;
			public const int WM_CLOSE = 0x0010;
			public const int WM_CHAR = 0x0102;
			public const int WM_MOUSEMOVE = 0x0200;
			public const int WM_PASTE = 0x0302;
			public const int WM_GETDLGCODE = 0x0087;
			public const int WM_NOTIFY = 0x004E;
			public const int WM_STYLECHANGED = 0x007D;
			public const int WM_LBUTTONDOWN = 0x0201;
			public const int WM_LBUTTONUP = 0x0202;
			public const int WM_LBUTTONDBLCLK = 0x0203;
			public const int WM_RBUTTONDOWN = 0x0204;
			public const int WM_RBUTTONUP = 0x0205;
			public const int WM_RBUTTONDBLCLK = 0x0206;
			public const int WM_MBUTTONDOWN = 0x0207;
			public const int WM_MBUTTONUP = 0x0208;
			public const int WM_MBUTTONDBLCLK = 0x0209;
			public const int WM_NCLBUTTONDOWN = 0x00A1;
			public const int WM_NCRBUTTONDOWN = 0x00A4;
			public const int WM_NCMBUTTONDOWN = 0x00A7;
			public const int WM_MOUSEACTIVATE = 0x0021;
			public const int WM_KEYFIRST = 0x0100;
			public const int WM_KEYDOWN = 0x0100;

			#endregion

			#region Key Codes

			public const int VK_ESCAPE = 0x1B;
			public const int VK_SPACE = 0x20;
			public const int VK_PRIOR = 0x21;
			public const int VK_NEXT = 0x22;
			public const int VK_END = 0x23;
			public const int VK_HOME = 0x24;
			public const int VK_LEFT = 0x25;
			public const int VK_UP = 0x26;
			public const int VK_RIGHT = 0x27;
			public const int VK_DOWN = 0x28;
			public const int VK_SELECT = 0x29;
			public const int VK_PRINT = 0x2A;
			public const int VK_EXECUTE = 0x2B;
			public const int VK_SNAPSHOT = 0x2C;
			public const int VK_INSERT = 0x2D;
			public const int VK_DELETE = 0x2E;
			public const int VK_HELP = 0x2F;

			#endregion

			#region Windows Notification

			[StructLayout(LayoutKind.Sequential)]
			public struct NMHDR
			{
				// Fields
				public int code;
				public IntPtr hwndFrom;
				public IntPtr idFrom;
			}
			const int TTN_FIRST = (0 - 520);       // tooltips
			const int TTN_LAST = (0 - 549);
			public const int TTN_SHOW = (TTN_FIRST - 1);

			#endregion

			#region Process

			[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			public static extern int GetWindowThreadProcessId(HandleRef hWnd, out int lpdwProcessId);
			[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			public static extern int GetCurrentThreadId();

			#endregion

			#region Windows Hooks

			public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
			public const int HC_ACTION = 0;
			public const int WH_MOUSE = 7;
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr SetWindowsHookEx(int hookid, NativeMethods.HookProc pfnhook, HandleRef hinst, int threadid);
			[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			public static extern IntPtr CallNextHookEx(HandleRef hhook, int code, IntPtr wparam, IntPtr lparam);
			[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			public static extern bool UnhookWindowsHookEx(HandleRef hhook);
			public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);
			[StructLayout(LayoutKind.Sequential)]
			public class MOUSEHOOKSTRUCT
			{
				// Fields
				public int dwExtraInfo = 0;
				public IntPtr hWnd = IntPtr.Zero;
				public int pointX = 0;
				public int pointY = 0;
				public int wHitTestCode = 0;
			}

			#endregion

			#region VC Macros

			public class Util
			{
				public static int LOWORD(int n)
				{
					return (n & 0xFFFF);
				}
				public static int LOWORD(IntPtr n)
				{
					return LOWORD(((int)((long)n)));
				}
			}

			#endregion
		}
		#endregion
		
		#region Services

		internal sealed class Services : ITypeDescriptorContext, IWindowsFormsEditorService
		{
			private ValueEditor valueEditor;
			private PropertyDescriptor propertyDescriptor;

			public Services(ValueEditor valueEditor)
			{
				this.valueEditor = valueEditor;
			}

			#region IServiceProvider Members

			object IServiceProvider.GetService(Type serviceType)
			{
				// Services we offer to editors.
				if (serviceType == typeof(IWindowsFormsEditorService))
				{
					return (IWindowsFormsEditorService)this;
				}
				else if (serviceType == typeof(ITypeDescriptorContext))
				{
					return (ITypeDescriptorContext)this;
				}
				else if (serviceType == typeof(IServiceProvider))
				{
					return (IServiceProvider)this;
				}
				return valueEditor.GetService(serviceType);
			}

			#endregion

			#region ITypeDescriptorContext Members

			IContainer ITypeDescriptorContext.Container
			{
				get { return valueEditor.GetService(typeof(IContainer)) as IContainer; }
			}

			object ITypeDescriptorContext.Instance
			{
				get { return valueEditor; }
			}

			void ITypeDescriptorContext.OnComponentChanged()
			{
				throw new NotImplementedException();
			}

			bool ITypeDescriptorContext.OnComponentChanging()
			{
				throw new NotImplementedException();
			}

			PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
			{
				get
				{
					if (propertyDescriptor == null)
					{
                        propertyDescriptor = TypeDescriptor.CreateProperty(
                            this.GetType(), "Value", this.valueEditor.ValueType);
					}
					return propertyDescriptor;
				}
			}

			#endregion

			#region IWindowsFormsEditorService Members

			void IWindowsFormsEditorService.CloseDropDown()
			{
				if (valueEditor.dropDownHolder != null && valueEditor.dropDownHolder.Visible)
				{
					// disable the ddh so it wont' steal the focus back
					//
					valueEditor.Holder.SetComponent(null, false);
					valueEditor.Holder.Visible = false;
					// when we disable the dropdown holder, focus will be lost,
					// so put it onto one of our children first.
					if (valueEditor.buttonDropDown.Visible)
					{
						valueEditor.buttonDropDown.Focus();
					}
					else if (valueEditor.Edit.Visible)
					{
						valueEditor.Edit.Focus();
					}
					else
					{
						valueEditor.Focus();
					}
				}
			}

			/// <summary>
			///      Displays the provided control in a drop down.  When possible, the
			///      current dimensions of the control will be respected.  If this is not possible
			///      for the current screen layout the control may be resized, so it should
			///      be implemented using appropriate docking and anchoring so it will resize
			///      nicely.  If the user performs an action that would cause the drop down
			///      to prematurely disappear the control will be hidden.
			/// </summary>
			/// <param name="ctl"></param>
			void IWindowsFormsEditorService.DropDownControl(Control ctl)
			{
				valueEditor.Holder.Visible = false;
				bool reSizable = true;
				if (valueEditor.typeEditor != null && !valueEditor.typeEditor.IsDropDownResizable)
				{
					reSizable = false;
				}
				valueEditor.Holder.SetComponent(ctl, reSizable);
				Size size = valueEditor.Holder.Size;
				Point loc = valueEditor.PointToScreen(new Point(0, 0));

				#region Calculate Rezize Up or Down
				//dropDownHolder.ResizeUp = false;
				//Rectangle rect = new Rectangle(this.Location, this.Size);
				Rectangle rect = valueEditor.DisplayRectangle;
				Rectangle rectScreen = Screen.FromControl(valueEditor).WorkingArea;
				size.Width = Math.Max(rect.Width + 1, size.Width);
				// Not needed... CYMAXDDLHEIGHT used to be 200, but why limit it???
				//size.Height = Math.Min(size.Height,CYMAXDDLHEIGHT);
				loc.X = Math.Min(rectScreen.X + rectScreen.Width - size.Width,
									Math.Max(rectScreen.X, loc.X + rect.X + rect.Width - size.Width));
				loc.Y += rect.Y;
				if (rectScreen.Y + rectScreen.Height < (size.Height + loc.Y + valueEditor.Edit.Height))
				{
					loc.Y -= size.Height;
					valueEditor.dropDownHolder.ResizeUp = true;
				}
				else
				{
					loc.Y += rect.Height + 1;
					valueEditor.dropDownHolder.ResizeUp = false;
				}
				#endregion


				IntPtr previousParent = NativeMethods.SetWindowLong(
					new HandleRef(valueEditor.Holder, valueEditor.Holder.Handle),
					NativeMethods.GWL_HWNDPARENT, new HandleRef(this, valueEditor.Handle));
                if (previousParent == IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
				valueEditor.Holder.SetBounds(loc.X, loc.Y, size.Width, size.Height);
				NativeMethods.ShowWindow(
					new HandleRef(valueEditor.Holder, valueEditor.Holder.Handle), 
					NativeMethods.SW_SHOWNA);
				valueEditor.Holder.Visible = true;
                if(!valueEditor.Holder.CanFocus)
					this.valueEditor.TraceInformation("Warning: Holder cannot gain focus");
                valueEditor.Focus();
				valueEditor.Holder.FocusComponent();
                valueEditor.Holder.DoModalLoop();
                valueEditor.DisposeHolder();
			}

			DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
			{
				
				IUIService uiService = (IUIService)this.valueEditor.Site.GetService(typeof(IUIService));
				IntPtr parentWnd = NativeMethods.GetFocus();
				try
				{
					return uiService.ShowDialog(dialog);
				}
				catch (Exception)
				{
					return DialogResult.Abort;
				}
				finally
				{
					dialog.Dispose();
					dialog = null;
					if (parentWnd != IntPtr.Zero)
					{
						NativeMethods.SetFocus(new HandleRef(null, parentWnd));
					}
				}
			}

			#endregion
		}

		#endregion

		#region ISupportInitialize Members

		private void InitializeButton()
		{
			this.buttonDropDown.BackColor = System.Drawing.SystemColors.Control;
			this.buttonDropDown.ForeColor = System.Drawing.SystemColors.Window;
			UITypeEditorEditStyle editStyle = UITypeEditorEditStyle.None;
			if (typeEditor != null)
			{
				editStyle = typeEditor.GetEditStyle(this.services);
			}
			if ( TypeHaveSetOfValues )
			{
				this.buttonDropDown.BackgroundImage = Image.FromStream(
						typeof(Form).Assembly.GetManifestResourceStream("System.Windows.Forms.down.bmp"));
				this.buttonDropDown.Enabled = true;
				this.buttonDropDown.Visible = true;
			}
			else if (editStyle == UITypeEditorEditStyle.Modal)
			{
				this.buttonDropDown.BackgroundImage = Image.FromStream(
						typeof(Form).Assembly.GetManifestResourceStream("System.Windows.Forms.dotdotdot.png"));
				this.buttonDropDown.Enabled = true;
				this.buttonDropDown.Visible = true;
			}
			else
			{
				this.buttonDropDown.BackgroundImage = null;
				this.buttonDropDown.Enabled = false;
				this.buttonDropDown.Visible = false;
			}
            if (ConverterInstance != null && TypeHaveSetOfValues && !this.ReadOnly && 
				(ConverterInstance.GetStandardValuesExclusive() ||
				((ConverterInstance is NullableConverter) && 
				((NullableConverter)ConverterInstance).UnderlyingTypeConverter.GetStandardValuesExclusive())))
            {
                this.Edit.Click += new System.EventHandler(this.EditDropDownBtn);
            }
			if (!TypeHaveSetOfValues && editStyle == UITypeEditorEditStyle.None && this.ReadOnly)
			{
				// NOTE: showing the value editor even if the argument is ReadOnly is *by design*.
				// The reasoning is that I may want to disable users from *typing* in the textbox, 
				// and force them to use the editor I assigned to the field. If I want the field 
				// to be completely reaonly, there's no point in assigning an editor to it, so 
				// in that case there will only be a disabled textbox.
				this.buttonDropDown.Enabled = false;
				this.buttonDropDown.Visible = false;
			}
		}
		
		/// <summary>
		/// Starts Initialization
		/// </summary>
		public void BeginInit()
		{
			initializing = true;
            ((ISupportInitialize)this.edit).BeginInit();
		}

		/// <summary>
		/// Set up the ValueEditor for editing
		/// </summary>
		public void EndInit()
		{
            if (typeEditor == null)
            {
				// NOTE: showing the value editor even if the argument is ReadOnly is *by design*.
				// The reasoning is that I may want to disable users from *typing* in the textbox, 
				// and force them to use the editor I assigned to the field. If I want the field 
				// to be completely reaonly, there's no point in assigning an editor to it, so 
				// in that case there will only be a disabled textbox.
                if (editorType != null)
                {
                    this.typeEditor = (UITypeEditor)Activator.CreateInstance(editorType);
                }
                else if (valueType != null)
                {
                    this.typeEditor = (UITypeEditor)TypeDescriptor.GetEditor(valueType, typeof(UITypeEditor));
                    if (this.typeEditor == null && CanUseDefaultValueEditor())
                    {
                        this.typeEditor = new DefaultValueEditor();
                    }
                }
            }
			InitializeButton();
            //if (this.ValueRequired)
            //{
            //    this.BackColor = SystemColors.Info;
            //    this.ForeColor = SystemColors.HotTrack;
            //}
            //else
            //{
            //    this.BackColor = SystemColors.Window;
            //    this.ForeColor = SystemColors.WindowText;
            //}
				this.BackColor = SystemColors.Window;
				this.ForeColor = SystemColors.WindowText;
            ((ISupportInitialize)this.edit).EndInit();
            if (this.valueType != null)
			{
				//Do we need the propertyDescriptor?
				//this.propertyDescriptor = TypeDescriptor.GetProperties(GetType())["Value"];
				if (this.Parent != null && this.Parent.Site!=null )
				{
					//We have a parent, so let's site ourselfs
					IServiceProvider serviceProvider = (IServiceProvider)this.Parent.Site.GetService(typeof(IServiceProvider));
					if (serviceProvider != null)
					{
						this.Site = new ComponentModel.Site(serviceProvider, this, this.Name);
					}
				}
				OnSizeChanged(new EventArgs());
				initializing = false;
			}
		}

		#endregion

        internal void DisposeHolder()
        {
            if (dropDownHolder != null)
            {
                dropDownHolder.Dispose();
                dropDownHolder = null;
            }
        }
    }
}
