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

namespace Microsoft.Practices.RecipeFramework
{

    /// <summary>
    /// The exception that is thrown when an unexpected error occurs inside the Recipe Framework.
    /// </summary>
    [Serializable]
    public class RecipeFrameworkException : Exception
    {
        /// <summary>
        /// Initializes the exception.
        /// </summary>
        public RecipeFrameworkException()
			: base()
        {
        }

		/// <summary>
		/// Initializes the exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        public RecipeFrameworkException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The <see cref="Exception"/> to encapsulate.</param>
        public RecipeFrameworkException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected RecipeFrameworkException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

}
