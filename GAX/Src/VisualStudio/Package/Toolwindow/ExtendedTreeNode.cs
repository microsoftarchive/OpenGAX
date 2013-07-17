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

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Microsoft.Practices.RecipeFramework.VisualStudio.ToolWindow
{
    internal class ExtendedTreeNode : TreeNode
    {
        private bool showAsLink;

        /// <summary>
        /// Gets or sets a value indicating whether the TreeNode show as link.
        /// </summary>
        /// <value><c>true</c> if [show as link]; otherwise, <c>false</c>.</value>
        public bool ShowAsLink
        {
            get { return showAsLink; }
            set { showAsLink = value; }
        }

        private bool showCloseButton;

        /// <summary>
        /// Gets or sets a value indicating if close button must be show or not.
        /// </summary>
        /// <value><c>true</c> if [show close button]; otherwise, <c>false</c>.</value>
        public bool ShowCloseButton
        {
            get { return showCloseButton; }
            set { showCloseButton = value; }
        }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExtendedTreeNode"/> class.
        /// </summary>
        public ExtendedTreeNode() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExtendedTreeNode"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public ExtendedTreeNode(string text) : base(text) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExtendedTreeNode"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="children">The children.</param>
        public ExtendedTreeNode(string text, TreeNode[] children) : this(text, 0, 0, children) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExtendedTreeNode"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="imageIndex">Index of the image.</param>
        /// <param name="selectedImageIndex">Index of the selected image.</param>
        public ExtendedTreeNode(string text, int imageIndex, int selectedImageIndex) : this(text, imageIndex, selectedImageIndex, new TreeNode[] { }) { }

        public ExtendedTreeNode(string text, int imageIndex, int selectedImageIndex, TreeNode[] children)
            : base(text, imageIndex, selectedImageIndex, children)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExtendedTreeNode"/> class.
        /// </summary>
        /// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> containing the data to deserialize the class.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> containing the source and destination of the serialized stream.</param>
        protected ExtendedTreeNode(SerializationInfo serializationInfo, StreamingContext context)
            : base()
        {
            this.Deserialize(serializationInfo, context);
        }
        #endregion

    }
}
