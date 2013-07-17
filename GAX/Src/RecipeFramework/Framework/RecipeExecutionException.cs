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
    /// Exception thrown when execution of the recipe failed.
    /// </summary>
    [Serializable]
    public class RecipeExecutionException : Exception
    {
        /// <summary>
        /// Initializes the exception.
        /// </summary>
        /// <param name="recipeName">The name of the recipe being executed at the moment the 
        /// exception is throw.</param>
        /// <param name="message">The error message.</param>
        public RecipeExecutionException(string recipeName, string message)
            : base(String.Format(System.Globalization.CultureInfo.CurrentCulture,
            Properties.Resources.Recipe_ExecutionException, recipeName, message))
        {
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        public RecipeExecutionException()
        {
        }

		/// <summary>
		/// Initializes the exception.
		/// </summary>
        /// <param name="message">The error message.</param>
        public RecipeExecutionException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">Exception to encapsulate.</param>
        public RecipeExecutionException(string message, Exception innerException)
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
        public RecipeExecutionException(string recipeName, string message, Exception innerException)
            : base(String.Format(System.Globalization.CultureInfo.CurrentCulture,
            Properties.Resources.Recipe_ExecutionException, recipeName, message), innerException)
        {
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Context for deserialization.</param>
        protected RecipeExecutionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}