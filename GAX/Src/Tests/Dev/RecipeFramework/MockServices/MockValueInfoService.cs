using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Common.Services;

namespace Microsoft.Practices.RecipeFramework.MockServices
{
	public class MockValueInfoService : IValueInfoService
	{
		public Dictionary<string, ValueInfo> Infos = new Dictionary<string, ValueInfo>();

		#region IValueInfoService Members

		public ValueInfo GetInfo(string valueName)
		{
			return Infos[valueName];
		}

		public string ComponentName
		{
			get { return string.Empty ; }
		}

		#endregion
	}
}
