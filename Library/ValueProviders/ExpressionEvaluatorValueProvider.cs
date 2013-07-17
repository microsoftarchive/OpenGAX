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
using System.ComponentModel.Design;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Services;
using System.ComponentModel;

#endregion Using directives

namespace Microsoft.Practices.RecipeFramework.Library.ValueProviders
{
	/// <summary>
	/// A value provider that can evaluate expressions in the context of the argument dictionary.
	/// </summary>
	/// <remarks>
    /// <para>
	/// This value provider requires an attribute on the ValueProvider configuration element named 
	/// <c>Expression</c>, containing the expression that will be evaluated at each stage during 
    /// the lifecycle of the provider. See <see cref="ExpressionEvaluationService"/> for information about the 
    /// format supported by the <c>Expression</c> attribute.
    /// </para>
    /// <para>
    /// To restrict the stage at which the value will be evaluated, a <c>Stage</c> attribute can be 
    /// applied to the configuration element, containing one of the values defined in <see cref="Stage"/>.
    /// </para>
    /// <para>
    /// A new value will be returned only if there's no current value already. If the element is set up 
    /// to monitor other arguments, each time any of the monitored argument changes, the expression will 
    /// be re-evaluated.
    /// </para>
	/// </remarks>
    /// <example>
    /// &lt;Argument Name="InstallerProject"&gt;
    ///   &lt;ValueProvider 
    ///       Type="Microsoft.Practices.RecipeFramework.Library.ValueProviders.ExpressionEvaluatorValueProvider, Microsoft.Practices.RecipeFramework.Library" 
    ///       Expression="$(PackageName)Installer" Stage="OnBeforeActions" /&gt;
    /// &lt;/Argument&gt;
    /// </example>
	[ServiceDependency(typeof(IDictionaryService))]
	public class ExpressionEvaluatorValueProvider : ValueProvider, IAttributesConfigurable
	{
		string expression;
        Stage stage = Stage.All;
		ExpressionEvaluationService evaluator = new ExpressionEvaluationService();

        /// <summary>
        /// <paramref name="newValue"/> will be updated by re-evaluating the expression if the 
        /// <c>Stage</c> configuration attribute on the provider element has the value <see cref="Stage.OnBeginRecipe"/> 
        /// or is not defined at all.
        /// </summary>
        /// <remarks>
        /// See <see cref="IValueProvider.OnBeginRecipe"/>.
        /// </remarks>
		public override bool OnBeginRecipe(object currentValue, out object newValue)
		{
			newValue = null;
			if (currentValue != null || stage == Stage.OnBeforeActions)
			{
				return false;
			}
			return Evaluate(out newValue);
		}

        /// <summary>
        /// <paramref name="newValue"/> will be updated by re-evaluating the expression if the 
        /// <c>Stage</c> configuration attribute on the provider element has the value <see cref="Stage.OnBeforeActions"/> 
        /// or is not defined at all.
        /// </summary>
        /// <remarks>
        /// See <see cref="IValueProvider.OnBeforeActions"/>.
        /// </remarks>
        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
            newValue = null;
            if (currentValue != null || stage == Stage.OnBeginRecipe)
            {
                return false;
            }
            return Evaluate(out newValue);
        }

		/// <summary>
		/// Causes the <paramref name="newValue"/> to always be updated by re-evaluating the expression.
		/// </summary>
        /// <remarks>
        /// See <see cref="IValueProvider.OnArgumentChanged"/>.
        /// </remarks>
		public override bool OnArgumentChanged(string changedArgumentName, object changedArgumentValue, 
			object currentValue, out object newValue)
		{
			return Evaluate(out newValue);
		}

		private bool Evaluate(out object newValue)
		{
			IDictionaryService dictservice = (IDictionaryService)
				ServiceHelper.GetService(this, typeof(IDictionaryService));
			newValue = evaluator.Evaluate(expression, new ServiceAdapterDictionary(dictservice));
			return true;
		}

		#region IAttributesConfigurable Members

		/// <summary>
        /// Allows configuration of the provider with two attributes: <c>Expression</c> and <c>Stage</c>.
		/// </summary>
        /// <remarks>
        /// See <see cref="IAttributesConfigurable.Configure"/>.
        /// </remarks>
		public void Configure(System.Collections.Specialized.StringDictionary attributes)
		{
			if (!attributes.ContainsKey("Expression"))
			{
				throw new ArgumentNullException("Expression", Properties.Resources.ExpressionProvider_ExpressionMissing);
			}
			expression = attributes["Expression"];
            if (attributes.ContainsKey("Stage"))
            {
                stage = (Stage)Enum.Parse(typeof(Stage), attributes["Stage"]);
            }
		}

		#endregion

        /// <summary>
        /// Stages that can be specified as an attribute named "Stage" on the value provider configuration element.
        /// </summary>
        public enum Stage : byte
        {
            /// <summary>
            /// Value provider will be run on all stages.
            /// </summary>
            All,
            /// <summary>
            /// Run the value provider only at <see cref="ExpressionEvaluatorValueProvider.OnBeginRecipe"/>.
            /// </summary>
            OnBeginRecipe,
            /// <summary>
            /// Run the value provider only at <see cref="ExpressionEvaluatorValueProvider.OnBeforeActions"/>.
            /// </summary>
            OnBeforeActions
        }
	}
}
