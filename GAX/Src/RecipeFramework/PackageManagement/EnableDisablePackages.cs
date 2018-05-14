#region Using directives

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Services;
using Config = Microsoft.Practices.RecipeFramework.Configuration;
using System.Globalization;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using System.Windows.Forms.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework.PackageManagement
{
	[System.ComponentModel.DesignerCategory("Form")]
	internal class EnableDisablePackages : ManagerForm
	{
		const string GaxPackageGuid = "{77D93A80-73FC-40f8-87DB-ACD3482964B2}";

		#region Designer stuff
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ImageList imageList2;
		private System.Windows.Forms.Button btnExplore;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView lstPackages;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private ColumnHeader columnHeader7;
		private System.ComponentModel.IContainer components;

		public EnableDisablePackages()
		{
			InitializeComponent();
		}

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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnableDisablePackages));
			

			this.imageList1 = new System.Windows.Forms.ImageList(this.components);

			this.imageList2 = new System.Windows.Forms.ImageList(this.components);
			this.btnExplore = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lstPackages = new System.Windows.Forms.ListView();
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "");
			this.imageList1.Images.SetKeyName(1, "");
			// 
			// imageList2
			// 
			this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
			this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList2.Images.SetKeyName(0, "");
			this.imageList2.Images.SetKeyName(1, "");
			// 
			// btnExplore
			// 
			resources.ApplyResources(this.btnExplore, "btnExplore");
			this.btnExplore.Name = "btnExplore";
			this.btnExplore.Click += new System.EventHandler(this.OnExplorePackage);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.BackColor = System.Drawing.SystemColors.Control;
			this.label1.Name = "label1";
			// 
			// lstPackages
			// 
			resources.ApplyResources(this.lstPackages, "lstPackages");
			this.lstPackages.CheckBoxes = true;
			this.lstPackages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
			this.lstPackages.MultiSelect = false;
			this.lstPackages.Name = "lstPackages";
			this.lstPackages.SmallImageList = this.imageList1;
			this.lstPackages.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lstPackages.UseCompatibleStateImageBehavior = false;
			this.lstPackages.View = System.Windows.Forms.View.Details;
			this.lstPackages.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lstPackages_ItemChecked);
			this.lstPackages.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstPackages_MouseDoubleClick);
			// 
			// columnHeader5
			// 
			resources.ApplyResources(this.columnHeader5, "columnHeader5");
			// 
			// columnHeader6
			// 
			resources.ApplyResources(this.columnHeader6, "columnHeader6");
			// 
			// columnHeader7
			// 
			resources.ApplyResources(this.columnHeader7, "columnHeader7");
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Name = "btnOK";
			this.btnOK.Click += new System.EventHandler(this.OnOkClick);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// EnableDisablePackages
			// 
			this.AcceptButton = this.btnOK;
			this.BackColor = System.Drawing.SystemColors.ControlLight;
			this.CancelButton = this.btnCancel;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnExplore);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lstPackages);
			this.Controls.Add(this.btnCancel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EnableDisablePackages";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);

		}
		#endregion

		#endregion Designer stuff

		/// <summary>
		/// Initializes the form with a parent provider.
		/// </summary>
		/// <param name="provider"></param>
		public EnableDisablePackages(IServiceProvider provider)
			: base(provider)
		{
			InitializeComponent();
		}

		/// <summary>
		/// Initializes services at siting time.
		/// </summary>
		public override ISite Site
		{
			get { return base.Site; }
			set
			{
				base.Site = value;
				base.InitServices(value);
			}
		}

		ArrayList changed = new ArrayList();

		private void OnOkClick(object sender, System.EventArgs e)
		{
			try
			{
				int current = 0;
				// Enable/Disable as appropriate.
				foreach (ListViewItem item in changed)
				{
					Config.Manifest.GuidancePackage Package = (Config.Manifest.GuidancePackage)item.Tag;
					if (item.Checked)
					{
						// Only enable if it wasn't previously enabled.
						if (base.RecipeManagerService.GetPackage(Package.Name) == null)
						{
							current++;
							try
							{
								base.RecipeManagerService.EnablePackage(Package.Name);
							}
							catch (Exception exception)
							{
								// Just ask if it is necessary
								bool askForTheRest = ((changed.Count - current) > 0);
								string message = string.Format(
									CultureInfo.CurrentCulture,
									askForTheRest ? Configuration.Resources.PackageManager_CannotLoadPackageAsking :
									Configuration.Resources.PackageManager_CannotLoadPackage,
									Package.Caption);
								DialogResult result = ErrorHelper.Show(this.Site, exception, message,
									askForTheRest ? MessageBoxButtons.YesNo : MessageBoxButtons.OK);
								if (askForTheRest && result == DialogResult.No)
								{
									break;
								}
							}
						}
					}
					else
					{
						base.RecipeManagerService.DisablePackage(Package.Name);
					}
				}
				this.Close();
			}
			catch (Exception ex)
			{
				ErrorHelper.Show(this.Site, ex);
			}
			finally
			{
				// Do not close if something went wrong.
			}
		}

		private void OnExplorePackage(object sender, System.EventArgs e)
		{
			try
			{
				if (lstPackages.SelectedItems.Count == 0)
				{
					MessageBox.Show(this, Configuration.Resources.EnableDisablePackages_MustSelectPackage,
						this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				// Make the form testable from Nunit/Maui
				if (this.Modal)
				{
					new PackageViewer(this.Site, (Config.Manifest.GuidancePackage)
					lstPackages.SelectedItems[0].Tag).ShowDialog(this);
				}
				else
				{
					new PackageViewer(this.Site, (Config.Manifest.GuidancePackage)
						lstPackages.SelectedItems[0].Tag).Show();
				}
			}
			catch (Exception ex)
			{
				ErrorHelper.Show(this.Site, ex);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			this.lstPackages.ItemChecked -= new ItemCheckedEventHandler(lstPackages_ItemChecked);
			try
			{
				btnOK.Enabled = (base.HostService != null);
				lstPackages.Items.Clear();
				base.OnLoad(e);
				this.SuspendLayout();

				Config.Manifest.GuidancePackage[] Packages;
				if (base.HostService != null)
				{
					Packages = base.RecipeManagerService.GetInstalledPackages(base.HostService.HostName);
				}
				else
				{
					// Otherwise, show all Packages, but the user will not be able to enable/disable anyone.
					Packages = base.RecipeManagerService.GetInstalledPackages();
				}

				foreach (Config.Manifest.GuidancePackage package in Packages)
				{
					ListViewItem item = new ListViewItem(
						new string[] {
						package.Caption, 
						package.Description,
						package.Version }, 0);
					item.Tag = package;
					item.Checked = (base.RecipeManagerService.GetPackage(package.Name) != null);
					lstPackages.Items.Add(item);
				}

				if (lstPackages.Items.Count > 0)
				{
					lstPackages.Items[0].Selected = true;
					btnExplore.Enabled = true;
				}

				this.ResumeLayout();
			}
			catch (Exception ex)
			{
				ErrorHelper.Show(this.Site, ex);
			}
			finally
			{
				this.lstPackages.ItemChecked += new ItemCheckedEventHandler(lstPackages_ItemChecked);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void lstPackages_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (lstPackages.SelectedItems.Count != 0)
			{
				ListViewHitTestInfo info = lstPackages.HitTest(e.X, e.Y);
				if (info != null)
				{
					ListViewItem itemPointed = info.Item;
					if (itemPointed != null)
					{
						itemPointed.Checked = !itemPointed.Checked;
					}
				}
				OnExplorePackage(sender, e);
			}
		}

		private void lstPackages_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			try
			{
				if (changed.IndexOf(e.Item) == -1)
				{
					changed.Add(e.Item);
				}
			}
			catch (Exception ex)
			{
				ErrorHelper.Show(this.Site, ex);
			}
		}
	}
}
