using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.ToolWindow
{
    /// <summary>
    /// Extendended treeview control
    /// </summary>
    internal class ExtendedTreeView : TreeView
    {
        #region Native Methods
        /// <summary>
        /// Gets the scroll info.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="fnBar">The fn bar.</param>
        /// <param name="si">The si.</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern bool GetScrollInfo(HandleRef hWnd, int fnBar, ScrollInfo si);
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        #endregion

        private int verticalScrollPadding = 0;
        private Font closeButtonFont = new Font("Courrier New", 12, FontStyle.Regular);

        /// <summary>
        /// Occurs when the user click any of close buttons.
        /// </summary>
        public event TreeNodeMouseClickEventHandler NodeClose;

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl"></see> event.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.DrawMode = TreeViewDrawMode.OwnerDrawText;

            if (this.Font.Height > this.ItemHeight)
            {
                this.ItemHeight = this.Font.Height;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.TreeView.DrawNode"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawTreeNodeEventArgs"></see> that contains the event data.</param>
        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            base.OnDrawNode(e);

            ExtendedTreeNode customTreeNode = e.Node as ExtendedTreeNode;
            Font nodeFont = (e.Node.NodeFont != null) ? e.Node.NodeFont : this.Font;
            Color nodeColor = (((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected) && e.Node.TreeView.Focused) ? SystemColors.HighlightText : ((e.Node.ForeColor != Color.Empty) ? e.Node.ForeColor : e.Node.TreeView.ForeColor);

            bool showAsLink = (customTreeNode != null && customTreeNode.ShowAsLink);
            Brush textBrush = SystemBrushes.MenuText;

            if (showAsLink)
            {
                nodeFont = new Font(nodeFont, FontStyle.Underline);
                textBrush = Brushes.Navy;
                nodeColor = Color.Navy;
            }

            // Draw the background and node text for a selected node.
            if ((e.State & TreeNodeStates.Selected) != 0 || (e.State & TreeNodeStates.Focused) != 0)
            {
                Rectangle hoverRectangle = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                e.Graphics.FillRectangle(SystemBrushes.Highlight, hoverRectangle);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, nodeFont, e.Bounds, SystemColors.HighlightText, TextFormatFlags.VerticalCenter);
            }
            // Use the default background and node text.
            else
            {
                e.Graphics.FillRectangle(SystemBrushes.Window, NodeBounds(e.Node));
                TextRenderer.DrawText(e.Graphics, e.Node.Text, nodeFont, e.Bounds, nodeColor, TextFormatFlags.VerticalCenter);
            }

            Rectangle closeButtonRectangle = new Rectangle(e.Bounds.X + e.Bounds.Width, e.Bounds.Y, this.Bounds.Width - (e.Bounds.X + e.Bounds.Width) - verticalScrollPadding, e.Bounds.Height);
            e.Graphics.FillRectangle(SystemBrushes.Window, closeButtonRectangle);

            if (customTreeNode != null && customTreeNode.ShowCloseButton)
            {
                // Fix for more than 96dpi screen resolutions.
                if (SystemFonts.IconTitleFont.Size > closeButtonFont.Size)
                {
                    closeButtonFont = SystemFonts.IconTitleFont;
                }

                TextRenderer.DrawText(e.Graphics, "x", closeButtonFont, closeButtonRectangle, SystemColors.WindowText, TextFormatFlags.Right);
            }

            this.ResumeLayout();
        }

        /// <summary>
        /// Overrides <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)"></see>.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_VSCROLL)
            {
                verticalScrollPadding = SystemInformation.VerticalScrollBarWidth;
            }
        }

        /// <summary>
        /// Gets the close button position of the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="leftPosition">The left position.</param>
        /// <param name="topPosition">The top position.</param>
        /// <param name="closeButtonSize">Size of the close button.</param>
        private void GetCloseButtonPosition(TreeNode node, out int leftPosition, out int topPosition, out SizeF closeButtonSize)
        {
            closeButtonSize = this.CreateGraphics().MeasureString("x", closeButtonFont);
            leftPosition = this.Bounds.Right - (int)closeButtonSize.Width - 5 - verticalScrollPadding;
            topPosition = node.Bounds.Top + this.ItemHeight / 2;
        }
        
        /// <summary>
        /// Returns the bounds of the specified node, including the region occupied by the node label and any node tag displayed.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Rectangle NodeBounds(TreeNode node)
        {
            // Set the return value to the normal node bounds.
            Rectangle bounds = node.Bounds;
            if (node.Tag != null)
            {
                //// Retrieve a Graphics object from the TreeView handle
                //// and use it to calculate the display width of the tag.
                //Graphics graphics = this.CreateGraphics();
                //int tagWidth = (int)graphics.MeasureString(node.Tag.ToString(), tagFont).Width + 6;

                //// Adjust the node bounds using the calculated value.
                //bounds.Offset(tagWidth / 2, 0);
                //bounds = Rectangle.Inflate(bounds, tagWidth / 2, 0);
            }
            return bounds;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (IsOnCloseButton(e) || IsOnLinkeableNode(e))
            {
                Cursor.Current = Cursors.Hand;
            }
            else
            {
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseClick"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            TreeNode selectedNode = base.GetNodeAt(e.X, e.Y);
            ExtendedTreeNode customTreeNode = selectedNode as ExtendedTreeNode;

            if (IsOnCloseButton(e))
            {
                if (this.NodeClose != null)
                {
                    this.NodeClose(this, new TreeNodeMouseClickEventArgs(customTreeNode, e.Button, e.Clicks, e.X, e.Y));
                }
            }
        }

        /// <summary>
        /// Determines whether is mouse is over a linkeable node.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        /// <returns>
        /// 	<c>true</c> if [is on linkeable node] [the specified e]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsOnLinkeableNode(MouseEventArgs e)
        {
            TreeNode selectedNode = base.GetNodeAt(e.X, e.Y);
            ExtendedTreeNode customTreeNode = selectedNode as ExtendedTreeNode;

            if (customTreeNode != null && customTreeNode.ShowAsLink)
            {
                if (e.X > customTreeNode.Bounds.Left && e.X < customTreeNode.Bounds.Right)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether is mouse over a close button.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        /// <returns>
        /// 	<c>true</c> if [is on close button] [the specified e]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsOnCloseButton(MouseEventArgs e)
        {
            TreeNode selectedNode = base.GetNodeAt(e.X, e.Y);
            ExtendedTreeNode customTreeNode = selectedNode as ExtendedTreeNode;

            if (customTreeNode != null && customTreeNode.ShowCloseButton)
            {
                SizeF closeButtonSize;
                int leftPosition;
                int topPosition;
                GetCloseButtonPosition(customTreeNode, out leftPosition, out topPosition, out closeButtonSize);

                if (e.X >= leftPosition && e.X <= leftPosition + (int)closeButtonSize.Width + 5)
                {
                    if (e.Y >= topPosition -5 && e.Y <= topPosition + (int)closeButtonSize.Height - 5)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class ScrollInfo
        {
            public int cbSize;
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:ScrollInfo"/> class.
            /// </summary>
            public ScrollInfo()
            {
                this.cbSize = Marshal.SizeOf(typeof(ScrollInfo));

            }
            /// <summary>
            /// Initializes a new instance of the <see cref="T:ScrollInfo"/> class.
            /// </summary>
            /// <param name="mask">The mask.</param>
            /// <param name="min">The min.</param>
            /// <param name="max">The max.</param>
            /// <param name="page">The page.</param>
            /// <param name="pos">The pos.</param>
            public ScrollInfo(int mask, int min, int max, int page, int pos)
            {
                this.cbSize = Marshal.SizeOf(typeof(ScrollInfo));
                this.fMask = mask;
                this.nMin = min;
                this.nMax = max;
                this.nPage = page;
                this.nPos = pos;
            }
        }
    }
}
