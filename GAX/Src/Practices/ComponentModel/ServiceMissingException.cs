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
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Microsoft.Practices.ComponentModel
{
    /// <summary>
    /// Exception thrown when a required service doesn't exist in 
    /// the component container.
    /// </summary>
    [Serializable]
    public class ServiceMissingException : Exception
    {
		/// <summary>
		/// Initializes the exception.
		/// </summary>
		public ServiceMissingException()
		{
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
		public ServiceMissingException(String message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
		public ServiceMissingException(String message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
        /// Initializes the exception.
        /// </summary>
        public ServiceMissingException(Type serviceType, object component)
            : base(String.Format(System.Globalization.CultureInfo.CurrentCulture,
            Properties.Resources.ServiceMissingException_Message,
            serviceType, component.ToString()))
        {
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        public ServiceMissingException(Type serviceType, object component, Exception innerException)
            : base(String.Format(System.Globalization.CultureInfo.CurrentCulture,
            Properties.Resources.ServiceMissingException_Message,
            serviceType, component.ToString()), innerException)
        {
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        protected ServiceMissingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
