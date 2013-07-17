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
using System.ComponentModel;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.ComponentModel;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Provides a default implementation of <see cref="IValueProvider"/> that allows a descendant class 
	/// to override only those methods it needs to modify or replace.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public abstract class ValueProvider : SitedComponent, IValueProvider
	{
		#region IValueProvider Members

        /// <summary>
        /// Contains code that will be called when recipe execution begins. This is the first method in the lifecycle.
        /// </summary>
        /// <param name="currentValue">An <see cref="Object"/> that contains the current value of the argument.</param>
        /// <param name="newValue">When this method returns, contains an <see cref="Object"/> that contains 
        /// the new value of the argument, if the returned value 
        /// of the method is <see langword="true"/>. Otherwise, it is ignored.</param>
        /// <returns><see langword="true"/> if the argument value should be replaced with 
        /// the value in <paramref name="newValue"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>By default, always returns <see langword="false"/>, unless overriden by a derived class.</remarks>
        public virtual bool OnBeginRecipe(object currentValue, out object newValue)
        {
            newValue = null;
            return false;
        }

        /// <summary>
        /// Contains code that will be called before actions are executed.
        /// </summary>
        /// <param name="currentValue">An <see cref="Object"/> that contains the current value of the argument.</param>
        /// <param name="newValue">When this method returns, contains an <see cref="Object"/> that contains 
        /// the new value of the argument, if the returned value 
        /// of the method is <see langword="true"/>. Otherwise, it is ignored.</param>
        /// <returns><see langword="true"/> if the argument value should be replaced with 
        /// the value in <paramref name="newValue"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>By default, always returns <see langword="false"/>, unless overridden by a derived class.</remarks>
        public virtual bool OnBeforeActions(object currentValue, out object newValue)
        {
            newValue = null;
            return false;
        }

        /// <summary>
        /// Contains code that will be called whenever an argument monitored by the value provider 
        /// changes, as specified in the configuration file.
        /// </summary>
        /// <param name="changedArgumentName">The name of the argument being monitored that changed.</param>
        /// <param name="changedArgumentValue">An <see cref="Object"/> that contains the value of the monitored argument.</param>
        /// <param name="currentValue">An <see cref="Object"/> that contains the current value of the argument.</param>
        /// <param name="newValue">When this method returns, contains an <see cref="Object"/> that contains 
        /// the new value of the argument, if the returned value 
        /// of the method is <see langword="true"/>. Otherwise, it is ignored.</param>
        /// <returns><see langword="true"/> if the argument value should be replaced with 
        /// the value in <paramref name="newValue"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>By default, always returns <see langword="false"/>, unless overriden by a derived class.</remarks>
        public virtual bool OnArgumentChanged(string changedArgumentName, object changedArgumentValue, object currentValue, out object newValue)
		{
            newValue = null;
            return false;
		}

        /// <summary>
        /// Initializes the value provider for a certain argument.
        /// </summary>
        /// <param name="argumentData">A <see cref="ValueInfo"/> that contains metadata information about the 
        /// argument to collect.</param>
        public virtual void Initialize(ValueInfo argumentData)
		{
			this.argumentData = argumentData;
		}

        ValueInfo argumentData;

        /// <summary>
        /// Gets the argument information received in <see cref="Initialize"/>.
        /// </summary>
        public virtual ValueInfo Argument
		{
			get { return argumentData; }
		} 
		
		#endregion
	}
}
