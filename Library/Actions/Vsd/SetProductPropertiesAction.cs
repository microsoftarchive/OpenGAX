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

using System;
using System.Runtime.InteropServices;
using ComTypes = System.Runtime.InteropServices.ComTypes;
using Microsoft.VisualStudio;
using System.Globalization;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions.Vsd
{
    /// <summary>
    /// The action sets the product properties author and installAllUsers to a project
    /// that must be a Setup Project.
    /// The action parameters are:
    /// 1. Setup project where the properties will be changed
    /// 2. InstallAllUsers bool value to set to the property of the same name
    /// 3. Author string value to set to the property Author and Manufacturer
    /// </summary>
    public class SetProductPropertiesAction: ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// The setup project where the changes will be applied
        /// </summary>
        [Input(Required=true)]
        public EnvDTE.Project SetupProject
        {
            get { return project; }
            set { project = value; }
        } EnvDTE.Project project;

        /// <summary>
        /// The bool value to set the property InstallAllUsers
        /// </summary>
        [Input(false)]
        public bool InstallAllUsers
        {
            get { return installAllUsers; }
            set { installAllUsers = value; }
        } bool installAllUsers;

        /// <summary>
        /// The string value to set the Author and Manufacturer
        /// </summary>
		[Input(Required=true)]
		public string Author
		{
			get { return author; }
			set { author = value; }
		} string author;

		/// <summary>
		/// The string value to set the Description
		/// </summary>
		[Input(false)]
		public string Description
		{
			get { return description; }
			set { description = value; }
		} string description;

		#endregion

        #region IAction members

        /// <summary>
        /// Performs the action of the changing of the properties to the project
        /// </summary>
        public override void Execute()
        {
            IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
            IVsdCollectionSubset plugins= deployable.GetPlugIns();
            IVsdProductPlugIn productPlugin = plugins.Item("Product") as IVsdProductPlugIn;
            if (productPlugin != null)
            {
                bool oldValue = productPlugin.InstallAllUsers;
                productPlugin.InstallAllUsers = InstallAllUsers;
				productPlugin.Manufacturer = Author;
				productPlugin.Author = Author;
				productPlugin.Description = Description;
                DisableUIInstallAllUsers(InstallAllUsers);
                InstallAllUsers = oldValue;
            }
        }

        private void DisableUIInstallAllUsers(bool InstallAllUsers)
        {
            IVsdDeployable deployable = (IVsdDeployable)SetupProject.Object;
            IVsdCollectionSubset plugins= deployable.GetPlugIns();
            IVsdUserInterfacePlugIn uiPlugin = plugins.Item("UserInterface") as IVsdUserInterfacePlugIn;
            if (uiPlugin != null)
            {
                foreach (object objectFile in uiPlugin.Items)
                {
                    if (objectFile is IVsdDialogModuleGroup)
                    {
                        IVsdDialogModuleGroup uiModuleGroup = objectFile as IVsdDialogModuleGroup;
                        foreach (object dialogObject in uiModuleGroup.Dialogs)
                        {
                            if (dialogObject is IVsdDialogModule)
                            {
                                ChangeInstallAllUsersUI(dialogObject as IVsdDialogModule);
                            }
                        }
                    }
                    else if ( objectFile is IVsdDialogModule )
                    {
                        ChangeInstallAllUsersUI(objectFile as IVsdDialogModule);
                    }
                }
            }
        }

        private void ChangeInstallAllUsersUI(IVsdDialogModule uiModule)
        {
            int hr = VSConstants.S_OK;
            NativeMethods.IDispatch properties = (NativeMethods.IDispatch)uiModule.DynamicProperties;
            Guid guid = Guid.Empty;
            string[] names = new string[] { "InstallAllUsersVisible" };
            int[] dispIds = new int[] { 0 };
            hr = properties.GetIDsOfNames(ref guid, names, names.Length, 0, dispIds);
            if (dispIds[0]!=-1)
            {
                ComTypes.DISPPARAMS[] dispParams = new ComTypes.DISPPARAMS[1];
                dispParams[0].cNamedArgs = 0;
                dispParams[0].rgdispidNamedArgs = IntPtr.Zero;
                dispParams[0].cArgs = 1;
                dispParams[0].rgvarg = Marshal.AllocCoTaskMem(0x1000);
                try
                {
                    Marshal.GetNativeVariantForObject(!InstallAllUsers, dispParams[0].rgvarg);
                    hr = properties.Invoke(
                        dispIds[0],
                        ref guid,
                        CultureInfo.CurrentCulture.LCID,
                        (short)ComTypes.INVOKEKIND.INVOKE_PROPERTYPUT,
                        dispParams,
                        null,
                        null,
                        null);
                }
                finally
                {
                    Marshal.FreeCoTaskMem(dispParams[0].rgvarg);
                    dispParams[0].rgvarg = IntPtr.Zero;
                    if (hr != VSConstants.S_OK)
                    {
                        Marshal.ThrowExceptionForHR(hr);
                    }
                }
            }
        }

        /// <summary>
        /// Performs the Undo changes
        /// </summary>
        public override void Undo()
        {
            Execute();
        }

        #endregion
    }
}
