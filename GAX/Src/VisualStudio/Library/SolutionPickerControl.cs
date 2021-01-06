#region Using

using System;
using System.Drawing;
using System.Windows.Forms;
using EnvDTE;
using System.Collections;
using System.IO;
using VSLangProj;
using System.Windows.Forms.Design;
using Microsoft.Practices.ComponentModel;

#endregion Using

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library
{
	/// <summary>
	/// User control that allows selection of a valid target given a <see cref="IUnboundAssetReference"/> or 
	/// <see cref="Type"/> of the target.
	/// </summary>
	internal partial class SolutionPickerControl : UserControl
	{
        DTE dte;
        IUnboundAssetReference reference;
        Type valueType;
        Hashtable icons = new Hashtable();
        // Whether the current selection is valid.
        bool validSelection = false;
        // The initially selected element.
        object originalSelection;

        /// <summary>
        /// Empty constructor for design-time support.
        /// </summary>
        public SolutionPickerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event rised whenever the user selects a new node in the tree.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        /// <summary>
        /// Initializes the control receiving the DTE, reference and current value 
        /// to customize the behavior of the control.
        /// </summary>
        /// <param name="dte">Reference to the Visual Studio environment.</param>
        /// <param name="reference">The unbound reference used to determine whether the current node is valid.</param>
        /// <param name="currentValue">The current value to preselect. Can be <see langword="null"/>.</param>
        /// <param name="valueType">The type of the element to pick.</param>
        /// <exception cref="ArgumentNullException">Either <paramref name="dte"/> or <paramref name="reference"/> are <see langword="null"/>.</exception>
        /// <remarks>
        /// If <paramref name="currentValue"/> is not valid for the given <paramref name="reference"/>, the value 
        /// is ignored and not pre-selected.
        /// If a <paramref name="reference"/> is not provided, then the <paramref name="valueType"/> is used to 
        /// determine a valid target. Note at least one of them has to have a non <see langword="null"/> value.
        /// </remarks>
        public SolutionPickerControl(DTE dte, IUnboundAssetReference reference, object currentValue, Type valueType)
		{
            Initialize(dte, reference, currentValue, valueType);
		}

        void Initialize(DTE dte, IUnboundAssetReference reference, object currentValue, Type valueType)
        {
            if (dte == null)
            {
                throw new ArgumentNullException("dte");
            }
            if (reference == null && valueType == null)
            {
                throw new ArgumentNullException("reference, valueType",
                    Properties.Resources.SolutionPicker_NullRefAndType);
            }
            this.dte = dte;
            this.reference = reference;
            this.valueType = valueType;
            this.originalSelection = currentValue;

			try
			{
				if (reference != null && !reference.IsEnabledFor(currentValue))
				{
					this.originalSelection = null;
				}
			}
			catch (Exception e)
			{
				throw new RecipeFrameworkException(
					Properties.Resources.Reference_FailEnabledFor, e);
			}
			InitializeComponent();
            this.SuspendLayout();
            LoadElements();
            this.ResumeLayout(false);
        }

        /// <summary>
        /// <seealso cref="Control.OnVisibleChanged"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (this.Visible)
            {
                // Enable events after being visible in case a pre-selection is made.
                this.solutionTree.AfterSelect -= new TreeViewEventHandler(this.solutionTree_AfterSelect);
                this.solutionTree.AfterSelect += new TreeViewEventHandler(this.solutionTree_AfterSelect);
            }
        }

        /// <summary>
        /// Gets the target selected in the treeview.
        /// </summary>
        public object SelectedTarget
        {
            get 
            { 
                return this.solutionTree.SelectedNode == null ?
                    null : this.solutionTree.SelectedNode.Tag; 
            }
        }

        #region Determine icons

        private void SetIconIndexes(ProjectItem item, TreeNode node)
        {
            switch (item.Kind)
            {
                case Constants.vsProjectItemKindPhysicalFolder:
                    // UNDONE: icon resources could be pulled from VS.
                    if (item.Name == "Configuration" || item.Name == "Properties")
                    {
                        node.ImageIndex = 34;
                        node.SelectedImageIndex = 2;
                    }
                    else if (item.Name == "Web References")
                    {
                        node.ImageIndex = 37;
                        node.SelectedImageIndex = 2;
                    }
                    else if (HasProperty(item.Properties, "WebReference"))
                    {
                        node.ImageIndex = 36;
                        node.SelectedImageIndex = 36;
                    }
                    else
                    {
                        node.ImageIndex = 3;
                        node.SelectedImageIndex = 4;
                    }
                    break;
                case Constants.vsProjectItemKindPhysicalFile:
                    try
                    {
                        // Try to get the dependent property.
                        if (item.Properties.Item("IsDependentFile").Value.ToString() == Boolean.TrueString)
                        {
                            node.ImageIndex = 35;
                            node.SelectedImageIndex = 35;
                            break;
                        }
                    }
                    catch { }
                    switch (Path.GetExtension(item.get_FileNames(1)))
                    {
                        case ".ascx":
                            node.ImageIndex = 5;
                            node.SelectedImageIndex = 5;
                            break;
                        case ".asmx":
                            node.ImageIndex = 6;
                            node.SelectedImageIndex = 6;
                            break;
                        case ".aspx":
                            node.ImageIndex = 7;
                            node.SelectedImageIndex = 7;
                            break;
                        case ".bmp":
                        case ".png":
                        case ".gif":
                        case ".jpg":
                            node.ImageIndex = 8;
                            node.SelectedImageIndex = 8;
                            break;
                        case ".config":
                            node.ImageIndex = 9;
                            node.SelectedImageIndex = 9;
                            break;
                        case ".cs":
                            node.ImageIndex = 11;
                            node.SelectedImageIndex = 11;
                            break;
                        case ".disco":
                            node.ImageIndex = 14;
                            node.SelectedImageIndex = 14;
                            break;
                        case ".html":
                        case ".htm":
                            node.ImageIndex = 15;
                            node.SelectedImageIndex = 15;
                            break;
                        case ".ico":
                            node.ImageIndex = 16;
                            node.SelectedImageIndex = 16;
                            break;
                        case ".js":
                            node.ImageIndex = 18;
                            node.SelectedImageIndex = 18;
                            break;
                        case ".resx":
                            node.ImageIndex = 21;
                            node.SelectedImageIndex = 21;
                            break;
                        case ".wsdl":
                            node.ImageIndex = 23;
                            node.SelectedImageIndex = 23;
                            break;
                        case ".settings":
                            node.ImageIndex = 24;
                            node.SelectedImageIndex = 24;
                            break;
                        case ".css":
                            node.ImageIndex = 25;
                            node.SelectedImageIndex = 25;
                            break;
                        case ".txt":
                            node.ImageIndex = 26;
                            node.SelectedImageIndex = 26;
                            break;
                        case ".vbs":
                            node.ImageIndex = 28;
                            node.SelectedImageIndex = 28;
                            break;
                        case ".wsf":
                            node.ImageIndex = 30;
                            node.SelectedImageIndex = 30;
                            break;
                        case ".xml":
                            node.ImageIndex = 31;
                            node.SelectedImageIndex = 31;
                            break;
                        case ".xsd":
                            node.ImageIndex = 32;
                            node.SelectedImageIndex = 32;
                            break;
                        case ".xsl":
                            node.ImageIndex = 33;
                            node.SelectedImageIndex = 33;
                            break;
                        default:
                            node.ImageIndex = 17;
                            node.SelectedImageIndex = 17;
                            break;

                    }
                    break;
                default:
                    node.ImageIndex = 11;
                    node.SelectedImageIndex = 11;
                    break;
            }
        }

        private void SetIconIndexes(Project project, TreeNode node)
        {
            switch (project.Kind)
            {
                case VSLangProj.PrjKind.prjKindCSharpProject:
                    // BACKPORT: property to check should be "WebServerVersion".
                    if (HasProperty(project.Properties, "CurrentWebsiteLanguage"))
                    {
                        node.ImageIndex = 13;
                        node.SelectedImageIndex = 13;
                    }
                    else
                    {
                        node.ImageIndex = 12;
                        node.SelectedImageIndex = 12;
                    }
                    break;
                case VSLangProj.PrjKind.prjKindVBProject:
                    // BACKPORT: property to check should be "WebServerVersion".
                    if (HasProperty(project.Properties, "CurrentWebsiteLanguage"))
                    {
                        node.ImageIndex = 29;
                        node.SelectedImageIndex = 29;
                    }
                    else
                    {
                        node.ImageIndex = 27;
                        node.SelectedImageIndex = 27;
                    }
                    break;
                case Constants.vsProjectKindSolutionItems:
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 2;
                    break;
                default:
                    node.ImageIndex = 19;
                    node.SelectedImageIndex = 19;
                    break;
            }
        }

        #endregion Determine icons

        internal void LoadElements()
        {
            try
            {
                TreeNode parent;
                this.solutionTree.Nodes.Clear();
                parent = new TreeNode(dte.Solution.Properties.Item("Name").Value.ToString());
                parent.SelectedImageIndex = 0;
                parent.ImageIndex = 0;
                this.solutionTree.Nodes.Add(parent);
                parent.Tag = dte.Solution;

                IsValidTarget(parent);

                foreach (Project prj in dte.Solution.Projects)
                {
                    LoadProject(prj, parent);
                }
                parent.Expand();
            }
            catch (Exception ex)
            {
				ErrorHelper.Show(this.Site, ex);
            }
        }

        private void IsValidTarget(TreeNode treeNode)
        {
            bool isValid = false;
            try
		    {
			    isValid = reference.IsEnabledFor(treeNode.Tag);
		    }
            catch(Exception)
            {
                // If the reference thrown any kind of exception we'll assume that the node is not valid.
            }

            if (!isValid)
            {
                treeNode.ForeColor = SystemColors.GrayText;
            }
            else
            {
                treeNode.ForeColor = SystemColors.WindowText;
                if (treeNode.Parent != null)
                {
                    treeNode.Parent.Expand();
                }
            }
        }


        private void LoadProject(Project project, TreeNode parent)
        {
            TreeNode projectnode = new TreeNode(project.Name);
            parent.Nodes.Add(projectnode);
            SetIconIndexes(project, projectnode);
            if (project.Object is EnvDTE80.SolutionFolder)
            {
                projectnode.Tag = project.Object;
            }
            else
            {
                projectnode.Tag = project;
            }
            if (projectnode.Tag == originalSelection)
            {
                this.solutionTree.SelectedNode = projectnode;
            }

            IsValidTarget(projectnode);

            if (project.Object is VSProject)
            {
                TreeNode refsnode = new TreeNode(Properties.Resources.SolutionPicker_ReferencesNode);
                refsnode.ImageIndex = 22;
                // Equals an opened solution folder.
                refsnode.SelectedImageIndex = 2;
                projectnode.Nodes.Add(refsnode);
                IsValidTarget(refsnode);

                References refs = ((VSProject)project.Object).References;
                foreach (Reference reference in refs)
                {
                    TreeNode refnode = new TreeNode(reference.Name);
                    refsnode.Nodes.Add(refnode);
                    refnode.ImageIndex = 38;
                    refnode.SelectedImageIndex = 38;
                    refnode.Tag = reference;
                    IsValidTarget(refnode);
                }
            }

            LoadItems(project.ProjectItems, projectnode);
        }

        private void LoadItems(ProjectItems items, TreeNode parent)
        {
            if (items == null)
            {
                return;
            }
            foreach (ProjectItem item in items)
            {
                if (item.Object is Project)
                {
                    LoadProject((Project)item.Object, parent);
                }
                else
                {
                    TreeNode node = new TreeNode(item.Name);
                    parent.Nodes.Add(node);
                    SetIconIndexes(item, node);
                    node.Tag = item;
                    if (item == originalSelection)
                    {
                        this.solutionTree.SelectedNode = node;
                    }

                    IsValidTarget(node);

                    LoadItems(item.ProjectItems, node);
                }
            }
        }

        /// <summary>
        /// Helper method to test if <paramref name="name"/> is defined in <paramref name="properties"/>
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasProperty(EnvDTE.Properties properties, string name)
        {
            try
            {
                return properties.Item(name) != null &&
                        properties.Item(name).Value != null &&
                        !string.IsNullOrEmpty(properties.Item(name).Value.ToString());
            }
            catch
            {
                return false;
            }
        }

        private void solutionTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                // May be a node that holds other elements.
                this.validSelection = false;
				if (SelectionChanged != null)
				{
					SelectionChanged(this, new SelectionChangedEventArgs(this.validSelection, e.Node.Tag));
				}
				return;
            }
			try
			{
				if (reference == null)
				{
					// UNDONE: the generic code at the end of the if statements 
					// doesn't work for COM objects. Find an alternative generic way of 
					// determining type compatibility in the future.
					if (this.valueType == typeof(EnvDTE.Solution))
					{
						this.validSelection = e.Node.Tag is EnvDTE.Solution;
					}
					else if (this.valueType == typeof(EnvDTE.Project))
					{
						this.validSelection = e.Node.Tag is EnvDTE.Project;
					}
					else if (this.valueType == typeof(EnvDTE.ProjectItem))
					{
						this.validSelection = e.Node.Tag is EnvDTE.ProjectItem;
					}
					else if (this.valueType == typeof(EnvDTE80.SolutionFolder))
					{
						this.validSelection = e.Node.Tag is EnvDTE80.SolutionFolder;
					}
					else if (this.valueType == typeof(VSLangProj.Reference))
					{
						this.validSelection = e.Node.Tag is VSLangProj.Reference;
					}
					else
					{
						this.validSelection = (this.valueType == e.Node.Tag.GetType() ||
							this.valueType.IsAssignableFrom(e.Node.Tag.GetType()));
					}
				}
				else
				{
					try
					{
						this.validSelection = reference.IsEnabledFor(e.Node.Tag);
					}
					catch (Exception ex)
					{
						throw new RecipeFrameworkException(
							Properties.Resources.Reference_FailEnabledFor, ex);
					}
				}
			}
			catch (Exception ex)
			{
				this.validSelection = false;
				ErrorHelper.Show(this.Site, ex);
			}
			finally
			{
				try
				{
					if (this.validSelection && GetService(typeof(IWindowsFormsEditorService)) != null)
					{
						((IWindowsFormsEditorService)GetService(typeof(IWindowsFormsEditorService))).CloseDropDown();
					}
					if (SelectionChanged != null)
					{
						SelectionChanged(this, new SelectionChangedEventArgs(this.validSelection, e.Node.Tag));
					}
				}
				catch (Exception ex)
				{
					ErrorHelper.Show(this.Site, ex);
				}
			}
        }
    }

    /// <summary>
    /// Argument passed for event <see cref="SolutionPickerControl.SelectionChanged"/>.
    /// </summary>
    public class SelectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor for the SelectionChangedEvent
        /// </summary>
        /// <param name="isValidSelection"></param>
        /// <param name="selection"></param>
        public SelectionChangedEventArgs(bool isValidSelection, object selection)
        {
            this.isvalid = isValidSelection;
            this.selection = selection;
        }

        private bool isvalid;

        /// <summary>
        /// Gets whether the selection is valid for the 
        /// specified <see cref="IUnboundAssetReference"/>.
        /// </summary>
        public bool IsValid
        {
            get { return isvalid; }
        }

        private object selection;

        /// <summary>
        /// Gets the selected target.
        /// </summary>
        public object Selection
        {
            get { return selection; }
        }
    }

    /// <summary>
    /// Delegate for event <see cref="SolutionPickerControl.SelectionChanged"/>.
    /// </summary>
    public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
}
