
#region Using directives

using System;
using System.ComponentModel;
using Microsoft.Practices.Common.Services;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
	/// <summary>
	/// Provides components with argument value collection strategies.
	/// </summary>
	public interface IValueProvider
	{
		/// <summary>
		/// Contains code that will be called when recipe execution begins. This is the first method in the lifecycle.
		/// </summary>
		/// <param name="currentValue">An <see cref="Object"/> that contains the current value of the argument.</param>
		/// <param name="newValue">When this method returns, contains an <see cref="Object"/> that contains 
		/// the new value of the argument, if the returned value 
		/// of the method is <see langword="true"/>. Otherwise, it is ignored.</param>
		/// <returns><see langword="true"/> if the argument value should be replaced with 
		/// the value in <paramref name="newValue"/>; otherwise, <see langword="false"/>.</returns>
		bool OnBeginRecipe(object currentValue, out object newValue);

        /// <summary>
        /// Contains code that will be called before actions are executed.
        /// </summary>
        /// <param name="currentValue">An <see cref="Object"/> that contains the current value of the argument.</param>
        /// <param name="newValue">When this method returns, contains an <see cref="Object"/> that contains 
        /// the new value of the argument, if the returned value 
        /// of the method is <see langword="true"/>. Otherwise, it is ignored.</param>
        /// <returns><see langword="true"/> if the argument value should be replaced with 
        /// the value in <paramref name="newValue"/>; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// If no gathering strategy (i.e. a wizard) is configured, this method will not me called. 
		/// In that case, the <see cref="OnBeginRecipe"/> should be used instead.
		/// </remarks>
		bool OnBeforeActions(object currentValue, out object newValue);

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
        bool OnArgumentChanged(string changedArgumentName, object changedArgumentValue, object currentValue, out object newValue);
		
        /// <summary>
		/// Initializes the value provider for a certain argument.
		/// </summary>
        /// <param name="argumentData">A <see cref="ValueInfo"/> that contains metadata information about the 
        /// argument to collect.</param>
		void Initialize(ValueInfo argumentData);
		
        /// <summary>
		/// Gets the argument information received in <see cref="Initialize"/>.
		/// </summary>
        ValueInfo Argument { get; }
	}
}
