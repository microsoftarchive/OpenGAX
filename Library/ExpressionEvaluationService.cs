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
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Practices.RecipeFramework.Library
{
	/// <summary>
	/// Allows evaluation of an expression in a given context.
	/// </summary>
    /// <remarks>
    /// See <see cref="ExpressionEvaluationService.Evaluate"/> for information 
    /// on the format of the expressions.
    /// </remarks>
	[System.ComponentModel.DesignerCategory("Code")]
	public class ExpressionEvaluationService
	{
		#region Fields & Ctor

		#region Expression split

		/// <summary>
		/// Provides an expression to match references in a string value.
		/// </summary>
		static readonly Regex ReferenceExpression = new Regex(@"
				# Matches invalid empty brackets #
				(?<empty>\$\(\))|
				# Matches a valid argument reference with potencial method calls and indexer accesses #
				(?<reference>\$\(([^\(]+([\(\[][^\)\]]*[\)\]])?)+\))|
				# Matches opened brackes that are not properly closed #
				(?<opened>\$\([^\)\$\(]*(?!\)))",
			RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
		const string EmptyGroup = "empty";
		const string RefereceGroup = "referece";
		const string OpenedGroup = "opened";

		#endregion Expression split

		#region Expression evaluation

		/// <summary>
		/// Provides an expression to match references in a string value.
		/// </summary>
		static readonly Regex ExpressionEvaluation = new Regex(@"
			# Matches a part with an indexer/method call # 
			# The part is optional as it can be a direct indexer access #
			(?<part>[^.\[\(]+?)?(?:[\[\(])(?<parameters>.*?)(?:[\]\)]) |
			# Matches a part without indexer/method call #
			(?<part>[^.\[\(]+)",
			RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
		const string GroupPart = "part";
		const string GroupParameters = "parameters";

		#endregion Expression evaluation

		#endregion Fields & Ctor
	
		/// <summary>
		/// Evaluates the expression given the context values.
		/// </summary>
		/// <returns>
		/// <see langword="null"/> if <paramref name="expression"/> is <see langword="null"/> or 
		/// an empty string. Otherwise, the string with argument references replaced by their actual values.
		/// <para>
		/// If the expression contains a single context reference, the returned value will be the 
		/// typed value resulting from the evaluation, instead of its string value. Mixture of 
        /// references and static text will always result in a string value being returned.
		/// </para>
		/// </returns>
		/// <exception cref="ArgumentException">Expression is not well formed.</exception>
        /// <remarks>
        /// The expression can contain mixed .NET expressions with static text in the form:
        /// <code>
        /// $([.NET EXPRESSION])SomeStaticString
        /// </code>
        /// <para>Note that the format of the argument reference is the same used for 
        /// VS build event macros and MSBuild files.
        /// </para>
        /// Any number of mixed .NET expressions and static text can be used. The .NET expression segment 
        /// must be a valid .NET dotted notation for accessing properties and 
        /// calling methods, enclosed in $(...). For example:
        /// <code>
        /// "$(BusinessActionProject.FullName)"
        /// "$(BusinessActionItem.get_Files(0))"
        /// "$(Configuration.Types[0].FullName)"
        /// </code>
        /// See the documentation for <c>System.Web.UI.DataBinder.Eval</c> as the format of 
        /// the .NET expression enclosed in the $() is the same. This is a more forgiving syntax than full C#, 
        /// and also allows VB-like indexer accesses, for example. 
        /// <para>
        /// The only exception is that the first segment of the expression (i.e. BusinessActionProject above) 
        /// is the key of an argument in <paramref name="context"/>, which is used as the context for 
        /// evaluating the rest of the expression (in terms of DataBinder.Eval, it's the container). This expression 
        /// doesn't have the limitation of the <c>System.Web.UI.DataBinder.Eval</c> with regards 
        /// to fields accesses and offers the enhanced ability to call methods too, with simple parameters. 
        /// </para>
        /// </remarks>
        public object Evaluate(string expression, IDictionary context)
		{
			if (expression == null || expression.Length == 0)
			{
				return null;
			}
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			// Resolve context references for the expression.
			Match referencematch = ReferenceExpression.Match(expression);
			// If we don't match, there was nothing to resolve. Return the input expression.
			if (!referencematch.Success)
			{
				return expression;
			}

			// Special case: if there is only one reference and no static text, the 
			// returned value will be the typed result of the reference evaluation, without 
			// converting it to string.
			if (referencematch.Length == expression.Length)
			{
				return EvaluateReference(expression, context);
			}

			StringBuilder sb = new StringBuilder();
			string original = expression;
			int start = 0;

			for (; referencematch.Success; referencematch = referencematch.NextMatch())
			{
				if (referencematch.Groups[EmptyGroup].Success)
				{
					throw new ArgumentException(String.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
						Properties.Resources.ExpressionEvaluationService_EmptyReference, expression));
				}
				if (referencematch.Groups[OpenedGroup].Success)
				{
					throw new ArgumentException(String.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
						Properties.Resources.ExpressionEvaluationService_OpenReference, referencematch.Value, expression));
				}

				// Append the unmatched text before the current match.
				sb.Append(original, start, referencematch.Index - start);

				// Append the replaced parameter, automatically converted to string.

				// Propagate nulls.
				object refvalue = EvaluateReference(referencematch.Value, context);
				if (refvalue == null)
				{
					return null;
				}
				sb.Append(refvalue);

				// Move the start position to the end of the last matched string.
				start = referencematch.Index + referencematch.Length;
			}

			// Append any remaining text.
			sb.Append(original, start, original.Length - start);

			return sb.ToString();
		}

		#region Private Members

		/// <summary>
		/// Evaluates the expression and returns a value.
		/// </summary>
		/// <param name="reference">The expression to evaluate.</param>
		/// <param name="context">Context to use for the evaluation operation.</param>
		/// <exception cref="ArgumentException">Expression is not well formed.</exception>
		/// <remarks>
		/// The expression must be a valid .NET dotted notation for accessing properties and 
		/// calling methods, enclosed in $(...). For example:
		/// <code>
		/// "$(BusinessActionProject.FullName)"
		/// "$(BusinessActionItem.get_Files(0))"
		/// "$(Configuration.Types[0].FullName)"
		/// </code>
        /// See the documentation for <c>System.Web.UI.DataBinder.Eval</c> as the expression format is 
		/// the same. The exception being that the first segment of the expression (i.e. BusinessActionProject above) 
		/// is the key of an argument in <paramref name="context"/>, which is used as the context for 
		/// evaluating the rest of the expression (in terms of DataBinder.Eval, it's the container). This expression 
        /// doesn't have the limitation of the <c>System.Web.UI.DataBinder.Eval</c> with regards to fields 
		/// accesses. 
		/// </remarks>
		object EvaluateReference(string reference, IDictionary context)
		{
			// Remove wrapping $( and ).
			string expression = reference.Substring(2, reference.Length - 3); 
			// The separator determines the split of each step.
			char[] separators = ".[(".ToCharArray();
			int firstsegment = expression.IndexOfAny(separators);

			// If there are no properties/fields to evaluate, return the context value directly.
			if (firstsegment == -1)
			{
				return context[expression];
			}

			// Otherwise, determine the first segment, which is the starting point 
			// for the evaluation.
			string key = expression.Substring(0, firstsegment);
			object startobject = context[key];

			if (startobject == null)
			{
                return null;
			}

			// Skip first segment which is the context.
			string toevaluate = expression.Substring(key.Length);

			return new BinderLocator(startobject, toevaluate).GetValue();
		}

		#endregion Private Members

		#region Internal Classes

		private class BindingFactory
		{
			private BindingFactory() { }

			public static Binding CreateBinding(object target, Group member, Group parameters)
			{
				if (target == null)
				{
                    throw new ArgumentNullException("target");
				}

				// If there are no parameters, this either a simple property access or a field access.
				if (!parameters.Success)
				{
					return new MemberBinding(target, member.Value);
				}
				// If there's no member and only parameters, then it's an indexer access on the parent itself.
				if (!member.Success && parameters.Success)
				{
					return new IndexerBinding(target, parameters.Value);
				}

				MethodBinding binding = new MethodBinding(target, member.Value, parameters.Value);
				if (binding.IsValid)
				{
					// If we have a method, return the binding.
					return binding;
				}

				// Otherwise, it's an indexer access of a property value.
				return new IndexerBinding(new MemberBinding(target, member.Value).GetValue(), parameters.Value);
			}
		}

		#region Binding

		private class ParameterValue
		{
			public string Value;
			public ParameterType ParamType;
			public ParameterValue(string value, ParameterType type)
			{
				this.Value = value;
				this.ParamType = type;
			}
		}

		private enum ParameterType
		{
			String, 
			Enum, 
			Bool,
			Number
		}

		private abstract class Binding
		{
			// Match both single and double quotes.
			const string QuotedString = "(?<string>['\"][^'\"]*?['\"])";
			// Detect open quotes that don't have a balancing closing quote.
			const string UnclosedQuotes = "(?<opened>['\"][^'\"]*?\\z)";
			// Special case for enum values. Note that enums defined inside other 
			// classes can have multiple dots in the name before the value.
			const string EnumValue = "(?<enum>[^\\.,\\s]+\\.[^\\.,\\s]+[^,\\s]+)";
			// Special case for booleans.
			const string BoolValue = "(?<bool>true|false|True|False)";
			// Everything else will be treated as a number (may fail on parse).
			const string NumberValue = "(?<number>[^,\\s]+)";
			static readonly Regex ParametersExpression = new Regex(
				QuotedString + " | " + UnclosedQuotes + " | " +
				EnumValue + " | " + BoolValue + " | " + NumberValue, 
				RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
			const string GroupStringValue = "string";
			const string GroupUnclosedQuotes = "opened";
			const string GroupEnumValue = "enum";
			const string GroupBoolValue = "bool";
			const string GroupNumberValue = "number";

			public Binding(object target)
			{
				this.target = target;
			}

			/// <summary>
			/// The target object of the binding.
			/// </summary>
			public object Target
			{
				get { return target; }
			} object target;

			public abstract object GetValue();

			protected ParameterValue[] GetParameterValues(string values)
			{
				if (!ParametersExpression.IsMatch(values))
				{
					return new ParameterValue[0];
				}

				ArrayList parameters = new ArrayList();

				// Parse attribute values and determine their type.
				for (Match matchedparam = ParametersExpression.Match(values); 
					matchedparam.Success; matchedparam = matchedparam.NextMatch())
				{
					// Detect errors first.
					if (matchedparam.Groups[GroupUnclosedQuotes].Success)
					{
						throw new ArgumentException(String.Format(
                            System.Globalization.CultureInfo.CurrentCulture,
							Properties.Resources.ExpressionEvaluationService_InvalidBindingParametersQuotes,
							matchedparam.Groups[GroupUnclosedQuotes].Value));
					}

					if (matchedparam.Groups[GroupStringValue].Success)
					{
						string stringvalue = matchedparam.Groups[GroupStringValue].Value;
						// Remove quotes for value.
						parameters.Add(new ParameterValue(
							stringvalue.Substring(1, stringvalue.Length - 2), 
							ParameterType.String));
					}
					else if (matchedparam.Groups[GroupBoolValue].Success)
					{
						// Make sure first letter is uppercase, to match Boolean.TrueString and Boolean.FalseString, 
						// used for parsing.
						Char[] letters = matchedparam.Groups[GroupBoolValue].Value.ToCharArray();
                        letters[0] = Char.ToUpper(letters[0], System.Globalization.CultureInfo.CurrentCulture);
						parameters.Add(new ParameterValue(
							new string(letters),
							ParameterType.Bool));
					}
					else if (matchedparam.Groups[GroupEnumValue].Success)
					{
						parameters.Add(new ParameterValue(
							matchedparam.Groups[GroupEnumValue].Value,
							ParameterType.Enum));
					}
					else if (matchedparam.Groups[GroupNumberValue].Success)
					{
						parameters.Add(new ParameterValue(
							matchedparam.Groups[GroupNumberValue].Value,
							ParameterType.Number));
					}
					else
					{
						throw new ArgumentException(String.Format(
                            System.Globalization.CultureInfo.CurrentCulture,
							Properties.Resources.ExpressionEvaluationService_InvalidBindingParameters,
							values));
					}
				}

				return (ParameterValue[])parameters.ToArray(typeof(ParameterValue));
			}

			protected object[] ConvertParameters(ParameterInfo[] parameters, ParameterValue[] values)
			{
				if (parameters.Length != values.Length)
				{
					throw new ArgumentException(Properties.Resources.ExpressionEvaluationService_InvalidBindingParametersLength);
				}

				object[] result = new object[parameters.Length];
				for (int i = 0; i < parameters.Length; i++)
				{
					// Special case for enums.
					if (values[i].ParamType == ParameterType.Enum)
					{
						// Ensure both enums are the "same".
						if (!parameters[i].ParameterType.IsEnum ||
							parameters[i].ParameterType.Name !=
							values[i].Value.Substring(0, values[i].Value.IndexOf('.')))
						{
							throw new ArgumentException(String.Format(
                                System.Globalization.CultureInfo.CurrentCulture,
								Properties.Resources.ExpressionEvaluationService_InvalidBindingParameterValue,
								values[i].Value, parameters[i].ParameterType.FullName));
						}

						result[i] = Enum.Parse(parameters[i].ParameterType,
							values[i].Value.Substring(values[i].Value.LastIndexOf('.') + 1));
					}
					else
					{
						// Let String IConvertible interface implementation do the rest.
						result[i] = ((IConvertible)values[i].Value).ToType(
							parameters[i].ParameterType, System.Globalization.CultureInfo.CurrentCulture);
					}
				}

				return result;
			}
		}

		#endregion Binding

		#region MemberBinding

		private class MemberBinding : Binding
		{
			PropertyInfo property;
			FieldInfo field;
            string name;

			public MemberBinding(object target, string name)
				: base(target) 
			{
                if (target.GetType().Name == "__ComObject")
                {
                    // We have to go the IDispatch route...
                    this.name = name;
                }
                else
                {
                    property = target.GetType().GetProperty(name);
                    if (property == null)
                    {
                        field = target.GetType().GetField(name);
                    }
                    if (property == null && field == null)
                    {
                        throw new ArgumentException(String.Format(
                            System.Globalization.CultureInfo.CurrentCulture,
                            Properties.Resources.ExpressionEvaluationService_InvalidBindingNoMember,
                            name, target.GetType().FullName));
                    }
                }
			}

			public override object GetValue()
			{
                if (this.name != null)
                {
                    // It's a COM object. Invoke with late binding (IDispatch)
                    return base.Target.GetType().InvokeMember(this.name,
                        BindingFlags.GetProperty, null, base.Target, new object[0]);
                }
                else
                {
                    return (property != null) ?
                        property.GetValue(base.Target, new object[0]) :
                        field.GetValue(base.Target);
                }
			}
		}

		#endregion PropertyBinding

		#region IndexerBinding

		private class IndexerBinding : Binding
		{
			PropertyInfo indexer;
			object[] parameters;
			int index = -1;

			public IndexerBinding(object target, string values)
				: base(target) 
			{
                if (target == null) return;

				// Get the length of index arguments
				ParameterValue[] paramvalues = GetParameterValues(values);
				ParameterInfo[] paraminfos = null;

				// Look for an indexer that matches the arguments length. Can't check
				// types yet.
				PropertyInfo[] props = target.GetType().GetProperties();
				foreach (PropertyInfo pi in props)
				{
					paraminfos = pi.GetIndexParameters();
					// Could have better heuristics, such as checking the initial 
					// tentative types we discovered from GetParameterValues().
					if (paraminfos.Length == paramvalues.Length)
					{
						bool compatible = true;
						// Check that we have initially compatible parameters.
						for (int i = 0; i < paraminfos.Length; i++)
						{
							bool isenumcompat = (paramvalues[i].ParamType == ParameterType.Enum && 
								paraminfos[i].ParameterType.IsEnum);
							bool isboolcompat = (paramvalues[i].ParamType == ParameterType.Bool && 
								paraminfos[i].ParameterType == typeof(bool));
							bool isstringcompat = (paramvalues[i].ParamType == ParameterType.String && 
								paraminfos[i].ParameterType.IsAssignableFrom(typeof(string)));

							// Object typed parameters are compatible with everything.
							// Numbered indexers too, even though there may be an 
							// error at index parsing time.
							if (!(paraminfos[i].ParameterType == typeof(object)) &&
								!(paramvalues[i].ParamType == ParameterType.Number) && 
								!(isenumcompat || isboolcompat || isstringcompat))
							{
								compatible = false;
								continue;
							}
						}
						if (!compatible)
						{
							// Skip this property.
							continue;
						}
						indexer = pi;
						break;
					}
				}

				// Special case if there's only one Int32 param and the target implements
				// IList (i.e. Array among others)
				if (indexer == null && paramvalues.Length == 1 &&
					paramvalues[0].ParamType == ParameterType.Number && 
					target is IList)
				{
					try
					{
						index = Int32.Parse(paramvalues[0].Value,
                            System.Globalization.CultureInfo.CurrentCulture);
					}
					catch (FormatException)
					{
						throw new ArgumentException(String.Format(
                            System.Globalization.CultureInfo.CurrentCulture,
							Properties.Resources.ExpressionEvaluationService_InvalidListIndex,
							paramvalues[0].Value));
					}
					catch (OverflowException)
					{
						throw new ArgumentException(String.Format(
                            System.Globalization.CultureInfo.CurrentCulture,
							Properties.Resources.ExpressionEvaluationService_InvalidListIndex,
							paramvalues[0].Value));
					}
				}

				if (indexer == null && index == -1)
				{
					throw new ArgumentException(String.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
						Properties.Resources.ExpressionEvaluationService_InvalidBindingNoIndexedProperty,
						target.GetType().FullName));
				}

				if (indexer != null)
				{
					// Convert the values if there is an indexer.
					parameters = ConvertParameters(paraminfos, paramvalues);
				}
			}

			public override object GetValue()
			{
                if (base.Target == null) return null;

				// Either an indexer access or a direct access on a list.
				return indexer != null ?
					indexer.GetValue(base.Target, parameters) :
					((IList)base.Target)[index];
			}
		}

		#endregion IndexerBinding

		#region MethodBinding

		private class MethodBinding : Binding
		{
			MethodInfo method;
			object[] parameters;
            string methodName;

			public MethodBinding(object target, string methodName, string values)
				: base(target) 
			{
				ParameterValue[] paramvalues = GetParameterValues(values);

                // If we have a COM object, there's not much we can do to inspect it. 
                // Just assume a method may be there.
                if (target.GetType().Name == "__ComObject")
                {
                    this.methodName = methodName;
                    // Just pass the parameter values as-is.
                    parameters = new object[paramvalues.Length];
                    for (int i = 0; i < paramvalues.Length; i++)
                    {
                        // Perform minimum conversion with the information we have.
						switch (paramvalues[i].ParamType)
                        {
                            case ParameterType.Bool:
                                parameters[i] = Boolean.Parse(paramvalues[i].Value);
                                break;
                            case ParameterType.Number:
                                // Try to parse as integer, which is probably the most common 
                                // parameter type.
                                parameters[i] = Int32.Parse(paramvalues[i].Value);
                                break;
                            case ParameterType.String:
                                parameters[i] = paramvalues[i].Value;
                                break;
                            default:
                                throw new NotSupportedException(String.Format(
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    Properties.Resources.ExpressionEvaluationService_COMUnsupportedParam,
									paramvalues[i].ParamType.ToString()));
                        }
                    }
                    return;
                }

				// We need to probe for methods, then for property value indexer.
				MethodInfo[] methods = target.GetType().GetMethods(
					BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance);
				// See if we have a method with the same number of parameters.
				MethodInfo method = null;
				ParameterInfo[] methodparams = null;
				foreach (MethodInfo mi in methods)
				{
					// Skip property access methods, including indexer.
					if (mi.Name == methodName && !mi.Name.StartsWith("get_"))
					{
						methodparams = mi.GetParameters();
						if (methodparams.Length == paramvalues.Length)
						{
							// We found a match in argument count. Use the method.
							method = mi;
							break;
						}
					}
				}

				if (method == null)
				{
					valid = false;
					return;
				}

				this.method = method;
				this.parameters = ConvertParameters(methodparams, paramvalues);
			}

			public bool IsValid
			{
				get { return valid; }
			} bool valid = true;
	
			public override object GetValue()
			{
                if (methodName != null)
                {
                    // It's a COM object. Invoke with late binding (IDispatch)
                    return base.Target.GetType().InvokeMember(this.methodName,
                        BindingFlags.InvokeMethod, null, base.Target, parameters);
                }
                else
                {
                    return method.Invoke(base.Target, parameters);
                }
			}
		}

		#endregion MethodBinding

		#region BinderLocator

		private class BinderLocator
		{
			Binding currentBinding;

			public BinderLocator() { }

			public BinderLocator(object target, string expression)
			{
				Advance(target, expression);
			}

			public void Advance(object target, string expression)
			{
				if (!ExpressionEvaluation.IsMatch(expression))
				{
					throw new ArgumentException(String.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
						Properties.Resources.ExpressionEvaluationService_InvalidReference, expression));
				}

				// Don't process null targets.
				if (target == null)
				{
					return;
				}

				// If we find a null, we exit.
				for (Match segmentmatch = ExpressionEvaluation.Match(expression);
					segmentmatch.Success && target != null;
					segmentmatch = segmentmatch.NextMatch())
				{
					// Catch all reflection-generated exceptions and throw the inner one.
					try
					{
						currentBinding = BindingFactory.CreateBinding(target,
							segmentmatch.Groups[GroupPart],
							segmentmatch.Groups[GroupParameters]);
						target = currentBinding.GetValue();
					}
					catch (ArgumentException aex)
					{
						// Rethrow with a more meaningful message.
						throw new ArgumentException(String.Format(
                            System.Globalization.CultureInfo.CurrentCulture,
							Properties.Resources.ExpressionEvaluationService_CantEvaluate,
							expression, segmentmatch.Value, aex.Message), aex);
					}
					catch (TargetInvocationException tiex)
					{
						// Rethrow with a more meaningful message.
						throw new ArgumentException(String.Format(
                            System.Globalization.CultureInfo.CurrentCulture,
							Properties.Resources.ExpressionEvaluationService_CantEvaluate,
							expression, segmentmatch.Value, tiex.InnerException.Message), tiex);
					}
				}
			}

			/// <summary>
			/// Gets the value on the current expression object.
			/// </summary>
			public object GetValue()
			{
				try
				{
					return currentBinding.GetValue();
				}
				catch (TargetInvocationException tex)
				{
					throw tex.InnerException;
				}
			}
		}

		#endregion BinderLocator

		#endregion Internal Classes
	}
}
