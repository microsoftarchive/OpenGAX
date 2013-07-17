using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

using EnvDTE;

namespace Mvp.Xml.Design.CustomTools.XGen
{
	internal class ClassPicker : System.Windows.Forms.Form
	{
		#region Designer Stuff
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnAccept;
		private Mvp.Xml.Design.ListViewEx lstClasses;
		private System.Windows.Forms.TextBox txtEditor;
		private System.Windows.Forms.ColumnHeader colClass;
		private System.Windows.Forms.ColumnHeader colTargetNs;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ClassPicker()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnAccept = new System.Windows.Forms.Button();
			this.lstClasses = new Mvp.Xml.Design.ListViewEx();
			this.colClass = new System.Windows.Forms.ColumnHeader();
			this.colTargetNs = new System.Windows.Forms.ColumnHeader();
			this.txtEditor = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(624, 48);
			this.label1.TabIndex = 1;
			this.label1.Text = "Select the classes you will use as the root of a deserialization process. A custo" +
				"m XmlSerializer and supporting classes will be generated for each of the selecte" +
				"d ones:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(395, 280);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Visible = false;
			// 
			// btnAccept
			// 
			this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnAccept.Location = new System.Drawing.Point(557, 280);
			this.btnAccept.Name = "btnAccept";
			this.btnAccept.Size = new System.Drawing.Size(75, 23);
			this.btnAccept.TabIndex = 3;
			this.btnAccept.Text = "&OK";
			// 
			// lstClasses
			// 
			this.lstClasses.AllowColumnReorder = true;
			this.lstClasses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstClasses.CheckBoxes = true;
			this.lstClasses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colClass,
            this.colTargetNs});
			this.lstClasses.DoubleClickActivation = false;
			this.lstClasses.FullRowSelect = true;
			this.lstClasses.Location = new System.Drawing.Point(8, 56);
			this.lstClasses.Name = "lstClasses";
			this.lstClasses.Size = new System.Drawing.Size(624, 216);
			this.lstClasses.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lstClasses.TabIndex = 0;
			this.lstClasses.UseCompatibleStateImageBehavior = false;
			this.lstClasses.View = System.Windows.Forms.View.Details;
			this.lstClasses.SubItemClicked += new Mvp.Xml.Design.SubItemEventHandler(this.lstClasses_SubItemClicked);
			// 
			// colClass
			// 
			this.colClass.Text = "Class";
			this.colClass.Width = 344;
			// 
			// colTargetNs
			// 
			this.colTargetNs.Text = "Target Namespace";
			this.colTargetNs.Width = 273;
			// 
			// txtEditor
			// 
			this.txtEditor.Location = new System.Drawing.Point(12, 280);
			this.txtEditor.Name = "txtEditor";
			this.txtEditor.Size = new System.Drawing.Size(100, 20);
			this.txtEditor.TabIndex = 4;
			this.txtEditor.Visible = false;
			// 
			// ClassPicker
			// 
			this.AcceptButton = this.btnAccept;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(640, 309);
			this.Controls.Add(this.txtEditor);
			this.Controls.Add(this.lstClasses);
			this.Controls.Add(this.btnAccept);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.label1);
			this.Name = "ClassPicker";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " Class Picker";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#endregion Designer Stuff

		string defaultNs;

		public ClassPicker(CodeElements elements, string defaultNs, SelectionCollection selections)
		{
			InitializeComponent();
			// Add all classes.
			this.defaultNs = defaultNs;
			IterateElements(elements);
			if (lstClasses.Items.Count == 1)
			{
				lstClasses.Items[0].Checked = true;
			}

			if (selections != null)
			{
				foreach (Selection sel in selections)
				{
					foreach (ListViewItem item in lstClasses.Items)
					{
						if (item.Text == sel.ClassName)
						{
							item.Checked = true;
						}
					}
				}
			}
		}

		private void IterateElements(CodeElements elements)
		{
			foreach (CodeElement element in elements)
			{
				if (element is CodeNamespace)
				{
					IterateElements(((CodeNamespace)element).Members);
				}
				else if (element is CodeClass)
				{
					lstClasses.Items.Add(new ListViewItem(
						new string[] { ((CodeClass)element).FullName, this.defaultNs }));
				}
			}
		}

		public SelectionCollection Selections
		{
			get 
			{
				SelectionCollection col = new SelectionCollection();
				foreach (ListViewItem item in lstClasses.Items)
				{
					if (item.Checked)
					{
						col.Add(new Selection(item.Text, item.SubItems[1].Text));
					}
				}
				return col;
			}
		}

		private void lstClasses_SubItemClicked(object sender, Mvp.Xml.Design.SubItemEventArgs e)
		{
			if (e.SubItem != 0)
			{
				lstClasses.StartEditing(txtEditor, e.Item, e.SubItem);
			}
		}
	}

	internal class SelectionCollection : CollectionBase
	{
		public static SelectionCollection FromString(string serializedData)
		{
			string[] items = serializedData.Split('|');
			SelectionCollection col = new SelectionCollection();
			foreach (string item in items)
			{
				col.Add(Selection.FromString(item));
			}
			return col;
		}

		public int Add(Selection selection)
		{
			return base.InnerList.Add(selection);
		}

		public void Remove(Selection selection)
		{
			base.InnerList.Remove(selection);
		}

		public Selection this[int idx]
		{
			get { return (Selection)base.InnerList[idx]; }
			set { base.InnerList[idx] = value; }
		}

		public override string ToString()
		{
			string[] items = new string[base.Count];
			for (int i = 0; i < items.Length; i++)
			{
				items[i] = base.InnerList[i].ToString();
			}
			return String.Join("|", items);
		}
	}

	internal class Selection
	{
		public static Selection FromString(string serializedData)
		{
            string[] values = serializedData.Split(';');
			return new Selection(values[0], values[1]);
		}
		
		public Selection(string className, string targetNs)
		{
			this.ClassName = className;
			this.TargetNamespace = targetNs;
		}
		
		public override string ToString()
		{
			return this.ClassName + ";" + this.TargetNamespace;
		}
		
		public string ClassName;
		public string TargetNamespace;
	}
}
