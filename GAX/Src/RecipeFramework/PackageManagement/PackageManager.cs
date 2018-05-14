#region Using directives

using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Services;
using Config = Microsoft.Practices.RecipeFramework.Configuration;
using System.Drawing.Imaging;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using System.Threading;
using System.Windows.Forms.Design;
using System.Diagnostics;
using System.Text;
using System.Globalization;

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework.PackageManagement
{
	/// <summary>
	/// Main UI for the management of recipes.
	/// </summary>
	/// <remarks>
	/// Shows all the recipe references existing in the manager.
	/// </remarks>
	[System.ComponentModel.DesignerCategory("Form")]
	public class PackageManager : ManagerForm
	{
		#region Designer stuff
		private System.Windows.Forms.ImageList imgImages;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button PackagesButton;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Button executeButton;
		private SplitContainer splitter;
		private ListView lstRecipes;
		private ColumnHeader Reference;
		private ColumnHeader AppliesTo;
		private GroupBox groupBox4;
		private TextBox txtDescription;
        private ColumnHeader Package;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Initializes the form PackageManager
		/// Mostly to be visible at design time 
		/// </summary>
		public PackageManager()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			CreateContextMenuRecipe();
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
				if (components != null) 
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageManager));
			this.imgImages = new System.Windows.Forms.ImageList(this.components);
			this.executeButton = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.closeButton = new System.Windows.Forms.Button();
			this.PackagesButton = new System.Windows.Forms.Button();
			this.splitter = new System.Windows.Forms.SplitContainer();
			this.lstRecipes = new System.Windows.Forms.ListView();
			this.Reference = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AppliesTo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Package = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.txtDescription = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
			this.splitter.Panel1.SuspendLayout();
			this.splitter.Panel2.SuspendLayout();
			this.splitter.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// imgImages
			// 
			this.imgImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgImages.ImageStream")));
			this.imgImages.TransparentColor = System.Drawing.Color.Transparent;
			this.imgImages.Images.SetKeyName(0, "IBoundAssetReference.bmp");
			this.imgImages.Images.SetKeyName(1, "IUnboundAssetReference.bmp");
			this.imgImages.Images.SetKeyName(2, "ProjectItemReference.bmp");
			this.imgImages.Images.SetKeyName(3, "ProjectReference.bmp");
			this.imgImages.Images.SetKeyName(4, "SolutionReference.bmp");
			this.imgImages.Images.SetKeyName(5, "BoundTemplateReference.bmp");
			this.imgImages.Images.SetKeyName(6, "UnboundTemplateReference.bmp");
			// 
			// executeButton
			// 
			resources.ApplyResources(this.executeButton, "executeButton");
			this.executeButton.Name = "executeButton";
			this.executeButton.Click += new System.EventHandler(this.OnExecuteRecipe);
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label6.Name = "label6";
			// 
			// closeButton
			// 
			resources.ApplyResources(this.closeButton, "closeButton");
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.Name = "closeButton";
			this.closeButton.Click += new System.EventHandler(this.OnCloseButtonClick);
			// 
			// PackagesButton
			// 
			resources.ApplyResources(this.PackagesButton, "PackagesButton");
			this.PackagesButton.Name = "PackagesButton";
			this.PackagesButton.Click += new System.EventHandler(this.OnEnableDisablePackages);
			// 
			// splitter
			// 
			resources.ApplyResources(this.splitter, "splitter");
			this.splitter.Name = "splitter";
			// 
			// splitter.Panel1
			// 
			this.splitter.Panel1.Controls.Add(this.lstRecipes);
			// 
			// splitter.Panel2
			// 
			this.splitter.Panel2.Controls.Add(this.groupBox4);
			this.splitter.TabStop = false;
			// 
			// lstRecipes
			// 
			this.lstRecipes.AllowColumnReorder = true;
			this.lstRecipes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Reference,
            this.AppliesTo,
            this.Package});
			resources.ApplyResources(this.lstRecipes, "lstRecipes");
			this.lstRecipes.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lstRecipes.Items"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lstRecipes.Items1"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lstRecipes.Items2"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lstRecipes.Items3"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lstRecipes.Items4"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lstRecipes.Items5"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lstRecipes.Items6")))});
			this.lstRecipes.MultiSelect = false;
			this.lstRecipes.Name = "lstRecipes";
			this.lstRecipes.SmallImageList = this.imgImages;
			this.lstRecipes.UseCompatibleStateImageBehavior = false;
			this.lstRecipes.View = System.Windows.Forms.View.Details;
			this.lstRecipes.SelectedIndexChanged += new System.EventHandler(this.lstRecipes_SelectedIndexChanged);
			this.lstRecipes.DoubleClick += new System.EventHandler(this.lstRecipes_DoubleClick);
			this.lstRecipes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstRecipes_KeyDown);
			this.lstRecipes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstRecipes_MouseDown);
			// 
			// Reference
			// 
			resources.ApplyResources(this.Reference, "Reference");
			// 
			// AppliesTo
			// 
			resources.ApplyResources(this.AppliesTo, "AppliesTo");
			// 
			// Package
			// 
			resources.ApplyResources(this.Package, "Package");
			// 
			// groupBox4
			// 
			this.groupBox4.BackColor = System.Drawing.SystemColors.ControlDark;
			this.groupBox4.Controls.Add(this.txtDescription);
			resources.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			// 
			// txtDescription
			// 
			resources.ApplyResources(this.txtDescription, "txtDescription");
			this.txtDescription.BackColor = System.Drawing.SystemColors.Control;
			this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ReadOnly = true;
			// 
			// PackageManager
			// 
			this.AcceptButton = this.executeButton;
			this.BackColor = System.Drawing.SystemColors.ControlLight;
			this.CancelButton = this.closeButton;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.splitter);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.executeButton);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.PackagesButton);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PackageManager";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.splitter.Panel1.ResumeLayout(false);
			this.splitter.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
			this.splitter.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);

        }
		#endregion
		
		#endregion Designer stuff

		ContextMenu contextMenuRecipe;
		object currentSelection;

		/// <summary>
		/// Initializes the form with a parent provider.
		/// </summary>
		public PackageManager(IServiceProvider provider, object currentHostSelection) : base(provider)
		{
			InitializeComponent();
			currentSelection = currentHostSelection;
            if (base.HostService != null)
            {
                this.PackagesButton.Enabled = base.RecipeManagerService.GetInstalledPackages(base.HostService.HostName).Length > 0;
            }
            else
            {
                // Otherwise, will show all Packages, but the user will not be able to enable/disable anyone.
                this.PackagesButton.Enabled = base.RecipeManagerService.GetInstalledPackages().Length > 0;
            }
			CreateContextMenuRecipe();
        }

		void CreateContextMenuRecipe()
		{
			contextMenuRecipe = new ContextMenu();
			MenuItem item1 = new MenuItem(Configuration.Resources.PackageManager_ExecuteRecipe, OnExecuteRecipe);
			MenuItem item2 = new MenuItem(Configuration.Resources.PackageManager_DeleteRecipe, OnDeleteRecipe);
			contextMenuRecipe.MenuItems.Add(item1);
			contextMenuRecipe.MenuItems.Add(item2);
		}

		/// <summary>
		/// Performs service initialization.
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

        private void OnEnableDisablePackages(object sender, System.EventArgs e)
        {
			try
			{
				// Make the form testable from NUnit/Maui
				if (this.Modal)
				{
					if (new EnableDisablePackages(this.Site).ShowDialog(this) == DialogResult.OK)
					{
						LoadReferences();
					}
				}
				else
				{
                    using (Form enable = new EnableDisablePackages(this.Site))
                    {
                        enable.FormClosed += new FormClosedEventHandler(OnFormClosed);
                        enable.Show();
                    }
				}
			}
			catch (Exception ex)
			{
				ErrorHelper.Show(this.Site, ex);
			}
		}

		void OnFormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{

				if (((Form)sender).DialogResult == DialogResult.OK)
				{
					LoadReferences();
				}
			}
			catch (Exception ex)
			{
				ErrorHelper.Show(this.Site, ex);
			}
		}

		private void OnCloseButtonClick(object sender, System.EventArgs e)
		{
			this.Close();
		}

        private void OnExecuteRecipe(object sender, System.EventArgs e)
        {
			GuidancePackage Package = null;
			try
			{
				if (lstRecipes.SelectedItems.Count == 0)
				{
					MessageBox.Show(this, Configuration.Resources.PackageManager_MustSelectRecipe,
						this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				IHostService host = (IHostService)ServiceHelper.GetService(this, typeof(IHostService));
				IAssetReference reference = ((ReferenceInfo)lstRecipes.SelectedItems[0].Tag).Reference;
				bool execute = false;
				if (reference is IBoundAssetReference)
				{
					execute = host.SelectTarget(((IBoundAssetReference)reference).Target);
				}
				else if (reference is IUnboundAssetReference)
				{
					execute = host.SelectTarget(this, (IUnboundAssetReference)reference);
				}

				if (execute)
				{
					Package = (GuidancePackage)ServiceHelper.GetService(reference, typeof(IExecutionService));
					Package.TurnOnOutput();
					reference.Execute();
					LoadReferences();
				}
			}
			catch (Exception ex)
			{
				ErrorHelper.Show(this.Site, ex);
			}
			finally
			{
				if (Package != null)
				{
					Package.TurnOffOutput();
				}
			}
        }

		/// <summary>
		/// Called when the form is loaded.
		/// </summary>
		/// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
			try
			{
				base.OnLoad(e);
				LoadReferences();
			}
			catch (Exception ex)
			{
				ErrorHelper.Show(this.Site, ex);
			}
        }

		private void LoadReferences()
		{
			this.SuspendLayout();
            this.imgImages.Images.Clear();
			this.lstRecipes.Items.Clear();
			GuidancePackage[] Packages = base.RecipeManagerService.GetEnabledPackages();

			ArrayList images = new ArrayList();
			// Add the two default images.
			ToolboxBitmapAttribute boundattr = new ToolboxBitmapAttribute(typeof(IBoundAssetReference));
            Image boundimg = boundattr.GetImage(boundattr);
			this.imgImages.Images.Add(boundimg);
            // Add gray version
            this.imgImages.Images.Add(ConvertToGrayscale(boundimg));
			ToolboxBitmapAttribute unboundattr = new ToolboxBitmapAttribute(typeof(IUnboundAssetReference));
            Image unboundimg = unboundattr.GetImage(unboundattr);
			this.imgImages.Images.Add(unboundimg);
            // Add gray version
            this.imgImages.Images.Add(ConvertToGrayscale(unboundimg));

            Hashtable groups = new Hashtable();
            ArrayList items = new ArrayList();

            // WORKAROUND: fixed for VSWhidbey "by design" bug #440390. Now we load items and groups 
            // first, then sort everything manually, and finally add stuff to the control.
            // Commented lines should be documented when feature is fixed.
			foreach (GuidancePackage Package in Packages)
			{
				IAssetReferenceService refservice = (IAssetReferenceService)
					Package.GetService(typeof(IAssetReferenceService), true);
				IConfigurationService configService =
					(IConfigurationService)Package.GetService(typeof(IConfigurationService), true);

				foreach (IAssetReference reference in refservice.GetAll())
				{
                    ReferenceInfo info = new ReferenceInfo(reference);

					#region Determine image

					int imageindex = images.IndexOf(reference.GetType());
					// Determine image to show.
                    if (imageindex == -1)
                    {
                        ToolboxBitmapAttribute bmp = (ToolboxBitmapAttribute)Attribute.GetCustomAttribute(
                            reference.GetType(), typeof(ToolboxBitmapAttribute), true);
                        if (bmp == null)
                        {
                            // Use default attributes.
                            if (reference is IBoundAssetReference)
                            {
                                imageindex = 0;
                            }
                            else
                            {
                                imageindex = 2;
                            }
                            if (!info.IsEnabled)
                            {
                                imageindex++;
                            }
                        }
                        else
                        {
                            imageindex = this.imgImages.Images.Count;
                            Image newimg = bmp.GetImage(reference);
                            if (info.IsEnabled)
                            {
                                this.imgImages.Images.Add(newimg);
                            }
                            else
                            {
                                // Add gray version.
                                this.imgImages.Images.Add(ConvertToGrayscale(newimg));
                            }
                            images.Add(reference.GetType());
                        }
                    }
                    else
                    {
                        // Account for the 4 built-in images.
                        imageindex = imageindex + 4;
                    }

					#endregion Determine image

					#region Determine group

					// Bug: Should we fix this?
					// We should get custom attributes instead of a single attr.
					// The reference could override the category
					//CategoryAttribute category = (CategoryAttribute)Attribute.GetCustomAttribute(
					//    reference.GetType(), typeof(CategoryAttribute), true);
					CategoryAttribute category = (CategoryAttribute)Attribute.GetCustomAttributes(
						reference.GetType(), typeof(CategoryAttribute), true).First();
					// ListViewGroup categorygroup = this.lstRecipes.Groups[category.Category];
                    ListViewGroup categorygroup = (ListViewGroup)groups[category.Category];
					if (categorygroup == null)
					{
						//categorygroup = this.lstRecipes.Groups.Add(category.Category, category.Category);
                        categorygroup = new ListViewGroup(category.Category, category.Category);
                        groups.Add(category.Category, categorygroup);
					}

					#endregion Determine group

					Config.Recipe recipe = null;
					if (reference is RecipeReference)
					{
						recipe = configService.CurrentPackage[reference.AssetName];
					}

					string[] subitems = new string[3];
					string errors = string.Empty;
					Exception ex = null;
					try
					{
						subitems[0] = reference.Caption;
					}
					catch (Exception e)
					{
						if (recipe != null)
						{
							subitems[0] = recipe.Caption;
						}
						ex = e;
						errors = "Caption";
					}
					try
					{
						subitems[1] = reference.AppliesTo;
					}
					catch (Exception e)
					{
						subitems[1] = Configuration.Resources.Reference_AppliesToThrew;
						if (ex == null)
						{
							ex = e;
						}
						if (!string.IsNullOrEmpty(errors))
						{
							errors += ", ";
						}
						errors += "AppliesTo";
					}
					subitems[2] = Package.Configuration.Caption;
					ListViewItem item = new ListViewItem(subitems, imageindex);
					item.Tag = info;
					item.Group = categorygroup;
					items.Add(item);
					if (ex != null)
					{
						ErrorHelper.Show(this.Site, new RecipeExecutionException(reference.AssetName, 
							string.Format(CultureInfo.CurrentCulture,
							Configuration.Resources.Reference_InvalidAttributes,
							errors), ex));
					}
				}
			}

            // Sort everything.
            ListViewGroup[] lvgroups = new ListViewGroup[groups.Count];
            groups.Values.CopyTo(lvgroups, 0);
            items.Sort(new ListViewItemSortComparer());
            Array.Sort(lvgroups, new GroupSortComparer());

            this.lstRecipes.Groups.AddRange(lvgroups);
            this.lstRecipes.Items.AddRange((ListViewItem[])items.ToArray(typeof(ListViewItem)));

            this.lstRecipes.Invalidate();
			this.ResumeLayout();

            if (this.lstRecipes.Items.Count > 0)
            {
                splitter.Panel1.Focus();
                splitter.Panel1.Select();
                ((ListViewItem)items[0]).Selected = true;
                this.lstRecipes.Focus();
                this.lstRecipes.Select();
            }
            else
            {
                this.txtDescription.Text = String.Empty;
            }
		}

        private void lstRecipes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstRecipes.SelectedItems.Count > 0)
            {
                ReferenceInfo info = (ReferenceInfo)lstRecipes.SelectedItems[0].Tag;
                txtDescription.Text = info.Reference.Description;
                executeButton.Enabled = info.IsEnabled;
            }
            else
            {
                executeButton.Enabled = false;
            }
            this.Invalidate(true);
        }

        private void lstRecipes_DoubleClick(object sender, EventArgs e)
        {
            if (this.lstRecipes.SelectedItems.Count == 1 &&
                ((ReferenceInfo)lstRecipes.SelectedItems[0].Tag).IsEnabled)
			{
				OnExecuteRecipe(sender, e);
            }
        }

        private void lstRecipes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && this.lstRecipes.SelectedItems.Count == 1)
            {
				OnDeleteRecipe(sender, e);
            }
        }

		private void OnDeleteRecipe(object sender, EventArgs e)
		{
			if (this.lstRecipes.SelectedItems.Count != 1)
			{
				return;
			}
			if (MessageBox.Show(this, Configuration.Resources.PackageManager_ConfirmDeleteReference,
				this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
			{
				try
				{
					ReferenceInfo refinfo = (ReferenceInfo)lstRecipes.SelectedItems[0].Tag;
					IAssetReferenceService refsvc = (IAssetReferenceService)
						ServiceHelper.GetService(refinfo.Reference, typeof(IAssetReferenceService));
					refsvc.Remove(refinfo.Reference);
					this.lstRecipes.Items.Remove(lstRecipes.SelectedItems[0]);
					if (this.lstRecipes.Items.Count != 0)
					{
						this.lstRecipes.SelectedIndices.Clear();
						this.lstRecipes.SelectedIndices.Add(0);
					}
					else
					{
						this.txtDescription.Clear();
					}
				}
				catch (Exception ex)
				{
					ErrorHelper.Show(this.Site, ex);
				}
			}
		}

		private void lstRecipes_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				try
				{
					ListViewHitTestInfo info = lstRecipes.HitTest(e.X, e.Y);
					if (info != null)
					{
						ListViewItem itemSelected = info.Item;
						if (itemSelected != null)
						{
							if (!itemSelected.Selected)
							{
								foreach (int oldSelected in this.lstRecipes.SelectedIndices)
								{
									this.lstRecipes.Items[oldSelected].Selected = false;
								}
								itemSelected.Selected = true;
								itemSelected.Focused = true;
							}
							ReferenceInfo referenceInfo = (ReferenceInfo)itemSelected.Tag;

							contextMenuRecipe.MenuItems[0].Enabled = referenceInfo.IsEnabled;
							contextMenuRecipe.Show((Control)sender, e.Location);
						}
					}
				}
				catch (Exception ex)
				{
					ErrorHelper.Show(this.Site, ex);
				}
			}
		}

		private class ReferenceInfo
        {
            public IAssetReference Reference;
            public bool IsEnabled;

            public ReferenceInfo(IAssetReference reference)
            {
                this.Reference = reference;
                ManagerExecutableAttribute attr = (ManagerExecutableAttribute)Attribute.GetCustomAttribute(
                    reference.GetType(), typeof(ManagerExecutableAttribute), true);
                // By default references are executable, unless they specify otherwise.
                this.IsEnabled = (attr == null) || attr.AllowExecute;
            }
        }

        static private Image ConvertToGrayscale(Image img)
        {
            Bitmap bm = new Bitmap(img.Width, img.Height);
            using (Graphics g = Graphics.FromImage(bm))
            {
                ColorMatrix cm = new ColorMatrix(new float[][]{   
                new float[]{0.3f,0.3f,0.3f,0,0},
                new float[]{0.59f,0.59f,0.59f,0,0},
                new float[]{0.11f,0.11f,0.11f,0,0},
                new float[]{0,0,0,1,0,0},
                new float[]{0,0,0,0,1,0},
                new float[]{0,0,0,0,0,1}});
                using (ImageAttributes ia = new ImageAttributes())
                {
                    ia.SetColorMatrix(cm);
                    g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height),
                        0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
                    g.DrawString("X", new Font("Arial Black", 11, FontStyle.Regular, GraphicsUnit.Point),
                        Brushes.Black, img.Width - 14, img.Height - 16);
                    return bm;
                }
            }
        }

        private class GroupSortComparer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                return String.Compare(((ListViewGroup)x).Header,
                    ((ListViewGroup)y).Header);
            }

            #endregion
        }

        private class ListViewItemSortComparer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                ListViewItem ix = (ListViewItem)x;
                ListViewItem iy = (ListViewItem)y;

                int groups = String.Compare(ix.Group.Header, iy.Group.Header);
                if (groups == 0)
                {
                    return String.Compare(ix.Text, iy.Text);
                }
                else
                {
                    return groups;
                }
            }

            #endregion
        }
    }
}
