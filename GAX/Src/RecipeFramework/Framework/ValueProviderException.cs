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
using System.Text;
using System.Runtime.Serialization;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
    /// <summary>
    /// Exception thrown when a value provider fails.
    /// </summary>
    [Serializable]
    public class ValueProviderException : RecipeExecutionException
    {
        /// <summary>
        /// Initializes the exception.
        /// </summary>
        /// <param name="recipeName">The name of the recipe being executed at the moment the 
        /// exception is throw.</param>
        /// <param name="message">The error message.</param>
        public ValueProviderException(string recipeName, string message)
            : base(recipeName, message)
        {
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        public ValueProviderException()
        {
        }

		/// <summary>
		/// Initializes the exception.
		/// </summary>
        /// <param name="message">The error message.</param>
        public ValueProviderException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">Exception to encapsulate.</param>
        public ValueProviderException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        /// <param name="recipeName">The name of the recipe being executed at the moment the 
        /// exception is throw.</param>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">Exception to encapsulate.</param>
        public ValueProviderException(string recipeName, string message, Exception innerException)
            : base(recipeName, message, innerException)
        {
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ValueProviderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}