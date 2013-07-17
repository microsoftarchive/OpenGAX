#region Using directives

using System;
using System.Text;

#endregion

namespace Microsoft.Practices.RecipeFramework.MockServices
{
	public class MockTypeResolutionService : System.ComponentModel.Design.ITypeResolutionService
	{
		#region ITypeResolutionService Members

		System.Reflection.Assembly System.ComponentModel.Design.ITypeResolutionService.GetAssembly(System.Reflection.AssemblyName name, bool throwOnError)
		{
			throw new NotImplementedException();
		}

		System.Reflection.Assembly System.ComponentModel.Design.ITypeResolutionService.GetAssembly(System.Reflection.AssemblyName name)
		{
			throw new NotImplementedException();
		}

		string System.ComponentModel.Design.ITypeResolutionService.GetPathOfAssembly(System.Reflection.AssemblyName name)
		{
			throw new NotImplementedException();
		}

		Type System.ComponentModel.Design.ITypeResolutionService.GetType(string name, bool throwOnError, bool ignoreCase)
		{
			throw new NotImplementedException();
		}

		Type System.ComponentModel.Design.ITypeResolutionService.GetType(string name, bool throwOnError)
		{
			throw new NotImplementedException();
		}

		Type System.ComponentModel.Design.ITypeResolutionService.GetType(string name)
		{
			throw new NotImplementedException();
		}

		void System.ComponentModel.Design.ITypeResolutionService.ReferenceAssembly(System.Reflection.AssemblyName name)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
