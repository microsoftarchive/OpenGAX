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

namespace Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates
{
    /// <summary>
    /// value-type pair used by ProcessDirectiveProcessor to 
    /// create the property type and assign the value.
    /// </summary>
	public class PropertyData : MarshalByRefObject
    {
        private object value;
        private Type type;

		/// <summary>
		/// Intializes the property data with the value and type.
		/// </summary>
        public PropertyData(object value, Type type)
        {
            this.value = value;
            this.type = type;
        }

        /// <summary>
        /// Property Value.
        /// </summary>
        public object Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Property Type.
        /// </summary>
        public Type Type
        {
            get { return this.type; }
        }
    }
}
