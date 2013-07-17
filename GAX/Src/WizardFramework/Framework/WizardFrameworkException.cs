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

namespace Microsoft.Practices.WizardFramework
{

    /// <summary>
    /// Exception thrown when an unexpected error occurs inside the Wizard Framework.
    /// </summary>
    [Serializable]
    public class WizardFrameworkException : ApplicationException
    {
        /// <summary>
        /// Initializes the exception.
        /// </summary>
        public WizardFrameworkException()
			: base()
        {
        }

		/// <summary>
		/// Initializes the exception.
		/// </summary>
		public WizardFrameworkException(String message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
		public WizardFrameworkException(String message, Exception innerException)
			: base(message, innerException)
		{
		}

        /// <summary>
        /// Initializes the exception.
        /// </summary>
		protected WizardFrameworkException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

}
