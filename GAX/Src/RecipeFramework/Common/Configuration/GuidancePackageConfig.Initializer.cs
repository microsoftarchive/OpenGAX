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
using System.Globalization;
using System.Xml.Serialization;
using System.Collections;

namespace Microsoft.Practices.RecipeFramework.Configuration
{
	/// <summary>
	/// Root of the configuration hierarchy for guidance packages.
	/// </summary>
	public partial class GuidancePackage
	{
		/// <summary>
		/// Name of the embedded resource that contains the schema for configuration validation.
		/// </summary>
		public const string SchemaResourceName = "Microsoft.Practices.RecipeFramework.Configuration.GuidancePackageConfig.xsd";

		[NonSerialized]
		Dictionary<string, Recipe> indexedRecipes;

		private Dictionary<string, Recipe> IndexedRecipes
		{
			get
			{
				if (indexedRecipes == null)
				{
					indexedRecipes = new Dictionary<string, Recipe>();
					if (recipesField != null)
					{
						foreach (Recipe recipe in recipesField)
						{
							indexedRecipes.Add(recipe.Name, recipe);
						}
					}
				}

				return indexedRecipes;
			}
		}

		/// <summary>
		/// Accesses a recipe configuration element by name.
		/// </summary>
		/// <param name="recipeName">Name of the recipe to retrieve configuration for.</param>
		/// <exception cref="KeyNotFoundException">The name of the recipe does not exist in the configuration file.</exception>
		public Recipe this[string recipeName]
		{
			get
			{
				if (!IndexedRecipes.ContainsKey(recipeName))
				{
					throw new ArgumentException(String.Format(
						CultureInfo.CurrentCulture,
						Properties.Resources.Recipe_Undefined,
						recipeName, this.Caption));
				}
				return IndexedRecipes[recipeName];
			}
		}

		/// <summary>
		/// Whether the package contains the given recipe.
		/// </summary>
		public bool Contains(string recipeName)
		{
			return IndexedRecipes.ContainsKey(recipeName);
		}
	}

	/// <remarks/>
	public partial class Recipe
	{
		[NonSerialized]
		Dictionary<string, Argument> indexedArguments;

		/// <summary>
		/// Gets the arguments indexed by name.
		/// </summary>
		[XmlIgnore]
		public Dictionary<string, Argument> ArgumentsByName
		{
			get
			{
				if (indexedArguments == null)
				{
					indexedArguments = new Dictionary<string, Argument>();
					if (argumentsField != null)
					{
						foreach (Argument arg in argumentsField)
						{
							indexedArguments.Add(arg.Name, arg);
						}
					}
				}

				return indexedArguments;
			}
		}

		[NonSerialized]
		Hashtable indexedAliases;

		/// <summary>
		/// Gets the type aliases indexed by name.
		/// </summary>
		[XmlIgnore]
		public IDictionary TypeAliasesByName
		{
			get
			{
				if (indexedAliases == null)
				{
					indexedAliases = new Hashtable();
					if (typesField != null)
					{
						foreach (TypeAlias alias in typesField)
						{
							indexedAliases.Add(alias.Name, alias.Type);
						}
					}
				}

				return indexedAliases;
			}
		}
	}

	/// <remarks/>
	public partial class RecipeActions
	{
		[NonSerialized]
		Dictionary<string, Action> indexedActions;

		/// <remarks/>
		public RecipeActions()
		{
			actionField = new Action[0];
		}

		/// <summary>
		/// Retrieves all the actions indexed by their name.
		/// </summary>
		public Dictionary<string, Action> GetIndexedActions()
		{
			if (indexedActions == null)
			{
				indexedActions = new Dictionary<string, Action>();
				if (actionField != null)
				{
					foreach (Action action in actionField)
					{
						indexedActions.Add(action.Name, action);
					}
				}
			}

			return indexedActions;
		}

		/// <summary>
		/// Retrieves the configuration for the action with the given name.
		/// </summary>
		public Action this[string actionName]
		{
			get { return GetIndexedActions()[actionName]; }
		}
	}

	/// <summary>
	/// Defines an argument used by a recipe.
	/// </summary>
	public partial class Argument
	{
		/// <summary>
		/// Initializes the argument with a name and a type.
		/// </summary>
		public Argument(string name, Type type)
		{
			this.nameField = name;
			this.typeField = type.AssemblyQualifiedName;
		}

		/// <summary>
		/// Initializes the argument with a name and a type.
		/// </summary>
		public Argument(string name, string type)
		{
			this.nameField = name;
			this.typeField = type;
		}
	}

	/// <summary>
	/// A component that provides a value to an argument automatically from the environment.
	/// </summary>
	public partial class ValueProvider
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ValueProvider()
		{
		}

		/// <summary>
		/// Initializes the value provider with the given type.
		/// </summary>
		public ValueProvider(Type type)
		{
			this.typeField = type.AssemblyQualifiedName;
		}

		/// <summary>
		/// Initializes the value provider with the given type.
		/// </summary>
		public ValueProvider(string type)
		{
			this.typeField = type;
		}
	}
}
