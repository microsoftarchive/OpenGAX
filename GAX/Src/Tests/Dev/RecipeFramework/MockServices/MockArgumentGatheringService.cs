#region Using directives

using System;
using System.Text;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.Common;
using System.Xml;

#endregion

namespace Microsoft.Practices.RecipeFramework.MockServices
{
	public class MockArgumentGatheringService : IValueGatheringService
    {
        #region IValueInfoService Members

        public ExecutionResult Execute(XmlElement serviceData, bool allowSuspend)
		{
			return ExecutionResult.Finish;
		}

        public string ComponentName
        {
            get { return string.Empty; }
        }

		#endregion
	}
}
