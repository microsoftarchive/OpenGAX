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
using Microsoft.Practices.Common;
using System.Collections;

namespace Microsoft.Practices.RecipeFramework.Services
{
    /// <summary>
    /// Executes a recipe.
    /// </summary>
    public interface IExecutionService
    {
        /// <summary>
        /// Executes a recipe with no persistence associated.
        /// </summary>
        /// <param name="recipe">The name of the recipe to execute.</param>
        /// <exception cref="InvalidOperationException">A recipe is already being executed.</exception>
        /// <exception cref="ArgumentException">The recipe does not exist in the package configuration.</exception>
        /// <returns>An <see cref="ExecutionResult"/> representing the result of the execution.</returns>
        ExecutionResult Execute(string recipe);

        /// <summary>
        /// Executes a recipe using the 
        /// received dictionary as the initial state for the execution.
        /// </summary>
        /// <param name="recipe">The name of the recipe to execute.</param>
        /// <param name="state">An <see cref="IDictionary"/> containing the initial state information 
        /// to use for the execution.</param>
        /// <exception cref="InvalidOperationException">A recipe is already being executed.</exception>
        /// <exception cref="ArgumentException">The recipe does not exist in the package configuration.</exception>
        /// <returns>An <see cref="ExecutionResult"/> representing the result of the execution.</returns>
        ExecutionResult Execute(string recipe, IDictionary state);

        /// <summary>
        /// Executes the recipe referenced by the <see cref="IAssetReference.AssetName"/>, using the 
        /// reference to restore any state that may have been saved.
        /// </summary>
        /// <param name="reference">An <see cref="IAssetReference"/> that identifies the recipe and any
        /// state associated with a previous execution.
        /// </param>
        /// <exception cref="InvalidOperationException">A recipe is already being executed.</exception>
        /// <exception cref="ArgumentException">The recipe does not exist in the package configuration.</exception>
        /// <returns>An <see cref="ExecutionResult"/> representing the result of the execution.</returns>
        ExecutionResult Execute(IAssetReference reference);
        
        /// <summary>
        /// Executes the recipe referenced by the <see cref="IAssetReference.AssetName"/>, using the 
        /// received dictionary as the initial state for the execution.
        /// </summary>
        /// <param name="reference">An <see cref="IAssetReference"/> that identifies the recipe and any
        /// state associated with a previous execution.
        /// </param>
        /// <param name="state">An <see cref="IDictionary"/> containing the initial state information 
        /// to use for the execution.</param>
        /// <exception cref="InvalidOperationException">A recipe is already being executed.</exception>
        /// <exception cref="ArgumentException">The recipe does not exist in the package configuration.</exception>
        /// <returns>An <see cref="ExecutionResult"/> representing the result of the execution.</returns>
        ExecutionResult Execute(IAssetReference reference, IDictionary state);
    }
}
