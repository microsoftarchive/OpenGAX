using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Common.Services;
using System.Collections;

namespace Microsoft.Practices.RecipeFramework.Library.TextTemplate.Tests.Mocks
{
    /// <summary>
    /// Simulates ValueInfoService used by Microsoft.Practices.RecipeFramework.Library.TextTemplateAction.
    /// </summary>
    class MockValueInfoService : IValueInfoService
    {
        IDictionary<string, ValueInfo> valueInfo;

        public MockValueInfoService()
        {
            valueInfo = new Dictionary<string, ValueInfo>(0);
            valueInfo.Add("NameSpace", new ValueInfo("NameSpace", true, typeof(string), null));
            valueInfo.Add("ClassName", new ValueInfo("ClassName", true, typeof(string), null));
            valueInfo.Add("ColorNames", new ValueInfo("ColorNames", true, typeof(List<string>), null));
            valueInfo.Add("MockColors", new ValueInfo("MockColors", true, typeof(Microsoft.Practices.RecipeFramework.Library.TextTemplate.Tests.MockColors), null));
            valueInfo.Add("Amounts", new ValueInfo("Amounts", true, typeof(List<decimal>), null));
            valueInfo.Add("Client", new ValueInfo("Client", true, typeof(string), null));
            valueInfo.Add("Amount", new ValueInfo("Amount", true, typeof(decimal), null));
        }

        #region IValueInfoService Members

        public string ComponentName
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ValueInfo GetInfo(string valueName)
        {
			if (valueInfo.ContainsKey(valueName) == false)
			{
				throw new ArgumentException("valueName");
			}

            return valueInfo[valueName];
        }

        #endregion
}
}
