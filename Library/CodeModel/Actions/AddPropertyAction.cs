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
using Microsoft.Practices.RecipeFramework;
using EnvDTE;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.CodeModel.Actions
{
    /// <summary>
    /// Adds a new property to an existing <see cref="EnvDTE.CodeClass"/>
    /// </summary>
    public class AddPropertyAction : ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// The class where the new property is been added
        /// </summary>
        [Input(Required=true)]
        public CodeClass CodeClass
        {
            get { return codeClass; }
            set { codeClass = value; }
        } CodeClass codeClass;

        /// <summary>
        /// The name of property
        /// </summary>
        [Input(Required=true)]
        public string PropertyName
        {
            get { return name; }
            set { name = value; }
        } string name;

        /// <summary>
        /// The type of the property
        /// </summary>
        [Input(Required=false)]
        public object PropertyType
        {
            get { return propertyType; }
            set { propertyType = value; }
        } object propertyType = vsCMTypeRef.vsCMTypeRefInt;

        /// <summary>
        /// The position in the class where the property will be added
        /// </summary>
        [Input(Required=false)]
        public object Position
        {
            get { return position; }
            set { position = value; }
        } object position = 0;

        /// <summary>
        /// The access kind of the new property
        /// </summary>
        [Input(Required=false)]
        public vsCMAccess Access
        {
            get { return access; }
            set { access = value; }
        } vsCMAccess access = vsCMAccess.vsCMAccessPublic;

        #endregion

        #region Output Properties

        /// <summary>
        /// The code element object of the new property
        /// </summary>
        [Output]
        public CodeProperty Property
        {
            get { return property; }
            set { property = value; }
        } CodeProperty property;

        #endregion

        #region Action members

        /// <summary>
        /// <see cref="IAction.Execute"/>
        /// </summary>
        public override void Execute()
        {
            this.Property = this.CodeClass.AddProperty(this.PropertyName,
                this.PropertyName,
                this.PropertyType,
                this.Position,
                this.Access,
                null);
            AddMethodAction.AddExpansionComment(this.Property.Getter);
            AddMethodAction.AddExpansionComment(this.Property.Setter);
        }

        /// <summary>
        /// <see cref="IAction.Undo"/>
        /// </summary>
        public override void Undo()
        {
            this.CodeClass.RemoveMember(this.Property);
        }

        #endregion
    }
}
