#region Using directives

using System;
using System.Collections;
using System.Diagnostics;

#endregion

namespace Microsoft.Practices.RecipeFramework
{
	public class HelloWorldAction : IAction
	{
		public bool Executed;
		public bool Undone;

		#region IAction Members

		void IAction.Execute()
		{
			Console.WriteLine("Hello World");
			Executed = true;
		}

		void IAction.Undo()
		{
			Undone = true;
		}

		#endregion
	}
}
