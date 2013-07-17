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
using System.Globalization;

#endregion

namespace Microsoft.Practices.WizardFramework
{
    /// <summary>
    /// Exception thrown when execution of the wizard failed.
    /// </summary>
    [Serializable]
    public class WizardExecutionException : ApplicationException
    {
        /// <summary>
        /// Initializes the exception.
        /// </summary>
        public WizardExecutionException(string wizard, string message)
            : base(String.Format(CultureInfo.CurrentCulture,
            Properties.Resources.WizardGatheringService_ExecutionFailed, 
            wizard, message))
        {
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        public WizardExecutionException(string wizard, Exception innerException)
            : base(String.Format(CultureInfo.CurrentCulture,
            Properties.Resources.WizardGatheringService_ExecutionFailed,
            wizard, innerException.Message), innerException)
        {
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        public WizardExecutionException()
        {
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        public WizardExecutionException(string wizard, string message, Exception innerException)
            : base(String.Format(CultureInfo.CurrentCulture,
            Properties.Resources.WizardGatheringService_ExecutionFailed,
            wizard, message), innerException)
        {
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        protected WizardExecutionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}