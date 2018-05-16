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

#region Using Directives

using EnvDTE;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Shell.Design.Serialization;
using Microsoft.Practices.ComponentModel;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions
{
    /// <summary>
    /// Sets the context of an project item, even if the project item is opened by Visual Studio
    /// </summary>
    [ServiceDependency(typeof(ILocalRegistry))]
    public sealed class SetProjectItemContentAction : Action
    {
        #region Input Properties

        /// <summary>
        /// The project item that is receiving the new content
        /// </summary>
        [Input(Required=true)]
        public ProjectItem ProjectItem
        {
            get { return projectItem; }
            set { projectItem = value; }
        } ProjectItem projectItem;

        /// <summary>
        /// The content been replaced into the project item
        /// </summary>
        [Input(Required=true)]
        public string Content
        {
            get { return content; }
            set { content = value; }
        } string content;

        #endregion

        #region Output Properties

        #endregion

        #region Private Implementation

        private void ProcessFile(string filePath)
        {
            IVsTextBuffer textBuffer = null;
            IPersistFileFormat textBufferPersist = null;
            IVsTextStream textStream = null;
            IVsUserData userData = null;
            IServiceProvider serviceProvider = (IServiceProvider)GetService(typeof(IServiceProvider));
            using (DocData docData = new DocData(serviceProvider, filePath))
                try
                {
                    textBuffer = docData.Buffer;
                    userData = (IVsUserData)textBuffer;
                    Guid VsBufferDetectLangSID = new Guid("{17F375AC-C814-11d1-88AD-0000F87579D2}");
                    Marshal.ThrowExceptionForHR(userData.SetData(ref VsBufferDetectLangSID, false));
                    textBufferPersist = (IPersistFileFormat)textBuffer;
                    textStream = (IVsTextStream)textBuffer;
                    int fileLenBeforeReplacement = 0;
                    Marshal.ThrowExceptionForHR(textStream.GetSize(out fileLenBeforeReplacement));
                    IntPtr buffer = IntPtr.Zero;
                    try
                    {
                        buffer = Marshal.StringToCoTaskMemUni(this.Content);
                        Marshal.ThrowExceptionForHR(textStream.ReloadStream(0, fileLenBeforeReplacement, buffer, Content.Length));
                    }
                    finally
                    {
                        Marshal.FreeCoTaskMem(buffer);
                        buffer = IntPtr.Zero;
                    }

                    // Get the original encoding as Texteditor detected it.
                    // The Unicode lib packs codePage in the low word and flags (as emmit BOM) in high word.
                    object encObj = null;
                    Guid VsBufferEncodingVSTFF = new Guid("{16417F39-A6B7-4c90-89FA-770D2C60440B}");
                    Marshal.ThrowExceptionForHR(userData.GetData(ref VsBufferEncodingVSTFF, out encObj));
                    uint encValue = (uint)encObj;
                    // set the desired encoding
                    encValue = 65001 | 0x10000;
                    Marshal.ThrowExceptionForHR(userData.SetData(ref VsBufferEncodingVSTFF, encValue));
                    Marshal.ThrowExceptionForHR(textBufferPersist.Save(filePath, 1, 0));
                    Marshal.ThrowExceptionForHR(textBufferPersist.SaveCompleted(filePath));
                }
                finally
                {
					//if (userData != null)
					//{
					//    Marshal.ReleaseComObject(userData);
					//}
					//if (textStream != null)
					//{
					//    Marshal.ReleaseComObject(textStream);
					//}
					//if (textBufferPersist != null)
					//{
					//    Marshal.ReleaseComObject(textBufferPersist);
					//}
					//if (textBuffer != null)
					//{
					//    Marshal.ReleaseComObject(textBuffer);
					//}
                }
        }

        #endregion

        #region IAction Members

        /// <summary>
        /// Performs the replacement with the given parameters
        /// </summary>
        public override void Execute()
        {
            bool reOpenItem = false;
            try
            {
                string fileName = this.ProjectItem.get_FileNames(0);
                Document doc = this.ProjectItem.Document;
                if (doc != null && doc.ActiveWindow.Object is IDesignerHost)
                {
					try
					{
						doc.Close(vsSaveChanges.vsSaveChangesNo);
						reOpenItem = true;
					}
					catch(Exception ex)
					{
						this.TraceError(ex.ToString());
						reOpenItem = false;
					}
                }
                if (System.IO.File.Exists(fileName))
                {
                    ProcessFile(fileName);
                }
            }
            finally
            {
                if (reOpenItem)
                {
                    Window window = this.ProjectItem.Open(EnvDTE.Constants.vsViewKindPrimary);
                    if (window != null)
                    {
                        window.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Undoes the replacement, not implemented
        /// </summary>
        public override void Undo()
        {
            // No undo implemented.
        }

        #endregion
    }
}
