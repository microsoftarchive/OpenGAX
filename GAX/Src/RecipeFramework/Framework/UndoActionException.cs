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

#region Using directives

using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Text;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
    [Serializable]
    internal class UndoActionException : ApplicationException
	{
        /// <summary>
        /// Initializes the exception.
        /// </summary>
        public UndoActionException()
        {
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        public UndoActionException(string actionName, Exception innerException)
            : base(String.Format(System.Globalization.CultureInfo.CurrentCulture,
            Properties.Resources.Recipe_ActionUndoException, actionName, innerException.Message), innerException)
        {
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected UndoActionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
	}
}
