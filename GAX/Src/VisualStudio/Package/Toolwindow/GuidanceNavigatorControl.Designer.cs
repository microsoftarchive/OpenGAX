namespace Microsoft.Practices.RecipeFramework.VisualStudio.ToolWindow
{
    partial class GuidanceNavigatorControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GuidanceNavigatorControl));
			this.wbPackageOverview = new System.Windows.Forms.WebBrowser();
			this.toolStripGuidancePackages = new System.Windows.Forms.ToolStrip();
			this.toolStripGuidancePackageLabel = new System.Windows.Forms.ToolStripLabel();
			this.cbGuidancePackages = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripPackage = new System.Windows.Forms.ToolStrip();
			this.toolStripPackageOverview = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripAvailableRecipes = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripCompletedRecipes = new System.Windows.Forms.ToolStripButton();
			this.toolStripRefreshButton = new System.Windows.Forms.ToolStripButton();
			this.panelOverview = new System.Windows.Forms.Panel();
			this.panelCompletedRecipes = new System.Windows.Forms.Panel();
			this.panelAvailableRecipes = new System.Windows.Forms.Panel();
			this.contextMenuRecipe = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItemExecuteRecipe = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuDocumentation = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItemOpenDocumentation = new System.Windows.Forms.ToolStripMenuItem();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.toolStripGuidancePackages.SuspendLayout();
			this.toolStripPackage.SuspendLayout();
			this.panelOverview.SuspendLayout();
			this.contextMenuRecipe.SuspendLayout();
			this.contextMenuDocumentation.SuspendLayout();
			this.SuspendLayout();
			// 
			// wbPackageOverview
			// 
			this.wbPackageOverview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wbPackageOverview.Location = new System.Drawing.Point(0, 0);
			this.wbPackageOverview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.wbPackageOverview.MinimumSize = new System.Drawing.Size(30, 31);
			this.wbPackageOverview.Name = "wbPackageOverview";
			this.wbPackageOverview.Size = new System.Drawing.Size(499, 154);
			this.wbPackageOverview.TabIndex = 0;
			this.wbPackageOverview.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.wbPackageOverview_Navigating);
			// 
			// toolStripGuidancePackages
			// 
			this.toolStripGuidancePackages.CanOverflow = false;
			this.toolStripGuidancePackages.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripGuidancePackages.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStripGuidancePackages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripGuidancePackageLabel,
            this.cbGuidancePackages});
			this.toolStripGuidancePackages.Location = new System.Drawing.Point(0, 0);
			this.toolStripGuidancePackages.Name = "toolStripGuidancePackages";
			this.toolStripGuidancePackages.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.toolStripGuidancePackages.Size = new System.Drawing.Size(540, 33);
			this.toolStripGuidancePackages.TabIndex = 5;
			this.toolStripGuidancePackages.Text = "toolStrip1";
			// 
			// toolStripGuidancePackageLabel
			// 
			this.toolStripGuidancePackageLabel.Name = "toolStripGuidancePackageLabel";
			this.toolStripGuidancePackageLabel.Size = new System.Drawing.Size(159, 30);
			this.toolStripGuidancePackageLabel.Text = "Guidance Package:";
			// 
			// cbGuidancePackages
			// 
			this.cbGuidancePackages.AutoSize = false;
			this.cbGuidancePackages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbGuidancePackages.DropDownWidth = 250;
			this.cbGuidancePackages.Name = "cbGuidancePackages";
			this.cbGuidancePackages.Size = new System.Drawing.Size(373, 33);
			this.cbGuidancePackages.SelectedIndexChanged += new System.EventHandler(this.cbGuidancePackages_SelectedIndexChanged);
			// 
			// toolStripPackage
			// 
			this.toolStripPackage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripPackage.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStripPackage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPackageOverview,
            this.toolStripSeparator1,
            this.toolStripAvailableRecipes,
            this.toolStripSeparator2,
            this.toolStripCompletedRecipes,
            this.toolStripRefreshButton});
			this.toolStripPackage.Location = new System.Drawing.Point(0, 33);
			this.toolStripPackage.Name = "toolStripPackage";
			this.toolStripPackage.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.toolStripPackage.Size = new System.Drawing.Size(540, 32);
			this.toolStripPackage.TabIndex = 6;
			this.toolStripPackage.Text = "toolStrip2";
			// 
			// toolStripPackageOverview
			// 
			this.toolStripPackageOverview.CheckOnClick = true;
			this.toolStripPackageOverview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripPackageOverview.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripPackageOverview.Name = "toolStripPackageOverview";
			this.toolStripPackageOverview.Size = new System.Drawing.Size(89, 29);
			this.toolStripPackageOverview.Text = "Overview";
			this.toolStripPackageOverview.Click += new System.EventHandler(this.toolStripPackageOverview_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
			// 
			// toolStripAvailableRecipes
			// 
			this.toolStripAvailableRecipes.CheckOnClick = true;
			this.toolStripAvailableRecipes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripAvailableRecipes.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAvailableRecipes.Image")));
			this.toolStripAvailableRecipes.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripAvailableRecipes.Name = "toolStripAvailableRecipes";
			this.toolStripAvailableRecipes.Size = new System.Drawing.Size(165, 29);
			this.toolStripAvailableRecipes.Text = "Available Guidance";
			this.toolStripAvailableRecipes.Click += new System.EventHandler(this.toolStripAvailableRecipes_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
			// 
			// toolStripCompletedRecipes
			// 
			this.toolStripCompletedRecipes.CheckOnClick = true;
			this.toolStripCompletedRecipes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripCompletedRecipes.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripCompletedRecipes.Name = "toolStripCompletedRecipes";
			this.toolStripCompletedRecipes.Size = new System.Drawing.Size(73, 29);
			this.toolStripCompletedRecipes.Text = "History";
			this.toolStripCompletedRecipes.Click += new System.EventHandler(this.toolStripCompletedRecipes_Click);
			// 
			// toolStripRefreshButton
			// 
			this.toolStripRefreshButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripRefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripRefreshButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripRefreshButton.Image")));
			this.toolStripRefreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripRefreshButton.Name = "toolStripRefreshButton";
			this.toolStripRefreshButton.Size = new System.Drawing.Size(28, 29);
			this.toolStripRefreshButton.Text = "toolStripButton1";
			this.toolStripRefreshButton.ToolTipText = "Refresh Available Guidance";
			this.toolStripRefreshButton.Click += new System.EventHandler(this.toolStripRefreshButton_Click);
			// 
			// panelOverview
			// 
			this.panelOverview.Controls.Add(this.wbPackageOverview);
			this.panelOverview.Location = new System.Drawing.Point(24, 95);
			this.panelOverview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.panelOverview.Name = "panelOverview";
			this.panelOverview.Size = new System.Drawing.Size(499, 154);
			this.panelOverview.TabIndex = 7;
			// 
			// panelCompletedRecipes
			// 
			this.panelCompletedRecipes.Location = new System.Drawing.Point(24, 276);
			this.panelCompletedRecipes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.panelCompletedRecipes.Name = "panelCompletedRecipes";
			this.panelCompletedRecipes.Size = new System.Drawing.Size(499, 170);
			this.panelCompletedRecipes.TabIndex = 8;
			// 
			// panelAvailableRecipes
			// 
			this.panelAvailableRecipes.Location = new System.Drawing.Point(24, 470);
			this.panelAvailableRecipes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.panelAvailableRecipes.Name = "panelAvailableRecipes";
			this.panelAvailableRecipes.Size = new System.Drawing.Size(499, 172);
			this.panelAvailableRecipes.TabIndex = 9;
			// 
			// contextMenuRecipe
			// 
			this.contextMenuRecipe.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.contextMenuRecipe.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemExecuteRecipe});
			this.contextMenuRecipe.Name = "contextMenuRecipe";
			this.contextMenuRecipe.Size = new System.Drawing.Size(144, 34);
			// 
			// toolStripMenuItemExecuteRecipe
			// 
			this.toolStripMenuItemExecuteRecipe.Name = "toolStripMenuItemExecuteRecipe";
			this.toolStripMenuItemExecuteRecipe.Size = new System.Drawing.Size(143, 30);
			this.toolStripMenuItemExecuteRecipe.Text = "Execute";
			// 
			// contextMenuDocumentation
			// 
			this.contextMenuDocumentation.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.contextMenuDocumentation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOpenDocumentation});
			this.contextMenuDocumentation.Name = "contextMenuDocumentation";
			this.contextMenuDocumentation.Size = new System.Drawing.Size(129, 34);
			// 
			// toolStripMenuItemOpenDocumentation
			// 
			this.toolStripMenuItemOpenDocumentation.Name = "toolStripMenuItemOpenDocumentation";
			this.toolStripMenuItemOpenDocumentation.Size = new System.Drawing.Size(128, 30);
			this.toolStripMenuItemOpenDocumentation.Text = "Open";
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "recipe.gif");
			this.imageList.Images.SetKeyName(1, "point.gif");
			this.imageList.Images.SetKeyName(2, "run.gif");
			this.imageList.Images.SetKeyName(3, "template.gif");
			// 
			// GuidanceNavigatorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panelAvailableRecipes);
			this.Controls.Add(this.panelCompletedRecipes);
			this.Controls.Add(this.panelOverview);
			this.Controls.Add(this.toolStripPackage);
			this.Controls.Add(this.toolStripGuidancePackages);
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "GuidanceNavigatorControl";
			this.Size = new System.Drawing.Size(540, 660);
			this.Load += new System.EventHandler(this.GuidanceNavigatorControl_Load);
			this.toolStripGuidancePackages.ResumeLayout(false);
			this.toolStripGuidancePackages.PerformLayout();
			this.toolStripPackage.ResumeLayout(false);
			this.toolStripPackage.PerformLayout();
			this.panelOverview.ResumeLayout(false);
			this.contextMenuRecipe.ResumeLayout(false);
			this.contextMenuDocumentation.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser wbPackageOverview;
        private System.Windows.Forms.ToolStrip toolStripGuidancePackages;
        private System.Windows.Forms.ToolStripLabel toolStripGuidancePackageLabel;
        private System.Windows.Forms.ToolStripComboBox cbGuidancePackages;
        private System.Windows.Forms.ToolStrip toolStripPackage;
        private System.Windows.Forms.ToolStripButton toolStripPackageOverview;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripCompletedRecipes;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripAvailableRecipes;
        private System.Windows.Forms.Panel panelOverview;
        private System.Windows.Forms.Panel panelCompletedRecipes;
        private System.Windows.Forms.Panel panelAvailableRecipes;
        private System.Windows.Forms.ContextMenuStrip contextMenuRecipe;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExecuteRecipe;
        private System.Windows.Forms.ContextMenuStrip contextMenuDocumentation;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpenDocumentation;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStripButton toolStripRefreshButton;

    }
}
