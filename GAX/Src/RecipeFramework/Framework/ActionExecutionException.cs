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
using System.Security.Permissions;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Exception thrown when an error happen while executing an action.
	/// </summary>
	[Serializable]
	public class ActionExecutionException : RecipeExecutionException
	{
		/// <summary>
		/// Initializes the exception.
		/// </summary>
        /// <param name="recipeName">The name of the recipe being executed at the moment the 
        /// exception is throw.</param>
        /// <param name="actionName">The name of the action being executed at the moment the 
        /// exception is thrown.</param>
        /// <param name="inner">Underlying exception happening that will be wrapped.</param>
        /// <param name="undoExceptions">The list of exceptions that happened while undo operations were performed.</param>
		internal ActionExecutionException(string recipeName, string actionName, Exception inner, Exception[] undoExceptions)
            : base(recipeName, String.Format(System.Globalization.CultureInfo.CurrentCulture, 
            Properties.Resources.Recipe_ActionExecutionFailed, actionName, inner.Message), inner)
		{
            this.undoExceptions = undoExceptions;
		}


		internal ActionExecutionException(string recipeName, string actionName, string message)
			: base(recipeName, String.Format(System.Globalization.CultureInfo.CurrentCulture,
			Properties.Resources.Recipe_ActionExecutionFailed, actionName, message))
		{
		}
        /// <summary>
        /// Initializes the exception.
        /// </summary>
        public ActionExecutionException()
        {
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
        /// <param name="message">The error message.</param>
        public ActionExecutionException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">Underlying exception happening that will be wrapped.</param>
        public ActionExecutionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

        [NonSerialized]
        Exception[] undoExceptions;

        /// <summary>
        /// Exposes the exceptions that happened while trying to undo the actions already 
        /// executed.
        /// </summary>
        public Exception[] UndoExceptions
        {
            get { return undoExceptions; }
        }

        #region Serialization members

        string serializedUndoExceptions;

        /// <summary>
        /// Special deserialization constructor.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ActionExecutionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            serializedUndoExceptions = info.GetString("UndoExceptions");
        }

        /// <summary>
        /// Adds the necessary state prior to serialization.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            StringBuilder builder = new StringBuilder();
            AppendUndoExceptions(builder);
            info.AddValue("UndoExceptions", builder.ToString(), typeof(string));
        }

        #endregion Serialization members

        /// <summary>
        /// Provides a string representation of the exception and all 
        /// <see cref="UndoExceptions"/> if any.
        /// </summary>
        /// <returns>
        /// A string representation of the exception.
        /// </returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString()).Append(Environment.NewLine);
            if (serializedUndoExceptions != null)
            {
                builder.Append(serializedUndoExceptions).Append(Environment.NewLine);
            }
            else
            {
                AppendUndoExceptions(builder);
            }
            return builder.ToString();
        }

        private void AppendUndoExceptions(StringBuilder builder)
        {
			if (undoExceptions != null)
			{
				foreach (Exception ex in undoExceptions)
				{
					// Indent undo exceptions. See Exception.ToString() for formatting.
					builder.Append("\t---> ").Append(ex.ToString()).Append(Environment.NewLine);
				}
			}
        }
	}
}
