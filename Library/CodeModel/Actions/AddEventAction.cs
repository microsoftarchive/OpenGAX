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
using System.Collections.Generic;
using System.Text;
using EnvDTE80;
using EnvDTE;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel.Actions
{
    /// <summary>
    /// Adds an event to a <see cref="CodeClass2"/> object
    /// </summary>
	public sealed class AddEventAction: ConfigurableAction
    {
        #region Input Properties

        private CodeClass2 codeClass;
        /// <summary>
        /// The class where the event will be acced
        /// </summary>
        [Input(Required=true)]
        public CodeClass2 CodeClass
        {
            get { return codeClass; }
            set { codeClass = value; }
        }

        private string eventName;
        /// <summary>
        /// The name of the event been added
        /// </summary>
        [Input(Required=true)]
        public string EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }

        private string delegateName;
        /// <summary>
        /// The delate for the new event
        /// </summary>
        [Input(Required=true)]
        public string DelegateName
        {
            get { return delegateName; }
            set { delegateName = value; }
        }
	
        private bool createProperyStyle = false;
        /// <summary>
        /// Creates the event using a property
        /// </summary>
        [Input(Required=false)]
        public bool CreatePropertyStyle
        {
            get { return createProperyStyle; }
            set { createProperyStyle = value; }
        }
	
        /// <summary>
        /// The position in the class where the member is been added
        /// </summary>
        [Input(Required=false)]
        public object Position
        {
            get { return position; }
            set { position = value; }
        } object position = 0;

        /// <summary>
        /// The kind of visibility of the new variable
        /// </summary>
        /// <seealso cref="vsCMAccess"/>
        [Input(Required=false)]
        public vsCMAccess Access
        {
            get { return access; }
            set { access = value; }
        } vsCMAccess access = vsCMAccess.vsCMAccessPublic;

        #endregion

        #region Output properties

        private CodeEvent codeEvent;

        /// <summary>
        /// The created event
        /// </summary>
        [Output]
        public CodeEvent Event
        {
            get { return codeEvent; }
            set { codeEvent = value; }
        }
	

        #endregion

        #region Overrides

        /// <summary>
        /// Adds the event
        /// </summary>
        public override void Execute()
        {
            this.Event = this.CodeClass.AddEvent(this.EventName,
                this.DelegateName, this.CreatePropertyStyle,
                this.Position, this.Access);
        }

        /// <summary>
        /// Removes the event
        /// </summary>
        public override void Undo()
        {
            this.CodeClass.RemoveMember(this.Event);
            this.Event = null;
        }

        #endregion
    }
}
