using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Practices.Common;
using EnvDTE;
using System.Text;
using EnvDTE80;
using Microsoft.Practices.RecipeFramework.Library.CodeModel;
using System.Globalization;
using Microsoft.Practices.WizardFramework;

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel.Editors
{
	/// <summary>
	/// Allows browsing for a class name.
	/// </summary>
	internal class CodeModelEditorForm : System.Windows.Forms.Form
	{
        System.ComponentModel.ITypeDescriptorContext context;
		CodeModelEditor.BrowseRoot root;
        CodeModelEditor.BrowseKind kind;
        ICodeModelEditorFilter filter;
        CodeElement customRoot;
        Hashtable filterState;
        bool onlyUserCode;

		#region Designer stuff
		private System.Windows.Forms.ImageList imgIcons;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.OpenFileDialog dlgOpenAssembly;
		private System.Windows.Forms.ToolTip tpTooltip;
        private CheckBox flattenNameSpaces;
		private System.Windows.Forms.TreeView tvBrowser;


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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeModelEditorForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Base Class", 2, 2);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Interface", 3, 3);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Base Types", 5, 5, new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Event");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("member");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("param");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Method", new System.Windows.Forms.TreeNode[] {
            treeNode6});
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Property");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Class", 2, 2, new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode7,
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Delegate");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Enum", 4, 4);
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Interface", 3, 3);
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Namespace", 1, 1, new System.Windows.Forms.TreeNode[] {
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12});
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Assembly", 0, 0, new System.Windows.Forms.TreeNode[] {
            treeNode13});
            this.imgIcons = new System.Windows.Forms.ImageList(this.components);
            this.tvBrowser = new System.Windows.Forms.TreeView();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.flattenNameSpaces = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.dlgOpenAssembly = new System.Windows.Forms.OpenFileDialog();
            this.tpTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgIcons
            // 
            this.imgIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgIcons.ImageStream")));
            this.imgIcons.Images.SetKeyName(0, "Module");
            this.imgIcons.Images.SetKeyName(1, "Namespace");
            this.imgIcons.Images.SetKeyName(2, "Class");
            this.imgIcons.Images.SetKeyName(3, "Interface");
            this.imgIcons.Images.SetKeyName(4, "Enum");
            this.imgIcons.Images.SetKeyName(5, "BaseTypes");
            this.imgIcons.Images.SetKeyName(6, "Parameter");
            this.imgIcons.Images.SetKeyName(7, "Member");
            this.imgIcons.Images.SetKeyName(8, "EnumMember");
            this.imgIcons.Images.SetKeyName(9, "Method");
            this.imgIcons.Images.SetKeyName(10, "Delegate");
            this.imgIcons.Images.SetKeyName(11, "Event");
            this.imgIcons.Images.SetKeyName(12, "Property");
            // 
            // tvBrowser
            // 
            this.tvBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvBrowser.ImageIndex = 0;
            this.tvBrowser.ImageList = this.imgIcons;
            this.tvBrowser.Location = new System.Drawing.Point(0, 0);
            this.tvBrowser.Name = "tvBrowser";
            treeNode1.ImageIndex = 2;
            treeNode1.Name = "Interface";
            treeNode1.SelectedImageIndex = 2;
            treeNode1.Text = "Base Class";
            treeNode2.ImageIndex = 3;
            treeNode2.Name = "Node7";
            treeNode2.SelectedImageIndex = 3;
            treeNode2.Text = "Interface";
            treeNode3.ImageIndex = 5;
            treeNode3.Name = "Base Types";
            treeNode3.SelectedImageIndex = 5;
            treeNode3.Text = "Base Types";
            treeNode4.ImageKey = "Event";
            treeNode4.Name = "EVent";
            treeNode4.SelectedImageKey = "Event";
            treeNode4.Text = "Event";
            treeNode5.ImageKey = "Member";
            treeNode5.Name = "Node1";
            treeNode5.SelectedImageKey = "Member";
            treeNode5.Text = "member";
            treeNode6.ImageKey = "Parameter";
            treeNode6.Name = "Node0";
            treeNode6.SelectedImageKey = "Parameter";
            treeNode6.Text = "param";
            treeNode7.ImageKey = "Method";
            treeNode7.Name = "Node0";
            treeNode7.SelectedImageKey = "Method";
            treeNode7.Text = "Method";
            treeNode8.ImageKey = "Property";
            treeNode8.Name = "Node2";
            treeNode8.SelectedImageKey = "Property";
            treeNode8.Text = "Property";
            treeNode9.ImageIndex = 2;
            treeNode9.Name = "";
            treeNode9.SelectedImageIndex = 2;
            treeNode9.Text = "Class";
            treeNode10.ImageKey = "Delegate";
            treeNode10.Name = "Delegate";
            treeNode10.SelectedImageKey = "Delegate";
            treeNode10.Text = "Delegate";
            treeNode11.ImageIndex = 4;
            treeNode11.Name = "Enum";
            treeNode11.SelectedImageIndex = 4;
            treeNode11.Text = "Enum";
            treeNode12.ImageIndex = 3;
            treeNode12.Name = "Node6";
            treeNode12.SelectedImageIndex = 3;
            treeNode12.Text = "Interface";
            treeNode13.ImageIndex = 1;
            treeNode13.Name = "";
            treeNode13.SelectedImageIndex = 1;
            treeNode13.Text = "Namespace";
            treeNode14.ImageIndex = 0;
            treeNode14.Name = "";
            treeNode14.SelectedImageIndex = 0;
            treeNode14.Text = "Assembly";
            this.tvBrowser.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode14});
            this.tvBrowser.SelectedImageIndex = 0;
            this.tvBrowser.Size = new System.Drawing.Size(440, 462);
            this.tvBrowser.Sorted = true;
            this.tvBrowser.TabIndex = 0;
            this.tvBrowser.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.OnBeforeExpand);
            this.tvBrowser.DoubleClick += new System.EventHandler(this.tvBrowser_DoubleClick);
            this.tvBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterSelect);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.flattenNameSpaces);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 462);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(440, 34);
            this.pnlButtons.TabIndex = 1;
            // 
            // flattenNameSpaces
            // 
            this.flattenNameSpaces.AutoSize = true;
            this.flattenNameSpaces.Location = new System.Drawing.Point(12, 6);
            this.flattenNameSpaces.Name = "flattenNameSpaces";
            this.flattenNameSpaces.Size = new System.Drawing.Size(119, 17);
            this.flattenNameSpaces.TabIndex = 2;
            this.flattenNameSpaces.Text = "Flatten Namespaces";
            this.flattenNameSpaces.CheckedChanged += new System.EventHandler(this.flattenNameSpaces_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(362, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(282, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            // 
            // dlgOpenAssembly
            // 
            this.dlgOpenAssembly.DefaultExt = "dll";
            this.dlgOpenAssembly.Filter = "Assemblies|*.dll;*.exe";
            this.dlgOpenAssembly.Title = "Open assembly";
            // 
            // CodeModelEditorForm
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(440, 496);
            this.Controls.Add(this.tvBrowser);
            this.Controls.Add(this.pnlButtons);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CodeModelEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Code Model Editor";
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#endregion

		#region Ctor

        /// <summary>
        /// Provides a UI for the <see cref="CodeModelEditor"/>
        /// </summary>
        public CodeModelEditorForm(
            System.ComponentModel.ITypeDescriptorContext context,
            CodeModelEditor.BrowseRoot root, 
            CodeModelEditor.BrowseKind kind, 
            ICodeModelEditorFilter filter, 
            CodeElement customRoot,
            bool onlyUserCode)
            : this()
        {
            this.context = context;
            this.customRoot = customRoot;
			this.root = root;
            this.kind = kind;
            this.filter = filter;
            this.filterState = new Hashtable();
            this.onlyUserCode = onlyUserCode;
        }

		/// <summary>
		/// Provides a UI for the <see cref="CodeModelEditor"/>
		/// </summary>
		public CodeModelEditorForm()
		{
			InitializeComponent();
		}

		#endregion Ctor

		#region Protected methods

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			LoadRootNodes();
		}

		#endregion Protected methods

        #region Private Implementation

        private CodeElements GetChilds(CodeElement codeElement)
        {
            if ((GetBrowseKind(codeElement) & this.kind) == this.kind)
            {
                return null;
            }
            return new CodeElementEx(codeElement).Members;
        }

        private bool Filter(CodeElement codeElement)
        {
            if (!CanBrowse(GetBrowseKind(codeElement)))
            {
                return true;
            }
            CodeElements children = GetChilds(codeElement);
            if ( children != null )
            {
                bool hasValidChildren = false;
                if (children != null)
                {
                    foreach (CodeElement child in children)
                    {
                        if (!Filter(child))
                        {
                            hasValidChildren = true;
                            break;
                        }
                    }
                }
                if (!hasValidChildren)
                {
                    return true;
                }
            }
            if (!filterState.ContainsKey(codeElement))
            {
                bool filterResult = !CanBrowse(GetBrowseKind(codeElement));
                if (!filterResult && filter != null)
                {
                    try
                    {
                        filterResult = filter.Filter(codeElement);
                    }
                    catch
                    {
                        filterResult = false;
                    }
                }
                filterState.Add(codeElement, filterResult);
            }
            return (bool)filterState[codeElement];
        }

        private void LoadProject(Project prj)
        {
            if (prj.Object is EnvDTE80.SolutionFolder)
            {
                foreach (ProjectItem prjItem in prj.ProjectItems)
                {
                    if (prjItem.Object is Project)
                    {
                        LoadProject(((Project)prjItem.Object));
                    }
                }
                // Ignore solution folders
                return;
            }
            if (prj.CodeModel == null)
            {
                // Ignore project that do not support the code model
                return;
            }
            AddNodeWithChilds(this.tvBrowser.Nodes,prj);
        }

        private void AddNodeWithChilds(TreeNodeCollection parent, object obj)
        {
            TreeNode node = null;
            if (obj is Project)
            {
                node = new ProjectNode((Project)obj);
            }
            else if (obj is CodeNamespace)
            {
                node = new NamespaceNode(((CodeNamespace)obj).FullName,this);
            }
            else if (obj is CodeClass)
            {
                node = new ClassNode((CodeClass)obj,this);
            }
            else
            {
                throw new InvalidOperationException("Invalid root type");
            }
            AddNodeWithChilds(parent, node);
        }

        private void AddNodeWithChilds(TreeNodeCollection parent,TreeNode node)
        {
            // So that the + sign appears.
            TreeNode mockNode = new TreeNode("mock");
            mockNode.Tag = MockMarker;
            node.Nodes.Add(mockNode);
            parent.Add(node);
        }

        private void LoadRootNodes()
        {
            tvBrowser.Nodes.Clear();
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                DTE vs = (DTE)GetService(typeof(DTE));
                if (root == CodeModelEditor.BrowseRoot.Solution)
                {
                    Projects projects = (Projects)vs.Solution.Projects;
                    foreach (Project prj in projects)
                    {
                        LoadProject(prj);
                    }
                }
                else if (root == CodeModelEditor.BrowseRoot.SelectedProjects)
                {
                    foreach (SelectedItem selection in vs.SelectedItems)
                    {
                        if (selection.Project != null)
                        {
                            LoadProject(selection.Project);
                        }
                        else if (selection.ProjectItem != null)
                        {
                            LoadProject(selection.ProjectItem.ContainingProject);
                        }
                    }
                }
                else if (root == CodeModelEditor.BrowseRoot.CustomRoot)
                {
                    LoadCustomRoot();
                }
            }
            catch (Exception e)
            {
                tvBrowser.Nodes.Clear();
                throw e;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void LoadCustomRoot()
        {
            if (customRoot == null)
            {
                throw new InvalidOperationException("Custom root not specified or not found.");
            }
            AddNodeWithChilds(tvBrowser.Nodes, customRoot);
        }

        private object MockMarker = new object();

        private bool HasToCreateChilds(TreeNode node)
        {
            return (node.Nodes.Count == 1 && node.Nodes[0].Tag == MockMarker);
        }

        #endregion

        #region Event handlers

        private bool NameSpaceIsEmpty(CodeNamespace cmNamespace)
        {
            foreach (CodeElement codeElement in cmNamespace.Members)
            {
                if (!(codeElement is CodeNamespace))
                {
                    return false;        
                }
                return NameSpaceIsEmpty((CodeNamespace)codeElement);
            }
            return true;
        }

        private void AddNamespace(TreeNode parentNode, CodeNamespace cmNamespace)
        {
            if ( Filter((CodeElement)cmNamespace))
            {
                return;
            }
            TreeNode namespaceNode = null;
            String name = cmNamespace.Name;
            if (flattenNameSpaces.Checked)
            {
                name = cmNamespace.FullName;
            }
            TreeNode[] namespaceNodes = parentNode.Nodes.Find(name, false);
            if (namespaceNodes.Length == 1)
            {
                namespaceNode = namespaceNodes[0];
            }
            else
            {
                namespaceNode = new NamespaceNode(name,this);
                AddNodeWithChilds(parentNode.Nodes, namespaceNode);
            }
            ((List<CodeNamespace>)namespaceNode.Tag).Add(cmNamespace);
        }

        private void BeforeExpand(TreeNode parentNode, CodeNamespace cmNamespace)
        {
            if ( Filter((CodeElement)cmNamespace))
            {
                return;
            }
            if (NameSpaceIsEmpty(cmNamespace))
            {
                return;
            }
            if (flattenNameSpaces.Checked)
            {
                if (parentNode is NamespaceNode)
                {
                    // If we have flatten namespaces then don't expand child namespaces
                    return;
                }
                List<CodeNamespace> nameSpaces = new List<CodeNamespace>();
                GetAllNamespaces(cmNamespace,ref nameSpaces);
                foreach (CodeNamespace nameSpace in nameSpaces)
                {
                    AddNamespace(parentNode, nameSpace);
                }
            }
            else
            {
                AddNamespace(parentNode, cmNamespace);
            }
        }

        private void GetAllNamespaces(CodeNamespace cmNamespace,ref List<CodeNamespace> nameSpaces)
        {
            bool insert = false;
            foreach (CodeElement codeElement in cmNamespace.Members)
            {
                if (codeElement is CodeNamespace)
                {
                    CodeNamespace subSpace = (CodeNamespace)codeElement;
                    GetAllNamespaces(subSpace, ref nameSpaces);
                }
                else
                {
                    // If the namespace constains other things other than just namespaces then inserted
                    insert = true;
                }
            }
            if (insert)
            {
                nameSpaces.Add(cmNamespace);
            }
        }

        private const string WebProjectKind = "{E24C65DC-7377-472B-9ABA-BC803B73C61A}";

        private void Expand(TreeNode parentNode, ProjectItem prItem)
        {
            ProjectItems childItems = prItem.ProjectItems;
            if (childItems != null)
            {
                foreach (ProjectItem subItem in childItems)
                {
                    Expand(parentNode,subItem);
                }
            }
            if (prItem.FileCodeModel != null)
            {
                foreach (CodeElement codeElement in prItem.FileCodeModel.CodeElements)
                {
                    /*if (codeElement.InfoLocation != vsCMInfoLocation.vsCMInfoLocationProject)
                    {
                        continue;
                    }*/
                    BeforeExpand(parentNode,codeElement);
                }
            }
        }

        private void Expand(TreeNode parentNode,Project prj)
        {
            if (onlyUserCode)
            {
                foreach (ProjectItem prItem in prj.ProjectItems)
                {
                    Expand(parentNode, prItem);
                }
            }
            else
            {
                foreach (CodeElement codeElement in prj.CodeModel.CodeElements)
                {
                    BeforeExpand(parentNode, codeElement);
                }
            }
        }

        private void Expand(TreeNode parentNode, List<CodeNamespace> namepaces)
        {
            foreach (CodeNamespace codeNamepace in namepaces)
            {
                foreach (CodeElement codeElement in codeNamepace.Members)
                {
                    BeforeExpand(parentNode, codeElement);
                }
            }
        }

        private void Expand(TreeNode parentNode, CodeFunction codeFunction)
        {
            foreach (CodeElement codeElement in codeFunction.Parameters)
            {
                BeforeExpand(parentNode, codeElement);
            }
        }

        private void Expand(TreeNode parentNode, CodeClass codeClass)
        {
            foreach(CodeElement codeElement in codeClass.Members)
            {
                BeforeExpand(parentNode, codeElement);
            }
        }

        private void BeforeExpand(TreeNode parentNode, CodeVariable codeVariable)
        {
            MemberNode cv = new MemberNode(codeVariable, this);
            parentNode.Nodes.Add(cv);
        }

        private void BeforeExpand(TreeNode parentNode, CodeProperty codeProperty)
        {
            PropertyNode cp = new PropertyNode(codeProperty, this);
            parentNode.Nodes.Add(cp);
        }

        private void BeforeExpand(TreeNode parentNode, CodeEvent codeEvent)
        {
            EventNode ce = new EventNode(codeEvent, this);
            parentNode.Nodes.Add(ce);
        }

        private void BeforeExpand(TreeNode parentNode, CodeFunction codeFunction)
        {
            MethodNode cn = new MethodNode(codeFunction, this);
            parentNode.Nodes.Add(cn);
        }

        private void BeforeExpand(TreeNode parentNode, CodeClass codeClass)
        {
            ClassNode cn = new ClassNode(codeClass, this);
            CodeElements children=GetChilds((CodeElement)codeClass);
            if (children != null && children.Count > 0)
            {
                AddNodeWithChilds(parentNode.Nodes, cn);
            }
            else
            {
                parentNode.Nodes.Add(cn);
            }
        }

        private void BeforeExpand(TreeNode parentNode, CodeInterface codeInterface)
        {
            InterfaceNode cn = new InterfaceNode(codeInterface, this);
            if (codeInterface.Members.Count > 0)
            {
                AddNodeWithChilds(parentNode.Nodes, cn);
            }
            else
            {
                parentNode.Nodes.Add(cn);
            }
        }

        private void BeforeExpand(TreeNode parentNode, CodeEnum codeEnum)
        {
            EnumNode cn = new EnumNode(codeEnum, this);
            if (codeEnum.Members.Count > 0)
            {
                AddNodeWithChilds(parentNode.Nodes, cn);
            }
            else
            {
                parentNode.Nodes.Add(cn);
            }
        }

        private void BeforeExpand(TreeNode parentNode, CodeDelegate codeDelegate)
        {
            DelegateNode dn = new DelegateNode(codeDelegate, this);
            parentNode.Nodes.Add(dn);
        }

        bool CanBrowse(CodeModelEditor.BrowseKind kind)
        {
            if (kind == CodeModelEditor.BrowseKind.Namespace)
            {
                return true;
            }
            return ((this.kind & kind) != 0);
        }

        private static CodeModelEditor.BrowseKind GetBrowseKind(CodeElement codeElement)
        {
            if (codeElement is CodeClass )
            {
                return CodeModelEditor.BrowseKind.Class;
            }
            else if (codeElement is CodeInterface )
            {
                return CodeModelEditor.BrowseKind.Interface;
            }
            else if ( codeElement is CodeEnum )
            {
                return CodeModelEditor.BrowseKind.Enum;
            }
            else if ( codeElement is CodeDelegate )
            {
                return CodeModelEditor.BrowseKind.Delegate;
            }
            else if (codeElement is CodeFunction )
            {
                return CodeModelEditor.BrowseKind.Function;
            }
            else if (codeElement is CodeVariable)
            {
                return CodeModelEditor.BrowseKind.Variable;
            }
            else if (codeElement is CodeEvent)
            {
                return CodeModelEditor.BrowseKind.Event;
            }
            else if (codeElement is CodeProperty)
            {
                return CodeModelEditor.BrowseKind.Prop;
            }
            else if (codeElement is CodeNamespace)
            {
                return CodeModelEditor.BrowseKind.Namespace;
            }
            return CodeModelEditor.BrowseKind.None;
        }

        private void BeforeExpand(TreeNode parentNode, CodeElement codeElement)
        {
            if ( Filter(codeElement))
            {
                return;
            }
            if (codeElement is CodeClass)
            {
                BeforeExpand(parentNode, (CodeClass)codeElement);
            }
            else if (codeElement is CodeInterface)
            {
                BeforeExpand(parentNode, (CodeInterface)codeElement);
            }
            else if (codeElement is CodeEnum)
            {
                BeforeExpand(parentNode, (CodeEnum)codeElement);
            }
            else if (codeElement is CodeDelegate)
            {
                BeforeExpand(parentNode, (CodeDelegate)codeElement);
            }
            else if (codeElement is CodeFunction)
            {
                BeforeExpand(parentNode, (CodeFunction)codeElement);
            }
            else if (codeElement is CodeVariable)
            {
                BeforeExpand(parentNode, (CodeVariable)codeElement);
            }
            else if (codeElement is CodeEvent)
            {
                BeforeExpand(parentNode, (CodeEvent)codeElement);
            }
            else if (codeElement is CodeProperty)
            {
                BeforeExpand(parentNode, (CodeProperty)codeElement);
            }
            else if (codeElement is CodeNamespace)
            {
                BeforeExpand(parentNode, (CodeNamespace)codeElement);
            }
        }

        private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
            if (!HasToCreateChilds(e.Node))
            {
                return;
            }
			tvBrowser.SuspendLayout();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Clear the previous "mock" node
                TreeNode parentNode = e.Node;
                parentNode.Nodes.Clear();
                object tag = e.Node.Tag;
                if (tag == null)
                {
                    return;
                }
    			else if ( tag is Project )
	    		{
                    Expand(parentNode, (Project)tag);
                }
                else if (tag is List<CodeNamespace>)
                {
                    Expand(parentNode, (List<CodeNamespace>)tag);
                }
                else if (tag is CodeClass)
                {
                    Expand(parentNode, (CodeClass)tag);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                tvBrowser.ResumeLayout();
            }
		}

		private void tvBrowser_DoubleClick(object sender, System.EventArgs e)
		{
            this.btnOK.PerformClick();
		}

        private void flattenNameSpaces_CheckedChanged(object sender, EventArgs e)
        {
            LoadRootNodes();
        }

        private void OnAfterSelect(object sender, TreeViewEventArgs e)
        {
            if ( e.Node is CodeModelNode && ((CodeModelNode)e.Node).Kind==kind ) 
            {
                this.btnOK.Enabled = true;
            }
            else
            {
                this.btnOK.Enabled = false;
            }
        }

		#endregion Event handlers

		#region Properties

		/// <summary>
		/// Exposes the selected class.
		/// </summary>
		internal object SelectedObject
		{
            get
            {
                if ( tvBrowser.SelectedNode == null ||
                     tvBrowser.SelectedNode.Tag == null )
                {
                    return null;
                }
                return tvBrowser.SelectedNode.Tag;
            }
		}

		#endregion Properties

		#region Derived nodes

		class ProjectNode : TreeNode
		{
            public ProjectNode(Project project)
			{
                try
                {
                    base.Text = project.Properties.Item("AssemblyName").Value.ToString();
                }
                catch
                {
                    base.Text = project.Name;
                }
				base.Tag = project;
				base.ImageIndex = 0;
				base.SelectedImageIndex = 0;
			}
		}

        class CodeModelNode : TreeNode
        {
            CodeModelEditorForm parentForm = null;

            public CodeModelNode(CodeModelEditor.BrowseKind kind, CodeModelEditorForm parentForm)
            {
                this.kind = kind;
                this.parentForm = parentForm;
            }

            public CodeModelEditor.BrowseKind Kind
            {
                get { return kind; }
            } CodeModelEditor.BrowseKind kind;

            protected void SetText(string text)
            {
                string newName = null;
                if (this.Kind != CodeModelEditor.BrowseKind.Namespace)
                {
                    try
                    {
                        TypeConverter typeConverter = null;
                        if (parentForm.context.Instance is ValueEditor)
                        {
                            typeConverter = ((ValueEditor)parentForm.context.Instance).ConverterInstance;
                        }
                        if (typeConverter != null)
                        {
                            newName = (string)typeConverter.ConvertTo(
                                parentForm.context,
                                CultureInfo.CurrentCulture,
                                this.Tag,
                                typeof(string));
                        }
                    }
                    catch
                    {
                        newName = text;
                    }
                }
                if (string.IsNullOrEmpty(newName))
                {
                    newName = text;
                }
                if (!string.IsNullOrEmpty(newName))
                {
                    this.Text = newName;
                }
            }
        }

        class NamespaceNode : CodeModelNode
        {
            public NamespaceNode(string Name, CodeModelEditorForm context)
                : base(CodeModelEditor.BrowseKind.Namespace,context)
            {
                base.Name = Name;
                SetText(Name);
                base.Tag = new List<CodeNamespace>();
                base.ImageKey = "Namespace";
                base.SelectedImageKey = "Namespace";
            }
        }

        class ClassNode : CodeModelNode
		{
            public ClassNode(CodeClass cc, CodeModelEditorForm context)
                : base(CodeModelEditor.BrowseKind.Class,context)
			{
                base.Tag = cc;
                base.ImageKey = "Class";
                base.SelectedImageKey = "Class";
                SetText(cc.Name);
            }
		}

        class FunctionNode : CodeModelNode
        {
            public FunctionNode(CodeFunction cf, CodeModelEditor.BrowseKind kind, CodeModelEditorForm context)
                : base(kind | CodeModelEditor.BrowseKind.Function,context)
            {
                StringBuilder sb = new StringBuilder();
                if (cf.FunctionKind == vsCMFunction.vsCMFunctionConstructor)
                {
                    sb.Append(".ctor");
                }
                else if (cf.FunctionKind == vsCMFunction.vsCMFunctionDestructor)
                {
                    sb.Append(".dtor");
                }
                else
                {
                    sb.Append(cf.Name);
                }
                sb.Append("(");
                int iParam=0;
                foreach (CodeParameter codeParameter in cf.Parameters)
                {
                    iParam++;
                    sb.Append(codeParameter.Type.AsString);
                    if (iParam != cf.Parameters.Count)
                    {
                        sb.Append(",");
                    }
                }
                sb.Append(")");
                base.Text = sb.ToString();
                base.Tag = cf;
                base.ImageKey = "Method";
                base.SelectedImageKey = "Method";
            }
        }

        class MethodNode : FunctionNode
        {
            public MethodNode(CodeFunction cf, CodeModelEditorForm context)
                : base(cf,CodeModelEditor.BrowseKind.Class,context)
            {
            }
        }

        class InterfaceNode : CodeModelNode
		{
            public InterfaceNode(CodeInterface ci, CodeModelEditorForm context)
                : base(CodeModelEditor.BrowseKind.Interface,context)
			{
                base.Tag = ci;
                base.ImageKey = "Interface";
                base.SelectedImageKey = "Interface";
                SetText(ci.Name);
            }
		}

        class EnumNode : CodeModelNode
		{
            public EnumNode(CodeEnum ce, CodeModelEditorForm context)
                : base(CodeModelEditor.BrowseKind.Enum,context)
			{
                base.Tag = ce;
                base.ImageKey = "Enum";
                base.SelectedImageKey = "Enum";
                SetText(ce.Name);
            }
		}

        class DelegateNode : CodeModelNode
        {
            public DelegateNode(CodeDelegate cd, CodeModelEditorForm context)
                : base(CodeModelEditor.BrowseKind.Delegate,context)
            {
                base.Tag = cd;
                base.ImageKey = "Delegate";
                base.SelectedImageKey = "Delegate";
                SetText(cd.Name);
            }
        }

        class MemberNode : CodeModelNode
        {
            public MemberNode(CodeVariable cv, CodeModelEditorForm context)
                : base(CodeModelEditor.BrowseKind.ClassMember,context)
            {
                base.Tag = cv;
                base.ImageKey = "Member";
                base.SelectedImageKey = "Member";
                SetText(cv.Name);
            }
        }

        class EventNode : CodeModelNode
        {
            public EventNode(CodeEvent ce, CodeModelEditorForm context)
                : base(CodeModelEditor.BrowseKind.ClassEvent,context)
            {
                base.Tag = ce;
                base.ImageKey = "Event";
                base.SelectedImageKey = "Event";
                SetText(ce.Name);
            }
        }

        class PropertyNode : CodeModelNode
        {
            public PropertyNode(CodeProperty cp, CodeModelEditorForm context)
                : base(CodeModelEditor.BrowseKind.ClassProperty, context)
            {
                base.Tag = cp;
                base.ImageKey = "Property";
                base.SelectedImageKey = "Property";
                SetText(cp.Name);
            }
        }

		#endregion Derived nodes

	}
}
