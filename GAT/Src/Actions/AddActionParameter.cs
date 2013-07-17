//===================================================================================
// Microsoft patterns & practices
// Guidance Automation Toolkit
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
using System.Text;
using Microsoft.Practices.ComponentModel;
using Microsoft.Win32;
using EnvDTE;
using System.IO;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.Common;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.RecipeFramework.Library;

#endregion

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.Actions
{
	/// <summary>
	/// Action that adds a new parameter to an existing action class
	/// </summary>
    [ServiceDependency(typeof(DTE))]
	public class AddActionParameter : Action
    {

		#region Input Properties

		/// <summary>
		/// Specifies the parameter name of the new Parameter
		/// </summary>
		[Input(Required=true)]
		public string ParameterName
		{
			get { return parameterName; }
			set { parameterName = value; }
		} string parameterName;

		/// <summary>
		/// Specifies the parameter type of the new Parameter
		/// </summary>
		[Input(Required=true)]
		public string ParameterType
		{
			get { return parameterType; }
			set { parameterType = value; }
		} string parameterType;

		/// <summary>
		/// Specifies if the parameter is going to be a Output otherwise 
		/// it will be Input
		/// </summary>
		[Input(Required=true)]
		public bool ParameterIsOutput
		{
			get { return parameterIsOutput; }
			set { parameterIsOutput = value; }
		} bool parameterIsOutput;

		#endregion

		/// <summary>
		/// Selects the introduction page html to be at the top of the windows
		/// </summary>
        public override void Execute()
        {
            DTE vs = GetService<DTE>(true);
			ProjectItem actionClassFile = (ProjectItem)DteHelper.GetTarget(vs);
			if ((actionClassFile == null) || (actionClassFile.FileCodeModel == null) 
				|| (actionClassFile.FileCodeModel.CodeElements == null)
				|| (actionClassFile.FileCodeModel.CodeElements.Count == 0))
			{
				return;
			}

			string fieldName = "";
			if ((ParameterName.Length > 1) && (ParameterName[0] >= 'A') && (ParameterName[0] <= 'Z'))
			{
				fieldName = char.ToLower(parameterName[0],CultureInfo.CurrentCulture) +
					parameterName.Substring(1);
			}
			else
			{
				fieldName = "_" + parameterName;
			}

			string attribute;
			if (ParameterIsOutput)
			{
				attribute = "Output";
			}
			else
			{
				attribute = "Input";
			}
			bool addedProperty = false;
			foreach (CodeElement element in actionClassFile.FileCodeModel.CodeElements)
			{
				if (element.Kind == vsCMElement.vsCMElementNamespace)
				{
					foreach (CodeElement elementNamespace in ((CodeNamespace)element).Members)
					{
						if (elementNamespace.Kind == vsCMElement.vsCMElementClass)
						{
							#region Look where to insert the property
							object whereToInsert = null;
							CodeClass classAction = (CodeClass)elementNamespace;

							CodeProperty foundProperty = null;
							foreach (CodeElement classElement in classAction.Members)
							{
								if (foundProperty != null)
								{
									if (classElement.Kind == vsCMElement.vsCMElementVariable)
									{
										// Then insert after this declaration of variable that was after the property
										whereToInsert = classElement;
									}
								}
								if (classElement.Kind == vsCMElement.vsCMElementProperty)
								{
									foundProperty = (CodeProperty)classElement;
									bool hasAttribute = false;
									for (int i = 1; i <= foundProperty.Attributes.Count; i++)
									{
										if (foundProperty.Attributes.Item(i).Name == attribute)
										{
											hasAttribute = true;
											break;
										}
									}
									if (!hasAttribute)
									{
										foundProperty = null;
									}
								}
							}
							if (foundProperty != null)
							{
								if (whereToInsert == null)
								{
									whereToInsert = foundProperty;
								}
							}
							else if (whereToInsert == null)
							{
								whereToInsert = 0;
							}

							#endregion

							CodeProperty prop = classAction.AddProperty(ParameterName, ParameterName, ParameterType, whereToInsert, vsCMAccess.vsCMAccessPublic, actionClassFile.Name);

							TextPoint getTextTP = prop.Getter.GetStartPoint(vsCMPart.vsCMPartBody);
							EditPoint getText = getTextTP.CreateEditPoint();
							getText.ReplaceText(prop.Getter.GetEndPoint(vsCMPart.vsCMPartBody),
								string.Format(CultureInfo.InvariantCulture, "return {0};", fieldName), (int)vsEPReplaceTextOptions.vsEPReplaceTextNormalizeNewlines);
							getText.SmartFormat(getTextTP);

							TextPoint setTextTP = prop.Setter.GetStartPoint(vsCMPart.vsCMPartBody);
							EditPoint setText = setTextTP.CreateEditPoint();
							setText.ReplaceText(0, string.Format(CultureInfo.InvariantCulture, "{0}=value;", fieldName), 0);
							setText.SmartFormat(setTextTP);

							if (ParameterIsOutput)
							{
								prop.AddAttribute(attribute, "", 0);
							}
							else
							{
								prop.AddAttribute(attribute, "true", 0);
							}
							classAction.AddVariable(fieldName, ParameterType, prop, vsCMAccess.vsCMAccessPrivate, actionClassFile.Name);

							// Stop adding property, just the first class found
							addedProperty = true;
							break;
						}
					}
					if (addedProperty)
					{
						break;
					}
				}
			}
        }

		/// <summary>
		/// Undo the set of the window not supported
		/// </summary>
        public override void Undo()
        {
        }
    }
}

