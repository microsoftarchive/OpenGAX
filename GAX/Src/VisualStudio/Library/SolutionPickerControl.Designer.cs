using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library
{
	internal partial class SolutionPickerControl
	{
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
			dte = null;
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionPickerControl));
			this.treeIcons = new System.Windows.Forms.ImageList(this.components);
			this.solutionTree = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// treeIcons
			// 
			this.treeIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeIcons.ImageStream")));
			this.treeIcons.Images.SetKeyName(0, "solution.gif");
			this.treeIcons.Images.SetKeyName(1, "SolutionFolderClose.gif");
			this.treeIcons.Images.SetKeyName(2, "SolutionFolderOpen.gif");
			this.treeIcons.Images.SetKeyName(3, "FolderClose.gif");
			this.treeIcons.Images.SetKeyName(4, "FolderOpen.gif");
			this.treeIcons.Images.SetKeyName(5, "ascx.gif");
			this.treeIcons.Images.SetKeyName(6, "asmx.gif");
			this.treeIcons.Images.SetKeyName(7, "aspxpage.gif");
			this.treeIcons.Images.SetKeyName(8, "bitmap.gif");
			this.treeIcons.Images.SetKeyName(9, "config.gif");
			this.treeIcons.Images.SetKeyName(10, "CppProject.gif");
			this.treeIcons.Images.SetKeyName(11, "CsFile.gif");
			this.treeIcons.Images.SetKeyName(12, "CsProject.gif");
			this.treeIcons.Images.SetKeyName(13, "CsWeb.gif");
			this.treeIcons.Images.SetKeyName(14, "disco.gif");
			this.treeIcons.Images.SetKeyName(15, "htmlpage.gif");
			this.treeIcons.Images.SetKeyName(16, "icon.gif");
			this.treeIcons.Images.SetKeyName(17, "Item.gif");
			this.treeIcons.Images.SetKeyName(18, "jscript.gif");
			this.treeIcons.Images.SetKeyName(19, "OtherProject.gif");
			this.treeIcons.Images.SetKeyName(20, "project.gif");
			this.treeIcons.Images.SetKeyName(21, "rctempl.gif");
			this.treeIcons.Images.SetKeyName(22, "References.gif");
			this.treeIcons.Images.SetKeyName(23, "sdl.gif");
			this.treeIcons.Images.SetKeyName(24, "Settings.gif");
			this.treeIcons.Images.SetKeyName(25, "stylesht.gif");
			this.treeIcons.Images.SetKeyName(26, "textfile.gif");
			this.treeIcons.Images.SetKeyName(27, "VbProject.gif");
			this.treeIcons.Images.SetKeyName(28, "vbscript.gif");
			this.treeIcons.Images.SetKeyName(29, "VbWeb.gif");
			this.treeIcons.Images.SetKeyName(30, "wscript.gif");
			this.treeIcons.Images.SetKeyName(31, "xmlfile.gif");
			this.treeIcons.Images.SetKeyName(32, "xsdschem.gif");
			this.treeIcons.Images.SetKeyName(33, "xsltfile.gif");
			this.treeIcons.Images.SetKeyName(34, "z-Configuration.gif");
			this.treeIcons.Images.SetKeyName(35, "z-LinkedFile.gif");
			this.treeIcons.Images.SetKeyName(36, "z-WebReference.gif");
			this.treeIcons.Images.SetKeyName(37, "z-WebReferences.gif");
			this.treeIcons.Images.SetKeyName(38, "zz-Assembly.gif");
			// 
			// solutionTree
			// 
			resources.ApplyResources(this.solutionTree, "solutionTree");
			this.solutionTree.ImageList = this.treeIcons;
			this.solutionTree.Name = "solutionTree";
			this.solutionTree.Sorted = true;
			// 
			// SolutionPickerControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.solutionTree);
			this.Name = "SolutionPickerControl";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ImageList treeIcons;
		private System.Windows.Forms.TreeView solutionTree;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
	}
}
