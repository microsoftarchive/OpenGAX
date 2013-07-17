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

#region Using Directives

using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.ComponentModel;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Library.Actions;

#endregion

namespace Microsoft.Practices.RecipeFramework.Library.Actions
{
    /// <summary>
    /// Action to evaluate an expression
    /// </summary>
    public sealed class EvaluateExpressionAction2 : DynamicInputAction
    {
        #region Input Properties

        /// <summary>
        /// The expression been evaluated
        /// </summary>
        [Input(Required=true)]
        public string Expression
        {
            get { return expression; }
            set { expression = value;  }
        } string expression;

        #endregion

        #region Output Properties

        /// <summary>
        /// The result of the evaluation
        /// </summary>
        [Output]
        public object ReturnValue
        {
            get { return returnValue; }
            set { returnValue = value; }
        } object returnValue;

        #endregion

        #region IAction members

        /// <summary>
        /// Evaluates the expression
        /// </summary>
        public override void Execute()
        {
            ExpressionEvaluationService2 evaluator = new ExpressionEvaluationService2();
            try
            {
                this.WrappedContainer.AddService(evaluator.GetType(),evaluator);
                IDictionaryService dictservice = (IDictionaryService)
                     this.WrappedContainer.GetService(typeof(IDictionaryService),true);
                this.ReturnValue = evaluator.Evaluate(
                    this.Expression, 
                    new ServiceAdapterDictionary(dictservice));
            }
            finally
            {
                this.WrappedContainer.RemoveService(evaluator.GetType());
                evaluator = null;
            }
        }

        /// <summary>
        /// Just sets the return value to null again
        /// </summary>
        public override void Undo()
        {
            this.ReturnValue = null;
        }

        #endregion
    }
}
