using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework.Services;

namespace Microsoft.Practices.RecipeFramework.Library.TextTemplate.Tests.Mocks
{
	class MockConfigurationService : IConfigurationService
	{
		string basePath;

		public MockConfigurationService(string basePath)
		{
			this.basePath = basePath;
		}

		#region IConfigurationService Members

		public Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage CurrentPackage
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public Microsoft.Practices.RecipeFramework.Configuration.Recipe CurrentRecipe
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public object CurrentGatheringServiceData
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public string BasePath
		{
			get { return basePath; }
		}

		#endregion
	}
}
