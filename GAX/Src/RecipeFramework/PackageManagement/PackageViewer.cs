#region Using directives

using System;
using System.ComponentModel;
using System.Windows.Forms;

using Config = Microsoft.Practices.RecipeFramework.Configuration;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.ComponentModel;
using System.Windows.Forms.Design;

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework.PackageManagement
{
	[System.ComponentModel.DesignerCategory("Form")]
	internal class PackageViewer : ManagerForm
	{
		#region Designer stuff
		private System.Windows.Forms.TreeView tvRecipes;
		private System.Windows.Forms.ImageList icons;
		private SplitContainer splitContainer1;
		private Panel panel1;
		private GroupBox groupBox1;
		private TextBox txtDescription;
		private Button CloseButton;
		private System.ComponentModel.IContainer components;

		public PackageViewer()
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageViewer));
			this.tvRecipes = new System.Windows.Forms.TreeView();
			this.icons = new System.Windows.Forms.ImageList(this.components);
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.CloseButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tvRecipes
			// 
			resources.ApplyResources(this.tvRecipes, "tvRecipes");
			this.tvRecipes.ImageList = this.icons;
			this.tvRecipes.Name = "tvRecipes";
			this.tvRecipes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvRecipes_AfterSelect);
			// 
			// icons
			// 
			this.icons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("icons.ImageStream")));
			this.icons.TransparentColor = System.Drawing.Color.Transparent;
			this.icons.Images.SetKeyName(0, "Dot.gif");
			this.icons.Images.SetKeyName(1, "Package.gif");
			this.icons.Images.SetKeyName(2, "Recipes.ico");
			this.icons.Images.SetKeyName(3, "Arguments.gif");
			this.icons.Images.SetKeyName(4, "Actions.gif");
			this.icons.Images.SetKeyName(5, "Templates.ico");
			this.icons.Images.SetKeyName(6, "Diamond.gif");
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tvRecipes);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.panel1);
			this.splitContainer1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this.groupBox1);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.groupBox1.Controls.Add(this.txtDescription);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// txtDescription
			// 
			resources.ApplyResources(this.txtDescription, "txtDescription");
			this.txtDescription.BackColor = System.Drawing.SystemColors.Control;
			this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ReadOnly = true;
			// 
			// CloseButton
			// 
			resources.ApplyResources(this.CloseButton, "CloseButton");
			this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Click += new System.EventHandler(this.OnCloseClick);
			// 
			// PackageViewer
			// 
			this.AcceptButton = this.CloseButton;
			this.BackColor = System.Drawing.SystemColors.ControlLight;
			this.CancelButton = this.CloseButton;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.CloseButton);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PackageViewer";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

        }
		#endregion

		#endregion Designer stuff

		/// <summary>
		/// Initializes the form with a parent provider.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="Package"></param>
		public PackageViewer(IServiceProvider provider, Config.Manifest.GuidancePackage Package) : base(provider)
		{
			InitializeComponent();
			this.Package = Package;
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

		private Config.Manifest.GuidancePackage Package;

		/// <summary>
		/// Gets/Sets the Package that will be explored.
		/// </summary>
		public Config.Manifest.GuidancePackage GuidancePackage
		{
			get { return Package; }
			set { Package = value; }
		}

		private void OnCloseClick(object sender, System.EventArgs e)
		{
			this.Close(); 
		}

        protected override void OnLoad(EventArgs e)
        {
			try
			{
				base.OnLoad(e);
				tvRecipes.Nodes.Clear();
				this.SuspendLayout();

				if (GuidancePackage != null)
				{
					TreeNode Packagenode = tvRecipes.Nodes.Add(String.Format(
						System.Globalization.CultureInfo.CurrentCulture,
						GuidancePackage.Caption, GuidancePackage.Version));
					Packagenode.ImageIndex = 1;
					Packagenode.SelectedImageIndex = 1;
					Packagenode.Tag = GuidancePackage;

					TreeNode recipes = Packagenode.Nodes.Add(
						Configuration.Resources.PackageViewer_RecipesNode);
					recipes.ImageIndex = 2;
					recipes.SelectedImageIndex = 2;

					// Read the Package configuration.
					Config.GuidancePackage configuration = RecipeFramework.GuidancePackage.ReadConfiguration(
						GuidancePackage.ConfigurationFile);

					foreach (Config.Recipe recipe in configuration.Recipes)
					{
						TreeNode recipenode = recipes.Nodes.Add(recipe.Caption);
						recipenode.Tag = new AssetDescription(
							Configuration.Resources.PackageViewer_RecipesNode,
							recipe.Caption, recipe.Description);
						recipenode.ImageIndex = 6;
						recipenode.SelectedImageIndex = 6;
						if (recipe.Arguments != null && recipe.Arguments.Length > 0)
						{
							TreeNode argumentsnode = recipenode.Nodes.Add(Configuration.Resources.PackageViewer_ArgumentsNode);
							argumentsnode.ImageIndex = 3;
							argumentsnode.SelectedImageIndex = 3;
							foreach (Config.Argument argument in recipe.Arguments)
							{
								TreeNode argnode = argumentsnode.Nodes.Add(argument.Name);
								argnode.Tag = argument;
								// UNDONE: we can provide as the description of the selected 
								// argument its type and maybe converter and value provider by creating a properly formatted 
								// AssetDescription and that's it. The node selection code will pull it automatically.
							}
						}
						if (recipe.Actions != null && recipe.Actions.Action.Length > 0)
						{
							TreeNode actionsnode = recipenode.Nodes.Add(Configuration.Resources.PackageViewer_ActionsNode);
							actionsnode.ImageIndex = 4;
							actionsnode.SelectedImageIndex = 4;
							foreach (Config.Action action in recipe.Actions.Action)
							{
								TreeNode actionnode = actionsnode.Nodes.Add(action.Name);
								actionnode.Tag = action;
								// UNDONE: we can provide as the description of the selected 
								// action its input and output values by creating a properly formatted 
								// AssetDescription and that's it. The node selection code will pull it automatically.
							}
						}
					}

					// Call host for additional assets that may be available.
					if (base.HostService != null)
					{
						IAssetDescription[] descriptions = base.HostService.GetHostAssets(
							System.IO.Path.GetDirectoryName(GuidancePackage.ConfigurationFile),
							configuration);
						if (descriptions != null)
						{
							foreach (IAssetDescription asset in descriptions)
							{
								TreeNode categorynode = GetCategoryNode(Packagenode, asset.Category.Split(new char[] { '\\', '/' }), 0);
								categorynode.ImageIndex = 5;
								categorynode.SelectedImageIndex = 5;
								TreeNode assetnode = new TreeNode(asset.Caption);
								assetnode.Tag = asset;
								assetnode.ImageIndex = 6;
								assetnode.SelectedImageIndex = 6;
								categorynode.Nodes.Add(assetnode);
							}
						}
					}
				}

				this.ResumeLayout();
			}
			catch (Exception ex)
			{
				ErrorHelper.Show(this.Site, ex);
			}
		}

		private TreeNode GetCategoryNode(TreeNode parentNode, string[] categories, int currentIndex)
		{
			string category = categories[currentIndex];
			if (category.Length == 0 && currentIndex == categories.Length)
			{
				return parentNode;
			}

			foreach (TreeNode child in parentNode.Nodes)
			{
				if (child.Text == category)
				{
					if (currentIndex == categories.Length - 1)
					{
						return child;
					}
					else
					{
						return GetCategoryNode(child, categories, ++currentIndex);
					}
				}
			}

			// All child nodes will be brand new.
			TreeNode newcategory = parentNode.Nodes.Add(category);
			for (int i = currentIndex + 1; i < categories.Length; i++)
			{
				if (categories[i].Length > 0)
				{
					newcategory = newcategory.Nodes.Add(categories[i]);
				}
			}

			return newcategory;
		}


		private void RefreshDescription()
		{
			txtDescription.Text = "";
			if (tvRecipes.SelectedNode.Tag is Config.Manifest.GuidancePackage)
			{
				txtDescription.Text = ((Config.Manifest.GuidancePackage)tvRecipes.SelectedNode.Tag).Description;
			}
			TreeNode node = tvRecipes.SelectedNode;
			while (node != null)
			{
				if (node.Tag is IAssetDescription)
				{
					txtDescription.Text = ((IAssetDescription)node.Tag).Description;
					break;
				}
				node = node.Parent;
			}
			this.Invalidate(true);
		}

        private void tvRecipes_AfterSelect(object sender, TreeViewEventArgs e)
        {
			try
			{
				RefreshDescription();
			}
			catch (Exception ex)
			{
				ErrorHelper.Show(this.Site, ex);
			}
		}
	}
}
