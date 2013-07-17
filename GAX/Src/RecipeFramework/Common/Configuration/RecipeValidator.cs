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
using System.Text;

#endregion

namespace Microsoft.Practices.RecipeFramework.Configuration
{
	/// <summary>
	/// Validates the configuration of a recipe.
	/// </summary>
	public sealed class RecipeValidator
	{
		private RecipeValidator() {}

		/// <summary>
		/// Validates the recipe configuration.
		/// </summary>
		/// <param name="recipe"></param>
		public static void Validate(Recipe recipe)
		{
			CheckArgumentClosure(recipe);
            if (!recipe.Bound && !recipe.Recurrent)
            {
                throw new System.Configuration.ConfigurationErrorsException(
                    Properties.Resources.RecipeValidator_InconsistentValues);
            }
		}

		private static void CheckArgumentClosure(Recipe recipe)
		{
			if (recipe.Arguments == null || recipe.Arguments.Length == 0)
			{
				return;
			}

			Hashtable graph = new Hashtable(recipe.Arguments.Length);
			StringBuilder errors = new StringBuilder();

			#region Build the graph

			foreach (Argument argument in recipe.Arguments)
			{
				if (argument.ValueProvider != null && argument.ValueProvider.MonitorArgument != null)
				{
					ArrayList dependencies = new ArrayList(argument.ValueProvider.MonitorArgument.Length);
					foreach (MonitorArgument monitored in argument.ValueProvider.MonitorArgument)
					{
						// Throw if the value provider is monitoring the same argument it's attached to.
						if (monitored.Name == argument.Name)
						{
							throw new System.Configuration.ConfigurationErrorsException(String.Format(
                                System.Globalization.CultureInfo.CurrentCulture,
								Properties.Resources.RecipeValidator_ArgumentCantMonitorItself,
								argument.ValueProvider.Type,
								argument.Name));
						}
						dependencies.Add(monitored.Name);
					}
					// Add dependencies to list.
					graph.Add(argument.Name, dependencies);
				}
				else
				{
					// Add dependencies to list.
					graph.Add(argument.Name, new ArrayList());
				}
			}

			#endregion Build the graph

			#region Walk the graph

			foreach (DictionaryEntry entry in graph)
			{
				foreach (string dependency in ((ArrayList)entry.Value))
				{
					StringBuilder stack = new StringBuilder();
					stack.Append(entry.Key);
					bool wrong = WalkGraph((string)entry.Key, dependency, graph, stack);
					if (wrong)
					{
						errors.Append(stack.ToString()).Append(Environment.NewLine);
					}
				}
			}

			#endregion Walk the graph

			string message = errors.ToString();
			if (message.Length > 0)
			{
				throw new System.Configuration.ConfigurationErrorsException(String.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
					Properties.Resources.RecipeValidator_CircularDependency,
					Environment.NewLine + message));
			}
		}

		private static bool WalkGraph(string start, string current, IDictionary graph, StringBuilder stack)
		{
			ArrayList dependencies = (ArrayList)graph[current];
			if (dependencies == null)
			{
				// Clear the stack. We haven't found anything wrong.
				stack.Remove(0, stack.Length);
				return false;
			}

			// Otherwise, append current element, and start recursion.
			stack.Append("->").Append(current);
			foreach (string dependency in dependencies)
			{
				if (start.IndexOf(dependency) != -1)
				{
					stack.Append("->").Append(dependency);
					// We found a circular dependency.
					return true;
				}

				StringBuilder innerstack = new StringBuilder();
				// The < character is guaranteed not to be on the argument name as it's invalid in an XML attribute.
				bool wrong = WalkGraph(start + "<" + dependency, dependency, graph, innerstack);
				if (wrong)
				{
					stack.Append(innerstack.ToString());
					return true;
				}
			}

			return false;
		}
	}
}
